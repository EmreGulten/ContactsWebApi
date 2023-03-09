using ContactsWebApi.Dtos;
using ContactsWebApi.Models.Context;
using ContactsWebApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInformationsController : ControllerBase
    {
        private readonly ContactDbContext _context;

        public ContactInformationsController(ContactDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ContactInformation contactInformation)
        {
            await _context.ContactInformations.AddAsync(contactInformation);
            await _context.SaveChangesAsync();

            Contact contact = await _context.Contacts.Include(x=> x.ContactInformations).FirstAsync(x=> x.Id== contactInformation.ContactId);

            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ContactInformation contactInformation = await _context.ContactInformations.FindAsync(id);
            _context.ContactInformations.Remove(contactInformation);
            await _context.SaveChangesAsync();

            Contact contact = await _context.Contacts.Include(x => x.ContactInformations).FirstAsync(x => x.Id == contactInformation.ContactId);

            return Ok(contact);
        }
    }
}
