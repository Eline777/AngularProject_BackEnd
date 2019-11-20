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
                new Gebruiker { Gebruikersnaam = "Eline777", Email = "r0751363@student.thomasmore.be", Wachtwoord = "1234", IsActief=true },
                new Gebruiker { Gebruikersnaam = "Test", Email = "eline.leysen@hotmail.be", Wachtwoord = "1234", IsActief=true }
                );
            context.Polls.AddRange(
                new Poll { Naam = "TestPoll1"});

            context.SaveChanges();
        }
    }
}
