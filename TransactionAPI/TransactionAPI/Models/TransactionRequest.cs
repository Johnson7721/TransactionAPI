using System.Text.Json.Serialization;

namespace TransactionAPI.Models
{
    public class TransactionRequest
    {
        [JsonPropertyName("partnerkey"), JsonRequired]
        public string PartnerKey { get; set; } = string.Empty;


        [JsonPropertyName("partnerrefno"), JsonRequired]
        public string PartnerRefNo { get; set; } = string.Empty;


        [JsonPropertyName("partnerpassword"), JsonRequired]
        public string PartnerPassword { get; set; } = string.Empty;

        [JsonPropertyName("totalamount"), JsonRequired]
        public long TotalAmount { get; set; }

       
        [JsonPropertyName("timestamp"), JsonRequired]
        public string Timestamp { get; set; } = string.Empty;

        [JsonPropertyName("sig"), JsonRequired]
        public string Sig { get; set; } = string.Empty;

        [JsonPropertyName("items")]
        public List<ItemDetail>? Items { get; set; }
    }

    public class ItemDetail
    {
        [JsonPropertyName("partneritemref"), JsonRequired]
        public string PartnerItemRef { get; set; } = string.Empty;

        [JsonPropertyName("name"), JsonRequired]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("qty"), JsonRequired]
        public int Qty { get; set; }

        [JsonPropertyName("unitprice"), JsonRequired]
        public long UnitPrice { get; set; }
    }
}
