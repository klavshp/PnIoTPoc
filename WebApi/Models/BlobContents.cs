using System;
using System.IO;

namespace PnIotPoc.WebApi.Models
{
    public class BlobContents
    {
        public Stream Data { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }
}
