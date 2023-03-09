using ContactsWebApi.Models.Context;
using ContactsWebApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContactsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ContactDbContext _context;

        public ReportsController(ContactDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var locations = _context.ContactInformations.GroupBy(x=> x.Location).Select(s=> s.Key).ToList();
            var result = from location in locations
                         select new
                         {
                             Location = location,
                             NumberOfPeople = _context.ContactInformations.Where(x => x.Location == location).Count(),
                             NumberOfPhone = _context.ContactInformations.Where(x => x.Location == location && x.PhoneNumber != "").Count(),
                         };

            return Ok(result.OrderBy(o => o.NumberOfPeople).ToList());
        }

    }
}
