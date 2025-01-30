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
    public class InvoiceFlatFeePreService:IInvoiceFlatFeePreService
    {
        private readonly UnitOfWork _unitOfWork = null;
        public InvoiceFlatFeePreService()
        {
            _unitOfWork = new UnitOfWork();
        }

       public List<PTXboInvoiceFlatFeePreAccountDetails> GetInvoiceFlatFeePreAccountDetail(PTXboInvoiceFlatFeeInput invoiceFlatFeeInput)
        {
            return _unitOfWork.InvoiceFlatFeePreRepository.GetInvoiceFlatFeePreAccountDetail(invoiceFlatFeeInput);
        }

        public List<PTXboInvoiceFlatFeePre> GetInvoiceFlatFeePreClientDetail(PTXboFlatFeeClientDetail flatFeeClientDetail)
        {
            return _unitOfWork.InvoiceFlatFeePreRepository.GetInvoiceFlatFeePreClientDetail(flatFeeClientDetail);
        }

        public bool FlatFeeInvoiceGeneration(PTXboFlatFeeInvoiceGenerationInput flatFeeInvoice, out string errorMessage)
        {
            return _unitOfWork.InvoiceFlatFeePreRepository.FlatFeeInvoiceGeneration(flatFeeInvoice,out errorMessage);
        }
    }
}
