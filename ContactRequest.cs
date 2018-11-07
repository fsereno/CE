using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CE
{
    public class ContactRequest
    {   
        public int Id {get; set;}
        public string Name {get; set;}
        public DateTime RequestAppointment {get; set;}
        public ContactRequest(string[] items){
            
            // Provding we know and can be sure the indexing of the incoming itmes are always the same and 0-2.
            // Could test agains the length of items to see if index available.
            // We can use the literal index value ie items[0] is ID etc...
            // Use the Constrcutor to map values, helps keep the service tidy

            // Data manipulation here
            int idTryParseOutput;
            var idTryParse = int.TryParse(items[0], out idTryParseOutput);
            DateTime requestAppointmentTryParseOutput;
            var requestAppointmentTryParse = DateTime.TryParse(items[2], out requestAppointmentTryParseOutput);

            DateTime.SpecifyKind(requestAppointmentTryParseOutput, DateTimeKind.Utc);
            requestAppointmentTryParseOutput.ToUniversalTime();

            // Map the result of above
            Id = idTryParse ? idTryParseOutput : throw new InvalidDataException();
            Name = items[1];
            RequestAppointment = requestAppointmentTryParse ? requestAppointmentTryParseOutput : throw new InvalidDataException();
        }
    }
}