﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.DataTables
{
    public class DataTablesRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<Column> Columns { get; set; }
        [JsonProperty("order")]
        public List<SortColumn> SortColumns { get; set; }
        public Search Search { get; set; }
        public List<FilterInfo> Filters { get; set; }
    }
}