using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(40)]
        public string? Name { get; set; }

        [MaxLength(100)]    
        public string? StreetAddress { get; set; }

        [MaxLength(40)]
        public string? City { get; set; }

        [MaxLength(40)]
        public string? State { get; set; }

        [Required]
        [MaxLength(5)]
        public string? PostalCode { get; set; }
    }
}
