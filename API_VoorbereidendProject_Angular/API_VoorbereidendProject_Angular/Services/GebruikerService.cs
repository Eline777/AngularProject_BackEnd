using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API_VoorbereidendProject_Angular.Helpers;
using API_VoorbereidendProject_Angular.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API_VoorbereidendProject_Angular.Services
{
    public class GebruikerService: IGebruikerService
    {
        private readonly AppSettings _appSettings;
        private readonly PollContext _pollContext;

        public GebruikerService(IOptions<AppSettings> appSettings, PollContext pollContext)
        {
            _appSettings = appSettings.Value;
            _pollContext = pollContext;
        }

        public Gebruiker Authenticate(string email, string wachtwoord)
        {
            Gebruiker gebruiker = _pollContext.Gebruikers.SingleOrDefault(x => x.Email == email && x.Wachtwoord == wachtwoord);

            // return null if user not found
            if (gebruiker == null)
            { 
                return null;
            }

            // authentication successful so generate jwttoken
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("GebruikerID", gebruiker.GebruikerID.ToString()),
                    new Claim("Email", gebruiker.Email),
                    new Claim("Gebruikersnaam", gebruiker.Gebruikersnaam)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            gebruiker.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            gebruiker.Wachtwoord = null;

            return gebruiker;
        }

      // Gebruiker IGebruikerService.Authenticate(string gebruikersnaam, string wachtwoord)
       // {
         //   throw new NotImplementedException();
       // }
    }
}
