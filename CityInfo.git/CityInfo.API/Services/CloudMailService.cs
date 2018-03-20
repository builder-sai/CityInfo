using System;
using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        
        private string _mailTo = "admin@cityinfo.com";
        private string _mailFrom = "noreply@cityinfo.com";


        public void Send(string subject, string message)
        {
            //these cannot be used on release of course but just as POC
            Debug.WriteLine($"Sending email from {_mailFrom} to {_mailTo}, with CloudMailService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");

        }
    }
}
