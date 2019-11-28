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
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using System.Text;

namespace API_VoorbereidendProject_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {
        private readonly PollContext _context;
        private IGebruikerService _gebruikerService;
        private readonly AuthMessageSenderOptions _authMessageSenderOptions;

        public GebruikerController(PollContext context, IGebruikerService gebruikerService, IOptions<AuthMessageSenderOptions> authMessageSenderOptions)
        {
            _context = context;
            _gebruikerService = gebruikerService;
            _authMessageSenderOptions = authMessageSenderOptions.Value;
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
     //   [Authorize]
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Gebruiker>>> GetGebruikers()
            {
            //  var userID = User.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
          //  var userID = User.Claims.FirstOrDefault(c => c.Type == "GebruikerID").Value;
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

        [HttpGet("aantal")]
        public async Task<ActionResult<int>> GetAantalGebruikers()
        {
            return await _context.Gebruikers.CountAsync();
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
            gebruiker.Activatiecode = Guid.NewGuid().ToString();
            _context.Gebruikers.Add(gebruiker);
            await _context.SaveChangesAsync();
            await SendActivationEmail(gebruiker);
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

        [HttpPost("confirmEmail/{activatiecode}")]
        public ActionResult ConfirmEmail(string activatiecode)
        {
            var result = ConfirmationEmailSucceeded(activatiecode);
            if (result == true)
            {
                return Ok(new EmailConfirmationResponse { EmailConfirmed = true });
            }
            else
            {
                return BadRequest("Er is iets misgelopen bij het bevestigen van het emailadres.");
            }
        }

        private bool ConfirmationEmailSucceeded(string activatiecode)
        {
            var gebruiker = _context.Gebruikers.SingleOrDefault(x => x.Activatiecode == activatiecode);
            // return false als gebruiker niet gevonden is
            if (gebruiker == null)
            {
                return false;
            }
            else
            {
                gebruiker.IsActief = true;
                _context.SaveChanges();
                return true;
            }
        }

        [HttpPost("sendEmail")]
        public async Task<Response> SendActivationEmail(Gebruiker gebruiker)
        {
            var apiKey = _authMessageSenderOptions.SendGridKey;
            var apiUser = _authMessageSenderOptions.SendGridUser;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("r0751363@student.thomasmore.be", apiUser);
            var subject = "Welkom bij Poll&Friends! Bevestig uw emailadres";
            var to = new EmailAddress(gebruiker.Email, gebruiker.Voornaam);

            var activatielink = "http://localhost:4200/activeren/" + gebruiker.Activatiecode;

            var message = "Beste " + gebruiker.Voornaam + ", </br> Bedankt om u te registreren bij PollVoter. </br> Door op onderstaande link te klikken word uw account geactiveerd. </br><a href=\"" + activatielink + "\"> " + activatielink + "</a>";

            var plainTextContent = message;
            var htmlContent =  message;

            //  List<EmailAddress> tos = new List<EmailAddress>
            //{
            //    new EmailAddress("test2@example.com", "Example User 2"),
            //    new EmailAddress("test3@example.com", "Example User 3"),
            //    new EmailAddress("test4@example.com","Example User 4")
            //};

            //var sendGridMessage = new SendGridMessage()
            //{
            //    From = new EmailAddress("r0751363@student.thomasmore.be", Options.SendGridUser),
            //    Subject = subject,
            //    PlainTextContent = message,
            //    HtmlContent = message
            //};

            //var displayRecipients = false; // set this to true if you want recipients to see each others mail id         
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response;
        }
    }
}
