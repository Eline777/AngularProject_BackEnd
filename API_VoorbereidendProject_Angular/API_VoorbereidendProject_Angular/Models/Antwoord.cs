using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_VoorbereidendProject_Angular.Models
{
    public class Antwoord
    {
        public int AntwoordID { get; set; }
        public string AntwoordPoll{ get; set; }
        public int PollID { get; set; }
    }
}
