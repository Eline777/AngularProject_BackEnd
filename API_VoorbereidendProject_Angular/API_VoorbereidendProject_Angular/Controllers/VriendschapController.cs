﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_VoorbereidendProject_Angular.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using API_VoorbereidendProject_Angular.Services;
using Microsoft.Extensions.Options;

namespace API_VoorbereidendProject_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VriendschapController : ControllerBase
    {
        private readonly PollContext _context;
        private readonly AuthMessageSenderOptions _authMessageSenderOptions;

        public VriendschapController(PollContext context, IOptions<AuthMessageSenderOptions> authMessageSenderOptions)
        {
            _context = context;
            _authMessageSenderOptions = authMessageSenderOptions.Value;
        }

        // GET: api/Vriendschappen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vriendschap>>> GetVriendschap() //Dit zijn alle vriendschappen waarbij het verzoek aanvaard is
        {
            return await _context.Vriendschappen.Where(x => x.Status == 1).ToListAsync();
        }

        // GET: api/Vriendschappen/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vriendschap>> GetVriendschap(int id)
        {
            var vriendschap = await _context.Vriendschappen.FindAsync(id);

            if (vriendschap == null)
            {
                return NotFound();
            }

            return vriendschap;
        }

        // GET: api/Vriendschappen
        [HttpGet("vriendschapverzoeken/{gebruikerID}")]
        public async Task<ActionResult<IEnumerable<Vriendschap>>> GetVriendschapverzoekenByGebruiker(int gebruikerID) //Vriendschappen die nog nog niet aanvaard of geweigerd zijn hebben een status 0
        {
            // wanneer ik enkel zou ophalen op status ==  0 zou ik ook de vriendschappen ophalen waarbij hij/ zij het verzoek zelf heeft gestuurd, dus controleer ik of het actieGebruikerID verschillend is van huidig gebruikerID
            // zo krijg ik enkel de vriendschap objecten terug waarbij de huidge gebruiker moet aanvaarden of weigeren
            return await _context.Vriendschappen.Where(x => x.Status == 0).Where(x => x.ActieGebruikerID != gebruikerID).ToListAsync();
        }

        // PUT: api/Vriendschappen/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVriendschap(int id, Vriendschap vriendschap)
        {
            if (id != vriendschap.VriendschapID)
            {
                return BadRequest();
            }

            _context.Entry(vriendschap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VriendschapExists(id))
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

        // POST: api/Vriendschappen
        [HttpPost]
        public async Task<ActionResult<Vriendschap>> PostVriendschap(Vriendschap vriendschap, string emailadresVriend, Gebruiker huidigeGebruiker)
        {
            Gebruiker vriend = _context.Gebruikers.Where(x => x.Email == emailadresVriend).SingleOrDefault();
            if (vriend != null)
            {
                // vriendschap.ActieGebruikerID = ID van de gebruiker die als laatste de status aangepast heeft, deze gebruiker kan bv een geweigerd verzoek toch nog aanvaarden of andersom, 
                // of het is de gebruiker die het verzoek gestuurd heeft
                // gebruikerEenID is altijd lager dan gebruikerTweeID
                if (vriendschap.ActieGebruikerID < vriend.GebruikerID)
                {
                    vriendschap.GebruikerEenID = vriendschap.ActieGebruikerID;
                    vriendschap.GebruikerTweeID = vriend.GebruikerID;
                }
                else
                {
                    vriendschap.GebruikerTweeID = vriendschap.ActieGebruikerID;
                    vriendschap.GebruikerEenID = vriend.GebruikerID;
                }

                vriendschap.Status = 0;  // 0 = verzoek verzonden, 1 = aanvaard, 2 = geweigerd, 3 = vriend verwijderd
                _context.Vriendschappen.Add(vriendschap);
                await _context.SaveChangesAsync();
                await SendEmailFriendRequest(huidigeGebruiker, emailadresVriend, vriend);
                return CreatedAtAction("GetVriendschap", new { id = vriendschap.VriendschapID }, vriendschap);
            }
            else
            {
                await SendEmailFriendRequest(huidigeGebruiker, emailadresVriend, vriend);
                return NoContent();
            }
    }

    // DELETE: api/Vriendschappen/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Vriendschap>> DeleteVriendschap(int id)
    {
        var vriendschap = await _context.Vriendschappen.FindAsync(id);
        if (vriendschap == null)
        {
            return NotFound();
        }

        _context.Vriendschappen.Remove(vriendschap);
        await _context.SaveChangesAsync();

        return vriendschap;
    }

    private bool VriendschapExists(int id)
    {
        return _context.Vriendschappen.Any(e => e.VriendschapID == id);
    }

    [HttpPost("sendEmailFriendRequest")]
    public async Task<Response> SendEmailFriendRequest(Gebruiker gebruiker, string emailadresVriend, Gebruiker vriend)
    {
        var apiKey = _authMessageSenderOptions.SendGridKey;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(gebruiker.Email, gebruiker.Voornaam + " " + gebruiker.Achternaam);
        var to = new EmailAddress("", "");
        var subject = "";
        var message = "";
        var link = "";

        if (vriend == null) // vriend heeft nog geen account
        {
            to = new EmailAddress(emailadresVriend, "");
            subject = "Iemand nodigt u uit om lid te worden van Poll&Friends";
            link = "http://localhost:4200/registreren/";
            message = "Beste, </br>" + gebruiker.Voornaam + " " + gebruiker.Achternaam + " heeft u uitgenodigd om lid te worden van Poll&Friends </br> Via onderstaande link kan u een account aanmaken " +
                "</br><a href='http://localhost:4200/registreren/'" + "> " + link + "</a>";
        }
        else
        {
            to = new EmailAddress(vriend.Email, vriend.Voornaam + " " + vriend.Achternaam);
            subject = gebruiker.Voornaam + " " + gebruiker.Achternaam + " heeft u een vriendschapverzoek gestuurd op Poll&Friends";
            link = "http://localhost:4200/vriendschappen/vriendschapverzoeken/" + vriend.GebruikerID;
            message = "Beste + " + vriend.Voornaam + ", </br>" + gebruiker.Voornaam + " " + gebruiker.Achternaam + " heeft u een vriendschapverzoekgestuurd op Poll&Friends </br> Via onderstaande link kan u dit verzoek aanvaarden of afwijzen. " +
               "</br><a href='http://localhost:4200/vriendschappen/vriendschapverzoeken/'" + "> " + link + "</a>";
        }

        //    StringBuilder builder = new StringBuilder();
        var plainTextContent = message;
        var htmlContent = message;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
        return response;
    }
}
}
