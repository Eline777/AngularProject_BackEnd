using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_VoorbereidendProject_Angular.Models
{
    public class PollGebruiker
    {
        public int PollGebruikerID { get; set; }
        public int PollID { get; set; }
        public int GebruikerID { get; set; }
        public bool HeeftGestemd { get; set; } // door het op false te zetten kan de gebruiker zien op het dashboard dat hij nog moet stemmen voor een bepaalde poll
    }
}
