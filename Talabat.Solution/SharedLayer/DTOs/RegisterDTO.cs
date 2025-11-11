using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.DTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
