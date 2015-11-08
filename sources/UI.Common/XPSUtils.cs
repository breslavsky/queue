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
        public static string WriteXaml(FrameworkElement template, object dataContext)
        {
            return InternalCreateFileFromXaml(template, dataContext).Path;
        }

        public static void PrintXaml(FrameworkElement template, object dataContext, string printer = null)
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

        private static CreateXpsFileResult InternalCreateFileFromXaml(FrameworkElement template, object dataContext)
        {
            string xpsFile = Path.GetTempFileName() + ".xps";

            using (var container = Package.Open(xpsFile, FileMode.Create))
            using (var document = new XpsDocument(container, CompressionOption.SuperFast))
            {
                var coupon = template;

                coupon.DataContext = dataContext;
                coupon.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                coupon.UpdateLayout();

                var ticket = new PrintTicket()
                {
                    PageMediaSize = new PageMediaSize(coupon.DesiredSize.Width, coupon.DesiredSize.Height)
                };

                XpsDocument.CreateXpsDocumentWriter(document).Write(coupon, ticket);

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