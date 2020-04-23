using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Wedding_Planner.Models;

namespace Wedding_Planner{

    public class Wedding
    {
        [Key]

        public int WeddingId {get;set;}
        [Required]
        public string WedderOne {get;set;}

        [Required]
        public string WedderTwo {get;set;}

        [Required]
        [FutureDate]
        public DateTime Date {get;set;}
        [Required]
        public string Address {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public int UserId {get;set;}
        public User Creator {get;set;}
        public List<RSVP> Attendees {get;set;}


    }


}