namespace SharedUtilities.Utilties
{
    public static class ConversionUtilities
    {
        public static int ParseInt(this string value, int defaultIntValue = 0)
        {
            int parsedInt;

            if (int.TryParse(value, out parsedInt))
                return parsedInt;

            return defaultIntValue;
        }
    }
}