using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.ViewModels;
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
  public  interface IMainscreenInvoiceService
    {
        PTXvmMainScreenInvoiceDetail MainScreenInvoiceStatusCollection(int clientID);
        List<PTXboCSInvoiceComments> GetInvoiceComments(PTXboInvoiceCommentsInput invoiceComments);
        bool SaveCollectionComments(PTXboInvoiceRemarks objInvoiceRemarks);
        bool SaveorUpdateInvoiceRemarks(PTXboInvoiceRemarks objboInvoiceRemarks);
        List<PTXboCSInvoiceComments> DeleteCSInvoiceComments(PTXboDeletecsInvoiceCommentsInput commentsInput);
        List<PTXboCSInvoiceHistory> GetCSInvoiceTrasactions(PTXboInvoiceCommentsInput invoiceComments);
        List<PTXboCSInvoiceDetails> GetInvoicecollectionDetails(PTXboInvoiceFilter gridvalues);
        PTXboInvoiceFilter GetInvoicecounts(int clientid);
        string GetUserMessage(string userMessageCode);
        List<PTXboPastDueNotices> GetPastDueNotices(int invoiceId);
        List<PTXboFlagCollectionDetails> GetFlagCollectionDetails(int clientId);
        List<PTXboInvoice> GetCCPaymentInvoiceDetails(int ccpaymentId);
        bool SaveManualPaymentPosting(PTXboManualPaymentPostingInput manualPaymentPosting);
        bool SaveCreditManualPaymentPosting(PTXboManualCreditPaymentPostingInput creditPaymentPosting);
        bool CheckLegalInvoice(string invoiceIds);
        bool UpdatePaymentOrSettlementPlan(PTXboPaymentSettlementPlanInput paymentSettlement);
        bool LegalInvoiceGeneration(PTXboLegalInvoiceGenerationInput legalInvoice);
        List<PTXboCSInvoiceHistory> UpdateCSInvoiceTransaction(PTXboUpdateCSInvoiceTranscationInput objUpdateCSInvoice);
        decimal WaiveCompoundInterest(string invoiceId);
        decimal? CheckCompoundInterestAvailability(string invoiceId);
        decimal? WaiveSimpleInterest(string invoiceId);
        bool UpdateSettlementDetails(PTXboUpdateSettlementDetailsInput objUpdateSettlement);
        PTXboAgentCollectionSummary GetCollectionDetailsSummary(int userId);
        List<PTXboCCPayInvoiceList> GetCCPaymentInvoiceListDetails(int clientID);
        PTXboCCPayment GetCCPaymentDetailsForInvoice(int ccpaymentID);
        PTXboCCPaymentStatus SaveCCPaymentDetails(PTXboSaveCCPaymentsInput objSaveCCPayments);
        string RemoveCCPaymentDetails(PTXboCCPayment objCCPayment);
        bool InsertInvoiceData(PTXboInvoice objInvoiceFromHearingResult);
         int? GetInvoiceCheckPaymentStatus(int invoiceId);
        bool WaiveInvoiceInterest(List<PTXboPayment> paymentList);
        string DownloadCCPaymentReceipt(int ccPaymentId); //Created by SaravananS. tfs id:61797
    }
}
