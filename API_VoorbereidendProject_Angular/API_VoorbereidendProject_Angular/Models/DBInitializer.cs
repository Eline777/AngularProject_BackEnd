using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_VoorbereidendProject_Angular.Models
{
    public class DBInitializer
    {
        public static void Initialize(PollContext context)
        {
            context.Database.EnsureCreated(); 
            if (context.Gebruikers.Any())
            {
                return;   // DB has been seeded
            }
            //context.Gebruikers.AddRange(
            //    new Gebruiker { Gebruikersnaam = "Eline777", Voornaam = "Eline", Achternaam = "Leysen", Email = "r0751363@student.thomasmore.be", Wachtwoord = "1234", IsActief = true
            //    }
                //new Gebruiker
                //{
                //    Gebruikersnaam = "Eline2",
                //    Voornaam = "Eline 2",
                //    Achternaam = "Leysen 2",
                //    Email = "eline.leysen@hotmail.be",
                //    Wachtwoord = "1234",
                //    IsActief = true
                //}
               // );
            //context.Vriendschappen.Add(new Vriendschap { GebruikerEenID = 0, GebruikerTweeID = 1, Status = 1, ActieGebruikerID = 0 });
            //context.Polls.AddRange(
            //    new Poll { PollID=0, MakerID=0, Naam = "Favoriete voetbalclub?"});
            //context.Antwoorden.Add(new Antwoord { PollID = 0, AntwoordID = 0, AntwoordPoll = "Club Brugge" });
            //context.Antwoorden.Add(new Antwoord { PollID = 0, AntwoordID = 1, AntwoordPoll = "Anderlecht" });
            //context.Stemmen.Add(new Stem { GebruikerID = 0, AntwoordID = 0 });
            //context.Stemmen.Add(new Stem { GebruikerID = 1, AntwoordID = 0 });
            context.SaveChanges();
        }
    }
}
