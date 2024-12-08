using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace ContactManagement.Models;

public class ContactModel
{
    [Key] 
    public int ContactId { get; set; }

    [Required] 
    public string? Name { get; set; }

    [Required]
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Company { get; set; }
    [Required]
    public int UserId { get; set; } // Foreign Key linking to UserModel
}
