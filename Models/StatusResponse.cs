using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class StatusResponse
    {
        [JsonProperty("status")]
        public string message { get; set; }
    }
}
