using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class GroupSettings
    {
        [Key]
        public int GroupSettingsId { get; set; }
        public double GroupSettingsAmount { get; set; }
        public DateTime GroupSettingsBegin { get; set; }
        public DateTime GroupSettingsEnd { get; set; }
        public int GroupSettingsStatus { get; set; }
        public string GroupSerial { get; set; }
    }
}
