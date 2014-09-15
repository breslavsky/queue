using System;
using System.Collections.Generic;

namespace Queue.Services.Portal
{
    public class ContentType
    {
        private static string HTML = "text/html";
        private static string XML = "text/xml";
        private static string XAML = "text/xaml";
        private static string DLL = "application/dll";
        private static string XAP = "application/xap";
        private static string CSS = "text/css";
        private static string IMAGE_PNG = "image/png";
        private static string IMAGE_GIF = "image/gif";
        private static string IMAGE_ICO = "image/ico";
        private static string JAVA_SCRIPT = "application/x-javascript";

        private static Dictionary<string, string> types = new Dictionary<string, string>()
        {
            {"html", HTML},
            {"xml", XML},
            {"xaml", XAML},
            {"dll", DLL},
            {"xap", XAP},
            {"css", CSS},
            {"png", IMAGE_PNG},
            {"gif", IMAGE_GIF},
            {"ico", IMAGE_ICO},
            {"js", JAVA_SCRIPT}
        };

        public static string GetType(string extension)
        {
            extension = extension.ToLower().Trim('.');

            if (!types.ContainsKey(extension))
            {
                throw new Exception("Указанный тип файла не найден");
            }

            return types[extension];
        }
    }
}