using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class GroupInfo
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public DateTime Begin { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
    }
}
