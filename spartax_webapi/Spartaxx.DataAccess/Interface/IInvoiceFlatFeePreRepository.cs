using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    /// <summary>
    /// added by saravanans-tfs:47247
    /// </summary>
    public interface IInvoiceFlatFeePreRepository
    {
        List<PTXboInvoiceFlatFeePreAccountDetails> GetInvoiceFlatFeePreAccountDetail(PTXboInvoiceFlatFeeInput invoiceFlatFeeInput);
        List<PTXboInvoiceFlatFeePre> GetInvoiceFlatFeePreClientDetail(PTXboFlatFeeClientDetail flatFeeClientDetail);
        bool FlatFeeInvoiceGeneration(PTXboFlatFeeInvoiceGenerationInput flatFeeInvoice, out string errorMessage);
    }
}
