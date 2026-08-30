namespace TransactionAPI.Helpers
{
 
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
        public const string RequestExpired = "Request has expired. Partner key: '{0}', timestamp: '{1}'";
        public const string InvalidItemTotal = "Item total mismatch for partner {0}, ref {1}.";

        // Processing
        public const string TransactionProcessed = "Transaction processed successfully. Partner key: '{0}', reference number: '{1}', total amount: {2}";
        public const string TransactionCancelled = "Transaction processing was cancelled by the client. Partner key: '{0}'";
        public const string TransactionError = "An unexpected error occurred while processing the transaction. Partner key: '{0}'";
    }
}