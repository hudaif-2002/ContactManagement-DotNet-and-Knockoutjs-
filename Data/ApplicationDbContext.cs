using ContactManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Data;
  

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<UserModel> Users { get; set; } 
        public DbSet<ContactModel> Contacts { get; set; }

    }


