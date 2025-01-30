﻿using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public interface IInvoicePendingRepository
    {
        List<PTXboPendingInvoices> GetPendingInvoiceDetails(PTXboPendingInvoiceSearchCriteria searchCriteria);
        List<PTXboPendingInvoices> GetInvoicePendingAccountDetails(PTXboPendingInvoiceInput pendingInvoiceInput);
        PTXboHearingResult GetHearingResult(int hearingResultID);

        bool InsertInvoiceDataAccountLevel(PTXboInvoice objInvoiceFromHearingResult);
    }
}
