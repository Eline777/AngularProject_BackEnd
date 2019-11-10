using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_VoorbereidendProject_Angular.Models;
using API_VoorbereidendProject_Angular.Services;
using Microsoft.AspNetCore.Authorization;

namespace API_VoorbereidendProject_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {
        private readonly PollContext _context;
        private IGebruikerService _gebruikerService;

        public GebruikerController(PollContext context, IGebruikerService gebruikerService)
        {
            _context = context;
            _gebruikerService = gebruikerService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]Gebruiker gebruikerParam)
        {
            var gebruiker = _gebruikerService.Authenticate(gebruikerParam.Gebruikersnaam, gebruikerParam.Wachtwoord);
            if (gebruiker == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            else
            {
                return Ok(gebruiker);
            }
        }
                
            
    


    // GET: api/Gebruiker
    [Authorize]
        [HttpGet]
            public async Task<ActionResult<IEnumerable<Gebruiker>>> GetGebruikers()
            {
                return await _context.Gebruikers.ToListAsync();
            }

        // GET: api/Gebruiker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gebruiker>> GetGebruiker(int id)
        {
            var gebruiker = await _context.Gebruikers.FindAsync(id);

            if (gebruiker == null)
            {
                return NotFound();
            }

            return gebruiker;
        }

        // PUT: api/Gebruiker/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGebruiker(int id, Gebruiker gebruiker)
        {
            if (id != gebruiker.GebruikerID)
            {
                return BadRequest();
            }

            _context.Entry(gebruiker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GebruikerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Gebruiker
        [HttpPost]
        public async Task<ActionResult<Gebruiker>> PostGebruiker(Gebruiker gebruiker)
        {
            _context.Gebruikers.Add(gebruiker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGebruiker", new { id = gebruiker.GebruikerID }, gebruiker);
        }

        // DELETE: api/Gebruiker/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Gebruiker>> DeleteGebruiker(int id)
        {
            var gebruiker = await _context.Gebruikers.FindAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            _context.Gebruikers.Remove(gebruiker);
            await _context.SaveChangesAsync();

            return gebruiker;
        }

        private bool GebruikerExists(int id)
        {
            return _context.Gebruikers.Any(e => e.GebruikerID == id);
        }
    }
}
