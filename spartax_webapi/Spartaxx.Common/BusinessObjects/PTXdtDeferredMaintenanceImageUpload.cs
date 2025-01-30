using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common.BusinessObjects
{
    public class PTXdtDeferredMaintenanceImageUpload
    {
        public int MediaId { get; set; }
        public string MediaPath { get; set; }
        public int PropertydetailsId { get; set; }
        public int AnswerId { get; set; }
        public string AccountNumber { get; set; }
        public string ClientNumber { get; set; }
        public string CountyName { get; set; }
        public string DocumentType { get; set; }
        public int TaxYear { get; set; }
        public int DocumentTypeID { get; set; }
    }
}
