using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class PaymentInfo
    {
        public string Date { get; set; }
        public string Extension { get; set; }
        public bool Status { get; set; }
        public double Amount { get; set; }
        public double Received { get; set; }
    }
}
