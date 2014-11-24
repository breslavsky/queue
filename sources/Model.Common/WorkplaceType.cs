using Translation = Queue.Model.Common.Translation;

namespace Queue.Model.Common
{
    public enum WorkplaceType
    {
        Window,
        Cabinet,
        Room,
        Box,
        Department,
        Area
    }

    public static partial class TranslationExtensions
    {
        public static string Translate(this WorkplaceType value)
        {
            return Translation.WorkplaceType.ResourceManager.GetString(value.ToString());
        }
    }
}