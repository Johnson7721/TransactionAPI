namespace TransactionAPI.Helpers.Validator
{
    public static class ValidationConstants
    {
        public static class StringLength
        {
            public const int PartnerKey = 50;
            public const int PartnerRefNo = 50;
            public const int PartnerPassword = 50;
            public const int PartnerItemRef = 50;
            public const int Name = 100;
        }

        public static class Numeric
        {
            public const int MaxQty = 5;
            public const int MinQty = 1;
            public const int MaxDiscountPercentage = 20;
        }

        public static class Time
        {
            public const int MaxTimestampDifferenceMinutes = 5;
        }
    }
}
