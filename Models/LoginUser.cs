using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Wedding_Planner.Models
{
    public class LoginUser
        {
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string LoginPassword {get;set;}
        }
}