using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class PaymentRequest
    {
        public int Week { get; set; }
        public string Payment_Date { get; set; }
        public string Payed_Date { get; set; }
        public string Extension_Date { get; set; }
        public double Amount { get; set; }
        public double Received { get; set; }
        public string User_Serial{ get; set; }
        public string Group_Serial{ get; set; }
    }
}
