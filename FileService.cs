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
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // Check file exists
            if (File.Exists(path)) {
                using(StreamReader reader = new StreamReader(path))
                {
                    var line = reader.ReadLine();
                    var items = line.Split(',');
                    
                    // now write to DB in a seperate using...
                    using (var context = new RequestContext<ContactRequest>())
                    {
                        context.Entities.Add(new ContactRequest(items));

                        // Save the changes and report back state entries saved to db if necessary.
                        var countOfSavedEntries = context.SaveChanges();
                    }
                }
            }
        }

        public string BuildMessage(RequestContext<ContactRequest> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            // Once we have requested the data / able to get data from db.
            // It looks like we can simply query against the DbSet in the context.
            // Use Linq to get an item, SingleOrDefault is maybe not specific enough 
            // but the ideas is we can limit the subset with linq in general.

            var contactRequest = context.Entities.SingleOrDefault();

            // Build the following string:
            // Hi Joe Bloggs, this is your Crazy Service Centre, your appointment will be today at 2pm}

            var message = contactRequest != null 
                ? $"Hi {contactRequest.Name}, this is your Crazy Service Centre, your appointment will be today at {contactRequest.RequestAppointment.ToString("hh")}pm." 
                :   throw new NullReferenceException();
            
            // Could consider using StringBuilder to help with performance. 
            // Especially if BuildMessage is called from an iteration of requests for example.

            return message;

        }

        // Could consider using deligates here to send multi channels / subscribe to an event?
        public void Send(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            
            // typical email setup here, I've left out from, to etc but would go here.
            // This could be anything really.

            var host = "myService.com";
            var port = 000; // some port 
            var  mailMessage = new MailMessage();
            
            mailMessage.Subject = "Appointment";
            mailMessage.Body = message;
    
            // Open a using for making the call to our service
            using (var client = new System.Net.Mail.SmtpClient(host, port))
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