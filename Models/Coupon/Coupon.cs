using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public DateTime CouponStart_Date { get; set; }
        public DateTime CouponEnd_Date { get; set; }
        public int CouponType { get; set; }
        public double CouponPorcent { get; set; }
        public double CouponAmount { get; set; }
        public string CouponPlatform { get; set; }
        public string CouponPlan { get; set; }
        public bool CouponStatus { get; set; }

    }
}
