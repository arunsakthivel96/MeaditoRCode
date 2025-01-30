using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using Spartaxx.DataObjects.NHibernate.DataObjects;
using Spartaxx.BusinessObjects.Litigation;
using Spartaxx.BusinessObjects.GAHearingDetails;

namespace Spartaxx.DataAccess
{
    public class ClientIndexRepository : IClientIndexRepository
    {
        private readonly DapperConnection _Connection;
        public ClientIndexRepository(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        public PXTboListDefinition getListDefinitions(PXTboListDefinition objListDefinition)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                //factory.CreateParameters(1); /* We need to set parameter count based on SP parameter count. */

                /* Add parameters */
                _hashtable.Add("@TokenID", objListDefinition.TokenID);

                var dsResult = _Connection.SelectDataSet(StoredProcedureNames.usp_get_AllReqListDefinitions,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                //return result;

                /* Get required result from SP */
                //DataSet dsResult = factory.ExecuteDataSet(System.Data.CommandType.StoredProcedure, PTXdoStoredProcedureNames.usp_get_AllReqListDefinitions);

                if (dsResult.Tables.Count > 1)
                {
                    /* Last table contains the table names. */
                    List<string> tableNames = dsResult.Tables[dsResult.Tables.Count - 1].Rows[0][0].ToString().Split(',').ToList<string>();
                    tableNames.Remove(string.Empty);

                    int iTableCount = dsResult.Tables.Count - 1;

                    /* To find index of the datatable in dataset */
                    int iCount = 0;

                    /* Process the loop until iTableCount gets zero */
                    while (iTableCount > 0)
                    {
                        /* Process the loop until all the tables bind to their List<> */
                        foreach (string tableName in tableNames)
                        {
                            if (tableName.ToUpper().Trim() == "LUC TYPE TRIAL-REPORT")
                            {
                            }
                            switch (tableName.ToUpper().Trim())
                            {
                                /* Check the "tableName" matches the "case name" */
                                case "CLIENT SOURCE":
                                    /* If tableName matches case then
                                            Convert datatable to List<>
                                            Assign the List<> to corresponding List<>
                                       End */
                                    objListDefinition.ClientSource = dsResult.Tables[iCount].ToCollection<PTXdoClientSource>();
                                    break;

                                case "CLIENTSOURCENCS":

                                    objListDefinition.ClientSource = dsResult.Tables[iCount].ToCollection<PTXdoClientSource>();
                                    break;

                                case "ACCOUNT SOURCE":

                                    objListDefinition.AccountSource = dsResult.Tables[iCount].ToCollection<PTXdoAccountSource>();
                                    break;

                                case "ACCOUNT REASON":

                                    objListDefinition.AccountReason = dsResult.Tables[iCount].ToCollection<PTXdoAccountReason>();
                                    break;

                                case "PROTEST REASON":

                                    objListDefinition.ProtestReason = dsResult.Tables[iCount].ToCollection<PTXdoProtestReason>();
                                    break;

                                case "PRIMARY AGENT":
                                    objListDefinition.PrimaryAgent = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;

                                case "SALES AGENT":
                                    objListDefinition.SalesAgent = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;

                                case "CONTACT AGENT":
                                    objListDefinition.ContactAgent = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;

                                case "CLIENT CLASSFICATION TYPE":
                                    objListDefinition.ClientClassficationType = dsResult.Tables[iCount].ToCollection<PTXdoClientClassficationType>();
                                    break;

                                case "CLIENT SETUP METHOD":
                                    objListDefinition.ClientSetupMethod = dsResult.Tables[iCount].ToCollection<PTXdoClientSetupMethod>();
                                    break;

                                case "COUNTY":
                                    objListDefinition.County = dsResult.Tables[iCount].ToCollection<PTXdoCounty>();
                                    break;

                                case "CORE COUNTY":
                                    objListDefinition.CoreCounty = dsResult.Tables[iCount].ToCollection<PTXdoCounty>();
                                    break;

                                case "ISSUE TYPE":
                                    objListDefinition.IssueType = dsResult.Tables[iCount].ToCollection<PTXdoDAIssueType>();
                                    break;

                                case "CAF REASON CODE":
                                    objListDefinition.CAFReasonCode = dsResult.Tables[iCount].ToCollection<PTXdoCAFReasonCode>();
                                    break;

                                case "CAF LETTER TYPE":
                                    objListDefinition.CAFLetterType = dsResult.Tables[iCount].ToCollection<PTXdoCAFLetterType>();
                                    break;

                                case "MANUAL PROTEST COUNTY":
                                    objListDefinition.ManualProtestCounty = dsResult.Tables[iCount].ToCollection<PTXdoCounty>();
                                    break;

                                case "HEARING TYPE":
                                    objListDefinition.HearingType = dsResult.Tables[iCount].ToCollection<PTXdoHearingType>();
                                    break;

                                case "HEARING AGENT":
                                    objListDefinition.HearingAgent = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;

                                case "HEARING AGENT INACTIVE":
                                    objListDefinition.HearingAgentInActive = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;

                                case "HEARING RESOLUTION":
                                    objListDefinition.HearingResolution = dsResult.Tables[iCount].ToCollection<PTXdoHearingResolution>();
                                    break;

                                case "HEARING RESULT REASON CODE":
                                    objListDefinition.HearingResultReasonCodes = dsResult.Tables[iCount].ToCollection<PTXdoHearingResultsReasonCode>();
                                    break;

                                case "HEARING STATUS":
                                    objListDefinition.HearingStatus = dsResult.Tables[iCount].ToCollection<PTXdoHearingStatus>();
                                    break;

                                case "HEARING DISMISSAL AUTH STATUS":
                                    objListDefinition.HearingDismissalAuthStatus = dsResult.Tables[iCount].ToCollection<PTXdoHearingDismissalAuthStatus>();
                                    break;

                                case "NOTICE DEFECT REASON CODE":
                                    objListDefinition.NoticeDefectCodes = dsResult.Tables[iCount].ToCollection<PTXdoDocumentDefectCodes>();
                                    break;

                                case "HEARING RESULT RPT":
                                    objListDefinition.HearingResultRpt = dsResult.Tables[iCount].ToCollection<PTXdoHearingResultReport>();
                                    break;

                                case "HEARINGCONFIGURATION":
                                    objListDefinition.HearingResultRpt = dsResult.Tables[iCount].ToCollection<PTXdoHearingResultReport>();
                                    break;

                                case "TAX YEAR":
                                    objListDefinition.TaxYear = dsResult.Tables[iCount].ToCollection<PTXdoYearlyHearingDetails>();
                                    break;

                                case "WITHDRAWAL STATUS":
                                    objListDefinition.WithdrawalStatus = dsResult.Tables[iCount].ToCollection<PTXdoStatus>();
                                    break;

                                case "NOTICE DEFECT LETTER TYPE":
                                    objListDefinition.NoticeDefectLetterType = dsResult.Tables[iCount].ToCollection<PTXdoNoticeDefectLetterType>();
                                    break;

                                case "HB LUC SELECTION TYPE":
                                    objListDefinition.HBLUCSelectionType = dsResult.Tables[iCount].ToCollection<PTXdoHBLUCSelectionType>();
                                    break;

                                case "HB GROUP TYPE":
                                    objListDefinition.HBGroupType = dsResult.Tables[iCount].ToCollection<PTXdoHBGroupType>();
                                    break;

                                case "OCA LUC TYPE":
                                    objListDefinition.OCALUCType = dsResult.Tables[iCount].ToCollection<PTXdoOCALUCType>();
                                    break;

                                case "HEARING BATCH TYPE":
                                    objListDefinition.HearingBatchType = dsResult.Tables[iCount].ToCollection<PTXdoHearingBatchType>();
                                    break;
                                //CAF Status for auto assigning contains Assign to CAF Audit User, Notice Period Completed, CAF Audit Completed, CAF Audit on Hold
                                case "CAF STATUS FOR AUTO ASSIGNING":
                                    objListDefinition.CAFStatus = dsResult.Tables[iCount].ToCollection<PTXdoClientProcessingStatus>();
                                    break;

                                case "NOTICE QUEUE":
                                    objListDefinition.NoticeQueue = dsResult.Tables[iCount].ToCollection<PTXdoNoticeQueue>();
                                    break;

                                case "BATCH PRIORITY":
                                    objListDefinition.BatchPriority = dsResult.Tables[iCount].ToCollection<PTXdoBatchPriority>();
                                    break;

                                case "ALL AGENTS":
                                    objListDefinition.AllAgents = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;

                                case "CITY":
                                    objListDefinition.City = dsResult.Tables[iCount].ToCollection<PTXdoCity>();
                                    break;

                                case "STATE":
                                    objListDefinition.State = dsResult.Tables[iCount].ToCollection<PTXdoState>();
                                    break;

                                case "COUNTRY":
                                    objListDefinition.Country = dsResult.Tables[iCount].ToCollection<PTXdoCountry>();
                                    break;

                                case "FINANCIAL CODES":
                                    objListDefinition.FinancialCodes = dsResult.Tables[iCount].ToCollection<PTXdoFinancialCodes>();
                                    break;

                                case "LUC TYPE":
                                    objListDefinition.LUCTYPE = dsResult.Tables[iCount].ToCollection<Spartaxx.BusinessObjects.PTXboLUC>();
                                    break;

                                case "LUC TYPE TRIAL-REPORT":
                                    objListDefinition.OCALUC = dsResult.Tables[iCount].ToCollection<PTXdoOCALUC>();
                                    break;
                                case "PRIORTAXYEAR":
                                    objListDefinition.PriorTaxYear = dsResult.Tables[iCount].ToCollection<PTXdoYearlyHearingDetails>();
                                    break;
                                case "VALUEFOR TAXYEAR":
                                    objListDefinition.ValueforYear = dsResult.Tables[iCount].ToCollection<PTXdoYearlyHearingDetails>();
                                    break;

                                case "HNDEPROCESSSTATUS":
                                    objListDefinition.DEHearingNoticeProcessStatus = dsResult.Tables[iCount].ToCollection<PTXboHNDEProcessStatus>();
                                    break;

                                case "CS TASK SEVERITY":
                                    objListDefinition.CSTaskSeverity = dsResult.Tables[iCount].ToCollection<PTXboCSTaskSeverity>();
                                    break;

                                case "CS TASK TYPE":
                                    objListDefinition.CSTaskType = dsResult.Tables[iCount].ToCollection<PTXboCSTaskTypes>();
                                    break;

                                case "CS TASK STATUS":
                                    objListDefinition.CSTaskStatus = dsResult.Tables[iCount].ToCollection<PTXboCSTaskStatus>();
                                    break;

                                case "CS REP":
                                    objListDefinition.CSREP = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;

                                case "PROBLEM A OF A":
                                    objListDefinition.ProblemAofAtype = dsResult.Tables[iCount].ToCollection<PTXdoProblemAofAtype>();
                                    break;

                                case "DELIVERY METHOD":
                                    objListDefinition.DeliveryMethod = dsResult.Tables[iCount].ToCollection<PTXdoDeliveryMethod>();
                                    break;

                                case "CLIENT DOCUMENT TYPES":
                                    objListDefinition.ClientDocumentType = dsResult.Tables[iCount].ToCollection<PTXdoClientDocumentType>();
                                    break;

                                case "AOFA SEND LETTER":
                                    objListDefinition.ProblemAofALetterType = dsResult.Tables[iCount].ToCollection<PTXdoProblemAofALetterType>();
                                    break;

                                case "DOCUMENT DEFECT RESOLUTION":
                                    objListDefinition.DefectResolution = dsResult.Tables[iCount].ToCollection<PTXdoDefectResolution>();
                                    break;

                                case "AOFA DOCUMENT TYPE":
                                    objListDefinition.AofADocumentType = dsResult.Tables[iCount].ToCollection<PTXdoAofADocumentType>();
                                    break;

                                case "CORRECTION MOTION HEARING TYPE":
                                    objListDefinition.CMHearingType = dsResult.Tables[iCount].ToCollection<PTXdoHearingType>();
                                    break;

                                case "FCISTATUS":
                                    objListDefinition.FCIStatus = dsResult.Tables[iCount].ToCollection<PTXdoFCIAccountCurrentYearStatusType>();
                                    break;

                                case "ALLCOUNTY":
                                    objListDefinition.County = dsResult.Tables[iCount].ToCollection<PTXdoCounty>();
                                    break;

                                case "TAXBILLAUDIT COUNTY":
                                    objListDefinition.County = dsResult.Tables[iCount].ToCollection<PTXdoCounty>();
                                    break;

                                case "TAXBILL REPORTTYPE":
                                    objListDefinition.ServiceDocument = dsResult.Tables[iCount].ToCollection<PTXdoServiceDocument>();
                                    break;

                                case "AOFA DOCUMENT TYPE CONFIGURATION":
                                    objListDefinition.AofADocumentTypeConfiguration = dsResult.Tables[iCount].ToCollection<PTXdoAofADocumentType>();
                                    break;

                                case "BPPDOCUMENTSTATUS":
                                    objListDefinition.BPPDocumentStatus = dsResult.Tables[iCount].ToCollection<PTXdoStatus>();
                                    break;

                                case "TBADOCUMENTGENERATIONSTATUS":
                                    objListDefinition.TBADocumentGenerationStatus = dsResult.Tables[iCount].ToCollection<PTXdoDocumentGenerationStatus>();
                                    break;

                                case "SERVICENAMES":
                                    objListDefinition.ServiceNames = dsResult.Tables[iCount].ToCollection<PTXdoServiceNames>();
                                    break;

                                case "SERVERNAMES":
                                    objListDefinition.ServerNames = dsResult.Tables[iCount].ToCollection<PTXdoServerNames>();
                                    break;

                                case "HB201 SCHEDULE TYPE":
                                    objListDefinition.Hb201ScheduledType = dsResult.Tables[iCount].ToCollection<PTXdoParameters>();
                                    break;

                                case "HB201 REQUEST TYPE":
                                    objListDefinition.HB201RequsetType = dsResult.Tables[iCount].ToCollection<PTXdoParameters>();
                                    break;

                                case "WEEK":
                                    objListDefinition.DaysForWeek = dsResult.Tables[iCount].ToCollection<PTXdoParameters>();
                                    break;

                                case "CLIENT DELIVERY METHOD":
                                    objListDefinition.DeliveryMethod = dsResult.Tables[iCount].ToCollection<PTXdoDeliveryMethod>();
                                    break;
                                case "INTERNALGROUPNAME":
                                    objListDefinition.InternalEmailGroup = dsResult.Tables[iCount].ToCollection<PTXdoInternalEmailgroup>();
                                    break;
                                case "HEARING TRACKING CODES":
                                    objListDefinition.HearingTrackingCodes = dsResult.Tables[iCount].ToCollection<PTXdoHearingTrackingCodes>();
                                    break;
                                case "TERM EXPIRY ACTION":
                                    objListDefinition.TermExpiryAction = dsResult.Tables[iCount].ToCollection<PTXdoTermExpiryAction>();
                                    break;
                                case "TERMSTYPE":
                                    objListDefinition.TermsType = dsResult.Tables[iCount].ToCollection<PTXdoTermsType>();
                                    break;
                                case "GROUPTYPE":
                                    objListDefinition.GroupType = dsResult.Tables[iCount].ToCollection<PTXdoGroupType>();
                                    break;
                                case "SETUPTYPE":
                                    objListDefinition.CommonDataSource = dsResult.Tables[iCount].ToCollection<PTXboComboDataSource>();
                                    break;
                                case "CLIENTPACKAGETYPE":
                                    objListDefinition.clientPackageType = dsResult.Tables[iCount].ToCollection<PTXdoClientPackageType>();
                                    break;
                                case "NEWCLIENTDOCUMENTTYPES":
                                    objListDefinition.NewClientDocumentTypes = dsResult.Tables[iCount].ToCollection<PTXdoClientDocumentType>();
                                    break;
                                case "AGREEMENT STATUS CODES":
                                    objListDefinition.AgreementStatusCodes = dsResult.Tables[iCount].ToCollection<PTXdoAgreementStatusCodes>();
                                    break;
                                case "DOCUMENT DEFECT CODES":
                                    objListDefinition.DocumentDefectCodes = dsResult.Tables[iCount].ToCollection<PTXdoDocumentDefectCodes>();
                                    break;
                                case "CAF TYPE":
                                    objListDefinition.CAFType = dsResult.Tables[iCount].ToCollection<PTXdoCAFType>();
                                    break;
                                case "PROBLEMAGREEMENTSTATUSCODES":
                                    objListDefinition.ProblemAgreementDefectCodes = dsResult.Tables[iCount].ToCollection<PTXdoDocumentDefectCodes>();
                                    break;
                                case "OCA LUC TYPE WITH ALL":
                                    objListDefinition.OCALUCType = dsResult.Tables[iCount].ToCollection<PTXdoOCALUCType>();
                                    break;
                                case "CALL DISPOSTION TYPE":
                                    objListDefinition.CSCallDispostionType = dsResult.Tables[iCount].ToCollection<PTXdoCallDispostionType>();
                                    break;
                                case "CS AGENT GROUP TYPE":
                                    objListDefinition.CSAgentGroupType = dsResult.Tables[iCount].ToCollection<PTXdoCSAgentGroupType>();
                                    break;

                                case "CLIENTDOCUMENTTYPESFORDAISSUE":
                                    objListDefinition.ClientDocumentTypesForDAIssue = dsResult.Tables[iCount].ToCollection<PTXdoClientDocumentType>();
                                    break;

                                case "NEWCLIENTTAXYEAR": // Package 1 Codes tab Tax year
                                    objListDefinition.TaxYear = dsResult.Tables[iCount].ToCollection<PTXdoYearlyHearingDetails>();
                                    break;
                                case "FINANCIALSTAXYEAR": // Package 7 Financial Tax year
                                    objListDefinition.TaxYear = dsResult.Tables[iCount].ToCollection<PTXdoYearlyHearingDetails>();
                                    break;

                                case "PRIORTAXYEARS": // common Prior Tax years
                                    objListDefinition.PriorTaxYear = dsResult.Tables[iCount].ToCollection<PTXdoYearlyHearingDetails>();
                                    break;

                                case "DOCUMENTTYPEFORINVOICEADJUSTMEMT":
                                    objListDefinition.ClientDocumentType = dsResult.Tables[iCount].ToCollection<PTXdoClientDocumentType>();
                                    break;

                                case "INVOICEADJUSTMENTTYPES":
                                    objListDefinition.InvoiceAdjustmentTypes = dsResult.Tables[iCount].ToCollection<PTXdoInvoiceAdjustmentManualType>();
                                    break;

                                case "INVOICEINTERESTRATES":
                                    objListDefinition.InvoiceInterestRates = dsResult.Tables[iCount].ToCollection<PTXdoInvoiceInterestRates>();
                                    break;

                                case "PASTDUENONCONTACTCODE":
                                    objListDefinition.NonContactCode = dsResult.Tables[iCount].ToCollection<PTXdoNonContactCode>();
                                    break;
                                case "PASTDUECOLLECTIONSTATUSCODE":
                                    objListDefinition.CollectionStatusCode = dsResult.Tables[iCount].ToCollection<PTXdoCollectionStatusCode>();
                                    break;

                                case "PASTDUEDELIVARYMETHOD":
                                    objListDefinition.InvoiceDeliveryMethod = dsResult.Tables[iCount].ToCollection<PTXdoInvoiceDeliveryMethod>();
                                    break;
                                case "PASTDUECONDITIONVALUE":
                                    objListDefinition.ConditionValue = dsResult.Tables[iCount].ToCollection<PTXdoConditionValue>();
                                    break;
                                case "INVOICEAGENTS":
                                    objListDefinition.InvoiceAgents = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "CREDIT CARD TYPE":
                                    objListDefinition.PaymentType = dsResult.Tables[iCount].ToCollection<PTXdoPaymentType>();
                                    break;
                                case "VETDISABILITYRATING":
                                    objListDefinition.VetDisabilityRating = dsResult.Tables[iCount].ToCollection<PTXdoVetDisabilityRating>();
                                    break;
                                case "NTSADCOUNTY":
                                    objListDefinition.NTSADCounty = dsResult.Tables[iCount].ToCollection<PTXdoCounty>();
                                    break;
                                case "COLLECTIONLETTERTYPE":
                                    objListDefinition.CommonDataSource = dsResult.Tables[iCount].ToCollection<PTXboComboDataSource>();
                                    break;
                                case "REPORTS":
                                    objListDefinition.Reports = dsResult.Tables[iCount].ToCollection<PTXdoReports>();
                                    break;
                                case "FCI DEFECT REASON CODE": //Package 7 - FCI Defect Reason Code - Added by Rajeswari
                                    objListDefinition.DocumentDefectCodes = dsResult.Tables[iCount].ToCollection<PTXdoDocumentDefectCodes>();
                                    break;
                                case "PROJECT NAME":
                                    objListDefinition.ProjectName = dsResult.Tables[iCount].ToCollection<PTXdoProject>();
                                    break;
                                case "OCALUC":
                                    objListDefinition.OCALUC = dsResult.Tables[iCount].ToCollection<PTXdoOCALUC>();
                                    break;
                                case "CMREQUESTFOR"://Package 7 - Correction Motion - Added by Rajeswari
                                    objListDefinition.CMRequestFor = dsResult.Tables[iCount].ToCollection<PTXdoParameters>();
                                    break;
                                case "REQUESTED BY":
                                    objListDefinition.RequestedBy = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;

                                //**Report Token
                                case "REPORT CATEGORY":
                                    break;
                                case "REPORT COUNTY"://Report Hearing Status
                                    objListDefinition.County = dsResult.Tables[iCount].ToCollection<PTXdoCounty>();
                                    break;
                                case "REPORT LUC":
                                    if (objListDefinition.TokenID != 49)
                                    {
                                        objListDefinition.OCALUC = dsResult.Tables[iCount].ToCollection<PTXdoOCALUC>();
                                    }
                                    break;
                                case "REPORTCLASSIFICATION":
                                    objListDefinition.ClientClassficationType = dsResult.Tables[iCount].ToCollection<PTXdoClientClassficationType>();
                                    break;
                                case "REPORT INFORMAL ASSIGNED AGENTS":
                                    objListDefinition.AssignedAgents = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "REPORT FORMAL ASSIGNED AGENTS":
                                    objListDefinition.FormalAssignedAgents = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "REPORT HEARING STATUS"://Report Hearing Status
                                    objListDefinition.HearingStatus = dsResult.Tables[iCount].ToCollection<PTXdoHearingStatus>();
                                    break;
                                case "REPORT STATE":
                                    objListDefinition.State = dsResult.Tables[iCount].ToCollection<PTXdoState>();
                                    break;
                                case "REPORT HEARINGTYPE":
                                    objListDefinition.HearingType = dsResult.Tables[iCount].ToCollection<PTXdoHearingType>();
                                    break;
                                case "REPORT PROJECT NAMES":
                                    objListDefinition.ProjectName = dsResult.Tables[iCount].ToCollection<PTXdoProject>();
                                    break;
                                case "REPORT ACCOUNTSTATUS":
                                    objListDefinition.AccountStatus = dsResult.Tables[iCount].ToCollection<PTXdoAccountStatus>();
                                    break;
                                case "REPORT FINANCIAL CODES":
                                    objListDefinition.FinancialCodes = dsResult.Tables[iCount].ToCollection<PTXdoFinancialCodes>();
                                    break;


                                case "REPORT INFORMAL HEARINGAGENT":
                                    objListDefinition.InFormalHearingAgents = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "REPORT FORMAL HEARINGAGENT":
                                    objListDefinition.FormalHearingAgents = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "REPORT PROTEST CODES":
                                    objListDefinition.ProtestCodeValues = dsResult.Tables[iCount].AsEnumerable().Select(t => new PTXdoProtestCodeValues()
                                    {
                                        ProtestCodeValuesId = Convert.ToInt32(t.ItemArray[0]),
                                        ProtestCodeValues = Convert.ToString(t.ItemArray[1])

                                    }).ToList<PTXdoProtestCodeValues>();

                                    break;
                                case "REPORT TAXYEAR":
                                    objListDefinition.TaxYear = dsResult.Tables[iCount].ToCollection<PTXdoYearlyHearingDetails>();
                                    break;

                                case "REPORT USERROLE MAPPING":
                                    objListDefinition.UserRoleMapping = dsResult.Tables[iCount].ToCollection<PTXdoUserRole>();
                                    break;

                                case "REPORT AGENT TYPE":
                                    objListDefinition.AgentType = dsResult.Tables[iCount].ToCollection<PTXdoAgentType>();
                                    break;

                                case "REPORTLITIGATIONCAUSE":
                                    objListDefinition.LitigationCauseNo = dsResult.Tables[iCount].ToCollection<PTXdoLitigation>();
                                    break;
                                case "REPORTLITIGATIONBATCHCAUSENO":
                                    objListDefinition.LitigationBatchCauseNo = dsResult.Tables[iCount].ToCollection<PTXdoLitigation>();
                                    break;
                                case "REPORTLITIGATIONBATCHCAUSENAME":
                                    objListDefinition.LitigationBatchCauseName = dsResult.Tables[iCount].ToCollection<PTXdoLitigation>();
                                    break;
                                case "REPORTLITIGATIONSTATUS":
                                    objListDefinition.LitigationStatus = dsResult.Tables[iCount].ToCollection<PTXdoLitigationStatus>();
                                    break;
                                case "REPORTARBITRATIONSTATUS":
                                    objListDefinition.ArbitrationStatus = dsResult.Tables[iCount].ToCollection<PTXdoArbitrationStatus>();
                                    break;
                                //**Report Token

                                case "ARBITRATION AGENT":
                                    objListDefinition.ArbitrationAgent = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "ARBITRATION AGENT INACTIVE":
                                    objListDefinition.ArbitrationAgentInActive = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "DENIEDRESON":
                                    objListDefinition.DeniedReason = dsResult.Tables[iCount].ToCollection<PtxdoArbitrationDenialReason>();
                                    break;
                                case "ARBITRATION STATUS":
                                    objListDefinition.ArbitrationStatus = dsResult.Tables[iCount].ToCollection<PTXdoArbitrationStatus>();
                                    break;
                                case "ARBITRATION REFERRAL":
                                    objListDefinition.ArbitrationReferralMethod = dsResult.Tables[iCount].ToCollection<PTXdoArbitrationReferralMethod>();
                                    break;
                                case "ARBITRATIONDOCUMENTTYPE":
                                    objListDefinition.ArbitrationDocumentType = dsResult.Tables[iCount].ToCollection<PTXdoClientDocumentType>();
                                    break;
                                case "ARBITRATION DEFECT":
                                    objListDefinition.ArbitrationDefectCodes = dsResult.Tables[iCount].ToCollection<PTXdoDocumentDefectCodes>();
                                    break;
                                case "PROBLEM AOFA":
                                    objListDefinition.ProblemAofACodes = dsResult.Tables[iCount].ToCollection<PTXdoProblemAofACode>();
                                    break;
                                case "LITIGATION STATUS":
                                    objListDefinition.LitigationStatus = dsResult.Tables[iCount].ToCollection<PTXdoLitigationStatus>();
                                    break;
                                case "SECLITIGATION STATUS":
                                    objListDefinition.SecLitigationStatus = dsResult.Tables[iCount].ToCollection<PTXdoLitigationStatus>();
                                    break;
                                case "SCHEDULESTATUS":
                                    objListDefinition.ScheduleStatus = dsResult.Tables[iCount].ToCollection<PTXdoLitigationStatus>();
                                    break;
                                case "SECSCHEDULESTATUS":
                                    objListDefinition.SecScheduleStatus = dsResult.Tables[iCount].ToCollection<PTXdoLitigationStatus>();
                                    break;
                                case "RESOLUTION":
                                    objListDefinition.Resolution = dsResult.Tables[iCount].ToCollection<PTXdoLitigationStatus>();
                                    break;
                                case "SECRESOLUTION":
                                    objListDefinition.SecResolution = dsResult.Tables[iCount].ToCollection<PTXdoLitigationStatus>();
                                    break;
                                case "OFFICE":
                                    objListDefinition.CountyGroupName = dsResult.Tables[iCount].ToCollection<PTXdoCountyGroup>();
                                    break;
                                case "EXPERT ASSIGNED":
                                    objListDefinition.ExpertAssigned = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "EXPERT ASSIGNED INACTIVE":
                                    objListDefinition.ExpertAssignedInactive = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "SECEXPERT ASSIGNED":
                                    objListDefinition.SecExpertAssigned = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "ANALYST ASSIGNED":
                                    objListDefinition.AnalystAssigned = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "SECANALYST ASSIGNED":
                                    objListDefinition.SecAnalystAssigned = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "LOCATION":
                                    objListDefinition.Location = dsResult.Tables[iCount].ToCollection<PTXdoLitLocation>();
                                    break;
                                case "SECLOCATION":
                                    objListDefinition.SecLocation = dsResult.Tables[iCount].ToCollection<PTXdoLitLocation>();
                                    break;
                                case "ARBSTATUSFORUNDERREVIEW":
                                    objListDefinition.ArbStatusforUnderReview = dsResult.Tables[iCount].ToCollection<PTXdoParameters>();
                                    break;
                                case "PROPERTYTYPE":
                                    objListDefinition.PropertyType = dsResult.Tables[iCount].ToCollection<PTXdoOCALUCType>();
                                    break;
                                case "PENDINGLITIGATIONSTATUS":
                                    objListDefinition.PendingLitigationStatus = dsResult.Tables[iCount].ToCollection<PTXdoPendingLitigationStatus>();
                                    break;
                                case "LITTAXYEAR":
                                    objListDefinition.LitTaxYear = dsResult.Tables[iCount].ToCollection<PTXdoTaxYear>();
                                    break;
                                case "LITDASHASSIGNEDTO":
                                    objListDefinition.LitDashAssignedTo = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "LITDASHASSIGTO":
                                    objListDefinition.LitDashAssigTo = dsResult.Tables[iCount].ToCollection<PTXdoUserRole>();
                                    break;
                                case "LITMONTHS":
                                    objListDefinition.LitMonths = dsResult.Tables[iCount].ToCollection<PTXdoLitMonth>();
                                    break;
                                case "LITDSNSTATUS":
                                    objListDefinition.LitigationDNSStatus = dsResult.Tables[iCount].ToCollection<PTXdoLitigationStatus>();
                                    break;
                                case "DND CODES":
                                    objListDefinition.DNDCodes = dsResult.Tables[iCount].ToCollection<PTXdoDNDCodes>();
                                    break;
                                //Added by Boopathi
                                case "PRIORITYDEFAULTVALUE":
                                    try
                                    {
                                        objListDefinition.PriorityDefaultValue = Convert.ToInt32(dsResult.Tables[iCount].Rows[0][0]);
                                    }
                                    catch
                                    {
                                        objListDefinition.PriorityDefaultValue = 0;
                                    }
                                    break;
                                case "AOFASTATUS":
                                    objListDefinition.AofAStatus = dsResult.Tables[iCount].ToCollection<PTXdoAofAStatus>();
                                    break;
                                case "LITEXPERTASSIGNSUMMARYTAXYEAR":
                                    objListDefinition.LitExpertAssignSummaryTaxYear = dsResult.Tables[iCount].ToCollection<PTXdoYearlyHearingDetails>();
                                    break;

                                case "REPORTPESEXPERTASSIGNED":
                                    objListDefinition.ReportPESExpertAssigned = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                case "REPORT CONTACT AGENT":
                                    objListDefinition.ContactAgent = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "FINANCIALS RECEIVED":
                                    objListDefinition.FinancialReceived = dsResult.Tables[iCount].ToCollection<PTXdoFinancialCodes>();
                                    break;
                                case "FINANCIALS STATUS":
                                    objListDefinition.FinancialStatus = dsResult.Tables[iCount].ToCollection<PTXdoFinancialCodes>();
                                    break;
                                case "REPORTFINANCIALSTATUS":
                                    objListDefinition.FinancialStatus = dsResult.Tables[iCount].ToCollection<PTXdoFinancialCodes>();
                                    break;
                                case "INVOICE MANUAL TYPE":
                                    objListDefinition.InvoiceManualType = dsResult.Tables[iCount].ToCollection<PTXdoInvoicePaymentMethod>();
                                    break;
                                case "BLANKET AUTHORITY STATUS":
                                    objListDefinition.BlanketAuthenticationStatus = dsResult.Tables[iCount].ToCollection<PTXdoBlankAuthStatus>();
                                    break;
                                case "EMAILSTATUS":
                                    objListDefinition.EmailStatus = dsResult.Tables[iCount].ToCollection<PTXdoEmailStatus>();
                                    break;
                                case "DISASTER":
                                    objListDefinition.Disaster = dsResult.Tables[iCount].ToCollection<PTXdoDisaster>();
                                    break;

                                case "BAUPDATETYPE":
                                    objListDefinition.BAUpdateType = dsResult.Tables[iCount].ToCollection<PTXdoBAUpdateType>();
                                    break;
                                //Added By:Kowsalya
                                case "INVOICEADJDOCUMENTTYPE":
                                    objListDefinition.InvoiceAdjustmentDocType = dsResult.Tables[iCount].ToCollection<PTXboInvoiceAdjustmentDocType>();
                                    break;
                                //Added by:Balaji.P
                                case "FILTER TYPE":
                                    objListDefinition.FilterType = dsResult.Tables[iCount].ToCollection<PTXdoCSFilterType>();
                                    break;
                                case "AOFA TYPE":
                                    objListDefinition.AofAType = dsResult.Tables[iCount].ToCollection<PTXdoCSFilterType>();
                                    break;
                                // added by Preethi 38928 and 38929
                                case "FLOODLITIGATIONTYPE":
                                    objListDefinition.FloodLitTypeID = dsResult.Tables[iCount].ToCollection<PTXdoFloodLitigationType>();
                                    break;
                                case "SECFLOODLITIGATIONTYPE":
                                    objListDefinition.SecFloodLitTypeID = dsResult.Tables[iCount].ToCollection<PTXdoFloodLitigationType>();
                                    break;
                                case "APPRAISER NAME":
                                    objListDefinition.Appraiserid = dsResult.Tables[iCount].ToCollection<PTXdoAppraiser>();
                                    break;
                                case "SECAPPRAISER NAME":
                                    objListDefinition.SecAppraiserid = dsResult.Tables[iCount].ToCollection<PTXdoAppraiser>();
                                    break;
                                //Lawsuit types for Litigation secondary
                                case "LAWSUIT TYPES":
                                    objListDefinition.LawsuitTypes = dsResult.Tables[iCount].ToCollection<PTXdoLawsuitType>();
                                    break;
                                //Added by Balaji
                                case "EXPRESS AGENT":
                                    objListDefinition.LitExpressAgent = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
                                    break;
                                // Added by saranya TFS:34013 Start
                                case "AgentCode":
                                    objListDefinition.AgentCode = dsResult.Tables[iCount].ToCollection<PTXdoSubAgentCode>();
                                    break;
                                case "SubAgentCode":
                                    objListDefinition.SubAgent = dsResult.Tables[iCount].ToCollection<PTXdoSubAgentCode>();
                                    break;
                                // Added by saranya TFS:34013 End
                                case "BOUNCED FLAG":
                                    objListDefinition.BouncedFlag = dsResult.Tables[iCount].ToCollection<PTXdoBouncedFlag>();
                                    break;
                                //Added by Mano
                                case "HEARING LEVEL":
                                    objListDefinition.HearingLevel = dsResult.Tables[iCount].ToCollection<PTXdoHearingLevel>();
                                    break;
                                case "ARBITRATIONCONDITIONVALUE":
                                    objListDefinition.ConditionValue = dsResult.Tables[iCount].ToCollection<PTXdoConditionValue>();
                                    break;
                                case "ASSIGNEDCONCIERGE":
                                    objListDefinition.AssignedConcierge = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "CONCIERGEDOCUMENTSTATUS":
                                    objListDefinition.ConciergeDocumentStatus = dsResult.Tables[iCount].ToCollection<PTXdoConciergeDocumentStatus>();
                                    break;
                                case "CONCIERGESUBMITTEDBY":
                                    objListDefinition.ConciergeSubmittedBy = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "PROPERTYOWNEDCREATEDBY":
                                    objListDefinition.Createdby = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "PROPERTYOWNEDREACTIVATEDBY":
                                    objListDefinition.Reactivatedby = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "CONCIERGENOOFPROPERTIESADDED":
                                    objListDefinition.ConciergeNoOfPropertiesAdded = dsResult.Tables[iCount].ToCollection<PTXdoConciergeNoOfPropertiesAdded>();
                                    break;
                                case "SALES IMPORT CATEGORY":
                                    objListDefinition.SalesImportCategory = dsResult.Tables[iCount].ToCollection<PTXboSalesLeadCategory>();
                                    break;
                                case "SALES DISPOSITION":
                                    objListDefinition.CallDispostionType = dsResult.Tables[iCount].ToCollection<PTXdoCallDispostionType>();
                                    break;
                                case "PHONE TYPE":
                                    objListDefinition.PhoneType = dsResult.Tables[iCount].ToCollection<PTXdoPhoneType>();
                                    break;
                                case "SALES CATEGORY":
                                    objListDefinition.SalesCategory = dsResult.Tables[iCount].ToCollection<PTXdoSalesLeadCategory>();
                                    break;
                                case "TOWNSHIP":
                                    objListDefinition.Township = dsResult.Tables[iCount].ToCollection<PTXdoTownship>();
                                    break;
                                case "APPOINTMENT STATUS":
                                    objListDefinition.AppointmentStatus = dsResult.Tables[iCount].ToCollection<PTXdoAppointmentStatus>();
                                    break;
                                case "APPOINTMENT MODE":
                                    objListDefinition.AppointmentMode = dsResult.Tables[iCount].ToCollection<PTXdoAppointmentMode>();
                                    break;
                                case "HOTEL SALES AGENT":
                                    objListDefinition.HotelSalesAgent = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                                case "PFBA SALES AGENT":
                                    objListDefinition.PFBASalesAgent = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                    break;
                            }
                            /* Remove processed table */
                            tableNames.Remove(tableName);
                            break;
                        }

                        /* Increment the iCount to proceed next table in dataset */
                        iCount++;

                        /* Decrement the iTableCount to proceed next table */
                        iTableCount--;
                    }
                }

                if (objListDefinition.AccountId > 0)
                    objListDefinition.IsOutOfTexasProperty = IsOutOfTexasProperty(objListDefinition.AccountId);

                return objListDefinition;
            }
            catch (Exception ex)
            {
                objListDefinition.errorstring = ex.Message;
                return objListDefinition;
            }
        }

