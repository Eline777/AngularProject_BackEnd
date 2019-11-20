using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_VoorbereidendProject_Angular.Models;

namespace API_VoorbereidendProject_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VriendschapController : ControllerBase
    {
        private readonly PollContext _context;

        public VriendschapController(PollContext context)
        {
            _context = context;
        }

        // GET: api/Vriendschappen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vriendschap>>> GetVriendschap()
        {
            return await _context.Vriendschappen.ToListAsync();
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
        public async Task<ActionResult<Vriendschap>> PostVriendschap(Vriendschap vriendschap)
        {
            Gebruiker gebruiker = _context.Gebruikers.Where(x => x.Email == vriendschap.EmailVriend).SingleOrDefault();
            if (gebruiker == null)
            {
                // email sturen
                return NoContent();
            }
            else
            {
                // vriendschap.ActieGebruikerID = ID van de gebruiker die als laatste de status aangepast heeft, deze gebruiker kan bv een geweigerd verzoek toch nog aanvaarden of andersom, 
                // of het is de gebruiker die het verzoek gestuurd heeft
                if (vriendschap.ActieGebruikerID < gebruiker.GebruikerID)
                {
                    vriendschap.GebruikerEenID = vriendschap.ActieGebruikerID;
                    vriendschap.GebruikerTweeID = gebruiker.GebruikerID;
                }
                else
                {
                   vriendschap.GebruikerTweeID = vriendschap.ActieGebruikerID;
                   vriendschap.GebruikerEenID = gebruiker.GebruikerID;
                }

                vriendschap.Status = 0;  // 0 = verzoek verzonden, 1 = aanvaard, 2 = geweigerd, 3 = vriend verwijderd
                _context.Vriendschappen.Add(vriendschap);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetVriendschap", new { id = vriendschap.VriendschapID }, vriendschap);
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
    }
}
