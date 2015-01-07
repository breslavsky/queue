using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace Queue.Common
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

                FixedPage fixedPage = new FixedPage();
                fixedPage.Children.Add(root);

                PageContent pageConent = new PageContent();
                ((IAddChild)pageConent).AddChild(fixedPage);

                FixedDocument fixedDocument = new FixedDocument();
                fixedDocument.Pages.Add(pageConent);

                XpsDocumentWriter xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(document);
                xpsDocumentWriter.Write(fixedDocument);
            }

            return xpsFile;
        }
    }
}