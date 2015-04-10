namespace Queue.Reports
{
    public static class ReportsUtils
    {
        private static char[] chars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static string ColumnName(int index)
        {
            index -= 1;

            int quotient = index / 26;
            return quotient > 0 ? ColumnName(quotient) + chars[index % 26].ToString() : chars[index % 26].ToString();
        }
    }
}