using System.Text.Json.Serialization;

namespace TransactionAPI.Models
{
    public class TransactionResponse
    {
    
        [JsonPropertyName("result")]
        public int Result { get; set; }

        [JsonIgnore]
        public bool IsSuccess => Result == 1;

        [JsonPropertyName("totalamount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? TotalAmount { get; set; }

        [JsonPropertyName("totaldiscount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? TotalDiscount { get; set; }

        [JsonPropertyName("finalamount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? FinalAmount { get; set; }

        [JsonPropertyName("resultmessage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ResultMessage { get; set; }

        public static TransactionResponse Success(long totalAmount, long totalDiscount, long finalAmount)
        {
            return new TransactionResponse
            {
                Result = 1,
                TotalAmount = totalAmount,
                TotalDiscount = totalDiscount,
                FinalAmount = finalAmount
            };
        }

        public static TransactionResponse Failure(string message)
        {
            return new TransactionResponse
            {
                Result = 0,
                ResultMessage = message
            };
        }
    }
}
