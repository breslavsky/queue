using Junte.Data.NHibernate;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Server
{
    public partial class DamaskForm : Queue.UI.WinForms.RichForm
    {
        private ISession session;

        private string codesFile = @"C:\projects\queue\list.csv";

        public DamaskForm()
            : base()
        {
            InitializeComponent();

            session = SessionProvider.OpenSession();
        }

        public ISessionProvider SessionProvider
        {
            get { return ServiceLocator.Current.GetInstance<ISessionProvider>(); }
        }

        private void log(object message)
        {
            Invoke(new MethodInvoker(() =>
            {
                logTextBox.Text += string.Format("{0}{1}", message, Environment.NewLine);
            }));
        }

        private async void loadButton_Click(object sender, EventArgs e)
        {
            var lines = File.ReadAllLines(codesFile);

            var codes = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                var element = line.Split(';');
                if (element.Length > 1)
                {
                    codes[element[0].Trim()] = element[1].Trim();
                }
            }

            loadButton.Enabled = false;

            try
            {
                await Task.Run(() =>
                {
                    session.Clear();

                    var db = new OleDbConnection(connectionStringTextBox.Text);
                    db.Open();

                    var serviceGroup = new ServiceGroup()
                    {
                        Code = "DAMASK",
                        Name = "Дамаск",
                        IsActive = true
                    };
                    session.Save(serviceGroup);

                    var services = new Dictionary<int, Service>();

                    var command = new OleDbCommand(string.Format("SELECT [Surname], [Name] as [ServiceName], [TimeCall], [Ticket] FROM [Terminal] LEFT JOIN [Services] ON  [Services].[ServiceId] = [Terminal].[ServiceId] WHERE TimeCall > '{0}' ORDER BY TimeCall desc", DateTime.Today.AddDays(1).ToString("yyyy-MM-dd")), db);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string serviceName = reader["ServiceName"].ToString();
                        if (string.IsNullOrWhiteSpace(serviceName))
                        {
                            continue;
                        }

                        int index = serviceName.IndexOf(' ');
                        if (index == -1)
                        {
                            log(string.Format("Не выделен код услуги [{0}]", serviceName));
                            continue;
                        }

                        string code = serviceName.Substring(0, index);
                        if (!codes.ContainsKey(code))
                        {
                            log(string.Format("Код услуги не сопоставлен [{0}]", code));
                            continue;
                        }

                        var comparison = codes[code];

                        var service = session.CreateCriteria<Service>()
                            .Add(Restrictions.Eq("Code", comparison))
                            .SetMaxResults(1)
                            .UniqueResult<Service>();

                        if (service == null)
                        {
                            log(string.Format("Не найдена услуга с кодом сопоставления [{0}]", comparison));

                            continue;
                        }

                        string surname = reader["Surname"].ToString();

                        var client = new Client()
                        {
                            Surname = surname
                        };
                        session.Save(client);

                        var clientRequest = new ClientRequest();

                        string timeCall = reader["TimeCall"].ToString();
                        var requestDateTime = DateTime.Parse(timeCall);
                        if (requestDateTime < DateTime.Now || requestDateTime > DateTime.Now.AddMonths(6))
                        {
                            log(string.Format("Не верная дата вызова посетителя [{0}]", requestDateTime));

                            continue;
                        }

                        log(requestDateTime.ToShortDateString());

                        string ticket = reader["Ticket"].ToString();
                        string[] cnunks = ticket.Split('-');
                        if (cnunks.Length < 1)
                        {
                            log(string.Format("Не верный номер талона [{0}]", ticket));
                            continue;
                        }

                        int number;
                        try
                        {
                            var x = cnunks[1];
                            var m = x.Substring(0, 5);

                            number = Convert.ToInt32(m);
                        }
                        catch
                        {
                            log(string.Format("Номер талона не распознан [{0}]", ticket));
                            continue;
                        }

                        clientRequest.CreateDate = DateTime.Now;
                        clientRequest.Client = client;
                        clientRequest.Number = number;
                        clientRequest.Service = service;
                        clientRequest.RequestDate = requestDateTime.Date;
                        clientRequest.RequestTime = requestDateTime.TimeOfDay;
                        clientRequest.Type = ClientRequestType.Early;
                        session.Save(clientRequest);

                        log(string.Format("Добавлен запрос [{0}]", client));
                    }
                });

                importButton.Enabled = true;
            }
            catch (Exception exception)
            {
                UIHelper.Warning(exception.Message);
            }
            finally
            {
                loadButton.Enabled = true;
            }
        }

        private async void importButton_Click(object sender, EventArgs e)
        {
            importButton.Enabled = false;

            await Task.Run(() =>
            {
                session.Flush();
            });

            UIHelper.Information("Загрузка данных успешно произведена");

            importButton.Enabled = true;
        }

        private void selectCodesFileButton_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog()
            {
                InitialDirectory = "c:\\",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                codesFile = openFileDialog1.FileName;
            }
        }
    }
}