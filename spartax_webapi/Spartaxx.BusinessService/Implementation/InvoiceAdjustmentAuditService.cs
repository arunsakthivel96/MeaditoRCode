using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
   public class InvoiceAdjustmentAuditService:IInvoiceAdjustmentAuditService
    {
        private readonly UnitOfWork _unitOfWork=null;
        public InvoiceAdjustmentAuditService()
        {
            _unitOfWork =new UnitOfWork();
        }

        public List<PTXboInvoiceAdjustmentAllotted> GetAllottedInvoiceAdjustmentRequest(PTXboInvoiceAdjustmentAllotedSearch objSearch)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetAllottedInvoiceAdjustmentRequest(objSearch);
        }

        public bool GetInvoiceNextAdjustmentAllottedDocument(int userID, int userRoleID, int queID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetInvoiceNextAdjustmentAllottedDocument(userID, userRoleID, queID);
        }

        public PTXboInvoiceAndClientDetails GetClientAndInvoiceDetails(int invoiceID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetClientAndInvoiceDetails(invoiceID);
        }

        public PTXboInvoiceAdjustmentRequest GetInvoiceAdjustmentRequestDetails(int invoiceAdjusmentRequestID, int requestType)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetInvoiceAdjustmentRequestDetails(invoiceAdjusmentRequestID, requestType);
        }

        public  PTXboPendingAdjustmentRequestDetails GetPendingAdjustmentRequestDetails(int invoiceID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetPendingAdjustmentRequestDetails(invoiceID);
        }

        public PTXboAutoAdjustmentInvoiceDetails GetAutoAdjustmentInvoiceDetails(int invoiceID, int invoiceAdjustmentRequestID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetAutoAdjustmentInvoiceDetails(invoiceID, invoiceAdjustmentRequestID);
        }

        public List<PTXboCollectionRemark> GetCSInvoiceComments(int invoiceID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetCSInvoiceComments(invoiceID);
        }

        public List<PTXboExemptionJurisdictions> GetExemptionRemovedJurisdictions(int invoiceID, int taxYear, int invoiceAdjustmentRequestID, int propertyDetails)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetExemptionRemovedJurisdictions(invoiceID, taxYear, invoiceAdjustmentRequestID, propertyDetails);
        }

        public PTXboInvoiceAdjustmentLineItem GetInvoiceAdjustmentLineItem(int invoiceAdjustmentId, int accountID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetInvoiceAdjustmentLineItem(invoiceAdjustmentId,accountID);
        }

        public int SaveOrUpdateInvoiceAdjustmentLineItem(PTXboInvoiceAdjustmentLineItem invoiceAdjustmentLineItem)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.SaveOrUpdateInvoiceAdjustmentLineItem(invoiceAdjustmentLineItem);
        }

        public int SaveOrUpdateInvoiceAdjustmentManualLineItem(PTXboInvoiceAdjustmentManualLineItem invoiceAdjustmentManualLineItem)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.SaveOrUpdateInvoiceAdjustmentManualLineItem(invoiceAdjustmentManualLineItem);
        }

        public PTXboInvoiceAdjustmentManualLineItem GetInvoiceAdjustmentManualLineItem(int invoiceAdjustmentManualLineItemID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetInvoiceAdjustmentManualLineItem(invoiceAdjustmentManualLineItemID);
        }

        public PTXboInvoiceExemptionJurisdiction GetInvoiceExemptionJurisdiction(int invoiceAdjustmentId, int jurisdictionId)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetInvoiceExemptionJurisdiction(invoiceAdjustmentId,jurisdictionId);
        }

        public bool SaveOrUpdateInvoiceExemptionJurisdiction(PTXboInvoiceExemptionJurisdiction ObjAdjLineItemFromDB)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.SaveOrUpdateInvoiceExemptionJurisdiction(ObjAdjLineItemFromDB);
        }
        public bool SaveOrUpdateInvoiceExemptionJurisdictionhistory(PTXboInvoiceExemptionJurisdictionHistory ObjAdjLineItemFromDB)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.SaveOrUpdateInvoiceExemptionJurisdictionhistory(ObjAdjLineItemFromDB);
        }
        public int SaveOrUpdateInvoiceAdjustment(PTXboInvoiceAdjustment invoiceadjustment)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.SaveOrUpdateInvoiceAdjustment(invoiceadjustment);
        }
        public int SaveOrUpdateInvoiceAdjustmentAttachments(PTXboInvoiceAdjustmentAttachments objInvoiceAttachments)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.SaveOrUpdateInvoiceAdjustmentAttachments(objInvoiceAttachments);
        }

        public int SaveOrUpdateInvoiceAdjustmentRequest(PTXboInvoiceAdjustmentRequest objInvoiceAdjustmentRequest)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.SaveOrUpdateInvoiceAdjustmentRequest(objInvoiceAdjustmentRequest);
        }

        public List<PTXboExemptionAccounts> GetExemptionAccountlist(int invoiceID, int invoiceAdjustmentRequestID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetExemptionAccountlist(invoiceID, invoiceAdjustmentRequestID);
        }

        public List<PTXboAdjustmentHistory> GetInvoiceAdjustmentHistory(int invoiceID)
        {
            return _unitOfWork.InvoiceAdjustmentAuditRepository.GetInvoiceAdjustmentHistory(invoiceID);
        }
    }
}
