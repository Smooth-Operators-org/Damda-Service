using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class CouponInfo
    {
        public string CouponCode { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public int Type { get; set; }
        public double Porcent { get; set; }
        public double Amount { get; set; }
        public string Platform { get; set; }
        public string Plan { get; set; }
        public bool Status { get; set; }

    }
}
