using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common.BusinessObjects
{
    public class ValueSummaryParam
    {
        public string ClientNumber { get; set; }
        public string ReportName { get; set; }
        public string InactiveAccounts { get; set; }
        public string FileName { get; set; }
        public int? CAFClientId { get; set; }
    }

}
