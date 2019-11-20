using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_VoorbereidendProject_Angular.Models
{
    public class Vriendschap
    {
        public int VriendschapID { get; set; }
        public int GebruikerEenID { get; set; }
        public int GebruikerTweeID { get; set; }
        public int Status { get; set; } // 0 = verzoek verzonden, 1 = aanvaard, 2 = geweigerd, 3 = vriend verwijderd
        public int ActieGebruikerID { get; set; } // ID van de gebruiker die als laatste de status aangepast heeft, deze gebruiker kan bv een geweigerd verzoek toch nog aanvaarden of andersom, of het is de gebruiker die het verzoek gestuurd heeft
        public string EmailVriend { get; set; }
    }
}
