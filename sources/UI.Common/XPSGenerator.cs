using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace Queue.UI.Common
{
    public class XPSGenerator
    {
        public static string FromXaml(string template, object dataContext, string destFile = null)
        {
            string xpsFile = destFile ?? Path.GetTempFileName() + ".xps";

            using (XmlReader stream = new XmlTextReader(new StringReader(template)))
            using (Package container = Package.Open(xpsFile, FileMode.Create))
            using (XpsDocument document = new XpsDocument(container, CompressionOption.SuperFast))
            {
                FrameworkElement root = XamlReader.Load(stream) as FrameworkElement;

                root.DataContext = dataContext;
                root.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                root.UpdateLayout();

                PrintTicket ticket = new PrintTicket()
                {
                    PageMediaSize = new PageMediaSize(root.DesiredSize.Width, root.DesiredSize.Height)
                };

                XpsDocument.CreateXpsDocumentWriter(document).Write(root, ticket);
            }

            return xpsFile;
        }
    }
}