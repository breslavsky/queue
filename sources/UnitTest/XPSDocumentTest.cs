using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace UnitTest
{
    [TestClass]
    public class XPSDocumentTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            object doc;

            FileInfo fileInfo = new FileInfo("D:\\tmp\\test.xaml");

            using (FileStream file = fileInfo.OpenRead())
            {
                ParserContext context = new System.Windows.Markup.ParserContext();
                context.BaseUri = new Uri(fileInfo.FullName, UriKind.Absolute);
                doc = XamlReader.Load(file, context);
            }

            string xpsFile = "D:\\tmp\\test.xps";

            using (var container = Package.Open(xpsFile, FileMode.Create))
            {
                using (XpsDocument xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
                {
                    XpsSerializationManager rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);

                    rsm.SaveAsXaml(doc);
                }
            }

            LocalPrintServer localPrintServer = new LocalPrintServer();
            PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

            PrintSystemJobInfo xpsPrintJob = defaultPrintQueue.AddJob(xpsFile, xpsFile, false);

            //XpsDocument xpsd = new XpsDocument("D:\\tmp\\test.xps", FileAccess.ReadWrite);
            //System.Windows.Xps.XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            //xw.Write("D:\\tmp\\test.jpg");
        }
    }
}