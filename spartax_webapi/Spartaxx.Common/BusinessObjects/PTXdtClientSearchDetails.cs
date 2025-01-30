using System;

namespace Spartaxx.Common.BusinessObjects
{
    class PTXdtClientSearchDetails
    {
        public int clientID { get; set; }
        public string clientNumber { get; set; }
        public string clientName { get; set; }
        public string mailingAddress { get; set; }
        public string clientStatus { get; set; }
        public string companyName { get; set; }
        public string CountyName { get; set; }
        public string ProcessingStatus { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
