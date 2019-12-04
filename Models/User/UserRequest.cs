using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Damda_Service.Models
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Created { get; set; }
        public bool Status { get; set; }
        public bool IsEnable { get; set; }
        public int Plan { get; set; }
        public int Level { get; set; }
    }
}
