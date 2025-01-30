using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common.BusinessObjects
{
    public class PTXdtPropertydetailsUpdateParam
    {

        public string AccountNumber { get; set; }
        public string County { get; set; }
        public int TaxYear { get; set; }
        public decimal FinalValue { get; set; }  
        public int UserId { get; set; }
        public string UpdatedUser { get; set; }
    }
}
