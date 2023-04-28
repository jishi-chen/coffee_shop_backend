
using Newtonsoft.Json;

namespace InsertAddressData
{
    public class AddressModel
    {
        [JsonProperty("Zip5")]
        public string Zip5 { get; set; }
        [JsonProperty("City")]
        public string City { get; set; }
        [JsonProperty("Area")]
        public string Area { get; set; }
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}
