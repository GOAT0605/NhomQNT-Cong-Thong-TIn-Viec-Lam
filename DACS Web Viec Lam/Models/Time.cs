﻿using DACS_Web_Viec_Lam.Models;
using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Data.Entities
{
    public class Time
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter type name")]
        [StringLength(20, ErrorMessage = "The type name cannot be more than 20 characters.")]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        public bool? Disable { get; set; }
        public ICollection<Job>? Jobs { get; set; }
    }
}