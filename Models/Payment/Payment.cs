using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentExtension { get; set; }
        public bool PaymentStatus { get; set; }
        public double PaymentAmount { get; set; }
        public double PaymentReceived { get; set; }
        public int GroupHasUsersId { get; set; }
    }
}
