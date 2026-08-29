namespace TransactionAPI.Helpers.Validator
{
    public static class ValidatorText
    {
        public static bool HasValue(string value)
         => !string.IsNullOrWhiteSpace(value);

        public static string TrimText(string value)
            => value.Trim();
    }
}
