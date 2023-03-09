using ContactsWebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebApi.Models.Context
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }


    }
}
