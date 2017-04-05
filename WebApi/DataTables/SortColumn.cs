using System.Globalization;
using Newtonsoft.Json;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.DataTables
{
    public class SortColumn
    {
        [JsonProperty("column")]
        public string ColumnIndexAsString { get; set; }
        public int ColumnIndex => int.Parse(this.ColumnIndexAsString, NumberStyles.Integer, CultureInfo.CurrentCulture);

        [JsonProperty("dir")]
        private string Direction { get; set; }

        public QuerySortOrder SortOrder => Direction == "asc" ? QuerySortOrder.Ascending : QuerySortOrder.Descending;
    }
}