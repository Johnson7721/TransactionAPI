namespace TransactionAPI.Helpers.Validator
{
    /// <summary>
    /// Contains validation constraint constants.
    /// </summary>
    public static class ValidationConstants
    {
        /// <summary>
        /// String length constraints
        /// </summary>
        public static class StringLength
        {
            public const int PartnerKey = 50;
            public const int PartnerRefNo = 50;
            public const int PartnerPassword = 50;
            public const int PartnerItemRef = 50;
            public const int Name = 100;
        }

        /// <summary>
        /// Numeric constraints
        /// </summary>
        public static class Numeric
        {
            public const int MaxQty = 5;
            public const int MinQty = 1;
            public const int MaxDiscountPercentage = 20;
        }

        /// <summary>
        /// Time-based constraints
        /// </summary>
        public static class Time
        {
            public const int MaxTimestampDifferenceMinutes = 5;
        }
    }
}
