using System;
using System.Globalization;
using System.Threading;
using Translation = Queue.Common.Translation;

namespace Queue.Common
{
    public enum Language
    {
        ru_RU,
        en_EN,
        zh_CN
    }

    public static partial class TranslationExtensions
    {
        public static Language GetLanguage(this CultureInfo culture)
        {
            if (culture.Equals(CultureInfo.CreateSpecificCulture("en-EN")))
            {
                return Language.en_EN;
            }

            if (culture.Equals(CultureInfo.CreateSpecificCulture("zh-CN")))
            {
                return Language.zh_CN;
            }

            return Language.ru_RU;
        }

        public static void SetCurrent(this Language language)
        {
            CultureInfo.DefaultThreadCurrentUICulture =
                Thread.CurrentThread.CurrentCulture = language.GetCulture();
        }

        public static CultureInfo GetCulture(this Language value)
        {
            switch (value)
            {
                case Language.en_EN:
                    return CultureInfo.CreateSpecificCulture("en-EN");

                case Language.zh_CN:
                    return CultureInfo.CreateSpecificCulture("zh-CN");

                default:
                    return CultureInfo.CreateSpecificCulture("ru-RU");
            }
        }
    }
}