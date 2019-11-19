using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Damda_Service.Models
{
    public class GroupHasUsersRequest
    {
        public string Group { get; set; }
        public string User { get; set; }
    }
}
