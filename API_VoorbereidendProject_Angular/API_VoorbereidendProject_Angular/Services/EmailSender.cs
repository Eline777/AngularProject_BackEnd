using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using API_VoorbereidendProject_Angular.Models;

namespace API_VoorbereidendProject_Angular.Services
{
    public class EmailSender
    {
        private readonly AuthMessageSenderOptions _authMessageSenderOptions;

        //    public string message = "Beste [#naam], \n Bedankt om u te registreren bij PollVoter. \n Door op onderstaande link te klikken word uw account geactiveerd. \n [#activatielink]";
   

        public EmailSender(IOptions<AuthMessageSenderOptions> authMessageSenderOptions)
        {
            _authMessageSenderOptions = authMessageSenderOptions.Value;
        }

        ////    public EmailSender() { }

        //    public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        //    public Task SendEmailAsync(string email, string subject, string message)
        //    {
        //        return Execute(Options.SendGridKey, subject, message, email);
        //    }

        //    public Task Execute(string apiKey, string subject, string message, string email)
        //    {
        //            message.Replace("[#naam]", gebruiker.Gebruikersnaam);
        //            message.Replace("[#activatielink]", "http://localhost:4200/activeren/" + "/" + gebruiker.GebruikerID + "/" + gebruiker.Activatiecode.ToString());
        //            SendEmailAsync(gemail, subject, message);

        //    var sendGridClient = new SendGridClient(apiKey);
        //        var sendGridMessage = new SendGridMessage()
        //        {
        //            From = new EmailAddress("r0751363@student.thomasmore.be", Options.SendGridUser),
        //            Subject = subject,
        //            PlainTextContent = message,
        //            HtmlContent = message
        //        };
        //    sendGridMessage.AddTo(new EmailAddress(email));

        //    // Disable click tracking.
        //    // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        //    sendGridMessage.SetClickTracking(false, false);

        //    return sendGridClient.SendEmailAsync(sendGridMessage);
        //}
        //    public void PrepareMessageToSend(Gebruiker gebruiker)
        //    {
        ////        message.Replace("[#naam]", gebruiker.Gebruikersnaam);
        ////        message.Replace("[#activatielink]", "http://localhost:4200/activeren/" + "/" + gebruiker.GebruikerID + "/" + gebruiker.Activatiecode.ToString());
        ////        SendEmailAsync(gebruiker.Email, subject, message);
        ////    }
        ///

        public async Task SendRegistrationMail(Gebruiker gebruiker)
        {
            string activatielink = "http://localhost:4200/activeren/" + "/" + gebruiker.GebruikerID + "/" + gebruiker.Activatiecode.ToString();
            string message = "Beste " + gebruiker.Gebruikersnaam + ", \n Bedankt om u te registreren bij PollVoter. \n Door op onderstaande link te klikken word uw account geactiveerd. \n" + activatielink;

            var apiKey = _authMessageSenderOptions.SendGridKey;
            var apiUser = _authMessageSenderOptions.SendGridUser;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("r0751363@student.thomasmore.be", apiUser);
            var subject = "Welkom bij Poll&Friends! Bevestig uw emailadres";
            var to = new EmailAddress(gebruiker.Email, gebruiker.Gebruikersnaam);
            var plainTextContent = message;
            var htmlContent = "<strong>" + message + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}
