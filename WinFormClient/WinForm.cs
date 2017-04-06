using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace WinFormClient
{
    public partial class WinForm : Form
    {
        public class Device
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public WinForm()
        {
            InitializeComponent();

            DownloadPageAsync();
        }

        static async void DownloadPageAsync()
        {
            string url = "http://localhost:59550/";

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    using (var content = response.Content)
                    {
                        var result = await content.ReadAsStringAsync();

                        if (result != null && result.Length >= 50)
                        {
                            Console.WriteLine(result.Substring(0, 50) + "...");
                        }
                    }
                }
            }
        }
    }
}
