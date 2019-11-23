using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_VoorbereidendProject_Angular.Models
{
    public class Poll
    {
        public int PollID { get; set; }
        public string Naam { get; set; }
        public int MakerID { get; set; } // ID van de gebruiker die de poll aangemaakt heeft

        public List<PollGebruiker> ListGebruikersPerPoll { get; set; }
        public List<Antwoord> LijstMogelijkeAntwoorden { get; set; }
    }
}
