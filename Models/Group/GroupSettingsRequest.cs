using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class GroupSettingRequest
    {
        public double Amount { get; set; }
        public DateTime Begin { get; set; }
        public double Timelapse { get; set; }
        public int Status { get; set; }
    }
}
