using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public class InvoicePastdueConfigurationService:IInvoicePastdueConfigurationService
    {
        private readonly UnitOfWork _unitOfWork = null;
        public InvoicePastdueConfigurationService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public List<PTXboPastdueRunningStatus> GetPastdueRunningStatus()
        {
            return _unitOfWork.InvoicePastdueConfigurationRepository.GetPastdueRunningStatus();

        }

        public List<PTXboPastDueGenerationHistory> GetPastDueHistory()
        {
            return _unitOfWork.InvoicePastdueConfigurationRepository.GetPastDueHistory();
        }
    }
}
