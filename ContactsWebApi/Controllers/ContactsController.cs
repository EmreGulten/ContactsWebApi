using AutoMapper;
using ContactsWebApi.Dtos;
using ContactsWebApi.Models.Context;
using ContactsWebApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace ContactsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactDbContext _context;
        private readonly IMapper _mapper;

        public ContactsController(ContactDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ContactDto contactDto)
        {
            Contact contact = _mapper.Map<Contact>(contactDto);

            await _context.AddAsync(contact);
            await _context.SaveChangesAsync();

            RemoveCache<List<Contact>>("Contact");

            return Ok("Registration process successful.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ContactDto contactDto)
        {
            Contact contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return BadRequest("Registration not found.");
            }

            contact.Name = contactDto.Name;
            contact.LastName = contactDto.LastName;
            contact.Company = contactDto.Company;

            await _context.SaveChangesAsync();

            RemoveCache<List<Contact>>("Contact");

            return Ok("Update process successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Contact contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return BadRequest("Registration not found.");
            }
            _context.Contacts.Remove(contact);

            await _context.SaveChangesAsync();

            RemoveCache<List<Contact>>("Contact");

            return Ok("Delete process successful");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            List<Contact> contactList = GetCache<List<Contact>>("Contact");

            if (contactList == null)
            {
                contactList = await _context.Contacts.Include(x => x.ContactInformations).ToListAsync();
                SetCache<List<Contact>>("Contact", contactList);
            }

            return Ok(contactList);
        }

        void SetCache<T>(string key, T value)
        {
            var redisclient = new RedisClient("localhost", 6379);
            IRedisTypedClient<T> contacts = redisclient.As<T>();

            redisclient.Set<T>(key, value);
        }

        T GetCache<T>(string key)
        {
            var redisclient = new RedisClient("localhost", 6379);
            IRedisTypedClient<T> contacts = redisclient.As<T>();

            return redisclient.Get<T>(key);
        }

        void RemoveCache<T>(string key)
        {
            var redisclient = new RedisClient("localhost", 6379);
            IRedisTypedClient<T> contacts = redisclient.As<T>();

            redisclient.Remove(key);
        }

    }
}
