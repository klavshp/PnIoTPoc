using Newtonsoft.Json;

namespace PnIotPoc.WebApi.Common.Models
{
    public class Telemetry
    {
        [JsonConstructor]
        public Telemetry() { }

        public Telemetry(string name, string displayName, string type)
        {
            Name = name;
            DisplayName = displayName;
            Type = type;
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}