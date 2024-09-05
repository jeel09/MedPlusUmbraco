using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedPlusUmbraco.Models
{
    public class SearchModel
    {
        public JObject Data { get; set; }
        public string ContentTypeAlias { get; set; }
    }
    public class Data
    {
        [JsonPropertyName("heading")]
        public List<string> Heading { get; set; }

        [JsonPropertyName("description")]
        public List<string> Description { get; set; }
    }
}
