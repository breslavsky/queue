using Queue.Common;

namespace Queue.UI.WPF.Models
{
    public class LanguageItem
    {
        public string Title { get; private set; }

        public Language Language { get; private set; }

        public LanguageItem(Language lang)
        {
            Language = lang;

            //Title = Queue.Common.Translation.Language_en_EN. title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}