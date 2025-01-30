using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common.BusinessObjects
{
    public class PTXdtNewClientQuickSetup
    {
        public int ClientId { get; set; }
        public int ClientSourceId { get; set; }
        public int ClientSetupMethodId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Userid { get; set; }
        public int TaxYear { get; set; }
        public string MailingAddress { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }
        public string ZipCode { get; set; }

        public string ClientNumber { get; set; }

        public string Status { get; set; }

        //Added By Pavithra.B on 08Mar2017 - Task Id - 29634
        public bool isJudicial { get; set; }

        public int ClientTypeId { get; set; }
        public int ClientStatusId { get; set; }
        public int AccountStatusId { get; set; }
        public bool IsNextYearSignup { get; set; }
    }
}
