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
    }
}
