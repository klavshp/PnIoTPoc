using Newtonsoft.Json;

namespace PnIotPoc.WebApi.Common.Models
{
    public class Parameter
    {
        /// <summary>
        /// Serialization deserialization constructor.
        /// </summary>
        [JsonConstructor]
        public Parameter()
        {
        }

        public Parameter(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public string Type { get; set; }
    }
}