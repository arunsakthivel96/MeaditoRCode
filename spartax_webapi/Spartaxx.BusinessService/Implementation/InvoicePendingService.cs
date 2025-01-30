using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
   public class InvoicePendingService:IInvoicePendingService
    {
        private readonly UnitOfWork _unitOfWork = null;
        public InvoicePendingService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<PTXboPendingInvoices> GetPendingInvoiceDetails(PTXboPendingInvoiceSearchCriteria searchCriteria)
        {
           return _unitOfWork.InvoicePendingRepository.GetPendingInvoiceDetails(searchCriteria);
        }

       public List<PTXboPendingInvoices> GetInvoicePendingAccountDetails(PTXboPendingInvoiceInput pendingInvoiceInput)
        {
            return _unitOfWork.InvoicePendingRepository.GetInvoicePendingAccountDetails(pendingInvoiceInput);
        }

        public PTXboHearingResult GetHearingResult(int hearingResultID)
        {
            return _unitOfWork.InvoicePendingRepository.GetHearingResult(hearingResultID);
        }

        public bool InsertInvoiceDataAccountLevel(PTXboInvoice objInvoiceFromHearingResult)
        {
            return _unitOfWork.InvoicePendingRepository.InsertInvoiceDataAccountLevel(objInvoiceFromHearingResult);
        }

    }
}
