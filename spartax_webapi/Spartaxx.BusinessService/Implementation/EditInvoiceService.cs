using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
   public class EditInvoiceService:IEditInvoiceService
    {
        private readonly UnitOfWork _unitOfWork = null;
        public EditInvoiceService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<PTXboEditInvoiceSearchResult> GetEditInvoiceDetails(PTXboEditInvoiceSearchCriteria SearchCriteria)
        {
            return _unitOfWork.EditInvoiceRepository.GetEditInvoiceDetails(SearchCriteria);
        }

        public PTXboEditInvoiceAccountDetails GetEditInvoiceClientDetails(int invoiceID)
        {
            return _unitOfWork.EditInvoiceRepository.GetEditInvoiceClientDetails(invoiceID);
        }

        public List<PTXboCSInvoiceHistory> GetEditInvoicePayments(int invoiceID, int invoiceType)
        {
            return _unitOfWork.EditInvoiceRepository.GetEditInvoicePayments(invoiceID,invoiceType);
        }

        public List<PTXboEditInvoiceAccountDetails> GetEditInvoiceAccountDetails(int invoiceID)
        {
            return _unitOfWork.EditInvoiceRepository.GetEditInvoiceAccountDetails(invoiceID);
        }

        public PTXboEditInvoiceDetails GetEditInvoiceDetailsSave(int invoiceID, int invoiceAdjustmentRequestID)
        {
            return _unitOfWork.EditInvoiceRepository.GetEditInvoiceDetailsSave(invoiceID,invoiceAdjustmentRequestID);
        }
    }
}
