using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
   public interface IInvoiceAdjustmentAuditRepository
    {
        List<PTXboInvoiceAdjustmentAllotted> GetAllottedInvoiceAdjustmentRequest(PTXboInvoiceAdjustmentAllotedSearch objSearch);
        bool GetInvoiceNextAdjustmentAllottedDocument(int userID, int userRoleID, int queID);
        PTXboInvoiceAndClientDetails GetClientAndInvoiceDetails(int invoiceID);
        PTXboInvoiceAdjustmentRequest GetInvoiceAdjustmentRequestDetails(int invoiceAdjusmentRequestID, int requestType);
        PTXboPendingAdjustmentRequestDetails GetPendingAdjustmentRequestDetails(int invoiceID);
        PTXboAutoAdjustmentInvoiceDetails GetAutoAdjustmentInvoiceDetails(int invoiceID, int invoiceAdjustmentRequestID);
        List<PTXboCollectionRemark> GetCSInvoiceComments(int invoiceID);
        List<PTXboExemptionJurisdictions> GetExemptionRemovedJurisdictions(int invoiceID, int taxYear, int invoiceAdjustmentRequestID, int propertyDetails);
        PTXboInvoiceAdjustmentLineItem GetInvoiceAdjustmentLineItem(int invoiceAdjustmentId, int accountID);
        int SaveOrUpdateInvoiceAdjustmentLineItem(PTXboInvoiceAdjustmentLineItem invoiceAdjustmentLineItem);
        int SaveOrUpdateInvoiceAdjustmentManualLineItem(PTXboInvoiceAdjustmentManualLineItem invoiceAdjustmentManualLineItem);
        PTXboInvoiceAdjustmentManualLineItem GetInvoiceAdjustmentManualLineItem(int invoiceAdjustmentManualLineItemID);
        PTXboInvoiceExemptionJurisdiction GetInvoiceExemptionJurisdiction(int invoiceAdjustmentId, int jurisdictionId);
        bool SaveOrUpdateInvoiceExemptionJurisdiction(PTXboInvoiceExemptionJurisdiction ObjAdjLineItemFromDB);
        bool SaveOrUpdateInvoiceExemptionJurisdictionhistory(PTXboInvoiceExemptionJurisdictionHistory ObjAdjLineItemFromDB);
        int SaveOrUpdateInvoiceAdjustment(PTXboInvoiceAdjustment invoiceadjustment);
        int SaveOrUpdateInvoiceAdjustmentAttachments(PTXboInvoiceAdjustmentAttachments objInvoiceAttachments);
        int SaveOrUpdateInvoiceAdjustmentRequest(PTXboInvoiceAdjustmentRequest objInvoiceAdjustmentRequest);
        List<PTXboExemptionAccounts> GetExemptionAccountlist(int invoiceID, int invoiceAdjustmentRequestID);
        List<PTXboAdjustmentHistory> GetInvoiceAdjustmentHistory(int invoiceID);
    }
}
