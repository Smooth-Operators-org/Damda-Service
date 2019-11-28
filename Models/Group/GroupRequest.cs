using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class GroupRequest
    {
        public string Name { get; set; }
        public string Creator { get; set; }
        public GroupSettingRequest Settings { get; set; }
        public Dictionary<int, string> Users { get; set; }
    }
}
