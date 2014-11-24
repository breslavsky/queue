using Translation = Queue.Model.Common.Translation;

namespace Queue.Model.Common
{
    public enum WorkplaceModificator
    {
        None = 0,

        A_EN = 1,
        B_EN = 2,
        C_EN = 3,
        D_EN = 4,
        E_EN = 5,
        F_EN = 6,
        G_EN = 7,
        H_EN = 8,

        CHAR1 = 101,
        CHAR2 = 102,
        CHAR3 = 103,
        CHAR4 = 104,
        CHAR5 = 105,
        CHAR6 = 106,
        CHAR7 = 107,
        CHAR8 = 108
    }

    public static partial class TranslationExtensions
    {
        public static string Translate(this WorkplaceModificator value)
        {
            return Translation.WorkplaceModificator.ResourceManager.GetString(value.ToString());
        }
    }
}