
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Damda_Service.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastname { get; set; }
        public string UserPassword { get; set; }
        public string UserSerial { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserCreated { get; set; }
        public int UserStatus { get; set; }
        public int UserEnable{ get; set; }
        public int PlanId { get; set; }
        public int LevelId { get; set; }
    }
}
