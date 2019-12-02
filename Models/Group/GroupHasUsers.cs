using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class GroupHasUsers
    {
        [Key]
        public int GroupHasUsersId { get; set; }
        public string GroupSerial { get; set; }
        public string UserSerial { get; set; }
        public int GroupHasUserPosition { get; set; }
        public int RoleId { get; set; }
    }
}
