using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class UserInfo
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Serial { get; set; }
        public string Phone { get; set; }
        public int Plan { get; set; }
        public int Level { get; set; }
        public bool Status { get; set; }
        public bool IsEnable { get; set; }

    }
}
