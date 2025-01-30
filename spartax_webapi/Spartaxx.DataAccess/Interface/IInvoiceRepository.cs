using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    /// <summary>
    /// Created by SaravananS....Dapper implementation.
    /// Created date :21/11/2019
    /// </summary>
   public interface IInvoiceRepository
    {
        List<PTXboInvoiceDetails> GetInvoiceGenerationDetails(PTXboInvoiceSearchCriteria objSearchCriteria, bool? isInvoiceDefects = null);
        PTXboInvoice GetSpecificInvoiceGenerationDetails(int invoiceID);
        bool GetCommonListDefinitions(ref PXTboListDefinition objListDefinition);
        List<PTXboClientRemarks> GetClientRemarks(int clientID);
        int GetServicePackageIDforInvoice(string servicePackageName);
        string GetLatestInvoiceFile(int invoiceId);
        PTXboInvoiceAdjustmentReportDetails GetInvoiceAdjustmentReportDetails(int invoiceId);
        PTXboAutoAdjustmentInvoiceDetails GetAutoAdjustmentInvoiceDetails(int invoiceId);
        bool SubmitInvoiceFiles(int invoiceID, int corrQID, string fileLocation, int userID);
        bool UpdateInvoiceFiles(int invoiceID, int corrQID, string fileLocation, int userID);
        string GetParamValue(int paramID);
        PTXboCADNavigatorDetails GetCADNavigatorDetails(string accountNumber, string countyName);
        PTXboClientDeliveryMethodId GetClientDefaultDeliveryMethod(int clientId);
        List<PTXboInvoice> GetInvoiceGenerationInputData(int clientID, int taxYear, int invoiceTypeId, bool outOfTexas = false,bool IsDisasterInvoice=false);
        List<PTXboInvoice> GetCreditInvoiceGenerationInputData(int Invoiceid);
        bool UpdateClientCorrQueueRecordInvoice(List<PTXboCorrQueue> objCorrQueueList, int groupId);
        PTXboCorrQueue InsertUpdateCorrQCreditInvoice(List<PTXboCorrQueue> objCorrQueueList);
       // PTXboInvoiceReportDetails GetInvoiceReportDetails(int invoiceID, int invoiceTypeId, bool? isInvoiceDefect,bool isOutOfTexas, bool IsOTEntryscreen );
        PTXboInvoiceReportDetails GetInvoiceReportDetails(PTXboInvoiceReportDetailsInput invoiceReportDetailsInput);
        bool AccountLevelInvoiceGeneration(List<PTXboInvoiceReport> getlstInvoiceDetails, List<PTXboInvoiceAccount> lstAccountDetails, int updatedBy);
        bool ProjectorTermInvoiceGeneration(List<PTXboInvoiceReport> getlstInvoiceDetails, List<PTXboInvoiceAccount> lstAccountDetails, int updatedBy);
        bool UpdateInvoiceProcessingStatus(int invoicingProcessingStatusID, PTXboInvoiceDetails invoiceDetails);
        bool SubmitInvoiceSummary(PTXboInvoiceSummary invoiceSummary);
        PTXboInvoice GetInvoiceDetailsById(int InvoiceID);
        PTXboInvoiceLineItem GetInvoiceLineItemByInvoiceId(int invoiceID);
        bool UpdateClientCorrQueueRecord(List<PTXboCorrQueue> objCorrQueueList, PTXboInvoice invoice);
        PTXboAccountLevelInvoiceOutput InsertAccountLevelInvoiceGeneration(PTXboInvoice objInvoice, List<PTXboInvoice> lstInvoiceDetails, string hearingType, string invoicingGroupType, int createdBy);
        bool UpdateLetterProcessStatus(PTXboUpdateLetterProcessStatusInput letterProcessStatus);
        List<PTXboCorrQueue> GetActiveCorrQueueRecordsForSelectedInvoices(PTXboActiveCorrqRecordsInput activeCorrqRecords);
        int GetInvoiceIdFromInvoiceAdjustment(int invoiceAdjustMentRequestId);
        bool SaveOrUpdateInvoice(PTXboInvoice invoice, out int invoiceID);
        PTXboClientEmail GetClientEmailInvoice(PTXboGetClientEmailInput objGetClientEmail);
        List<PTXboInvoiceBasicDetails> GetInvoiceBasicDetails(int invoiceID);
        bool GetInvoiceTermsType(PTXboInvoice objInvoice);
        PTXboInvoiceReportOutput GenerateInvoicereports(PTXboInvoiceReportInput objInvoiceReportInput);
        bool UpdateInvoiceDetails(PTXboInvoice objInvoice);

        List<PTXboCorrQueue> GetFailedCorrQueueRecords();

        bool CheckInvoicefilepath(string fileLocation);
        bool SubmitOutOfTexasInvoice(PTXboOutOfTexasInvoiceDetails outOfTexasInvoice);
        PTXboOutOfTexasInvoiceDetails SubmitOutOfTexasProjectLevelInvoice(PTXboOutOfTexasInvoiceDetails outOfTexasInvoice);
        PTXboInvoiceReportOutput GenerateInvoiceUsmailCoverLetter(PTXboInvoiceReportInput objInvoiceReportInput);
        PTXboClientCommunicationEmailDetails GetClientCommunicationEmailDetails(int servicePakageId);//Added by SaravananS. tfs id:61434
        PTXboInvoiceRegenerationDetails GetInvoiceRegenerationDetails(int invoiceId);//Added by SaravananS. tfs id:61434
        bool IsOutOfTexas(int invoiceId);//Added by SaravananS. tfs id:61434
         bool IsMultiyear(int invoiceId);//Added by SaravananS. tfs id:61434
        bool GenerateLetterForSelectedInvoices(PTXboActiveCorrqRecordsInput corrqRecordsInput);//Added by SaravananS. tfs id:62016
        PTXboClientEmail GetClientDeliveryAddressForInvoice(PTXboGetClientEmailInput objGetClientEmail);//Added by SaravananS. tfs id:63159
    }
}
