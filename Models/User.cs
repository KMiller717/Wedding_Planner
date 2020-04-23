using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
    


namespace Wedding_Planner{

    public class User
    {
        [Key]

        public int UserId {get;set;}

        [Required]
        [Display(Name = "First Name")]
        [MinLength(2, ErrorMessage = "Must be at least 2 characters")]
        public string FirstName {get;set;}
        [Required]
        [Display(Name = "Last Name")]
        [MinLength(2, ErrorMessage = "Must be at least 2 characters")]
        public string LastName {get;set;}
        [EmailAddress]
        [Required]
        public string Email {get;set;}
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer ")]
        public string Password {get; set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}

        public List<Wedding> CreatedWeddings {get;set;}

        public List<RSVP> MyWeddings {get;set;}

    }



}