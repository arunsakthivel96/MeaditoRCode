using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    /// <summary>
    /// added by saravanans-tfs:47247
    /// </summary>
   public class InvoiceDefectsService:IInvoiceDefectsService
    {
        private readonly UnitOfWork _unitOfWork = null;
        public InvoiceDefectsService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public bool UpdateInvoiceStatusinMainTable(PTXboUpdateInvoiceMaintableInput maintableInput)
        {
            return _unitOfWork.InvoiceDefectsRepository.UpdateInvoiceStatusinMainTable(maintableInput.InvoiceID, maintableInput.InvoiceTypeId, maintableInput.IsSpecialTerm);
        }

        public bool SubmitInvoiceGenerationDefects(List<PTXboInvoiceDetails> lstInvoiceDetails)
        {
            return _unitOfWork.InvoiceDefectsRepository.SubmitInvoiceGenerationDefects(lstInvoiceDetails);
        }

        public List<PTXboInvoiceDetails> GetInvoiceGenerationDefects(PTXboInvoiceSearchCriteria objSearchCriteria)
        {
            return _unitOfWork.InvoiceDefectsRepository.GetInvoiceGenerationDefects(objSearchCriteria);
        }

        public List<PTXboAccountDetails> GetInvoiceDefectsAccountDetails(int invoiceId)
        {
            return _unitOfWork.InvoiceDefectsRepository.GetInvoiceDefectsAccountDetails(invoiceId);
        }

        public List<PTXboAccountJurisdiction> GetInvoiceDefectsAccountJurisdictionDetails(PTXboInvoiceDefectJurisdictionInput invoiceDefectJurisdiction)
        {
            return _unitOfWork.InvoiceDefectsRepository.GetInvoiceDefectsAccountJurisdictionDetails(invoiceDefectJurisdiction);
        }
        public PTXboInvoiceDefectDetails GetInvoiceDefectsTermDetails(int invoiceId)
        {
            return _unitOfWork.InvoiceDefectsRepository.GetInvoiceDefectsTermDetails(invoiceId);
        }

        public bool UpdateInvoiceDefectsTermsDetails(PTXboInvoiceDefectDetails invoiceDefectDetails)
        {
            return _unitOfWork.InvoiceDefectsRepository.UpdateInvoiceDefectsTermsDetails(invoiceDefectDetails);
        }
    }
}
