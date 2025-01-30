using Newtonsoft.Json;
using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using Spartaxx.Common;
using Spartaxx.Common.BusinessObjects;
using Spartaxx.Common.Reports;
using Spartaxx.DataObjects;
using Spartaxx.DataObjects.NHibernate.DataObjects;
using Spartaxx.Utilities;
using Spartaxx.Utilities.Email;
using Spartaxx.Utilities.Extenders;
using Spartaxx.Utilities.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Spartaxx.DataAccess
{
   public class InvoiceRepository:IInvoiceRepository
    {
        private readonly DapperConnection _connection;
        private readonly InvoiceCalculationRepository _invoiceCalculation; //Invoice calculation engine
        public InvoiceRepository(DapperConnection Connection)
        {
            _connection = Connection;
            _invoiceCalculation = new InvoiceCalculationRepository(Connection);
        }

        public List<PTXboInvoiceDetails> GetInvoiceGenerationDetails(PTXboInvoiceSearchCriteria objSearchCriteria, bool? isInvoiceDefects = null)
        {
            List<PTXboInvoiceDetails>  lstInvoiceDetails = new List<PTXboInvoiceDetails>();
            lstInvoiceDetails = null;
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();

            try
            {
                Logger.For(this).Invoice("GetInvoiceGenerationDetails-API  reached " + ((object)objSearchCriteria).ToJson(false));

                _hashtable.Add("@InvoiceID", objSearchCriteria.InvoiceId);
                _hashtable.Add("@AccountNumber", objSearchCriteria.AccountNumber);
                _hashtable.Add("@ClientNumber", objSearchCriteria.ClientNumber);
                _hashtable.Add("@County", objSearchCriteria.County);
                _hashtable.Add("@FromValue", objSearchCriteria.FromValue);
                _hashtable.Add("@HearingAgent", objSearchCriteria.HearingAgent);
                _hashtable.Add("@HearingType", objSearchCriteria.HearingType);
                _hashtable.Add("@Project", objSearchCriteria.Project);
                _hashtable.Add("@PropertyType", objSearchCriteria.PropertyType);
                _hashtable.Add("@SelectedValueType", objSearchCriteria.SelectedValueType);
                _hashtable.Add("@TaxYear", objSearchCriteria.TaxYear);
                _hashtable.Add("@ToValue", objSearchCriteria.TotalValue);
                _hashtable.Add("@IsQ", objSearchCriteria.IsQ);
                _hashtable.Add("@InvoiceProcessingStatusId", objSearchCriteria.InvoiceProcessingStatusId);
                _hashtable.Add("@InvoiceGroupingTypeId", objSearchCriteria.InvoiceGroupingType);
                

                if (objSearchCriteria.IsDisasterInvoice)
                {
                    _hashtable.Add("@InvoiceTypeId", objSearchCriteria.InvoiceType);
                    SPName = StoredProcedureNames.usp_getDisasterInvoiceDetails;
                }
                else if (objSearchCriteria.IsOutOfTexas == true) //added by saravanans-tfs:48138
                {
                    _hashtable.Add("@InvoiceTypeId", objSearchCriteria.InvoiceType);
                    _hashtable.Add("@IsRegeneration", objSearchCriteria.IsRegeneration);//added by saravanans-tfs:63342
                    if (objSearchCriteria.IsMultiyear == true)
                    {
                        SPName = StoredProcedureNames.usp_getInvoiceDetailsForReport_Regular_Multiyear;
                    }
                    else
                    {
                        SPName = StoredProcedureNames.usp_getInvoiceDetailsForReport_Regular_OT;
                    }

                    //SPName = StoredProcedureNames.usp_invoiceregularqueue_standard_OT;
                }
                else if (objSearchCriteria.InvoiceType ==2)
                {
                    _hashtable.Add("@IsRegeneration", objSearchCriteria.IsRegeneration);
                    //SPName = StoredProcedureNames.usp_getInvoiceDetailsForReportLitigation;
                    SPName = StoredProcedureNames.usp_invoiceregularqueue_litigation;
                }
                else if (objSearchCriteria.InvoiceType == 3)
                {
                    _hashtable.Add("@IsRegeneration", objSearchCriteria.IsRegeneration);
                    SPName = StoredProcedureNames.usp_invoiceregularqueue_arbitration;
                    //SPName = StoredProcedureNames.usp_invoiceregularaccountlevel;
                }
                else if (objSearchCriteria.InvoiceType == 6)
                {
                    SPName = StoredProcedureNames.usp_getInvoiceDetailsForReportBppRendition;
                    //SPName = StoredProcedureNames.usp_invoiceregularaccountlevel;

                }
                else if (objSearchCriteria.InvoiceType == 4)
                {
                    SPName = StoredProcedureNames.usp_getInvoiceDetailsForReportTaxBill;
                    //SPName = StoredProcedureNames.usp_invoiceregularaccountlevel;

                }
                //Added by SaravananS. tfs id:59604
                else if (objSearchCriteria.InvoiceType == 8)
                {
                    _hashtable.Add("@IsRegeneration", objSearchCriteria.IsRegeneration);//added by saravanans-tfs:63342
                    SPName = StoredProcedureNames.usp_getInvoiceDetailsForReport_SOAH;
                }
                //Ends here.
                else
                {     //SPName = StoredProcedureNames.usp_getInvoiceDetailsForReportStandard;
                    _hashtable.Add("@IsRegeneration", objSearchCriteria.IsRegeneration);
                    SPName = StoredProcedureNames.usp_invoiceregularqueue_standard;

                }

                lstInvoiceDetails = _connection.Select<PTXboInvoiceDetails>(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("GetInvoiceGenerationDetails-API  ends successfully ");

                return lstInvoiceDetails;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceGenerationDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
            
        }


        public PTXboInvoice GetSpecificInvoiceGenerationDetails(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceGenerationDetails-API  reached " + ((object)invoiceID).ToJson(false));
                //Hashtable parameters = new Hashtable();
                //PTXboInvoice invoiceDetails = new PTXboInvoice();
                //parameters.Add("@InvoiceID", invoiceID);
                //invoiceDetails = _connection.Select<PTXboInvoice>(StoredProcedureNames.usp_get_specificinvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                var invoiceDetails = _invoiceCalculation.GetSpecificInvoiceGenerationDetails(invoiceID);
                Logger.For(this).Invoice("GetInvoiceGenerationDetails-API  ends successfully ");
                return invoiceDetails;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceGenerationDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboInvoice GetInvoiceDetailsById(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceDetailsById-API  reached " + ((object)invoiceID).ToJson(false));
                //Hashtable parameters = new Hashtable();
                PTXboInvoice invoiceDetails = new PTXboInvoice();
                //parameters.Add("@InvoiceID", invoiceID);
                //invoiceDetails = _connection.Select<PTXboInvoice>(StoredProcedureNames.usp_get_InvoicedetailsbyId, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                invoiceDetails = _invoiceCalculation.GetInvoiceDetailsById(invoiceID).FirstOrDefault() ;
                Logger.For(this).Invoice("GetInvoiceDetailsById-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceDetailsById-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public List<PTXboInvoiceBasicDetails> GetInvoiceBasicDetails(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceDetailsById-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                PTXboInvoice invoiceDetails = new PTXboInvoice();
                parameters.Add("@InvoiceID", invoiceID);
                var invoiceBasicDetails = _connection.Select<PTXboInvoiceBasicDetails>(StoredProcedureNames.usp_getInvoiceBasicDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                
                Logger.For(this).Invoice("GetInvoiceDetailsById-API  ends successfully ");
                return invoiceBasicDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceDetailsById-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public bool SubmitOutOfTexasInvoice(PTXboOutOfTexasInvoiceDetails outOfTexasInvoice)
        {
            try
            {
                Logger.For(this).Invoice("SubmitOutOfTexasInvoice-API  reached " + ((object)outOfTexasInvoice).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@INVOICEID", outOfTexasInvoice.InvoiceId);
                parameters.Add("@CLIENTNUMBER", outOfTexasInvoice.ClientNumber);
                parameters.Add("@ACCOUNTNUMBER", outOfTexasInvoice.AccountNumber);
                parameters.Add("@PROJECTNAME", outOfTexasInvoice.ProjectName);
                parameters.Add("@INVOICETYPE", outOfTexasInvoice.InvoiceType);
                parameters.Add("@TAXYEAR", outOfTexasInvoice.TaxYear);
                parameters.Add("@CONTINGENCYPERCENTAGE", outOfTexasInvoice.ContingencyPercentage);
                parameters.Add("@CONTINGENCYFEE", outOfTexasInvoice.ContingencyFee);
                parameters.Add("@FLATFEE", outOfTexasInvoice.Flatfee);
                parameters.Add("@CAPVALUE", outOfTexasInvoice.Capvalue);
                parameters.Add("@MINIMUMAMOUNT", outOfTexasInvoice.MinimumAmount);
                parameters.Add("@INITIALMARKETVALUE", outOfTexasInvoice.InitialMarketValue);
                parameters.Add("@FINALMARKETVALUE", outOfTexasInvoice.FinalMarketValue);
                parameters.Add("@MARKETVALUEREDUCTION", outOfTexasInvoice.MarketValueReduction);
                parameters.Add("@ASSESSMENTRATIO", outOfTexasInvoice.AssessmentRatio);
                parameters.Add("@ASSESMENTVALUEREDUCTION", outOfTexasInvoice.AssessmentValueReduction);
                parameters.Add("@TAXRATEMILLAGERATE", outOfTexasInvoice.MillageRate);
                parameters.Add("@ESTIMATEDTAXSAVINGS", outOfTexasInvoice.EstimatedTaxSavings);
                parameters.Add("@AMOUNTPAIDTOOCA", outOfTexasInvoice.AmountPaidToOca);
                parameters.Add("@AMOUNTPAIDDIRECTLYTOATTORNEY", outOfTexasInvoice.AmountPaidDirectlyToAttorney);
                parameters.Add("@CACULATEDINVOICEAMOUNT", outOfTexasInvoice.CalculatedInvoiceAmount);
                parameters.Add("@PaymentDuration", outOfTexasInvoice.PaymentDuration);
                parameters.Add("@NoticeTax", outOfTexasInvoice.NoticeTax);
                parameters.Add("@FinalTax", outOfTexasInvoice.FinalTax);
                parameters.Add("@IsMultiyear", outOfTexasInvoice.IsMultiYear);

                parameters.Add("@AdministrativeAppeal", outOfTexasInvoice.AdministrativeAppeal);
                parameters.Add("@JudicialAppeal", outOfTexasInvoice.JudicialAppeal);
                parameters.Add("@TotalBaseYearInvoice", outOfTexasInvoice.TotalBaseYearInvoice);
                parameters.Add("@FeeFactor", outOfTexasInvoice.FeeFactor);
                //Added by SaravananS.tfs id:55736
                parameters.Add("@IntialTaxableValue", outOfTexasInvoice.TotalInitialTaxableValue);
                parameters.Add("@FinalTaxableValue", outOfTexasInvoice.TotalFinalTaxableValue);
                parameters.Add("@IsTaxableCalculation", outOfTexasInvoice.IsTaxableCalculation);
                parameters.Add("@Descriptions", outOfTexasInvoice.Descriptions);
                //Ends here.

                //Added by SaravananS. tfs id:58575
                parameters.Add("@StateEVAValue", outOfTexasInvoice.StateEVAValue);
                //Ends here.
                //Added by SaravananS. tfs id:63221
                parameters.Add("@InitialAssessedValue", outOfTexasInvoice.InitialAssessedValue);
                parameters.Add("@FinalAssessedValue", outOfTexasInvoice.FinalAssessedValue);
                //Ends here.


                parameters.Add("@SeniorFreezeValue", outOfTexasInvoice.SeniorFreezeValue); //Added by Mohanapriya s. tfs id: 64616

                var result = Convert.ToBoolean(_connection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateOUTOFTEXASINVOICES, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));

                Logger.For(this).Invoice("SubmitOutOfTexasInvoice-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SubmitOutOfTexasInvoice-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public PTXboOutOfTexasInvoiceDetails SubmitOutOfTexasProjectLevelInvoice(PTXboOutOfTexasInvoiceDetails outOfTexasInvoice)
        {
            try
            {
                Logger.For(this).Invoice("SubmitOutOfTexasInvoice-API  reached " + ((object)outOfTexasInvoice).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@INVOICEID", outOfTexasInvoice.InvoiceId);
                parameters.Add("@CLIENTNUMBER", outOfTexasInvoice.ClientNumber);
                parameters.Add("@ACCOUNTNUMBER", outOfTexasInvoice.AccountNumber);
                parameters.Add("@PROJECTNAME", outOfTexasInvoice.ProjectName);
                parameters.Add("@INVOICETYPE", outOfTexasInvoice.InvoiceType);
                parameters.Add("@TAXYEAR", outOfTexasInvoice.TaxYear);
                parameters.Add("@CONTINGENCYPERCENTAGE", outOfTexasInvoice.ContingencyPercentage);
                parameters.Add("@CONTINGENCYFEE", outOfTexasInvoice.ContingencyFee);
                parameters.Add("@FLATFEE", outOfTexasInvoice.Flatfee);
                parameters.Add("@CAPVALUE", outOfTexasInvoice.Capvalue);
                parameters.Add("@MINIMUMAMOUNT", outOfTexasInvoice.MinimumAmount);
                parameters.Add("@INITIALMARKETVALUE", outOfTexasInvoice.InitialMarketValue);
                parameters.Add("@FINALMARKETVALUE", outOfTexasInvoice.FinalMarketValue);
                parameters.Add("@MARKETVALUEREDUCTION", outOfTexasInvoice.MarketValueReduction);
                parameters.Add("@ASSESSMENTRATIO", outOfTexasInvoice.AssessmentRatio);
                parameters.Add("@ASSESMENTVALUEREDUCTION", outOfTexasInvoice.AssessmentValueReduction);
                parameters.Add("@TAXRATEMILLAGERATE", outOfTexasInvoice.MillageRate);
                parameters.Add("@ESTIMATEDTAXSAVINGS", outOfTexasInvoice.EstimatedTaxSavings);
                parameters.Add("@AMOUNTPAIDTOOCA", outOfTexasInvoice.AmountPaidToOca);
                parameters.Add("@AMOUNTPAIDDIRECTLYTOATTORNEY", outOfTexasInvoice.AmountPaidDirectlyToAttorney);
                parameters.Add("@CACULATEDINVOICEAMOUNT", outOfTexasInvoice.CalculatedInvoiceAmount);
                parameters.Add("@PaymentDuration", outOfTexasInvoice.PaymentDuration);
                parameters.Add("@NoticeTax", outOfTexasInvoice.NoticeTax);
                parameters.Add("@FinalTax", outOfTexasInvoice.FinalTax);
                parameters.Add("@IsMultiyear", outOfTexasInvoice.IsMultiYear);

                parameters.Add("@AdministrativeAppeal", outOfTexasInvoice.AdministrativeAppeal);
                parameters.Add("@JudicialAppeal", outOfTexasInvoice.JudicialAppeal);
                parameters.Add("@TotalBaseYearInvoice", outOfTexasInvoice.TotalBaseYearInvoice);
                parameters.Add("@FeeFactor", outOfTexasInvoice.FeeFactor);
                //Added by SaravananS.tfs id:55736
                parameters.Add("@IntialTaxableValue", outOfTexasInvoice.TotalInitialTaxableValue);
                parameters.Add("@FinalTaxableValue", outOfTexasInvoice.TotalFinalTaxableValue);
                parameters.Add("@IsTaxableCalculation", outOfTexasInvoice.IsTaxableCalculation);
                parameters.Add("@Descriptions", outOfTexasInvoice.Descriptions);
                //Ends here.

                //Added by SaravananS. tfs id:58575
                parameters.Add("@StateEVAValue", outOfTexasInvoice.StateEVAValue);
                //Ends here.
                //Added by SaravananS. tfs id:63221
                parameters.Add("@InitialAssessedValue", outOfTexasInvoice.InitialAssessedValue);
                parameters.Add("@FinalAssessedValue", outOfTexasInvoice.FinalAssessedValue);
                //Ends here.

                var InvoiceDetails = _connection.Select<PTXboOutOfTexasInvoiceDetails>(StoredProcedureNames.usp_SaveOrUpdateOUTOFTEXASINVOICES, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Single();

                Logger.For(this).Invoice("SubmitOutOfTexasInvoice-API  ends successfully ");
                return InvoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SubmitOutOfTexasInvoice-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboInvoiceLineItem GetInvoiceLineItemByInvoiceId(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceLineItemByInvoiceId-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                PTXboInvoiceLineItem invoiceDetails = new PTXboInvoiceLineItem();
                parameters.Add("@InvoiceID", invoiceID);
                invoiceDetails = _connection.Select<PTXboInvoiceLineItem>(StoredProcedureNames.InvoiceLineItembyInvoiceId, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetInvoiceLineItemByInvoiceId-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceLineItemByInvoiceId-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public bool GetCommonListDefinitions(ref PXTboListDefinition objListDefinition)
        {
            List<PXTboListDefinition> listDefinition = new List<PXTboListDefinition>();
            try
            {
                Hashtable parameters = new Hashtable();

                parameters.Add("@TokenID", objListDefinition.TokenID);
                //listDefinition = _connection.Select<PXTboListDefinition>(StoredProcedureNames.usp_get_AllReqListDefinitions, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                //return listDefinition;

                /* Get required result from SP */
                DataSet dsResult = _connection.SelectDataSet(StoredProcedureNames.usp_get_AllReqListDefinitions, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);


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
                                case "AOFADISPOSITION":
                                    objListDefinition.AofADisposition = dsResult.Tables[iCount].ToCollection<PTXdoAofADisposition>();
                                    break;
                                case "NONCONTACTFLAG":
                                    objListDefinition.NonContactCode = dsResult.Tables[iCount].ToCollection<PTXdoNonContactCode>();
                                    break;
                                //case "COLLECTIONAGENT":                                    
                                //    objListDefinition.ReportCollectionAgent = dsResult.Tables[iCount].ToCollection<PTXdoAgent>();
                                //    break;
                                case "AUDITREPORT":
                                    objListDefinition.ReportType = dsResult.Tables[iCount].ToCollection<PTXdoAuditReportType>();
                                    break;
                                case "EXEMPT REASON":
                                    objListDefinition.Exemption = dsResult.Tables[iCount].ToCollection<PTXdoExemption>();
                                    break;
                                case "PROPERTY TYPE BATCHING":
                                    objListDefinition.PropertyTypeBatching = dsResult.Tables[iCount].ToCollection<PTXdoPropertyTypeBatching>();
                                    break;
                                case "SURVEY CAMPAIGN NAME":
                                    objListDefinition.SurveyCampaignName = dsResult.Tables[iCount].ToCollection<PTXdoSurveyCampaignName>();
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
                                case "CONCIERGENOOFPROPERTIESADDED":
                                    objListDefinition.ConciergeNoOfPropertiesAdded = dsResult.Tables[iCount].ToCollection<PTXdoConciergeNoOfPropertiesAdded>();
                                    break;
                                case "COLLECTIONAGENT":
                                    objListDefinition.CollectionAgent = dsResult.Tables[iCount].ToCollection<PTXdoUser>();
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
                return true;
            }
            catch (Exception ex)
            {

                objListDefinition.errorstring = ex.Message;
                return false;
            }
        }

        public List<PTXboClientRemarks>  GetClientRemarks(int clientID)
        {
            Hashtable parameters = new Hashtable();
            List<PTXboClientRemarks> clientRemarks = new List<PTXboClientRemarks>();
            try
            {
                Logger.For(this).Invoice("GetClientRemarks-API  reached " + ((object)clientID).ToJson(false));
                parameters.Add("@ClientID", clientID);
                clientRemarks = _connection.Select<PTXboClientRemarks>(StoredProcedureNames.usp_GetClientRemarks, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("GetClientRemarks-API  ends successfully ");
                return clientRemarks;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("GetClientRemarks-API  error "+ ((object)ex).ToJson(false));
                throw ex ;
            }
           
        }


        /// <summary>
        /// Get service pacakage Id 
        /// </summary>
        public int GetServicePackageIDforInvoice(string servicePackageName)
        {
            Hashtable parameters = new Hashtable();
            int servicePackageID = 0;
            try
            {
                Logger.For(this).Invoice("GetServicePackageIDforInvoice-API  reached " + ((object)servicePackageName).ToJson(false));
                // servicePackageID = Convert.ToInt32(_connection.ExecuteScalar(StoredProcedureNames.usp_getInvoiceServicePackageID, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                servicePackageID = _invoiceCalculation.GetServicePackageIDforInvoice(servicePackageName);
                Logger.For(this).Invoice("GetServicePackageIDforInvoice-API  ends successfully ");
                return servicePackageID;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("GetServicePackageIDforInvoice-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        /// <summary>
        /// Get the latest invoice file location
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public string GetLatestInvoiceFile(int invoiceId)
        {
            Hashtable parameters = new Hashtable();
            string fileLocation = string.Empty;
            try
            {
                Logger.For(this).Invoice("GetLatestInvoiceFile-API  reached " + ((object)invoiceId).ToJson(false));
                parameters.Add("@InvoiceID", invoiceId);
                fileLocation = _connection.ExecuteScalar(StoredProcedureNames.USP_getInvoiceFiles, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("GetLatestInvoiceFile-API  ends successfully ");
                return fileLocation;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("GetLatestInvoiceFile-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboInvoiceAdjustmentReportDetails GetInvoiceAdjustmentReportDetails(int invoiceId)
        {
            Hashtable parameters = new Hashtable();
            
            try
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentReportDetails-API  reached " + ((object)invoiceId).ToJson(false));
                //parameters.Add("@InvoiceID", invoiceId);
                //var result = _connection.SelectMultiple(StoredProcedureNames.usp_getInvoiceGeneratedreportDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure,
                //    Enumerator.Enum_ConnectionString.Spartaxx,
                //    gr => gr.Read<List<PTXboInvoiceReport>>(),
                //    gr => gr.Read<List<PTXboInvoiceAccount>>());

                //PTXboInvoiceAdjustmentReportDetails invoiceAdjustmentReportDetails = new PTXboInvoiceAdjustmentReportDetails
                //{
                //    InvoiceDetails= result.Item1.Count()>0?(List<PTXboInvoiceReport>)result.Item1:new List<PTXboInvoiceReport>(),
                //    InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2:new List<PTXboInvoiceAccount>()
                //};
                Logger.For(this).Invoice("GetInvoiceAdjustmentReportDetails-API  ends successfully ");
                return _invoiceCalculation.GetInvoiceAdjustmentReportDetails(invoiceId);
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentReportDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboAutoAdjustmentInvoiceDetails GetAutoAdjustmentInvoiceDetails(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  reached " + ((object)invoiceId).ToJson(false));
                parameters.Add("@InvoiceID", invoiceId);
                var result = _connection.SelectMultiple(StoredProcedureNames.usp_getAutoAdjustmentInvoiceDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure,
                    Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXboInvoice>(),
                    gr => gr.Read<PTXboInvoiceAdjustment>(),
                    gr=>gr.Read<List<PTXboInvoiceAccount>>(),
                    gr=>gr.Read<List<PTXboInvoiceAdjustmentClarifications>>());

                PTXboAutoAdjustmentInvoiceDetails autoAdjustmentInvoiceDetails = new PTXboAutoAdjustmentInvoiceDetails
                {
                    Invoice = result.Item1.Count() > 0 ? (PTXboInvoice)result.Item1.FirstOrDefault() : new PTXboInvoice(),
                    Adjustment = result.Item2.Count() > 0 ? (PTXboInvoiceAdjustment)result.Item2.FirstOrDefault() : new PTXboInvoiceAdjustment(),
                    InvoiceLienItem = result.Item3.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item3 : new List<PTXboInvoiceAccount>(),
                    InvoiceAdjustmentClarification = result.Item4.Count() > 0 ? (List<PTXboInvoiceAdjustmentClarifications>)result.Item4 : new List<PTXboInvoiceAdjustmentClarifications>()
                };
                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  ends successfully ");
                return autoAdjustmentInvoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public bool SubmitInvoiceFiles(int invoiceID, int corrQID, string fileLocation, int userID)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("SubmitInvoiceFiles-API  reached " + ((object)"invoiceID="+invoiceID.ToString()+"corrQID="+corrQID.ToString()+ "fileLocation="+ fileLocation+ "userID="+ userID.ToString()).ToJson(false));
                parameters.Add("@InvoiceID", invoiceID);
                parameters.Add("@corrQID", corrQID);
                parameters.Add("@FileLocation", fileLocation);
                parameters.Add("@CreatedBy", userID);
                var result = _connection.Execute(StoredProcedureNames.USP_InsertInvoiceFiles,parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SubmitInvoiceFiles-API  ends successfully ");
                return true;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("SubmitInvoiceFiles-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public bool UpdateInvoiceFiles(int invoiceID, int corrQID, string fileLocation, int userID)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("UpdateInvoiceFiles-API  reached " + ((object)"invoiceID=" + invoiceID.ToString() + "corrQID=" + corrQID.ToString() + "fileLocation=" + fileLocation + "userID=" + userID.ToString()).ToJson(false));
                parameters.Add("@InvoiceID", invoiceID);
                parameters.Add("@corrQID", corrQID);
                parameters.Add("@FileLocation", fileLocation);
                parameters.Add("@CreatedBy", userID);
                var result = _connection.Execute(StoredProcedureNames.USP_UpdateInvoiceFiles, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("UpdateInvoiceFiles-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateInvoiceFiles-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }



        public bool CheckInvoicefilepath(string fileLocation)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("CheckInvoicefilepath-API  reached " + ((object) "fileLocation=" + fileLocation).ToJson(false));
                parameters.Add("@FileLocation", fileLocation);
                var result =Convert.ToBoolean( _connection.ExecuteScalar(StoredProcedureNames.USP_CheckInvoicefilepath, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("CheckInvoicefilepath-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("CheckInvoicefilepath-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }


        /// <summary>
        /// Get parameter values.
        /// </summary>
        /// <param name="paramID"></param>
        /// <returns></returns>
        public string GetParamValue(int paramID)
        {
            Hashtable parameters = new Hashtable();
            string paramValue = string.Empty;
            try
            {
                Logger.For(this).Invoice("GetParamValue-API  reached " + ((object)paramID).ToJson(false));
                //parameters.Add("@Param_ID", paramID);
                //paramValue = _connection.ExecuteScalar(StoredProcedureNames.usp_GET_ParamValue, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                paramValue = _invoiceCalculation.GetParamValue(paramID);
                Logger.For(this).Invoice("GetParamValue-API  ends successfully ");
                return paramValue;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetParamValue-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public bool GetInvoiceTermsType(PTXboInvoice objInvoice)
        {
            Hashtable parameters = new Hashtable();
            
            try
            {
                Logger.For(this).Invoice("GetInvoiceTermsType-API  reached " + ((object)objInvoice).ToJson(false));
                parameters.Add("@ClientID", objInvoice.ClientId);
                parameters.Add("@AccountID", objInvoice.AccountId);
                parameters.Add("@GroupID", objInvoice.GroupId);
                parameters.Add("@TermType", objInvoice.InvoiceTypeId);// InvoiceGroupingTypeId
                 var result = _connection.ExecuteScalar(StoredProcedureNames.usp_GetTermtype, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("GetInvoiceTermsType-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceTermsType-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboCADNavigatorDetails GetCADNavigatorDetails(string accountNumber, string countyName)
        {
            Hashtable parameters = new Hashtable();
            PTXboCADNavigatorDetails cADNavigatorDetails = new PTXboCADNavigatorDetails();
            try
            {
                Logger.For(this).Invoice("GetCADNavigatorDetails-API  reached " + ((object)"accountNumber="+ accountNumber+ "countyName=" + countyName).ToJson(false));
                parameters.Add("@accountNumber", accountNumber);
                parameters.Add("@CountyName", countyName);
                cADNavigatorDetails = _connection.Select<PTXboCADNavigatorDetails>(StoredProcedureNames.usp_GetCadNavigatorDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetCADNavigatorDetails-API  ends successfully ");
                return cADNavigatorDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCADNavigatorDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }



        public PTXboClientDeliveryMethodId GetClientDefaultDeliveryMethod(int clientId)
        {
            Hashtable parameters = new Hashtable();
            PTXboClientDeliveryMethodId clientDeliveryMethodId = new PTXboClientDeliveryMethodId();
            try
            {
                Logger.For(this).Invoice("GetClientDefaultDeliveryMethod-API  reached " + ((object)"clientId="+ clientId.ToString()).ToJson(false));
                //parameters.Add("@clientID", clientId);
                //clientDeliveryMethodId = _connection.Select<PTXboClientDeliveryMethodId>(StoredProcedureNames.usp_GetClientDefaultDeliveryMethod, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                clientDeliveryMethodId = _invoiceCalculation.GetClientDefaultDeliveryMethod(clientId);
                Logger.For(this).Invoice("GetClientDefaultDeliveryMethod-API  ends successfully ");
                return clientDeliveryMethodId;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetClientDefaultDeliveryMethod-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }

        public List<PTXboInvoice> GetInvoiceGenerationInputData(int clientID, int taxYear, int invoiceTypeId, bool outOfTexas = false,bool IsDisasterInvoice =false)
        {
            Hashtable parameters = new Hashtable();
            List<PTXboInvoice> invoice = new List<PTXboInvoice>();
            try
            {
                Logger.For(this).Invoice("GetInvoiceGenerationInputData-API  reached " + ((object)"clientID="+ clientID.ToString() + "taxYear=" + taxYear.ToString() + "invoiceTypeId="+ invoiceTypeId.ToString()).ToJson(false));
                //parameters.Add("@ClientID", clientID);
                //parameters.Add("@TaxYear", taxYear);
                //parameters.Add("@InvoiceTypeId", invoiceTypeId);

                //invoice = _connection.Select<PTXboInvoice>(StoredProcedureNames.usp_getInvoiceInputData, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                invoice = _invoiceCalculation.GetInvoiceGenerationInputData(clientID, taxYear, invoiceTypeId, outOfTexas,IsDisasterInvoice);
                Logger.For(this).Invoice("GetInvoiceGenerationInputData-API  ends successfully ");
                return invoice;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceGenerationInputData-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }

        public List<PTXboInvoice> GetCreditInvoiceGenerationInputData(int Clientid)
        {
            Hashtable parameters = new Hashtable();
            List<PTXboInvoice> invoice = new List<PTXboInvoice>();
            try
            {
                 invoice = _invoiceCalculation.GetCreditInvoiceGenerationInputData(Clientid);
                Logger.For(this).Invoice("GetInvoiceGenerationInputData-API  ends successfully ");
                return invoice;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceGenerationInputData-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }

        //public PTXboInvoiceContact GetInvoiceGroupContactDetails(int groupId, int clientId, int corrQId = 0)
        //{
        //    //Hashtable parameters = new Hashtable();
        //    PTXboInvoiceContact invoiceContact = new PTXboInvoiceContact();
        //    try
        //    {
        //        //parameters.Add("@ClientID", clientId);
        //        //parameters.Add("@groupId", groupId);
        //        //parameters.Add("@corrQId", corrQId);

        //        //invoiceContact = _connection.Select<PTXboInvoiceContact>(StoredProcedureNames.usp_getInvoiceGroupContactDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        invoiceContact = _invoiceCalculation.GetInvoiceGroupContactDetails(groupId, clientId, corrQId);
        //        return invoiceContact;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public PTXboInvoiceAddressReference GetInvoiceAddressReference( int clientId, int addressTypeId = 1)
        {
            Hashtable parameters = new Hashtable();
            PTXboInvoiceAddressReference invoiceContact = new PTXboInvoiceAddressReference();
            try
            {
                Logger.For(this).Invoice("GetInvoiceAddressReference-API  reached " + ((object)"clientId="+ clientId.ToString() + "addressTypeId" + addressTypeId.ToString()).ToJson(false));
                parameters.Add("@ClientID", clientId);
                parameters.Add("@AddressTypeId", addressTypeId);
                
                invoiceContact = _connection.Select<PTXboInvoiceAddressReference>(StoredProcedureNames.usp_get_InvoiceAddressReference, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetInvoiceAddressReference-API  ends successfully ");
                return invoiceContact;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceAddressReference-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }

        public PTXboPhone GetContactDetails(int contactID)
        {
            Hashtable parameters = new Hashtable();
            PTXboPhone phone = new PTXboPhone();
            try
            {
                Logger.For(this).Invoice("GetContactDetails-API  reached " + ((object)"contactID="+contactID.ToString()).ToJson(false));
                parameters.Add("@ContactID", contactID);

                // phone = _connection.Select<PTXboPhone>(StoredProcedureNames.usp_get_clientcontact, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                phone = _invoiceCalculation.GetContactDetails(contactID);
                Logger.For(this).Invoice("GetContactDetails-API  ends successfully ");
                return phone;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetContactDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }

        /// <summary>
        /// To validate Email whether Email null or bounced Email
        /// </summary>
        /// <param name="objCorrQueueList"></param>
        /// <param name="lstCorrQueue"></param>
        /// <returns></returns>
        public List<PTXboCorrQueue> ValidateClientEmail(List<PTXboCorrQueue> objCorrQueueList)
        {
            //Hashtable parameters = new Hashtable();
            //int BouncedEmailCount = 0;
            //bool IsBadAddress = false;
            string ClientEmail = string.Empty;
            List<PTXboCorrQueue> lstCorrQueue = new List<PTXboCorrQueue>();
            try
            {
                Logger.For(this).Invoice("ValidateClientEmail-API  reached " + ((object)objCorrQueueList).ToJson(false));
                //foreach (var item in objCorrQueueList)
                //{
                //    parameters.Add("@ClientID", item.ClientID);
                //    var result =_connection.Select<PTXboClientEmailValidation>(StoredProcedureNames.Usp_ValidateClientEmail, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                //    if(result !=null)
                //    {

                //        BouncedEmailCount = result.BouncedEmails;
                //        ClientEmail = result.ClientEmail;
                //        IsBadAddress = result.IsBadAddress;
                //        if (item.DeliveryMethodId == Enumerators.PTXenumDefaultDeliveryMethod.Email.GetId())
                //        {
                //            if (BouncedEmailCount >= 1 || string.IsNullOrEmpty(ClientEmail))
                //            {
                //                item.DeliveryMethodId = Enumerators.PTXenumDefaultDeliveryMethod.USMail.GetId();
                //                if (IsBadAddress)
                //                {
                //                    item.CorrProcessingStatusID = Enumerators.PTXenumCorrProcessingStatus.BadAddress.GetId();
                //                }
                //            }
                //        }
                //        else if (item.DeliveryMethodId == Enumerators.PTXenumDefaultDeliveryMethod.USMail.GetId())
                //        {
                //            if (IsBadAddress)
                //            {
                //                item.CorrProcessingStatusID = Enumerators.PTXenumCorrProcessingStatus.BadAddress.GetId();
                //            }
                //        }
                //        lstCorrQueue.Add(item);

                //    }

                //}
                lstCorrQueue = _invoiceCalculation.ValidateClientEmail(objCorrQueueList);

                Logger.For(this).Invoice("ValidateClientEmail-API  ends successfully ");
                return lstCorrQueue;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("ValidateClientEmail-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public bool UpdateCorrAccounts(int accountId,int corrQId)
        {
            //Hashtable parameters = new Hashtable();
            
            try
            {
                Logger.For(this).Invoice("UpdateCorrAccounts-API  reached " + ((object)"accountId="+ accountId.ToString() + "corrQId="+ corrQId.ToString()).ToJson(false));
                // parameters.Add("@AccountId", accountId);
                // parameters.Add("@CorrQId", corrQId);
                //var  result = _connection.ExecuteScalar(StoredProcedureNames.usp_insertcorrqaccounts, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                var result = _invoiceCalculation.UpdateCorrAccounts(accountId, corrQId);
                Logger.For(this).Invoice("UpdateCorrAccounts-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateCorrAccounts-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }

        public int SaveOrUpdateCorrQ(PTXboCorrQueue corrQueue)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateCorrQ-API  reached " + ((object)corrQueue).ToJson(false));
                //parameters.Add("@CorrQID", corrQueue.CorrQID);
                //parameters.Add("@ServicePackageID ", corrQueue.ServicePackageID);
                //parameters.Add("@ClientID", corrQueue.ClientID);
                //parameters.Add("@TaxYear", corrQueue.TaxYear);
                //parameters.Add("@CorrProcessingStatusID", corrQueue.CorrProcessingStatusID);
                //parameters.Add("@DeliveryMethodID", corrQueue.DeliveryMethodId);
                //parameters.Add("@IsSpecificAccount", corrQueue.IsSpecificAccount);
                //parameters.Add("@SentToContactAgent", corrQueue.SentToContactAgent);
                //parameters.Add("@SentToPrimaryAgent", corrQueue.SentToPrimaryAgent);
                //parameters.Add("@SentToSalesAgent", corrQueue.SentToSalesAgent);
                //parameters.Add("@Createdby", corrQueue.CreatedBy);
                //parameters.Add("@SentToClient", corrQueue.SentToClient);
                //parameters.Add("@InvoiceID", corrQueue.LinkFieldValue);
                //parameters.Add("@IsCustomDelivery", corrQueue.IsCustomDelivery);
                //parameters.Add("@DeliveryAddress", corrQueue.DeliveryAddress);
                //parameters.Add("@StartDateTime", DateTime.Now);
                //var result = _connection.ExecuteScalar(StoredProcedureNames.usp_insertcorrq, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                var result = _invoiceCalculation.SaveOrUpdateCorrQ(corrQueue);
                Logger.For(this).Invoice("SaveOrUpdateCorrQ-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateCorrQ-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }




        public void UpdateInvoiceStatus(int invoiceID,int corrProcessingStatusID)
        {
           // Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("UpdateInvoiceStatus-API  reached " + ((object)"invoiceID="+ invoiceID.ToString() + "corrProcessingStatusID="+ corrProcessingStatusID.ToString()).ToJson(false));
                //parameters.Add("@InvoiceID", invoiceID);
                //parameters.Add("@CorrProcessingStatusId", corrProcessingStatusID);

                //var result = _connection.Execute(StoredProcedureNames.usp_updateinvoicestatus, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                 _invoiceCalculation.UpdateInvoiceStatus(invoiceID, corrProcessingStatusID);
                Logger.For(this).Invoice("UpdateInvoiceStatus-API  ends successfully ");
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateInvoiceStatus-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        //public List<PTXboInvoicePaymentMap> GetInvoicePaymentMap(int invoiceID)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("GetInvoicePaymentMap-API  reached " + ((object)"invoiceID="+ invoiceID.ToString()).ToJson(false));
        //        //parameters.Add("@InvoiceID", invoiceID);
        //        //var result = _connection.Select<PTXboInvoicePaymentMap>(StoredProcedureNames.usp_get_InvoicePaymentMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
        //        var result = _invoiceCalculation.GetInvoicePaymentMap(invoiceID);
        //        Logger.For(this).Invoice("GetInvoicePaymentMap-API  ends successfully ");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetInvoicePaymentMap-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        public bool UpdateClientCorrQueueRecordInvoice(List<PTXboCorrQueue> objCorrQueueList, int groupId)
        {
            
            Hashtable parameters = new Hashtable();
            PTXboInvoiceContact invoiceContact = new PTXboInvoiceContact();
            try
            {
                Logger.For(this).Invoice("UpdateClientCorrQueueRecordInvoice-API  reached " + ((object)objCorrQueueList).ToJson(false));
                // if (objCorrQueueList !=null)
                // {


                // List<PTXboCorrQueue> lstCorrQueue = new List<PTXboCorrQueue>();
                // lstCorrQueue=ValidateClientEmail(objCorrQueueList);

                // foreach (PTXboCorrQueue objCorrQueue in lstCorrQueue)
                // {
                //     if (!objCorrQueue.IsCustomDelivery)
                //     {
                //         var clientGroupContact = GetInvoiceGroupContactDetails(groupId, objCorrQueue.ClientID);

                //         if (clientGroupContact != null)
                //         {
                //             if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.Email.GetId())
                //             {
                //                 objCorrQueue.DeliveryAddress = clientGroupContact.ClientEmail;
                //             }
                //             else if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.USMail.GetId())
                //             {
                //                 objCorrQueue.DeliveryAddress = clientGroupContact.address;
                //             }
                //             else if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.Fax.GetId())
                //             {
                //                 if (clientGroupContact.ContactId != null)
                //                 {
                //                     var Contact = GetContactDetails(Convert.ToInt32(clientGroupContact.ContactId));

                //                     if (Contact != null && Contact.PhoneNumber != null)
                //                     {
                //                         if (Contact.IsItFax != true)
                //                         {
                //                             objCorrQueue.DeliveryAddress = Contact.PhoneNumber;
                //                         }
                //                     }
                //                 }
                //             }
                //         }

                //     }

                //     //Inserting invoice details in ptaxcorrqueue table
                //     var corrQID=  SaveOrUpdateCorrQ(objCorrQueue);
                //     //update corrid in corracc
                //     if (objCorrQueue.AccountList.Any())
                //     {
                //         foreach (var CorrAccount in objCorrQueue.AccountList.Split(','))
                //         {
                //             //Inserting account details in ptaxcorraccounts table
                //             UpdateCorrAccounts(Convert.ToInt32(CorrAccount), corrQID);
                //         }
                //     }



                //     //updating invoice status in 1-ptaxhearingresult,2-ptaxlitigation,3-ptaxarbitrationdetails
                //     UpdateInvoiceStatus(objCorrQueue.LinkFieldValue, objCorrQueue.CorrProcessingStatusID);
                //     Logger.For(this).Invoice("UpdateClientCorrQueueRecordInvoice-API  ends successfully ");
                //}

                // return true;
                // }
                // else
                // {

                //     return false;
                // }
                return _invoiceCalculation.UpdateClientCorrQueueRecordInvoice(objCorrQueueList, groupId);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateClientCorrQueueRecordInvoice-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboCorrQueue InsertUpdateCorrQCreditInvoice(List<PTXboCorrQueue> objCorrQueueList)
        {

            Hashtable parameters = new Hashtable();
            PTXboInvoiceContact invoiceContact = new PTXboInvoiceContact();
            try
            {
                Logger.For(this).Invoice("UpdateClientCorrQueueRecordInvoice-API  reached " + ((object)objCorrQueueList).ToJson(false));
                
                return _invoiceCalculation.InsertUpdateCorrQCreditInvoice(objCorrQueueList);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateClientCorrQueueRecordInvoice-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        //public PTXboInvoiceReportDetails GetInvoiceReportDetails(int invoiceID, int invoiceTypeId = (int)Enumerators.PTXenumInvoiceType.Standard, bool? isInvoiceDefect = null, bool isOutOfTexas = false, bool IsOTEntryscreen = false)
        public PTXboInvoiceReportDetails GetInvoiceReportDetails(PTXboInvoiceReportDetailsInput invoiceReportDetailsInput)
        {
            Hashtable parameters = new Hashtable();
            PTXboPhone phone = new PTXboPhone();
            PTXboInvoiceReportDetails invoiceReportDetails = new PTXboInvoiceReportDetails();
            try
            {
                Logger.For(this).Invoice("GetInvoiceReportDetails-API  reached " + ((object)invoiceReportDetailsInput).ToJson(false));
                //parameters.Add("@InvoiceID", invoiceID);
                //parameters.Add("@isInvoiceDefect", isInvoiceDefect);
                //if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                //{
                //  var result=  _connection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReport_Arbitration, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                //              gr => gr.Read<PTXboInvoiceReport>(),
                //              gr => gr.Read<PTXboInvoiceAccount>());
                //    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                //    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();

                //}
                //else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
                //{
                //    var result = _connection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReportLitigation, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                //              gr => gr.Read<PTXboInvoiceAccount>(),
                //              gr => gr.Read<PTXboInvoiceReport>());
                //    invoiceReportDetails.InvoiceAccount = result.Item1.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item1 : new List<PTXboInvoiceAccount>();
                //    invoiceReportDetails.InvoiceDetails = result.Item2.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item2 : new List<PTXboInvoiceReport>();
                //}
                //else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
                //{
                //    var result = _connection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReportBppRendition, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                //              gr => gr.Read<PTXboInvoiceReport>(),
                //              gr => gr.Read<PTXboInvoiceAccount>());
                //    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                //    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                //}
                //else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                //{

                //    var result = _connection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReportTaxBill, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                //              gr => gr.Read<PTXboInvoiceReport>(),
                //              gr => gr.Read<PTXboInvoiceAccount>());
                //    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                //    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                //}
                //else
                //{
                //    var result = _connection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReport, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                //              gr => gr.Read<PTXboInvoiceReport>(),
                //              gr => gr.Read<PTXboInvoiceAccount>());
                //    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                //    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                //}

                //invoiceReportDetails = _invoiceCalculation.GetInvoiceReportDetails(invoiceID, invoiceTypeId, isInvoiceDefect,isOutOfTexas, IsOTEntryscreen);
                invoiceReportDetails = _invoiceCalculation.GetInvoiceReportDetails(invoiceReportDetailsInput);
               
                Logger.For(this).Invoice("GetInvoiceReportDetails-API  ends successfully ");

                return invoiceReportDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceReportDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }

        public bool IsOutOfTexas(int invoiceId)
        {
            try
            {
                Logger.For(this).Invoice("IsOutOfTexas-API  reached " + ((object)invoiceId).ToJson(false));
                return _invoiceCalculation.IsOutOfTexas(invoiceId);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("IsOutOfTexas-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public bool IsMultiyear(int invoiceId)
        {
            try
            {
                Logger.For(this).Invoice("IsMultiyear-API  reached " + ((object)invoiceId).ToJson(false));
                return _invoiceCalculation.IsMultiyear(invoiceId);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("IsMultiyear-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }
        public bool IsILAccount(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("IsOOT-API  reached " + ((object)invoiceId).ToJson(false));
                return _invoiceCalculation.IsILAccount(invoiceId);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("IsOOT-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }

        }

        public bool IsHotelInvoice(int invoiceId)
        {
            try
            {
                Logger.For(this).Invoice("IsMultiyear-API  reached " + ((object)invoiceId).ToJson(false));
                return _invoiceCalculation.IsHotelInvoice(invoiceId);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("IsMultiyear-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public bool SaveOrUpdateInvoice(PTXboInvoice invoice,out int invoiceID)
        {
            Hashtable parameters = new Hashtable();
            invoiceID = 0;
            try
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoice-API  reached " + ((object)invoice).ToJson(false));
                //parameters.Add("@InvoiceId",invoice.InvoiceID);
                //parameters.Add("@GroupId",invoice.GroupId);
                //parameters.Add("@ProjectId",invoice.ProjectId);
                //parameters.Add("@YealyHearingDetailsId",(invoice.YealyHearingDetailsId==0)?invoice.YearlyHearingDetailsId:invoice.YealyHearingDetailsId);
                //parameters.Add("@ClientId",invoice.ClientId);
                //parameters.Add("@InvoiceTypeId", invoice.InvoiceTypeId);
                //parameters.Add("@InvoiceDate",   invoice.InvoiceDate);
                //parameters.Add("@PaymentDueDate",invoice.PaymentDueDate);
                //parameters.Add("@InvoiceGenerationConfigIDUsedForCalculation",invoice.InvoiceGenerationConfigIDUsedForCalculation);
                //parameters.Add("@InitialAssessedValue ",invoice.InitialAssessedValue);
                //parameters.Add("@FinalAssessedValue", invoice.FinalAssessedValue);
                //parameters.Add("@PriorYearTaxRate",invoice.PriorYearTaxRate);
                //parameters.Add("@ContingencyPercentage",invoice.ContingencyPercentage);
                //parameters.Add("@ContingencyFee",invoice.ContingencyFee);
                //parameters.Add("@FlatFee",invoice.FlatFee);
                //parameters.Add("@InvoiceAmount",invoice.InvoiceAmount);
                //parameters.Add("@CreatedDateAndTime",invoice.CreatedDateAndTime);
                //parameters.Add("@SentDateAndTime", invoice.SentDateAndTime);
                //parameters.Add("@DeliveryMethodId",invoice.DeliveryMethodId);
                //parameters.Add("@DeliveryStatusId",(invoice.DeliveryStatusId==0)?GetClientDefaultDeliveryMethod(invoice.ClientId).DefaultDeliveryTypeId:invoice.DeliveryStatusId);
                //parameters.Add("@AutoGenerated", invoice.AutoGenerated);
                //parameters.Add("@ManuallyGeneratedUserId",invoice.ManuallyGeneratedUserId);
                //parameters.Add("@ManuallyGeneratedUserRoleId",invoice.ManuallyGeneratedUserRoleId);
                //parameters.Add("@OnHold",invoice.OnHold);
                //parameters.Add("@OnHoldDate",invoice.OnHoldDate);
                //parameters.Add("@TotalAmountPaid", invoice.TotalAmountPaid);
                //parameters.Add("@InvoicingStatusId",invoice.InvoicingStatusId);
                //parameters.Add("@InvoicingProcessingStatusId",invoice.InvoicingProcessingStatusId);
                //parameters.Add("@InvoiceDescription ",invoice.InvoiceDescription);
                //parameters.Add("@CompoundInterest",invoice.CompoundInterest);
                //parameters.Add("@InvoiceCreditAmount",invoice.InvoiceCreditAmount);
                //parameters.Add("@PaymentStatusId",invoice.PaymentStatusId);
                //parameters.Add("@InvoiceGroupingTypeId ",invoice.InvoiceGroupingTypeId);
                //parameters.Add("@TaxYear", invoice.TaxYear);
                //parameters.Add("@Reduction",invoice.Reduction);
                //parameters.Add("@TotalEstimatedTaxSavings",invoice.TotalEstimatedTaxSavings);
                //parameters.Add("@AmountPaid",invoice.TotalAmountPaid);
                //parameters.Add("@AmountAdjusted",invoice.AmountAdjusted);
                //parameters.Add("@ApplicableInterest",invoice.InterestAmount);
                //parameters.Add("@InterestPaid",invoice.InterestPaid);
                //parameters.Add("@InterestAdjusted",invoice.InterestAdjustment);
                //parameters.Add("@AmountDue",invoice.AmountDue);
                //parameters.Add("@UpdatedBy",invoice.CreatedBy);
                //parameters.Add("@UpdatedDateTime",DateTime.Now);
                //parameters.Add("@InterestRateID",invoice.InterestRateID);
                //parameters.Add("@IntitalLand",invoice.IntitalLand);
                //parameters.Add("@IntialImproved",invoice.IntialImproved);
                //parameters.Add("@InitialMarket",invoice.InitialMarket);
                //parameters.Add("@FinalLand",invoice.FinalLand);
                //parameters.Add("@FinalImproved",invoice.FinalImproved);
                //parameters.Add("@FinalMarket",invoice.FinalMarket);
                //parameters.Add("@CreditAmountApplied",invoice.InvoiceCreditAmount);
                //parameters.Add("@isInvoiceDefect",invoice.isInvoiceDefect);

                //var result = _connection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                var result = _invoiceCalculation.SaveOrUpdateInvoice(invoice, out invoiceID);
                Logger.For(this).Invoice("SaveOrUpdateInvoice-API  ends successfully ");
                //invoiceID = Convert.ToInt32(result);
                if (invoiceID != 0)
                    return true;
                else
                    return false;
               // return Convert.ToBoolean(result);
                
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoice-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        //public PTXboPayment GetInvoicePayment(int paymentId,string paymentDescription)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("GetInvoicePayment-API  reached " + ((object)"paymentId="+ paymentId.ToString()+ "paymentDescription="+ paymentDescription).ToJson(false));
        //        //parameters.Add("@PaymentId", paymentId);
        //        //parameters.Add("@PaymentDescription", paymentDescription);
        //        //var result = _connection.Select<PTXboPayment>(StoredProcedureNames.usp_get_InvoicePayment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        var result = _invoiceCalculation.GetInvoicePayment(paymentId, paymentDescription);
        //        Logger.For(this).Invoice("GetInvoicePayment-API  ends successfully ");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetInvoicePayment-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public int SaveOrUpdatePayment(PTXboPayment payment)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdatePayment-API  reached " + ((object)payment).ToJson(false));
        //        //parameters.Add("@PaymentId", payment.PaymentId);
        //        //parameters.Add("@ClientId", payment.ClientId);
        //        //parameters.Add("@PaymentDescription", payment.PaymentDescription);
        //        //parameters.Add("@PaymentAmount", payment.PaymentAmount);
        //        //parameters.Add("@InterestAmount", payment.InterestAmount);
        //        //parameters.Add("@PaymentReceivedDate", payment.PaymentReceivedDate);
        //        //parameters.Add("@PostedDate", payment.PostedDate);
        //        //parameters.Add("@InvoicePaymentMethodId", payment.InvoicePaymentMethodId);
        //        //parameters.Add("@PaymentTypeID", payment.PaymentTypeID);
        //        //parameters.Add("@PropertyTaxInvoiceID", payment.PropertyTaxInvoiceID);
        //        //parameters.Add("@PropertyTaxPaymentTypeID", payment.PropertyTaxPaymentTypeID);
        //        //parameters.Add("@CreatedBy", payment.CreatedBy);
        //        //parameters.Add("@CreatedDateTime", payment.CreatedDateTime);
        //        //parameters.Add("@CheckNumber", payment.CheckNumber);
        //        //parameters.Add("@BatchNumber", payment.BatchNumber);
        //        //var result = _connection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdatePayment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        var result = _invoiceCalculation.SaveOrUpdatePayment(payment);
        //        Logger.For(this).Invoice("SaveOrUpdatePayment-API  ends successfully ");
        //        return Convert.ToInt32(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdatePayment-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public int SaveOrUpdateInvoicePaymentMap(PTXboInvoicePaymentMap paymentInvoiceMap)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateInvoicePaymentMap-API  reached " + ((object)paymentInvoiceMap).ToJson(false));
        //        //parameters.Add("@InvoicePaymentMapId", paymentInvoiceMap.InvoicePaymentMapId);
        //        //parameters.Add("@InvoiceId", paymentInvoiceMap.InvoiceId);
        //        //parameters.Add("@PaymentId", paymentInvoiceMap.PaymentId);

        //        //var result = _connection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateInvoicePaymentMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        var result = _invoiceCalculation.SaveOrUpdateInvoicePaymentMap(paymentInvoiceMap);
        //        Logger.For(this).Invoice("SaveOrUpdateInvoicePaymentMap-API  ends successfully ");
        //        return Convert.ToInt32(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateInvoicePaymentMap-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public bool UpdateAmountDueCapValueChange(int invoiceID, out string errorString)
        //{
        //    Hashtable parameters = new Hashtable();
        //    errorString = string.Empty;
        //    try
        //    {
        //        Logger.For(this).Invoice("UpdateAmountDueCapValueChange-API  reached " + ((object)invoiceID).ToJson(false));
        //        //parameters.Add("@invoiceID", invoiceID);
        //        //parameters.Add("@IsMainScreen", false);

        //        //var result = _connection.ExecuteScalar(StoredProcedureNames.usp_UpdateInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        _invoiceCalculation.UpdateAmountDueCapValueChange(invoiceID,out errorString);
        //        Logger.For(this).Invoice("UpdateAmountDueCapValueChange-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("UpdateAmountDueCapValueChange-API  error "+ ((object)ex).ToJson(false));
        //        errorString = ex.Message;
        //        throw ex;
        //    }
        //}

        //public bool SubmitCapValueAdjustment(PTXboPayment ObjPayment, int invoiceID, out string errorString)
        //{
        //    errorString = string.Empty;
        //    try
        //    {
        //        Logger.For(this).Invoice("SubmitCapValueAdjustment-API  reached " + ((object)ObjPayment+ "invoiceID=" + invoiceID.ToString()).ToJson(false));
        //        //if (ObjPayment != null && ObjPayment.PaymentId == 0)
        //        //{
        //        //    PTXboPayment PrincipalAdjPayment = new PTXboPayment();
        //        //    PrincipalAdjPayment.PostedDate = DateTime.Now;
        //        //    PrincipalAdjPayment.PaymentDescription = ObjPayment.PaymentDescription;
        //        //    PrincipalAdjPayment.PaymentAmount = ObjPayment.PaymentAmount;
        //        //    PrincipalAdjPayment.InvoicePaymentMethodId = (ObjPayment.InvoicePaymentMethodId == 0) ? 0 :  ObjPayment.InvoicePaymentMethodId ;
        //        //    PrincipalAdjPayment.InterestAmount = ObjPayment.InterestAmount;
        //        //    PrincipalAdjPayment.ClientId = (ObjPayment.ClientId== 0) ? 0 : ObjPayment.ClientId;
        //        //    PrincipalAdjPayment.CreatedBy = ObjPayment.CreatedBy;
        //        //    PrincipalAdjPayment.CreatedDateTime = ObjPayment.CreatedDateTime;
        //        //    PrincipalAdjPayment.UpdatedBy = ObjPayment.UpdatedBy;
        //        //    PrincipalAdjPayment.UpdatedDateTime = ObjPayment.UpdatedDateTime;
        //        //    SaveOrUpdatePayment(PrincipalAdjPayment);
        //        //    PTXboInvoicePaymentMap PaymentInvoiceMap = new PTXboInvoicePaymentMap();
        //        //    PaymentInvoiceMap.InvoiceId = invoiceID;
        //        //    PaymentInvoiceMap.PaymentId = (PrincipalAdjPayment.PaymentId == 0) ? 0 :PrincipalAdjPayment.PaymentId ;
        //        //    SaveOrUpdateInvoicePaymentMap(PaymentInvoiceMap);

        //        //}
        //        //else
        //        //{
        //        //    PTXboPayment PrincipalAdjPayment = ObjPayment;
        //        //    PrincipalAdjPayment.PostedDate = DateTime.Now;
        //        //    PrincipalAdjPayment.PaymentDescription = ObjPayment.PaymentDescription;
        //        //    PrincipalAdjPayment.PaymentAmount = ObjPayment.PaymentAmount;
        //        //    PrincipalAdjPayment.InvoicePaymentMethodId = (ObjPayment.InvoicePaymentMethodId == 0) ? 0 :  ObjPayment.InvoicePaymentMethodId ;
        //        //    PrincipalAdjPayment.InterestAmount = ObjPayment.InterestAmount;
        //        //    PrincipalAdjPayment.ClientId = (ObjPayment.ClientId == 0) ? 0: ObjPayment.ClientId;
        //        //    PrincipalAdjPayment.CreatedBy = ObjPayment.CreatedBy;
        //        //    PrincipalAdjPayment.CreatedDateTime = ObjPayment.CreatedDateTime;
        //        //    PrincipalAdjPayment.UpdatedBy = ObjPayment.UpdatedBy;
        //        //    PrincipalAdjPayment.UpdatedDateTime = ObjPayment.UpdatedDateTime;
        //        //    SaveOrUpdatePayment(PrincipalAdjPayment);

        //        //}
        //        _invoiceCalculation.SubmitCapValueAdjustment(ObjPayment, invoiceID,out errorString);
        //        Logger.For(this).Invoice("SubmitCapValueAdjustment-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SubmitCapValueAdjustment-API  error "+ ((object)ex).ToJson(false));
        //        errorString = ex.Message;
        //        return false;
        //    }
        //}

        //public bool RemoveCapvalueAdjustment(int invoiceID)
        //{
        //    Hashtable parameters = new Hashtable();
       
        //    try
        //    {
        //        Logger.For(this).Invoice("RemoveCapvalueAdjustment-API  reached " + ((object)invoiceID).ToJson(false));
        //        //parameters.Add("@InvoiceId", invoiceID);
        //        //var result = _connection.ExecuteScalar(StoredProcedureNames.usp_RemoveCapvalueAdjustment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        var result = _invoiceCalculation.RemoveCapvalueAdjustment(invoiceID);
        //        Logger.For(this).Invoice("RemoveCapvalueAdjustment-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("RemoveCapvalueAdjustment-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        public bool AccountLevelInvoiceGeneration(List<PTXboInvoiceReport> getlstInvoiceDetails, List<PTXboInvoiceAccount> lstAccountDetails, int updatedBy)
        {
            try
            {
                Logger.For(this).Invoice("AccountLevelInvoiceGeneration-API  reached " + ((object)getlstInvoiceDetails).ToJson(false));
                bool isReturn = false;
                string errorMessage = string.Empty;
                _invoiceCalculation.AccountLevelInvoiceGeneration(getlstInvoiceDetails, lstAccountDetails, updatedBy);
                //foreach (PTXboInvoiceReport objInv in getlstInvoiceDetails)
                //{
                //    bool isSuccessCapValue = false;
                //    List<PTXboInvoicePaymentMap> lstInvoicePaymentMap;
                //    var objInvoiceid = GetInvoiceGenerationDetails(objInv.InvoiceId);
                //    if (objInvoiceid != null)
                //    {
                //        int invoiceID = 0;
                //        objInvoiceid.InitialAssessedValue = Convert.ToDecimal(objInv.Total_Initial_Assessed_Value);
                //        objInvoiceid.FinalAssessedValue = Convert.ToDecimal(objInv.Total_Final_Assessed_Value);
                //        objInvoiceid.Reduction = Convert.ToDecimal((objInv.Total_Initial_Assessed_Value) - (objInv.Total_Final_Assessed_Value));
                //        objInvoiceid.PriorYearTaxRate = Math.Round(Convert.ToDouble(objInv.Prior_Year_TaxRate * 100), 8);
                //        if (objInvoiceid.Reduction > 0)
                //        {
                //            objInvoiceid.TotalEstimatedTaxSavings = Math.Round(Convert.ToDecimal((objInvoiceid.PriorYearTaxRate / 100)) * Convert.ToDecimal((objInvoiceid.Reduction)), 2);
                //            objInvoiceid.ContingencyPercentage = objInv.Contingency / 100;
                //            objInvoiceid.ContingencyFee = Math.Round(Convert.ToDecimal((objInvoiceid.TotalEstimatedTaxSavings) * Convert.ToDecimal((objInv.Contingency / 100))), 2);
                //            objInvoiceid.InvoiceAmount = Math.Round(objInvoiceid.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee, 2);
                //            objInvoiceid.FlatFee = objInv.Flat_fee;
                //            isReturn= SaveOrUpdateInvoice(objInvoiceid,out invoiceID);
                //            objInvoiceid.InvoiceID = invoiceID;
                //            if (objInv.CapValue > 0 && Convert.ToDecimal((objInv.Contingency / 100)) > 0 && Math.Round(objInvoiceid.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee, 2) > Convert.ToDecimal(objInv.CapValue))
                //            {
                //                lstInvoicePaymentMap = GetInvoicePaymentMap(objInv.InvoiceId);
                //                if (lstInvoicePaymentMap != null && lstInvoicePaymentMap.Count > 0)
                //                {
                //                    PTXboPayment Objpayment;
                //                    foreach (PTXboInvoicePaymentMap obiInvPay in lstInvoicePaymentMap)
                //                    {
                //                        var lstPayment = GetInvoicePayment(obiInvPay.PaymentId, "Cap Value Adjustment");
                //                        if (lstPayment != null)
                //                        {
                //                            Objpayment = lstPayment;
                //                        }
                //                        else
                //                        {
                //                            Objpayment = new PTXboPayment();
                //                        }
                //                        decimal PaymentAmount = 0;
                //                        PaymentAmount = Math.Round((objInvoiceid.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee) - Convert.ToDecimal(objInv.CapValue), 2);
                //                        Objpayment.ClientId = objInv.ClientId;
                //                        Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                //                        Objpayment.PaymentAmount = PaymentAmount;
                //                        Objpayment.PaymentDescription = "Cap Value Adjustment";
                //                        Objpayment.CreatedBy = updatedBy;
                //                        Objpayment.CreatedDateTime = DateTime.Now;
                //                        Objpayment.UpdatedBy = updatedBy;
                //                        Objpayment.UpdatedDateTime = DateTime.Now;
                //                        isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, objInv.InvoiceId, out errorMessage);
                //                        if (isSuccessCapValue)
                //                        {
                //                            bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errorMessage);
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    PTXboPayment Objpayment = new PTXboPayment();
                //                    decimal PaymentAmount = 0;
                //                    PaymentAmount = Math.Round((objInvoiceid.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee) - Convert.ToDecimal(objInv.CapValue), 2);
                //                    Objpayment.ClientId = objInv.ClientId;
                //                    Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                //                    Objpayment.PaymentAmount = PaymentAmount;
                //                    Objpayment.PaymentDescription = "Cap Value Adjustment";
                //                    Objpayment.CreatedBy = updatedBy;
                //                    Objpayment.CreatedDateTime = DateTime.Now;
                //                    Objpayment.UpdatedBy = updatedBy;
                //                    Objpayment.UpdatedDateTime = DateTime.Now;
                //                    isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, objInv.InvoiceId, out errorMessage);
                //                    if (isSuccessCapValue)
                //                    {
                //                        bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errorMessage);
                //                    }
                //                }
                //            }
                //            else if (Convert.ToDecimal(objInvoiceid.AmountAdjusted) != 0)
                //            {
                //                RemoveCapvalueAdjustment(objInvoiceid.InvoiceID);
                //                bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errorMessage);
                //            }
                //        }
                //    }
                //}
                Logger.For(this).Invoice("AccountLevelInvoiceGeneration-API  ends successfully ");
                return isReturn;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("AccountLevelInvoiceGeneration-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public bool ProjectorTermInvoiceGeneration(List<PTXboInvoiceReport> getlstInvoiceDetails, List<PTXboInvoiceAccount> lstAccountDetails, int updatedBy)
        {

            try
            {
              Logger.For(this).Invoice("ProjectorTermInvoiceGeneration-API  reached " + ((object)getlstInvoiceDetails).ToJson(false));
              bool isReturn = false;
                isReturn = _invoiceCalculation.ProjectorTermInvoiceGeneration(getlstInvoiceDetails, lstAccountDetails, updatedBy);
            //  string errorMessage = string.Empty;
            //  PTXboInvoice objInsertInvoiceData = new PTXboInvoice();
            //  foreach (PTXboInvoiceReport objInv in getlstInvoiceDetails)
            //  {
            //    bool isSuccessCapValue = false;
            //    List<PTXboInvoicePaymentMap> lstInvoicePaymentMap;
            //    var objInvoiceid = GetInvoiceGenerationDetails(objInv.InvoiceId);
            //    if (objInvoiceid != null)
            //    {
            //        decimal TotalNoticedValue = 0;
            //        decimal TotalPostHearingValue = 0;
            //        decimal TotalReduction = 0;
            //        double TotalPriorYearTaxRate = 0;
            //        decimal TotalEstimatedTaxSavings = 0;
            //        decimal? TotalInvoiceAmount = 0;
            //        decimal Reduction = 0;
            //        decimal? ContingencyFee = 0;
            //        decimal? InvoiceAmount = 0;
            //        int invoiceID = 0;
            //        foreach (PTXboInvoiceAccount lstAccount in lstAccountDetails)
            //        {
            //            TotalNoticedValue = TotalNoticedValue + Convert.ToDecimal(lstAccount.NoticedValue);
            //            TotalPostHearingValue = TotalPostHearingValue + Convert.ToDecimal(lstAccount.FinalValue);
            //            TotalPriorYearTaxRate = TotalPriorYearTaxRate + Convert.ToDouble(lstAccount.PrevYearTaxRate);
            //            TotalReduction = (TotalNoticedValue - TotalPostHearingValue); //Modified By Pavithra.B on 31Aug2016 - calculating Reduction value from TotalNoticedValue and TotalPostHearingValue                                
            //            Reduction = Convert.ToDecimal(lstAccount.NoticedValue) - Convert.ToDecimal(lstAccount.FinalValue);
            //            if (TotalReduction > 0)
            //            {
            //                lstAccount.Estimated_Tax_Savings = (Convert.ToDouble(Convert.ToDouble(lstAccount.PrevYearTaxRate))) * Convert.ToDouble((Reduction));
            //                TotalEstimatedTaxSavings = TotalEstimatedTaxSavings + (Convert.ToDecimal(Convert.ToDouble(lstAccount.PrevYearTaxRate))) * Convert.ToDecimal((Reduction));
            //                ContingencyFee = (TotalEstimatedTaxSavings) * Convert.ToDecimal((objInv.Contingency / 100));

            //                    if ((objInvoiceid.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() ||
            //                                objInvoiceid.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoiceid.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
            //                                && objInvoiceid.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
            //                //if (objInvoiceid.InvoiceType.Termstypeid == Enumerators.PTXenumInvoiceType.Standard.GetId() && objInvoiceid.InvoiceGroupType.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
            //                {
            //                    //Modified by Pavithra.B on 3Nov2016 - TFS Id-26636
            //                    InvoiceAmount = (Convert.ToDecimal(lstAccount.Estimated_Tax_Savings) * Convert.ToDecimal((objInv.Contingency / 100)));
            //                    TotalInvoiceAmount = TotalInvoiceAmount + InvoiceAmount;
            //                }
            //                else if ((objInvoiceid.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() ||
            //                                objInvoiceid.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoiceid.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
            //                                && objInvoiceid.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
            //                {
            //                    //Modified by Pavithra.B on 3Nov2016 - TFS Id-26636
            //                    InvoiceAmount = (Convert.ToDecimal(lstAccount.Estimated_Tax_Savings) * Convert.ToDecimal((objInv.Contingency / 100)));
            //                    TotalInvoiceAmount = TotalInvoiceAmount + InvoiceAmount;
            //                }
            //                //else if(objInvoiceid.InvoiceType.Termstypeid == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId() ||
            //                //   objInvoiceid.InvoiceType.Termstypeid == Enumerators.PTXenumInvoiceType.BPP.GetId())
            //                //{
            //                //    InvoiceAmount= Convert.ToDecimal(objInv.Flat_fee);
            //                //    TotalInvoiceAmount = InvoiceAmount;
            //                //}
            //                objInsertInvoiceData.InvoiceID = objInv.InvoiceId;
            //                objInsertInvoiceData.TotalEstimatedTaxSaving = Convert.ToDouble(TotalEstimatedTaxSavings);
            //                objInsertInvoiceData.ContingencyFee = ContingencyFee;
            //                objInsertInvoiceData.InvoiceAmount = TotalInvoiceAmount;
            //                objInsertInvoiceData.InitialAssessedValue = TotalNoticedValue;
            //                objInsertInvoiceData.FinalAssessedValue = TotalPostHearingValue;
            //                objInsertInvoiceData.PriorYearTaxRate = TotalPriorYearTaxRate;
            //                objInsertInvoiceData.Reduction = TotalReduction;
            //                objInsertInvoiceData.FlatFee = objInv.Flat_fee;
            //            }
            //        }
            //        //Added by Pavithra.B on 3Nov2016 - TFS Id-26636
            //        objInsertInvoiceData.InvoiceAmount = Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount) + Convert.ToDecimal(objInv.Flat_fee);
            //        objInvoiceid.InitialAssessedValue = Convert.ToDecimal(objInsertInvoiceData.InitialAssessedValue);
            //        objInvoiceid.FinalAssessedValue = Convert.ToDecimal(objInsertInvoiceData.FinalAssessedValue);
            //        objInvoiceid.Reduction = Convert.ToDecimal(objInsertInvoiceData.Reduction);
            //        objInvoiceid.PriorYearTaxRate = Math.Round(Convert.ToDouble(objInsertInvoiceData.PriorYearTaxRate * 100), 8);
            //        if (objInvoiceid.Reduction > 0)
            //        {
            //            objInvoiceid.TotalEstimatedTaxSavings = Math.Round(Convert.ToDecimal(objInsertInvoiceData.TotalEstimatedTaxSaving), 2);
            //            objInvoiceid.ContingencyFee = Math.Round(Convert.ToDecimal(objInsertInvoiceData.ContingencyFee), 2);
            //            objInvoiceid.ContingencyPercentage = objInv.Contingency / 100;
            //            objInvoiceid.InvoiceAmount = Math.Round(Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount), 2);
            //            objInvoiceid.FlatFee = objInv.Flat_fee;
            //            SaveOrUpdateInvoice(objInvoiceid,out invoiceID);
            //            objInvoiceid.InvoiceID = invoiceID;
            //            if (objInv.CapValue > 0 && Convert.ToDecimal((objInv.Contingency / 100)) > 0 && Math.Round(objInvoiceid.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee, 2) > Convert.ToDecimal(objInv.CapValue))
            //            {
            //                lstInvoicePaymentMap = GetInvoicePaymentMap(objInv.InvoiceId);
            //                if (lstInvoicePaymentMap != null && lstInvoicePaymentMap.Count > 0)
            //                {
            //                    PTXboPayment Objpayment;
            //                    foreach (PTXboInvoicePaymentMap obiInvPay in lstInvoicePaymentMap)
            //                    {
            //                        var lstPayment = GetInvoicePayment(obiInvPay.PaymentId, "Cap Value Adjustment");
            //                        if (lstPayment != null)
            //                        {
            //                            Objpayment = lstPayment;
            //                        }
            //                        else
            //                        {
            //                            Objpayment = new PTXboPayment();
            //                        }
            //                        decimal PaymentAmount = 0;
            //                        PaymentAmount = Math.Round((objInvoiceid.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee) - Convert.ToDecimal(objInv.CapValue), 2);
            //                        Objpayment.ClientId = objInv.ClientId ;
            //                        Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId() ;
            //                        Objpayment.PaymentAmount = PaymentAmount;
            //                        Objpayment.PaymentDescription = "Cap Value Adjustment";
            //                        Objpayment.CreatedBy =  updatedBy ;
            //                        Objpayment.CreatedDateTime = DateTime.Now;
            //                        Objpayment.UpdatedBy =   updatedBy ;
            //                        Objpayment.UpdatedDateTime = DateTime.Now;
            //                        isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, objInv.InvoiceId, out errorMessage);
            //                        if (isSuccessCapValue)
            //                        {
            //                            bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errorMessage);
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    PTXboPayment Objpayment = new PTXboPayment();
            //                    decimal PaymentAmount = 0;
            //                    PaymentAmount = Math.Round((objInvoiceid.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee) - Convert.ToDecimal(objInv.CapValue), 2);
            //                    Objpayment.ClientId =  objInv.ClientId ;
            //                    Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId() ;
            //                    Objpayment.PaymentAmount = PaymentAmount;
            //                    Objpayment.PaymentDescription = "Cap Value Adjustment";
            //                    Objpayment.CreatedBy =  updatedBy ;
            //                    Objpayment.CreatedDateTime = DateTime.Now;
            //                    Objpayment.UpdatedBy = updatedBy ;
            //                    Objpayment.UpdatedDateTime = DateTime.Now;
            //                    isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, objInv.InvoiceId, out errorMessage);
            //                    if (isSuccessCapValue)
            //                    {
            //                        bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errorMessage);
            //                    }
            //                }
            //            }
            //            else if (Convert.ToDecimal(objInvoiceid.AmountAdjusted) != 0)
            //            {
            //                RemoveCapvalueAdjustment(objInvoiceid.InvoiceID);
            //               bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errorMessage);
            //            }
            //        }
            //    }
            //}
                Logger.For(this).Invoice("ProjectorTermInvoiceGeneration-API  ends successfully ");
                return isReturn;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("ProjectorTermInvoiceGeneration-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public bool UpdateInvoiceProcessingStatus(int invoicingProcessingStatusID, PTXboInvoiceDetails invoiceDetails)
        {

            try
            {
               // int invoiceID = 0;
                Logger.For(this).Invoice("UpdateInvoiceProcessingStatus-API  reached " + ((object)invoiceDetails).ToJson(false));
                //    var ObjInvoice = GetInvoiceGenerationDetails(invoiceDetails.InvoiceId);
                //if (ObjInvoice != null)
                //{

                //    ObjInvoice.ManuallyGeneratedUserId = invoiceDetails.ManualGeneratedUseriD;
                //    ObjInvoice.ManuallyGeneratedUserRoleId = invoiceDetails.ManualGeneratedUserRoleID ;
                //    if (invoiceDetails.onHold)
                //    {
                //        ObjInvoice.OnHold = invoiceDetails.onHold;
                //        ObjInvoice.OnHoldDate = invoiceDetails.onHoldDate;
                //        ObjInvoice.InvoicingProcessingStatusId =  Enumerators.PTXenumInvoicingPRocessingStatus.InvoiceSkippedForTheCurrentDate.GetId() ;
                //    }
                //    else
                //    {
                //        ObjInvoice.InvoicingProcessingStatusId =  invoicingProcessingStatusID ;
                //    }
                //    SaveOrUpdateInvoice(ObjInvoice,out invoiceID);

                //    }

                _invoiceCalculation.UpdateInvoiceProcessingStatus(invoicingProcessingStatusID, invoiceDetails);
                Logger.For(this).Invoice("UpdateInvoiceProcessingStatus-API  ends successfully ");
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateInvoiceProcessingStatus-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        //public PTXboInvoiceSummary GetSpecificInvoiceSummary(int invoicesummaryID)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("GetSpecificInvoiceSummary-API  reached " + ((object)invoicesummaryID).ToJson(false));
        //        //parameters.Add("@InvoicesummaryID", invoicesummaryID);

        //        //var result = _connection.Select<PTXboInvoiceSummary>(StoredProcedureNames.GetSpecificInvoiceSummary, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        var result = _invoiceCalculation.GetSpecificInvoiceSummary(invoicesummaryID);
        //        Logger.For(this).Invoice("GetSpecificInvoiceSummary-API  ends successfully ");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetSpecificInvoiceSummary-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}





        //public bool SaveOrUpdateInvoiceSummary(PTXboInvoiceSummary invoiceSummary)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceSummary-API  reached " + ((object)invoiceSummary).ToJson(false));
        //        //parameters.Add("@InvoiceSummaryId", invoiceSummary.InvoicesummaryID);
        //        //parameters.Add("@YearlyHearingDetailsId", invoiceSummary.YearlyHearingDetailsID);
        //        //parameters.Add("@InvoiceGenerated", invoiceSummary.InvoiceGenerated);
        //        //parameters.Add("@InvoiceGeneratedForId", invoiceSummary.InvoiceGeneratedForID);
        //        //parameters.Add("@InvoiceStatusId", invoiceSummary.InvoiceStatusID);
        //        //parameters.Add("@InvoiceSummaryProcessingStatusId", invoiceSummary.InvoiceSummaryProcessingStatusID);
        //        //parameters.Add("@GroupId", invoiceSummary.GroupId);
        //        //parameters.Add("@ClientId", invoiceSummary.ClientId);
        //        //parameters.Add("@ProjectId", invoiceSummary.ProjectID);
        //        //parameters.Add("@CreateDateTime", DateTime.Now);//invoiceSummary.CreateDateTime
        //        //parameters.Add("@InvoiceID", invoiceSummary.InvoiceID);
        //        //var result = _connection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateInvoiceSummary, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        var result = _invoiceCalculation.SaveOrUpdateInvoiceSummary(invoiceSummary);
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceSummary-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceSummary-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}


        public bool SubmitInvoiceSummary(PTXboInvoiceSummary invoiceSummary)
        {
            try
            {
                Logger.For(this).Invoice("SubmitInvoiceSummary-API  reached " + ((object)invoiceSummary).ToJson(false));
                //var objAccountInvoiceSummary = GetSpecificInvoiceSummary(invoiceSummary.InvoicesummaryID);
                //if (objAccountInvoiceSummary == null)
                //{
                //    objAccountInvoiceSummary = new PTXboInvoiceSummary();
                //}
                //objAccountInvoiceSummary.YearlyHearingDetailsID = invoiceSummary.YearlyHearingDetailsID;
                //objAccountInvoiceSummary.InvoiceGenerated = invoiceSummary.InvoiceGenerated;
                //objAccountInvoiceSummary.InvoiceGeneratedForID = invoiceSummary.InvoiceGeneratedForID;
                //objAccountInvoiceSummary.InvoiceStatusID = invoiceSummary.InvoiceStatusID;
                //objAccountInvoiceSummary.InvoiceSummaryProcessingStatusID = invoiceSummary.InvoiceSummaryProcessingStatusID;
                //objAccountInvoiceSummary.GroupId = invoiceSummary.GroupId;
                //objAccountInvoiceSummary.ProjectID = invoiceSummary.ProjectID;
                //objAccountInvoiceSummary.ClientId = invoiceSummary.ClientId;

                //SaveOrUpdateInvoiceSummary(objAccountInvoiceSummary);
                _invoiceCalculation.SubmitInvoiceSummary(invoiceSummary);
                Logger.For(this).Invoice("SubmitInvoiceSummary-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SubmitInvoiceSummary-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }



        public bool UpdateClientCorrQueueRecord(List<PTXboCorrQueue> objCorrQueueList, PTXboInvoice invoice)
        {

            Hashtable parameters = new Hashtable();
            PTXboInvoiceContact invoiceContact = new PTXboInvoiceContact();
            try
            {
                int invoiceID = 0;
                Logger.For(this).Invoice("UpdateClientCorrQueueRecord-API  reached " + ((object)objCorrQueueList).ToJson(false));
                if (objCorrQueueList != null)
                {
                    List<PTXboCorrQueue> lstCorrQueue = new List<PTXboCorrQueue>();
                    lstCorrQueue = ValidateClientEmail(objCorrQueueList);

                    foreach (PTXboCorrQueue objCorrQueue in lstCorrQueue)
                    {
                        if (!objCorrQueue.IsCustomDelivery)
                        {
                            var clientGroupContact = GetInvoiceAddressReference(objCorrQueue.ClientID,1);

                            if (clientGroupContact != null)
                            {
                                if (objCorrQueue.DeliveryMethodId ==1 )//Email
                                {
                                    objCorrQueue.DeliveryAddress = clientGroupContact.Email;
                                }
                                else if (objCorrQueue.DeliveryMethodId == 3) //USMail
                                {
                                    objCorrQueue.DeliveryAddress = clientGroupContact.Address;
                                }
                                else if (objCorrQueue.DeliveryMethodId ==2)//Fax
                                {
                                    if (clientGroupContact.ContactID != 0)
                                    {
                                        var Contact = GetContactDetails(Convert.ToInt32(clientGroupContact.ContactID));

                                        if (Contact != null && Contact.PhoneNumber != null)
                                        {
                                            if (Contact.IsItFax != true)
                                            {
                                                objCorrQueue.DeliveryAddress = Contact.PhoneNumber;
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        //Inserting invoice details in ptaxcorrqueue table
                        var corrQID = SaveOrUpdateCorrQ(objCorrQueue);
                        //update corrid in corracc
                        if (objCorrQueue.AccountList.Any())
                        {
                            foreach (var CorrAccount in objCorrQueue.AccountList.Split(','))
                            {
                                //Inserting account details in ptaxcorraccounts table
                                UpdateCorrAccounts(Convert.ToInt32(CorrAccount), corrQID);
                            }
                        }

                        SaveOrUpdateInvoice(invoice,out invoiceID);

                        //updating invoice status in 1-ptaxhearingresult,2-ptaxlitigation,3-ptaxarbitrationdetails
                        //UpdateInvoiceStatus(objCorrQueue.LinkFieldValue, objCorrQueue.CorrProcessingStatusID);
                    }
                    Logger.For(this).Invoice("UpdateClientCorrQueueRecord-API  ends successfully ");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateClientCorrQueueRecord-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }



        /// <summary>
        /// This method is used to bind  the Invoice summary data with the appropriate field and insert invoice summary.-added by saravanans-tfs:47247
        /// </summary>
        /// <param name="invoicesummary"></param>
        /// <param name="invoiceSummaryProcessingStatusID"></param>
        //public void InsertInvoiceSummaryWithProcessingStatus(PTXboInvoiceSummary invoiceSummary, PTXboInvoice objInvoice, out string errorString)
        //{
        //    invoiceSummary.YearlyHearingDetailsID = objInvoice.YearlyHearingDetailsId;
        //    invoiceSummary.GroupId = objInvoice.GroupId;
        //    invoiceSummary.ProjectID = objInvoice.ProjectId;// == null ? 0 : boInvoice.ProjectId ;
        //    invoiceSummary.ClientId = objInvoice.ClientId;
        //    //Added by Pavithra.B on 17Nov2015 - Arbitration
        //    errorString = string.Empty;
        //    try
        //    {
        //        Logger.For(this).Invoice("InsertInvoiceSummaryWithProcessingStatus-API  reached " + ((object)invoiceSummary).ToJson(false));
        //        _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice,out errorString);
        //        //if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
        //        //{
        //        //    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.ArbitrationLegalFees.GetId();
        //        //}
        //        //else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
        //        //{
        //        //    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.LitigationLegalFees.GetId();
        //        //}
        //        //else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
        //        //{
        //        //    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.BPPrenditionFees.GetId();
        //        //}
        //        //else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //        //{
        //        //    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.TaxBillAuditFees.GetId();
        //        //}
        //        //else
        //        //{
        //        //    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.HearingProcessFees.GetId();
        //        //}

        //        //if (objInvoice.IsSpecialTerm == true && objInvoice.CanGenerateInvoice == false && objInvoice.DontGenerateInvoiceFlag == true)
        //        //{
        //        //    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.SpecialtermFees.GetId();
        //        //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.Specialterminvoicingnotgeneratedforaccount.GetId();
        //        //}
        //        //invoiceSummary.InvoiceGenerated = true;
        //        //SubmitInvoiceSummary(invoiceSummary);
               
        //        Logger.For(this).Invoice("InsertInvoiceSummaryWithProcessingStatus-API  ends successfully ");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("InsertInvoiceSummaryWithProcessingStatus-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}


        /// <summary>
        /// This method is used to validate the Invoice data whether it is valid or not for generating a invoice.-added by saravanans-tfs:47247
        /// </summary>
        /// <param name="objInv"></param>
        /// <param name="errorstring"></param>
        /// <returns></returns>
        //public bool ValidateInvoiceDetails(PTXboInvoice objInv, out string errorstring)
        //{
        //    PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
        //    errorstring = string.Empty;
        //    try
        //    {
        //        Logger.For(this).Invoice("validateInvoiceDetails-API  reached " + ((object)objInv).ToJson(false));
        //        var result = _invoiceCalculation.ValidateInvoiceDetails(objInv,out errorstring);
        //    //if (objInv.InitialAssessedValue == 0 && (objInv.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInv.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId()))
        //    //{
        //    //    errorstring = "Update the status as Noticed Data not available message";
                
        //    //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ErrorTotalNoticedValueIsNotAvailable.GetId();
        //    //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInv, out errorstring);
        //    //    return false;
        //    //}
  
        //    //if (objInv.FinalAssessedValue == 0 && (objInv.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInv.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId()))
        //    //{
        //    //    errorstring = "Post Hearing value is not available";
                
        //    //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ErrorTotalPostHearingValueIsNotAvailable.GetId();
        //    //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInv, out errorstring);
        //    //    return false;
        //    //}


        //    //if (objInv.TermExpiryDate != null && objInv.TermExpiryDate < DateTime.Now)
        //    //{
        //    //    errorstring="Terms has expired";
                
        //    //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ErrorReadyForInvoicingButTermHasExpired.GetId();
        //    //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInv, out errorstring);
        //    //    return false;
        //    //}
        //    //if (objInv.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId() && (objInv.FinalAssessedValue == null || objInv.FinalAssessedValue == 0))
        //    //{
        //    //    errorstring = " Arbitration SettlementAmt is not present";
                
        //    //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ArbitrationSettlementAmtisnotpresent.GetId();
        //    //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInv, out errorstring);
        //    //    return false;
        //    //}
        //        Logger.For(this).Invoice("validateInvoiceDetails-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("validateInvoiceDetails-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public List<PTXboInvoiceLineItem> CheckPosttoFlatFeeInvoicing(int accountId, int invoicingLevel)
        //{
        //    try
        //    {
        //        Logger.For(this).Invoice("CheckPosttoFlatFeeInvoicing-API  reached " + ((object)"accountId="+ accountId.ToString()+ "invoicingLevel="+ invoicingLevel.ToString()).ToJson(false));
        //        //Hashtable parameters = new Hashtable();
        //        List<PTXboInvoiceLineItem> invoiceDetails = new List<PTXboInvoiceLineItem>();
        //        //parameters.Add("@Id", accountId);
        //        //parameters.Add("@Invoicinglevel", invoicingLevel);
        //        //invoiceDetails = _connection.Select<PTXboInvoiceLineItem>(StoredProcedureNames.usp_CheckPosttoFlatFeeInvoicing, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
        //        invoiceDetails = _invoiceCalculation.CheckPosttoFlatFeeInvoicing(accountId, invoicingLevel);
        //        Logger.For(this).Invoice("CheckPosttoFlatFeeInvoicing-API  ends successfully ");
        //        return invoiceDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("CheckPosttoFlatFeeInvoicing-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public List<Int32> GetDistinctHearingResultIDs(List<PTXboInvoice> lstInvoiceGroupTypeAccounts, int InvoiceTypeId)
        //{
        //    Logger.For(this).Invoice("GetDistinctHearingResultIDs-API  reached " + ((object)lstInvoiceGroupTypeAccounts).ToJson(false));
        //    List<int> lstHearingResultIDs = new List<int>();
        //    //if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
        //    //{
        //    //    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.ArbitrationDetilId).Distinct().ToList();
        //    //}
        //    //else if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
        //    //{
        //    //    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.LitigationId).Distinct().ToList();
        //    //}
        //    //else if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
        //    //{
        //    //    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.BppRenditionId).Distinct().ToList();
        //    //}
        //    //else if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //    //{
        //    //    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.TaxBillAuditId).Distinct().ToList();
        //    //}
        //    //else
        //    //{
        //    //    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.HearingResultId).Distinct().ToList();
        //    //}
        //    List<Int32> lstHearingResultID = new List<int>();
        //    //foreach (int hearingResultId in lstHearingResultIDs)
        //    //{
        //    //    if (hearingResultId != 0)
        //    //    {
        //    //        lstHearingResultID.Add(hearingResultId);
        //    //    }
        //    //}
        //    lstHearingResultID = _invoiceCalculation.GetDistinctHearingResultIDs(lstInvoiceGroupTypeAccounts, InvoiceTypeId);
        //    Logger.For(this).Invoice("GetDistinctHearingResultIDs-API  ends successfully ");
        //    return lstHearingResultID;
        //}

        //public bool GetInvoiceAndHearingResultMapStatus(int invoiceID, int hearingResultId, int invoiceTypeId, int invoicingProcessingStatusId)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("GetInvoiceAndHearingResultMapStatus-API  reached " + ((object)"invoiceID="+ invoiceID.ToString()+ "hearingResultId="+ hearingResultId.ToString()+ "invoiceTypeId="+ invoiceTypeId.ToString()+ "invoicingProcessingStatusId="+ invoicingProcessingStatusId.ToString()).ToJson(false));
        //        parameters.Add("@HearingResultID", hearingResultId);
        //        parameters.Add("@InvoiceID", invoiceID);
        //        parameters.Add("@InvoiceTypeID", invoiceTypeId);
        //        parameters.Add("@InvoicingProcessingStatusID", invoicingProcessingStatusId);

        //        var result = _connection.ExecuteScalar(StoredProcedureNames.usp_insert_InvoiceAndHearingResultMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        Logger.For(this).Invoice("GetInvoiceAndHearingResultMapStatus-API  ends successfully ");
        //        return Convert.ToBoolean(result);
        //       // return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetInvoiceAndHearingResultMapStatus-API  error "+ ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public void SubmitInvoiceAndHearingResultMap(int invoiceID,int hearingResultId,int invoiceTypeId,int invoicingProcessingStatusId, PTXboInvoice objInvoice)
        //{
        //    string errorMessage = string.Empty;

        //    Logger.For(this).Invoice("SubmitInvoiceAndHearingResultMap-API  reached " + ((object)objInvoice).ToJson(false));
        //    //if (invoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId() || invoiceTypeId== Enumerators.PTXenumInvoiceType.BPP.GetId() || invoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId())
        //    //    {
        //    //       if(!GetInvoiceAndHearingResultMapStatus(invoiceID, hearingResultId, invoiceTypeId, invoicingProcessingStatusId))
        //    //        {
        //    //            PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
        //    //            invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceAndHearingResultMapNotMapped.GetId();
        //    //            invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
        //    //            InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //         if (invoicingProcessingStatusId != Enumerators.PTXenumInvoicingPRocessingStatus.WaitingInPendingResearchQueue.GetId())
        //    //          {
        //    //                if (!GetInvoiceAndHearingResultMapStatus(invoiceID, hearingResultId, invoiceTypeId, invoicingProcessingStatusId))
        //    //                 {
        //    //                      PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
        //    //                      invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceAndHearingResultMapNotMapped.GetId();
        //    //                      invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
        //    //                      InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
        //    //                 }
        //    //          }

        //    //    }
        //    _invoiceCalculation.SubmitInvoiceAndHearingResultMap(invoiceID, hearingResultId, invoiceTypeId, invoicingProcessingStatusId, objInvoice);

        //    Logger.For(this).Invoice("SubmitInvoiceAndHearingResultMap-API  ends successfully ");
        //}


        /// <summary>
        /// this method is used to submit a record into Invoice table and also InvoiceHearingResultMap table
        /// </summary>

        //public bool SubmitInvoice(PTXboInvoice objInvoice, List<Int32> lstHearingResultID, out int NewlyGeneratedInvoiceID, out string errorstring)
        //{
        //    NewlyGeneratedInvoiceID = 0;
        //    errorstring = string.Empty;
        //    try
        //    {
        //        Logger.For(this).Invoice("SubmitInvoice-API  reached " + ((object)objInvoice).ToJson(false));
        //        //var objAccountInvoice = GetInvoiceDetailsById(objInvoice.InvoiceID);
        //        //if (objAccountInvoice == null)
        //        //{
        //        //    objAccountInvoice = new PTXboInvoice();
        //        //    objAccountInvoice.CreatedDateAndTime = objInvoice.CreatedDateAndTime;
        //        //}

        //        //objAccountInvoice.InvoicingStatusId = objInvoice.InvoicingStatusId;
        //        //objAccountInvoice.AutoGenerated = objInvoice.AutoGenerated == null ? false : objAccountInvoice.AutoGenerated = objInvoice.AutoGenerated;
        //        //objAccountInvoice.ClientId =  objInvoice.ClientId;
        //        //// objAccountInvoice.CompoundInterest = objInvoice.CompoundInterest;
        //        //objAccountInvoice.ContingencyFee = objInvoice.ContingencyFee;
        //        //objAccountInvoice.ContingencyPercentage = objInvoice.ContingencyPercentage;

        //        //objAccountInvoice.DeliveryMethodId = objInvoice.DeliveryMethodId ;
        //        ////objAccountInvoice.DeliveryStatus = objInvoice.DeliveryStatusId == 0 ? null : new PTXdoDeliveryStatus() { DeliverystatusId = objInvoice.DeliveryStatusId };
        //        //objAccountInvoice.FinalAssessedValue = objInvoice.FinalAssessedValue;
        //        //objAccountInvoice.InitialAssessedValue = objInvoice.InitialAssessedValue;
        //        //objAccountInvoice.FlatFee = objInvoice.FlatFee;
        //        ////  objAccountInvoice.InterestBalance = objInvoice.InterestBalance;
        //        ////objAccountInvoice.InterestPayments = objInvoice.InterestPayments;
        //        ////  objAccountInvoice.InterestRate = objInvoice.InterestRate;
        //        //objAccountInvoice.InvoiceAmount = objInvoice.InvoiceAmount;
        //        ////objAccountInvoice.InvoiceCreditAmount = objInvoice.InvoiceCreditAmount;
        //        //// Commented by yuvaraj.
        //        ////objAccountInvoice.InvoiceDate = objInvoice.InvoiceDate;
        //        //objAccountInvoice.InvoiceDescription = objInvoice.InvoiceDescription;
        //        //objAccountInvoice.OnHold = objInvoice.OnHold == null ? false : objAccountInvoice.OnHold = objInvoice.OnHold;
        //        //objAccountInvoice.OnHoldDate = objInvoice.OnHoldDate;
        //        //objAccountInvoice.PaymentDueDate = null; // modified by Arunkumar S | TFS ID: 40863
        //        //objAccountInvoice.PriorYearTaxRate = objInvoice.PriorYearTaxRate;
        //        //objAccountInvoice.ProjectId= objInvoice.ProjectId ;
        //        //objAccountInvoice.SentDateAndTime = objInvoice.SentDateAndTime;
        //        //objAccountInvoice.TaxYear = objInvoice.TaxYear;
        //        //objAccountInvoice.TotalAmountPaid = objInvoice.TotalAmountPaid;
        //        //objAccountInvoice.UpdatedBy =objInvoice.CreatedBy;
        //        //objAccountInvoice.UpdatedDateTime = DateTime.Now;
        //        ////    objAccountInvoice.TotalBalance = objInvoice.TotalBalance;
        //        //// objAccountInvoice.TotalInvoiceAmount = objInvoice.TotalInvoiceAmount;
        //        //objAccountInvoice.YearlyHearingDetailsId = objInvoice.YearlyHearingDetailsId;
        //        //objAccountInvoice.GroupId = objInvoice.GroupId;
        //        //objAccountInvoice.InvoiceTypeId = objInvoice.InvoiceTypeId;
        //        //objAccountInvoice.InvoicingProcessingStatus =objInvoice.InvoicingProcessingStatusId;
        //        //objAccountInvoice.InvoiceGroupingTypeId =objInvoice.InvoiceGroupingTypeId;

        //        //if (lstHearingResultID.Count > 0)
        //        //{
        //        //    //if (objAccountInvoice.InvoiceAndHearingResultMap == null)
        //        //    //{
        //        //    //    objAccountInvoice.InvoiceAndHearingResultMap = new List<PTXboInvoiceAndHearingResultMap>();
        //        //    //}


        //        //    foreach (int hearingResultId in lstHearingResultID)
        //        //    {
        //        //        if (hearingResultId != 0)
        //        //        {
        //        //            SubmitInvoiceAndHearingResultMap(objAccountInvoice.InvoiceID, hearingResultId, objInvoice.InvoiceTypeId, objInvoice.InvoicingProcessingStatusId, objAccountInvoice);

        //        //        }
        //        //    }
        //        //}

        //        //SaveOrUpdateInvoice(objAccountInvoice,out NewlyGeneratedInvoiceID);
        //        //if(objAccountInvoice.InvoiceID!=0)
        //        //{
        //        //    NewlyGeneratedInvoiceID = objAccountInvoice.InvoiceID;//Newly created invoice ID  
        //        //}
        //        _invoiceCalculation.SubmitInvoice(objInvoice, lstHearingResultID, out NewlyGeneratedInvoiceID, out errorstring);
        //        Logger.For(this).Invoice("SubmitInvoice-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SubmitInvoice-API  error "+ ((object)ex).ToJson(false));
        //        errorstring = ex.Message;
        //        return false;
        //    }
        //}


        /// <summary>
        /// added by saravanans-tfs:47247
        /// </summary>
        /// <param name="objInvoice"></param>
        /// <param name="lstInvoiceDetails"></param>
        /// <param name="hearingType"></param>
        /// <param name="invoicingGroupType"></param>
        /// <param name="createdBy"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public PTXboAccountLevelInvoiceOutput InsertAccountLevelInvoiceGeneration(PTXboInvoice objInvoice, List<PTXboInvoice> lstInvoiceDetails, string hearingType, string invoicingGroupType, int createdBy)
        {
           // bool isInvoiceRecordCreated = false;
            PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
            PTXboInvoice objInsertInvoiceData = new PTXboInvoice();
            PTXboAccountLevelInvoiceOutput invoiceOutput = new PTXboAccountLevelInvoiceOutput();
           // bool isinvoicedatavalid = false;
           // bool isinvoicevalidationfails = false;
           // int newlycreatedinvoiceid = 0;
           // string errormessage = string.empty;
            try
            {
                //    Logger.For(this).Invoice("InsertAccountLevelInvoiceGeneration-API  reached " + ((object)lstInvoiceDetails).ToJson(false));
                //    //Added by Kishore to prevent inserting multiple Hearing Type
                //    lstInvoiceDetails = lstInvoiceDetails.Where(a => a.AccountId == objInvoice.AccountId && a.HearingTypeId == objInvoice.HearingTypeId).ToList();
                //if (ValidateInvoiceDetails(objInvoice, out errorMessage))
                //{
                //    //Kishore- For Special Term Invoices
                //    if ((objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId()) && objInvoice.IsSpecialTerm == true)
                //    {
                //        objInvoice.Reduction = Convert.ToDecimal((objInvoice.InitialAssessedValue) - (objInvoice.FinalAssessedValue));
                //        objInvoice.EstimatedTaxSaving = (Convert.ToDecimal(objInvoice.PriorYearTaxRate) / 100) * Convert.ToDecimal((objInvoice.Reduction));
                //        objInvoice.ContingencyFee = (objInvoice.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage));
                //        objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
                //        objInsertInvoiceData = objInvoice;
                //        IsInvoiceDataValid = true;

                //    }
                //    else
                //    {
                //        // K.Selva - It Checks   the flat fee   amount for this account
                //        if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                //        {
                //            if (objInvoice.FlatFee > 0 && objInvoice.FlatFee != null)
                //            {
                //                objInvoice.InvoiceAmount = objInvoice.FlatFee;
                //                objInsertInvoiceData = objInvoice;
                //                IsInvoiceDataValid = true;
                //            }
                //            else
                //            {
                //                invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoicenotrequiredFlatFeenotNoticed.GetId();
                //                invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //                InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                //                IsInvoiceDataValid = false;
                //            }
                //        }
                //        else
                //        {
                //            objInvoice.Reduction = Convert.ToDecimal((objInvoice.InitialAssessedValue) - (objInvoice.FinalAssessedValue));
                //            if (objInvoice.Reduction > 0)
                //            {
                //                objInvoice.EstimatedTaxSaving = Convert.ToDecimal((objInvoice.PriorYearTaxRate / 100)) * Convert.ToDecimal((objInvoice.Reduction));
                //                objInvoice.ContingencyFee = Math.Round((objInvoice.EstimatedTaxSaving) * Convert.ToDecimal(objInvoice.ContingencyPercentage), 2);
                //                if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId())
                //                {
                //                        List<PTXboInvoiceLineItem> dtInvoiceLineItem = CheckPosttoFlatFeeInvoicing(objInvoice.AccountId, 1);
                //                    if (dtInvoiceLineItem.Count == 0)
                //                    {
                //                        objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
                //                    }
                //                    else
                //                    {
                //                        objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault();
                //                    }
                //                }
                //                else
                //                {
                //                    objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
                //                }

                //                objInsertInvoiceData = objInvoice;
                //                IsInvoiceDataValid = true;
                //            }
                //            else if (objInvoice.Reduction <= 0 && (objInvoice.FlatFee != null && objInvoice.FlatFee != 0))//Added By Pavithra.B on 02Feb2017 - if Reduction <=0 and flatfee is available then create Invoice based on FlatFee
                //            {
                //                objInvoice.EstimatedTaxSaving = Convert.ToDecimal((objInvoice.PriorYearTaxRate / 100)) * Convert.ToDecimal((objInvoice.Reduction));
                //                objInvoice.ContingencyFee = Math.Round((objInvoice.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)), 2);
                //                objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
                //                objInsertInvoiceData = objInvoice;
                //                IsInvoiceDataValid = true;
                //            }
                //            else
                //            {
                //                invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceNotRequiredReductionNotNoticed.GetId();
                //                invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //                InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                //                IsInvoiceDataValid = false;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    objInsertInvoiceData = objInvoice;
                //    IsInvoiceValidationFails = true;
                //}

                //objInsertInvoiceData.InvoiceDescription = objInvoice.AccountNumber;
                //objInsertInvoiceData.InvoiceDate = DateTime.Now;
                //if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                //{
                //    objInsertInvoiceData.PaymentDueDate = DateTime.Now.AddDays(90);
                //}
                //else
                //{
                //    objInsertInvoiceData.PaymentDueDate = DateTime.Now.AddDays(30);
                //}
                //objInsertInvoiceData.CreatedDateAndTime = DateTime.Now;
                //objInsertInvoiceData.AutoGenerated = true;
                //List<Int32> lstHearingResultID = GetDistinctHearingResultIDs(lstInvoiceDetails, objInvoice.InvoiceTypeId);

                //if (IsInvoiceDataValid)
                //{
                //    if (objInvoice.IsSpecialTerm)
                //        objInsertInvoiceData.FlatFee = objInvoice.FlatFee.GetValueOrDefault();

                //    objInsertInvoiceData.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
                //    objInsertInvoiceData.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.ReadyForInvoicing.GetId();
                //    objInsertInvoiceData.InvoiceTypeId = objInvoice.InvoiceTypeId;
                //    isInvoiceRecordCreated = SubmitInvoice(objInsertInvoiceData, lstHearingResultID, out NewlyCreatedInvoiceID, out errorMessage);

                //      //added by saravanans-updating invoiceid if it's 0
                //     if (objInsertInvoiceData.InvoiceID == 0)
                //     {
                //      objInsertInvoiceData.InvoiceID = NewlyCreatedInvoiceID;
                //     }
                //     //ends here

                //    if (isInvoiceRecordCreated == true && NewlyCreatedInvoiceID != 0)
                //    {
                //        bool isSuccessCapValue = false;
                //        string errormessage = string.Empty;
                //        List<PTXboInvoicePaymentMap> lstInvoicePaymentMap;
                //        var objInvoiceid = GetInvoiceDetailsById(NewlyCreatedInvoiceID); 
                //        if (objInvoiceid != null)
                //        {
                //            //Pavithra.B - Included Cap Value
                //            if (objInsertInvoiceData.CapValue > 0 && Convert.ToDecimal((objInvoice.ContingencyPercentage)) > 0 && Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount) > Convert.ToDecimal(objInsertInvoiceData.CapValue))
                //            {
                //                    lstInvoicePaymentMap = GetInvoicePaymentMap(NewlyCreatedInvoiceID);
                //                if (lstInvoicePaymentMap != null && lstInvoicePaymentMap.Count > 0)
                //                {
                //                    PTXboPayment Objpayment = new PTXboPayment();
                //                    foreach (PTXboInvoicePaymentMap obiInvPay in lstInvoicePaymentMap)
                //                    {
                //                            var lstPayment = GetInvoicePayment(obiInvPay.PaymentId, "Cap Value Adjustment"); 
                //                        if (lstPayment != null)
                //                        {
                //                            Objpayment = lstPayment;
                //                        }

                //                        decimal PaymentAmount = 0;
                //                        PaymentAmount = Math.Round((objInvoiceid.InvoiceAmount.GetValueOrDefault()) - Convert.ToDecimal(objInsertInvoiceData.CapValue), 2);
                //                        Objpayment.ClientId =objInsertInvoiceData.ClientId ;
                //                        Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                //                        Objpayment.PaymentAmount = PaymentAmount;
                //                        Objpayment.PaymentDescription = "Cap Value Adjustment";
                //                        Objpayment.CreatedBy = createdBy ;
                //                        Objpayment.CreatedDateTime = DateTime.Now;
                //                        Objpayment.UpdatedBy = createdBy;
                //                        Objpayment.UpdatedDateTime = DateTime.Now;
                //                        isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, NewlyCreatedInvoiceID, out errormessage);
                //                        if (isSuccessCapValue)
                //                        {
                //                            bool isSuccess = UpdateAmountDueCapValueChange(NewlyCreatedInvoiceID, out errormessage);
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    PTXboPayment Objpayment = new PTXboPayment();
                //                    decimal PaymentAmount = 0;
                //                    PaymentAmount = Math.Round((objInvoiceid.InvoiceAmount.GetValueOrDefault()) - Convert.ToDecimal(objInsertInvoiceData.CapValue), 2);
                //                    Objpayment.ClientId = objInsertInvoiceData.ClientId;
                //                    Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId() ;
                //                    Objpayment.PaymentAmount = PaymentAmount;
                //                    Objpayment.PaymentDescription = "Cap Value Adjustment";
                //                    Objpayment.CreatedBy = createdBy ;
                //                    Objpayment.CreatedDateTime = DateTime.Now;
                //                    Objpayment.UpdatedBy = createdBy ;
                //                    Objpayment.UpdatedDateTime = DateTime.Now;
                //                    isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, NewlyCreatedInvoiceID, out errormessage);
                //                    if (isSuccessCapValue)
                //                    {
                //                        bool isSuccess = UpdateAmountDueCapValueChange(NewlyCreatedInvoiceID, out errormessage);
                //                    }
                //                }
                //            }
                //            else if (Convert.ToDecimal(objInvoiceid.AmountAdjusted) != 0)
                //            {
                //                    RemoveCapvalueAdjustment(objInvoiceid.InvoiceID);
                //                bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errormessage);
                //            }
                //        }

                //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ReadyForInvoicing.GetId();
                //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();

                //        invoiceSummary.InvoiceID = NewlyCreatedInvoiceID;
                //        InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);

                //        //Need to insert into Invoice Remarks
                //        PTXboInvoiceRemarks objInvoiceRemarks = new PTXboInvoiceRemarks();
                //        objInvoiceRemarks.InvoiceID = NewlyCreatedInvoiceID;

                //        if (invoicingGroupType == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString())
                //            objInvoiceRemarks.InvoiceRemarks = "Invoice created for Grouping level : " + Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString();
                //        else
                //            objInvoiceRemarks.InvoiceRemarks = "Invoice created for Grouping level : " + Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString() + " but the Original Grouping level is " + invoicingGroupType;
                //        //}

                //        objInvoiceRemarks.UpdatedBy = createdBy;
                //        InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objInvoiceRemarks, out errorMessage);
                //    }
                //    else
                //    {
                //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.TachnicalErrorInInvoiceGeneration.GetId();
                //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //        InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);
                //    }
                //}
                //if (IsInvoiceValidationFails)
                //{
                //    objInsertInvoiceData.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
                //    objInsertInvoiceData.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.WaitingInPendingResearchQueue.GetId();
                //    objInsertInvoiceData.InvoiceTypeId = objInvoice.InvoiceTypeId;
                //    isInvoiceRecordCreated = SubmitInvoice(objInsertInvoiceData, lstHearingResultID, out NewlyCreatedInvoiceID, out errorMessage);
                //        //added by saravanans-updating invoiceid if it's 0
                //        if (objInsertInvoiceData.InvoiceID == 0)
                //        {
                //            objInsertInvoiceData.InvoiceID = NewlyCreatedInvoiceID;
                //        }
                //        //ends here
                //     if (isInvoiceRecordCreated)
                //    {
                //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.WaitingInPendingResearchQueue.GetId();
                //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();

                //        invoiceSummary.InvoiceID = NewlyCreatedInvoiceID;
                //        InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);

                //        //Need to insert into Invoice Remarks
                //        PTXboInvoiceRemarks objInvoiceRemarks = new PTXboInvoiceRemarks();
                //        objInvoiceRemarks.InvoiceID = NewlyCreatedInvoiceID;
                //        if (invoicingGroupType == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString())
                //            objInvoiceRemarks.InvoiceRemarks = "Invoice created for Hearing Type : " + hearingType + " and Grouping level : " + Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString();
                //        else
                //            objInvoiceRemarks.InvoiceRemarks = "Invoice created for Hearing Type : " + hearingType + " and Grouping level : " + Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString() + " but the Original Grouping level is " + invoicingGroupType;
                //        objInvoiceRemarks.UpdatedBy = createdBy;
                //        InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objInvoiceRemarks, out errorMessage);
                //    }
                //    else
                //    {
                //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.TachnicalErrorInInvoiceGeneration.GetId();
                //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //        InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);
                //    }
                //}

                //    invoiceOutput.IsInvoiceRecordCreated = isInvoiceRecordCreated;
                //    invoiceOutput.InvoiceDetails = objInsertInvoiceData;
                //    return invoiceOutput;
                //    //return isInvoiceRecordCreated;
                invoiceOutput = _invoiceCalculation.InsertAccountLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, createdBy);
                Logger.For(this).Invoice("InsertAccountLevelInvoiceGeneration-API  ends successfully ");
                return invoiceOutput;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("InsertAccountLevelInvoiceGeneration-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }


        //public bool InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(PTXboInvoiceRemarks objBOInvoiceRemarks, out string errorMessage)
        //{

        //    errorMessage = string.Empty;
        //    string remarks = string.Empty;
        //    try
        //    {
        //        Logger.For(this).Invoice("InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel-API  reached " + ((object)objBOInvoiceRemarks).ToJson(false));
        //        //Hashtable parameters = new Hashtable();
        //        //parameters.Add("@InvoiceRemarksID", objBOInvoiceRemarks.InvoiceRemarksID);
        //        //parameters.Add("@InvoiceID", objBOInvoiceRemarks.InvoiceID);
        //        //parameters.Add("@InvoiceRemarks", objBOInvoiceRemarks.InvoiceRemarks);
        //        //parameters.Add("@RemarksTypeId", objBOInvoiceRemarks.RemarksTypeId);
        //        //parameters.Add("@UpdatedBy", objBOInvoiceRemarks.UpdatedBy);
        //        //parameters.Add("@UpdatedDateTime", objBOInvoiceRemarks.UpdatedDateTime);

        //        //var result = _connection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceRemarks, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        var result = _invoiceCalculation.InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objBOInvoiceRemarks,out errorMessage);
        //        Logger.For(this).Invoice("InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel-API  ends successfully ");
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel-API  error "+ ((object)ex).ToJson(false));
        //        errorMessage = ex.Message;
        //        throw ex;
                
        //    }
            
        //}

        public int GetInvoiceIdFromInvoiceAdjustment(int invoiceAdjustMentRequestId)
        {
            int invoiceId = 0;
            try
            {
                Logger.For(this).Invoice("GetInvoiceIdFromInvoiceAdjustment-API  reached " + ((object)invoiceAdjustMentRequestId).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceAdjustMentRequestId", invoiceAdjustMentRequestId);

                var result = _connection.ExecuteScalar(StoredProcedureNames.usp_GetInvoiceIdFromInvoiceAdjustment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("GetInvoiceIdFromInvoiceAdjustment-API  ends successfully ");
                int.TryParse(result.ToString(), out invoiceId);
                return invoiceId;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceIdFromInvoiceAdjustment-API  error "+ ((object)ex).ToJson(false));
                throw ex;

            }

        }

        public bool UpdateLetterProcessStatus(PTXboUpdateLetterProcessStatusInput letterProcessStatus)
        {
            Hashtable parameters = new Hashtable();
            
            try
            {

               parameters.Add("@corrQId", letterProcessStatus.CorrQID);
               parameters.Add("@status", letterProcessStatus.Status);
               parameters.Add("@notes", letterProcessStatus.Notes);
               parameters.Add("@servicePackageId", letterProcessStatus.ServicePackageId);
               parameters.Add("@messageId", letterProcessStatus.MessageId);
               parameters.Add("@pwImageId", letterProcessStatus.ImageId);
               parameters.Add("@fileCabinetId", letterProcessStatus.FileCabinetId);
                Logger.For(this).Invoice("UpdateLetterProcessStatus-API  reached " + ((object)letterProcessStatus).ToJson(false));
               var result = _connection.Execute(StoredProcedureNames.USP_UpdateQueueProcessStatus, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
               Logger.For(this).Invoice("UpdateLetterProcessStatus-API  ends successfully ");
               return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateLetterProcessStatus-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public List<PTXboCorrQueue> GetActiveCorrQueueRecordsForSelectedInvoices(PTXboActiveCorrqRecordsInput activeCorrqRecords)
        {
            Hashtable parameters = new Hashtable();
            List<PTXboCorrQueue>  corrQueueAccountList = new List<PTXboCorrQueue>();
            try
            {

                parameters.Add("@serviceName", activeCorrqRecords.ServiceName);
                parameters.Add("@invoices", activeCorrqRecords.Invoices);
                parameters.Add("@UserId", activeCorrqRecords.UserId); 
                Logger.For(this).Invoice("GetActiveCorrQueueRecordsForSelectedInvoices-API  reached " + ((object)activeCorrqRecords).ToJson(false));
                corrQueueAccountList = _connection.Select<PTXboCorrQueue>(StoredProcedureNames.USP_GetLetterAndClientReadyToProcessForSelectedInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetActiveCorrQueueRecordsForSelectedInvoices-API  ends successfully ");
                return corrQueueAccountList;
                
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetActiveCorrQueueRecordsForSelectedInvoices-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public PTXboClientEmail GetClientEmailInvoice(PTXboGetClientEmailInput objGetClientEmail)
        {
            try
            {
                Logger.For(this).Invoice("GetClientEmailInvoice-API  reached " + ((object)objGetClientEmail).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ClientId", objGetClientEmail.ClientId);
                parameters.Add("@GroupId", objGetClientEmail.GroupId);
                parameters.Add("@CorrQId", objGetClientEmail.CorrQId);
                
                var result = _connection.Select<PTXboClientEmail>(StoredProcedureNames.usp_GetClientInvoiceEmailDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetClientEmailInvoice-API  ends successfully ");

                //if (!(string.IsNullOrEmpty(result.CC)))
                //{
                //    result.AlternateEmail = result.CC;
                //}
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetClientEmailInvoice-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
        }

        /// <summary>
        /// Created by SaravananS. tfs id:60248
        /// </summary>
        /// <param name="objInvoiceReportInput"></param>
        /// <returns></returns>
        public PTXboInvoiceReportOutput GenerateInvoiceUsmailCoverLetter(PTXboInvoiceReportInput objInvoiceReportInput)
        {
            try
            {
                Logger.For(this).Invoice("GenerateInvoiceUsmailCoverLetter-API  reached " + ((object)objInvoiceReportInput).ToJson(false));
                var result = _invoiceCalculation.GenerateInvoiceUsmailCoverLetter(objInvoiceReportInput);
                Logger.For(this).Invoice("GenerateInvoiceUsmailCoverLetter-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GenerateInvoiceUsmailCoverLetter-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboInvoiceReportOutput GenerateInvoicereports(PTXboInvoiceReportInput objInvoiceReportInput)
        {
            try
            {
                Logger.For(this).Invoice("GenerateInvoicereports-API  reached " + ((object)objInvoiceReportInput).ToJson(false));
                var result = _invoiceCalculation.GenerateInvoicereports(objInvoiceReportInput) ;
                Logger.For(this).Invoice("GenerateInvoicereports-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GenerateInvoicereports-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }

        }


        public bool UpdateInvoiceDetails(PTXboInvoice objInvoice)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("UpdateInvoiceDetails-API  reached " + ((object)objInvoice).ToJson(false));
                parameters.Add("@InvoiceID", objInvoice.InvoiceID);
                parameters.Add("@InterestRateID", objInvoice.InterestRateID);
                parameters.Add("@InitialAssessedValue", objInvoice.InitialAssessedValue);
                parameters.Add("@FinalAssessedValue", objInvoice.FinalAssessedValue);
                parameters.Add("@PriorYearTaxRate", objInvoice.PriorYearTaxRate);
                parameters.Add("@ContingencyPercentage", objInvoice.ContingencyPercentage);
                parameters.Add("@ContingencyFee", objInvoice.ContingencyFee);
                parameters.Add("@FlatFee ", objInvoice.FlatFee);
                parameters.Add("@InvoiceAmount", objInvoice.InvoiceAmount);
                parameters.Add("@CompoundInterest", objInvoice.CompoundInterest);
                parameters.Add("@Reduction", objInvoice.Reduction);
                parameters.Add("@TotalEstimatedTaxSavings", objInvoice.TotalEstimatedTaxSavings);
                parameters.Add("@AmountPaid", objInvoice.AmountPaid);
                parameters.Add("@AmountAdjusted", objInvoice.AmountAdjusted);
                parameters.Add("@ApplicableInterest", objInvoice.ApplicableInterest);
                parameters.Add("@InterestPaid", objInvoice.InterestPaid);
                parameters.Add("@InterestAdjusted ", objInvoice.InterestAdjusted);
                parameters.Add("@AmountDue", objInvoice.AmountDue);
                
                var result = _connection.Execute(StoredProcedureNames.usp_InvoiceReportStatusUpdate, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("UpdateInvoiceDetails-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateInvoiceDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        /// <summary>
        /// Added by saravanans-to collect top 25 failed invoice corrq records for send email
        /// </summary>
        /// <returns></returns>
        public List<PTXboCorrQueue> GetFailedCorrQueueRecords()
        {
            Hashtable parameters = new Hashtable();
            List<PTXboCorrQueue> corrQueueAccountList = new List<PTXboCorrQueue>();
            try
            {

                Logger.For(this).Invoice("GetFailedCorrQueueRecords-API  reached " + ((object)DateTime.Now).ToJson(false));
                corrQueueAccountList = _connection.Select<PTXboCorrQueue>(StoredProcedureNames.USP_GetFailedCorrQueueRecords, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetFailedCorrQueueRecords-API  ends successfully ");
                return corrQueueAccountList;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetFailedCorrQueueRecords-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
        }

        /// <summary>
        /// Created by SaravananS. tfs id:61434
        /// </summary>
        /// <param name="servicePakageId"></param>
        /// <returns></returns>
        public PTXboClientCommunicationEmailDetails GetClientCommunicationEmailDetails(int servicePakageId)
        {
            Hashtable parameters = new Hashtable();
            PTXboClientCommunicationEmailDetails clientCommunicationEmailDetails = new PTXboClientCommunicationEmailDetails();
            try
            {
                parameters.Add("@ServicePakageId", servicePakageId);
                Logger.For(this).Invoice("GetClientCommunicationEmailDetails-API  reached.servicePakageId: "+ servicePakageId.ToJson(false));
                clientCommunicationEmailDetails = _connection.Select<PTXboClientCommunicationEmailDetails>(PTXdoStoredProcedureNames.USP_GetClientCommunicationEmailDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetClientCommunicationEmailDetails-API  ends successfully ");
                return clientCommunicationEmailDetails;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetClientCommunicationEmailDetails.servicePakageId: " + servicePakageId +"- API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        /// <summary>
        /// Created by SaravananS. tfs id:61434
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public PTXboInvoiceRegenerationDetails GetInvoiceRegenerationDetails(int invoiceId)
        {
            Hashtable parameters = new Hashtable();
            PTXboInvoiceRegenerationDetails invoiceRegenerationDetails = new PTXboInvoiceRegenerationDetails();
            try
            {
                parameters.Add("@Invoiceid", invoiceId);
                Logger.For(this).Invoice("GetInvoiceRegenerationDetails-API  reached.invoiceId: " + invoiceId.ToJson(false));
                var dtResult = _connection.SelectDataSet(PTXdoStoredProcedureNames.Usp_GetUnmappedInvoiceId, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).Tables[0];
                Logger.For(this).Invoice("GetInvoiceRegenerationDetails-API  ends successfully ");
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    invoiceRegenerationDetails.IsRegenerated = Convert.ToBoolean(dtResult.Rows[0][1]);
                    invoiceRegenerationDetails.OldInvoiceId = Convert.ToString(dtResult.Rows[0][0]);
                }
                return invoiceRegenerationDetails;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceRegenerationDetails.invoiceId: " + invoiceId + "- API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }




        public  bool GenerateLetterForSelectedInvoices(PTXboActiveCorrqRecordsInput corrqRecordsInput)
        {
            string serviceName = corrqRecordsInput.ServiceName;
            int currentUserid = corrqRecordsInput.UserId;
            string selectedInvoices = corrqRecordsInput.Invoices;
            Logger.For(this).Invoice("Reached Inside GenerateLetterForSelectedInvoices-is called . selectedInvoices: " + selectedInvoices);

            string errorMessage = string.Empty;
            bool IsSucess = false;
            List<PTXboCorrQueue> objCorrQueueInvoiceList;
            List<PTXboCorrQueue> objCorrQueueAccountListFromDB;
            StringBuilder processingStatus;

            try
            {
                PTxboActiveCorrqRecords ActiveRecords = new PTxboActiveCorrqRecords();
                //PTXboActiveCorrqRecordsInput corrqRecordsInput = new PTXboActiveCorrqRecordsInput()
                //{
                //    ServiceName=serviceName,Invoices=selectedInvoices,UserId=currentUserid
                //};
                Logger.For(this).Invoice("Calling GetActiveCorrQueueRecordsForSelectedInvoices-is called " + " selectedInvoices: " + selectedInvoices);
                ActiveRecords.Corrqueue = GetActiveCorrQueueRecordsForSelectedInvoices(corrqRecordsInput);
                Logger.For(this).Invoice("Received data from GetActiveCorrQueueRecordsForSelectedInvoices " + ((object)ActiveRecords.Corrqueue).ToJson(false));
                if(ActiveRecords.Corrqueue!=null && ActiveRecords.Corrqueue.Count>0)
                {
                    ActiveRecords.IsSuccess = true;
                }
                objCorrQueueAccountListFromDB = ActiveRecords.Corrqueue;
                errorMessage = ActiveRecords.ErrorMessage;
                IsSucess = ActiveRecords.IsSuccess;

                objCorrQueueInvoiceList = objCorrQueueAccountListFromDB.Where(a => a.CreatedBy == currentUserid).Distinct().ToList();

                Logger.For(this).Invoice("GenerateLetterForSelectedInvoices-selected corrq records objCorrQueueAccountList:" + ((object)objCorrQueueInvoiceList).ToJson(false));

                if ((objCorrQueueInvoiceList != null) && (objCorrQueueInvoiceList.Count > 0))
                {

                  string  reportPath = System.Configuration.ConfigurationManager.AppSettings["ReportGenExportPath"].ToString();

                    #region for loop 
                    foreach (PTXboCorrQueue QueueItem in objCorrQueueInvoiceList)
                    {
                        Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices for loop -QueueItem:" + ((object)QueueItem).ToJson(false));
                        //Added by saravanans.tfs id:54978
                        SendGridEventsAPI.SDK.ResponseEntity _response;
                        string messageId = string.Empty;
                        //Ends here.

                        //Added by SaravananS.tfs id:55433
                        string fileCabinetId = string.Empty;
                        double pwImageId = 0;
                        bool deliveryStatus;
                        //Ends here.
                        bool isEligibletoDeliver = true;//Added by SaravananS.tfs id:55498
                        string reportOutputPath = string.Empty;
                        processingStatus = new StringBuilder();
                        int corrQId = QueueItem.CorrQID;
                        int clientID = QueueItem.ClientID;
                        int linkFieldValue = QueueItem.LinkFieldValue;
                        int servicePackageId = QueueItem.ServicePackageID;
                        string accountList = QueueItem.AccountList;
                        int deliverMethodId = QueueItem.DeliveryMethodId;
                        bool IsCustomDelivery = QueueItem.IsCustomDelivery;
                        bool isAccountsChoosed = QueueItem.IsSpecificAccount;
                        int taxYear = QueueItem.TaxYear;
                        string ServicepackageName = QueueItem.ServicePackageName;
                        int Userid = QueueItem.CreatedBy;
                        string clientNumber = QueueItem.ClientNumber;

                        //Added by SaravananS.tfs id:55498
                        //bool isOutofTexas = PTXdsInvoice.CurrentInstance.IsOutOfTexas(linkFieldValue, out errorMessage);
                        bool isOutofTexas = IsOutOfTexas(linkFieldValue);
                        //bool isMultiyearInvoice = BusinessService.PTXbsInvoice.CurrentInstance.IsMultiyearInvoice(linkFieldValue, out errorMessage);
                        bool isMultiyearInvoice = IsMultiyear(linkFieldValue);
                        //Ends here.

                        //Added by SaravananS.tfs id:55498
                        if (isOutofTexas )
                        {
                            isEligibletoDeliver = IsILAccount(linkFieldValue);
                        }
                        else if (isMultiyearInvoice)
                        {
                            isEligibletoDeliver = IsHotelInvoice(linkFieldValue);
                        }
                        //Ends here.


                        Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices for loop -InvoiceFileGenPath:" + reportPath);

                        string AttachementLetter1 = string.Empty;
                        string AttachementLetter2 = string.Empty;
                        string outputNeopostpath = string.Empty;
                        int client = Convert.ToInt32(clientID);
                        
                        processingStatus.Append("\n Invoice Letter Deliver process Started.linkFieldValue:" + linkFieldValue.ToString() + ",corrQId:" + corrQId.ToString() + ",isOutofTexas:" + isOutofTexas.ToString() + ",isMultiyearInvoice:" + isMultiyearInvoice.ToString() + ",isEligibletoDeliver:" + isEligibletoDeliver.ToString());

                        Logger.For(this).Invoice(" GenerateLetterForSelectedInvoices Account List from SP: " + accountList + ",selectedInvoices:" + selectedInvoices);

                        List<int> choosedAccounts = new List<int>();

                        if (!(accountList == null || accountList.Length == 0))
                        {
                            choosedAccounts = accountList == null ? null : accountList.Split(',').ToList().ConvertAll(s => int.Parse(s));
                        }

                        Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices for loop -choosedAccounts:" + ((object)choosedAccounts).ToJson(false));
                        try
                        {
                            processingStatus.Append(" \n Letter Creation started.accountList:" + accountList);

                            PTXboInvoice objInvoice = GetInvoiceDetailsById(linkFieldValue);


                            if (objInvoice == null)
                            {
                                Logger.For(this).Invoice("GetInvoiceReportDetails returns empty values for invoiceid." + Convert.ToString(linkFieldValue) + ",selectedInvoices:" + selectedInvoices);
                                processingStatus.Append(" \n Letter Creation started.objInvoice is empty.");
                            }

                            Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices -Before inserting line item:GetInvoiceReportDetails:" + ((object)objInvoice).ToJson(false));

                            string FileLocation = GetLatestInvoiceFile(linkFieldValue);

                            #region  Invoice copy already exists
                            if (!string.IsNullOrEmpty(FileLocation))
                            {
                                if (System.IO.File.Exists(FileLocation))
                                {
                                    reportOutputPath = FileLocation;
                                    IsSucess = true;
                                    Logger.For(this).Invoice("GetInvoiceReportDetails Existing file path " + reportOutputPath + " available for invoiceid." + Convert.ToString(linkFieldValue) + ",selectedInvoices:" + selectedInvoices);
                                    processingStatus.Append(" \n Previewed Invoice copy is available.FileLocation:" + FileLocation);
                                }
                                else
                                {
                                    PTXboInvoiceReportInput objInvoiceReportInput = new PTXboInvoiceReportInput()
                                    {
                                        Groupid = objInvoice.GroupId,
                                        LinkFieldValue = linkFieldValue,
                                        ClientId = clientID,
                                        ServicePackageId = servicePackageId,
                                        Taxyear = taxYear,
                                        InvoiceTypeId = objInvoice.InvoiceTypeId,// Enumerators.PTXenumInvoiceType.Standard.GetId(),
                                        //InvoiceAdjustmentRequestId = InvoiceAdjustmentRequestId,
                                        ReportExportPath = reportPath,
                                        //IsInvoiceDefects = isInvoiceDefects,
                                        IsOutOfTexas = isOutofTexas,
                                        IsOTEntryscreen = false,
                                        IsMultiyear = isMultiyearInvoice
                                    };
                                    Logger.For(this).Invoice("GenerateLetterForSelectedInvoices :: new invoice copy because invoice copy not available in the folder." + Convert.ToString(linkFieldValue) + " - " + reportOutputPath + ",selectedInvoices:" + selectedInvoices);
                                    var invoiceResult = GenerateInvoicereports(objInvoiceReportInput);
                                    reportOutputPath = invoiceResult.ReportOutputPath;
                                    processingStatus.Append(" \n new invoice copy because invoice copy not available in the folder.");

                                    if (!CheckInvoicefilepath(reportOutputPath))
                                    {
                                        UpdateInvoiceFiles(linkFieldValue, corrQId, reportOutputPath, Userid);
                                    }
                                    else
                                    {
                                        reportOutputPath = string.Empty;
                                        IsSucess = false;
                                        errorMessage = "Invoice file path already exists.Invoiceid : " + Convert.ToString(linkFieldValue);
                                    }
                                }
                            }
                            #endregion  Invoice copy already exists

                            #region new Invoice copy generation
                            else
                            {
                                processingStatus.Append(" \n Previewed Invoice copy is not available.");

                                PTXboInvoiceReportInput objInvoiceReportInput = new PTXboInvoiceReportInput()
                                {
                                    Groupid = objInvoice.GroupId,
                                    LinkFieldValue = linkFieldValue,
                                    ClientId = clientID,
                                    ServicePackageId = servicePackageId,
                                    Taxyear = taxYear,
                                    InvoiceTypeId = 1,
                                    //InvoiceAdjustmentRequestId = InvoiceAdjustmentRequestId,
                                    ReportExportPath = reportPath,
                                    //IsInvoiceDefects = isInvoiceDefects,
                                    IsOutOfTexas = isOutofTexas,
                                    IsOTEntryscreen = false,
                                    IsMultiyear = isMultiyearInvoice
                                };

                                Logger.For(this).Invoice("GenerateLetterForSelectedInvoices :: new invoice copy generation." + Convert.ToString(linkFieldValue) + "-" + reportOutputPath + ",selectedInvoices:" + selectedInvoices);
                                var invoiceResult = GenerateInvoicereports(objInvoiceReportInput);
                                reportOutputPath=invoiceResult.ReportOutputPath;
                                Logger.For(this).Invoice("GenerateLetterForSelectedInvoices :: new generated copy." + Convert.ToString(linkFieldValue) + "-" + reportOutputPath + ",selectedInvoices:" + selectedInvoices);

                                if (!CheckInvoicefilepath(reportOutputPath))
                                {
                                    SubmitInvoiceFiles(linkFieldValue, corrQId, reportOutputPath, Userid);
                                }
                                else
                                {
                                    reportOutputPath = string.Empty;
                                    IsSucess = false;
                                    errorMessage = "Invoice file path already exists.Invoiceid : " + Convert.ToString(linkFieldValue);
                                }

                            }
                            #endregion new Invoice copy generation

                            Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices.isEligibletoDeliver:" + isEligibletoDeliver + ",reportOutputPath:" + Convert.ToString(linkFieldValue) + "-" + reportOutputPath + ",selectedInvoices:" + selectedInvoices);

                            if (!string.IsNullOrEmpty(reportOutputPath.Trim()) && IsSucess && isEligibletoDeliver)
                            {
                                processingStatus.Append(" \n Invoice copy created .reportOutputPath:" + reportOutputPath);
                                AttachementLetter1 = reportOutputPath;
                                processingStatus.Append(" \n Deliver to client process is started.");

                                Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices:reportOutputPath is not empty." + Convert.ToString(linkFieldValue) + "-" + reportOutputPath + ",selectedInvoices:" + selectedInvoices);
                                #region Send to Client
                                if (QueueItem.SentToClient)
                                {
                                    processingStatus.Append(" \n SentToClient.");
                                    Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices:send to client:true." + Convert.ToString(linkFieldValue) + "-" + reportOutputPath + ",selectedInvoices:" + selectedInvoices);
                                    if (deliverMethodId == 1)  //to test invoice email we can use 3 here
                                    {
                                        processingStatus.Append(" \n Delivery method:Email.");
                                        _response = new SendGridEventsAPI.SDK.ResponseEntity();
                                        Logger.For(this).Invoice(" GenerateLetterForSelectedInvoices Sending email to the client: " + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                        if (IsCustomDelivery == true)
                                        {
                                            processingStatus.Append(" \n IsCustomDelivery:true.");
                                            Logger.For(this).Invoice(" GenerateLetterForSelectedInvoices-deliverymethod :email,custom delivery:true  clientid:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                            deliveryStatus = SendCustomDeliveryInvoiceEMailToClient(client, linkFieldValue, AttachementLetter1, AttachementLetter2, out errorMessage, out _response, objInvoice.GroupId, corrQId);
                                        }
                                        else
                                        {
                                            processingStatus.Append(" \n IsCustomDelivery:false.");
                                            Logger.For(this).Invoice(" GenerateLetterForSelectedInvoices-deliverymethod :email,custom delivery:false  clientid:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                            deliveryStatus = SendInvoiceEMail(client, corrQId, linkFieldValue, AttachementLetter1, AttachementLetter2, out errorMessage, out _response, objInvoice.GroupId);
                                        }

                                        //Added by saravanans.tfs id:54978
                                        //processingStatus.Append(_response.ToString());
                                        messageId = _response.X_Message_ID.ToString();
                                        processingStatus.Append(" \n messageId:" + messageId + ",deliveryStatus:" + deliveryStatus.ToString());
                                        //Ends here.

                                        Logger.For(this).Invoice("GenerateLetterForSelectedInvoices email was sent to the client." + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                        //Added by SaravananS.tfs id:55433--Moving invoicecopy to paperwise
                                        if (deliveryStatus)
                                        {
                                            Logger.For(this).Invoice("started UpdateInvoiceCorrqFilePath.corrQId: " + Convert.ToString(corrQId) + ",client:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                            UpdateInvoiceCorrqFilePath(servicePackageId, corrQId, AttachementLetter1); //Added by SaravananS. tfs id:56254 
                                            Logger.For(this).Invoice("ended UpdateInvoiceCorrqFilePath.corrQId: " + Convert.ToString(corrQId) + ",client:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                            // SendDocumentsToPaperWise(clientNumber, taxYear, AttachementLetter1, "Invoice", out pwImageId, out fileCabinetId, out errorMessage);
                                            //Logger.For(this).Invoice("GenerateLetterForSelectedInvoices email was sent to the client " + Convert.ToString(client) + "-" + AttachementLetter1 + ".pwImageId:" + Convert.ToString(pwImageId) + ".fileCabinetId:" + fileCabinetId + " selectedInvoices " + selectedInvoices);
                                        }
                                        //Ends here.
                                    }
                                    #region Usmail
                                    else if (deliverMethodId ==3)
                                    {
                                        processingStatus.Append(" \n deliverMethodId:USMail.");
                                        //Added by saravanans.tfs id:55021
                                        string reportExportPath = System.Configuration.ConfigurationManager.AppSettings["InvoiceFileGenPath"].ToString();
                                        string invoiceUsmailcoverLetter = string.Empty;
                                        //InvoiceReport objReportData = new InvoiceReport();

                                        processingStatus.Append(" \n Invoice cover letter generation is started.");

                                        PTXboInvoiceReportInput objInvoiceReportInput = new PTXboInvoiceReportInput()
                                        {
                                            Groupid = objInvoice.GroupId,
                                            LinkFieldValue = linkFieldValue,
                                            ClientId = clientID,
                                            ServicePackageId = servicePackageId,
                                            Taxyear = taxYear,
                                            InvoiceTypeId = objInvoice.InvoiceTypeId,
                                            ReportExportPath = reportExportPath,

                                        };

                                        Logger.For(this).Invoice("GenerateLetterForSelectedInvoices :: GenerateInvoiceUsmailCoverLetter generation." + objInvoiceReportInput);
                                        var reportResult = GenerateInvoiceUsmailCoverLetter(objInvoiceReportInput);
                                        invoiceUsmailcoverLetter = reportResult.ReportOutputPath;
                                        Logger.For(this).Invoice("GenerateLetterForSelectedInvoices :: GenerateInvoiceUsmailCoverLetter generated ." + objInvoiceReportInput);

                                        //objReportData.GenerateInvoiceUsmailCoverLetter(Convert.ToString(linkFieldValue), objInvoice.GroupId, QueueItem.ClientID, QueueItem.TaxYear, reportExportPath, invoiceTypeId, out invoiceUsmailcoverLetter);

                                        string usmailFilePath = string.Empty;
                                        string mergedInvoiceUsMail = reportPath + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff") + ".pdf";
                                        Logger.For(this).Invoice("mergedInvoiceUsMail final path." + mergedInvoiceUsMail + ",selectedInvoices:" + selectedInvoices);
                                        string[] sourceDocuments = new string[] { invoiceUsmailcoverLetter, reportOutputPath };
                                        if (invoiceUsmailcoverLetter != "No reports found")
                                        {
                                            PDFUtilities.MergePdfDocument(sourceDocuments, mergedInvoiceUsMail, out errorMessage);
                                            MoveInvoiceFileToNeoPost(servicePackageId, mergedInvoiceUsMail, out outputNeopostpath, out errorMessage);//Added by saravanans-tfs:48560
                                            usmailFilePath = mergedInvoiceUsMail;
                                            processingStatus.Append(" \n Invoice cover letter generation is completed.");
                                        }
                                        else
                                        {
                                            processingStatus.Append(" \n Invoice cover letter generation is not completed.");
                                            Logger.For(this).Invoice("Invoicecover letter is not generated   to the client." + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                            MoveInvoiceFileToNeoPost(servicePackageId, AttachementLetter1, out outputNeopostpath, out errorMessage);//Added by saravanans-tfs:48560
                                            usmailFilePath = AttachementLetter1;
                                        }
                                        //Ends here

                                        AttachementLetter1 = outputNeopostpath;
                                        Logger.For(this).Invoice("GenerateLetterForSelectedInvoices usmail was sent to the client." + Convert.ToString(client) + "-" + AttachementLetter1 + "Neopost path:" + outputNeopostpath + ",selectedInvoices:" + selectedInvoices);

                                        //Added by SaravananS.tfs id:56254
                                        Logger.For(this).Invoice("started UpdateInvoiceCorrqFilePath.corrQId: " + corrQId + ",client:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                        UpdateInvoiceCorrqFilePath(servicePackageId, corrQId, usmailFilePath);
                                        Logger.For(this).Invoice("ended UpdateInvoiceCorrqFilePath.corrQId: " + corrQId + ",client:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                        //Ends here.

                                    }
                                    #endregion usmail
                                    else if (deliverMethodId ==2)
                                    {
                                        Logger.For(this).Invoice("GenerateLetterForSelectedInvoices sending Fax  to the client " + Convert.ToString(client) + "-" + AttachementLetter1);
                                        SendFaxToClient(client, corrQId, AttachementLetter1, AttachementLetter2, out errorMessage);
                                        Logger.For(this).Invoice("GenerateLetterForSelectedInvoices sending Fax  to the client " + Convert.ToString(client) + "-" + AttachementLetter1);

                                        //Added by SaravananS.tfs id:56254
                                        Logger.For(this).Invoice("started UpdateInvoiceCorrqFilePath.corrQId: " + corrQId + ",client:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                        UpdateInvoiceCorrqFilePath(62, corrQId, AttachementLetter1);
                                        Logger.For(this).Invoice("ended UpdateInvoiceCorrqFilePath.corrQId: " + corrQId + ",client:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                        //Ends here.

                                        
                                    }

                                    PTXboUpdateLetterProcessStatusInput letterProcessStatus = new PTXboUpdateLetterProcessStatusInput()
                                    {
                                        CorrQID = corrQId,
                                        Notes = Enumerators.PTXenumQueueProcessingStatus.Completed.ToString() + processingStatus.ToString(),
                                        ServicePackageId = QueueItem.ServicePackageID,
                                        MessageId = messageId,
                                        Status = 2,
                                        FileCabinetId = fileCabinetId,
                                        ImageId = pwImageId
                                    };

                                    Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices-UpdateLetterProcessStatus -ProcessingStatus.Completed,deliverMethodId-" + Convert.ToString(deliverMethodId) + ",linkFieldValue:" + Convert.ToString(linkFieldValue) + "ProcessingStatus-" + Convert.ToString(2) + ",selectedInvoices:" + selectedInvoices);
                                    UpdateLetterProcessStatus(letterProcessStatus);
                                    Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices -completed UpdateLetterProcessStatus -:deliverMethodId-" + Convert.ToString(deliverMethodId) + ",linkFieldValue:" + Convert.ToString(linkFieldValue) + "ProcessingStatus-" + Convert.ToString(2) + ",selectedInvoices:" + selectedInvoices);
                                    processingStatus.Append(" \n Deliver to client is completed");
                                }
                                #endregion send to client

                                #region send to Agent
                                if (QueueItem.SentToContactAgent == true || QueueItem.SentToPrimaryAgent == true || QueueItem.SentToSalesAgent == true)
                                {
                                    processingStatus.Append(" \n Sent to Agent Started");
                                    Logger.For(this).Invoice("GenerateLetterForSelectedInvoices SentToContactAgent is started " + Convert.ToString(corrQId) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                    _response = new SendGridEventsAPI.SDK.ResponseEntity();
                                    SendEmailToAgent(corrQId, AttachementLetter1, out errorMessage, out _response, QueueItem.ServicePackageID);
                                    Logger.For(this).Invoice("GenerateLetterForSelectedInvoices SentToContactAgent was success " + Convert.ToString(corrQId) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                    processingStatus.Append(" \n Sent to Agent Completed");

                                    //Added by saravanans.tfs id:54978
                                    //processingStatus.Append(_response.ToString());
                                    messageId = _response.X_Message_ID.ToString();
                                    processingStatus.Append(" \n messageId:" + messageId);
                                    //Ends here.
                                   
                                    //Added by SaravananS.tfs id:56254
                                    Logger.For(this).Invoice("started UpdateInvoiceCorrqFilePath.corrQId: " + Convert.ToString(corrQId) + ",client:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                    UpdateInvoiceCorrqFilePath(62, corrQId, AttachementLetter1);
                                    Logger.For(this).Invoice("ended UpdateInvoiceCorrqFilePath.corrQId: " + Convert.ToString(corrQId) + ",client:" + Convert.ToString(client) + "-" + AttachementLetter1 + ",selectedInvoices:" + selectedInvoices);
                                    //Ends here.

                                    Logger.For(this).Invoice(" GenerateLetterForSelectedInvoices-UpdateLetterProcessStatus -ProcessingStatus.Completed,deliverMethodId-" + Convert.ToString(deliverMethodId) + ",linkFieldValue:" + Convert.ToString(linkFieldValue) + ",ProcessingStatus: Completed,selectedInvoices:" + selectedInvoices);
                                    PTXboUpdateLetterProcessStatusInput letterProcessStatus = new PTXboUpdateLetterProcessStatusInput()
                                    {
                                        CorrQID = corrQId,
                                        Notes = Enumerators.PTXenumQueueProcessingStatus.Completed.ToString() + processingStatus.ToString(),
                                        ServicePackageId = QueueItem.ServicePackageID,
                                        MessageId = messageId,
                                        Status = 2,
                                        FileCabinetId= fileCabinetId,
                                        ImageId=pwImageId
                                    };

                                    UpdateLetterProcessStatus(letterProcessStatus);

                                    Logger.For(this).Invoice(" GenerateLetterForSelectedInvoices-UpdateLetterProcessStatus -GenerateDefaultmethodForInvoice:deliverMethodId-" + Convert.ToString(deliverMethodId) + ",linkFieldValue:" + Convert.ToString(linkFieldValue) + ",ProcessingStatus-Completed,selectedInvoices:" + selectedInvoices);
                                    processingStatus.Append(" \n Deliver to Agent is completed");
                                }
                                #endregion send to Agent

                                Logger.For(this).Invoice("Inside GenerateLetterForSelectedInvoices -deliverMethodId-" + Convert.ToString(deliverMethodId) + "linkFieldValue:" + Convert.ToString(linkFieldValue) + "ProcessingStatus-Completed,selectedInvoices:" + selectedInvoices);

                            }
                            else
                            {
                                //Added by SaravananS.tfs id:55498
                                if (!isEligibletoDeliver)
                                {
                                    errorMessage = errorMessage + "out of texas invoice.Not a hotel luc account.";
                                }
                                //Ends here.

                                Logger.For(this).Invoice("Error GenerateLetterForSelectedInvoices while sending email ProcessingStatus.failed-UpdateLetterProcessStatus ::errorMessage :" + errorMessage + " reportoutputpath is empty" + Convert.ToString(deliverMethodId) + "linkFieldValue:" + Convert.ToString(linkFieldValue) + ",ProcessingStatus-Completed,selectedInvoices:" + selectedInvoices);
                                PTXboUpdateLetterProcessStatusInput letterProcessStatus = new PTXboUpdateLetterProcessStatusInput()
                                {
                                    CorrQID = corrQId,
                                    Notes = "errorMessage:" + errorMessage + " isEligibletoDeliver - " + isEligibletoDeliver + "or reportoutputpath is empty" + processingStatus.ToString(),
                                    ServicePackageId = QueueItem.ServicePackageID,
                                    MessageId = messageId,
                                    Status = 3
                                };
                                UpdateLetterProcessStatus(letterProcessStatus);
                            }

                            Logger.For(this).Invoice("GenerateLetterForSelectedInvoices- ends successfully invoiceid=" + Convert.ToString(linkFieldValue) + " corrqid=" + Convert.ToString(corrQId) + " selectedInvoices:" + selectedInvoices);
                        }

                        catch (Exception ex)
                        {
                            errorMessage = errorMessage + ex.InnerException;
                            Logger.For(this).Invoice("Error GenerateLetterForSelectedInvoices while sending invoices errorMessage:" + errorMessage + " selectedInvoices:" + selectedInvoices + " ex:" + ex);
                            PTXboUpdateLetterProcessStatusInput letterProcessStatus = new PTXboUpdateLetterProcessStatusInput()
                            {
                                CorrQID = corrQId,
                                Notes = errorMessage + ex.Message + processingStatus.ToString(),
                                ServicePackageId = QueueItem.ServicePackageID,
                                MessageId = messageId,
                                Status = 3
                            };
                            UpdateLetterProcessStatus(letterProcessStatus);

                            //log = new LoggingHelper("Spartaxx", "ERROR");
                            //log.WriteToEventLog(ex.ToString(), "GenerateLetterForSelectedInvoices error" + " selectedInvoices " + selectedInvoices);
                            Logger.For(this).Error("Error GenerateLetterForSelectedInvoices while sending invoices errorMessage:" + errorMessage + " selectedInvoices:" + selectedInvoices + " ex:" + ex);
                        }

                        reportOutputPath = string.Empty;
                        AttachementLetter1 = string.Empty;
                        AttachementLetter2 = string.Empty;
                        outputNeopostpath = string.Empty;
                    }

                    #endregion for loop
                }
                else
                {
                    Logger.For(this).Invoice("GenerateLetterForSelectedInvoices- failed because  not available in corrque." + " selectedInvoices: " + selectedInvoices);
                    Logger.For(this).Error("GenerateLetterForSelectedInvoices- failed because not available in corrque." + " selectedInvoices: " + selectedInvoices);
                }


                IsSucess = true;
                
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GenerateLetterForSelectedInvoices- error." + ex + ",selectedInvoices: " + selectedInvoices);
                errorMessage = ex.Message;
                IsSucess = false;
                
                Logger.For(this).Error("GenerateLetterForSelectedInvoices- error." + ex + ",selectedInvoices: " + selectedInvoices);
            }

            return IsSucess;
        }


        public bool SendFaxToClient(int ClientID, int corrQId, string AttachementLetter1, string AttachementLetter2, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool isSucess = false;
            try
            {
                //DataTable clientFaxDetails;
                PTXboClientFaxDetails objClientFaxDetails;
                //DataTable OconnorCompanyFaxDetails;
                PTXboCompanydetails objOconnorCompanyDetails;
                string clientName;
                string clientFaxNumber;
                string clientCompanyName;

                GetClientFaxDetails(ClientID, corrQId, out objClientFaxDetails, out errorMessage);

                if (objClientFaxDetails != null)
                {


                    clientName = objClientFaxDetails.ClientName;
                    if (objClientFaxDetails.IsCustomDelivery)
                    {
                        clientFaxNumber = objClientFaxDetails.CustomFax;
                    }
                    else if (objClientFaxDetails.IsItFax)
                    {
                        clientFaxNumber = objClientFaxDetails.FaxNumber;
                    }
                    else
                    {
                        return false;
                    }
                    clientCompanyName = objClientFaxDetails.CompanyName;

                    GetOconnorCompanyDetails(out objOconnorCompanyDetails, out errorMessage);

                    FaxEntity objFaxEntity = new FaxEntity();


                    objFaxEntity.faxAddress = objOconnorCompanyDetails.CompanyAddress;
                    objFaxEntity.faxCity = objOconnorCompanyDetails.CityName;
                    objFaxEntity.faxPhone = objOconnorCompanyDetails.Phone;
                    objFaxEntity.faxRecCompany = clientCompanyName;
                    objFaxEntity.faxRecipientName = clientName;
                    objFaxEntity.faxSenderName = objOconnorCompanyDetails.CompanyName;
                    objFaxEntity.faxWebsite = objOconnorCompanyDetails.Website;
                    objFaxEntity.faxZipCode = objOconnorCompanyDetails.Zip;
                    //objFaxEntity.faxMessage = "Test";
                    //objFaxEntity.faxRecipient = "713";// clientFaxNumber;                    
                    //objFaxEntity.faxSender = "713";// OconnorCompanyFaxDetails.Rows[0]["Fax"].ToString();                    
                    objFaxEntity.faxSubject = "";

                    List<string> lstAtt = new List<string>();
                    lstAtt.Add(AttachementLetter1);
                    if (AttachementLetter2 != string.Empty) lstAtt.Add(AttachementLetter2);
                    objFaxEntity.attachmentFiles = lstAtt;

                    Fax objFax = new Fax();
                    objFax.FaxReport(objFaxEntity, out errorMessage);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isSucess = false;
                Logger.For(this).Error(ex);
                
                throw ex;
            }
            return isSucess;
        }


        /// <summary>
        /// added by saravanans-invoice email
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="invoiceid"></param>
        /// <param name="AttachmentFile1"></param>
        /// <param name="AttachmentFile2"></param>
        /// <param name="errorMessage"></param>
        private bool SendInvoiceEMail(int clientID, int corrqid, int invoiceid, string AttachmentFile1, string AttachmentFile2, out string errorMessage, out SendGridEventsAPI.SDK.ResponseEntity _response, int groupId = 0)
        {
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("Inside SendInvoiceEMail clientid:" + clientID + " invoiceid: " + (invoiceid + "AttachmentFile1: " + AttachmentFile1).ToJson(false));
                string emailAddress = string.Empty;
                string alternateEmailAddress = string.Empty;
                string clientFirstName = string.Empty;
                string companyType = string.Empty;
                string AdditionalEmail = string.Empty;
                EmailEntity objEmailEntity;

                PTXboGetClientEmailInput objGetClientEmail = new PTXboGetClientEmailInput()
                {
                    ClientId = clientID,
                    CorrQId = corrqid,
                    GroupId = groupId

                };
                
               var result = GetClientEmailInvoice(objGetClientEmail);

                if (result != null)
                {
                    if (result.IsCustomDelivery)
                    {
                        emailAddress = result.CustomEmail.Trim();
                    }
                    else
                    {
                        emailAddress = result.Email.Trim();
                    }

                    alternateEmailAddress = result.AlternateEmail.Trim();
                    AdditionalEmail = result.AdditionalEmail.Trim();
                    clientFirstName = result.FirstName.Trim();
                    companyType = result.CompanyType.Trim();
                }

                GenerateEmailBodyforInvoice(emailAddress, alternateEmailAddress, clientFirstName, companyType, AttachmentFile1, AttachmentFile2, out objEmailEntity, AdditionalEmail, invoiceid);
                
                //Added by SaravananS.tfs id:54978
                _response = new SendGridEventsAPI.SDK.ResponseEntity();
                bool issuccess = SendEmail_SendGrid(objEmailEntity, out _response, out errorMessage);
                //Ends here.

                Logger.For(this).Invoice("Finished SendInvoiceEMail clientid:" + clientID + " invoiceid: " + (invoiceid + "AttachmentFile1: " + AttachmentFile1).ToJson(false));
                return issuccess;
            }
            catch (Exception ex)
            {
                // Logger.For(this).Error(ex);
                errorMessage = errorMessage + ex.ToString();
                // log.WriteToEventLog(ex.ToString(), "error");
                Logger.For(this).Invoice("error SendInvoiceEMail invoiceid:" + invoiceid.ToString() + ",AttachmentFile1:" + AttachmentFile1 + ",errorMessage ::" + errorMessage + ",error:" + ex.Message);
                Logger.For(this).Error("error SendInvoiceEMail invoiceid:" + invoiceid.ToString() + ",AttachmentFile1:" + AttachmentFile1 + ",errorMessage ::" + errorMessage + ",error:" + ex.Message);
                throw ex;
            }
        }


        public bool GenerateEmailBodyforInvoice(string emailAddress, string alternateEmailAddress, string clientFirstName, string companyType, string AttachmentFile1, string AttachmentFile2, out EmailEntity objEmailEntity, string AdditionalEmail = "", int invoiceid = 0)
        {
            bool isSucess = true;
            string CCEmailId = string.Empty;
            StringBuilder strHistory = new StringBuilder();
            try
            {

                strHistory.Append("\n Inside GenerateEmailBodyforInvoice Inputs ::emailAddress:" + emailAddress + ",alternateEmailAddress:" + alternateEmailAddress + ",clientFirstName:" + clientFirstName + ",companyType:" + companyType + ",AttachmentFile1:" + AttachmentFile1 + ",AttachmentFile2:" + AttachmentFile2 + ",AdditionalEmail:" + AdditionalEmail);
                Logger.For(this).Invoice("Inside GenerateEmailBodyforInvoice Inputs ::emailAddress:" + emailAddress + ",alternateEmailAddress:" + alternateEmailAddress + ",clientFirstName:" + clientFirstName + ",companyType:" + companyType + ",AttachmentFile1:" + AttachmentFile1 + ",AttachmentFile2:" + AttachmentFile2 + ",AdditionalEmail:" + AdditionalEmail);

                if (!string.IsNullOrEmpty(AdditionalEmail))
                {
                    CCEmailId = alternateEmailAddress + ";" + AdditionalEmail.Trim();
                    // CCEmailId = "saravanans@parkisolutions.com";//added by saravanans...for testing
                }
                else
                {
                    CCEmailId = alternateEmailAddress.Trim();
                    // CCEmailId = "saravanans@parkisolutions.com";//added by saravanans...for testing
                }

                //Added to remove duplicates email.tfs id:56116
                var ccEmail = CCEmailId.ToLower().Split(';').ToList().Distinct();
                var result = string.Join("; ", ccEmail.ToArray());
                CCEmailId = result;
                //Ends here.
                strHistory.Append("\n CCEmailId:" + CCEmailId);
                Logger.For(this).Invoice("GenerateEmailBodyforInvoice CCEmailId:" + CCEmailId);
                string xmlReportFilePath = Path.Combine(Path.GetDirectoryName(typeof(ReportData).Assembly.CodeBase), "XML", "Invoice Mail.xml").Replace("file:\\", "");
                XDocument doc = XDocument.Load(xmlReportFilePath);

                XmlDocument docEmailSettings = new XmlDocument();

                docEmailSettings.Load(xmlReportFilePath);

                //Added by SaravananS.tfs id:55215
                string oldInvoiceid = "0";
                bool isRegenerated = false;
                string errorMessage = string.Empty;
                //PTXdsWindowsServiceCommon.CurrentInstance.GetInvoiceRegenerationDetails(invoiceid, out isRegenerated, out oldInvoiceid, out errorMessage);
                var invoiceRegenerationDetails = GetInvoiceRegenerationDetails(invoiceid);
                isRegenerated = invoiceRegenerationDetails.IsRegenerated;
                oldInvoiceid = invoiceRegenerationDetails.OldInvoiceId;

                string subject = string.Empty;
                string message = string.Empty;
                string Sincerely = string.Empty;
                string fromEmail = string.Empty;
                string bcc = string.Empty;
                var salutation = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/Salutation").InnerText);
                //Added by SaravananS.tfs id:55498
                bool isOutofTexas = IsOutOfTexas(invoiceid);

                Logger.For(this).Invoice("GenerateEmailBodyforInvoice isOutofTexas:" + isOutofTexas + ",isRegenerated:" + isRegenerated);
                //Ends here.
                strHistory.Append("\n isRegenerated:" + isRegenerated.ToString() + ",isOutofTexas:" + isOutofTexas.ToString() + ",oldInvoiceid:" + oldInvoiceid.ToString());

                //Added by SaravananS.tfs id:56116
                
              var objClientCommunicationEmailDetails=  GetClientCommunicationEmailDetails(62);

                if (string.IsNullOrEmpty(objClientCommunicationEmailDetails.FromMail))
                {
                    strHistory.Append("fromEmail is empty" + Convert.ToString(invoiceid));
                }

                if (string.IsNullOrEmpty(emailAddress.Trim()))
                {
                    strHistory.Append("emailRecipient is empty" + Convert.ToString(invoiceid));
                }

                strHistory.Append("objClientCommunicationEmailDetails.FromMail:" + objClientCommunicationEmailDetails.FromMail + ",objClientCommunicationEmailDetails.BCCMailList:" + objClientCommunicationEmailDetails.BCCMailList);

                Logger.For(this).Invoice("objClientCommunicationEmailDetails:" + objClientCommunicationEmailDetails + ",invoiceid:" + invoiceid.ToString());

                if (objClientCommunicationEmailDetails == null)
                {
                    Logger.For(this).Invoice("GenerateEmailBodyforInvoice .objClientCommunicationEmailDetails is empty.invoiceid:" + invoiceid.ToString());
                    strHistory.Append("\n .objClientCommunicationEmailDetails is empty." + invoiceid.ToString());
                }
                //Ends here.

                if (isOutofTexas)
                {
                    subject = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/subject").InnerText);
                    message = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/bodyOutofTexas").InnerText);
                    Sincerely = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/OutofTexasSincerely").InnerText);
                    //fromEmail = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/fromOutofTexas").InnerText);
                    fromEmail = objClientCommunicationEmailDetails.FromMail;
                    //bcc = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/bcc").InnerText);
                    bcc = objClientCommunicationEmailDetails.BCCMailList;

                    strHistory.Append("\n OutofTexas Invoice.subject:" + subject + ",Sincerely:" + Sincerely + ",fromEmail:" + fromEmail + ",bcc:" + bcc);
                }
                else if (isRegenerated)
                {
                    subject = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/subjectRegeneration").InnerText);
                    message = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/bodyRegeneration").InnerText);
                    message = message.Replace("oldinvoiceid", oldInvoiceid.ToString());
                    Sincerely = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/CommercialSincerely").InnerText);
                    //fromEmail = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/from").InnerText);
                    fromEmail = objClientCommunicationEmailDetails.FromMail;
                    //bcc = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/bcc").InnerText);
                    bcc = objClientCommunicationEmailDetails.BCCMailList;
                    // strHistory.Append("\n regeneration invoice .subject:" + subject + ",message:" + message + ",Sincerely:" + Sincerely + ",fromEmail:" + fromEmail + ",bcc:" + bcc);
                    strHistory.Append("\n regeneration invoice .subject:" + subject + ",Sincerely:" + Sincerely + ",fromEmail:" + fromEmail + ",bcc:" + bcc);
                }
                else
                {
                    subject = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/subject").InnerText);
                    message = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/body").InnerText);
                    Sincerely = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/CommercialSincerely").InnerText);
                    //fromEmail = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/from").InnerText);
                    fromEmail = objClientCommunicationEmailDetails.FromMail;
                    //bcc = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/bcc").InnerText);
                    bcc = objClientCommunicationEmailDetails.BCCMailList;
                    strHistory.Append("\n Texas invoice.subject:" + subject + ",Sincerely:" + Sincerely + ",fromEmail:" + fromEmail + ",bcc:" + bcc);
                }
                //Ends here.


                if (companyType == "Residential") //!isOutofTexas && 
                {
                    subject = subject.Replace("O'Connor", "O'Connor & Associates");
                    message = message.Replace("O'Connor", "O'Connor & Associates");
                    Sincerely = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/ResidetialSincerely").InnerText);
                    strHistory.Append("\n Residential invoice." + subject + ",Sincerely:" + Sincerely);
                }

                string emailBody = salutation.ToString() + clientFirstName + "\r\n <br />".Replace("\n", "<br />") + message.ToString() + "\r\n <br />" + Sincerely;//.Replace("\n", "<br />"); 


                objEmailEntity = new EmailEntity();
                StringBuilder strMessage = new StringBuilder();
                List<string> lstAttachmentFiles = new List<string>();
                objEmailEntity.emailSender = fromEmail.Trim();
                objEmailEntity.emailRecipient = (emailAddress.Trim());
                objEmailEntity.emailCc = CCEmailId.Trim();
                objEmailEntity.emailBcc = bcc.Trim();
                // objEmailEntity.emailBcc = "";//added by saravanans...for testing

                objEmailEntity.emailSubject = subject.ToString();

                objEmailEntity.emailMessage = emailBody;
                objEmailEntity.isBodyHtml = true;

                if (!string.IsNullOrEmpty(AttachmentFile1))
                    lstAttachmentFiles.Add(AttachmentFile1);
                if (!string.IsNullOrEmpty(AttachmentFile2))
                    lstAttachmentFiles.Add(AttachmentFile2);
                objEmailEntity.attachmentFiles = lstAttachmentFiles;

                Logger.For(this).Invoice(strHistory.ToString());
                Logger.For(this).Invoice("Finish GenerateEmailBodyforInvoice.strHistory:" + strHistory.ToString());

            }
            catch (Exception ex)
            {
                isSucess = false;
                Logger.For(this).Error("Error in GenerateEmailBodyforInvoice :" + strHistory.ToString() + ",Error :" + ex.ToString());
                Logger.For(this).Invoice("Error in GenerateEmailBodyforInvoice :" + strHistory.ToString() + ",Error :" + ex.ToString());
                throw ex;
            }
            return isSucess;
        }


        private bool SendEmailToAgent(int corrQueueId, string AttachmentFile, out string errorMessage, out SendGridEventsAPI.SDK.ResponseEntity _response, int servicePackageId = 0)
        {
            try
            {
                _response = new SendGridEventsAPI.SDK.ResponseEntity();
                bool issuccess = false;
                Logger.For(this).Invoice("Inside SendEmailToAgent Inputs:: corrQueueId:" + corrQueueId + " AttachmentFile:" + AttachmentFile);
                PTXboAgentEmail agetEmail; string emailIds = string.Empty;
                if (GetAgentEmail(corrQueueId, out agetEmail, out errorMessage, servicePackageId))
                {
                    emailIds = agetEmail.PrimaryAgentEmail != null ? emailIds + agetEmail.PrimaryAgentEmail + ";" : emailIds;
                    emailIds = agetEmail.ContactAgentEmail != null ? emailIds + agetEmail.ContactAgentEmail + ";" : emailIds;
                    emailIds = agetEmail.SalesAgentEmail != null ? emailIds + agetEmail.SalesAgentEmail + ";" : emailIds;

                    EmailEntity objEmailEntity = new EmailEntity();
                    buildEmailBody(emailIds, string.Empty, string.Empty, string.Empty, AttachmentFile, string.Empty, out objEmailEntity);

                    //Added by SaravananS.tfs id:54978
                    issuccess = SendEmail_SendGrid(objEmailEntity, out _response, out errorMessage);
                    //Ends here.
                    Logger.For(this).Invoice("Finish SendEmailToAgent Inputs:: corrQueueId:" + corrQueueId + " AttachmentFile:" + AttachmentFile + " emailIds :" + emailIds);

                }
                return issuccess;
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("Error SendInvoiceEmailToAgent Inputs:: corrQueueId:" + corrQueueId + " AttachmentFile:" + AttachmentFile);
                throw ex;
            }
        }

        public bool buildEmailBody(string emailId, string alternateEmailId, string Name, string companyType, string AttachmentFile1, string AttachmentFile2, out EmailEntity objEmailEntity, string AdditionalEmail = null, int? ServicePackageId = null, string ClientNumber = "", string Link = "", int CAFClientId = 0)
        {
            bool isSucess = true;
            string CCEmailId = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(alternateEmailId) && !string.IsNullOrEmpty(AdditionalEmail))
                {
                    CCEmailId = alternateEmailId + ";" + AdditionalEmail;
                }
                else if (!string.IsNullOrEmpty(AdditionalEmail))
                {
                    CCEmailId = AdditionalEmail;
                }
                else if (!string.IsNullOrEmpty(alternateEmailId))
                {
                    CCEmailId = alternateEmailId;
        }

                string xmlReportFilePath = "";
                
                xmlReportFilePath = Path.Combine(Path.GetDirectoryName(typeof(ReportData).Assembly.CodeBase), "XML", "CAF Mail.xml").Replace("file:\\", "");

                XDocument doc = XDocument.Load(xmlReportFilePath);

                XmlDocument docEmailSettings = new XmlDocument();

                docEmailSettings.Load(xmlReportFilePath);

                var subject = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/subject").InnerText);
                var message = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/body").InnerText);
                var salutation = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/Salutation").InnerText);
                var fromEmail = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/from").InnerText);
                var Sincerely = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/CommercialSincerely").InnerText);
                var bcc = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/bcc").InnerText);

                var objValueSummaryParam = new ValueSummaryParam
                {
                    ClientNumber = ClientNumber,
                    CAFClientId = CAFClientId,
                    ReportName = "Account Summary",
                    FileName = "mstrpt_AccountSummary.rpt",
                    InactiveAccounts = "YES"
                };
                string Request = Newtonsoft.Json.JsonConvert.SerializeObject(objValueSummaryParam);

                string reason = "";
                string InactivationInError = "";
                
                
                message = message.Replace("{Link}", Link);
                message = message.Replace("{encrypted_Client_Number}", Spartaxx.Utilities.Utilities.TDESCryptorEngine.Encrypt(Request, true));
                message = message.Replace("{Date}", DateTime.Now.Date.ToString("MM/dd/yyyy"));
                message = message.Replace("{Reason}", reason);
                message = message.Replace("{InactivationInError}", InactivationInError);

                if (companyType == "Residential")
                {
                    subject = subject.Replace("O'Connor", "O'Connor & Associates");
                    message = message.Replace("O'Connor", "O'Connor & Associates");
                    Sincerely = Convert.ToString(docEmailSettings.DocumentElement.SelectSingleNode("/email/ResidetialSincerely").InnerText);
                }

                string emailBody = salutation.ToString() + Name + "\n" + message.ToString() + "\r\n" + Sincerely;


                objEmailEntity = new EmailEntity();
                StringBuilder strMessage = new StringBuilder();
                List<string> lstAttachmentFiles = new List<string>();
                objEmailEntity.emailSender = fromEmail;
                objEmailEntity.emailRecipient = (emailId);
                objEmailEntity.emailCc = CCEmailId;
                objEmailEntity.emailBcc = bcc;

                objEmailEntity.emailSubject = subject.ToString();

                objEmailEntity.emailMessage = emailBody;
                objEmailEntity.isBodyHtml = true;

                if (!string.IsNullOrEmpty(AttachmentFile1))
                    lstAttachmentFiles.Add(AttachmentFile1);
                if (AttachmentFile2 != string.Empty)
                    lstAttachmentFiles.Add(AttachmentFile2);
                objEmailEntity.attachmentFiles = lstAttachmentFiles;


            }
            catch (Exception ex)
            {
                isSucess = false;
                throw ex;
            }
            return isSucess;
        }




        public bool GetAgentEmail(int CorrqueuId, out PTXboAgentEmail agetEmail, out string errorMessage, int servicePackageId = 0)
        {
            Hashtable parameters = new Hashtable();
            agetEmail = new PTXboAgentEmail();
            errorMessage = string.Empty;
            try
            {
                parameters.Add("@corrqueueId", CorrqueuId);
                parameters.Add("@ServicePackageid", servicePackageId);
                Logger.For(this).Invoice("GetAgentEmail-API  reached.servicePakageId: " + servicePackageId.ToJson(false));
                agetEmail = _connection.Select<PTXboAgentEmail>(PTXdoStoredProcedureNames.usp_getAgentEmail, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetAgentEmail-API  ends successfully ");
                return true;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetAgentEmail.servicePakageId: " + servicePackageId + "- API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        /// <summary>
        /// Created by SaravananS.
        /// </summary>
        /// <param name="objEmailEntity"></param>
        /// <param name="_response"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool SendEmail_SendGrid(EmailEntity objEmailEntity, out SendGridEventsAPI.SDK.ResponseEntity _response, out string errorMessage)
        {
            bool isSucess = true;
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("Inside SendEmail_SendGrid. ;" + ((object)objEmailEntity).ToJson(false));
                Email objEmail = new Email();
                bool Status = objEmail.SendEmail_SendGrid(objEmailEntity, out _response, out errorMessage);
                isSucess = Status;

                Logger.For(this).Invoice("SendEmail_SendGrid.Mail has been forwarded ;" + ((object)objEmailEntity).ToJson(false));
            }
            catch (Exception ex)
            {
                errorMessage += ex.Message;
                Logger.For(this).Invoice(ex);

                Logger.For(this).Error("error in SendEmail_SendGrid: " + ((object)objEmailEntity).ToJson(false) + ",error:" + errorMessage);//Added by SaravananS
                // log.WriteToEventLog(ex.ToString(), "error");
                isSucess = false;
                throw ex;
            }
            return isSucess;
        }



        /// <summary>
        /// Method used to copy the file from source path to desination path --Added by SaravananS
        /// </summary>
        /// <param name="servicePackageId"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceFilePath2"></param>
        /// <param name="errorMessage"></param>
        public void MoveInvoiceFileToNeoPost(int servicePackageId, string sourceFilePath, out string outputPath, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("Inside MoveInvoiceFileToNeoPost servicePackageId:" + servicePackageId + " sourceFilePath:" + sourceFilePath);
                string letterFileName = string.Empty;
                string neoPostPath = string.Empty;

                GetNeoPostPath(servicePackageId, out neoPostPath);

                letterFileName = sourceFilePath.Substring(sourceFilePath.LastIndexOf("\\", sourceFilePath.Length));
                
                //added by saravanans:couldn't move the file if users open it.-tfs:48560
                if (letterFileName.Trim().Substring(0, 1) == "\\")
                {
                    letterFileName = letterFileName.Replace("\\", "").Trim();
                }
                outputPath = Path.Combine(neoPostPath, letterFileName);//Added by Saravanans
                Logger.For(this).Invoice("Moving file from " + sourceFilePath + " to " + outputPath);
                File.Copy(sourceFilePath, outputPath, true);//added by saravanans:couldn't move the file if users open it.-tfs:48560
                                                            //---Ends here...
                                                            //File.Move(sourceFilePath, outputPath);
                Logger.For(this).Invoice("Finished MoveInvoiceFileToNeoPost servicePackageId:" + servicePackageId + " sourceFilePath:" + sourceFilePath);

            }
            catch (Exception ex)
            {
                //Logger.For("moveFileToNeoPost").Error(ex);
                Logger.For(this).Invoice("Error MoveInvoiceFileToNeoPost servicePackageId:" + servicePackageId + " sourceFilePath:" + sourceFilePath + " ex:" + ex);
                errorMessage = ex.Message;
                throw ex;
            }
        }


        public bool GetNeoPostPath(int servicePackageId,out string neoPostPath)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("GetNeoPostPath.servicePackageId:" + servicePackageId);
                
                parameters.Add("@ServicePackageId", servicePackageId);

                neoPostPath = _connection.ExecuteScalar(StoredProcedureNames.usp_getServicePackageNeoPostpath, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Created by SaravananS. 
        /// </summary>
        /// <param name="servicePackageId"></param>
        /// <param name="corrqId"></param>
        /// <param name="invoiceFilePath"></param>
        /// <param name="invoiceSourcePath"></param>
        /// <returns></returns>
        public bool UpdateInvoiceCorrqFilePath(int servicePackageId, int corrqId, string invoiceFilePath, string invoiceSourcePath = "")
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("UpdateInvoiceCorrqFilePath.CorrqId:" + corrqId + ",ServicePackageId: " + servicePackageId + ", InvoiceFilePath:" + invoiceFilePath);
                parameters.Add("@CorrqId", corrqId);
                parameters.Add("@InvoiceFilePath", invoiceFilePath);
                parameters.Add("@ServicePackageId", servicePackageId);
                parameters.Add("@InvoiceSourcePath", invoiceSourcePath);
                var result = Convert.ToBoolean(_connection.ExecuteScalar(StoredProcedureNames.usp_UpdateInvoiceCorrqFilePath, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Created by SaravananS. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="corrQId"></param>
        /// <param name="objClientFaxDetails"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool GetClientFaxDetails(int clientId, int corrQId, out PTXboClientFaxDetails objClientFaxDetails, out string errorMessage)
        {
            Hashtable parameters = new Hashtable();
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("GetClientFaxDetails.CorrqId:" + corrQId + ",ServicePackageId: " + clientId);
                parameters.Add("@ClientId", clientId);
                parameters.Add("@CorrQid", corrQId);

                objClientFaxDetails = _connection.Select<PTXboClientFaxDetails>(PTXdoStoredProcedureNames.usp_GetClientFaxDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Created by SaravananS
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="corrQId"></param>
        /// <param name="objOconnorCompanyDetails"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool GetOconnorCompanyDetails( out PTXboCompanydetails objOconnorCompanyDetails, out string errorMessage)
        {
            Hashtable parameters = new Hashtable();
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("GetOconnorCompanyDetails.");

                objOconnorCompanyDetails = _connection.Select<PTXboCompanydetails>(PTXdoStoredProcedureNames.usp_getOconnorCompanyDetailsWS, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private bool SendCustomDeliveryInvoiceEMailToClient(int clientID, int invoiceid, string AttachmentFile1, string AttachmentFile2, out string errorMessage, out SendGridEventsAPI.SDK.ResponseEntity _response, int groupId = 0, int CorrId = 0)
        {
            errorMessage = string.Empty;
            try
            {
                _response = new SendGridEventsAPI.SDK.ResponseEntity();
                Logger.For(this).Invoice("Inside SendCustomDeliveryInvoiceEMailToClient  clientID:" + clientID + " invoiceid :" + invoiceid + " AttachmentFile1:" + AttachmentFile1 + " AttachmentFile2:" + AttachmentFile2 + " CorrId:" + CorrId);
                string emailAddress = string.Empty;
                string alternateEmailAddress = string.Empty;
                string clientFirstName = string.Empty;
                string companyType = string.Empty;
                string AdditionalEmail = string.Empty;
                EmailEntity objEmailEntity;
                PTXboGetClientEmailInput objGetClientEmail = new PTXboGetClientEmailInput()
                {
                    ClientId = clientID,
                    CorrQId = CorrId,
                    GroupId = groupId

                };
              var result =  GetClientEmailInvoice(objGetClientEmail);
                if (result != null)
                {

                    if (result.IsCustomDelivery)
                    {
                        emailAddress = result.CustomEmail.Trim();
                        // emailId = "SpartaxxDevTeam@poconnor.com";//added by saravanans for testing..This line must be commented after finishing test
                    }
                    else
                    {
                        emailAddress = result.Email.Trim();
                        // emailId = "SpartaxxDevTeam@poconnor.com";//added by saravanans for testing..This line must be commented after finishing test
                    }
                    alternateEmailAddress = result.AlternateEmail.Trim();
                    AdditionalEmail = result.AdditionalEmail.Trim();
                    clientFirstName = result.FirstName.Trim();
                    companyType = result.CompanyType.Trim();

                    Logger.For(this).Invoice("End GetClientEmailInvoice output:corrQid:" + CorrId.ToString() + ",custom delivery:" + result.IsCustomDelivery.ToString() + ",emailId:" + emailAddress + ",alternateEmailAddrss :" + alternateEmailAddress + ",AdditionalEmail:" + AdditionalEmail + ",clientFirstName:" + clientFirstName + ",companyType:" + companyType);
                    
                }
                GenerateEmailBodyforInvoice(emailAddress, alternateEmailAddress, clientFirstName, companyType, AttachmentFile1, AttachmentFile2, out objEmailEntity, AdditionalEmail, invoiceid);
                
                //Added by SaravananS.tfs id:54978
                _response = new SendGridEventsAPI.SDK.ResponseEntity();
                bool issuccess = SendEmail_SendGrid(objEmailEntity, out _response, out errorMessage);
                //Ends here.
                Logger.For(this).Invoice("Finished SendCustomDeliveryInvoiceEMailToClient  clientID:" + clientID.ToString() + ",invoiceid :" + invoiceid.ToString() + ",AttachmentFile1:" + AttachmentFile1 + ",AttachmentFile2:" + AttachmentFile2 + ",CorrId:" + CorrId.ToString());
                return issuccess;
            }
            catch (Exception ex)
            {
                errorMessage += ex.Message;
                Logger.For(this).Invoice("Error in SendCustomDeliveryInvoiceEMailToClient clientID:" + clientID.ToString() + ",invoiceid :" + invoiceid.ToString() + ",AttachmentFile1:" + AttachmentFile1 + ",AttachmentFile2:" + AttachmentFile2 + ",CorrId:" + CorrId.ToString() + " ,error :" + errorMessage);
                Logger.For(this).Error("Error in SendCustomDeliveryInvoiceEMailToClient clientID:" + clientID.ToString() + ",invoiceid :" + invoiceid.ToString() + ",AttachmentFile1:" + AttachmentFile1 + ",AttachmentFile2:" + AttachmentFile2 + ",CorrId:" + CorrId.ToString() + " ,error: " + errorMessage);

                //log.WriteToEventLog(ex.ToString(), "error");
                throw ex;
                
            }
        }

        ///// <summary>
        ///// Created by SaravananS. tfs id:61899
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="errorMessage"></param>
        ///// <returns></returns>
        //public bool GenerateInvoice(PTXboGenerateInvoice_Request request, out string errorMessage)
        //{
        //    errorMessage = string.Empty;
        //    try
        //    {
        //        Logger.For(this).Invoice("GenerateInvoiceUsmailCoverLetter-API  reached " + ((object)request).ToJson(false));
        //        var result = _invoiceCalculation.GenerateInvoice(request,out errorMessage);
        //        Logger.For(this).Invoice("GenerateInvoiceUsmailCoverLetter-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GenerateInvoiceUsmailCoverLetter-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}


        /// <summary>
        /// Added by SaravananS. tfs id:63159
        /// </summary>
        /// <param name="objGetClientEmail"></param>
        /// <returns></returns>
        public PTXboClientEmail GetClientDeliveryAddressForInvoice(PTXboGetClientEmailInput objGetClientEmail)
        {
            try
            {
                if(objGetClientEmail.GroupId==0 && objGetClientEmail.InvoiceId>0)
                {
                    var invoiceDetails = GetInvoiceDetailsById(objGetClientEmail.InvoiceId);
                    objGetClientEmail.GroupId = invoiceDetails.GroupId;
                }

                Logger.For(this).Invoice("GetClientDeliveryAddressForInvoice-API  reached " + ((object)objGetClientEmail).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ClientId", objGetClientEmail.ClientId);
                // parameters.Add("@InvoiceId", objGetClientEmail.InvoiceId);
                // parameters.Add("@CorrQId", objGetClientEmail.CorrQId);
                parameters.Add("@GroupId", objGetClientEmail.GroupId.ToString());
                var result = _connection.Select<PTXboClientEmail>(StoredProcedureNames.usp_get_ClientEmailAddress_Invoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetClientDeliveryAddressForInvoice-API  ends successfully ");

                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetClientDeliveryAddressForInvoice-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
        }



    }
}
