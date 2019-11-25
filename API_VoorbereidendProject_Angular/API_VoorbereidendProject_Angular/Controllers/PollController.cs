﻿using System;
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
    public class PollController : ControllerBase
    {
        private readonly PollContext _context;

        public PollController(PollContext context)
        {
            _context = context;
        }

        // GET: api/Poll
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Poll>>> GetPolls()
        {
            return await _context.Polls.ToListAsync();
        }

        // GET: api/Poll/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Poll>> GetPoll(int id)
        {
            var poll = await _context.Polls.FindAsync(id);

            if (poll == null)
            {
                return NotFound();
            }

            return poll;
        }

        [HttpGet("aantal")]
        public async Task<ActionResult<int>> GetAantalPolls()
        {
            return await _context.Polls.CountAsync();
        }

        // PUT: api/Poll/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoll(int id, Poll poll)
        {
            if (id != poll.PollID)
            {
                return BadRequest();
            }

            _context.Entry(poll).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollExists(id))
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

        // POST: api/Poll
        [HttpPost]
        public async Task<ActionResult<Poll>> PostPoll(Poll poll)
        {

           // RedirectToAction("Action", "Controller", new { id = 99 })
            // Antwoorden van de poll toevoegen aan DB
            foreach (Antwoord antwoord in poll.LijstMogelijkeAntwoorden)
            {
                // antwoord.PollID = poll.PollID;
                // _context.Antwoorden.Add(antwoord);
                RedirectToAction("PostAntwoord", "AntwoordController", new { id = antwoord.AntwoordID });
            }

            // PollGebruiker tabel opvullen
            foreach (PollGebruiker pollGebruiker in poll.ListGebruikersPerPoll)
            {
                pollGebruiker.PollID = poll.PollID;
                pollGebruiker.HeeftGestemd = false; // door het op false te zetten kan de gebruiker zien op het dashboard dat hij nog moet stemmen
                //_context.PollGebruikers.Add(pollGebruiker);
            }
            _context.PollGebruikers.AddRange(poll.ListGebruikersPerPoll);

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoll", new { id = poll.PollID }, poll);
        }

        // DELETE: api/Poll/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Poll>> DeletePoll(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();

            return poll;
        }

        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.PollID == id);
        }

        // POST: api/Poll/StemToevoegen
        //[HttpPost("StemToevoegen")]
        //public async Task<ActionResult<Poll>> AddStemToPollAntwoord(int antwoordID, int gebruikerID)
        //{
        //    Stem stem = new Stem();
        //    stem.AntwoordID = antwoordID;
        //    stem.GebruikerID = gebruikerID;

        //    _context.Stemmen.Add(stem);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("GetStemmenByAntwoord", "StemController", new { id = antwoordID });
        //  //  return CreatedAtAction("GetPoll", new { id = stem.StemID }, stem);
        //}

        // GET: api/Poll
        //[HttpGet("{antwoordID}/Stemmen")]
        //public async Task<ActionResult<IEnumerable<Stem>>> GetStemmenByAntwoord(int antwoordID)
        //{
        //   // List<Stem> lijstStemmen = _context.Stemmen.Where(x => x.AntwoordID == antwoordID).ToList();
        //    return await _context.Stemmen.Where(x => x.AntwoordID == antwoordID).ToListAsync();
        //}
    }
}
