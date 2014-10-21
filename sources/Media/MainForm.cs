using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Services.Media;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Media
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private static Properties.Settings settings = Properties.Settings.Default;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;

        private Administrator currentUser;

        public MainForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
        }

        public bool IsLogout { get; private set; }

        private int port
        {
            get
            {
                return (int)portUpDown.Value;
            }
            set
            {
                portUpDown.Value = value;
            }
        }

        private string folder
        {
            get
            {
                return folderTextBox.Text;
            }
            set
            {
                folderTextBox.Text = value;
            }
        }

        [DllImport("shell32.dll")]
        internal static extern bool IsUserAnAdmin();

        private void MainForm_Load(object sender, EventArgs e)
        {
            UACButton.Enabled = !IsUserAnAdmin();

            port = settings.Port;
            folder = settings.Folder;
        }

        private async void startButton_Click(object sender, EventArgs eventArgs)
        {
            Uri uri = new Uri(string.Format("{0}://0.0.0.0:{1}/", Schemes.HTTP, port));

            if (IsUserAnAdmin())
            {
                try
                {
                    addListenAddress(uri);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            ServiceHost serviceHost = new MediaServiceHost(channelBuilder, currentUser, folder, typeof(MediaService));
            ServiceEndpoint serviceEndpoint = serviceHost.AddServiceEndpoint(typeof(IMediaService), Bindings.WebHttpBinding, uri.ToString());
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior());

            try
            {
                startButton.Enabled = false;

                await Task.Run(() =>
                {
                    serviceHost.Open();
                });
            }
            catch (AddressAccessDeniedException)
            {
                UIHelper.Error(string.Format("Доступ к адресу ({0}) запрещен. Необходимо повысить права программы что бы добавить разрение для текущего пользователя.", uri));
                return;
            }
            catch (Exception exception)
            {
                UIHelper.Error(exception);
            }
            finally
            {
                startButton.Enabled = true;
            }

            FormClosing += (e, s) =>
            {
                serviceHost.Close();
            };

            settings.Port = port;
            settings.Folder = folder;
            settings.Save();

            serverGroupBox.Enabled = false;
            UACButton.Enabled = false;
            openButton.Enabled = true;
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = folder;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folder = folderBrowserDialog.SelectedPath;
            }
        }

        private void addListenAddress(Uri uri)
        {
            string address = uri.ToString().Replace("0.0.0.0", "+");

            ProcessStartInfo processInfo = new ProcessStartInfo()
            {
                FileName = "netsh",
                Arguments = string.Format(@"http delete urlacl url={0}", address),
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var p = Process.Start(processInfo);
            p.WaitForExit();

            processInfo.Arguments = string.Format(@"http add urlacl url={0} user={1}\{2}", address, Environment.UserDomainName, Environment.UserName);

            p = Process.Start(processInfo);
            string output = new StreamReader(p.StandardOutput.BaseStream, Encoding.GetEncoding(866)).ReadToEnd();
            p.WaitForExit();
            if (output.IndexOf("Ошибка") != -1)
            {
                throw new Exception(output);
            }
        }

        private void UACButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo()
            {
                FileName = Application.ExecutablePath,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Normal
            };

            try
            {
                Process.Start(processInfo);
                Application.Exit();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            Process.Start(string.Format("http://localhost:{0}/", port));
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            Close();
        }
    }
}