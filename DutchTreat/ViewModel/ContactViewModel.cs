﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.ViewModel
{
    public class ContactViewModel
    {
        [Required]
        [MinLength(5,ErrorMessage ="Name is Too Small")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Subject  { get; set; }
        [Required]
        [MaxLength(50 , ErrorMessage ="Message is too Long")]
        public string MessageBody { get; set; }
    }
}

