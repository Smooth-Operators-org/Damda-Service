using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
