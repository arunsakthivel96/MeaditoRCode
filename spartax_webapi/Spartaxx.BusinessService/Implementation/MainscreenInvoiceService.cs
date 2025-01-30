using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.ViewModels;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    /// <summary>
    /// added by saravanans -tfs:47247
    /// </summary>
    public class MainscreenInvoiceService:IMainscreenInvoiceService
    {
        private readonly UnitOfWork _unitOfWork;
        public MainscreenInvoiceService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public PTXvmMainScreenInvoiceDetail MainScreenInvoiceStatusCollection(int clientID)
        {
            return _unitOfWork.MainscreenInvoiceRepository.MainScreenInvoiceStatusCollection(clientID);
        }
        public List<PTXboCSInvoiceComments> GetInvoiceComments(PTXboInvoiceCommentsInput invoiceComments)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetInvoiceComments(invoiceComments);
        }

        public bool SaveCollectionComments(PTXboInvoiceRemarks objInvoiceRemarks)
        {
            return _unitOfWork.MainscreenInvoiceRepository.SaveCollectionComments(objInvoiceRemarks);
        }

        public bool SaveorUpdateInvoiceRemarks(PTXboInvoiceRemarks objboInvoiceRemarks)
        {
            return _unitOfWork.MainscreenInvoiceRepository.SaveorUpdateInvoiceRemarks(objboInvoiceRemarks);
        }

        public List<PTXboCSInvoiceComments> DeleteCSInvoiceComments(PTXboDeletecsInvoiceCommentsInput commentsInput)
        {
            return _unitOfWork.MainscreenInvoiceRepository.DeleteCSInvoiceComments(commentsInput);
        }


        public List<PTXboCSInvoiceHistory> GetCSInvoiceTrasactions(PTXboInvoiceCommentsInput invoiceComments)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetCSInvoiceTrasactions(invoiceComments);
        }

        public List<PTXboCSInvoiceDetails> GetInvoicecollectionDetails(PTXboInvoiceFilter gridvalues)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetInvoicecollectionDetails(gridvalues);
        }

        public PTXboInvoiceFilter GetInvoicecounts(int clientid)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetInvoicecounts(clientid);
        }
        public string GetUserMessage(string userMessageCode)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetUserMessage(userMessageCode);
        }
        public List<PTXboPastDueNotices> GetPastDueNotices(int invoiceId)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetPastDueNotices(invoiceId);
        }

        public List<PTXboFlagCollectionDetails> GetFlagCollectionDetails(int clientId)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetFlagCollectionDetails(clientId);
        }
        public List<PTXboInvoice> GetCCPaymentInvoiceDetails(int ccpaymentId)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetCCPaymentInvoiceDetails(ccpaymentId);
        }

        public bool SaveManualPaymentPosting(PTXboManualPaymentPostingInput manualPaymentPosting)
        {
            return _unitOfWork.MainscreenInvoiceRepository.SaveManualPaymentPosting(manualPaymentPosting);
        }
        public bool SaveCreditManualPaymentPosting(PTXboManualCreditPaymentPostingInput creditPaymentPosting)
        {
            return _unitOfWork.MainscreenInvoiceRepository.SaveCreditManualPaymentPosting(creditPaymentPosting);
        }
        public bool CheckLegalInvoice(string invoiceIds)
        {
            return _unitOfWork.MainscreenInvoiceRepository.CheckLegalInvoice(invoiceIds);
        }

        public bool UpdatePaymentOrSettlementPlan(PTXboPaymentSettlementPlanInput paymentSettlement)
        {
            return _unitOfWork.MainscreenInvoiceRepository.UpdatePaymentOrSettlementPlan(paymentSettlement);
        }
        public bool LegalInvoiceGeneration(PTXboLegalInvoiceGenerationInput legalInvoice)
        {
            return _unitOfWork.MainscreenInvoiceRepository.LegalInvoiceGeneration(legalInvoice);
        }
        public List<PTXboCSInvoiceHistory> UpdateCSInvoiceTransaction(PTXboUpdateCSInvoiceTranscationInput objUpdateCSInvoice)
        {
            return _unitOfWork.MainscreenInvoiceRepository.UpdateCSInvoiceTransaction(objUpdateCSInvoice);
        }
        public decimal WaiveCompoundInterest(string invoiceId)
        {
            return _unitOfWork.MainscreenInvoiceRepository.WaiveCompoundInterest(invoiceId);
        }

        public decimal? CheckCompoundInterestAvailability(string invoiceId)
        {
            return _unitOfWork.MainscreenInvoiceRepository.CheckCompoundInterestAvailability(invoiceId);
        }

        public decimal? WaiveSimpleInterest(string invoiceId)
        {
            return _unitOfWork.MainscreenInvoiceRepository.WaiveSimpleInterest(invoiceId);
        }

        public bool UpdateSettlementDetails(PTXboUpdateSettlementDetailsInput objUpdateSettlement)
        {
            return _unitOfWork.MainscreenInvoiceRepository.UpdateSettlementDetails(objUpdateSettlement);
        }

        public PTXboAgentCollectionSummary GetCollectionDetailsSummary(int userId)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetCollectionDetailsSummary(userId);
        }


        public List<PTXboCCPayInvoiceList> GetCCPaymentInvoiceListDetails(int clientID)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetCCPaymentInvoiceListDetails(clientID);
        }

        public PTXboCCPayment GetCCPaymentDetailsForInvoice(int ccpaymentID)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetCCPaymentDetailsForInvoice(ccpaymentID);
        }
        public PTXboCCPaymentStatus SaveCCPaymentDetails(PTXboSaveCCPaymentsInput objSaveCCPayments)
        {
            return _unitOfWork.MainscreenInvoiceRepository.SaveCCPaymentDetails(objSaveCCPayments);
        }

        public string RemoveCCPaymentDetails(PTXboCCPayment objCCPayment)
        {
            return _unitOfWork.MainscreenInvoiceRepository.RemoveCCPaymentDetails(objCCPayment);
        }

        public bool InsertInvoiceData(PTXboInvoice objInvoiceFromHearingResult)
        {
            return _unitOfWork.MainscreenInvoiceRepository.InsertInvoiceData(objInvoiceFromHearingResult);
        }

        public int? GetInvoiceCheckPaymentStatus(int invoiceId)
        {
            return _unitOfWork.MainscreenInvoiceRepository.GetInvoiceCheckPaymentStatus(invoiceId);
        }

        public bool WaiveInvoiceInterest(List<PTXboPayment> paymentList)
        {
            return _unitOfWork.MainscreenInvoiceRepository.WaiveInvoiceInterest(paymentList);
        }

       public string DownloadCCPaymentReceipt(int ccPaymentId) //Created by SaravananS. tfs id:61797
        {
            return _unitOfWork.MainscreenInvoiceRepository.DownloadCCPaymentReceipt(ccPaymentId);
        }
    }
}
