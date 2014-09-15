using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Services.Portal;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Portal
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private static Properties.Settings settings = Properties.Settings.Default;

        private DuplexChannelBuilder<IServerService> channelBuilder;

        private Administrator currentUser;

        public MainForm(DuplexChannelBuilder<IServerService> channelBuilder, Administrator currentUser)
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

        [DllImport("shell32.dll")]
        internal static extern bool IsUserAnAdmin();

        private static void addListenAddress(Uri uri)
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
                throw new ApplicationException(output);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UACButton.Enabled = !IsUserAnAdmin();

            port = settings.Port;
        }

        private async void startButton_Click(object sender, EventArgs eventArgs)
        {
            var uri = new Uri(string.Format("{0}://0.0.0.0:{1}/", Schemes.HTTP, port));

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

            var serviceHost1 = new PortalServiceHost(channelBuilder, currentUser, typeof(PortalService));
            ServiceEndpoint serviceEndpoint = serviceHost1.AddServiceEndpoint(typeof(IPortalService), Bindings.WebHttpBinding, uri.ToString());
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior());

            uri = new Uri(string.Format("{0}://0.0.0.0:{1}/client", Schemes.HTTP, port));
            var serviceHost2 = new PortalClientServiceHost(channelBuilder, currentUser, typeof(PortalClientService));
            serviceEndpoint = serviceHost2.AddServiceEndpoint(typeof(IPortalClientService), Bindings.WebHttpBinding, uri);
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior());

            uri = new Uri(string.Format("{0}://0.0.0.0:{1}/operator", Schemes.HTTP, port));
            var serviceHost3 = new PortalOperatorServiceHost(channelBuilder, currentUser, typeof(PortalOperatorService));
            serviceEndpoint = serviceHost3.AddServiceEndpoint(typeof(IPortalOperatorService), Bindings.WebHttpBinding, uri);
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior());

            try
            {
                startButton.Enabled = false;

                await Task.Run(() =>
                {
                    serviceHost1.Open();
                    serviceHost2.Open();
                    serviceHost3.Open();
                });
            }
            catch (Exception exception)
            {
                UIHelper.Error(exception);
                return;
            }
            finally
            {
                startButton.Enabled = true;
            }

            FormClosing += (e, s) =>
            {
                serviceHost1.Close();
                serviceHost2.Close();
                serviceHost3.Close();
            };

            settings.Port = port;
            settings.Save();

            serverGroupBox.Enabled = false;
            UACButton.Enabled = false;
            openButton.Enabled = true;
        }

        private void UACButton_Click(object sender, EventArgs e)
        {
            var processInfo = new ProcessStartInfo()
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