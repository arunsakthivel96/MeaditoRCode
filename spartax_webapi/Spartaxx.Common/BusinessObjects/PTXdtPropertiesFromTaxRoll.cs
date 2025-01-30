using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common.BusinessObjects
{
    public class PTXdtPropertiesFromTaxRoll
    {
        public int CountyID { get; set; }
        public string County { get; set; }
        public string CountyCode { get; set; }
        public int ClientID { get; set; }
        public string ClientNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Exemption { get; set; }

        public string PropertyType { get; set; }
        public string OwnerName { get; set; }
        public string MailingAddress { get; set; }
        public string PropertyAddress { get; set; }

        /* Mandatory to set whether it is added by using Manual Setup popup OR from Tax Roll DB. It will be set in WF */
        public bool isAddedFromManualSetup { get; set; }

        /* Properties used to get account details from Tax Roll DB */
        public int taxYear { get; set; }
        public string AccountNoXML { get; set; } /* To retrieve details for account numbers presents in the string. The string will be in XML format */

        /* User Details */
        public int UpdatedBy { get; set; }

        /* CAFrequest */
        public bool isCAFRequest { get; set; }

        /* AccountID */
        public int AccountID { get; set; }

        /* WebSite address*/
        public string Website { get; set; }

        /* User Details */
        public int CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
    }
}
