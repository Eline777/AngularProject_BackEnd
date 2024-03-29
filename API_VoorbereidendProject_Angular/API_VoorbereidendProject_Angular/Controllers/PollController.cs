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
            return await _context.Polls.Include(x => x.LijstMogelijkeAntwoorden).Include(x => x.LijstGebruikersPerPoll).ToListAsync();
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
            poll.LijstMogelijkeAntwoorden = _context.Antwoorden.Where(x => x.PollID == poll.PollID).ToList();
            List<Stem> lijstStemmen = new List<Stem>();
            foreach (Antwoord item in poll.LijstMogelijkeAntwoorden)
            {
                Stem stem = _context.Stemmen.Where(x => x.AntwoordID.Equals(item.AntwoordID)).SingleOrDefault();
                lijstStemmen.Add(stem);
                item.LijstStemmen = lijstStemmen;
            }
            return poll;
        }
        
        [HttpGet("aantal")]
        public async Task<ActionResult<int>> GetAantalPolls() // Om het totaal te tonen op de homepagina
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
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync(); // Eerst poll aanmaken zodat het juiste pollID kan toegevoegd worden aan pollGebruiker

            // Antwoorden van de poll toevoegen aan DB
            foreach (Antwoord antwoord in poll.LijstMogelijkeAntwoorden)
            {
                RedirectToAction("PostAntwoord", "AntwoordController", new { id = antwoord.AntwoordID });
            }

            // PollGebruiker tabel opvullen
            foreach (PollGebruiker pollGebruiker in poll.LijstGebruikersPerPoll)
            {
                pollGebruiker.PollID = poll.PollID;
                pollGebruiker.HeeftGestemd = false; // door het op false te zetten kan de gebruiker zien op het dashboard dat hij nog moet stemmen
            }
            _context.PollGebruikers.AddRange(poll.LijstGebruikersPerPoll);

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
    }
}
