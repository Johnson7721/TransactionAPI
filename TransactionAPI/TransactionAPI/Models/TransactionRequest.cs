using System.Text.Json.Serialization;

namespace TransactionAPI.Models
{
    public class TransactionRequest
    {
        [JsonPropertyName("partnerkey")]
        public string PartnerKey { get; set; } = string.Empty;


        [JsonPropertyName("partnerrefno")]
        public string PartnerRefNo { get; set; } = string.Empty;


        [JsonPropertyName("partnerpassword")]
        public string PartnerPassword { get; set; } = string.Empty;

        [JsonPropertyName("totalamount")]
        public long? TotalAmount { get; set; }

       
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = string.Empty;

        [JsonPropertyName("sig")]
        public string Sig { get; set; } = string.Empty;

        [JsonPropertyName("items")]
        public List<ItemDetail>? Items { get; set; }
    }

    public class ItemDetail
    {
        [JsonPropertyName("partneritemref")]
        public string PartnerItemRef { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("qty")]
        public int? Qty { get; set; }

        [JsonPropertyName("unitprice")]
        public long? UnitPrice { get; set; }
    }
}
