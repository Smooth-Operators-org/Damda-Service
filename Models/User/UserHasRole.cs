using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class UserHasRole
    {
        [Key]
        public int UserHasRoleId { get; set; }
        public string UserSerial { get; set; }
        public string GroupSerial { get; set; }
        public int RoleId { get; set; }
    }
}
