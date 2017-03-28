using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace PnIotPoc.WebApi
{
    public partial class Startup
    {
        public void ConfigureJson(IAppBuilder app)
        {
            MediaTypeFormatterCollection formatters = HttpConfiguration.Formatters;
            JsonMediaTypeFormatter jsonFormatter = formatters.JsonFormatter;
            JsonSerializerSettings settings = jsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}