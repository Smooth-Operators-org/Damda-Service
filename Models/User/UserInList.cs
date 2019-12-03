using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class UserInList
    {
        public int Position { get; set; }
        public string Serial { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Payment_Date { get; set; }
        public string Payment_Extension { get; set; }
        public double Payment_Amount { get; set; }
        public double Payment_Expected { get; set; }
        public bool Payment_Status { get; set; }
    }
}
