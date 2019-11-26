using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class CouponRequest
    {
        public int Type { get; set; }
        public double Porcent { get; set; }
        public double Amount { get; set; }
        public string[] Platform { get; set; }
        public string[] Plan { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
