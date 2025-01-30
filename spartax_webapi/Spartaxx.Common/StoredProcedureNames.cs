using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common
{
    public static class StoredProcedureNames
    {
        #region Common
        public const string usp_getPWDocumentTypesByFileCabinetID = "usp_getPWDocumentTypes";
        public const string usp_getIndexedNoticeDocumentRouteFrom = "usp_getIndexedNoticeDocumentRouteFrom";
        public const string usp_getCountyURL = "usp_getCountyURL";        
        public const string usp_get_AllReqListDefinitions = "usp_get_AllReqListDefinitions";
        public const string usp_GET_ParamValue = "usp_GET_ParamValue";
        #endregion

        #region Task Allocation
        public const string usp_GetDataEntryHearingNoticeForManualAllocation = "usp_GetDataEntryHearingNoticeForManualAllocation";
        public const string usp_GetDataEntryHearingNoticeForManualAllocationAPI = "usp_GetDataEntryHearingNoticeForManualAllocationAPI"; //New
        public const string usp_GetClientSetupDetailsForManualAllocation = "usp_GetClientSetupDetailsForManualAllocation";
        public const string USP_UpdateHearingNoticeTaskAllocation = "USP_UpdateHearingNoticeTaskAllocation";    //New
        public const string USP_UpdateClientSetupTaskAllocation = "USP_UpdateClientSetupTaskAllocation";    //New
        public const string usp_GetDataEntryPropertySurveyForManualAllocation = "usp_GetDataEntryPropertySurveyForManualAllocation"; 
        public const string usp_GetDataEntryAffidavitForManualAllocation = "usp_GetDataEntryAffidavitForManualAllocation";
        #endregion

        #region Client Search
        public const string usp_SPARTAXX_SEL_getSearchedAccountDetails = "usp_SPARTAXX_SEL_getSearchedAccountDetails";
        public const string usp_SPARTAXX_SEL_getSearchedAccountDetails_address = "usp_SPARTAXX_SEL_getSearchedAccountDetails_address";
        public const string usp_SPARTAXX_SEL_getSearchedAccountDetails_propertyaddress = "usp_SPARTAXX_SEL_getSearchedAccountDetails_propertyaddress";
        public const string usp_SPARTAXX_SEL_getSearchedAccountDetails_clientaddress = "usp_SPARTAXX_SEL_getSearchedAccountDetails_clientaddress";
        public const string usp_SPARTAXX_SEL_getSearchedClientDetails = "usp_SPARTAXX_SEL_getSearchedClientDetails_010422";
        public const string usp_SPARTAXX_SEL_getSearchedClientDetails_address = "usp_SPARTAXX_SEL_getSearchedClientDetails_address";
        public const string usp_SPARTAXX_SEL_getSearchedClientDetails_propertyaddress = "usp_SPARTAXX_SEL_getSearchedClientDetails_propertyaddress";
        public const string usp_SPARTAXX_SEL_getSearchedClientDetails_clientaddress = "usp_SPARTAXX_SEL_getSearchedClientDetails_clientaddress";
        public const string usp_getSearchedClientDetails = "usp_getSearchedClientDetails";
        #endregion

        #region TaxBill Audit
        //public const string usp_getTaxBillAuditClients = "usp_getTaxBillAuditClients";
        //public const string usp_getTaxBillAuditJurisdictionGridInformation = "usp_getTaxBillAuditJurisdictionGridInformation";
        //public const string usp_getTaxBillAuditJurisdictionGridInformationforAudit = "usp_getTaxBillAuditJurisdictionGridInformationforAudit";
        //public const string usp_getAccountDetailsForPopup = "usp_getAccountDetailsForPopup";
        //public const string usp_getJurisdictionDetailsForPopup = "usp_getJurisdictionDetailsForPopup";
        //public const string usp_getTaxBillAuditDetails = "usp_getTaxBillAuditDetails";
        //public const string usp_getTaxBillAuditDetailsForSave = "usp_getTaxBillAuditDetailsForSave";
        //public const string usp_GetJuristictionsDetails = "usp_GetJuristictionsDetails";
        //public const string usp_GetCollectorDetails = "usp_GetCollectorDetails";
        //public const string usp_GetCollectorId = "usp_GetCollectorId";
        public const string usp_getProjectDetails = "usp_getProjectDetails";
        //public const string usp_getFCICurrentYearStatusDetails = "usp_getFCICurrentYearStatusDetails";
        //public const string usp_getFCIDashboardData = "usp_getFCIDashboardData";
        //public const string usp_getTaxBillAuditClientsForSearch = "usp_getTaxBillAuditClientsForSearch";
        //public const string usp_SPARTAXX_SEL_getSearchedClientDetailsForTaxBillAudit = "usp_SPARTAXX_SEL_getSearchedClientDetailsForTaxBillAuditScreen";
        //public const string usp_getTxBillAuditAllottedRecords = "usp_getTxBillAuditAllottedRecords";
        //public const string USP_UpdateTaxBillDocumentStatus = "USP_UpdateTaxBillDocumentStatus";
        //public const string usp_getTaxBillAuditJurisdictionResultInformation = "usp_getTaxBillAuditJurisdictionResultInformation";
        #endregion

        #region Account
        public const string usp_GeneratingNewClientQuickSetup = "usp_GeneratingNewClientQuickSetup";
        public const string usp_InsertSearchedAccountQuickClientManualSetup = "usp_InsertSearchedAccountQuickClientManualSetup";
        public const string usp_InsertSearchedAccountQuickClient_UtahFlorida_State = "usp_InsertSearchedAccountQuickClient_UtahFlorida_State";
        public const string usp_InsertSearchedAccountQuickClient = "usp_InsertSearchedAccountQuickClient";
        public const string usp_InsertTermsAndGroupsQuickClient = "usp_InsertTermsAndGroupsQuickClient";
        #endregion

        #region Existing Client
        public const string usp_get_ClientCodes = "usp_get_ClientCodes";
        public const string usp_Save_Address_ContactPhone = "usp_Save_Address_ContactPhone";
        public const string usp_getAccountDetailsSummary = "usp_getAccountDetailsSummary";
        #endregion

        #region Hearing Notices
        //public const string usp_getNextHearingNotice = "usp_getNextHearingNotice";
        public const string usp_getNextEntryHearingNotice = "usp_getNextEntryHearingNotice";
        public const string usp_getNextAuditHearingNotice = "usp_getNextAuditHearingNotice";
        public const string usp_getNextDefectHearingNotice = "usp_getNextDefectHearingNotice";
        public const string usp_getNextClarificationHearingNotice = "usp_getNextClarificationHearingNotice";
        public const string usp_getNextQCHearingNotice = "usp_getNextQCHearingNotice";
        public const string usp_getNextMultipleEntryRecordList = "usp_getNextMultipleEntryRecordList";

        public const string usp_getYearlyHearingDetailsById = "usp_getYearlyHearingDetailsById";    //New
        public const string usp_getYearlyHearingDetails = "usp_getYearlyHearingDetails";    //New

        /* Hearing Notice Allotted Master  */
        public const string usp_GetHearingNoticeEntryAllottedDocuments = "usp_GetHearingNoticeEntryAllottedDocuments";
        public const string usp_GetHearingNoticeAuditAllottedDocuments = "usp_GetHearingNoticeAuditAllottedDocuments";
        public const string usp_GetHearingNoticeDefectAllottedDocuments = "usp_GetHearingNoticeDefectAllottedDocuments";
        public const string usp_GetHearingNoticeClarificationAllottedDocuments = "usp_GetHearingNoticeClarificationAllottedDocuments";
        public const string usp_GetHearingNoticeQCAllottedDocuments = "usp_GetHearingNoticeQCAllottedDocuments";
        public const string usp_GetHearingNoticeMultipleEntryAllottedDocuments = "usp_GetHearingNoticeMultipleEntryAllottedDocuments_Perf"; //New

        /*Save Or Update*/
        public const string usp_InsertUserEnteredHearingNoticeData = "usp_InsertUserEnteredHearingNoticeData"; //New
        public const string usp_InsertUserEnteredHearingNoticeRemarks = "usp_InsertUserEnteredHearingNoticeRemarks"; //New
        public const string usp_InsertUserEnteredHearingTypeForNotice = "usp_InsertUserEnteredHearingTypeForNotice"; //New
        public const string usp_UpdateYearlyHearingDetails = "usp_UpdateYearlyHearingDetails"; //New
        public const string usp_InsertUpdateNoticeClarification = "usp_InsertUpdateNoticeClarification"; //New

        #endregion

        #region Hearing Results
        public const string usp_getHearingResultEntryAllottedDocuments = "usp_getHearingResultEntryAllottedDocuments";
        public const string usp_getHearingResultAuditAllottedDocuments = "usp_getHearingResultAuditAllottedDocuments";
        public const string usp_getHearingResultDefectAllottedDocuments = "usp_getHearingResultDefectAllottedDocuments";
        public const string usp_getHearingResultClarificationAllottedDocuments = "usp_getHearingResultClarificationAllottedDocuments";
        public const string usp_getHearingResultQCAllottedDocuments = "usp_getHearingResultQCAllottedDocuments";
        public const string usp_getNextHearingResultDocument_Defect = "usp_getNextHearingResultDocument_Defect";
        public const string usp_getSelectedHearingStatusByHearingType = "USP_getSelectedHearingStatusByHearingType";
        public const string usp_SEL_ExistingHearingDetailsForHearingResult = "USP_SEL_ExistingHearingDetailsForHearingResult";
        public const string usp_SEL_getDocumentDetailsFromPaperwise = "USP_SEL_getDocumentDetailsFromPaperwise";
        public const string usp_getHearingDetailsYearly = "usp_getHearingDetailsYearly"; //New
        public const string usp_getYearlyHearingDetailsByAccountId = "usp_getYearlyHearingDetailsByAccountId"; //New
        public const string usp_getHearingResultByHearingDetailsId = "usp_getHearingResultByHearingDetailsId"; //New
        public const string usp_getAccount = "usp_getAccount"; // New
        public const string usp_getInvoiceAndHearingResultMap = "usp_getInvoiceAndHearingResultMap"; //New
        public const string usp_getValueNotice = "usp_getValueNotice"; //New
        public const string usp_getInvoice = "usp_getInvoice"; //New
        public const string usp_getInvoiceWithInvoiceAndHearingResultMap = "usp_getInvoiceWithInvoiceAndHearingResultMap"; //New
        public const string usp_InsertUserEnteredHearingResults = "usp_InsertUserEnteredHearingResults"; //New
        public const string usp_updateIndexedNoticeDocument = "usp_updateIndexedNoticeDocument"; //New
        public const string usp_updateHearingDetails = "usp_updateHearingDetails"; //New
        public const string usp_InsertHearingDetailsRemarks = "usp_InsertHearingDetailsRemarks"; //New
        public const string usp_UpdateHearingResult = "usp_UpdateHearingResult"; //New
        public const string usp_UpdateValueNotice = "usp_UpdateValueNotice"; //New
        public const string usp_InsertInvoice = "usp_InsertInvoice"; //New
        #endregion

        //Added by SaravananS-Tfs id:52726
        public const string usp_getInvoiceDetailsForPreview_Multiyear = "usp_getInvoiceDetailsForPreview_Multiyear";
       //Ends here..

        //Added by SaravananS-TFS:47247
        public const string usp_getInvoiceDetailsForPreview_OT = "usp_getInvoiceDetailsForPreview_OT";
        public const string usp_checkinvoiceisoutoftexas = "usp_checkinvoiceisoutoftexas";
        #region Invoice Regular queue
        public const string usp_invoiceregularqueue_standard_OT = "usp_invoiceregularqueue_standard_OT";
        public const string usp_SaveOrUpdateOUTOFTEXASINVOICES = "usp_SaveOrUpdateOUTOFTEXASINVOICES";
        public const string USP_Checkhearingfinalized = "USP_Checkhearingfinalized";
        public const string usp_invoiceregularqueue_standard = "usp_invoiceregularqueue_standard";
        public const string usp_invoiceregularqueue_litigation = "usp_invoiceregularqueue_litigation";
        public const string usp_invoicedefectqueue_litigation = "usp_invoicedefectqueue_litigation";
        public const string usp_get_specificinvoice = "usp_get_specificinvoice";
        public const string usp_get_clientcontact = "usp_get_clientcontact";
        public const string usp_insertcorrqaccounts = "usp_insertcorrqaccounts";
        public const string usp_InsertCreditCorrqAccounts = "usp_InsertCreditCorrqAccounts";
        public const string usp_insertcorrq = "usp_insertcorrq";
        public const string USP_InsertCreditletterCorrq = "USP_InsertCreditletterCorrq";
        public const string usp_updateinvoicestatus = "usp_updateinvoicestatus"; 
        public const string usp_SaveOrUpdateInvoice = "usp_SaveOrUpdateInvoice";
        public const string usp_get_InvoicePaymentMap = "usp_get_InvoicePaymentMap";
        public const string usp_get_InvoicePayment = "usp_get_InvoicePayment";
        public const string usp_SaveOrUpdatePayment = "usp_SaveOrUpdatePayment";
        public const string usp_SaveOrUpdateInvoicePaymentMap = "usp_SaveOrUpdateInvoicePaymentMap";
        public const string usp_RemoveCapvalueAdjustment = "usp_RemoveCapvalueAdjustment";
        public const string GetSpecificInvoiceSummary = "GetSpecificInvoiceSummary";
        public const string usp_SaveOrUpdateInvoiceSummary = "usp_SaveOrUpdateInvoiceSummary";
        public const string usp_get_InvoiceAddressReference = "usp_get_InvoiceAddressReference";
        public const string usp_insertorupdateInvoiceRemarks = "usp_insertorupdateInvoiceRemarks";
        public const string usp_GetInvoiceIdFromInvoiceAdjustment = "usp_GetInvoiceIdFromInvoiceAdjustment";
        public const string usp_UpdateInvoiceProcessingStatus = "usp_UpdateInvoiceProcessingStatus";
        public const string usp_GetYearlyHearingDetailsByAccount = "usp_GetYearlyHearingDetailsByAccount";
        public const string USP_GetFailedCorrQueueRecords = "USP_GetFailedCorrQueueRecords";

        public const string usp_GetClientInvoiceEmailDetails = "usp_GetClientInvoiceEmailDetails"; //Added by SaravananS. tfs id:59825

        //Added by SaravananS.tfs id:59604
        public const string usp_getInvoiceDetailsForReport_SOAH = "usp_getInvoiceDetailsForReport_SOAH";
       //Ends here.
        //Existing SPs
        //public const string usp_getInvoiceDetailsForReportStandard = "usp_getInvoiceDetailsForReport_Test";
        public const string usp_getInvoiceDetailsForReportStandard = "usp_getInvoiceDetailsForReport_Regular";
        public const string usp_getInvoiceDetailsForReport_Regular_OT = "usp_getInvoiceDetailsForReport_Regular_OT";
        public const string  usp_getInvoiceDetailsForReport_Regular_Multiyear= "usp_getInvoiceDetailsForReport_Regular_Multiyear";
        public const string USP_getInvoiceFiles = "USP_getInvoiceFiles";
        public const string USP_InsertInvoiceFiles = "USP_InsertInvoiceFiles";
        public const string USP_UpdateInvoiceFiles = "USP_UpdateInvoiceFiles";
        public const string usp_getInvoiceDetailsForReportLitigation = "usp_getInvoiceDetailsForReportLitigation";
        public const string usp_getInvoiceDetailsForReport_Arbitration = "usp_getInvoiceDetailsForReport_Arbitration";
        public const string usp_getInvoiceDetailsForReportBppRendition = "usp_getInvoiceDetailsForReportBppRendition";
        public const string usp_getInvoiceDetailsForReportTaxBill = "usp_getInvoiceDetailsForReportTaxBill";
        public const string usp_GetClientRemarks = "usp_GetClientRemarks";
        public const string usp_getInvoiceServicePackageID = "usp_getInvoiceServicePackageID";
        public const string usp_getInvoiceGeneratedreportDetails = "usp_getInvoiceGeneratedreportDetails";
        public const string usp_getCreditInvoiceInputData = "usp_getCreditInvoiceInputData";
        public const string usp_getAutoAdjustmentInvoiceDetails = "usp_getAutoAdjustmentInvoiceDetails";
        public const string usp_GetCadNavigatorDetails = "usp_GetCadNavigatorDetails";
        public const string usp_GetClientDefaultDeliveryMethod = "usp_GetClientDefaultDeliveryMethod";
        public const string usp_getInvoiceInputData = "usp_getInvoiceInputData";
        public const string usp_getInvoiceInputData_OT = "usp_getInvoiceInputData_OT";
        public const string usp_getInvoiceGroupContactDetails = "usp_getInvoiceGroupContactDetails";
        public const string Usp_ValidateClientEmail = "Usp_ValidateClientEmail";
        public const string usp_getInvoiceDetailsForReport = "usp_getInvoiceDetailsForReport";
        public const string usp_getInvoiceDetailsForReport_OT = "usp_getInvoiceDetailsForReport_OT";
        public const string usp_UpdateInvoice = "usp_Epp_UpdateInvoice";
        public const string usp_CheckPosttoFlatFeeInvoicing = "usp_CheckPosttoFlatFeeInvoicing";
        public const string USP_UpdateQueueProcessStatus = "USP_UpdateInvoiceCorrQueueProcessStatus";
        public const string USP_GetLetterAndClientReadyToProcessForSelectedInvoice = "USP_GetLetterAndClientReadyToProcessForSelectedInvoice";
        public const string usp_get_Invoice_ClientEmailAddress = "usp_get_Invoice_ClientEmailAddress";
        public const string usp_getInvoiceBasicDetails = "usp_getInvoiceBasicDetails";
        public const string USP_CheckInvoicefilepath = "USP_CheckInvoicefilepath";
        public const string usp_InsertUpdateCreditInvoicePayment = "usp_InsertUpdateCreditInvoicePayment";
        //Existing SPs end here
        #endregion

        #region Invoice Adjustment Audit
        public const string usp_get_InvoiceAdjustmentRequest = "usp_get_InvoiceAdjustmentRequest";
        public const string usp_get_InvoiceAdjustmentLineItem = "usp_get_InvoiceAdjustmentLineItem"; 
        public const string usp_insertorupdateInvoiceAdjustmentLineItem = "usp_insertorupdateInvoiceAdjustmentLineItem";
        public const string usp_insertorupdateinvoiceAdjustmentManualLineItem = "usp_insertorupdateinvoiceAdjustmentManualLineItem";
        public const string usp_get_InvoiceAdjustmentManualLineItem = "usp_get_InvoiceAdjustmentManualLineItem";
        public const string usp_get_InvoiceExemptionJurisdiction = "usp_get_InvoiceExemptionJurisdiction";
        public const string usp_insertorupdateInvoiceExemptionJurisdiction = "usp_insertorupdateInvoiceExemptionJurisdiction";
        public const string usp_insertorupdateInvoiceAdjustment = "usp_insertorupdateInvoiceAdjustment";
        public const string usp_insertorupdateInvoiceAdjustmentAttachments = "usp_insertorupdateInvoiceAdjustmentAttachments";
        public const string usp_insertorupdateInvoiceAdjustmentRequest = "usp_insertorupdateInvoiceAdjustmentRequest";
        public const string usp_get_InvoicedetailsbyId = "usp_get_InvoicedetailsbyId";
        public const string InvoiceLineItembyInvoiceId = "InvoiceLineItembyInvoiceId";

        //Existing SPs
        public const string usp_GetAllottedInvoiceAdjustmentRequest = "usp_GetAllottedInvoiceAdjustmentRequest";
        public const string usp_getInvoiceNextAdjustmentAllottedDocument = "usp_getInvoiceNextAdjustmentAllottedDocument";
        public const string usp_getClientAndInvoiceDetails = "usp_getClientAndInvoiceDetails";
        public const string usp_getInvoiceAdjustmentPaymentsOrAdjustments = "usp_getInvoiceAdjustmentPaymentsOrAdjustments";
        public const string usp_getCollectionRemarks = "usp_getCollectionRemarks";
        public const string usp_getExemptionRemovedJurisdiction = "usp_getExemptionRemovedJurisdiction";
        public const string usp_getExemptionAccountList = "usp_getExemptionAccountList";
        public const string usp_getInvoiceAdjustmentHistory = "usp_getInvoiceAdjustmentHistory";
        //Existing SPs end here
        #endregion

        #region Pending Invoice
        public const string usp_get_HearingResult = "usp_get_HearingResult";
        public const string usp_insert_InvoiceAndHearingResultMap = "usp_insert_InvoiceAndHearingResultMap";

        //Existing SPs
        public const string usp_get_PendingGroupInvoices = "usp_get_PendingGroupInvoices";
        public const string usp_get_PendingGroupInvoicesSorted = "usp_get_PendingGroupInvoicesSorted";
        //Existing SPs end here
        #endregion

        #region Invoice special terms
        public const string usp_get_YearlyHearingDetails = "usp_get_YearlyHearingDetails";
        public const string usp_get_HearingResultByType = "usp_get_HearingResultByType";
        public const string usp_get_ArbitrationDetails = "usp_get_ArbitrationDetails";
        public const string usp_get_LitigationDetails = "usp_get_LitigationDetails";
        public const string usp_insertorupdateHearingResult = "usp_insertorupdateHearingResult";
        public const string usp_insertorupdateLitigation = "usp_insertorupdateLitigation";
        public const string usp_insertorupdateArbitrationDetails = "usp_insertorupdateArbitrationDetails";
        public const string usp_get_InvoicedetailsForTermLevel = "usp_get_InvoicedetailsForTermLevel";
        public const string usp_get_InvoicedetailsForProjectLevel = "usp_get_InvoicedetailsForProjectLevel";  
        public const string usp_insertorupdateInvoiceLineItem = "usp_insertorupdateInvoiceLineItem";
        public const string usp_insertorupdateInvoiceListMap = "usp_insertorupdateInvoiceListMap";
        public const string usp_get_DefaultDeliveryType = "usp_get_DefaultDeliveryType";
        public const string usp_get_InvoiceAndHearingResultMap = "usp_get_InvoiceAndHearingResultMap";
        public const string usp_GetTermtype = "usp_GetTermtype";
        //Existing SPs
        public const string usp_GetSpecialTermClients = "usp_GetSpecialTermClients";
        public const string usp_GetSpecialTermClients_OT = "usp_GetSpecialTermClients_OT";
        public const string Usp_GetDonotGenerateInvoicesList = "Usp_GetDonotGenerateInvoicesList";
        public const string usp_GetSpecialTermRegenerateClients = "usp_GetSpecialTermRegenerateClients";//Added by saravanans.tfs id:55312
        public const string usp_InvoiceReportStatusUpdate = "usp_InvoiceReportStatusUpdate";
        public const string usp_InvoiceStatusUpdate = "usp_InvoiceStatusUpdate";
        public const string Usp_GetSpecialTermAccounts = "Usp_GetSpecialTermAccounts";
        public const string Usp_GetSpecialTermOutofTexasAccounts = "Usp_GetSpecialTermAccounts_OT";
        //Existing SPs end here
        #endregion

        #region Invoice defects
        public const string usp_get_ptaxterms = "usp_get_ptaxterms";
        public const string usp_get_ptaxgroup = "usp_get_ptaxgroup";
        public const string usp_SaveOrUpdateTerms = "usp_SaveOrUpdateTerms";

        //Existing SPs
        public const string usp_getInvoiceDetailsForDefects_Litigation = "usp_getInvoiceDetailsForDefects_Litigation";
        public const string usp_getInvoiceDetailsForDefects_Arbitration = "usp_getInvoiceDetailsForDefects_Arbitration";
        public const string usp_getInvoiceDetailsForDefects_BppRendition = "usp_getInvoiceDetailsForDefects_BppRendition";
        public const string usp_getInvoiceDetailsForDefects_TaxBill = "usp_getInvoiceDetailsForDefects_TaxBill";
        public const string usp_getInvoiceDetailsForDefects_Standard = "usp_getInvoiceDetailsForDefects_Standard";
        public const string usp_getInvoiceDefectAccountDetails = "usp_getInvoiceDefectAccountDetails";
        public const string usp_getInvoiceDefectAccountJurisdictionDetails = "usp_getInvoiceDefectAccountJurisdictionDetails";
        //Existing SPs end here
        #endregion

        #region Flat fee
        //Existing SPs
        public const string usp_getInvoiceFlatFeePreHearingAccountDetails = "usp_getInvoiceFlatFeePreHearingAccountDetails";
        public const string usp_get_InvoiceFlatFeePreClietDetails = "usp_get_InvoiceFlatFeePreClietDetails";
        public const string usp_get_FlatFeeInvoiceGenerateAccountDetails = "usp_get_FlatFeeInvoiceGenerateAccountDetails";
        public const string usp_get_FlatFeeInvoiceGenerateProjectDetails = "usp_get_FlatFeeInvoiceGenerateProjectDetails";
        public const string usp_GetServicePackageID = "usp_GetServicePackageID";

        //Existing SPs end here
        #endregion
        #region Invoice pastdue 
        //Existing SPs
        public const string usp_getPastdueRunningStatus = "usp_getPastdueRunningStatus";
        public const string usp_getPastdueHistory = "usp_getPastdueHistory";
        //Existing SPs end here
        #endregion
        #region Mainscreen Invoice
        public const string usp_get_InvoiceRemarks = "usp_get_InvoiceRemarks";
        public const string usp_delete_InvoiceRemarks = "usp_delete_InvoiceRemarks";
        public const string usp_insert_CorrPaymentMapping = "usp_insert_CorrPaymentMapping";
        public const string usp_InsertUpdateClientCreditAdjustment = "usp_InsertUpdateClientCreditAdjustment";
        public const string usp_UpdateDebitClientCreditPayment = "usp_UpdateDebitClientCreditPayment";
        public const string usp_update_PaymentSettlementPlan = "usp_update_PaymentSettlementPlan";
        public const string usp_insert_LegalInvoice = "usp_insert_LegalInvoice";
        public const string usp_SaveOrUpdateSETTLEMENTPLAN = "usp_SaveOrUpdateSETTLEMENTPLAN";
        public const string usp_SaveOrUpdateSETTLEMENTPLANInvoices = "usp_SaveOrUpdateSETTLEMENTPLANInvoices";
        public const string usp_SaveOrUpdateCCPayment = "usp_SaveOrUpdateCCPayment";
        public const string usp_SaveOrUpdateContactLog = "usp_SaveOrUpdateContactLog";
        public const string usp_SaveOrUpdateContactLogRemarks = "usp_SaveOrUpdateContactLogRemarks";
        public const string usp_UpdateInvoiceAdjustment = "usp_UpdateInvoiceAdjustment";
        public const string usp_getinvoicecheckpaymentstatus = "usp_getinvoicecheckpaymentstatus";
        public const string usp_checkoutoftexas = "usp_checkoutoftexas";
        //Existing SPs
        public const string usp_get_MainScreenInvoiceStatusCollection = "usp_get_MainScreenInvoiceStatusCollection";
        public const string usp_getInvoiceComments = "usp_getInvoiceComments";
        public const string usp_SaveCollectionComments = "usp_SaveCollectionComments";
        public const string usp_getInvoiceTransactions = "usp_getInvoiceTransactions";
        public const string usp_getinvoicecollectiondetails = "usp_getinvoicecollectiondetails";
        public const string usp_getinvoicecollectiondetailscounts = "usp_getinvoicecollectiondetailscounts";
        public const string Usp_GetPastDueNotices = "Usp_GetPastDueNotices";
        public const string usp_getCollectionCodeAndFlagDetails = "usp_getCollectionCodeAndFlagDetails";
        public const string usp_GetCCPaymentInvoiceDetailsBasedOnInvoice = "usp_GetCCPaymentInvoiceDetailsBasedOnInvoice";
        public const string usp_getCollectionDetails = "usp_getCollectionDetails";
        public const string usp_GetCCPaymentInvoiceList = "usp_GetCCPaymentInvoiceList";
        public const string usp_GetCCPaymentDetailsForInvoices = "usp_GetCCPaymentDetailsForInvoices";
        public const string USP_POW_GETCLIENTDETAILS = "USP_POW_GETCLIENTDETAILS";
        public const string USP_UpdateCCPartialPayment = "USP_UpdateCCPartialPayment";
        //Existing SPs end here
        #endregion

        #region Invoice Calculation
        public const string usp_get_Payment = "usp_get_Payment";
        public const string usp_get_Client = "usp_get_Client";
        //Existing SPs
        public const string usp_GET_UserMessage = "usp_GET_UserMessage";

        //Existing SPs end here
        #endregion

        #region Invoice Report Generation
        public const string usp_getdocumentNomenClature = "usp_getdocumentNomenClature";
        public const string usp_InvoiceFlatFeeValidationForReportGeneration = "usp_InvoiceFlatFeeValidationForReportGeneration";
        #endregion Invoice Report Generation

        #region Invoice Payment
        public const string usp_InsertOneTimePaymentDetails = "usp_InsertOneTimePaymentDetails";
        #endregion Invoice Payment
        //ends here


        #region "Property Condition"
        public const string usp_getMSPropertyConditionQuestions = "usp_getMSPropertyConditionQuestions";
        #endregion

        #region MainScreen
        //public const string usp_getMSHearingdetails = "usp_getMSHearingdetails";
        //public const string usp_getMSAofAdetails = "usp_getMSAofAdetails";
        //public const string usp_getMSJurisdiction = "usp_getMSJurisdiction";
        //public const string usp_getInvoicePaymentSettlementDetailsforMainScreen = "usp_getInvoicePaymentSettlementDetailsforMainScreen";
        public const string USP_GetClientAccountSummary_MainScreen = "USP_GetClientAccountSummary_MainScreen";
        public const string usp_GetAccountDetailsforProperty = "usp_GetAccountDetailsforProperty";
        public const string usp_getMainScreenClientGroupContactDetails = "usp_getMainScreenClientGroupContactDetails";
        public const string usp_getMainScreenFieldname = "usp_getMainScreenFieldnameByRoleid";
        public const string usp_getClientAvailableDeliveryMethods = "usp_getClientAvailableDeliveryMethods";
        public const string Sp_DropDownForTermsFilter = "Sp_DropDownForTermsFilter";
        public const string usp_getGiftCardSentDateHistory = "usp_getGiftCardSentDateHistory";
        public const string usp_getClientTerms = "usp_getClientTerms";
        public const string usp_getTermLevelHistory = "usp_getTermLevelHistory";
        public const string USP_getClientGroupRecentPackageDetails = "USP_getClientGroupRecentPackageDetails";
        public const string USP_getClientPackageAccountDetails = "USP_getClientPackageAccountDetails";
        public const string usp_mainScreengetClientContactLog = "usp_mainScreengetClientContactLog";
        public const string usp_getClientAndDeliveryMethod = "usp_getClientAndDeliveryMethod";
        public const string USP_Get_OldCorrLog = "GetOldCorrLog";
        public const string usp_getClientConsolidationClientDetails = "usp_getClientConsolidationClientDetails";
        public const string usp_getCAFHistory = "usp_getCAFHistory";
        public const string usp_getTaxBillAuditClients = "usp_getTaxBillAuditClients";
        public const string usp_getExistingCAFHistory = "usp_getExistingCAFHistory";
        public const string usp_GetAccountNotes = "usp_GetAccountNotes";
        public const string usp_GetHearingRemarksforProperty = "usp_GetHearingRemarksforProperty";        
        public const string usp_GetAgentRemarksforAccount = "usp_GetAgentRemarksforAccount";
        public const string usp_GetAccountDetailAllRemarks = "usp_GetAccountDetailAllRemarks";
        public const string usp_getMainScreeninvoicedetails = "usp_getMainScreeninvoicedetails";
        public const string usp_GetInvoiceAdjustmentRequestDetails = "usp_GetInvoiceAdjustmentRequestDetails";
        public const string usp_GetCCPaymentDetailsBasedOnInvoice = "usp_GetCCPaymentDetailsBasedOnInvoice";
        public const string usp_get_MainScreenClientCreditDetails = "usp_get_MainScreenClientCreditDetails";
        public const string usp_GetInvoiceAdjustmentAuditDetails = "usp_GetInvoiceAdjustmentAuditDetails";
        public const string usp_getClientPaymentPlan = "usp_getClientPaymentPlan";
        public const string usp_getClientSettlememtPlan = "usp_getClientSettlememtPlan";
        public const string usp_getFinancialDetails = "usp_getFinancialDetails";
        public const string USP_GetDNDStatus = "USP_GetDNDStatus";
        public const string USP_GetAccountDetailFlags = "USP_GetAccountDetailFlags";
        public const string usp_getAccountSetupParamtersData = "usp_getAccountSetupParamtersData";
        public const string usp_getPropertyDetailsData = "usp_getPropertyDetailsData";
        public const string usp_getValueNoticeData = "usp_getValueNoticeData";
        public const string usp_getHearingDetailsData = "usp_getHearingDetailsData";
        public const string usp_getHearingResultData = "usp_getHearingResultData";
        public const string usp_CalculatePriorYearTaxRatebasedonAccountJurisdiction = "usp_CalculatePriorYearTaxRatebasedonAccountJurisdiction";
        public const string usp_PTAX_get_Jurisdiction = "usp_PTAX_get_Jurisdiction";
        public const string usp_GetAccountTaxRollJurisdiction = "usp_GetAccountTaxRollJurisdiction";
        public const string usp_PTAX_get_CountyJurisdiction = "usp_PTAX_get_CountyJurisdiction";
        public const string usp_getFCIPropertyDetails = "usp_getFCIPropertyDetails";
        public const string usp_GetFCIDataRemarksDetails = "usp_GetFCIDataRemarksDetails";
        public const string usp_getmenuIsActivebasedOnUserRoletemp = "usp_getmenuIsActivebasedOnUserRoletemp";
        public const string usp_getACCLlogDetails = "usp_getACCLlogDetails";
        public const string usp_get_NextClientNumber = "usp_get_NextClientNumber";
        public const string SaveOrUpdate_PTAX_Client = "SaveOrUpdate_PTAX_Client";
        public const string SaveOrUpdate_PTAX_ClientRemarks_Type = "SaveOrUpdate_PTAX_ClientRemarks_Type";
        public const string SaveOrUpdate_PTAX_Account_Type = "SaveOrUpdate_PTAX_Account_Type";
        public const string SaveOrUpdate_PTAX_ClientSetupParameters = "SaveOrUpdate_PTAX_ClientSetupParameters";
        public const string usp_get_VerifyClientDocuments = "usp_get_VerifyClientDocuments";
        public const string USP_GetClassificationType = "USP_GetClassificationType";
        public const string USP_GetClientNumber = "USP_GetClientNumber";
        public const string USP_GetActiveAccount = "USP_GetActiveAccount";
        public const string SaveOrUpdate_PTAX_IndexedClientDocument_ClientIDIntoIndexClientDocument = "SaveOrUpdate_PTAX_IndexedClientDocument_ClientIDIntoIndexClientDocument";
        public const string SaveOrUpdate_PTAX_Account_CanbeDelete = "SaveOrUpdate_PTAX_Account_CanbeDelete";
        public const string usp_SPARTAXX_SEL_AllCAFAccounts = "usp_SPARTAXX_SEL_AllCAFAccounts";
        public const string SaveOrUpdate_PTAX_Group = "SaveOrUpdate_PTAX_Group";
        public const string SaveOrUpdate_PTAX_Terms = "SaveOrUpdate_PTAX_Terms";
        public const string usp_getTermAccountLevelHistory = "usp_getTermAccountLevelHistory";
        public const string usp_getGroupTermsDetails = "usp_getGroupTermsDetails";
        public const string usp_getAssandUnAssAccountscount = "usp_getAssandUnAssAccountscount";
        public const string usp_getAgreementHistory = "usp_getAgreementHistory";
        public const string usp_getTermsAccounts = "usp_getTermsAccounts";
        public const string usp_getAccountTermHistory = "usp_getAccountTermHistory";
        public const string usp_getAccountCurrentTermDetails = "usp_getAccountCurrentTermDetails";
        public const string usp_getAgreementsDetailsRecent = "usp_getAgreementsDetailsRecent";
        public const string usp_getAccountCountbasedonAccountStatusandClientID = "usp_getAccountCountbasedonAccountStatusandClientID";
        public const string usp_GetHearingDetailsForProperty = "usp_GetHearingDetailsForProperty";
        public const string usp_GetHearingDetailsForProperty_NonTexas = "usp_GetHearingDetailsForProperty_NonTexas";
        public const string usp_GetInformalInformalDateForMainScreen = "usp_GetInformalInformalDateForMainScreen";
        public const string usp_getHeaingDetailsDateHistory = "usp_HearingDateHistory";
        public const string usp_GetCorrectionMotionFiledDetails = "usp_GetCorrectionMotionFiledDetails";
        public const string usp_getPanelDocketIDHistory = "usp_getPanelDocketIDHistory";
        public const string USP_GetHearingNoticeLetterDateHistory = "USP_GetHearingNoticeLetterDateHistory";
        public const string usp_getAofADetails = "usp_getAofADetails";
        public const string usp_GetHearingDetailsforAOfA = "usp_GetHearingDetailsforAOfA";
        public const string usp_GetPastHearingDetailsforAOfA = "usp_GetPastHearingDetailsforAOfA";
        public const string usp_getAofAPreviousRevisedDateHistory = "usp_getAofAPreviousRevisedDateHistory";
        public const string usp_getAofADateCodedEndHistory = "usp_getAofADateCodedEndHistory";
        public const string usp_CadLegalNameHistory = "usp_CadLegalNameHistory";
        public const string usp_getClientContactLog = "usp_getClientContactLog";
        public const string usp_getAofADateCodedHistory = "usp_getAofADateCodedHistory";
        public const string usp_getTaxyearBasedArbitration = "usp_getTaxyearBasedArbitration";
        public const string Usp_getArbitratorDetails = "Usp_getArbitratorDetails";
        public const string usp_getDocumentsFromPaperwiseUsingMutiDocType = "usp_getDocumentsFromPaperwiseUsingMutiDocType";
        public const string USP_LoadLitigationDetails = "USP_LoadLitigationDetails";
        public const string usp_GetPendingLitigationTaxYear = "usp_GetPendingLitigationTaxYear";
        public const string usp_ViewDocuments = "usp_ViewDocuments";
        public const string Usp_LoadSupplementHistory = "Usp_LoadSupplementHistory";
        public const string usp_getLitigationTrialDateHistory = "usp_getLitigationTrialDateHistory";
        public const string USP_LoadLitigationSecDetails = "USP_LoadLitigationSecDetails";
        public const string usp_GetPendingSecLitigationTaxYear = "usp_GetPendingSecLitigationTaxYear";
        public const string usp_GetMainScreenValues = "usp_GetMainScreenValues";
        public const string usp_SPARTAXX_SEL_AofARemarks = "usp_SPARTAXX_SEL_AofARemarks";
        public const string usp_IsOutOfTexasProperty = "usp_IsOutOfTexasProperty";
        public const string usp_CheckinvoiceisMultiyear = "usp_CheckinvoiceisMultiyear";//Added by SaravananS.tfs id:55925

        //public const string usp_GetHearingDetailsForProperty = "usp_GetHearingDetailsForProperty";
        //public const string usp_GetAccountDetailAllRemarks = "usp_GetAccountDetailAllRemarks";
        //public const string usp_GetAccountNotes = "usp_GetAccountNotes";
        //public const string usp_GetHearingRemarksforProperty = "usp_GetHearingRemarksforProperty";
        //public const string usp_GetAgentRemarksforAccount = "usp_GetAgentRemarksforAccount";
        //public const string usp_get_MainScreenInvoiceStatusCollection = "usp_get_MainScreenInvoiceStatusCollection";
        //public const string usp_getMainScreeninvoicedetails = "usp_getMainScreeninvoicedetails";
        //public const string USP_GetLitigationTaxRefundDetails = "USP_GetLitigationTaxRefundDetails";
        //public const string usp_getHeaingDetailsDateHistory = "usp_HearingDateHistory";
        //public const string usp_GetTaxRefundAcccountsAndTaxYearList = "usp_GetTaxRefundAcccountsAndTaxYearList";
        //public const string usp_GetTaxRefundCollectorList = "usp_GetTaxRefundCollectorList";
        //public const string usp_Get_TaxRefundLetterDetails = "usp_Get_TaxRefundLetterDetails";
        //public const string usp_GetMainScreenValues = "usp_GetMainScreenValues";
        //public const string usp_GetClientRemarks = "usp_GetClientRemarks";
        //public const string usp_GetCCPaymentDetailsBasedOnInvoice = "usp_GetCCPaymentDetailsBasedOnInvoice";
        //public const string usp_GetCCPaymentInvoiceDetailsBasedOnInvoice = "usp_GetCCPaymentInvoiceDetailsBasedOnInvoice";
        //public const string usp_GetHearingDetailsforAOfA = "usp_GetHearingDetailsforAOfA";
        //public const string usp_getLitigationTrialDateHistory = "usp_getLitigationTrialDateHistory";
        //public const string usp_GetTermRemarks = "usp_GetTermRemarks";
        //public const string usp_getPanelDocketIDHistory = "usp_getPanelDocketIDHistory"; 

        public const string USP_GetILPropertyHearingRemarks = "USP_GetILPropertyHearingRemarks";
        public const string USP_GetILPropertyAgentRemarks = "USP_GetILPropertyAgentRemarks";
        public const string USP_GetILHearingDetails = "usp_GetILHearingDetails";

        public const string USP_GetIOWAPropertyHearingRemarks = "USP_GetIOWAPropertyHearingRemarks";
        public const string USP_GetIOWAPropertyAgentRemarks = "USP_GetIOWAPropertyAgentRemarks";
        public const string USP_GetIOWAHearingDetails = "usp_GetIOWAHearingDetails";
        public const string USP_GetMainscreenGotoDDLValues = "USP_GetMainscreenGotoDDLValues";
        #endregion

        public const string usp_getMSPropertySurvey = "usp_getMSPropertySurvey";
        public const string usp_GetInvoiceExemptionJurisdictionHistory = "usp_GetInvoiceExemptionJurisdictionHistory";
        public const string usp_InsertUpdateExemptionJurisdictionHistory = "usp_InsertUpdateExemptionJurisdictionHistory";
        public const string usp_getMSBPPPropertyDetails = "usp_getMSBPPPropertyDetails";

        #region Disaster invoice generation..tfs id:56033
        public const string usp_getDisasterInvoiceDetails = "usp_getDisasterInvoiceDetails";
        public const string usp_getInvoiceDetailsForPreview_Disaster = "usp_getInvoiceDetailsForPreview_Disaster";
        public const string usp_GetDisasterInvoiceInputData = "usp_GetDisasterInvoiceInputData";
        public const string usp_IsDisasterInvoice = "usp_IsDisasterInvoice";
        #endregion Disaster invoice generation

        #region Affidavit Workflow
        
        public const string usp_getAffidavitAuditAllottedDocuments = "usp_getAffidavitAuditAllottedDocuments";
        public const string usp_getNextAffidavitDocument_Audit = "usp_getNextAffidavitDocument_Audit";
        #endregion

        #region Optimization //Added by SaravananS
        public const string usp_getMainScreeninvoicedetails_AllInvoices = "usp_getMainScreeninvoicedetails_AllInvoices";
        public const string usp_getMainScreeninvoicedetails_NotPaidInvoices = "usp_getMainScreeninvoicedetails_NotPaidInvoices";
        public const string usp_getMainScreeninvoicedetails_PaidInvoices = "usp_getMainScreeninvoicedetails_PaidInvoices";
        public const string usp_getMainScreeninvoicedetails_PastDueInvoices = "usp_getMainScreeninvoicedetails_PastDueInvoices";
        public const string usp_getMainScreeninvoicedetails_TotalInvoices = "usp_getMainScreeninvoicedetails_TotalInvoices";
        public const string usp_getMainScreeninvoicedetails_CreditInvoices = "usp_getMainScreeninvoicedetails_CreditInvoices";
        public const string usp_checkinvoiceisHotelLuc = "usp_checkinvoiceisHotelLuc";
        public const string usp_UpdateInvoiceCorrqFilePath = "usp_UpdateInvoiceCorrqFilePath";
        public const string usp_getServicePackageNeoPostpath = "usp_getServicePackageNeoPostpath";
        public const string usp_getMainScreeninvoicedetails_AdjustedInvoices = "usp_getMainScreeninvoicedetails_AdjustedInvoices";//Added by SaravananS. tfs id:60677
        #endregion Optimization 

        #region DeferredMaintenance
        public const string usp_DM_MediaDetailMapPaperwise = "usp_DM_MediaDetailMapPaperwise";
        public const string usp_DM_GetPropertyMediaDetailList = "usp_DM_GetPropertyMediaDetailList";
        public const string usp_DM_UpdateDMFinalValue = "usp_DM_UpdateDMFinalValue";
        public const string USP_DM_ConciergeLinkGenerate = "USP_DM_ConciergeLinkGenerate";
        #endregion

        #region IL hearing
        public const string usp_IsILHearing = "usp_IsILHearing";
        #endregion

        #region Concierge
        public const string usp_CG_SavePWImageId = "usp_CG_SavePWImageId";
        #endregion

        public const string usp_GetClientDeliveryAddressForInvoice = "usp_GetClientDeliveryAddressForInvoice";//Added by SaravananS. tfs id:63159

        #region //Added by SaravananS. tfs id:63335
        public const string usp_GetHearingType = "usp_GetHearingType";
        #endregion

        public const string usp_invoiceregularqueue_arbitration = "usp_invoiceregularqueue_arbitration";
        public const string usp_invoicedefectqueue_arbitration = "usp_invoicedefectqueue_arbitration";
        public const string usp_get_ClientEmailAddress_Invoice = "usp_get_ClientEmailAddress_Invoice";//Added by SaravananS. tfs id:62789

        //Added by SaravananS. tfs id:63613
        public const string Usp_GetTaxrateForTheGivenTaxyear = "Usp_GetTaxrateForTheGivenTaxyear";
        //Ends here.

        //Added by Saravanans. tfs id:64496
        public const string Usp_Get_InvoiceDetailsForJudgment = "Usp_Get_InvoiceDetailsForJudgment";
        //Ends here.
        public const string usp_IsILAccount = "usp_IsILAccount";
        public const string USP_GetGAHearingDetails = "usp_GetGAHearingDetails";
    }
}
