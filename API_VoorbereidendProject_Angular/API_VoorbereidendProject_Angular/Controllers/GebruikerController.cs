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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace API_VoorbereidendProject_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {
        private readonly PollContext _context;
        private IGebruikerService _gebruikerService;
        private readonly EmailSender _emailSender;

        public GebruikerController(PollContext context, IGebruikerService gebruikerService, EmailSender emailSender)
        {
            _context = context;
            _gebruikerService = gebruikerService;
            _emailSender = emailSender;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]Gebruiker gebruikerParam)
        {
            var gebruiker = _gebruikerService.Authenticate(gebruikerParam.Email, gebruikerParam.Wachtwoord);
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
            //  var userID = User.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            var userID = User.Claims.FirstOrDefault(c => c.Type == "GebruikerID").Value;
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
            gebruiker.Activatiecode = Guid.NewGuid();
            _context.Gebruikers.Add(gebruiker);
            await _context.SaveChangesAsync();
            await _emailSender.SendRegistrationMail(gebruiker);
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


       


        [HttpPost("confirmEmail")]
       // [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public IActionResult ConfirmEmail(int gebruikerID, Guid activatieCode)
        {
            var result = ConfirmationEmailSucceeded(gebruikerID, activatieCode);
            if (result)
            {
                return Ok(new EmailConfirmationResponse { EmailConfirmed = true });
            }
            else
            {
                return BadRequest("Er is iets misgelopen bij het bevestigen van het emailadres.");
            }
        }

        private bool ConfirmationEmailSucceeded(int gebruikerID, Guid activatiecode)
        {
            var gebruiker = _context.Gebruikers.SingleOrDefault(x => x.GebruikerID == gebruikerID && x.Activatiecode == activatiecode);
            // return false als gebruiker niet gevonden is
            if (gebruiker == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
