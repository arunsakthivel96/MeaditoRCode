using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using Spartaxx.DataAccess;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
   public class InvoiceService:IInvoiceService
    {
        private readonly UnitOfWork _unitOfWork = null;
        public InvoiceService()
        {
            _unitOfWork = new UnitOfWork();

        }

        /// <summary>
        /// Get doucuments from invoice table which are ready for invoicing
        /// </summary>
        /// <param name="objSearchCriteria"></param>
        /// <param name="lstInvoiceDetails"></param>
        /// <param name="errorstring"></param>
        /// <returns></returns>
        public List<PTXboInvoiceDetails> GetInvoiceGenerationDetails(PTXboInvoiceSearchCriteria objSearchCriteria, bool? isInvoiceDefects = null)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceGenerationDetails(objSearchCriteria, isInvoiceDefects);
        }

        /// <summary>
        /// select a specific invoice by invoiceid
        /// </summary>
        /// <param name="InvoiceID"></param>
        /// <returns></returns>
        public PTXboInvoice GetSpecificInvoiceGenerationDetails(int invoiceID)
        {
            return _unitOfWork.InvoiceRepository.GetSpecificInvoiceGenerationDetails(invoiceID);
        }
        /// <summary>
        /// Description: Common function to get specified list to bind in dropdowns
        /// </summary>

        public bool GetCommonListDefinitions(ref PXTboListDefinition objListDefinition)
        {
            return _unitOfWork.InvoiceRepository.GetCommonListDefinitions(ref objListDefinition);
        }

        public List<PTXboClientRemarks> GetClientRemarks(int clientID)
        {
            return _unitOfWork.InvoiceRepository.GetClientRemarks(clientID);
        }

        public int GetServicePackageIDforInvoice(string ServicePackageName)
        {
            return _unitOfWork.InvoiceRepository.GetServicePackageIDforInvoice(ServicePackageName);
        }

        public string GetLatestInvoiceFile(int invoiceId)
        {
            return _unitOfWork.InvoiceRepository.GetLatestInvoiceFile(invoiceId);
        }

        public PTXboInvoiceAdjustmentReportDetails GetInvoiceAdjustmentReportDetails(int invoiceId)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceAdjustmentReportDetails(invoiceId);
        }

        public PTXboAutoAdjustmentInvoiceDetails GetAutoAdjustmentInvoiceDetails(int invoiceId)
        {
            return _unitOfWork.InvoiceRepository.GetAutoAdjustmentInvoiceDetails(invoiceId);
        }

        public bool SubmitInvoiceFiles(int invoiceID, int corrQID, string fileLocation, int userID)
        {
            return _unitOfWork.InvoiceRepository.SubmitInvoiceFiles(invoiceID,corrQID,fileLocation,userID);
        }

        public bool UpdateInvoiceFiles(int invoiceID, int corrQID, string fileLocation, int userID)
        {
            return _unitOfWork.InvoiceRepository.SubmitInvoiceFiles(invoiceID, corrQID, fileLocation, userID);
        }

        public string GetParamValue(int paramID)
        {
            return _unitOfWork.InvoiceRepository.GetParamValue(paramID);
        }

        public PTXboCADNavigatorDetails GetCADNavigatorDetails(string accountNumber, string countyName)
        {
            return _unitOfWork.InvoiceRepository.GetCADNavigatorDetails(accountNumber, countyName);
        }

        public PTXboClientDeliveryMethodId GetClientDefaultDeliveryMethod(int clientId)
        {
            return _unitOfWork.InvoiceRepository.GetClientDefaultDeliveryMethod(clientId);
        }

        public List<PTXboInvoice> GetInvoiceGenerationInputData(int clientID, int taxYear, int invoiceTypeId, bool outOfTexas,bool IsDisasterInvoice=false)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceGenerationInputData(clientID, taxYear, invoiceTypeId, outOfTexas, IsDisasterInvoice);
        }

        public List<PTXboInvoice> GetCreditInvoiceGenerationInputData(int Invoiceid)
        {
            return _unitOfWork.InvoiceRepository.GetCreditInvoiceGenerationInputData(Invoiceid);
        }

        public bool UpdateClientCorrQueueRecordInvoice(List<PTXboCorrQueue> objCorrQueueList, int groupId)
        {
            return _unitOfWork.InvoiceRepository.UpdateClientCorrQueueRecordInvoice(objCorrQueueList,groupId);
        }

        public PTXboCorrQueue InsertUpdateCorrQCreditInvoice(List<PTXboCorrQueue> objCorrQueueList)
        {
            return _unitOfWork.InvoiceRepository.InsertUpdateCorrQCreditInvoice(objCorrQueueList);
        }

        
        public PTXboInvoiceReportDetails GetInvoiceReportDetails(PTXboInvoiceReportDetailsInput invoiceReportDetailsInput)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceReportDetails(invoiceReportDetailsInput);
        }
        //public PTXboInvoiceReportDetails GetInvoiceReportDetails(int invoiceID, int invoiceTypeId, bool? isInvoiceDefect, bool isOutOfTexas, bool IsOTEntryscreen)
        //{
        //    return _unitOfWork.InvoiceRepository.GetInvoiceReportDetails(invoiceID,invoiceTypeId,isInvoiceDefect, isOutOfTexas, IsOTEntryscreen);
        //}

        public bool AccountLevelInvoiceGeneration(List<PTXboInvoiceReport> getlstInvoiceDetails, List<PTXboInvoiceAccount> lstAccountDetails, int updatedBy)
        {
            return _unitOfWork.InvoiceRepository.AccountLevelInvoiceGeneration(getlstInvoiceDetails,lstAccountDetails, updatedBy);
        }

        public bool ProjectorTermInvoiceGeneration(List<PTXboInvoiceReport> getlstInvoiceDetails, List<PTXboInvoiceAccount> lstAccountDetails, int updatedBy)
        {
            return _unitOfWork.InvoiceRepository.ProjectorTermInvoiceGeneration(getlstInvoiceDetails, lstAccountDetails, updatedBy);
        }

        public bool UpdateInvoiceProcessingStatus(int invoicingProcessingStatusID, PTXboInvoiceDetails invoiceDetails)
        {
            return _unitOfWork.InvoiceRepository.UpdateInvoiceProcessingStatus(invoicingProcessingStatusID, invoiceDetails);
        }
        public bool SubmitInvoiceSummary(PTXboInvoiceSummary invoiceSummary)
        {
            return _unitOfWork.InvoiceRepository.SubmitInvoiceSummary(invoiceSummary);
        }

        public PTXboInvoice GetInvoiceDetailsById(int invoiceId)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceDetailsById(invoiceId);
        }
        public PTXboInvoiceLineItem GetInvoiceLineItemByInvoiceId(int invoiceId)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceLineItemByInvoiceId(invoiceId);
        }

        public bool UpdateClientCorrQueueRecord(List<PTXboCorrQueue> objCorrQueueList, PTXboInvoice invoice)
        {
            return _unitOfWork.InvoiceRepository.UpdateClientCorrQueueRecord(objCorrQueueList, invoice);
        }

        public PTXboAccountLevelInvoiceOutput InsertAccountLevelInvoiceGeneration(PTXboInvoice objInvoice, List<PTXboInvoice> lstInvoiceDetails, string hearingType, string invoicingGroupType, int createdBy)
        {
            return _unitOfWork.InvoiceRepository.InsertAccountLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, createdBy);
        }

        public bool UpdateLetterProcessStatus(PTXboUpdateLetterProcessStatusInput letterProcessStatus)
        {
            return _unitOfWork.InvoiceRepository.UpdateLetterProcessStatus(letterProcessStatus);
        }

        public List<PTXboCorrQueue> GetActiveCorrQueueRecordsForSelectedInvoices(PTXboActiveCorrqRecordsInput activeCorrqRecords)
        {
            return _unitOfWork.InvoiceRepository.GetActiveCorrQueueRecordsForSelectedInvoices(activeCorrqRecords);
        }

        public int GetInvoiceIdFromInvoiceAdjustment(int invoiceAdjustMentRequestId)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceIdFromInvoiceAdjustment(invoiceAdjustMentRequestId);
        }

        public bool SaveOrUpdateInvoice(PTXboInvoice invoice, out int invoiceID)
        {
            invoiceID = 0;
            return _unitOfWork.InvoiceRepository.SaveOrUpdateInvoice(invoice,out invoiceID);
        }


        public PTXboClientEmail GetClientEmailInvoice(PTXboGetClientEmailInput objGetClientEmail)
        {
            return _unitOfWork.InvoiceRepository.GetClientEmailInvoice(objGetClientEmail);
        }

        public List<PTXboInvoiceBasicDetails> GetInvoiceBasicDetails(int invoiceID)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceBasicDetails(invoiceID);
        }

        public bool GetInvoiceTermsType(PTXboInvoice objInvoice)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceTermsType(objInvoice);
        }

        public PTXboInvoiceReportOutput GenerateInvoicereports(PTXboInvoiceReportInput objInvoiceReportInput)
        {
            return _unitOfWork.InvoiceRepository.GenerateInvoicereports(objInvoiceReportInput);
        }

        public bool UpdateInvoiceDetails(PTXboInvoice objInvoice)
        {
            return _unitOfWork.InvoiceRepository.UpdateInvoiceDetails(objInvoice);
        }

        public List<PTXboCorrQueue> GetFailedCorrQueueRecords()
        {
            return _unitOfWork.InvoiceRepository.GetFailedCorrQueueRecords();
        }
        public bool CheckInvoicefilepath(string fileLocation)
        {
            return _unitOfWork.InvoiceRepository.CheckInvoicefilepath(fileLocation);
        }

        public bool SubmitOutOfTexasInvoice(PTXboOutOfTexasInvoiceDetails outOfTexasInvoice)
        {
            return _unitOfWork.InvoiceRepository.SubmitOutOfTexasInvoice(outOfTexasInvoice);
        }


        public PTXboOutOfTexasInvoiceDetails SubmitOutOfTexasProjectLevelInvoice(PTXboOutOfTexasInvoiceDetails outOfTexasInvoice)
        {
            return _unitOfWork.InvoiceRepository.SubmitOutOfTexasProjectLevelInvoice(outOfTexasInvoice);
        }


        public PTXboInvoiceReportOutput GenerateInvoiceUsmailCoverLetter(PTXboInvoiceReportInput objInvoiceReportInput)
        {
            return _unitOfWork.InvoiceRepository.GenerateInvoiceUsmailCoverLetter(objInvoiceReportInput);
        }

        /// <summary>
        /// //Added by SaravananS. tfs id:61434
        /// </summary>
        /// <param name="servicePakageId"></param>
        /// <returns></returns>
        public PTXboClientCommunicationEmailDetails GetClientCommunicationEmailDetails(int servicePakageId)
        {
            return _unitOfWork.InvoiceRepository.GetClientCommunicationEmailDetails(servicePakageId);
        }

        /// <summary>
        /// //Added by SaravananS. tfs id:61434
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public PTXboInvoiceRegenerationDetails GetInvoiceRegenerationDetails(int invoiceId)
        {
            return _unitOfWork.InvoiceRepository.GetInvoiceRegenerationDetails(invoiceId);
        }


        public bool IsOutOfTexas(int invoiceId)
        {
            return _unitOfWork.InvoiceRepository.IsOutOfTexas(invoiceId);
        }

        public bool IsMultiyear(int invoiceId)
        {
            return _unitOfWork.InvoiceRepository.IsMultiyear(invoiceId);
        }

        public bool GenerateLetterForSelectedInvoices(PTXboActiveCorrqRecordsInput corrqRecordsInput)//Added by SaravananS. tfs id:62016
        {
            return _unitOfWork.InvoiceRepository.GenerateLetterForSelectedInvoices(corrqRecordsInput);
        }


        /// <summary>
        /// //Added by SaravananS. tfs id:63159
        /// </summary>
        /// <param name="objGetClientEmail"></param>
        /// <returns></returns>
        public PTXboClientEmail GetClientDeliveryAddressForInvoice(PTXboGetClientEmailInput objGetClientEmail)
        {
            return _unitOfWork.InvoiceRepository.GetClientDeliveryAddressForInvoice(objGetClientEmail);
        }

    }
}