        private bool IsOutOfTexasProperty(int AccountId)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                DataTable dtResultset = new DataTable();
                string YearlyHearingDetailsId = string.Empty;
                _hashtable.Add("@AccountId", AccountId);
                var data = _Connection.ExecuteScalar(StoredProcedureNames.usp_IsOutOfTexasProperty,
                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return Convert.ToBoolean(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  This method is used to get the Project Details based on ClientId
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="objProject"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<PTXboProject> getProjectDetails(int ClientId)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                DataTable dtResultset = new DataTable();
                string YearlyHearingDetailsId = string.Empty;
                _hashtable.Add("@ClientId", ClientId);
                return _Connection.Select<PTXboProject>(StoredProcedureNames.usp_getProjectDetails,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PTXboClientDeliveryMethods getClientDeliveryMethods(int ClientID)
        {
            PTXboClientDeliveryMethods objClientDeliveryMethods = new PTXboClientDeliveryMethods();

            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientID", ClientID);
                return _Connection.Select<PTXboClientDeliveryMethods>(StoredProcedureNames.usp_getClientAvailableDeliveryMethods,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get Account Counts in MainScreen based on ClientNumber.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="msHearingDetails"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public DataTable getAccountDetailsCount(string ClientNumber)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientNumber", ClientNumber);

                var dsResult = _Connection.SelectDataSet(StoredProcedureNames.USP_GetClientAccountSummary_MainScreen,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return dsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboDashboardAccountCount getAccountDetailsCount_Dashboard(int ClientID)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientID", ClientID);

                return _Connection.Select<PTXboDashboardAccountCount>("USP_GetClientAccountSummary_Dashboard",
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To get Account Details in MainScreen based on ClientNumber.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="msHearingDetails"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<PTXboAccountDetails> getAccountsForBinding(PTXboGetAccountsForBinding model)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXboAccountDetails> objAccountDetailsList = new List<PTXboAccountDetails>();
            try
            {
                _hashtable.Add("@ClientNumber", model.ClientNumber);
                _hashtable.Add("@Flag", model.LinkField);

                return _Connection.Select<PTXboAccountDetails>(StoredProcedureNames.usp_GetAccountDetailsforProperty,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboMSPropertyConditionQuestionnaire> getMSPropertyCondition(PTXboPropertyConditionModel objmodel)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@AccountId", objmodel.AccountId);
                _hashtable.Add("@TaxYear", objmodel.TaxYear);
                _hashtable.Add("@DisasterId", objmodel.DisasterId);
                return _Connection.Select<PTXboMSPropertyConditionQuestionnaire>(StoredProcedureNames.usp_getMSPropertyConditionQuestions,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getParamValue(int paramID)
        {
            Hashtable _hashtable = new Hashtable();
            string taxyear = string.Empty;
            //return objDetails;
            try
            {
                _hashtable.Add("@Param_ID", paramID);
                return _Connection.ExecuteScalar(StoredProcedureNames.usp_GET_ParamValue,
                                      _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getClientNumber(string ClientNumber)
        {
            Hashtable _hashtable = new Hashtable();
            string taxyear = string.Empty;
            //return objDetails;
            try
            {
                return _Connection.SelectDataSet("select ClientNumber from PTAX_DonotContactClients where ClientNumber='" + ClientNumber + "'",
                                      _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getResultState(int ClientID)
        {
            Hashtable _hashtable = new Hashtable();
            string taxyear = string.Empty;
            //return objDetails;
            try
            {
                return _Connection.SelectDataSet("select s.StateId from PTAX_Account ac join ptax_county co on co.countyid = ac.CountyId join ptax_state s on s.stateID = co.StateId where clientid ='" + ClientID + "' group by s.stateid",
                                      _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public PTXboClientAndAccountDetails getClientAndAccountDetails(PTXboClientAndAccountDetails objDetails)
        //{            
        //    Hashtable _hashtable = new Hashtable();
        //    string taxyear = string.Empty;
        //    //return objDetails;
        //    try
        //    {
        //        _hashtable.Add("@Param_ID", 1);
        //        taxyear = _Connection.ExecuteScalar(StoredProcedureNames.usp_GET_ParamValue,
        //                              _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx);

        //        objDetails.objClient.clientSetupParameters = new PTXdoClientSetupParameters();
        //        objDetails.objClient.clientSetupParameters.TaxyearSelected = taxyear;


        //        DataSet dsResult = new DataSet();
        //        DataTable dsResult_State = new DataTable();
        //        Hashtable _hashtableclt = new Hashtable();

        //        dsResult = _Connection.SelectDataSet("select ClientNumber from PTAX_DonotContactClients where ClientNumber='" + Convert.ToString(objDetails.objClient.ClientNumber + "'"),
        //                                _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx);
        //        /* Check Client is exists. If so return true else return false*/
        //        if (dsResult == null || dsResult.Tables[0].Rows.Count == 0)
        //        {
        //            objDetails.objClient.DoNotContacts = false;
        //        }
        //        else
        //        {
        //            objDetails.objClient.DoNotContacts = true;
        //        }

        //        dsResult_State = _Connection.SelectDataSet("select s.StateId from PTAX_Account ac join ptax_county co on co.countyid = ac.CountyId join ptax_state s on s.stateID = co.StateId where clientid ='" + objDetails.ClientID + "' group by s.stateid",
        //                                _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
        //        /* Check Client is exists. If so return true else return false*/
        //        List<PTXdoState> LState = new List<PTXdoState>();
        //        if (dsResult_State != null || dsResult_State.Rows.Count != 0)
        //        {
        //            LState = (from DataRow dr in dsResult_State.Rows
        //                      select new PTXdoState()
        //                      {
        //                          stateID = Convert.ToInt32(dr["StateId"]),
        //                      }).ToList();

        //            foreach (var item in LState)
        //            {
        //                if (item.stateID == 44)
        //                {
        //                    objDetails.objClient.Stateid = true;
        //                }
        //                else
        //                {
        //                    objDetails.objClient.Stateid = false;
        //                }

        //            }
        //        }

        //        _hashtable = new Hashtable();
        //        /* Get Account Details */
        //        List<PTXdoAccount> AccountList = _Connection.Select<PTXdoAccount>("Select * from PTAX_Account with(nolock) Where ClientId=" + objDetails.ClientID,
        //                               _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx);
        //        //var AccountList = Repository<PTXdoAccount>.GetQuery().Where(a => a.Client.clientId == ClientID).ToList();
        //        if (AccountList != null)
        //            objDetails.objAccountList = AccountList;

        //        PTXdoIndexedClientDocument objDocumentDetails = _Connection.Select<PTXdoIndexedClientDocument>("Select * from PTAX_IndexedClientDocument with(nolock) where ClientId=" + objDetails.ClientID + " and IndexClientDocumentId=" + Enumerators.PTXenumClientDocumentTypes.Agreements.GetId(),
        //                               _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        objDetails.ClientDocument = objDocumentDetails;
        //        return objDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public PTXboClientGroupContact getClientGroupContact(PTXboParameters model)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientID", model.Value2);
                _hashtable.Add("@groupId", model.Value1);

                return _Connection.Select<PTXboClientGroupContact>(StoredProcedureNames.usp_getMainScreenClientGroupContactDetails,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboMainScreenFieldNames> getMainScreenFieldName(PTXboParameters model)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@Userroleid", model.Value1);
                _hashtable.Add("@userId", model.Value2);
                _hashtable.Add("@Subtab", model.Value3);

                return _Connection.Select<PTXboMainScreenFieldNames>(StoredProcedureNames.usp_getMainScreenFieldname,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet LoadDropDownForTermsFilter(int clienID)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientID", clienID);
                return _Connection.SelectDataSet(StoredProcedureNames.Sp_DropDownForTermsFilter,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboClientGiftcardHistory> getGiftCardSentDateHistory(int ClientId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientId", ClientId);
                DataTable dtResult = new DataTable();
                //dtResult = dbAccess.ExecuteDataTable(System.Data.CommandType.StoredProcedure, PTXdoStoredProcedureNames.usp_getGiftCardSentDateHistory);
                return _Connection.Select<PTXboClientGiftcardHistory>(StoredProcedureNames.usp_getGiftCardSentDateHistory,
                                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get the list of terms associated with client
        /// 
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="clientTermsList"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<PTXboClientTermsDetails> getClientTerms(PTXboClientTerms model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", model.ClientId);
                _hashtable.Add("@AccountNumber", model.AccountNumber);
                _hashtable.Add("@Project", model.Project);
                return _Connection.Select<PTXboClientTermsDetails>(StoredProcedureNames.usp_getClientTerms,
                                     _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get the list of term level history
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="termlevelHistoryList"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<PTXboTermLevelHistory> getTermLevelHistory(int GroupId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@GroupId", GroupId);
                return _Connection.Select<PTXboTermLevelHistory>(StoredProcedureNames.usp_getTermLevelHistory,
                                     _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method usd to get client group details and recently send package information
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public List<PTXboClientGroupRecentPackageDetails> getClientGroupRecentPackageDetails(int clientId, int stateID, string County)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@clientId", clientId);
                _hashtable.Add("@stateID", stateID);
                _hashtable.Add("@County", County);

                var clientGrouRecentPackageTermsDetails = _Connection.Select<PTXdtGroupandResendPackageDetails>(StoredProcedureNames.USP_getClientGroupRecentPackageDetails,
                                     _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                return clientGrouRecentPackageTermsDetails.GroupBy(a => new { a.GroupId, a.GroupName, a.ClientId, a.AgreementStatusCode, a.AgreementStatusDescription, a.recentPackageSent, a.sentBy, a.sentDate }).Select(b => new PTXboClientGroupRecentPackageDetails
                {
                    groupId = b.Key.GroupId ?? 0,
                    groupName = b.Key.GroupName,
                    CanEnableGroup = !b.All(g => g.CanEnableGroup == false),
                    client = new PTXdoClient() { clientId = b.Key.ClientId ?? 0 },
                    AgreementStatusCode = b.Key.AgreementStatusCode,
                    AgreementStatusDescription = b.Key.AgreementStatusDescription,
                    recentPackageSent = b.Key.recentPackageSent,
                    sentBy = b.Key.sentBy,
                    sentDate = b.Key.sentDate,
                    termsList = clientGrouRecentPackageTermsDetails.Where(c => c.GroupId == b.Key.GroupId).Select(d => new PTXdoTerms
                    {
                        termsID = d.TermsId,
                        termsType = new PTXdoTermsType() { Termstypeid = d.TermsTypeId, TermsType = d.TermsType },
                        EffectiveDate = d.EffectiveDate,
                        ExpiryDate = d.ExpiryDate,
                        remarks = d.Remarks,
                        UpdatedBy = new PTXdoUser() { Userid = d.UpdatedBy },
                        UpdatedDateTime = d.UpdatedDateTime,
                        Contingency = d.Contingency,
                        FlatFee = d.FlatFee,
                        ExpiryRemarks = d.ExpiryRemarks,
                        ContingencyAfterExpiry = d.ContingencyAfterExpiry,
                        FlatFeeAfterExpiry = d.FlatFeeAfterExpiry,
                        TermExpiryAction = new PTXdoTermExpiryAction() { TermExpiryActionID = d.TermExpiryActionID, TermExpiryAction = d.TermExpiryAction }
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboClientPackageAccountDetails> GetClientPackageAccountDetails(int clientId, int stateID, string County, int packageTypeId, string packageTypeMode)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@clientId", clientId);
                _hashtable.Add("@stateID", stateID);
                _hashtable.Add("@packageTypeId", packageTypeId);
                _hashtable.Add("@packageTypeMode", packageTypeMode);
                _hashtable.Add("@County", County);

                return _Connection.Select<PTXboClientPackageAccountDetails>(StoredProcedureNames.USP_getClientPackageAccountDetails,
                                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboClientContactLog> getClientContactLog(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@clientID", Convert.ToInt32(model.ClientID));
                _hashtable.Add("@CallType", model.CallType);
                _hashtable.Add("@CallDateFrom", model.CallDateFrom);
                _hashtable.Add("@CallDateTo", model.CallDateTo);
                _hashtable.Add("@IsAllLog", model.IsAllLog);
                return _Connection.Select<PTXboClientContactLog>(StoredProcedureNames.usp_mainScreengetClientContactLog,
                                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getClientAndDeliveryMethod(string ClientNumber)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientNumber", ClientNumber);
                DataSet dtResult = _Connection.SelectDataSet(StoredProcedureNames.usp_getClientAndDeliveryMethod,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                return dtResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCorrespondenceLogOld> GetOldCorrLog(string ClientNumber)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientNumber", ClientNumber);

                return _Connection.Select<PTXboCorrespondenceLogOld>(StoredProcedureNames.USP_Get_OldCorrLog,
                                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return new List<PTXboCorrespondenceLogOld>();
        }
        public DataSet getClientConsolidationClientDetails(string ClientNumber)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientNumber", ClientNumber);
                return _Connection.SelectDataSet(StoredProcedureNames.usp_getClientConsolidationClientDetails,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCAFHistory(int ClientId)
        {
            DataSet dsresult = new DataSet();
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientId", ClientId);

                return _Connection.SelectDataSet(StoredProcedureNames.usp_getCAFHistory,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboTaxBillClientSearchResult> getTaxBillAuditClients(PTXboTaxBillClientSearchRequest objPTXboTaxBillClientSearch)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", objPTXboTaxBillClientSearch.ClientID);
                _hashtable.Add("@CountyID", objPTXboTaxBillClientSearch.CountyID);
                _hashtable.Add("@Taxyear", objPTXboTaxBillClientSearch.TaxYear);
                _hashtable.Add("@AccountNumber", objPTXboTaxBillClientSearch.AccountNumber);
                _hashtable.Add("@PropertyAddress", objPTXboTaxBillClientSearch.PropertyAddress);
                _hashtable.Add("@ProjectId", objPTXboTaxBillClientSearch.ProjectID);
                _hashtable.Add("@Flag", objPTXboTaxBillClientSearch.Flag);
                _hashtable.Add("@TaxBillAudit", objPTXboTaxBillClientSearch.TaxBillAuditIds);
                _hashtable.Add("@OwnerName", objPTXboTaxBillClientSearch.OwnerName);

                return _Connection.Select<PTXboTaxBillClientSearchResult>(StoredProcedureNames.usp_getTaxBillAuditClients,
                                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getPTAXCAFHistory(string ClientNumber)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientNumber", ClientNumber);
                return _Connection.SelectDataSet(StoredProcedureNames.usp_getExistingCAFHistory,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboAccountNotesProperty> GetAccountNotes(int accountId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", accountId);
                return _Connection.Select<PTXboAccountNotesProperty>(StoredProcedureNames.usp_GetAccountNotes,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboAccountNotesProperty
                                   {
                                       AccountNotes = a.AccountNotes,
                                       UpdatedBy = a.UpdatedBy,
                                       UpdatedDatetime = a.UpdatedDatetime ?? null,


                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboHearingDetailsRemarks> GetHearingRemarks(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@TaxYear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);

                return _Connection.Select<PTXboHearingDetailsRemarks>(StoredProcedureNames.usp_GetHearingRemarksforProperty,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboHearingDetailsRemarks
                                   {
                                       HearingDetailsId = a.HearingDetailsId,
                                       Remarks = a.Remarks,

                                       UpdatedDatetime = a.UpdatedDatetime ?? null,
                                       Username = a.Username

                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboAofARemarks> getAofARemarks(int accountId)
        {
            try
            {
                List<PTXboAofARemarks> AofARemarksList = new List<PTXboAofARemarks>();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", accountId);
                DataTable dtResultset = _Connection.SelectDataSet(StoredProcedureNames.usp_SPARTAXX_SEL_AofARemarks,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                var dtAofARemarks = dtResultset.ToCollection<PTXboAofARemarksLoad>();

                AofARemarksList = dtAofARemarks.Select(a => new PTXboAofARemarks
                {
                    AofARemarksID = a.AofARemarksId,
                    AofAID = new PTXdoAofA { AofAId = a.AofAId },
                    remarks = a.Remarks,
                    Disposition = a.Disposition,
                    accountId = new PTXdoAccount { AccountId = a.AccountId },
                    updatedBy = new PTXdoUser { Userid = a.UpdatedBy, Username = a.UpdatedName },
                    updatedDatetime = a.UpdatedDatetime,
                    UserName = a.UserName
                }).ToList();

                return AofARemarksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboAgentRemarks> GetAgentRemarks(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                return _Connection.Select<PTXboAgentRemarks>(StoredProcedureNames.usp_GetAgentRemarksforAccount,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboAgentRemarks
                                   {
                                       AgentRemarksId = a.AgentRemarksId,
                                       AgentRemarks = a.AgentRemarks,

                                       UpdatedDatetime = a.UpdatedDatetime ?? null,
                                       Username = a.Username
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboPropertyRemarks> GetPropertyRemarks(int accountId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", accountId);
                return _Connection.Select<PTXboPropertyRemarks>(StoredProcedureNames.usp_GetAccountDetailAllRemarks,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboPropertyRemarks
                                   {
                                       RemarkTitle = a.RemarkTitle,
                                       Remarks = a.Remarks,
                                       RemarkId = a.RemarkId,
                                       UpdatedDatetime = a.UpdatedDatetime ?? null,
                                       UpdatedBy = a.UpdatedBy

                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BusinessObjects.ViewModels.PTXMainScreenInvoiceDetailResult> getInvoicecollectionDetails(PTXboParameters model)
        {
            //Added by Saravanan.S tfs id:56254
            string spName = string.Empty;
            int paymentstatus = model.Value2 ?? 0;
            //Ends here.

            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@Clientid", model.Value1);
                _hashtable.Add("@PaymentStatus", model.Value2);
                //Added by SaravananS.tfs id:56254
                if (paymentstatus == 0)
                {
                    spName = StoredProcedureNames.usp_getMainScreeninvoicedetails_AllInvoices;
                }
                else if (paymentstatus == 1 || paymentstatus == 6)
                {
                    spName = StoredProcedureNames.usp_getMainScreeninvoicedetails_NotPaidInvoices;
                }
                else if (paymentstatus == 2)
                {
                    spName = StoredProcedureNames.usp_getMainScreeninvoicedetails_PaidInvoices;
                }
                //Added by SaravananS. tfs id:60677
                else if (paymentstatus == 9)
                {
                    spName = StoredProcedureNames.usp_getMainScreeninvoicedetails_AdjustedInvoices;
                }
                //Ends here.

                else if (paymentstatus == 4 || paymentstatus == 6)
                {
                    spName = StoredProcedureNames.usp_getMainScreeninvoicedetails_PastDueInvoices;

                }
                else if (paymentstatus == 5)
                {
                    spName = StoredProcedureNames.usp_getMainScreeninvoicedetails_TotalInvoices;

                }
                else if (paymentstatus == 7 || paymentstatus == 8)
                {
                    spName = StoredProcedureNames.usp_getMainScreeninvoicedetails_CreditInvoices;

                }
                //Ends here.

                //StoredProcedureNames.usp_getMainScreeninvoicedetails
                return _Connection.Select<BusinessObjects.ViewModels.PTXMainScreenInvoiceDetailResult>(spName,
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboInvoiceAdjustmentRequestDetails> GetInvoiceAdjRequestDetails(int Invoiceid)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@Invoiceid", Invoiceid);
                return _Connection.Select<PTXboInvoiceAdjustmentRequestDetails>(StoredProcedureNames.usp_GetInvoiceAdjustmentRequestDetails,
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCCPayment> GetCCPaymentDetails(int ClientID)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", ClientID);
                return _Connection.Select<PTXboCCPayment>(StoredProcedureNames.usp_GetCCPaymentDetailsBasedOnInvoice,
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboClientCreditTransaction> GetClientCreditDetails(int ClientID)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", ClientID);
                return _Connection.Select<PTXboClientCreditTransaction>(StoredProcedureNames.usp_get_MainScreenClientCreditDetails,
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboInvoiceAdjustmentDetails> GetInvoiceAdjustmentAuditDetails(int Invoiceid)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@Invoiceid", Invoiceid);
                return _Connection.Select<PTXboInvoiceAdjustmentDetails>(StoredProcedureNames.usp_GetInvoiceAdjustmentAuditDetails,
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboInvoiceExemptionJurisdictionHistory> GetInvoiceExemptionJurisdictionHistory(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@InvoiceAdjustmentId", model.Value1);
                _hashtable.Add("@RequestTypeid", model.Value2);
                return _Connection.Select<PTXboInvoiceExemptionJurisdictionHistory>(StoredProcedureNames.usp_GetInvoiceExemptionJurisdictionHistory,
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCSPaymentPlanDetails> getClientPaymentPlan(string invoiceIdList)
        {
            try
            {
                // Need to verify on this data with output.            
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@invoiceIdList", invoiceIdList);
                List<PTXboCSPaymentPlanDetails> objClientPaymentplan = new List<PTXboCSPaymentPlanDetails>();
                objClientPaymentplan = _Connection.Select<PTXboCSPaymentPlanDetails>(StoredProcedureNames.usp_getClientPaymentPlan,
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                objClientPaymentplan = objClientPaymentplan.GroupBy(a => new
                {
                    a.PaymentPlanInvoicesId,
                    a.PaymentPlanId,
                    a.ClientId,
                    a.PlanLevel,
                    a.StartDate,
                    a.EndDate,
                    a.TotalBalanceDue,
                    a.FreezeInterest,
                    a.NumberOfMonths,
                    a.AmountDueEachMonth,
                    a.PlanStartDate,
                    a.PlanEndDate,
                    a.PlanStatusId,
                    a.Waiveinterest,
                    a.IsActive,
                    a.PaymentStatus,
                    a.CreatedBy,
                    a.CreatedDateTime
                }).Select(b => new PTXboCSPaymentPlanDetails
                {
                    PaymentPlanInvoicesId = b.Key.PaymentPlanInvoicesId,
                    PaymentPlanId = b.Key.PaymentPlanId,
                    ClientId = b.Key.ClientId,
                    PlanLevel = b.Key.PlanLevel,
                    StartDate = b.Key.StartDate,
                    EndDate = b.Key.EndDate,
                    TotalBalanceDue = Convert.ToDecimal(b.Key.TotalBalanceDue),
                    FreezeInterest = b.Key.FreezeInterest,
                    NumberOfMonths = b.Key.NumberOfMonths,
                    AmountDueEachMonth = Convert.ToDecimal(b.Key.AmountDueEachMonth),
                    PlanStartDate = b.Key.PlanStartDate,
                    PlanEndDate = b.Key.PlanEndDate,
                    PlanStatusId = b.Key.PlanStatusId,
                    Waiveinterest = b.Key.Waiveinterest,
                    IsActive = b.Key.IsActive,
                    PaymentStatus = b.Key.PaymentStatus,
                    CreatedBy = b.Key.CreatedBy,
                    CreatedDateTime = b.Key.CreatedDateTime,
                    InvoiceList = string.Join(",", objClientPaymentplan.Where(k => k.PaymentPlanId == b.Key.PaymentPlanId).Select(c => c.InvoiceId).Distinct().ToList()),
                    PaymentDetails = objClientPaymentplan.Where(m => m.PaymentPlanId == b.Key.PaymentPlanId).Select(o => new PTXboPaymentDetails
                    {
                        id = o.PaymentPlanId,
                        //PaymentAmount = o.PaymentAmount,
                        //PaymentReceivedDate = o.PaymentReceivedDate
                    }).ToList()

                }).DistinctBy(n => n.PaymentPlanId).ToList();
                return objClientPaymentplan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCSSettlementPlanDetails> getClientSettlementPlan(string invoiceIdList)
        {
            try
            {
                // Need to verify on this data with output.            
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@invoiceIdList", invoiceIdList);
                List<PTXboCSSettlementPlanDetails> objClientSettlementPlan = new List<PTXboCSSettlementPlanDetails>();
                objClientSettlementPlan = _Connection.Select<PTXboCSSettlementPlanDetails>(StoredProcedureNames.usp_getClientSettlememtPlan,
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                objClientSettlementPlan = objClientSettlementPlan.GroupBy(a => new
                {
                    a.SettlementPlanInvoicesId,
                    a.SettlementPlanId,
                    a.ClientId,
                    a.PlanLevel,
                    a.StartDate,
                    a.EndDate,
                    a.TotalBalanceDue,
                    a.SettlementAmount,
                    a.SavingInPercentage,
                    a.SettlementDueDate,
                    a.PlanStatusId,
                    a.IsActive,
                    a.CreatedBy,
                    a.CreatedDateTime
                }).Select(b => new PTXboCSSettlementPlanDetails
                {
                    SettlementPlanInvoicesId = b.Key.SettlementPlanInvoicesId,
                    SettlementPlanId = b.Key.SettlementPlanId,
                    ClientId = b.Key.ClientId,
                    PlanLevel = b.Key.PlanLevel,
                    StartDate = b.Key.StartDate,
                    EndDate = b.Key.EndDate,
                    TotalBalanceDue = Convert.ToDecimal(b.Key.TotalBalanceDue),
                    SettlementAmount = Convert.ToDecimal(b.Key.SettlementAmount),
                    SavingInPercentage = b.Key.SavingInPercentage,
                    SettlementDueDate = b.Key.SettlementDueDate,
                    PlanStatusId = b.Key.PlanStatusId,
                    IsActive = b.Key.IsActive,
                    CreatedBy = b.Key.CreatedBy,
                    CreatedDateTime = b.Key.CreatedDateTime,
                    InvoiceList = string.Join(",", objClientSettlementPlan.Where(k => k.SettlementPlanId == b.Key.SettlementPlanId).Select(c => c.InvoiceId).ToList()),
                    PaymentDetails = objClientSettlementPlan.Where(m => m.SettlementPlanId == b.Key.SettlementPlanId).Select(o => new PTXboPaymentDetails
                    {
                        id = o.SettlementPlanId
                        //PaymentAmount = o.PaymentAmount,
                        //PaymentReceivedDate = o.PaymentReceivedDate
                    }).ToList()

                }).DistinctBy(n => n.SettlementPlanId).ToList();
                return objClientSettlementPlan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCSPaymentPlanDetails> getClientPaymentPlanHistory(int invoiceId)
        {
            try
            {
                // Need to verify on this data with output.            
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@invoiceIdList", invoiceId);
                List<PTXboCSPaymentPlanDetails> objClientPaymentplan = new List<PTXboCSPaymentPlanDetails>();
                objClientPaymentplan = _Connection.Select<PTXboCSPaymentPlanDetails>("usp_getClientPaymentPlanHistory",
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                objClientPaymentplan = objClientPaymentplan.GroupBy(a => new
                {
                    a.PaymentPlanInvoicesId,
                    a.PaymentPlanId,
                    a.ClientId,
                    a.PlanLevel,
                    a.StartDate,
                    a.EndDate,
                    a.TotalBalanceDue,
                    a.FreezeInterest,
                    a.NumberOfMonths,
                    a.AmountDueEachMonth,
                    a.PlanStartDate,
                    a.PlanEndDate,
                    a.PlanStatusId,
                    a.Waiveinterest,
                    a.IsActive,
                    a.PaymentStatus,
                    a.CreatedBy,
                    a.CreatedDateTime
                }).Select(b => new PTXboCSPaymentPlanDetails
                {
                    PaymentPlanInvoicesId = b.Key.PaymentPlanInvoicesId,
                    PaymentPlanId = b.Key.PaymentPlanId,
                    ClientId = b.Key.ClientId,
                    PlanLevel = b.Key.PlanLevel,
                    StartDate = b.Key.StartDate,
                    EndDate = b.Key.EndDate,
                    TotalBalanceDue = Convert.ToDecimal(b.Key.TotalBalanceDue),
                    FreezeInterest = b.Key.FreezeInterest,
                    NumberOfMonths = b.Key.NumberOfMonths,
                    AmountDueEachMonth = Convert.ToDecimal(b.Key.AmountDueEachMonth),
                    PlanStartDate = b.Key.PlanStartDate,
                    PlanEndDate = b.Key.PlanEndDate,
                    PlanStatusId = b.Key.PlanStatusId,
                    Waiveinterest = b.Key.Waiveinterest,
                    IsActive = b.Key.IsActive,
                    PaymentStatus = b.Key.PaymentStatus,
                    CreatedBy = b.Key.CreatedBy,
                    CreatedDateTime = b.Key.CreatedDateTime,
                    InvoiceList = string.Join(",", objClientPaymentplan.Where(k => k.PaymentPlanId == b.Key.PaymentPlanId).Select(c => c.InvoiceId).Distinct().ToList()),
                    PaymentDetails = objClientPaymentplan.Where(m => m.PaymentPlanId == b.Key.PaymentPlanId).Select(o => new PTXboPaymentDetails
                    {
                        id = o.PaymentPlanId,
                        //PaymentAmount = o.PaymentAmount,
                        //PaymentReceivedDate = o.PaymentReceivedDate
                    }).ToList()

                }).DistinctBy(n => n.PaymentPlanId).ToList();
                return objClientPaymentplan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCSSettlementPlanDetails> getClientSettlementPlanHistory(int invoiceId)
        {
            try
            {
                // Need to verify on this data with output.            
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@invoiceId", invoiceId);
                List<PTXboCSSettlementPlanDetails> objClientSettlementPlan = new List<PTXboCSSettlementPlanDetails>();
                objClientSettlementPlan = _Connection.Select<PTXboCSSettlementPlanDetails>("usp_getClientSettlememtPlanHistory",
                                  _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                objClientSettlementPlan = objClientSettlementPlan.GroupBy(a => new
                {
                    a.SettlementPlanInvoicesId,
                    a.SettlementPlanId,
                    a.ClientId,
                    a.PlanLevel,
                    a.StartDate,
                    a.EndDate,
                    a.TotalBalanceDue,
                    a.SettlementAmount,
                    a.SavingInPercentage,
                    a.SettlementDueDate,
                    a.PlanStatusId,
                    a.IsActive,
                    a.CreatedBy,
                    a.CreatedDateTime
                }).Select(b => new PTXboCSSettlementPlanDetails
                {
                    SettlementPlanInvoicesId = b.Key.SettlementPlanInvoicesId,
                    SettlementPlanId = b.Key.SettlementPlanId,
                    ClientId = b.Key.ClientId,
                    PlanLevel = b.Key.PlanLevel,
                    StartDate = b.Key.StartDate,
                    EndDate = b.Key.EndDate,
                    TotalBalanceDue = Convert.ToDecimal(b.Key.TotalBalanceDue),
                    SettlementAmount = Convert.ToDecimal(b.Key.SettlementAmount),
                    SavingInPercentage = b.Key.SavingInPercentage,
                    SettlementDueDate = b.Key.SettlementDueDate,
                    PlanStatusId = b.Key.PlanStatusId,
                    IsActive = b.Key.IsActive,
                    CreatedBy = b.Key.CreatedBy,
                    CreatedDateTime = b.Key.CreatedDateTime,
                    InvoiceList = string.Join(",", objClientSettlementPlan.Where(k => k.SettlementPlanId == b.Key.SettlementPlanId).Select(c => c.InvoiceId).ToList()),
                    PaymentDetails = objClientSettlementPlan.Where(m => m.SettlementPlanId == b.Key.SettlementPlanId).Select(o => new PTXboPaymentDetails
                    {
                        id = o.SettlementPlanId
                        //PaymentAmount = o.PaymentAmount,
                        //PaymentReceivedDate = o.PaymentReceivedDate
                    }).ToList()

                }).DistinctBy(n => n.SettlementPlanId).ToList();
                return objClientSettlementPlan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboFinancials> getFinancialDetails(int Accountid)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", Accountid);
                return _Connection.Select<PTXboFinancials>(StoredProcedureNames.usp_getFinancialDetails,
                                 _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDNDFlagStatus(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                return _Connection.SelectDataSet(StoredProcedureNames.USP_GetDNDStatus,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetAccountDetailFlags(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                DataTable dtResultset = _Connection.SelectDataSet(StoredProcedureNames.USP_GetAccountDetailFlags,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                if (dtResultset.Rows.Count > 0 && dtResultset != null)
                {
                    return Convert.ToString(dtResultset.Rows[0]["AccountFlagStatus"]);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboAccountSetupParameters getAccountSetupParameterDetails(int AccountID)
        {
            try
            {
                PTXboAccountSetupParameters objAccountSetupParameters = new PTXboAccountSetupParameters();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", AccountID);
                DataSet dsResult = _Connection.SelectDataSet(StoredProcedureNames.usp_getAccountSetupParamtersData,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                {
                    /* Convert datatable to out list */
                    objAccountSetupParameters = dsResult.Tables[0].ToCollection<PTXboAccountSetupParameters>().FirstOrDefault();
                    //log.WriteToEventLog("DS: getAccountSetupParameterDetails method List conversion END Seconds: " + TimeDiff, "Information");
                    if (dsResult.Tables[1] != null && dsResult.Tables[1].Rows.Count > 0)
                    {
                        /* Convert datatable to out list - Account Notes*/
                        objAccountSetupParameters.AccountNotes = dsResult.Tables[1].ToCollection<PTXboAccountNotes>();
                        //  log.WriteToEventLog("DS: getAccountSetupParameterDetails method List accountNotes conversion END Seconds: " + TimeDiff, "Information");
                    }
                }

                return objAccountSetupParameters;
                ///* PropSearchDS001: No records found */
                //PTXdsCommon.CurrentInstance.GetUserMessage("PropSearchDS001", out errorString);
                //return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboPropertyDetails getPropertyDetailsData(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.Value1);
                _hashtable.Add("@taxYear", model.Value2);

                return _Connection.Select<PTXboPropertyDetails>(StoredProcedureNames.usp_getPropertyDetailsData,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PTXboValueNotice getValueNoticeData(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.Value1);
                _hashtable.Add("@TaxYear", model.Value2);
                return _Connection.Select<PTXboValueNotice>(StoredProcedureNames.usp_getValueNoticeData,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboHearingDetails> getHearingDetailsData(PTXboParameters model)
        {
            try
            {
                List<PTXboHearingDetails> objHearingDetailList = new List<PTXboHearingDetails>();
                PTXboHearingDetails objHearingDetails = new PTXboHearingDetails();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.Value1);
                _hashtable.Add("@TaxYear", model.Value2);
                DataSet dsResult = _Connection.SelectDataSet(StoredProcedureNames.usp_getHearingDetailsData,
                     _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                objHearingDetailList = _Connection.Select<PTXboHearingDetails>(StoredProcedureNames.usp_getHearingDetailsData,
                     _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                {
                    /* Convert datatable to out list */
                    // Need to work on this
                    //objHearingDetailList = dsResult.Tables[0].ToCollection<PTXboHearingDetails>();

                    if (dsResult.Tables[1] != null && dsResult.Tables[1].Rows.Count > 0)
                    {
                        objHearingDetails.HearingDetailsRemarks = dsResult.Tables[1].ToCollection<PTXboHearingDetailsRemarks>();
                        foreach (PTXboHearingDetails HearingDetails in objHearingDetailList)
                        {
                            HearingDetails.HearingDetailsRemarks = objHearingDetails.HearingDetailsRemarks.Where(a => a.HearingDetailsId == HearingDetails.HearingDetailsId).ToList();
                        }
                    }
                }
                return objHearingDetailList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboHearingResults> getHearingResultData(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.Value1);
                _hashtable.Add("@Flag", model.Value4);
                _hashtable.Add("@PrevTaxYear", model.Value2);
                return _Connection.Select<PTXboHearingResults>(StoredProcedureNames.usp_getHearingResultData,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double getPriorYearTaxRateForSelectedAccount(PTXboParameters modelrate)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountNumber", modelrate.AccountNumber);
                _hashtable.Add("@PriorYear", modelrate.TaxYear);
                _hashtable.Add("@CountyId", modelrate.CountyId.Value);
                _hashtable.Add("@Accountid", modelrate.AccountId);
                DataTable dtResult = _Connection.SelectDataSet(StoredProcedureNames.usp_CalculatePriorYearTaxRatebasedonAccountJurisdiction,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                return Convert.ToDouble(dtResult.Rows[0]["TaxRate"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXdoAccountJurisdiction> getAccountJurisdiction(PTXboParameters modelrate)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@PropertyDetailsId", modelrate.AccountId);
                _hashtable.Add("@CountyId", modelrate.CountyId);
                _hashtable.Add("@AccountNumber", modelrate.AccountNumber);
                _hashtable.Add("@TaxYear", modelrate.TaxYear);
                return _Connection.Select<PTXdoAccountJurisdiction>(StoredProcedureNames.usp_PTAX_get_Jurisdiction,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXdoAccountJurisdiction> getAccountTaxRollJurisdiction(PTXboParameters modelrate)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@CountyId", modelrate.CountyId);
                _hashtable.Add("@Accountid", modelrate.AccountId);
                _hashtable.Add("@TaxYear", modelrate.TaxYear);
                return _Connection.Select<PTXdoAccountJurisdiction>(StoredProcedureNames.usp_GetAccountTaxRollJurisdiction,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCountyJurisdiction> getCountyJurisdiction(PTXboParameters modelrate)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@CountyId", modelrate.CountyId);
                _hashtable.Add("@PropertyDetailsId", modelrate.AccountId);
                return _Connection.Select<PTXboCountyJurisdiction>(StoredProcedureNames.usp_PTAX_get_CountyJurisdiction,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboFCIPropertyDetails getFCIPropertyDetails(PTXboFCIPropertyDetails propertyDetails)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", propertyDetails.AccountId);
                _hashtable.Add("@TaxYear", propertyDetails.TaxYear);

                DataSet dsResult = new DataSet();
                dsResult = _Connection.SelectDataSet(StoredProcedureNames.usp_getFCIPropertyDetails,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);


                var objpropertyDetails = dsResult.Tables[0].ToCollection<PTXboFCIPropertyDetails>();
                propertyDetails = objpropertyDetails.Where(a => a.AccountId == propertyDetails.AccountId).FirstOrDefault();

                if (dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                {
                    var dtPropertySurroundingAreaList = dsResult.Tables[1].ToCollection<PTXboPropertySurroundingArea>();
                    //List<PTXboPropertySurroundingArea> propertySurroundingArea = dtPropertySurroundingAreaList.Select(c => new PTXboPropertySurroundingArea
                    //{
                    //    PropertyCondition = c.PropertyCondition,
                    //    PropertyConstructionType = c.PropertyConstructionType,
                    //    RoofType = c.RoofType
                    //}).ToList();
                    foreach (var item in dtPropertySurroundingAreaList)
                    {
                        if (item.PropertyCondition == "Poor")
                        {
                            propertyDetails.Poor = true;
                        }
                        if (item.PropertyCondition == "Fair")
                        {
                            propertyDetails.Fair = true;
                        }
                        if (item.PropertyCondition == "Average")
                        {
                            propertyDetails.Average = true;
                        }
                        if (item.PropertyCondition == "Good")
                        {
                            propertyDetails.Good = true;
                        }
                        if (item.PropertyConstructionType == "Metal")
                        {
                            propertyDetails.Metal = true;
                        }
                        if (item.PropertyConstructionType == "Frame")
                        {
                            propertyDetails.Frame = true;
                        }
                        if (item.PropertyConstructionType == "Brick")
                        {
                            propertyDetails.Brick = true;
                        }
                        if (item.RoofType == "Flat")
                        {
                            propertyDetails.Flat = true;
                        }
                        if (item.RoofType == "Pitched")
                        {
                            propertyDetails.Pitched = true;
                        }
                    }
                }
                return propertyDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboFCIDataRemarks> GetFCIDataRemarksDetails(int PropertyDetailsId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@PropertyDetailsId", PropertyDetailsId);
                return _Connection.Select<PTXboFCIDataRemarks>(StoredProcedureNames.usp_GetFCIDataRemarksDetails,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool getmenuIsActivebasedOnUserRole(PTXbomenuIsActivebasedOnUserRole model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@userRoleID", model.UserRoleId);
                _hashtable.Add("@actionName", model.actionName);
                _hashtable.Add("@ControllerName", model.controllerName);
                _hashtable.Add("@MenuId", model.MenuId);

                string iresult = _Connection.ExecuteScalar(StoredProcedureNames.usp_getmenuIsActivebasedOnUserRoletemp,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                //factory.AddParameters(3, "@url", url);
                //object iresult = factory.ExecuteScalar(System.Data.CommandType.StoredProcedure, PTXdoStoredProcedureNames.usp_getmenuIsActivebasedOnUserRoletemp);
                if (iresult != null)
                {
                    if (iresult == "false")
                        return false;
                    else return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboACCLlogSearchResult> getACCLlogDetails(PTXboACCLlogSearchRequest objACCLlogSearchRequest)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientNumber", objACCLlogSearchRequest.ClientNumber);
                _hashtable.Add("@DeliveryMethodId", objACCLlogSearchRequest.DeliveryMethodID);
                _hashtable.Add("@DeliveryFromDate", objACCLlogSearchRequest.DeliveryFromDate);
                _hashtable.Add("@DeliveryToDate", objACCLlogSearchRequest.DeliveryToDate);
                _hashtable.Add("@IsDisplayOnlySuccessRecords", objACCLlogSearchRequest.IsDisplayOnlySuccessRecords);
                return _Connection.Select<PTXboACCLlogSearchResult>(StoredProcedureNames.usp_getACCLlogDetails,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getACCLlogDetailsNew(PTXboACCLlogSearchRequest objACCLlogSearchRequest)
        {
            try
            {
                DataSet dsResult = new DataSet();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientNumber", objACCLlogSearchRequest.ClientNumber);
                _hashtable.Add("@DeliveryMethodId", objACCLlogSearchRequest.DeliveryMethodID);
                _hashtable.Add("@DeliveryFromDate", objACCLlogSearchRequest.DeliveryFromDate);
                _hashtable.Add("@DeliveryToDate", objACCLlogSearchRequest.DeliveryToDate);
                _hashtable.Add("@IsDisplayOnlySuccessRecords", objACCLlogSearchRequest.IsDisplayOnlySuccessRecords);
                _hashtable.Add("@AccountNumber", objACCLlogSearchRequest.AccountNumber);
                _hashtable.Add("@ServicePackageId", objACCLlogSearchRequest.ServicePackageId);

                //return _Connection.Select<PTXboACCLlogSearchResult>(StoredProcedureNames.usp_getACCLlogDetails,
                //                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                dsResult = _Connection.SelectDataSet(StoredProcedureNames.usp_getACCLlogDetails,
                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int MainScreenupdateClientCodes(PTXboInsertUpdateClientCode model)
        {
            try
            {

                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientId", model.objClient.clientId);

                _hashtable.Add("@ClientNumber", model.objClient.ClientNumber);
                _hashtable.Add("@ClientName", model.objClient.ClientName);
                _hashtable.Add("@ClassifiationTypeID", model.objClient.ClassifiationTypeID);

                _hashtable.Add("@PrimaryAgentId", model.objClient.PrimaryAgentId);
                _hashtable.Add("@ContactAgentId", model.objClient.ContactAgentId);
                _hashtable.Add("@SalesAgentId", model.objClient.SalesAgentId);
                _hashtable.Add("@ClientSetupMethodId", model.objClient.ClientSetupMethodId);
                _hashtable.Add("@ClientSourceId", model.objClient.ClientSourceId);
                _hashtable.Add("@SpecialHandlingRequired", model.objClient.SpecialHandlingRequired);
                _hashtable.Add("@TPVRequired", model.objClient.TPVRequired);
                _hashtable.Add("@TPVCode", model.objClient.TPVCode);
                //_hashtable.Add("@StartDate", model.objClient.StartDate);
                //_hashtable.Add("@EndDate", model.objClient.EndDate);
                _hashtable.Add("@ClientStatusId", model.objClient.ClientStatusId);
                _hashtable.Add("@ClientProcessingStatusId", model.objClient.ClientProcessingStatusId);

                _hashtable.Add("@SpecialHandlingRequiredRemark", model.objClient.SpecialHandlingRequiredRemark);
                _hashtable.Add("@ReferralName", model.objClient.ReferralName);
                _hashtable.Add("@UpdatedByID", model.objClient.UpdatedByID);
                _hashtable.Add("@UpdatedDateTime", model.objClient.UpdatedDateTime);
                _hashtable.Add("@CreatedByID", model.objClient.CreatedByID);
                _hashtable.Add("@CreatedDateTime", model.objClient.CreatedDateTime);
                //_hashtable.Add("@LegalCollectionsFlag", model.objClient.LegalCollectionsFlag);
                //_hashtable.Add("@VERIFIEDUSER", model.objClient.VERIFIEDUSER);
                //_hashtable.Add("@VERIFIEDDATE", model.objClient.VERIFIEDDATE);
                //_hashtable.Add("@AUDITUSER", model.objClient.AUDITUSER);
                //_hashtable.Add("@AUDITDATE", model.objClient.AUDITDATE);
                //_hashtable.Add("@VerificationUserRoleID", model.objClient.VerificationUserRoleID);
                //_hashtable.Add("@AuditUserRoleID", model.objClient.AuditUserRoleID);
                //_hashtable.Add("@DonotAssign", model.objClient.DonotAssign);
                _hashtable.Add("@CreatedUserRoleID", model.objClient.CreatedUserRoleID);
                //_hashtable.Add("@NoncontactCode", model.objClient.NoncontactCode);
                _hashtable.Add("@CollectionAgentId", model.objClient.CollectionAgentId);
                //_hashtable.Add("@CollectionStatusCode", model.objClient.CollectionStatusCode);
                //_hashtable.Add("@PropertyTaxClientID", model.objClient.PropertyTaxClientID);
                _hashtable.Add("@BASentDate", model.objClient.BASentDate);
                _hashtable.Add("@BAReceivedDate", model.objClient.BAReceivedDate);
                _hashtable.Add("@BlankAuthID", model.objClient.BlankAuthID);
                _hashtable.Add("@GiftCardSentDate", model.objClient.GiftCardSentDate);
                _hashtable.Add("@isJudicial", model.objClient.isJudicial);
                _hashtable.Add("@SpecialHandlingforAgent", model.objClient.SpecialHandlingforAgent);
                _hashtable.Add("@SpecialHandlingforEscalation", model.objClient.SpecialHandlingforEscalation);
                //_hashtable.Add("@isValidBALitAgmt", model.objClient.isValidBALitAgmt);
                _hashtable.Add("@IsNextYearSignup", model.objClient.IsNextYearSignup);
                _hashtable.Add("@isRestrictedCommunication", model.objClient.isRestrictedCommunication);
                _hashtable.Add("@isHarveyLegalCollection", model.objClient.isHarveyLegalCollection);
                int ClientID = Convert.ToInt32(_Connection.ExecuteScalar(StoredProcedureNames.SaveOrUpdate_PTAX_Client,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));

                _hashtable = new Hashtable();
                _hashtable.Add("@TaxyearSelected", model.ClientSetupParameters.TaxyearSelected);
                if (model.objClient.clientId > 0)
                {
                    ClientID = model.objClient.clientId;
                }
                _hashtable.Add("@ClientId", ClientID);
                _hashtable.Add("@TaxBillAuditSelected", model.ClientSetupParameters.TaxBillAuditSelected);
                _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_ClientSetupParameters,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (model.ClientRemarks.Remarks != null)
                {
                    _hashtable = new Hashtable();
                    _hashtable.Add("@ClientId", model.ClientRemarks.ClientID);
                    _hashtable.Add("@Remarks", model.ClientRemarks.Remarks);
                    _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_ClientRemarks_Type,
                                            _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                }
                if (model.Account != null && model.Account.Count() > 0)
                {
                    foreach (PTXdoAccount acc in model.Account)
                    {
                        _hashtable = new Hashtable();
                        _hashtable.Add("@AccountId", acc.AccountId);
                        _hashtable.Add("@ClientId", acc.Client.clientId);
                        _hashtable.Add("@AccountNumber", acc.AccountNumber);
                        _hashtable.Add("@CountyId", acc.County.Countyid);
                        _hashtable.Add("@ProjectId", acc.Project.ProjectId);
                        _hashtable.Add("@GroupId", acc.Group.groupID);
                        _hashtable.Add("@Over65CADVerified", acc.Over65CADVerified);
                        _hashtable.Add("@StartDate", acc.StartDate);
                        _hashtable.Add("@EndDate", acc.EndDate);
                        _hashtable.Add("@AccountStatusId", acc.AccountStatus.Accountstatusid);
                        _hashtable.Add("@AccountProcessStatusId", acc.AccountProcessStatus.AccProcessstatusid);
                        _hashtable.Add("@CreatedBy", acc.CreatedBy.Userid);
                        _hashtable.Add("@UpdatedBy", acc.UpdatedBy.Userid);
                        _hashtable.Add("@UpdatedDateTime", acc.UpdatedDateTime);
                        _hashtable.Add("@CreatedDateTime", acc.CreatedDateTime);
                        _hashtable.Add("@ConfidentialAccount", acc.ConfidentialAccount);
                        _hashtable.Add("@VERIFICATIONASSIGNEDUSERID", acc.VERIFICATIONASSIGNEDUSERID.Userid);
                        _hashtable.Add("@VERIFICATIONASSIGNEDDATE", acc.VERIFICATIONASSIGNEDDATE);
                        _hashtable.Add("@AUDITASSIGNEDUSERID", acc.AUDITASSIGNEDUSERID.Userid);
                        _hashtable.Add("@AUDITASSIGNEDDATE", acc.AUDITASSIGNEDDATE);
                        _hashtable.Add("@VerificationUserRoleID", acc.VerificationUserRoleID.UserRoleid);
                        _hashtable.Add("@AuditUserRoleID", acc.AuditUserRoleID.UserRoleid);
                        //_hashtable.Add("@PropertyTaxAccountID", acc.PropertyTaxAccountID);
                        _hashtable.Add("@canBeDeleted", acc.canBeDeleted);
                        _hashtable.Add("@AuditCompletedDate", acc.AuditCompletedDate);
                        //_hashtable.Add("@Accountnumber_Backup", acc.Accountnumber_Backup);
                        _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_Account_Type,
                                                _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    }
                }
                return ClientID;
            }
            catch (Exception ex)
            {
                _Connection.RollbackTransaction();
                throw ex;
            }
        }
        public string generateTemporaryClientNumber(string TaxYear)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@TaxYear", Convert.ToInt32(TaxYear));
                var obj = _Connection.ExecuteScalar(StoredProcedureNames.usp_get_NextClientNumber,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet CheckClientDetailsBeforeGeneration(int ClientID)
        {
            try
            {
                DataSet dsResult = new DataSet();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", ClientID);
                dsResult = _Connection.SelectDataSet(StoredProcedureNames.usp_get_VerifyClientDocuments,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool updateClientDeliveryMethods(PTXboClientDeliveryMethods objClientDeliveryMethods)
        {
            try
            {

                PTXdoClientSetupParameters ClientSetupParameters = new PTXdoClientSetupParameters();
                //ClientSetupParameters = Repository<PTXdoClientSetupParameters>.GetQuery().Where(a => a.Client.clientId == objClientDeliveryMethods.ClientId).FirstOrDefault();
                //if (ClientSetupParameters == null)
                //{
                //    ClientSetupParameters = new PTXdoClientSetupParameters();
                //    ClientSetupParameters.Client = new PTXdoClient() { clientId = objClientDeliveryMethods.ClientId };
                //}
                Hashtable _hashtable = new Hashtable();
                //_hashtable.Add("@ClientSetupParameterid", objClientDeliveryMethods.ClientSetupParameterid);
                _hashtable.Add("@ClientId", objClientDeliveryMethods.ClientId);
                //_hashtable.Add("@TaxyearSelected", objClientDeliveryMethods.TaxyearSelected);
                //_hashtable.Add("@PackageTypeID", objClientDeliveryMethods.PackageTypeID);
                _hashtable.Add("@EnableDefaultDeliveryMethod", objClientDeliveryMethods.EnableDefaultDeliveryMethod);
                _hashtable.Add("@DefaultDeliveryTypeId", objClientDeliveryMethods.DefaultDeliveryTypeEmail == false ? 1 : 3);
                _hashtable.Add("@EnableALTDeliveryMethod", objClientDeliveryMethods.EnableALTDeliveryMethod);
                _hashtable.Add("@ALTDeliveryTypeId", objClientDeliveryMethods.ALTDeliveryTypeId == 0 ? null : (int?)objClientDeliveryMethods.ALTDeliveryTypeId);
                //_hashtable.Add("@TaxBillAuditSelected", objClientDeliveryMethods.TaxBillAuditSelected);
                //_hashtable.Add("@SendClientPackageToID", objClientDeliveryMethods.SendClientPackageToID);
                //_hashtable.Add("@AuditedFlag", objClientDeliveryMethods.AuditedFlag);                
                //_hashtable.Add("@SetupUserId", objClientDeliveryMethods.SetupUserId);
                //_hashtable.Add("@SetupUserRoleId", objClientDeliveryMethods.SetupUserRoleId);
                //_hashtable.Add("@SetupUserComments", objClientDeliveryMethods.SetupUserComments);
                //_hashtable.Add("@SetupDatetime", objClientDeliveryMethods.SetupDatetime);
                if (objClientDeliveryMethods.ClientSetUpType != string.Empty)
                {
                    if (objClientDeliveryMethods.ClientSetUpType == "AccountVerification")
                    {
                        _hashtable.Add("@VerifiedUserId", objClientDeliveryMethods.VerifiedUserId);
                        _hashtable.Add("@VerifiedUserRoleId", objClientDeliveryMethods.VerifiedUserRoleId);
                        //_hashtable.Add("@VerifiedUserComments", objClientDeliveryMethods.VerifiedUserComments);
                        _hashtable.Add("@VerifiedUserDatetime", objClientDeliveryMethods.VerifiedUserDatetime);
                    }
                    else if (objClientDeliveryMethods.ClientSetUpType == "ClientAuditing")
                    {
                        _hashtable.Add("@AuditedUserId", objClientDeliveryMethods.AuditedUserId);
                        _hashtable.Add("@AuditedUserRoleId", objClientDeliveryMethods.AuditedUserRoleId);
                        //_hashtable.Add("@AuditedUserComments", objClientDeliveryMethods.AuditedUserComments);
                        _hashtable.Add("@AuditedDateTime", objClientDeliveryMethods.AuditedDateTime);
                    }
                }

                _hashtable.Add("@UpdatedBy", objClientDeliveryMethods.UpdatedBy == 0 ? null : (int?)objClientDeliveryMethods.UpdatedBy);
                _hashtable.Add("@UpdatedDateTime", objClientDeliveryMethods.UpdatedDateTime == default(DateTime) ? null : objClientDeliveryMethods.UpdatedDateTime);
                //_hashtable.Add("@LitigateAll", objClientDeliveryMethods.LitigateAll);

                _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_ClientSetupParameters,
                                               _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                //ClientSetupParameters.ALTDeliveryType = objClientDeliveryMethods.ALTDeliveryTypeId == 0 ? null : new PTXdoDeliveryMethod() { id = objClientDeliveryMethods.ALTDeliveryTypeId };
                //ClientSetupParameters.DefaultDeliveryType = objClientDeliveryMethods.DefaultDeliveryTypeId == 0 ? null : new PTXdoDeliveryMethod() { id = objClientDeliveryMethods.DefaultDeliveryTypeId };
                //ClientSetupParameters.EnableALTDeliveryMethod = objClientDeliveryMethods.EnableALTDeliveryMethod;
                //ClientSetupParameters.EnableDefaultDeliveryMethod = objClientDeliveryMethods.EnableDefaultDeliveryMethod;
                //ClientSetupParameters.UpdatedBy = objClientDeliveryMethods.UpdatedBy == 0 ? null : new PTXdoUser { Userid = objClientDeliveryMethods.UpdatedBy };
                //ClientSetupParameters.UpdatedDateTime = objClientDeliveryMethods.UpdatedDateTime == default(DateTime) ? null : objClientDeliveryMethods.UpdatedDateTime;

                //if (objClientDeliveryMethods.ClientSetUpType != string.Empty)
                //{
                //    if (objClientDeliveryMethods.ClientSetUpType == "AccountVerification")
                //    {
                //        ClientSetupParameters.VerifiedUser = new PTXdoUser() { Userid = objClientDeliveryMethods.VerifiedUserId };
                //        ClientSetupParameters.VerifiedUserRole = new PTXdoUserRole() { UserRoleid = objClientDeliveryMethods.VerifiedUserRoleId };
                //        ClientSetupParameters.VerifiedUserDatetime = objClientDeliveryMethods.VerifiedUserDatetime;
                //    }
                //    else if (objClientDeliveryMethods.ClientSetUpType == "ClientAuditing")
                //    {
                //        ClientSetupParameters.AuditedUser = new PTXdoUser() { Userid = objClientDeliveryMethods.AuditedUserId };
                //        ClientSetupParameters.AuditedUserRole = new PTXdoUserRole() { UserRoleid = objClientDeliveryMethods.AuditedUserRoleId };
                //        ClientSetupParameters.AuditedDateTime = objClientDeliveryMethods.AuditedDateTime;
                //    }
                //}


                //Repository<PTXdoClientSetupParameters>.SaveOrUpdate(ClientSetupParameters);

                _hashtable = new Hashtable();
                _hashtable.Add("@ClientId", objClientDeliveryMethods.ClientId);
                //_hashtable.Add("@ClientName", model.objClient.ClientName);
                int ClassificationType = 0;
                bool IsClassType = GetClassificationType(objClientDeliveryMethods.ClientId, out ClassificationType);
                if (IsClassType)
                {
                    _hashtable.Add("@ClassifiationTypeID", ClassificationType);
                }

                //_hashtable.Add("@PrimaryAgentId", model.objClient.PrimaryAgentId);
                //_hashtable.Add("@ContactAgentId", model.objClient.ContactAgentId);
                //_hashtable.Add("@SalesAgentId", model.objClient.SalesAgentId);
                //_hashtable.Add("@ClientSetupMethodId", model.objClient.ClientSetupMethodId);
                //_hashtable.Add("@ClientSourceId", model.objClient.ClientSourceId);
                //_hashtable.Add("@SpecialHandlingRequired", model.objClient.SpecialHandlingRequired);
                //_hashtable.Add("@TPVRequired", model.objClient.TPVRequired);
                //_hashtable.Add("@TPVCode", model.objClient.TPVCode);
                //_hashtable.Add("@StartDate", model.objClient.StartDate);
                //_hashtable.Add("@EndDate", model.objClient.EndDate);

                if (objClientDeliveryMethods.ClientProcessingStatusID == Enumerators.PTXenumClientProcessingStatus.AuditCompleted.GetId())
                {
                    _hashtable.Add("@ClientProcessingStatusId", objClientDeliveryMethods.ClientProcessingStatusID);
                }
                string ClientNumber;
                DataSet ds = GetClientNumber(objClientDeliveryMethods.ClientId);
                ClientNumber = ds.Tables[0].Rows[0]["ClientNumber"].ToString();
                if (ClientNumber.Contains('T'))
                {
                    _hashtable.Add("@ClientNumber", ClientNumber.Replace('T', ' ').Trim());
                    _hashtable.Add("@UpdatedByID", objClientDeliveryMethods.UpdatedBy == 0 ? null : (int?)objClientDeliveryMethods.UpdatedBy);
                    _hashtable.Add("@UpdatedDateTime", objClientDeliveryMethods.UpdatedDateTime == default(DateTime) ? null : objClientDeliveryMethods.UpdatedDateTime);
                    _hashtable.Add("@StartDate", DateTime.Now);
                    if (objClientDeliveryMethods.ClientProcessingStatusID != 0)
                        _hashtable.Add("@ClientProcessingStatusId", objClientDeliveryMethods.ClientProcessingStatusID);
                }

                if (Convert.ToInt16(ds.Tables[0].Rows[0]["Clientstatusid"]) == Enumerators.PTXenumClientStatus.FormerClient.GetId() || Convert.ToInt16(ds.Tables[0].Rows[0]["Clientstatusid"]) == Enumerators.PTXenumClientStatus.Inactive.GetId() || Convert.ToInt16(ds.Tables[0].Rows[0]["Clientstatusid"]) == Enumerators.PTXenumClientStatus.PendingforReactivation.GetId())
                {
                    //bool ContainsActiveAccount = Repository<PTXdoAccount>.GetQuery().Any(Acc => Acc.Client.clientId == client.clientId && Acc.AccountStatus.Accountstatusid == Enumerators.PTXenumAccountStatus.Active.GetId());
                    DataSet accDS = GetActiveAccount(objClientDeliveryMethods.ClientId, Enumerators.PTXenumAccountStatus.Active.GetId());

                    if (accDS.Tables[0].Rows.Count > 0)
                    {
                        _hashtable.Add("@ClientStatusId", Enumerators.PTXenumClientStatus.Active.GetId());
                        _hashtable.Add("@ClientProcessingStatusId", Enumerators.PTXenumClientProcessingStatus.AuditCompleted.GetId());
                    }
                }



                //_hashtable.Add("@SpecialHandlingRequiredRemark", model.objClient.SpecialHandlingRequiredRemark);
                //_hashtable.Add("@ReferralName", model.objClient.ReferralName);

                //_hashtable.Add("@CreatedByID", model.objClient.CreatedByID);
                //_hashtable.Add("@CreatedDateTime", model.objClient.CreatedDateTime);
                //_hashtable.Add("@LegalCollectionsFlag", model.objClient.LegalCollectionsFlag);
                //_hashtable.Add("@VERIFIEDUSER", model.objClient.VERIFIEDUSER);
                //_hashtable.Add("@VERIFIEDDATE", model.objClient.VERIFIEDDATE);
                //_hashtable.Add("@AUDITUSER", model.objClient.AUDITUSER);
                //_hashtable.Add("@AUDITDATE", model.objClient.AUDITDATE);
                //_hashtable.Add("@VerificationUserRoleID", model.objClient.VerificationUserRoleID);
                //_hashtable.Add("@AuditUserRoleID", model.objClient.AuditUserRoleID);
                //_hashtable.Add("@DonotAssign", model.objClient.DonotAssign);
                //_hashtable.Add("@CreatedUserRoleID", model.objClient.CreatedUserRoleID);
                //_hashtable.Add("@NoncontactCode", model.objClient.NoncontactCode);
                //_hashtable.Add("@CollectionAgentId", model.objClient.CollectionAgentId);
                //_hashtable.Add("@CollectionStatusCode", model.objClient.CollectionStatusCode);
                //_hashtable.Add("@PropertyTaxClientID", model.objClient.PropertyTaxClientID);
                //_hashtable.Add("@BASentDate", model.objClient.BASentDate);
                //_hashtable.Add("@BAReceivedDate", model.objClient.BAReceivedDate);
                //_hashtable.Add("@BlankAuthID", model.objClient.BlankAuthID);
                //_hashtable.Add("@GiftCardSentDate", model.objClient.GiftCardSentDate);
                //_hashtable.Add("@isJudicial", model.objClient.isJudicial);
                //_hashtable.Add("@SpecialHandlingforAgent", model.objClient.SpecialHandlingforAgent);
                //_hashtable.Add("@SpecialHandlingforEscalation", model.objClient.SpecialHandlingforEscalation);
                //_hashtable.Add("@isValidBALitAgmt", model.objClient.isValidBALitAgmt);
                //_hashtable.Add("@IsNextYearSignup", model.objClient.IsNextYearSignup);
                //_hashtable.Add("@isRestrictedCommunication", model.objClient.isRestrictedCommunication);
                //_hashtable.Add("@isHarveyLegalCollection", model.objClient.isHarveyLegalCollection);
                _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_Client,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                //Repository<PTXdoClient>.SaveOrUpdate(client);

                if (objClientDeliveryMethods.DocumentTypeID > 0 && objClientDeliveryMethods.IndexClientDocumentID > 0)
                {
                    UpdateClientIDIntoIndexClientDocument(objClientDeliveryMethods.DocumentTypeID, objClientDeliveryMethods.PWImageID, objClientDeliveryMethods.ClientId, objClientDeliveryMethods.IndexClientDocumentID);
                }

                /* Update the Accounts as Not removable after client got generated */
                //Repository<PTXdoAccount>.GetQuery().Where(a => a.Client.clientId == client.clientId).ToList().ForEach(a => { a.canBeDeleted = false; Repository<PTXdoAccount>.SaveOrUpdate(a); });
                /*Save the Transaction */
                //Registry.UnitofWorkFactory.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool UpdateClientIDIntoIndexClientDocument(int DocumentTypeID, int PWImageID, int ClientID, int IndexClientDocumentID)
        {
            try
            {
                if (IndexClientDocumentID != 0)
                {
                    Hashtable _hashtable = new Hashtable();
                    _hashtable.Add("@DocumentTypeID", DocumentTypeID);
                    _hashtable.Add("@ClientID", ClientID);
                    _hashtable.Add("@IndexClientDocumentID", IndexClientDocumentID);
                    _hashtable.Add("@DocumentProcessingStatusId", Enumerators.PTXenumDocumentProcessingStatus.ClientEntryCompleted.GetId());
                    _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_IndexedClientDocument_ClientIDIntoIndexClientDocument,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool UpdateAccountCanbeDelete(int ClientID)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", ClientID);
                _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_Account_CanbeDelete,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool GetClassificationType(int ClientId, out int ClassificationType)
        {
            ClassificationType = 0;
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", ClientId);
                DataTable dtResult = _Connection.SelectDataSet(StoredProcedureNames.USP_GetClassificationType,
                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];

                if (dtResult.Rows.Count > 0)
                {
                    ClassificationType = Convert.ToInt32(dtResult.Rows[0][0]);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataSet GetClientNumber(int ClientId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", ClientId);
                return _Connection.SelectDataSet(StoredProcedureNames.USP_GetClientNumber,
                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataSet GetActiveAccount(int ClientId, int AccountStatusId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", ClientId);
                _hashtable.Add("@AccountStatusId", AccountStatusId);
                return _Connection.SelectDataSet(StoredProcedureNames.USP_GetActiveAccount,
                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCAFRequestonExpiry> getCAFRequestonExpiry(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientId", model.Value1);
                _hashtable.Add("@GroupId", model.Value2);
                return _Connection.Select<PTXboCAFRequestonExpiry>(StoredProcedureNames.usp_SPARTAXX_SEL_AllCAFAccounts,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool updateGroupDetails(PTXboGroup objGroupDetails)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@groupID", objGroupDetails.groupID);
                _hashtable.Add("@clientId", objGroupDetails.clientID);
                _hashtable.Add("@groupName", objGroupDetails.groupName);
                _hashtable.Add("@GroupOrderNumber", objGroupDetails.GroupOrderNumber);
                _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_Group,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                foreach (PTXboTerms _terms in objGroupDetails.termsList)
                {
                    _hashtable = new Hashtable();
                    _hashtable.Add("@TermsId", _terms.termsID);
                    _hashtable.Add("@EffectiveDate", _terms.EffectiveDate);
                    _hashtable.Add("@UpdatedByID", _terms.UpdatedByID);
                    _hashtable.Add("@UpdatedDateTime", _terms.UpdatedDateTime);
                    _hashtable.Add("@InvoiceQueueDate", _terms.InvoiceQueueDate);
                    _hashtable.Add("@Contingency", _terms.Contingency);
                    _hashtable.Add("@ContingencyAfterExpiry", _terms.ContingencyAfterExpiry);
                    _hashtable.Add("@InvoiceFrequencyID", _terms.InvoiceFrequencyID);
                    _hashtable.Add("@FrequencyDate", _terms.FrequencyDate);
                    _hashtable.Add("@ExpiryRemarks", _terms.ExpiryRemarks);
                    _hashtable.Add("@ExpiryDate", _terms.ExpiryDate);
                    _hashtable.Add("@FlatFee", _terms.FlatFee);
                    _hashtable.Add("@FlatFeeAfterExpiry", _terms.FlatFeeAfterExpiry);
                    _hashtable.Add("@groupID", _terms.groupID);
                    _hashtable.Add("@remarks", _terms.remarks);
                    _hashtable.Add("@CapValue", _terms.CapValue);
                    _hashtable.Add("@IsSpecializedTerm", _terms.IsSpecializedTerm);
                    _hashtable.Add("@Termstypeid", _terms.termsTypeID);
                    _hashtable.Add("@TermExpiryActionID", _terms.TermExpiryActionId);
                    _hashtable.Add("@InvoiceGroupingTypeId", _terms.InvoiceGroupingTypeId);
                    _Connection.Execute(StoredProcedureNames.SaveOrUpdate_PTAX_Terms,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboAccountLevelHistory> getTermAccountLevelHistory(int GroupId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@GroupId", GroupId);
                return _Connection.Select<PTXboAccountLevelHistory>(StoredProcedureNames.usp_getTermAccountLevelHistory,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboGroup getTermsDetails(int GroupId)
        {
            try
            {
                PTXboGroup objGroup = new PTXboGroup();
                DataTable dtResultset = new DataTable();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@GroupID", GroupId);
                dtResultset = _Connection.SelectDataSet(StoredProcedureNames.usp_getGroupTermsDetails,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                if (dtResultset == null || dtResultset.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    var termsDetailsList = dtResultset.ToCollection<PTXboTermsDetails>();
                    //objGroup = termsDetailsList.GroupBy(a => new { a.GroupId, a.GroupName, a.ClientId, a.GroupTypeId }).Select(b => new PTXdoGroup
                    objGroup = termsDetailsList.GroupBy(a => new { a.GroupId, a.GroupName, a.ClientId }).Select(b => new PTXboGroup
                    {
                        groupID = b.Key.GroupId ?? 0,
                        groupName = b.Key.GroupName,
                        //groupType = new PTXdoGroupType() { GroupTypeId = b.Key.GroupTypeId.Value },
                        client = new PTXboClient() { clientId = b.Key.ClientId ?? 0 },
                        termsList = termsDetailsList.Where(c => c.GroupId == b.Key.GroupId).Select(d => new PTXboTerms
                        {
                            termsID = d.TermsId,
                            termsType = new PTXboTermsType() { Termstypeid = d.TermsTypeId, TermsType = d.TermsType },
                            EffectiveDate = d.EffectiveDate,
                            ExpiryDate = d.ExpiryDate,
                            remarks = d.Remarks,
                            UpdatedByID = d.UpdatedBy,
                            UpdatedDateTime = d.UpdatedDateTime,
                            Contingency = d.Contingency,
                            FlatFee = d.FlatFee,
                            CapValue = d.CapValue,
                            IsSpecializedTerm = d.IsSpecializedTerm,
                            InvoiceFrequency = d.InvoiceFrequencyId > 0 ? new PTXboInvoiceFrequency { InvoiceFrequencyID = d.InvoiceFrequencyId, InvoiceFrequencyCode = d.InvoiceFrequencyCode } : new PTXboInvoiceFrequency { InvoiceFrequencyID = 1, InvoiceFrequencyCode = "Immediate" },
                            FrequencyDate = d.FrequencyDate,
                            ExpiryRemarks = d.ExpiryRemarks,
                            ContingencyAfterExpiry = d.ContingencyAfterExpiry,
                            FlatFeeAfterExpiry = d.FlatFeeAfterExpiry,
                            TermExpiryAction = new PTXboTermExpiryAction() { TermExpiryActionID = d.TermExpiryActionID, TermExpiryAction = d.TermExpiryAction },
                            InvoiceGroupingType = new PTXboInvoiceGroupType() { InvoiceGroupingTypeId = d.InvoiceGroupingTypeId },
                            InvoiceQueueDate = d.InvoiceQueueDate
                        }).ToList()
                    }).FirstOrDefault();
                }
                return objGroup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getAssandUnAssAccountscount(PTXboSearchAccountCriteria objSearchAccountCriteria)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", objSearchAccountCriteria.ClientID);
                _hashtable.Add("@GroupID", objSearchAccountCriteria.GroupID);//@AccountNumber
                _hashtable.Add("@AccountNumber", objSearchAccountCriteria.AccountNumber);
                _hashtable.Add("@ProjectName", objSearchAccountCriteria.ProjectName);
                _hashtable.Add("@isProject", objSearchAccountCriteria.isProject);
                return _Connection.SelectDataSet(StoredProcedureNames.usp_getAssandUnAssAccountscount,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboAgreements> getAgreementsHistory(int GroupID)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@GroupId", GroupID);
                return _Connection.Select<PTXboAgreements>(StoredProcedureNames.usp_getAgreementHistory,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboTermAccounts> getTermsAccounts(PTXboSearchAccountCriteria objSearchAccountCriteria)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@isAssociated", objSearchAccountCriteria.isAssociated);
                _hashtable.Add("@ClientID", objSearchAccountCriteria.ClientID);
                _hashtable.Add("@GroupID", objSearchAccountCriteria.GroupID);
                _hashtable.Add("@AccountNumber", objSearchAccountCriteria.AccountNumber);
                _hashtable.Add("@ProjectName", objSearchAccountCriteria.ProjectName);
                _hashtable.Add("@isCommercial", objSearchAccountCriteria.isCommercial);
                _hashtable.Add("@isResidential", objSearchAccountCriteria.isResidential);
                return _Connection.Select<PTXboTermAccounts>(StoredProcedureNames.usp_getTermsAccounts,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboTermsDetails> getAccountTermHistory(int AccountId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", AccountId);
                return _Connection.Select<PTXboTermsDetails>(StoredProcedureNames.usp_getAccountTermHistory,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboTermsDetails> getAccountCurrentTermDetails(int AccountId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", AccountId);

                return _Connection.Select<PTXboTermsDetails>(StoredProcedureNames.usp_getAccountCurrentTermDetails,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboAgreements getAgreementDetails(int GroupID)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@GroupId", GroupID);
                //return _Connection.SelectDataSet(StoredProcedureNames.usp_getAgreementsDetailsRecent,
                //        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                return _Connection.Select<PTXboAgreements>(StoredProcedureNames.usp_getAgreementsDetailsRecent,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getCafAccountCountbasedonStatus(int clientID)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientID", clientID);
                return _Connection.SelectDataSet(StoredProcedureNames.usp_getAccountCountbasedonAccountStatusandClientID,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboHearingDetailsProperty GetHearingDetails(PTXboParameters model)
        {
            string SPName = "";
            try
            {
                PTXboHearingDetailsProperty objHearingDetailsProperty = new PTXboHearingDetailsProperty();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.Value1);
                _hashtable.Add("@TaxYear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);
                _hashtable.Add("@HearingLevelId", model.Value6);

                if (model.Value1 > 0)
                {
                    int accountid = model.Value1 == null ? 0 : Convert.ToInt32(model.Value1);
                    model.IsOutOfTexasProperty = IsOutOfTexasProperty(accountid);
                }


                if (model.IsOutOfTexasProperty)
                    SPName = StoredProcedureNames.usp_GetHearingDetailsForProperty_NonTexas;
                else
                    SPName = StoredProcedureNames.usp_GetHearingDetailsForProperty;

                DataSet dsResult = _Connection.SelectDataSet(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                {
                    objHearingDetailsProperty = dsResult.Tables[0].ToCollection<PTXboHearingDetailsProperty>().FirstOrDefault();
                    if (objHearingDetailsProperty.completionDateAndTime != null)
                    {
                        objHearingDetailsProperty.completionTime = objHearingDetailsProperty.completionDateAndTime.Value.TimeOfDay;
                    }
                }

                else if (dsResult != null && dsResult.Tables[1].Rows.Count > 0)
                {
                    objHearingDetailsProperty = dsResult.Tables[1].ToCollection<PTXboHearingDetailsProperty>().FirstOrDefault();
                }

                if ((objHearingDetailsProperty.HearingTypeId == 0 || objHearingDetailsProperty.HearingTypeId == null) && model.Value3 != 0)
                {
                    PTXdoHearingType objHearingType = new PTXdoHearingType();

                    DataTable dt = _Connection.SelectDataSet("Select * from PTAX_HearingType WITH(NOLOCK) WHERE HearingTypeId= " + model.Value3,
                       _hashtable, Enumerator.Enum_CommandType.InlineQuery, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                    objHearingDetailsProperty.HearingTypeId = Convert.ToInt32(dt.Rows[0]["HearingTypeId"].ToString());
                    objHearingDetailsProperty.HearingType = dt.Rows[0]["HearingType"].ToString();

                }
                return objHearingDetailsProperty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet loadInformalInformalFlag(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                return _Connection.SelectDataSet(StoredProcedureNames.usp_GetInformalInformalDateForMainScreen,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboHearingDateHistory> GetHearingDateHistory(int HearingDetailsId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@HearingDetailsId", HearingDetailsId);
                return _Connection.Select<PTXboHearingDateHistory>(StoredProcedureNames.usp_getHeaingDetailsDateHistory,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCMAccountLetterDetails> GetCorrectionMotionFiledDetails(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@TaxYear", model.Value1);
                _hashtable.Add("@AccountId", model.Value2);
                return _Connection.Select<PTXboCMAccountLetterDetails>(StoredProcedureNames.usp_GetCorrectionMotionFiledDetails,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboPanelDocketIDDetails> GetPanelDocketIDHistory(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@HearingDetailsId", model.Value1);
                _hashtable.Add("@PanelIDType", model.Value5);

                return _Connection.Select<PTXboPanelDocketIDDetails>(StoredProcedureNames.usp_getPanelDocketIDHistory,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboHearingNoticeLetterHistory> GetHearingNoticeLetterDateHistory(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@HearingDetailsId", model.Value1);
                _hashtable.Add("@Hearingtype", model.Value2);

                return _Connection.Select<PTXboHearingNoticeLetterHistory>(StoredProcedureNames.USP_GetHearingNoticeLetterDateHistory,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboUserEnteredAofADetail getAofADetails(PTXboParameters model)
        {
            PTXboUserEnteredAofADetail objUserEnteredAofADetail = new PTXboUserEnteredAofADetail();
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.Value1);
                _hashtable.Add("@AofATypeID", model.Value2);
                _hashtable.Add("@ClientId", model.Value3);
                _hashtable.Add("@AofACS", model.Value4);

                DataSet dsResult = _Connection.SelectDataSet(StoredProcedureNames.usp_getAofADetails,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                /*If DataSet have datatable*/
                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    /*Convert UserEnteredAofADetail Object*/
                    if (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    {
                        objUserEnteredAofADetail = dsResult.Tables[0].ToCollection<PTXboUserEnteredAofADetail>().FirstOrDefault();

                        /*Convert UserEnteredAofADetail Object*/
                        if (dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                        {
                            objUserEnteredAofADetail.UserEnteredAofAAttachment = dsResult.Tables[1].ToCollection<PTXboUserEnteredAofAAttachment>().ToList();
                        }

                        /*Convert UserEnteredAofADetail Object*/
                        if (dsResult.Tables.Count > 2 && dsResult.Tables[2].Rows.Count > 0)
                        {
                            objUserEnteredAofADetail.objCadLegalNameHistory = dsResult.Tables[2].ToCollection<PTXboCadLegalNameHistory>().ToList();
                        }
                    }
                }
                return objUserEnteredAofADetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboHearingDetails> getHearingInfo(int Accountid)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", Accountid);
                return _Connection.Select<PTXboHearingDetails>(StoredProcedureNames.usp_GetHearingDetailsforAOfA,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboHearingDetails> getPastHearingInfo(int Accountid)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", Accountid);
                return _Connection.Select<PTXboHearingDetails>(StoredProcedureNames.usp_GetPastHearingDetailsforAOfA,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboPreviousAofADetails> getAofAPreviousRevisedDateHistory(int AccountId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", AccountId);
                return _Connection.Select<PTXboPreviousAofADetails>(StoredProcedureNames.usp_getAofAPreviousRevisedDateHistory,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboDateCodedHistory> getDateCodedEndHistory(int AccountId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", AccountId);
                return _Connection.Select<PTXboDateCodedHistory>(StoredProcedureNames.usp_getAofADateCodedEndHistory,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboCadLegalNameHistory> getCADLegalNameHistory(int AccountId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", AccountId);
                return _Connection.Select<PTXboCadLegalNameHistory>(StoredProcedureNames.usp_CadLegalNameHistory,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getClientContactLogLitigation(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@clientID", model.Value1);
                _hashtable.Add("@IsLitigation", model.Value4);
                return _Connection.SelectDataSet(StoredProcedureNames.usp_getClientContactLog,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboDateCodedHistory> getDateCodedHistory(int AccountId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", AccountId);
                return _Connection.Select<PTXboDateCodedHistory>(StoredProcedureNames.usp_getAofADateCodedHistory,
                                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboArbitrationDetails GetTaxyearBasedArbitration(PTXboParameters model)
        {
            try
            {
                PTXboArbitrationDetails ArbitrationDetails = new PTXboArbitrationDetails();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@TaxYear", model.TaxYear);
                _hashtable.Add("@AccountNo", model.AccountNumber);
                _hashtable.Add("@AccountId", model.AccountId);
                DataTable dtResult = new DataTable();
                dtResult = _Connection.SelectDataSet(StoredProcedureNames.usp_getTaxyearBasedArbitration,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                if (dtResult != null || dtResult.Rows.Count > 0)
                {
                    ArbitrationDetails = dtResult.ToCollection<PTXboArbitrationDetails>().FirstOrDefault();
                    ArbitrationDetails.Arbitrator = dtResult.ToCollection<PTXboArbitratorDetails>().FirstOrDefault();
                    ArbitrationDetails.SOAHDetails = dtResult.ToCollection<PTXboSOAHDetails>().FirstOrDefault();
                }
                return ArbitrationDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboArbitratorDetails> getArbitratorDetails(int ArbitratorId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ArbitratorId", ArbitratorId);
                return _Connection.Select<PTXboArbitratorDetails>(StoredProcedureNames.Usp_getArbitratorDetails,
                                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboDocumentsFromPaperwise> getDocumentsfromPaperwiseUsingMultiDoctype(PTXboDocumentsFromPaperwise objSearchCriteria)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientNumber", objSearchCriteria.ClientNumber);
                _hashtable.Add("@AccountNumber", objSearchCriteria.AccountNumber);
                _hashtable.Add("@TaxYear", objSearchCriteria.TaxYear);
                _hashtable.Add("@CountyCode", objSearchCriteria.CountyCode);
                _hashtable.Add("@DocumentType", objSearchCriteria.DocumentType);
                _hashtable.Add("@IsAccountWise", objSearchCriteria.IsAccountWise);
                return _Connection.Select<PTXboDocumentsFromPaperwise>(StoredProcedureNames.usp_getDocumentsFromPaperwiseUsingMutiDocType,
                                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet LoadLitigationDetails(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.Value1);
                _hashtable.Add("@TaxYear", model.Value2);
                return _Connection.SelectDataSet(StoredProcedureNames.USP_LoadLitigationDetails,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetPendingLitigationTaxYear(int AccountID)
        {
            try
            {
                string PendingLitigationTaxYear = string.Empty;
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", AccountID);
                DataTable dtResult = _Connection.SelectDataSet(StoredProcedureNames.usp_GetPendingLitigationTaxYear,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        PendingLitigationTaxYear += dtResult.Rows[i]["TaxYear"].ToString() + ", ";
                    }
                    PendingLitigationTaxYear = PendingLitigationTaxYear.Remove(PendingLitigationTaxYear.Length - 2);
                }
                return PendingLitigationTaxYear;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboLitigationDocuments> ViewDocuments(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@LitigationID", model.Value1);
                _hashtable.Add("@DocumentType", model.Value2);
                return _Connection.Select<PTXboLitigationDocuments>(StoredProcedureNames.usp_ViewDocuments,
                                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboSupplementaryDetails> LoadSupplementHistory(int LitigationID)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@LitigationID", LitigationID);
                return _Connection.Select<PTXboSupplementaryDetails>(StoredProcedureNames.Usp_LoadSupplementHistory,
                                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboLitigationAccountDetails> GetTrialDateHistory(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.AccountId);
                _hashtable.Add("@TaxYear", model.TaxYear);
                return _Connection.Select<PTXboLitigationAccountDetails>(StoredProcedureNames.usp_getLitigationTrialDateHistory,
                                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet LoadLitigationDetailsII(PTXboParameters model)
        {
            try
            {
                PTXboLitigationSecAccountDetails LitigationDetails = new PTXboLitigationSecAccountDetails();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.AccountId);
                _hashtable.Add("@TaxYear", model.TaxYear);
                _hashtable.Add("@LawsuitTypeid", model.Value1);
                return _Connection.SelectDataSet(StoredProcedureNames.USP_LoadLitigationSecDetails,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetPendingLitigationTaxYearII(PTXboParameters model)
        {
            string PendingLitigationTaxYear = string.Empty;
            try
            {

                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.AccountId);
                _hashtable.Add("@TaxYear", model.TaxYear);
                _hashtable.Add("@Lawsuittypeid", model.Value1);


                DataTable dtResult = new DataTable();
                dtResult = _Connection.SelectDataSet(StoredProcedureNames.usp_GetPendingSecLitigationTaxYear,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        PendingLitigationTaxYear += dtResult.Rows[i]["TaxYear"].ToString() + ", ";
                    }
                    PendingLitigationTaxYear = PendingLitigationTaxYear.Remove(PendingLitigationTaxYear.Length - 2);
                }
                return PendingLitigationTaxYear;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboLitigationSecAccountDetails> SecGetTrialDateHistory(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.AccountId);
                _hashtable.Add("@TaxYear", model.TaxYear);
                return _Connection.Select<PTXboLitigationSecAccountDetails>(StoredProcedureNames.usp_getLitigationTrialDateHistory,
                                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboMainScreenValues getMainScreenValues(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountID", model.AccountId);
                _hashtable.Add("@TaxYear", model.TaxYear);
                _hashtable.Add("@HearingTypeID", model.Value1);
                return _Connection.Select<PTXboMainScreenValues>(StoredProcedureNames.usp_GetMainScreenValues,
                                     _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboPropertySurveyClientInputs> getMSPropertySurvey(PTXboPropertyConditionModel objmodel)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@AccountId", objmodel.AccountId);
                _hashtable.Add("@TaxYear", objmodel.TaxYear);
                return _Connection.Select<PTXboPropertySurveyClientInputs>(StoredProcedureNames.usp_getMSPropertySurvey,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboBPPPropertyValues> getMainScreenBPPPropertyValues(PTXboBPPPropertyValues objmodel)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@AccountId", objmodel.AccountId);
                _hashtable.Add("@TaxYear", objmodel.TaxYear);
                return _Connection.Select<PTXboBPPPropertyValues>(StoredProcedureNames.usp_getMSBPPPropertyDetails,
                                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboILHearingRemarks> GetILHearingRemarks(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@TaxYear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);

                return _Connection.Select<PTXboILHearingRemarks>(StoredProcedureNames.USP_GetILPropertyHearingRemarks,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboILHearingRemarks
                                   {
                                       ILHearingRemarkId = a.ILHearingRemarkId,
                                       ILHearingDetailsId = a.ILHearingDetailsId,
                                       HearingRemark = a.HearingRemark,
                                       CreatedDateTime = a.CreatedDateTime,
                                       UserName = a.UserName
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboILAgentRemarks> GetILAgentRemarks(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);
                return _Connection.Select<PTXboILAgentRemarks>(StoredProcedureNames.USP_GetILPropertyAgentRemarks,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboILAgentRemarks
                                   {
                                       ILAgentRemarkId = a.ILAgentRemarkId,
                                       ILHearingDetailsId = a.ILHearingDetailsId,
                                       AgentRemark = a.AgentRemark,
                                       CreatedDateTime = a.CreatedDateTime,
                                       UserName = a.UserName
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboILHearingDetails GetILHearingDetails(PTXboParameters model)
        {
            try
            {
                PTXboILHearingDetails ILHearingDetails = new PTXboILHearingDetails();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);

                var objDataTable = _Connection.SelectSingle(StoredProcedureNames.USP_GetILHearingDetails, _hashtable,
                   Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                   gr => gr.Read<PTXboDVMILHearingDetails>()
                );
                //var objDataTable = _Connection.SelectDataSet(StoredProcedureNames.USP_GetILHearingDetails,
                //      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                //var objDataTable = _Connection.Select<PTXboDVMILHearingDetails>(StoredProcedureNames.USP_GetILHearingDetails,
                //      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return convertdtResultToILHearingObject(objDataTable, ILHearingDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool GetMainscreenGotoDDLValues(PTXboMainScreenFieldNames requestInput, out List<PTXboMainScreenFieldNames> getGotoDDLVal, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@userId", requestInput.Userid);
                getGotoDDLVal = _Connection.Select<PTXboMainScreenFieldNames>(StoredProcedureNames.USP_GetMainscreenGotoDDLValues,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboMainScreenFieldNames
                                   {
                                       GotoDDLValues = a.GotoDDLValues,
                                       GotoDDLText = a.GotoDDLText
                                   }).ToList();
                return true;
            }
            catch(Exception ex)
            {
                errorMessage = ex.ToString();
                getGotoDDLVal = new List<PTXboMainScreenFieldNames>();
                return false;
            }
        }
        private PTXboILHearingDetails convertdtResultToILHearingObject(dynamic SP_Result, PTXboILHearingDetails ILHearingDetails)
        {
            try
            {
                if (SP_Result != null)
                {
                    var dtILHearingDetails = (List<PTXboDVMILHearingDetails>)SP_Result.Item1;

                    int Hearintypeid = dtILHearingDetails.Select(b => b.ILHearingTypeId).First();
                    if (Hearintypeid == 1)
                    {
                        ILHearingDetails = dtILHearingDetails.Select(b => new PTXboILHearingDetails
                        {
                            ILHearingDetailsId = b.ILHearingDetailsId,
                            HearingTypeid = b.ILHearingTypeId,
                            YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                            AssosserHearing = new PTXboILAssessorHearingDetails()
                            {
                                YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                                AssessorConfirmationDate = b.AssessorConfirmationDate,
                                AssessorDeadlineDate = b.AssessorDeadlineDate,
                                AssessorStatusId = b.AssessorStatusId,
                                ILCompletionDate = b.ILCompletionDate,
                                ILExemptInvoice = b.ILExemptInvoice,
                                ILExemptReasonId = b.ILExemptReasonId,
                                ILHearingAgentId = b.ILHearingAgentId,
                                ILHearingDetailsId = b.ILHearingDetailsId,
                                ILHearingFinalized = b.ILHearingFinalized,
                                ILHearingFinalizedDate = b.ILHearingFinalizedDate,
                                ILHearingTypeId = b.ILHearingTypeId,
                                ILInvoiceDate = b.ILInvoiceDate,
                                ILInvoiceId = b.ILInvoiceId,
                                ILInvoiceStatusId = b.ILInvoiceStatusId,
                                ILPreppedDate = b.ILPreppedDate,
                                ILTargetValue = b.ILTargetValue,
                                NoticeImprovedValue = b.NoticeImprovedValue,
                                NoticeLandValue = b.NoticeLandValue,
                                NoticeMarketValue = b.NoticeMarketValue,
                                NoticeTotalValue = b.NoticeTotalValue,
                                PostAssessedImprovedValue = b.PostAssessedImprovedValue,
                                PostAssessedLandValue = b.PostAssessedLandValue,
                                PostMarketValue = b.PostMarketValue,
                                PostTexableAssessedValue = b.PostTexableAssessedValue,
                                PublicationDate = b.PublicationDate,
                                StateEVAValue = b.StateEVAValue,
                                TownShipGroupName = b.GroupName,
                                AssessorFilingdate=b.AssessorFilingdate,
                                AssessorProtestCodeValueId = b.AssessorProtestCodeValueId,
                                AssessorSettlingAttorneyId=b.AssessorSettlingAttorneyId,
                                AssessorProtestDetailsid = b.ProtestDetailsid,
                                AssessorFormReceivedDate = b.FormReceivedDate,
                                AssessorProtestReasonId = b.AssessorProtestReasonId,
                                ILGeneratedHearingResultAssessor = b.ILGeneratedHearingResult

                            }
                        }).FirstOrDefault();
                    }
                    if (Hearintypeid == 2)
                    {
                        ILHearingDetails = dtILHearingDetails.Select(b => new PTXboILHearingDetails
                        {
                            ILHearingDetailsId = b.ILHearingDetailsId,
                            HearingTypeid = b.ILHearingTypeId,
                            YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                            BORHearing = new PTXboILBORHearingDetails()
                            {
                                BORYearlyHearingDetailsId = b.YearlyHearingDetailsId,
                                BORFilingClosedDate = b.BORFilingClosedDate,
                                BORFilingOpenDate = b.BORFilingOpenDate,
                                BORStatusId = b.BORStatusId,
                                ILBORCompletionDate = b.ILCompletionDate,
                                ILBORExemptInvoice = b.ILExemptInvoice,
                                ILBORExemptReasonId = b.ILExemptReasonId,
                                ILBORHearingAgentId = b.ILHearingAgentId,
                                ILBORHearingDetailsId = b.ILHearingDetailsId,
                                ILBORHearingFinalized = b.ILHearingFinalized,
                                ILBORHearingFinalizedDate = b.ILHearingFinalizedDate,
                                ILBORHearingTypeId = b.ILHearingTypeId,
                                ILBORInvoiceDate = b.ILInvoiceDate,
                                ILBORInvoiceId = b.ILInvoiceId,
                                ILBORInvoiceStatusId = b.ILInvoiceStatusId,
                                ILBORPreppedDate = b.ILPreppedDate,
                                ILBORTargetValue = b.ILTargetValue,
                                BORNoticeImprovedValue = b.NoticeImprovedValue,
                                BORNoticeLandValue = b.NoticeLandValue,
                                BORNoticeMarketValue = b.NoticeMarketValue,
                                BORNoticeTotalValue = b.NoticeTotalValue,
                                BORPostAssessedImprovedValue = b.PostAssessedImprovedValue,
                                BORPostAssessedLandValue = b.PostAssessedLandValue,
                                BORPostMarketValue = b.PostMarketValue,
                                BORPostTexableAssessedValue = b.PostTexableAssessedValue,
                                BORStateEVAValue = b.StateEVAValue,
                                BORTownShipGroupName = b.GroupName,
                                BORAppealFilingdate = b.BORAppealFilingdate,
                                BORProtestCodeValueId = b.BORProtestCodeValueId,
                                BORSettlingAttorneyId = b.BORSettlingAttorneyId,
                                BORProtestDetailsid = b.ProtestDetailsid,
                                BORFormReceivedDate = b.FormReceivedDate,
                                BORFormalHearingDate = b.FormalHearingDate,
                                BORFormalHearingTime = b.FormalHearingTime,
                                BORProtestReasonId = b.BORProtestReasonId,
                                ILGeneratedHearingResultBOR = b.ILGeneratedHearingResult
                            }
                        }).FirstOrDefault();
                    }

                    if (Hearintypeid == 3)
                    {
                        ILHearingDetails = dtILHearingDetails.Select(b => new PTXboILHearingDetails
                        {
                            ILHearingDetailsId = b.ILHearingDetailsId,
                            HearingTypeid = b.ILHearingTypeId,
                            YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                            PTABHearing = new PTXboILPTABHearingDetails()
                            {
                                PTABYearlyHearingDetailsId = b.YearlyHearingDetailsId,
                                ILPTABCompletionDate = b.ILCompletionDate,
                                ILPTABExemptInvoice = b.ILExemptInvoice,
                                ILPTABExemptReasonId = b.ILExemptReasonId,
                                ILPTABHearingAgentId = b.ILHearingAgentId,
                                ILPTABHearingDetailsId = b.ILHearingDetailsId,
                                ILPTABHearingFinalized = b.ILHearingFinalized,
                                ILPTABHearingFinalizedDate = b.ILHearingFinalizedDate,
                                ILPTABHearingTypeId = b.ILHearingTypeId,
                                ILPTABInvoiceDate = b.ILInvoiceDate,
                                ILPTABInvoiceId = b.ILInvoiceId,
                                ILPTABInvoiceStatusId = b.ILInvoiceStatusId,
                                ILPTABPreppedDate = b.ILPreppedDate,
                                PTABNoticeImprovedValue = b.NoticeImprovedValue,
                                PTABNoticeLandValue = b.NoticeLandValue,
                                PTABNoticeMarketValue = b.NoticeMarketValue,
                                PTABNoticeTotalValue = b.NoticeTotalValue,
                                PTABPostAssessedImprovedValue = b.PostAssessedImprovedValue,
                                PTABPostAssessedLandValue = b.PostAssessedLandValue,
                                PTABPostMarketValue = b.PostMarketValue,
                                PTABPostTexableAssessedValue = b.PostTexableAssessedValue,
                                PTABStateEVAValue = b.StateEVAValue,
                                PTABTownShipGroupName = b.GroupName,
                                ILPTABDeadlineDate = b.PTABDeadlineDate,
                                PTABProtestCodeValueId = b.PTABProtestCodeValueId,
                                PTABSettlingAttorneyId = b.PTABSettlingAttorneyId,
                                PTABBORCompletionDate = b.PTABBORCompletionDate,
                                PTABProtestDetailsid = b.ProtestDetailsid,
                                ILPTABFilingDate = b.PTABFilingDate,
                                DocketNumber = b.DocketNumber,
                                PTABProtestReasonId = b.PTABProtestReasonId,
                                ILGeneratedHearingResultPTAB = b.ILGeneratedHearingResult
                            }
                        }).FirstOrDefault();
                    }
                }
                return ILHearingDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public DataSet GetDMflag(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@AccountId", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                return _Connection.SelectDataSet("usp_GetDMflag",
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboPOAReceivedDateHistory> getPOAReceivedDateHistory(int ClientId)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@ClientId", ClientId);
                DataTable dtResult = new DataTable();
                return _Connection.Select<PTXboPOAReceivedDateHistory>("usp_GetPOAReceivedDateHistory",
                                      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboILHearingRemarks> GetIOWAHearingRemarks(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@TaxYear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);

                return _Connection.Select<PTXboILHearingRemarks>(StoredProcedureNames.USP_GetIOWAPropertyHearingRemarks,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboILHearingRemarks
                                   {
                                       ILHearingRemarkId = a.ILHearingRemarkId,
                                       ILHearingDetailsId = a.ILHearingDetailsId,
                                       HearingRemark = a.HearingRemark,
                                       CreatedDateTime = a.CreatedDateTime,
                                       UserName = a.UserName
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PTXboILAgentRemarks> GetIOWAAgentRemarks(PTXboParameters model)
        {
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);
                return _Connection.Select<PTXboILAgentRemarks>(StoredProcedureNames.USP_GetIOWAPropertyAgentRemarks,
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Select(a => new PTXboILAgentRemarks
                                   {
                                       ILAgentRemarkId = a.ILAgentRemarkId,
                                       ILHearingDetailsId = a.ILHearingDetailsId,
                                       AgentRemark = a.AgentRemark,
                                       CreatedDateTime = a.CreatedDateTime,
                                       UserName = a.UserName
                                   }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PTXboILHearingDetails GetIOWAHearingDetails(PTXboParameters model)
        {
            try
            {
                PTXboILHearingDetails ILHearingDetails = new PTXboILHearingDetails();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);

                var objDataTable = _Connection.SelectSingle(StoredProcedureNames.USP_GetIOWAHearingDetails, _hashtable,
                   Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                   gr => gr.Read<PTXboDVMILHearingDetails>()
                );
                return convertdtResultToIOWAHearingObject(objDataTable, ILHearingDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private PTXboILHearingDetails convertdtResultToIOWAHearingObject(dynamic SP_Result, PTXboILHearingDetails ILHearingDetails)
        {
            try
            {
                if (SP_Result != null)
                {
                    var dtILHearingDetails = (List<PTXboDVMILHearingDetails>)SP_Result.Item1;

                    int Hearintypeid = dtILHearingDetails.Select(b => b.ILHearingTypeId).First();
                    if (Hearintypeid == 1)
                    {
                        ILHearingDetails = dtILHearingDetails.Select(b => new PTXboILHearingDetails
                        {
                            ILHearingDetailsId = b.ILHearingDetailsId,
                            HearingTypeid = b.ILHearingTypeId,
                            YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                            AssosserHearing = new PTXboILAssessorHearingDetails()
                            {
                                YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                                AssessorConfirmationDate = b.AssessorConfirmationDate,
                                AssessorDeadlineDate = b.AssessorDeadlineDate,
                                AssessorStatusId = b.AssessorStatusId,
                                ILCompletionDate = b.ILCompletionDate,
                                ILExemptInvoice = b.ILExemptInvoice,
                                ILExemptReasonId = b.ILExemptReasonId,
                                ILHearingAgentId = b.ILHearingAgentId,
                                ILHearingDetailsId = b.ILHearingDetailsId,
                                ILHearingFinalized = b.ILHearingFinalized,
                                ILHearingFinalizedDate = b.ILHearingFinalizedDate,
                                ILHearingTypeId = b.ILHearingTypeId,
                                ILInvoiceDate = b.ILInvoiceDate,
                                ILInvoiceId = b.ILInvoiceId,
                                ILInvoiceStatusId = b.ILInvoiceStatusId,
                                ILPreppedDate = b.ILPreppedDate,
                                ILTargetValue = b.ILTargetValue,
                                NoticeImprovedValue = b.NoticeImprovedValue,
                                NoticeLandValue = b.NoticeLandValue,
                                NoticeMarketValue = b.NoticeMarketValue,
                                NoticeTotalValue = b.NoticeTotalValue,
                                PostAssessedImprovedValue = b.PostAssessedImprovedValue,
                                PostAssessedLandValue = b.PostAssessedLandValue,
                                PostMarketValue = b.PostMarketValue,
                                PostTexableAssessedValue = b.PostTexableAssessedValue,
                                PublicationDate = b.PublicationDate,
                                StateEVAValue = b.StateEVAValue,
                                TownShipGroupName = b.GroupName,
                                AssessorFilingdate = b.AssessorFilingdate,
                                AssessorProtestCodeValueId = b.AssessorProtestCodeValueId,
                                AssessorSettlingAttorneyId = b.AssessorSettlingAttorneyId,
                                AssessorProtestDetailsid = b.ProtestDetailsid,
                                AssessorFormReceivedDate = b.FormReceivedDate,
                                AssessorFormalHearingDate = b.FormalHearingDate,
                                AssessorFormalHearingTime = b.FormalHearingTime
                            }
                        }).FirstOrDefault();
                    }
                    if (Hearintypeid == 2)
                    {
                        ILHearingDetails = dtILHearingDetails.Select(b => new PTXboILHearingDetails
                        {
                            ILHearingDetailsId = b.ILHearingDetailsId,
                            HearingTypeid = b.ILHearingTypeId,
                            YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                            BORHearing = new PTXboILBORHearingDetails()
                            {
                                BORYearlyHearingDetailsId = b.YearlyHearingDetailsId,
                                BORFilingClosedDate = b.BORFilingClosedDate,
                                BORFilingOpenDate = b.BORFilingOpenDate,
                                BORStatusId = b.BORStatusId,
                                ILBORCompletionDate = b.ILCompletionDate,
                                ILBORExemptInvoice = b.ILExemptInvoice,
                                ILBORExemptReasonId = b.ILExemptReasonId,
                                ILBORHearingAgentId = b.ILHearingAgentId,
                                ILBORHearingDetailsId = b.ILHearingDetailsId,
                                ILBORHearingFinalized = b.ILHearingFinalized,
                                ILBORHearingFinalizedDate = b.ILHearingFinalizedDate,
                                ILBORHearingTypeId = b.ILHearingTypeId,
                                ILBORInvoiceDate = b.ILInvoiceDate,
                                ILBORInvoiceId = b.ILInvoiceId,
                                ILBORInvoiceStatusId = b.ILInvoiceStatusId,
                                ILBORPreppedDate = b.ILPreppedDate,
                                ILBORTargetValue = b.ILTargetValue,
                                BORNoticeImprovedValue = b.NoticeImprovedValue,
                                BORNoticeLandValue = b.NoticeLandValue,
                                BORNoticeMarketValue = b.NoticeMarketValue,
                                BORNoticeTotalValue = b.NoticeTotalValue,
                                BORPostAssessedImprovedValue = b.PostAssessedImprovedValue,
                                BORPostAssessedLandValue = b.PostAssessedLandValue,
                                BORPostMarketValue = b.PostMarketValue,
                                BORPostTexableAssessedValue = b.PostTexableAssessedValue,
                                BORStateEVAValue = b.StateEVAValue,
                                BORTownShipGroupName = b.GroupName,
                                BORAppealFilingdate = b.BORAppealFilingdate,
                                BORProtestCodeValueId = b.BORProtestCodeValueId,
                                BORSettlingAttorneyId = b.BORSettlingAttorneyId,
                                BORProtestDetailsid = b.ProtestDetailsid,
                                BORFormReceivedDate = b.FormReceivedDate,
                                BORFormalHearingDate = b.FormalHearingDate,
                                BORFormalHearingTime = b.FormalHearingTime

                            }
                        }).FirstOrDefault();
                    }

                    if (Hearintypeid == 3)
                    {
                        ILHearingDetails = dtILHearingDetails.Select(b => new PTXboILHearingDetails
                        {
                            ILHearingDetailsId = b.ILHearingDetailsId,
                            HearingTypeid = b.ILHearingTypeId,
                            YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                            PTABHearing = new PTXboILPTABHearingDetails()
                            {
                                PTABYearlyHearingDetailsId = b.YearlyHearingDetailsId,
                                ILPTABCompletionDate = b.ILCompletionDate,
                                ILPTABExemptInvoice = b.ILExemptInvoice,
                                ILPTABExemptReasonId = b.ILExemptReasonId,
                                ILPTABHearingAgentId = b.ILHearingAgentId,
                                ILPTABHearingDetailsId = b.ILHearingDetailsId,
                                ILPTABHearingFinalized = b.ILHearingFinalized,
                                ILPTABHearingFinalizedDate = b.ILHearingFinalizedDate,
                                ILPTABHearingTypeId = b.ILHearingTypeId,
                                ILPTABInvoiceDate = b.ILInvoiceDate,
                                ILPTABInvoiceId = b.ILInvoiceId,
                                ILPTABInvoiceStatusId = b.ILInvoiceStatusId,
                                ILPTABPreppedDate = b.ILPreppedDate,
                                PTABNoticeImprovedValue = b.NoticeImprovedValue,
                                PTABNoticeLandValue = b.NoticeLandValue,
                                PTABNoticeMarketValue = b.NoticeMarketValue,
                                PTABNoticeTotalValue = b.NoticeTotalValue,
                                PTABPostAssessedImprovedValue = b.PostAssessedImprovedValue,
                                PTABPostAssessedLandValue = b.PostAssessedLandValue,
                                PTABPostMarketValue = b.PostMarketValue,
                                PTABPostTexableAssessedValue = b.PostTexableAssessedValue,
                                PTABStateEVAValue = b.StateEVAValue,
                                PTABTownShipGroupName = b.GroupName,
                                ILPTABDeadlineDate = b.PTABDeadlineDate,
                                PTABProtestCodeValueId = b.PTABProtestCodeValueId,
                                PTABSettlingAttorneyId = b.PTABSettlingAttorneyId,
                                PTABBORCompletionDate = b.PTABBORCompletionDate,
                                PTABProtestDetailsid = b.ProtestDetailsid,
                                ILPTABFilingDate = b.PTABFilingDate,
                                DocketNumber = b.DocketNumber,
                                PTABFormalHearingDate = b.FormalHearingDate,
                                PTABFormalHearingTime = b.FormalHearingTime
                            }
                        }).FirstOrDefault();
                    }
                }
                return ILHearingDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public PTXboGAHearingDetails GetGAHearingDetails(PTXboParameters model)
        {
            try
            {
                PTXboGAHearingDetails GAHearingDetails = new PTXboGAHearingDetails();
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@accountID", model.Value1);
                _hashtable.Add("@Taxyear", model.Value2);
                _hashtable.Add("@HearingTypeId", model.Value3);

                var objDataTable = _Connection.SelectSingle(StoredProcedureNames.USP_GetGAHearingDetails, _hashtable,
                   Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                   gr => gr.Read<PTXboDVMGAHearingDetails>()
                );
                //var objDataTable = _Connection.SelectDataSet(StoredProcedureNames.USP_GetILHearingDetails,
                //      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                //var objDataTable = _Connection.Select<PTXboDVMILHearingDetails>(StoredProcedureNames.USP_GetILHearingDetails,
                //      _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return convertdtResultToGAHearingObject(objDataTable, GAHearingDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private PTXboGAHearingDetails convertdtResultToGAHearingObject(dynamic SP_Result, PTXboGAHearingDetails GAHearingDetails)
        {
            try
            {
                if (SP_Result != null)
                {
                    var dtGAHearingDetails = (List<PTXboDVMGAHearingDetails>)SP_Result.Item1;

                    int GAHearintypeid = dtGAHearingDetails.Select(b => b.GAHearingTypeId).First();
                    if (GAHearintypeid == 1)
                    {
                        GAHearingDetails = dtGAHearingDetails.Select(b => new PTXboGAHearingDetails
                        {
                            GAHearingDetailsId = b.GAHearingDetailsId,
                            GAHearingTypeid = b.GAHearingTypeId,
                            GAYearlyHearingDetailsId = b.GAYearlyHearingDetailsId,
                            GAAssosserHearing = new PTXboGAAssessorHearingDetails()
                            {
                                GAAssessorProtestCodeValueId = b.GAAssessorProtestCodeValueId,
                                GAAssessorProtestModeValueID = b.GAAssessorProtestModeValueID,
                                GAProtestFilingDate = b.GAProtestFilingDate,
                                GANoticeLandValue = b.GANoticeLandValue,
                                GAPostAssessedLandValue = b.GAPostAssessedLandValue,
                                GAAssessmentNoticeDate = b.GAAssessmentNoticeDate,
                                GANoticeImprovedValue = b.GANoticeImprovedValue,
                                GAPostAssessedImprovedValue = b.GAPostAssessedImprovedValue,
                                GAAssessmentReceivedDate = b.GAAssessmentReceivedDate,
                                GANoticeMarketValue = b.GANoticeMarketValue,
                                GAPostMarketValue = b.GAPostMarketValue,
                                GAAssessmentDeadlineDate = b.GAAssessmentDeadlineDate,
                                GANoticeTotalValue = b.GANoticeTotalValue,
                                GAPostTexableAssessedValue = b.GAPostTexableAssessedValue,
                                GAReAppealDate = b.GAReAppealDate,
                                GAHearingAgentId = b.GAHearingAgentId,
                                GACompletionDate = b.GACompletionDate,
                                GAReAppealDeadlineDate = b.GAReAppealDeadlineDate,
                                GAAssessorStatusId = b.GAAssessorStatusId,
                                GAHearingFinalized = b.GAHearingFinalized,
                                GAOpinionValue = b.GAOpinionValue,
                                GAHearingFinalizedDate = b.GAHearingFinalizedDate,
                                GAInvoiceStatusId = b.GAInvoiceStatusId,
                                GAInvoiceId = b.GAInvoiceId,
                                GAInvoiceDate = b.GAInvoiceDate,
                                GAExemptInvoice = b.GAExemptInvoice,
                                GAExemptReasonId = b.GAExemptReasonId,
                                GAAgentRemark = b.GAAgentRemark


                                //YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                                //AssessorConfirmationDate = b.AssessorConfirmationDate,
                                //AssessorDeadlineDate = b.AssessorDeadlineDate,
                                //AssessorStatusId = b.AssessorStatusId,
                                //ILCompletionDate = b.GACompletionDate,
                                //ILExemptInvoice = b.GAExemptInvoice,
                                //ILExemptReasonId = b.GAExemptReasonId,
                                //ILHearingAgentId = b.GAHearingAgentId,
                                //ILHearingDetailsId = b.GAHearingDetailsId,
                                //ILHearingFinalized = b.GAHearingFinalized,
                                //ILHearingFinalizedDate = b.GAHearingFinalizedDate,
                                //ILHearingTypeId = b.GAHearingTypeId,
                                //ILInvoiceDate = b.GAInvoiceDate,
                                //ILInvoiceId = b.GAInvoiceId,
                                //ILInvoiceStatusId = b.GAInvoiceStatusId,
                                //ILPreppedDate = b.GAPreppedDate,
                                //ILTargetValue = b.GATargetValue,
                                //NoticeImprovedValue = b.NoticeImprovedValue,
                                //NoticeLandValue = b.NoticeLandValue,
                                //NoticeMarketValue = b.NoticeMarketValue,
                                //NoticeTotalValue = b.NoticeTotalValue,
                                //PostAssessedImprovedValue = b.PostAssessedImprovedValue,
                                //PostAssessedLandValue = b.PostAssessedLandValue,
                                //PostMarketValue = b.PostMarketValue,
                                //PostTexableAssessedValue = b.PostTexableAssessedValue,
                                //PublicationDate = b.PublicationDate,
                                //StateEVAValue = b.StateEVAValue,
                                //TownShipGroupName = b.GroupName,
                                //AssessorFilingdate = b.AssessorFilingdate,
                                //GAAssessorProtestCodeValueId = b.AssessorProtestCodeValueId,
                                //AssessorSettlingAttorneyId = b.AssessorSettlingAttorneyId,
                                //AssessorProtestDetailsid = b.ProtestDetailsid,
                                //AssessorFormReceivedDate = b.FormReceivedDate,
                                //GAProtestModeValueID = b.GAProtestModeValueID
                                //ILGeneratedHearingResultAssessor = b.ILGeneratedHearingResult

                            }
                        }).FirstOrDefault();
                    }
                    //if (Hearintypeid == 2)
                    //{
                    //    GAHearingDetails = dtGAHearingDetails.Select(b => new PTXboGAHearingDetails
                    //    {
                    //        GAHearingDetailsId = b.GAHearingDetailsId
                    //            HearingTypeid = b.ILHearingTypeId,
                    //        YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                    //        BORHearing = new PTXboILBORHearingDetails()
                    //        {
                    //            BORYearlyHearingDetailsId = b.YearlyHearingDetailsId,
                    //            BORFilingClosedDate = b.BORFilingClosedDate,
                    //            BORFilingOpenDate = b.BORFilingOpenDate,
                    //            BORStatusId = b.BORStatusId,
                    //            ILBORCompletionDate = b.ILCompletionDate,
                    //            ILBORExemptInvoice = b.ILExemptInvoice,
                    //            ILBORExemptReasonId = b.ILExemptReasonId,
                    //            ILBORHearingAgentId = b.ILHearingAgentId,
                    //            ILBORHearingDetailsId = b.ILHearingDetailsId,
                    //            ILBORHearingFinalized = b.ILHearingFinalized,
                    //            ILBORHearingFinalizedDate = b.ILHearingFinalizedDate,
                    //            ILBORHearingTypeId = b.ILHearingTypeId,
                    //            ILBORInvoiceDate = b.ILInvoiceDate,
                    //            ILBORInvoiceId = b.ILInvoiceId,
                    //            ILBORInvoiceStatusId = b.ILInvoiceStatusId,
                    //            ILBORPreppedDate = b.ILPreppedDate,
                    //            ILBORTargetValue = b.ILTargetValue,
                    //            BORNoticeImprovedValue = b.NoticeImprovedValue,
                    //            BORNoticeLandValue = b.NoticeLandValue,
                    //            BORNoticeMarketValue = b.NoticeMarketValue,
                    //            BORNoticeTotalValue = b.NoticeTotalValue,
                    //            BORPostAssessedImprovedValue = b.PostAssessedImprovedValue,
                    //            BORPostAssessedLandValue = b.PostAssessedLandValue,
                    //            BORPostMarketValue = b.PostMarketValue,
                    //            BORPostTexableAssessedValue = b.PostTexableAssessedValue,
                    //            BORStateEVAValue = b.StateEVAValue,
                    //            BORTownShipGroupName = b.GroupName,
                    //            BORAppealFilingdate = b.BORAppealFilingdate,
                    //            BORProtestCodeValueId = b.BORProtestCodeValueId,
                    //            BORSettlingAttorneyId = b.BORSettlingAttorneyId,
                    //            BORProtestDetailsid = b.ProtestDetailsid,
                    //            BORFormReceivedDate = b.FormReceivedDate,
                    //            BORFormalHearingDate = b.FormalHearingDate,
                    //            BORFormalHearingTime = b.FormalHearingTime,
                    //            BORProtestReasonId = b.BORProtestReasonId,
                    //            ILGeneratedHearingResultBOR = b.ILGeneratedHearingResult
                    //        }
                    //    }).FirstOrDefault();
                    //}

                    //if (Hearintypeid == 3)
                    //{
                    //    GAHearingDetails = dtGAHearingDetails.Select(b => new PTXboGAHearingDetails
                    //    {
                    //        GAHearingDetailsId = b.GAHearingDetailsId
                    //            HearingTypeid = b.ILHearingTypeId,
                    //        YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                    //        PTABHearing = new PTXboILPTABHearingDetails()
                    //        {
                    //            PTABYearlyHearingDetailsId = b.YearlyHearingDetailsId,
                    //            ILPTABCompletionDate = b.ILCompletionDate,
                    //            ILPTABExemptInvoice = b.ILExemptInvoice,
                    //            ILPTABExemptReasonId = b.ILExemptReasonId,
                    //            ILPTABHearingAgentId = b.ILHearingAgentId,
                    //            ILPTABHearingDetailsId = b.ILHearingDetailsId,
                    //            ILPTABHearingFinalized = b.ILHearingFinalized,
                    //            ILPTABHearingFinalizedDate = b.ILHearingFinalizedDate,
                    //            ILPTABHearingTypeId = b.ILHearingTypeId,
                    //            ILPTABInvoiceDate = b.ILInvoiceDate,
                    //            ILPTABInvoiceId = b.ILInvoiceId,
                    //            ILPTABInvoiceStatusId = b.ILInvoiceStatusId,
                    //            ILPTABPreppedDate = b.ILPreppedDate,
                    //            PTABNoticeImprovedValue = b.NoticeImprovedValue,
                    //            PTABNoticeLandValue = b.NoticeLandValue,
                    //            PTABNoticeMarketValue = b.NoticeMarketValue,
                    //            PTABNoticeTotalValue = b.NoticeTotalValue,
                    //            PTABPostAssessedImprovedValue = b.PostAssessedImprovedValue,
                    //            PTABPostAssessedLandValue = b.PostAssessedLandValue,
                    //            PTABPostMarketValue = b.PostMarketValue,
                    //            PTABPostTexableAssessedValue = b.PostTexableAssessedValue,
                    //            PTABStateEVAValue = b.StateEVAValue,
                    //            PTABTownShipGroupName = b.GroupName,
                    //            ILPTABDeadlineDate = b.PTABDeadlineDate,
                    //            PTABProtestCodeValueId = b.PTABProtestCodeValueId,
                    //            PTABSettlingAttorneyId = b.PTABSettlingAttorneyId,
                    //            PTABBORCompletionDate = b.PTABBORCompletionDate,
                    //            PTABProtestDetailsid = b.ProtestDetailsid,
                    //            ILPTABFilingDate = b.PTABFilingDate,
                    //            DocketNumber = b.DocketNumber,
                    //            PTABProtestReasonId = b.PTABProtestReasonId,
                    //            ILGeneratedHearingResultPTAB = b.ILGeneratedHearingResult
                    //        }
                    //    }).FirstOrDefault();
                    //}
                }
                return GAHearingDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

    }

    public static class ConvertDatableToList
    {
        /* Soruce: CodeProject. Link: http://www.codeproject.com/Tips/195889/Convert-Datatable-to-Collection-using-Generic */

        public static List<T> ToCollection<T>(this DataTable dt)
        {
            List<T> lst = new System.Collections.Generic.List<T>();
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(tClass);
                foreach (PropertyInfo pc in pClass)
                {
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName.ToLower().Equals(pc.Name.ToLower())); /* All the field names in the dataobject class should be same as the column name in select query. */
                        if (d != null)
                        {
                            if (item[pc.Name] == System.DBNull.Value)
                            {
                                pc.SetValue(cn, null, null);
                            }
                            else if (pc.PropertyType.FullName.Contains("TimeSpan"))
                            {
                                TimeSpan time = TimeSpan.Parse(item[pc.Name].ToString());

                                var underlyingType = time.GetType();

                                pc.SetValue(cn, Convert.ChangeType(time, underlyingType), null);
                            }
                            else
                            {
                                Type originalType = Type.GetType(pc.PropertyType.ToString());

                                var underlyingType = Nullable.GetUnderlyingType(originalType);

                                pc.SetValue(cn, Convert.ChangeType(item[pc.Name], underlyingType ?? originalType), null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                lst.Add(cn);
            }
            return lst;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        //public static List<T> ToCustomList<T>(this DataTable dt) where T : new()
        //{
        //    List<T> lst = new System.Collections.Generic.List<T>();
        //    Type tClass = typeof(T);
        //    PropertyInfo[] pClass = tClass.GetProperties();
        //    List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
        //    T cn;
        //    foreach (DataRow item in dt.Rows)
        //    {
        //        cn = (T)Activator.CreateInstance(tClass);
        //        foreach (PropertyInfo pc in pClass)
        //        {
        //            try
        //            {
        //                DataColumn d = dc.Find(c => string.Equals(c.ColumnName, pc.Name, StringComparison.OrdinalIgnoreCase));
        //                if (d != null)
        //                {
        //                    if (item[pc.Name] == System.DBNull.Value)
        //                    {
        //                        pc.SetValue(cn, null, null);
        //                    }
        //                    else
        //                    {
        //                        Type originalType = Type.GetType(pc.PropertyType.ToString());

        //                        var underlyingType = Nullable.GetUnderlyingType(originalType);

        //                        pc.SetValue(cn, Convert.ChangeType(item[pc.Name], underlyingType ?? originalType), null);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        lst.Add(cn);
        //    }
        //    return lst;
        //}
    }
    public class PTXdtGroupandResendPackageDetails
    {
        public int TermsId { get; set; }
        public int? GroupId { get; set; }
        public int? ClientId { get; set; }
        //public int? GroupTypeId { get; set; }
        public string GroupName { get; set; }
        public int TermsTypeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Remarks { get; set; }
        public Single Contingency { get; set; }
        public decimal FlatFee { get; set; }
        public string ExpiryRemarks { get; set; }
        public float ContingencyAfterExpiry { get; set; }
        public decimal FlatFeeAfterExpiry { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string TermsType { get; set; }
        public bool IsDefault { get; set; }
        public string TermExpiryAction { get; set; }
        public int TermExpiryActionID { get; set; }
        public bool CanEnableGroup { get; set; }

        public virtual string AgreementStatusDescription { get; set; }
        public virtual string AgreementStatusCode { get; set; }
        public virtual string recentPackageSent { get; set; }
        public virtual string sentBy { get; set; }
        public virtual DateTime? sentDate { get; set; }

    }
}
