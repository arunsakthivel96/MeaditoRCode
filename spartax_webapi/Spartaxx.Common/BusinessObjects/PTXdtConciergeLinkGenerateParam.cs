using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common.BusinessObjects
{
    public class PTXdtConciergeLinkGenerateParam
    {
        public string AccountNumber { get; set; }
        public string County { get; set; }
        public int UserId { get; set; }
        public int ConciergeAgentID { get; set; }
        public string DMLink { get; set; }
    }
}
