using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace CE
{
    public class FileService
    {
        public void ReadFileToDB(string path){

            // Check file exists
            if (File.Exists(path)) {
                using(StreamReader reader = new StreamReader(path))
                {
                    var line = reader.ReadLine();
                    var items = line.Split(',');
                    
                    // now write to DB in a seperate using...
                    using (var context = new RequestContext<ContactRequest>())
                    {
                        context.Collection.Add(new ContactRequest(items));

                        // Save the changes and report back state entries saved to db.
                        var countOfSavedEntries = context.SaveChanges();
                    }
                }
            }
        }

        public string BuildMessage(RequestContext<ContactRequest> context)
        {
            // Once we have requested the data
            // Use Linq to get an item from DB, SingleOrDefault is maybe not specific enough 
            //but we can limit the subset with linq in general.

            var contactRequest = context.Collection.SingleOrDefault();

            // Build the following string:
            // Hi Joe Bloggs, this is your Crazy Service Centre, your appointment will be today at 2pm}

            var message = $"Hi {contactRequest.Name}, this is your Crazy Service Centre, your appointment will be today at {contactRequest.RequestAppointment.ToString("hh")}pm.";

            return message;

        }

        // Could consider using deligates here to send multi channels / subscribe to an event?
        public void Send(ContactRequest context, string message)
        {

            string HOST = "myService.com";
            int PORT = 587;
            
            // typical email setup here, I've left out from, to etc but would go here
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "Appointment";
            mailMessage.Body = message;
    
            // Open a using for making the call to our service
            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Do any config here for client
                // Try to send
                try
                {
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error message: " + ex.Message);
                }
            }
        }
    }
}