using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "User")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Grant")]
        public string Grant_Type { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Client")]
        public string Client_Id { get; set; }

        [Display(Name = "JWT Token")]
        public string Token { get; set; }

    }
}