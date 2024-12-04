using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContactManagement.Models
{
    public class ContactModel
    {
        [Key] // Explicitly mark this as the primary key
        public int ContactId { get; set; }

        [Required] // Mark as required to avoid null values
        public string? Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Company { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign Key linking to UserModel
    }


}
