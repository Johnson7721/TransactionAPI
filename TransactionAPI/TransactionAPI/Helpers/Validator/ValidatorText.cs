namespace TransactionAPI.Helpers.Validator
{
    public static class ValidatorText
    {
        /// <summary>
        /// Checks if a string has a non-whitespace value.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if the string contains non-whitespace characters; otherwise, false.</returns>
        public static bool HasValue(string value)
         => !string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Trims a string.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <returns>The trimmed string or empty string if null.</returns>
        public static string TrimText(string value)
            => value.Trim();
    }
}
