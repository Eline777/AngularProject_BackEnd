using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_VoorbereidendProject_Angular.Models
{
    public class Gebruiker
    {
        public int GebruikerID { get; set; }
        public string Email { get; set; }
        public string Wachtwoord { get; set; }
        public string Gebruikersnaam { get; set; }
        [NotMapped] //Token moet geen kolom zijn in de tabel
        public string Token { get; set; }
        public string Activatiecode { get; set; }
        public bool IsActief { get; set; }
    }
}
