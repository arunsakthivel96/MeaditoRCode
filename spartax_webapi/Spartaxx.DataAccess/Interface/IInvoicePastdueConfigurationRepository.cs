using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
   public interface IInvoicePastdueConfigurationRepository
    {
        List<PTXboPastdueRunningStatus> GetPastdueRunningStatus();
        List<PTXboPastDueGenerationHistory> GetPastDueHistory();
    }
}
