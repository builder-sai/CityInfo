using System;
using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {

        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            //these cannot be used on release of course but just as POC
            Debug.WriteLine($"Sending email from {_mailFrom} to {_mailTo}, with CloudMailService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");

        }
    }
}
