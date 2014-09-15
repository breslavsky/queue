namespace Queue.Common
{
    public static class StringExtensions
    {
        public static string ToInOffer(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            char[] chars = source.ToCharArray();

            chars[0] = char.ToUpper(chars[0]);

            for (int i = 1; i < chars.Length; i++)
            {
                chars[i] = char.ToLower(chars[i]);
            }

            return new string(chars);
        }
    }
}