using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
  public interface IEditInvoiceRepository
    {
        List<PTXboEditInvoiceSearchResult> GetEditInvoiceDetails(PTXboEditInvoiceSearchCriteria SearchCriteria);
        PTXboEditInvoiceAccountDetails GetEditInvoiceClientDetails(int invoiceID);
        List<PTXboCSInvoiceHistory> GetEditInvoicePayments(int invoiceID, int invoiceType);
        List<PTXboEditInvoiceAccountDetails> GetEditInvoiceAccountDetails(int invoiceID);
        PTXboEditInvoiceDetails GetEditInvoiceDetailsSave(int invoiceID, int invoiceAdjustmentRequestID);
    }
}
