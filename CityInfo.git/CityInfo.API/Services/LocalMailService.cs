using System;
using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        //adding a fake/local mail custom service
        private string _mailTo = "admin@cityinfo.com";
        private string _mailFrom = "noreply@cityinfo.com";

       //adding a function to mimic sending an email
        public void Send(string subject, string message)
        {
            //"SEND" email, this will write to the output window
            Debug.WriteLine($"Sending email from {_mailFrom} to {_mailTo}, with LocalMailService");//this.GetType().Name};
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
            //now we have created a "mail service"
            //now we can register this with the container so we can inject it using 
            //the build-in dependency injection system onto the ConfigureServices method
            //in the Startup class
        }
    }
}
