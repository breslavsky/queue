using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Queue.Common
{
    public enum Language
    {
        ru_RU,
        en_US
    }

    public static partial class TranslationExtensions
    {
        private static Dictionary<Language, string> CulturesNames = new Dictionary<Language, string>()
        {
            {Language.ru_RU, "ru-RU"},
            {Language.en_US, "en-US"}
        };

        public static Language GetLanguage(this CultureInfo culture)
        {
            return CulturesNames.Single(n => n.Value == culture.Name).Key;
        }

        public static void SetCurrent(this Language language)
        {
            var culture = language.GetCulture();

            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public static CultureInfo GetCulture(this Language value)
        {
            return CultureInfo.CreateSpecificCulture(CulturesNames[value]);
        }
    }
}