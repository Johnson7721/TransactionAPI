namespace TransactionAPI.Helpers.Validator
{
    public static class ValidationMessages
    {
        // Required field messages 
      
        public const string RequestBodyRequired = "Request body is required.";

        public const string PartnerKeyRequired = "partnerkey is required.";

        public const string PartnerRefNoRequired = "partnerrefno is required.";

        public const string PartnerPasswordRequired = "partnerpassword is required.";
       
        public const string TotalAmountRequired = "totalamount is required.";

        public const string TimestampRequired = "timestamp is required.";

        public const string SigRequired = "sig is required.";

        public const string PartnerItemRefRequired = "partneritemref is required.";

        public const string NameRequired = "name is required.";

        public const string ItemQtyRequired = "item qty is required.";
       
        public const string ItemUnitPriceRequired = "item unitprice is required.";

        public const string InvalidRequestFormat = "Invalid request format.";
       
        // Maximum length messages

        public static string PartnerKeyTooLong => $"partnerkey must not exceed {ValidationConstants.StringLength.PartnerKey} characters.";

        public static string PartnerRefNoTooLong => $"partnerrefno must not exceed {ValidationConstants.StringLength.PartnerRefNo} characters.";
      
        public static string PartnerPasswordTooLong => $"partnerpassword must not exceed {ValidationConstants.StringLength.PartnerPassword} characters.";
       
        public static string PartnerItemRefTooLong => $"partneritemref must not exceed {ValidationConstants.StringLength.PartnerItemRef} characters.";
       
        public static string NameTooLong => $"name must not exceed {ValidationConstants.StringLength.Name} characters.";

      
        // Value / format rules 
        public const string TotalAmountPositive = "totalamount must be a positive value.";
      
        public const string InvalidTotalAmount = "Invalid Total Amount.";
       
        public const string TimestampInvalidFormat = "timestamp must be a valid ISO 8601 format eg: 2024-08-15T02:11:22.0000000Z.";
       
        public const string QtyPositive = "qty must be a positive value.";
     
        public static string QtyMaxExceeded => $"qty must not exceed {ValidationConstants.Numeric.MaxQty}.";
       
        public const string UnitPricePositive = "unitprice must be a positive value.";
     
        public const string InvalidItemDetail = "Invalid item detail.";

       
        // Security / authorization
        public const string AccessDenied = "Access Denied!";
      
        public const string Expired = "Expired.";

        
        // unexpected server-side failures.
        public const string ProcessingError = "An error occurred while processing the transaction.";
    }
}
