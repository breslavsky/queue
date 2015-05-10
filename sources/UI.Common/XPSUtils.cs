using Junte.UI.WinForms;
using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace Queue.UI.Common
{
    public class XPSUtils
    {
        public static string WriteXaml(string template, object dataContext)
        {
            return InternalCreateFileFromXaml(template, dataContext).Path;
        }

        public static void PrintXaml(string template, object dataContext, string printer = null)
        {
            var result = InternalCreateFileFromXaml(template, dataContext);

            var printQueue = GetPrintQueue(printer);
            printQueue.AddJob(result.Path, result.Path, false, result.Ticket);
        }

        private static PrintQueue GetPrintQueue(string printer)
        {
            if (printer == null)
            {
                return LocalPrintServer.GetDefaultPrintQueue();
            }

            try
            {
                return new PrintServer().GetPrintQueue(printer);
            }
            catch
            {
                UIHelper.Warning("Ошибка получения принтера, будет использован принтер по умолчанию");
                return LocalPrintServer.GetDefaultPrintQueue();
            }
        }

        private static CreateXpsFileResult InternalCreateFileFromXaml(string template, object dataContext)
        {
            string xpsFile = Path.GetTempFileName() + ".xps";

            using (var stream = new XmlTextReader(new StringReader(template)))
            using (var container = Package.Open(xpsFile, FileMode.Create))
            using (var document = new XpsDocument(container, CompressionOption.SuperFast))
            {
                var root = XamlReader.Load(stream) as FrameworkElement;

                root.DataContext = dataContext;
                root.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                root.UpdateLayout();

                var ticket = new PrintTicket()
                {
                    PageMediaSize = new PageMediaSize(root.DesiredSize.Width, root.DesiredSize.Height)
                };

                XpsDocument.CreateXpsDocumentWriter(document).Write(root, ticket);

                return new CreateXpsFileResult()
                {
                    Path = xpsFile,
                    Ticket = ticket
                };
            }
        }

        private struct CreateXpsFileResult
        {
            public string Path;
            public PrintTicket Ticket;
        }
    }
}