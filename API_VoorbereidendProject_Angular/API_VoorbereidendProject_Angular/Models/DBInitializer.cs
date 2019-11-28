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
            context.Gebruikers.AddRange(
                new Gebruiker { Gebruikersnaam = "Eline1", Voornaam = "Eline", Achternaam = "Leysen", Email = "r0751363@student.thomasmore.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Eline2", Voornaam = "Eline", Achternaam = "Leysen", Email = "eline.leysen@hotmail.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy1", Voornaam = "Jos", Achternaam = "Janssen", Email = "Dummy1@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy2", Voornaam = "Tom", Achternaam = "Peeters", Email = "Dummy2@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy3", Voornaam = "Jan", Achternaam = "Mertens", Email = "Dummy3@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy4", Voornaam = "Tim", Achternaam = "Hannes", Email = "Dummy4@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy5", Voornaam = "Marc", Achternaam = "Laenen", Email = "Dummy5@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy6", Voornaam = "Pieter", Achternaam = "Wilms", Email = "Dummy6@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy7", Voornaam = "Leen", Achternaam = "Ooms", Email = "Dummy7@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy8", Voornaam = "Sarah", Achternaam = "Mols", Email = "Dummy8@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy9", Voornaam = "Jef", Achternaam = "Stevens", Email = "Dummy9@test.be", Wachtwoord = "1234", IsActief = true },
                new Gebruiker { Gebruikersnaam = "Dummy10", Voornaam = "Lucas", Achternaam = "Nys", Email = "Dummy10@test.be", Wachtwoord = "1234", IsActief = true });
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
