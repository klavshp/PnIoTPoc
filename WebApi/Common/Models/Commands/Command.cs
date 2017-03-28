using System.Collections.Generic;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace PnIotPoc.WebApi.Common.Models.Commands
{
    public class Command
    {
        /// <summary>
        /// Serialziation deserialziation constructor.
        /// </summary>
        [JsonConstructor]
        public Command()
        {
            Parameters = new List<Parameter>();
        }

        public Command(string name, IEnumerable<Parameter> parameters = null) : this()
        {
            Name = name;
            if (parameters != null)
            {
                Parameters.AddRange(parameters);
            }
        }

        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}