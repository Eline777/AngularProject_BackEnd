using API_VoorbereidendProject_Angular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_VoorbereidendProject_Angular.Services
{
    public interface IGebruikerService
    {
        Gebruiker Authenticate(string gebruikersnaam, string wachtwoord);
    }
}
