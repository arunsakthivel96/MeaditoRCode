using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
   public interface IInvoiceDefectsRepository
    {
        bool UpdateInvoiceStatusinMainTable(int invoiceID, int invoiceTypeId, bool isSpecialTerm = false);
        bool SubmitInvoiceGenerationDefects(List<PTXboInvoiceDetails> lstInvoiceDetails);
        List<PTXboInvoiceDetails> GetInvoiceGenerationDefects(PTXboInvoiceSearchCriteria objSearchCriteria);
        List<PTXboAccountDetails> GetInvoiceDefectsAccountDetails(int invoiceId);
        List<PTXboAccountJurisdiction> GetInvoiceDefectsAccountJurisdictionDetails(PTXboInvoiceDefectJurisdictionInput invoiceDefectJurisdiction);
        PTXboInvoiceDefectDetails GetInvoiceDefectsTermDetails(int invoiceId);
        bool UpdateInvoiceDefectsTermsDetails(PTXboInvoiceDefectDetails invoiceDefectDetails);
    }
}
