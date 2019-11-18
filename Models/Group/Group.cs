
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Damda_Service.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupSerial { get; set; }
        public string GroupName { get; set; }
        public string UserSerial { get; set; }
    }
}
