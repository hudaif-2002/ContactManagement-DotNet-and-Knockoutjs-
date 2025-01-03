﻿using Microsoft.AspNetCore.Mvc;
using ContactManagement.Data;
using ContactManagement.Models;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;
namespace ContactManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : Controller
{
    private readonly ApplicationDbContext _db;
    public ContactsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: /Contacts/AddContact
    [HttpGet("AddContact")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult AddContact()
    {
        return View();
    }

    [HttpGet("GetAllContacts")]
    public IActionResult GetAllContacts()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        //   To retrieve FullName
        var user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
        if (user != null)
        {
            ViewData["FullName"] = user.FullName;
        }
        var contacts = _db.Contacts.Where(c => c.UserId == userId).ToList();
        // Check if the request is AJAX
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(contacts); // Return JSON for AJAX requests
        }
        return View("GetAllContacts", contacts); // Return view for normal requests
    }

    // POST: api/contacts
    [HttpPost]
    public ActionResult AddContact([FromBody] ContactModel contact)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return Unauthorized(new { message = "User is not logged in." });
        }
        // Ensure the contact has the correct UserId
        contact.UserId = userId.Value;
        // Check if the contact already exists in the database
        var existingContact = _db.Contacts
            .FirstOrDefault(c => c.Name == contact.Name && c.Email == contact.Email && c.Phone == contact.Phone && c.UserId == userId);
        if (existingContact != null)
        {
            return Conflict(new { message = "This contact already exists." });
        }
        _db.Contacts.Add(contact);
        _db.SaveChanges();
        // Return the newly created contact with status 201 Created
        return CreatedAtAction(nameof(GetAllContacts), new { id = contact.ContactId }, contact);
    }







    [HttpPut("Edit/{id}")]
    public IActionResult EditContact(int id, [FromBody] ContactModel contact)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        // Check if the contact to edit exists
        var existingContact = _db.Contacts.Find(id);
        if (existingContact == null)
        {
            return NotFound(new { message = "Contact not found." });
        }

        // Check if another contact with the same details already exists
        var duplicateContact = _db.Contacts
            .FirstOrDefault(c => c.Name == contact.Name
                                 && c.Email == contact.Email
                                && c.Phone==contact.Phone
                                 && c.UserId == userId); // Exclude the current contact being edited
        if (duplicateContact != null)
        {
            return Conflict(new { message = "A contact with the same details already exists." });
        }

        // Update the contact details
        existingContact.Name = contact.Name;
        existingContact.Email = contact.Email;
        existingContact.Phone = contact.Phone;
        existingContact.Company = contact.Company;

        _db.SaveChanges();
        return Ok(new { message = "Contact updated successfully." });
    }






    //[HttpPut("Edit/{id}")]
    //public IActionResult EditContact(int id, [FromBody] ContactModel contact)
    //{
    //    var existingContact = _db.Contacts.Find(id);
    //    if (existingContact == null)
    //    {
    //        return NotFound(new { message = "Contact not found." });
    //    }
    //    existingContact.Name = contact.Name;
    //    existingContact.Email = contact.Email;
    //    existingContact.Phone = contact.Phone;
    //    existingContact.Company = contact.Company;

    //    _db.SaveChanges();
    //    return Ok(new { message = "Contact updated successfully." });
    //}

    // DELETE: api/contacts/delete/{id}
    [HttpDelete("DeleteContact/{id}")]
    public IActionResult DeleteContact(int id)
    {
        var contact = _db.Contacts.Find(id);
        if (contact == null)
        {
            return NotFound(new { message = "Contact not found." });
        }
        _db.Contacts.Remove(contact);
        _db.SaveChanges();
        return Ok(new { message = "Contact deleted successfully." });
    }

    // GET: api/contacts/edit/{id}
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("Edit/{id}")]
    public IActionResult EditContact(int id)
    {
        var contact = _db.Contacts.Find(id);
        if (contact == null)
        {
            return NotFound(new { message = "Contact not found." });
        }
        return View("EditContact", contact); // View for editing a contact
    }
}


