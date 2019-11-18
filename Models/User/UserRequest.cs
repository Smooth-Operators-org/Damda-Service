using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Damda_Service.Models
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Password Must be Between 5 and 16 Characters")]
        public string Password { get; set; }
        public string Email { get; set; }
        [StringLength(11, MinimumLength = 8, ErrorMessage = "Phone Must be Between 8 and 11 Characters")]
        public string Phone { get; set; }
        public string Created { get; set; }
        public int Status { get; set; }
        public int IsEnable { get; set; }
        public int Plan { get; set; }
        public int Level { get; set; }
    }
}
