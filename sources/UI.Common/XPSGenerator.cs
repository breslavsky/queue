using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Xps;
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
                FrameworkElement root = (FrameworkElement)XamlReader.Load(stream);

                root.DataContext = dataContext;

                root.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                root.Arrange(new Rect(0, 0, root.DesiredSize.Width, root.DesiredSize.Height));

                //FixedPage fixedPage = new FixedPage();
                //fixedPage.Children.Add(root);

                //PageContent pageConent = new PageContent();
                //((IAddChild)pageConent).AddChild(fixedPage);

                //FixedDocument fixedDocument = new FixedDocument();
                //fixedDocument.Pages.Add(pageConent);

                XpsDocumentWriter xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(document);
                xpsDocumentWriter.Write(root, new PrintTicket()
                    {
                        //PageMediaType = PageMediaType.Plain
                        //PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA6)
                        PageMediaSize = new PageMediaSize(root.ActualWidth, root.ActualHeight)
                    });
            }

            return xpsFile;
        }
    }
}