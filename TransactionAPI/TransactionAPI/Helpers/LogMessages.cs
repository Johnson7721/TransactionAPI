namespace TransactionAPI.Helpers
{
    /// <summary>
    /// Centralized log message templates.
    /// Placeholders {0}, {1}... are replaced with real values via string.Format at runtime.
    /// Date/time is added automatically by the log4net pattern layout.
    /// </summary>
    public static class LogMessages
    {
        // Requests / responses ({0} = request URL, {1} = body)
        public const string RequestBody = "Incoming request to URL '{0}' with body: {1}";
        public const string ResponseSuccess = "Request to URL '{0}' completed successfully with response: {1}";
        public const string ResponseFailure = "Request to URL '{0}' failed with response: {1}";

        // Validation
        public const string PartnerValidationFailed = "Partner validation failed. Partner key: '{0}'. Reason: {1}";
        public const string SignatureMismatch = "Signature check failed - the provided signature does not match. Partner key: '{0}', reference number: '{1}'";
        public const string SignatureException = "An unexpected error occurred while validating the signature";

        // Processing
        public const string TransactionProcessed = "Transaction processed successfully. Partner key: '{0}', reference number: '{1}', total amount: {2}";
        public const string TransactionCancelled = "Transaction processing was cancelled by the client. Partner key: '{0}'";
        public const string TransactionError = "An unexpected error occurred while processing the transaction. Partner key: '{0}'";
    }
}