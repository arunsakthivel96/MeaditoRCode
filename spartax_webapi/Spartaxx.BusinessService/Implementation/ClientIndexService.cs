using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Spartaxx.BusinessObjects.Litigation;
using Spartaxx.BusinessObjects.GAHearingDetails;

namespace Spartaxx.BusinessService
{
    public class ClientIndexService : IClientIndexService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public ClientIndexService()
        {
            _UnitOfWork = new UnitOfWork();
        }

        public PXTboListDefinition getListDefinitions(PXTboListDefinition objListDefinition)
        {
            return _UnitOfWork.ClientIndexRepository.getListDefinitions(objListDefinition);
        }
        public List<PTXboProject> getProjectDetails(int ClientId)
        {
            return _UnitOfWork.ClientIndexRepository.getProjectDetails(ClientId);
        }
        public PTXboClientDeliveryMethods getClientDeliveryMethods(int ClientId)
        {
            return _UnitOfWork.ClientIndexRepository.getClientDeliveryMethods(ClientId);
        }
        public DataTable getAccountDetailsCount(string ClientNumber)
        {
            return _UnitOfWork.ClientIndexRepository.getAccountDetailsCount(ClientNumber);
        }
        public PTXboDashboardAccountCount getAccountDetailsCount_Dashboard(int ClientID)
        {
            return _UnitOfWork.ClientIndexRepository.getAccountDetailsCount_Dashboard(ClientID);
        }
        public List<PTXboAccountDetails> getAccountsForBinding(PTXboGetAccountsForBinding model)
        {
            return _UnitOfWork.ClientIndexRepository.getAccountsForBinding(model);
        }
        public List<PTXboMSPropertyConditionQuestionnaire> getMSPropertyCondition(PTXboPropertyConditionModel objmodel)
        {
            return _UnitOfWork.ClientIndexRepository.getMSPropertyCondition(objmodel);
        }
        //public PTXboClientAndAccountDetails getClientAndAccountDetails(PTXboClientAndAccountDetails model)
        //{
        //    return _UnitOfWork.ClientIndexRepository.getClientAndAccountDetails(model);
        //}
        public string getParamValue(int paramID)
        {
            return _UnitOfWork.ClientIndexRepository.getParamValue(paramID);
        }
        public DataSet getClientNumber(string ClientNumber)
        {
            return _UnitOfWork.ClientIndexRepository.getClientNumber(ClientNumber);
        }
        public DataSet getResultState(int ClientID)
        {
            return _UnitOfWork.ClientIndexRepository.getResultState(ClientID);
        }
        public PTXboClientGroupContact getClientGroupContact(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getClientGroupContact(model);
        }
        public List<PTXboMainScreenFieldNames> getMainScreenFieldName(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getMainScreenFieldName(model);
        }
        public DataSet LoadDropDownForTermsFilter(int clienID)
        {
            return _UnitOfWork.ClientIndexRepository.LoadDropDownForTermsFilter(clienID);
        }
        public List<PTXboClientGiftcardHistory> getGiftCardSentDateHistory(int ClientID)
        {
            return _UnitOfWork.ClientIndexRepository.getGiftCardSentDateHistory(ClientID);
        }
        public List<PTXboClientTermsDetails> getClientTerms(PTXboClientTerms model)
        {
            return _UnitOfWork.ClientIndexRepository.getClientTerms(model);
        }
        public List<PTXboTermLevelHistory> getTermLevelHistory(int GroupId)
        {
            return _UnitOfWork.ClientIndexRepository.getTermLevelHistory(GroupId);
        }
        public List<PTXboClientGroupRecentPackageDetails> getClientGroupRecentPackageDetails(int clientId, int stateID, string County)
        {
            return _UnitOfWork.ClientIndexRepository.getClientGroupRecentPackageDetails(clientId, stateID, County);
        }
        public List<PTXboClientPackageAccountDetails> GetClientPackageAccountDetails(int clientId, int stateID, string County, int packageTypeId, string packageTypeMode)
        {
            return _UnitOfWork.ClientIndexRepository.GetClientPackageAccountDetails(clientId,stateID, County, packageTypeId, packageTypeMode);
        }
        public List<PTXboClientContactLog> getClientContactLog(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getClientContactLog(model);
        }
        public DataTable getClientAndDeliveryMethod(string ClientNumber)
        {
            return _UnitOfWork.ClientIndexRepository.getClientAndDeliveryMethod(ClientNumber);
        }
        public List<PTXboCorrespondenceLogOld> GetOldCorrLog(string ClientNumber)
        {
            return _UnitOfWork.ClientIndexRepository.GetOldCorrLog(ClientNumber);
        }
        public DataSet getClientConsolidationClientDetails(string ClientNumber)
        {
            return _UnitOfWork.ClientIndexRepository.getClientConsolidationClientDetails(ClientNumber);
        }
        public DataSet getCAFHistory(int ClientId)
        {
            return _UnitOfWork.ClientIndexRepository.getCAFHistory(ClientId);
        }
        public List<PTXboTaxBillClientSearchResult> getTaxBillAuditClients(PTXboTaxBillClientSearchRequest objPTXboTaxBillClientSearch)
        {
            return _UnitOfWork.ClientIndexRepository.getTaxBillAuditClients(objPTXboTaxBillClientSearch);
        }
        public DataSet getPTAXCAFHistory(string ClientNumber)
        {
            return _UnitOfWork.ClientIndexRepository.getPTAXCAFHistory(ClientNumber);
        }
        public List<PTXboAccountNotesProperty> GetAccountNotes(int accountId)
        {
            return _UnitOfWork.ClientIndexRepository.GetAccountNotes(accountId);
        }
        public List<PTXboHearingDetailsRemarks> GetHearingRemarks(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetHearingRemarks(model);
        }
        public List<PTXboAofARemarks> getAofARemarks(int accountId)
        {
            return _UnitOfWork.ClientIndexRepository.getAofARemarks(accountId);
        }
        public List<PTXboAgentRemarks> GetAgentRemarks(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetAgentRemarks(model);
        }
        public List<PTXboPropertyRemarks> GetPropertyRemarks(int accountId)
        {
            return _UnitOfWork.ClientIndexRepository.GetPropertyRemarks(accountId);
        }
        public List<BusinessObjects.ViewModels.PTXMainScreenInvoiceDetailResult> getInvoicecollectionDetails(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getInvoicecollectionDetails(model);
        }
        public List<PTXboInvoiceAdjustmentRequestDetails> GetInvoiceAdjRequestDetails(int Invoiceid)
        {
            return _UnitOfWork.ClientIndexRepository.GetInvoiceAdjRequestDetails(Invoiceid);
        }
        public List<PTXboCCPayment> GetCCPaymentDetails(int ClientID)
        {
            return _UnitOfWork.ClientIndexRepository.GetCCPaymentDetails(ClientID);
        }
        public List<PTXboInvoiceAdjustmentDetails> GetInvoiceAdjustmentAuditDetails(int Invoiceid)
        {
            return _UnitOfWork.ClientIndexRepository.GetInvoiceAdjustmentAuditDetails(Invoiceid);
        }
        public List<PTXboInvoiceExemptionJurisdictionHistory> GetInvoiceExemptionJurisdictionHistory(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetInvoiceExemptionJurisdictionHistory(model);
        }
        public List<PTXboCSPaymentPlanDetails> getClientPaymentPlan(string invoiceIdList)
        {
            return _UnitOfWork.ClientIndexRepository.getClientPaymentPlan(invoiceIdList);
        }
        public List<PTXboCSSettlementPlanDetails> getClientSettlementPlan(string invoiceIdList)
        {
            return _UnitOfWork.ClientIndexRepository.getClientSettlementPlan(invoiceIdList);
        }
        public List<PTXboCSPaymentPlanDetails> getClientPaymentPlanHistory(int invoiceId)
        {
            return _UnitOfWork.ClientIndexRepository.getClientPaymentPlanHistory(invoiceId);
        }
        public List<PTXboCSSettlementPlanDetails> getClientSettlementPlanHistory(int invoiceId)
        {
            return _UnitOfWork.ClientIndexRepository.getClientSettlementPlanHistory(invoiceId);
        }
        public List<PTXboFinancials> getFinancialDetails(int Accountid)
        {
            return _UnitOfWork.ClientIndexRepository.getFinancialDetails(Accountid);
        }
        public DataSet GetDNDFlagStatus(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetDNDFlagStatus(model);
        }
        public string GetAccountDetailFlags(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetAccountDetailFlags(model);
        }
        public PTXboAccountSetupParameters getAccountSetupParameterDetails(int AccountID)
        {
            return _UnitOfWork.ClientIndexRepository.getAccountSetupParameterDetails(AccountID);
        }
        public PTXboPropertyDetails getPropertyDetailsData(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getPropertyDetailsData(model);
        }
        public bool GetMainscreenGotoDDLValues(PTXboMainScreenFieldNames requestInput, out List<PTXboMainScreenFieldNames> getGotoDDLVal, out string errorMessage)
        {
            return _UnitOfWork.ClientIndexRepository.GetMainscreenGotoDDLValues(requestInput, out getGotoDDLVal, out errorMessage);
        }
        public PTXboValueNotice getValueNoticeData(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getValueNoticeData(model);
        }
        public List<PTXboHearingDetails> getHearingDetailsData(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getHearingDetailsData(model);
        }
        public List<PTXboHearingResults> getHearingResultData(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getHearingResultData(model);
        }
        public double getPriorYearTaxRateForSelectedAccount(PTXboParameters modelrate)
        {
            return _UnitOfWork.ClientIndexRepository.getPriorYearTaxRateForSelectedAccount(modelrate);
        }
        public List<DataObjects.PTXdoAccountJurisdiction> getAccountJurisdiction(PTXboParameters modelrate)
        {
            return _UnitOfWork.ClientIndexRepository.getAccountJurisdiction(modelrate);
        }
        public List<DataObjects.PTXdoAccountJurisdiction> getAccountTaxRollJurisdiction(PTXboParameters modelrate)
        {
            return _UnitOfWork.ClientIndexRepository.getAccountTaxRollJurisdiction(modelrate);
        }
        public List<PTXboCountyJurisdiction> getCountyJurisdiction(PTXboParameters modelrate)
        {
            return _UnitOfWork.ClientIndexRepository.getCountyJurisdiction(modelrate);
        }
        public PTXboFCIPropertyDetails getFCIPropertyDetails(PTXboFCIPropertyDetails propertyDetails)
        {
            return _UnitOfWork.ClientIndexRepository.getFCIPropertyDetails(propertyDetails);
        }
        public List<PTXboFCIDataRemarks> GetFCIDataRemarksDetails(int PropertyDetailsId)
        {
            return _UnitOfWork.ClientIndexRepository.GetFCIDataRemarksDetails(PropertyDetailsId);
        }
        public bool getmenuIsActivebasedOnUserRole(PTXbomenuIsActivebasedOnUserRole model)
        {
            return _UnitOfWork.ClientIndexRepository.getmenuIsActivebasedOnUserRole(model);
        }
        public List<PTXboACCLlogSearchResult> getACCLlogDetails(PTXboACCLlogSearchRequest objACCLlogSearchRequest)
        {
            return _UnitOfWork.ClientIndexRepository.getACCLlogDetails(objACCLlogSearchRequest);
        }
        public DataSet getACCLlogDetailsNew(PTXboACCLlogSearchRequest objACCLlogSearchRequest)
        {
            return _UnitOfWork.ClientIndexRepository.getACCLlogDetailsNew(objACCLlogSearchRequest);
        }
        public int MainScreenupdateClientCodes(PTXboInsertUpdateClientCode model)
        {
            return _UnitOfWork.ClientIndexRepository.MainScreenupdateClientCodes(model);
        }
        public string generateTemporaryClientNumber(string TaxYear)
        {
            return _UnitOfWork.ClientIndexRepository.generateTemporaryClientNumber(TaxYear);
        }
        public DataSet CheckClientDetailsBeforeGeneration(int ClientId)
        {
            return _UnitOfWork.ClientIndexRepository.CheckClientDetailsBeforeGeneration(ClientId);
        }
        public bool updateClientDeliveryMethods(PTXboClientDeliveryMethods objClientDeliveryMethods)
        {
            return _UnitOfWork.ClientIndexRepository.updateClientDeliveryMethods(objClientDeliveryMethods);
        }
        public List<PTXboCAFRequestonExpiry> getCAFRequestonExpiry(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getCAFRequestonExpiry(model);
        }
        public bool updateGroupDetails(PTXboGroup objGroupDetails)
        {
            return _UnitOfWork.ClientIndexRepository.updateGroupDetails(objGroupDetails);
        }
        public List<PTXboAccountLevelHistory> getTermAccountLevelHistory(int GroupId)
        {
            return _UnitOfWork.ClientIndexRepository.getTermAccountLevelHistory(GroupId);
        }
        public PTXboGroup getTermsDetails(int GroupId)
        {
            return _UnitOfWork.ClientIndexRepository.getTermsDetails(GroupId);
        }
        public DataTable getAssandUnAssAccountscount(PTXboSearchAccountCriteria objSearchAccountCriteria)
        {
            return _UnitOfWork.ClientIndexRepository.getAssandUnAssAccountscount(objSearchAccountCriteria);
        }
        public List<PTXboAgreements> getAgreementsHistory(int GroupId)
        {
            return _UnitOfWork.ClientIndexRepository.getAgreementsHistory(GroupId);
        }
        public List<PTXboTermAccounts> getTermsAccounts(PTXboSearchAccountCriteria objSearchAccountCriteria)
        {
            return _UnitOfWork.ClientIndexRepository.getTermsAccounts(objSearchAccountCriteria);
        }
        public List<PTXboTermsDetails> getAccountTermHistory(int AccountId)
        {
            return _UnitOfWork.ClientIndexRepository.getAccountTermHistory(AccountId);
        }
        public List<PTXboTermsDetails> getAccountCurrentTermDetails(int AccountId)
        {
            return _UnitOfWork.ClientIndexRepository.getAccountCurrentTermDetails(AccountId);
        }
        public PTXboAgreements getAgreementDetails(int GroupID)
        {
            return _UnitOfWork.ClientIndexRepository.getAgreementDetails(GroupID);
        }
        public DataTable getCafAccountCountbasedonStatus(int clientID)
        {
            return _UnitOfWork.ClientIndexRepository.getCafAccountCountbasedonStatus(clientID);
        }
        public PTXboHearingDetailsProperty GetHearingDetails(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetHearingDetails(model);
        }
        public DataSet loadInformalInformalFlag(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.loadInformalInformalFlag(model);
        }
        public List<PTXboHearingDateHistory> GetHearingDateHistory(int HearingDetailsId)
        {
            return _UnitOfWork.ClientIndexRepository.GetHearingDateHistory(HearingDetailsId);
        }
        public List<PTXboCMAccountLetterDetails> GetCorrectionMotionFiledDetails(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetCorrectionMotionFiledDetails(model);
        }
        public List<PTXboPanelDocketIDDetails> GetPanelDocketIDHistory(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetPanelDocketIDHistory(model);
        }
        public List<PTXboHearingNoticeLetterHistory> GetHearingNoticeLetterDateHistory(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetHearingNoticeLetterDateHistory(model);
        }
        public PTXboUserEnteredAofADetail getAofADetails(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getAofADetails(model);
        }
        public List<PTXboHearingDetails> getHearingInfo(int Accountid)
        {
            return _UnitOfWork.ClientIndexRepository.getHearingInfo(Accountid);
        }
        public List<PTXboHearingDetails> getPastHearingInfo(int Accountid)
        {
            return _UnitOfWork.ClientIndexRepository.getPastHearingInfo(Accountid);
        }
        public List<PTXboPreviousAofADetails> getAofAPreviousRevisedDateHistory(int Accountid)
        {
            return _UnitOfWork.ClientIndexRepository.getAofAPreviousRevisedDateHistory(Accountid);
        }
        public List<PTXboDateCodedHistory> getDateCodedEndHistory(int Accountid)
        {
            return _UnitOfWork.ClientIndexRepository.getDateCodedEndHistory(Accountid);
        }
        public List<PTXboCadLegalNameHistory> getCADLegalNameHistory(int Accountid)
        {
            return _UnitOfWork.ClientIndexRepository.getCADLegalNameHistory(Accountid);
        }
        public DataTable getClientContactLogLitigation(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getClientContactLogLitigation(model);
        }
        public List<PTXboDateCodedHistory> getDateCodedHistory(int Accountid)
        {
            return _UnitOfWork.ClientIndexRepository.getDateCodedHistory(Accountid);
        }
        public PTXboArbitrationDetails GetTaxyearBasedArbitration(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetTaxyearBasedArbitration(model);
        }
        public List<PTXboArbitratorDetails> getArbitratorDetails(int Arbitrator)
        {
            return _UnitOfWork.ClientIndexRepository.getArbitratorDetails(Arbitrator);
        }
        public List<PTXboDocumentsFromPaperwise> getDocumentsfromPaperwiseUsingMultiDoctype(PTXboDocumentsFromPaperwise objSearchCriteria)
        {
            return _UnitOfWork.ClientIndexRepository.getDocumentsfromPaperwiseUsingMultiDoctype(objSearchCriteria);
        }
        public DataSet LoadLitigationDetails(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.LoadLitigationDetails(model);
        }
        public string GetPendingLitigationTaxYear(int AccountID)
        {
            return _UnitOfWork.ClientIndexRepository.GetPendingLitigationTaxYear(AccountID);
        }
        public List<PTXboLitigationDocuments> ViewDocuments(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.ViewDocuments(model);
        }
        public List<PTXboSupplementaryDetails> LoadSupplementHistory(int LitigationID)
        {
            return _UnitOfWork.ClientIndexRepository.LoadSupplementHistory(LitigationID);
        }
        public List<PTXboLitigationAccountDetails> GetTrialDateHistory(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetTrialDateHistory(model);
        }
        public DataSet LoadLitigationDetailsII(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.LoadLitigationDetailsII(model);
        }
        public string GetPendingLitigationTaxYearII(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetPendingLitigationTaxYearII(model);
        }
        public List<PTXboLitigationSecAccountDetails> SecGetTrialDateHistory(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.SecGetTrialDateHistory(model);
        }
        public PTXboMainScreenValues getMainScreenValues(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.getMainScreenValues(model);
        }
        public List<PTXboPropertySurveyClientInputs> getMSPropertySurvey(PTXboPropertyConditionModel objmodel)
        {
            return _UnitOfWork.ClientIndexRepository.getMSPropertySurvey(objmodel);
        }
        public List<PTXboClientCreditTransaction> GetClientCreditDetails(int clientID)
        {
            return _UnitOfWork.ClientIndexRepository.GetClientCreditDetails(clientID);
        }
        public List<PTXboBPPPropertyValues> getMainScreenBPPPropertyValues(PTXboBPPPropertyValues objmodel)
        {
            return _UnitOfWork.ClientIndexRepository.getMainScreenBPPPropertyValues(objmodel);
        }
        public List<PTXboILHearingRemarks> GetILHearingRemarks(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetILHearingRemarks(model);
        } 
        public List<PTXboILAgentRemarks> GetILAgentRemarks(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetILAgentRemarks(model);
        }
        public PTXboILHearingDetails GetILHearingDetails(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetILHearingDetails(model);
        }
        public DataSet GetDMflag(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetDMflag(model);
        }
        public List<PTXboPOAReceivedDateHistory> getPOAReceivedDateHistory(int ClientID)
        {
            return _UnitOfWork.ClientIndexRepository.getPOAReceivedDateHistory(ClientID);
        }
        public List<PTXboILHearingRemarks> GetIOWAHearingRemarks(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetIOWAHearingRemarks(model);
        }
        public List<PTXboILAgentRemarks> GetIOWAAgentRemarks(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetIOWAAgentRemarks(model);
        }
        public PTXboILHearingDetails GetIOWAHearingDetails(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetIOWAHearingDetails(model);
        }
        public PTXboGAHearingDetails GetGAHearingDetails(PTXboParameters model)
        {
            return _UnitOfWork.ClientIndexRepository.GetGAHearingDetails(model);
        }
    }
}
