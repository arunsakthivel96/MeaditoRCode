using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using Spartaxx.Common;
using Spartaxx.DataObjects;
using Spartaxx.Utilities.Extenders;
using Spartaxx.Utilities.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    //added by saravanans-tfs:47247
   public class InvoiceSpecialTermsRepository: IInvoiceSpecialTermsRepository,IDisposable
    {
        private readonly DapperConnection _dapperConnection;
       // private readonly InvoiceRepository _invoiceRepository;
        private readonly InvoiceCalculationRepository _invoiceCalculation; //Invoice calculation engine
        public InvoiceSpecialTermsRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
           // _invoiceRepository = new InvoiceRepository(dapperConnection);
            _invoiceCalculation = new InvoiceCalculationRepository(dapperConnection);

        }

        private bool disposed = false;
        /// <summary>
        /// Dispose all used resources..Added by saravanans
        /// </summary>

        public void Dispose()
        {
            // this.Dispose(true);
            // GC.SuppressFinalize(this);
            GC.Collect();
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                disposed = true;
            }
        }

        //public InvoiceSpecialTermsRepository(InvoiceRepository invoiceRepository)
        //{
        //    _invoiceRepository = invoiceRepository;
        //}

        public List<PTXboSpecialTermClients> LoadSpecialTermClients(PTXboSpecialTermClientsFilter specialTermClientFilter)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("LoadSpecialTermClients-API  reached " + ((object)specialTermClientFilter).ToJson(false));
                parameters.Add("@ClientNumber", specialTermClientFilter.ClientNumber);
                parameters.Add("@AccountNumber", specialTermClientFilter.AccountNumber);
                parameters.Add("@ProjectName", specialTermClientFilter.ProjectName);
                parameters.Add("@InvoiceTypeId", specialTermClientFilter.InvoiceTypeID);
                parameters.Add("@TaxYear", specialTermClientFilter.TaxYear);
                parameters.Add("@IsHearingFinalized", specialTermClientFilter.IsHearingFinalized);
                parameters.Add("@InvoiceGroupingType", specialTermClientFilter.InvoiceGroupingType);
                var result = _dapperConnection.Select<PTXboSpecialTermClients>(StoredProcedureNames.usp_GetSpecialTermClients, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("LoadSpecialTermClients-API  ends successfully ");
                return result;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("LoadSpecialTermClients-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public List<PTXboSpecialTermClients> LoadSpecialTermOutofTexasClients(PTXboSpecialTermClientsFilter specialTermClientFilter)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("LoadSpecialTermOutofTexasClients-API  reached " + ((object)specialTermClientFilter).ToJson(false));
                parameters.Add("@ClientNumber", specialTermClientFilter.ClientNumber);
                parameters.Add("@AccountNumber", specialTermClientFilter.AccountNumber);
                parameters.Add("@ProjectName", specialTermClientFilter.ProjectName);
                parameters.Add("@InvoiceTypeId", specialTermClientFilter.InvoiceTypeID);
                parameters.Add("@TaxYear", specialTermClientFilter.TaxYear);
                parameters.Add("@IsHearingFinalized", specialTermClientFilter.IsHearingFinalized);
                parameters.Add("@InvoiceGroupingType", specialTermClientFilter.InvoiceGroupingType);
                parameters.Add("@StateId", specialTermClientFilter.StateId);
                var result = _dapperConnection.Select<PTXboSpecialTermClients>(StoredProcedureNames.usp_GetSpecialTermClients_OT, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("LoadSpecialTermOutofTexasClients-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("LoadSpecialTermOutofTexasClients-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public List<PTXboSpecialTermClients> LoadSpecialTermsDonotGenerateInvoices(PTXboSpecialTermClientsFilter objFilter)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("LoadSpecialTermsDonotGenerateInvoices-API  reached " + ((object)objFilter).ToJson(false));
                parameters.Add("@InvoiceTypeId", objFilter.InvoiceTypeID);
                parameters.Add("@ClientNumber", objFilter.ClientNumber == "" ? null: objFilter.ClientNumber);
                parameters.Add("@AccountNumber", objFilter.AccountNumber==""?null: objFilter.AccountNumber);
                parameters.Add("@ProjectName", objFilter.ProjectName==""?null: objFilter.ProjectName);
                parameters.Add("@TaxYear", objFilter.TaxYear);
                parameters.Add("@InvoiceGroupingType", objFilter.InvoiceGroupingType);
                var result = _dapperConnection.Select<PTXboSpecialTermClients>(StoredProcedureNames.Usp_GetDonotGenerateInvoicesList, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("LoadSpecialTermsDonotGenerateInvoices-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("LoadSpecialTermsDonotGenerateInvoices-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        /// <summary>
        /// Added by saravanans.tfs id:55312
        /// </summary>
        /// <param name="objFilter"></param>
        /// <returns></returns>
        public List<PTXboSpecialTermClients> LoadSpecialTermsReGenerateInvoices(PTXboSpecialTermClientsFilter objFilter)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("LoadSpecialTermsReGenerateInvoices-API  reached " + ((object)objFilter).ToJson(false));
                parameters.Add("@InvoiceTypeId", objFilter.InvoiceTypeID);
                parameters.Add("@ClientNumber", objFilter.ClientNumber == "" ? null : objFilter.ClientNumber);
                parameters.Add("@AccountNumber", objFilter.AccountNumber == "" ? null : objFilter.AccountNumber);
                parameters.Add("@ProjectName", objFilter.ProjectName == "" ? null : objFilter.ProjectName);
                parameters.Add("@TaxYear", objFilter.TaxYear);
                parameters.Add("@InvoiceGroupingType", objFilter.InvoiceGroupingType);
                var result = _dapperConnection.Select<PTXboSpecialTermClients>(StoredProcedureNames.usp_GetSpecialTermRegenerateClients, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("LoadSpecialTermsReGenerateInvoices-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("LoadSpecialTermsReGenerateInvoices-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }
        public bool UpdateInvoiceDetailsReport(PTXboInvoiceReport objInvoice)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("UpdateInvoiceDetailsReport-API  reached " + ((object)objInvoice).ToJson(false));
                parameters.Add("@InvoiceID", objInvoice.InvoiceId);
                parameters.Add("@InterestRateID", objInvoice.InterestRateID);
                parameters.Add("@InitialAssessedValue", objInvoice.Total_Initial_Assessed_Value);
                parameters.Add("@FinalAssessedValue", objInvoice.Total_Final_Assessed_Value);
                parameters.Add("@PriorYearTaxRate", objInvoice.Prior_Year_TaxRate);
                parameters.Add("@ContingencyPercentage", objInvoice.Contingency);
                parameters.Add("@ContingencyFee", objInvoice.Contingency_fee);
                parameters.Add("@FlatFee ", objInvoice.Flat_fee);
                parameters.Add("@InvoiceAmount", objInvoice.Invoice_amount);
                parameters.Add("@CompoundInterest", objInvoice.CompoundInterest);
                parameters.Add("@Reduction", objInvoice.Reduction);
                parameters.Add("@TotalEstimatedTaxSavings", objInvoice.Estimated_Tax_Savings);
                parameters.Add("@AmountPaid", objInvoice.Amount_paid);
                parameters.Add("@AmountAdjusted", objInvoice.Amount_adjusted);
                parameters.Add("@ApplicableInterest", objInvoice.Applicable_interest);
                parameters.Add("@InterestPaid", objInvoice.Interest_paid);
                parameters.Add("@InterestAdjusted ", objInvoice.Interest_adjusted);
                parameters.Add("@AmountDue", objInvoice.Amount_due);
                //added for out of texas invoice generation...tfs-50617--saravanans
                parameters.Add("@InvoiceDescription", objInvoice.Description);
                parameters.Add("@AssessmentRatio", objInvoice.AssessmentRatio);
                parameters.Add("@AssessmentValue", objInvoice.AssessmentValue);
                parameters.Add("@AmountPaidAttorney", objInvoice.AmountPaidAttorney);
                parameters.Add("@MillageRate", objInvoice.MillageRate);
                parameters.Add("@PaymentDuration", objInvoice.PaymentDuration); //tfs-52537
                parameters.Add("@InterestRate", objInvoice.InterestRate); //Added by SaravananS tfs id:59605
                //ends here..
                var result = _dapperConnection.Execute(StoredProcedureNames.usp_InvoiceReportStatusUpdate, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("UpdateInvoiceDetailsReport-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateInvoiceDetailsReport-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public bool UpdateInvoiceProcessStatus(PTXboUpdateInvoiceProcessingStatusInput invoiceProcessingStatus)
        {
            try
            {
                Logger.For(this).Invoice("UpdateInvoiceProcessStatus-API  reached " + ((object)invoiceProcessingStatus).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ClientID", invoiceProcessingStatus.ClientID);
                parameters.Add("@GroupID", invoiceProcessingStatus.GroupID);
                parameters.Add("@ProjectID", invoiceProcessingStatus.ProjectID);
                parameters.Add("@InvoiceID", invoiceProcessingStatus.InvoiceID);
                parameters.Add("@InvoiceProcessingStatusID", invoiceProcessingStatus.InvoiceProcessingStatusID);
                parameters.Add("@YearlyHearingDetailsID", invoiceProcessingStatus.YearlyHearingDetailsID);
                parameters.Add("@InvoiceSummaryProcessId", invoiceProcessingStatus.InvoiceSummaryProcessId);
                parameters.Add("@InvoiceGeneartedForID", invoiceProcessingStatus.InvoiceGeneartedForID);
                parameters.Add("@CorrProcessId", invoiceProcessingStatus.CorrProcessId);
                var result = _dapperConnection.Execute(StoredProcedureNames.usp_InvoiceStatusUpdate, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("UpdateInvoiceProcessStatus-API  ends successfully ");
                return true;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("UpdateInvoiceProcessStatus-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboSpecialTermInvoiceDetails SpecialTermInvoicesGeneration(PTXboSpecialTermInput specialInput)
        {
            Hashtable parameters = new Hashtable();
            PTXboSpecialTermInvoiceDetails objSpecialTermInvoiceDetails = new PTXboSpecialTermInvoiceDetails();
            try
            {
                Logger.For(this).Invoice("SpecialTermInvoicesGeneration-API  reached " + ((object)specialInput).ToJson(false));
                parameters.Add("@TermId", specialInput.TermId);
                parameters.Add("@Id", specialInput.Id);
                parameters.Add("@TaxYear", specialInput.TaxYear);
                var result = _dapperConnection.SelectMultiple(StoredProcedureNames.Usp_GetSpecialTermAccounts, parameters, Enumerator.Enum_CommandType.StoredProcedure,
                    Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXboSpecialTermInvoiceDetails>(),
                    gr => gr.Read<PTXboSpecialTermAccounts>());

                objSpecialTermInvoiceDetails = result.Item1.Count() > 0 ? result.Item1.FirstOrDefault() : new PTXboSpecialTermInvoiceDetails();
                objSpecialTermInvoiceDetails.lstTermAccounts = result.Item2.Count() > 0 ? (List<PTXboSpecialTermAccounts>)result.Item2 : new List<PTXboSpecialTermAccounts>();
                Logger.For(this).Invoice("SpecialTermInvoicesGeneration-API  ends successfully ");

                return objSpecialTermInvoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SpecialTermInvoicesGeneration-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public PTXboSpecialTermInvoiceDetails SpecialTermOutofTexasInvoicesGeneration(PTXboSpecialTermInput specialInput)
        {
            Hashtable parameters = new Hashtable();
            PTXboSpecialTermInvoiceDetails objSpecialTermOutofTexasInvoiceDetails = new PTXboSpecialTermInvoiceDetails();
            try
            {
                Logger.For(this).Invoice("SpecialTermOutofTexasInvoicesGeneration-API  reached " + ((object)specialInput).ToJson(false));
                parameters.Add("@TermId", specialInput.TermId);
                parameters.Add("@Id", specialInput.Id);
                parameters.Add("@TaxYear", specialInput.TaxYear); 
                parameters.Add("@StateId", specialInput.StateId);
                parameters.Add("@InvoiceTypeId", specialInput.InvoiceTypeId);
                var result = _dapperConnection.SelectMultiple(StoredProcedureNames.Usp_GetSpecialTermOutofTexasAccounts, parameters, Enumerator.Enum_CommandType.StoredProcedure,
                    Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXboSpecialTermInvoiceDetails>(),
                    gr => gr.Read<PTXboSpecialTermAccounts>());

                objSpecialTermOutofTexasInvoiceDetails = result.Item1.Count() > 0 ? result.Item1.FirstOrDefault() : new PTXboSpecialTermInvoiceDetails();
                objSpecialTermOutofTexasInvoiceDetails.lstTermAccounts = result.Item2.Count() > 0 ? (List<PTXboSpecialTermAccounts>)result.Item2 : new List<PTXboSpecialTermAccounts>();
                Logger.For(this).Invoice("SpecialTermOutofTexasInvoicesGeneration-API  ends successfully ");

                return objSpecialTermOutofTexasInvoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SpecialTermOutofTexasInvoicesGeneration-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        //public PTXboYearlyHearings GetYearlyHearingDetails(int yearlyHearingDetailsId)
        //{
        //    Hashtable parameters = new Hashtable();
        //    try
        //    {
        //        Logger.For(this).Invoice("GetYearlyHearingDetails-API  reached " + ((object)yearlyHearingDetailsId).ToJson(false));
        //        parameters.Add("@YearlyHearingDetailsId", yearlyHearingDetailsId);

        //        var result = _dapperConnection.Select<PTXboYearlyHearings>(StoredProcedureNames.usp_get_YearlyHearingDetails, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        Logger.For(this).Invoice("GetYearlyHearingDetails-API  ends successfully ");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetYearlyHearingDetails-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}



        //public PTXboHearingResult GetHearingResultByType(int hearingDetailsId,int hearingTypeId=0)
        //{
        //    Hashtable parameters = new Hashtable();
        //    try
        //    {
        //        Logger.For(this).Invoice("GetHearingResultByType-API  reached " + ((object)"hearingDetailsId="+ hearingDetailsId.ToString()+ "hearingTypeId="+ hearingTypeId.ToString()).ToJson(false));
        //        //parameters.Add("@HearingDetailsId", hearingDetailsId);
        //        //parameters.Add("@HearingTypeId", hearingTypeId);
        //        //var result = _dapperConnection.Select<PTXboHearingResult>(StoredProcedureNames.usp_get_HearingResultByType, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        var result = _invoiceCalculation.GetHearingResultByType(hearingDetailsId, hearingTypeId);
        //        Logger.For(this).Invoice("GetHearingResultByType-API  ends successfully ");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetHearingResultByType-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public PTXboLitigationDetails GetLitigation(int invoiceID)
        //{
        //    Hashtable parameters = new Hashtable();
        //    try
        //    {
        //        Logger.For(this).Invoice("GetLitigation-API  reached " + ((object)invoiceID).ToJson(false));
        //        //parameters.Add("@LitigationID", invoiceID);

        //        //var result = _dapperConnection.Select<PTXboLitigationDetails>(StoredProcedureNames.usp_get_LitigationDetails, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        var result = _invoiceCalculation.GetLitigation(invoiceID);
        //        Logger.For(this).Invoice("GetLitigation-API  ends successfully ");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetLitigation-API  error " + ((object)ex).ToJson(false));

        //        throw ex;
        //    }
        //}
        //public PTXboArbitration GetArbitrationDetails(int invoiceID)
        //{
        //    Hashtable parameters = new Hashtable();
        //    try
        //    {
        //        Logger.For(this).Invoice("GetArbitrationDetails-API  reached " + ((object)invoiceID).ToJson(false));
        //        //parameters.Add("@ArbitrationDetailsId", invoiceID);

        //        //var result = _dapperConnection.Select<PTXboArbitration>(StoredProcedureNames.usp_get_ArbitrationDetails, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        var result = _invoiceCalculation.GetArbitrationDetails(invoiceID);
        //        Logger.For(this).Invoice("GetArbitrationDetails-API  ends successfully ");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetArbitrationDetails-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public bool SaveOrUpdateHearingResult(PTXboHearingResult hearingResult)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateHearingResult-API  reached " + ((object)hearingResult).ToJson(false));
        //        //parameters.Add("@HearingResultId", hearingResult.HearingResultId);
        //        //parameters.Add("@HearingDetailsId", hearingResult.HearingDetailsId);
        //        //parameters.Add("@HearingTypeId", hearingResult.HearingTypeId);
        //        //parameters.Add("@HearingResolutionId", hearingResult.HearingResolutionId);
        //        //parameters.Add("@HearingValue", hearingResult.HearingValue);
        //        //parameters.Add("@HearingResultReasonCodeId", hearingResult.HearingResultReasonCodeId);
        //        //parameters.Add("@PostHearingLandValue", hearingResult.PostHearingLandValue);
        //        //parameters.Add("@PostHearingImprovedValue", hearingResult.PostHearingImprovedValue);
        //        //parameters.Add("@PostHearingMarketValue", hearingResult.PostHearingMarketValue);
        //        //parameters.Add("@PostHearingTotalValue", hearingResult.PostHearingTotalValue);
        //        //parameters.Add("@HearingAgentId", hearingResult.HearingAgentId);
        //        //parameters.Add("@HearingStatusId", hearingResult.HearingStatusId);
        //        //parameters.Add("@HearingFinalized", hearingResult.HearingFinalized);
        //        //parameters.Add("@DismissalAuthStatusId", hearingResult.DismissalAuthStatusId);
        //        //parameters.Add("@confirmationLetterReceivedDate", hearingResult.confirmationLetterReceivedDate);
        //        //parameters.Add("@confirmationLetterDateTime", hearingResult.confirmationLetterDateTime);
        //        //parameters.Add("@completionDateAndTime", hearingResult.completionDateAndTime);
        //        //parameters.Add("@HearingResultsSentOn", hearingResult.HearingResultsSentOn);
        //        //parameters.Add("@InvoiceGenerated", hearingResult.InvoiceGenerated);
        //        //parameters.Add("@ReviewForArbitration", hearingResult.ReviewForArbitration);
        //        //parameters.Add("@ClientRequestedForArbitration", hearingResult.ClientRequestedForArbitration);
        //        //parameters.Add("@HearingProcessingStatusId", hearingResult.HearingProcessingStatusId);
        //        //parameters.Add("@UpdatedBy", hearingResult.UpdatedBy);
        //        //parameters.Add("@UpdatedDateTime", hearingResult.UpdatedDateTime);
        //        //parameters.Add("@HearingTrackingCodeId", hearingResult.HearingTrackingCodeId);
        //        //parameters.Add("@HearingResultReportId", hearingResult.HearingResultReportId);
        //        //parameters.Add("@ExemptInvoice", hearingResult.ExemptInvoice);
        //        //parameters.Add("@ResultGenerated", hearingResult.ResultGenerated);
        //        //parameters.Add("@FinalizedDate", hearingResult.FinalizedDate);
        //        //parameters.Add("@HRInvoiceStatusid", hearingResult.HRInvoiceStatusid);
        //        //parameters.Add("@AppraiserName", hearingResult.AppraiserName);
        //        //parameters.Add("@ReviewBindingArbitration", hearingResult.ReviewBindingArbitration);

        //        //var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateHearingResult, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        var result = _invoiceCalculation.SaveOrUpdateHearingResult(hearingResult);
        //        Logger.For(this).Invoice("SaveOrUpdateHearingResult-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateHearingResult-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}


        //public bool SaveOrUpdateLitigation(PTXboLitigationDetails litigationDetails)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateLitigation-API  reached " + ((object)litigationDetails).ToJson(false));
        //        //parameters.Add("@CauseNo", litigationDetails.CauseNo);
        //        //parameters.Add("@CauseName", litigationDetails.CauseName);
        //        //parameters.Add("@TrailDate", litigationDetails.TrailDate);
        //        //parameters.Add("@LateFiled", litigationDetails.LateFiled);
        //        //parameters.Add("@DoNotContact", litigationDetails.DoNotContact);
        //        //parameters.Add("@FilingDueDate", litigationDetails.FilingDueDate);
        //        //parameters.Add("@SupplementName", litigationDetails.SupplementName);
        //        //parameters.Add("@LitigationStatusID", litigationDetails.LitigationStatusID);
        //        //parameters.Add("@AuthLetterDate", litigationDetails.AuthLetterDate);
        //        //parameters.Add("@ExpertAssignedID", litigationDetails.ExpertAssignedID);
        //        //parameters.Add("@FilingRequestDate", litigationDetails.FilingRequestDate);
        //        //parameters.Add("@AnalystID", litigationDetails.AnalystID);
        //        //parameters.Add("@AttorneyConfDate", litigationDetails.AttorneyConfDate);
        //        //parameters.Add("@DocketReceivedDate", litigationDetails.DocketReceivedDate);
        //        //parameters.Add("@InitialAgreedDate", litigationDetails.InitialAgreedDate);
        //        //parameters.Add("@FinalAgreedDate", litigationDetails.FinalAgreedDate);
        //        //parameters.Add("@BatchCauseNo", litigationDetails.BatchCauseNo);
        //        //parameters.Add("@BatchCauseName", litigationDetails.BatchCauseName);
        //        //parameters.Add("@ExpertsReportsDate", litigationDetails.ExpertsReportsDate);
        //        //parameters.Add("@ExpertRemarks", litigationDetails.ExpertRemarks);
        //        //parameters.Add("@SettlementDate", litigationDetails.SettlementDate);
        //        //parameters.Add("@SupplementDate", litigationDetails.SupplementDate);
        //        //parameters.Add("@LCLetterDate", litigationDetails.LCLetterDate);
        //        //parameters.Add("@CourtNo", litigationDetails.CourtNo);
        //        //parameters.Add("@CADIntroLetterDate", litigationDetails.CADIntroLetterDate);
        //        //parameters.Add("@LitMarketValue", litigationDetails.LitMarketValue);
        //        //parameters.Add("@Attorney", litigationDetails.Attorney);
        //        //parameters.Add("@OIRSentDate", litigationDetails.OIRSentDate);
        //        //parameters.Add("@LitAppraisalValue", litigationDetails.LitAppraisalValue);
        //        //parameters.Add("@Judge", litigationDetails.Judge);
        //        //parameters.Add("@OIRReceivedDate", litigationDetails.OIRReceivedDate);
        //        //parameters.Add("@LitFinalized", litigationDetails.LitFinalized);
        //        //parameters.Add("@ClientConfirmed", litigationDetails.ClientConfirmed);
        //        //parameters.Add("@DateConfirmed", litigationDetails.DateConfirmed);
        //        //parameters.Add("@DeedProvided", litigationDetails.DeedProvided);
        //        //parameters.Add("@CADVerified", litigationDetails.CADVerified);
        //        //parameters.Add("@AllottedUserID", litigationDetails.AllottedUserID);
        //        //parameters.Add("@AllottedUserRoleID", litigationDetails.AllottedUserRoleID);
        //        //parameters.Add("@AssignedBy", litigationDetails.AssignedBy);
        //        //parameters.Add("@AssignedRoleID", litigationDetails.AssignedRoleID);
        //        //parameters.Add("@AllottedDateTime", litigationDetails.AllottedDateTime);
        //        //parameters.Add("@AssignedDateTime", litigationDetails.AssignedDateTime);
        //        //parameters.Add("@CADVerificationUserID", litigationDetails.CADVerificationUserID);
        //        //parameters.Add("@CADVerifiedDate", litigationDetails.CADVerifiedDate);
        //        //parameters.Add("@LitRemarks", litigationDetails.LitRemarks);
        //        //parameters.Add("@IsAccountMoved", litigationDetails.IsAccountMoved);
        //        //parameters.Add("@LitReasonID", litigationDetails.LitReasonID);
        //        //parameters.Add("@UpdatedBy", litigationDetails.UpdatedBy);
        //        //parameters.Add("@UpdatedDate", litigationDetails.UpdatedDate);
        //        //parameters.Add("@PESMarketValue", litigationDetails.PESMarketValue);
        //        //parameters.Add("@PESUnequalAppraisalValue", litigationDetails.PESUnequalAppraisalValue);
        //        //parameters.Add("@ExportedBy", litigationDetails.ExportedBy);
        //        //parameters.Add("@IsDontCopy", litigationDetails.IsDontCopy);
        //        //parameters.Add("@PropertyTaxYear", litigationDetails.PropertyTaxYear);
        //        //parameters.Add("@PropertyTaxAccID", litigationDetails.PropertyTaxAccID);
        //        //parameters.Add("@PFVerifiedDate", litigationDetails.PFVerifiedDate);
        //        //parameters.Add("@PFVerifiedBy", litigationDetails.PFVerifiedBy);
        //        //parameters.Add("@ClientConfirmDate", litigationDetails.ClientConfirmDate);
        //        //parameters.Add("@NameChgSubmitDate", litigationDetails.NameChgSubmitDate);
        //        //parameters.Add("@CovLtrSentDate", litigationDetails.CovLtrSentDate);
        //        //parameters.Add("@DiscoverySentDate", litigationDetails.DiscoverySentDate);
        //        //parameters.Add("@SpecialFilingAccount", litigationDetails.SpecialFilingAccount);
        //        //parameters.Add("@LITInvoiceStatusid", litigationDetails.LITInvoiceStatusid);
        //        //parameters.Add("@RecievedAuthStatus", litigationDetails.RecievedAuthStatus);
        //        //parameters.Add("@ExemptInvoice", litigationDetails.ExemptInvoice);
        //        //parameters.Add("@FloodLitTypeID", litigationDetails.FloodLitTypeID);
        //        //parameters.Add("@Appraiserid", litigationDetails.Appraiserid);
        //        //parameters.Add("@AppraisalOrderCreated", litigationDetails.AppraisalOrderCreated);
        //        //parameters.Add("@AppraisalCompleted", litigationDetails.AppraisalCompleted);
        //        //parameters.Add("@AppraisalValue", litigationDetails.AppraisalValue);
        //        //parameters.Add("@ExpressAgentID", litigationDetails.ExpressAgentID);
        //        //parameters.Add("@ExpressOfferNotAccepted", litigationDetails.ExpressOfferNotAccepted);
        //        //parameters.Add("@NoExpressOffer", litigationDetails.NoExpressOffer);
        //        //parameters.Add("@SettlememtConferenceDate", litigationDetails.SettlememtConferenceDate);
        //        //parameters.Add("@SettlementOffer", litigationDetails.SettlementOffer);
        //        //parameters.Add("@TwoWeekTrial", litigationDetails.TwoWeekTrial);
        //        //parameters.Add("@HardTrialDate", litigationDetails.HardTrialDate);
        //        //parameters.Add("@HardTrialTime", litigationDetails.HardTrialTime);
        //        //parameters.Add("@DiscoveryDeadline", litigationDetails.DiscoveryDeadline);
        //        //parameters.Add("@CADUEValue", litigationDetails.CADUEValue);
        //        //parameters.Add("@CADAppraisedValue", litigationDetails.CADAppraisedValue);
        //        //parameters.Add("@PAPESRatioStudy", litigationDetails.PAPESRatioStudy);
        //        //parameters.Add("@TargetValue", litigationDetails.TargetValue);
        //        //parameters.Add("@TriggerValue", litigationDetails.TriggerValue);
        //        //parameters.Add("@Projectid", litigationDetails.Projectid);


        //        //var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateLitigation, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        var result = _invoiceCalculation.SaveOrUpdateLitigation(litigationDetails);
        //        Logger.For(this).Invoice("SaveOrUpdateLitigation-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateLitigation-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public bool SaveOrUpdateArbitrationDetails(PTXboArbitration arbitrationDetails)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateArbitrationDetails-API  reached " + ((object)arbitrationDetails).ToJson(false));
        //        //parameters.Add("@ArbitrationDetailsId	", arbitrationDetails.ArbitrationDetailsId);
        //        //parameters.Add("@ArbitrationId	", arbitrationDetails.ArbitrationId);
        //        //parameters.Add("@RequestDate	", arbitrationDetails.RequestDate);
        //        //parameters.Add("@RequestedByAgent	", arbitrationDetails.RequestedByAgent);
        //        //parameters.Add("@InitialArbitrationDeadLineDate	", arbitrationDetails.InitialArbitrationDeadLineDate);
        //        //parameters.Add("@FinalArbitrationDeadLineDate	", arbitrationDetails.FinalArbitrationDeadLineDate);
        //        //parameters.Add("@Approved	", arbitrationDetails.Approved);
        //        //parameters.Add("@ApprovedByAgent	", arbitrationDetails.ApprovedByAgent);
        //        //parameters.Add("@TargetValue	", arbitrationDetails.TargetValue);
        //        //parameters.Add("@AssignedAgentId	", arbitrationDetails.AssignedAgentId);
        //        //parameters.Add("@DeniedReasonId	", arbitrationDetails.DeniedReasonId);
        //        //parameters.Add("@ArbitrationOrder	", arbitrationDetails.ArbitrationOrder);
        //        //parameters.Add("@IndexedArbitrationPackageDocumentId	", arbitrationDetails.IndexedArbitrationPackageDocumentId);
        //        //parameters.Add("@GeoId	", arbitrationDetails.GeoId);
        //        //parameters.Add("@ComptrollerLetterDate	", arbitrationDetails.ComptrollerLetterDate);
        //        //parameters.Add("@FilingDate	", arbitrationDetails.FilingDate);
        //        //parameters.Add("@EvidenceDueDate	", arbitrationDetails.EvidenceDueDate);
        //        //parameters.Add("@CADEvidenceReceivedDate	", arbitrationDetails.CADEvidenceReceivedDate);
        //        //parameters.Add("@EvidenceSentDate	", arbitrationDetails.EvidenceSentDate);
        //        //parameters.Add("@RebuttalEvidenceDueDate	", arbitrationDetails.RebuttalEvidenceDueDate);
        //        //parameters.Add("@CADRebuttalEvidenceReceivedDate	", arbitrationDetails.CADRebuttalEvidenceReceivedDate);
        //        //parameters.Add("@RebuttalEvidenceSentDate	", arbitrationDetails.RebuttalEvidenceSentDate);
        //        //parameters.Add("@CADAppraiser	", arbitrationDetails.CADAppraiser);
        //        //parameters.Add("@ArbitratorName	", arbitrationDetails.ArbitratorName);
        //        //parameters.Add("@ArbitratorAddress	", arbitrationDetails.ArbitratorAddress);
        //        //parameters.Add("@ArbitratorPhone	", arbitrationDetails.ArbitratorPhone);
        //        //parameters.Add("@ArbitratorFax	", arbitrationDetails.ArbitratorFax);
        //        //parameters.Add("@ArbitratorEmail	", arbitrationDetails.ArbitratorEmail);
        //        //parameters.Add("@SettlementDate	", arbitrationDetails.SettlementDate);
        //        //parameters.Add("@FinalAmount	", arbitrationDetails.FinalAmount);
        //        //parameters.Add("@ArbitrationStatusId	", arbitrationDetails.ArbitrationStatusId);
        //        //parameters.Add("@OwnerOrAgnetNumber	", arbitrationDetails.OwnerOrAgnetNumber);
        //        //parameters.Add("@HearingDate	", arbitrationDetails.HearingDate);
        //        //parameters.Add("@HearingTime	", arbitrationDetails.HearingTime);
        //        //parameters.Add("@BatchPrintDate	", arbitrationDetails.BatchPrintDate);
        //        //parameters.Add("@Photos	", arbitrationDetails.Photos);
        //        //parameters.Add("@Finalized	", arbitrationDetails.Finalized);
        //        //parameters.Add("@InvoiceId	", arbitrationDetails.InvoiceId);
        //        //parameters.Add("@ArbitrationAgent	", arbitrationDetails.ArbitrationAgent);
        //        //parameters.Add("@ArbitrationRemarks	", arbitrationDetails.ArbitrationRemarks);
        //        //parameters.Add("@RefferalMethodID	", arbitrationDetails.RefferalMethodID);
        //        //parameters.Add("@Status	", arbitrationDetails.Status);
        //        //parameters.Add("@FeePaidByOCA	", arbitrationDetails.FeePaidByOCA);
        //        //parameters.Add("@FeePaidByClient	", arbitrationDetails.FeePaidByClient);
        //        //parameters.Add("@WelcomeLetterDate	", arbitrationDetails.WelcomeLetterDate);
        //        //parameters.Add("@ProblemAofAId	", arbitrationDetails.ProblemAofAId);
        //        //parameters.Add("@PropertyTaxYear	", arbitrationDetails.PropertyTaxYear);
        //        //parameters.Add("@PropertyTaxAccID	", arbitrationDetails.PropertyTaxAccID);
        //        //parameters.Add("@UpdatedBy	", arbitrationDetails.UpdatedBy);
        //        //parameters.Add("@UpdatedDatetime	", arbitrationDetails.UpdatedDatetime);
        //        //parameters.Add("@CADLegalFirstName	", arbitrationDetails.CADLegalFirstName);
        //        //parameters.Add("@CADLegalLastName	", arbitrationDetails.CADLegalLastName);
        //        //parameters.Add("@MiddleInitial	", arbitrationDetails.MiddleInitial);
        //        //parameters.Add("@Suffix	", arbitrationDetails.Suffix);
        //        //parameters.Add("@IsAgreementonFile	", arbitrationDetails.IsAgreementonFile);
        //        //parameters.Add("@OCALucTypeId	", arbitrationDetails.OCALucTypeId);
        //        //parameters.Add("@ARBInvoiceStatusid	", arbitrationDetails.ARBInvoiceStatusid);
        //        //parameters.Add("@InitialHearingDate	", arbitrationDetails.InitialHearingDate);
        //        //parameters.Add("@InitialHearingTime	", arbitrationDetails.InitialHearingTime);
        //        //parameters.Add("@WithdrawalDeadlineDate	", arbitrationDetails.WithdrawalDeadlineDate);
        //        //parameters.Add("@MarketValueTarget	", arbitrationDetails.MarketValueTarget);
        //        //parameters.Add("@UnequalAppraisalValueTarget	", arbitrationDetails.UnequalAppraisalValueTarget);
        //        //parameters.Add("@SoftTarget	", arbitrationDetails.SoftTarget);
        //        //parameters.Add("@MarketValue	", arbitrationDetails.MarketValue);
        //        //parameters.Add("@ArbitratorLocation	", arbitrationDetails.ArbitratorLocation);
        //        //parameters.Add("@WithdrawalDate	", arbitrationDetails.WithdrawalDate);
        //        //parameters.Add("@FileFeeAmount	", arbitrationDetails.FileFeeAmount);
        //        //parameters.Add("@FileFeeChequeNumber	", arbitrationDetails.FileFeeChequeNumber);
        //        //parameters.Add("@RefundsReceivedDate	", arbitrationDetails.RefundsReceivedDate);
        //        //parameters.Add("@RefundAmount	", arbitrationDetails.RefundAmount);
        //        //parameters.Add("@ChequeNumber	", arbitrationDetails.ChequeNumber);
        //        //parameters.Add("@Rescheduling	", arbitrationDetails.Rescheduling);
        //        //parameters.Add("@EvidenceCreated	", arbitrationDetails.EvidenceCreated);
        //        //parameters.Add("@SoftTargetUpdatedAgentId	", arbitrationDetails.SoftTargetUpdatedAgentId);
        //        //parameters.Add("@MarketValueUpdatedAgentId	", arbitrationDetails.MarketValueUpdatedAgentId);
        //        //parameters.Add("@ExemptInvoice	", arbitrationDetails.ExemptInvoice);
        //        //parameters.Add("@ArbitratorId	", arbitrationDetails.ArbitratorId);
        //        //parameters.Add("@IntialReviewGoodUE	", arbitrationDetails.IntialReviewGoodUE);
        //        //parameters.Add("@IntialReviewGoodSales	", arbitrationDetails.IntialReviewGoodSales);
        //        //parameters.Add("@OCAConfRoom	", arbitrationDetails.OCAConfRoom);
        //        //parameters.Add("@BACadConfLtrDt	", arbitrationDetails.BACadConfLtrDt);
        //        //parameters.Add("@HNRcvdDate	", arbitrationDetails.HNRcvdDate);
        //        //parameters.Add("@SettlementOffer	", arbitrationDetails.SettlementOffer);
        //        //parameters.Add("@SettlementOfferDate	", arbitrationDetails.SettlementOfferDate);
        //        //parameters.Add("@TaxesPaid	", arbitrationDetails.TaxesPaid);
        //        //parameters.Add("@AwardReceiptDate	", arbitrationDetails.AwardReceiptDate);
        //        //parameters.Add("@AssignedAgentAlternateId	", arbitrationDetails.AssignedAgentAlternateId);

        //        //var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateArbitrationDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        var result = _invoiceCalculation.SaveOrUpdateArbitrationDetails(arbitrationDetails);
        //        Logger.For(this).Invoice("SaveOrUpdateArbitrationDetails-API  ends successfully ");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateArbitrationDetails-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}


        public bool SubmitForInvoiceGeneration(int currentUserId, int currentUserRoleID, PTXboSpecialTermInvoiceDetails objdetails, out string errorMessage)
        {
            errorMessage = string.Empty;

           // bool Issucess = true;

            //List<PTXboInvoiceDetails> lstInvoiceDetails = new List<PTXboInvoiceDetails>();

            //PTXboInvoiceDetails invoiceDetails = new PTXboInvoiceDetails();
            //Logger.For(this).Invoice("SubmitForInvoiceGeneration-API  reached " + ((object)objdetails).ToJson(false));
            //foreach (var item in objdetails.lstTermAccounts)
            //{
            //    var YearlyHearingDetails = GetYearlyHearingDetails(item.YearlyHearingDetailsId);

            //    int projectid = 0;
            //    if (YearlyHearingDetails.ProjectId != 0)
            //    {
            //        projectid = YearlyHearingDetails.ProjectId;
            //    }

            //    invoiceDetails = new PTXboInvoiceDetails();
            //    invoiceDetails.InvoiceTypeId = objdetails.TermType;
            //    invoiceDetails.ClientId = Convert.ToInt32(YearlyHearingDetails.ClientId);
            //    invoiceDetails.Taxyear = Convert.ToInt32(YearlyHearingDetails.TaxYear);
            //    invoiceDetails.InvoiceId = Convert.ToInt32(item.InvoiceId);
            //    invoiceDetails.updatedBy = currentUserId;
            //    invoiceDetails.ManualGeneratedUseriD = currentUserId;
            //    invoiceDetails.ManualGeneratedUserRoleID = currentUserRoleID;
            //    invoiceDetails.InvoiceTypeId = objdetails.TermType;
            //    invoiceDetails.ProjectId = projectid;
            //    lstInvoiceDetails.Add(invoiceDetails);


            //    PTXboInvoice objInvoiceFromHearingResult = new PTXboInvoice();
            //    objInvoiceFromHearingResult.ClientId = invoiceDetails.ClientId;
            //    invoiceDetails.Taxyear = invoiceDetails.Taxyear;
            //    objInvoiceFromHearingResult.CreatedBy =currentUserId;
            //    objInvoiceFromHearingResult.InvoiceID = invoiceDetails.InvoiceId;
            //    objInvoiceFromHearingResult.InvoiceTypeId = objdetails.TermType;
            //    objInvoiceFromHearingResult.ProjectId = invoiceDetails.ProjectId;
            //    objInvoiceFromHearingResult.ContingencyPercentage = Convert.ToSingle(objdetails.Contigency);
            //    objInvoiceFromHearingResult.FlatFee = objdetails.FlatFee;
            //    objInvoiceFromHearingResult.InvoiceAmount = objdetails.InvoiceAmount;
            //    objInvoiceFromHearingResult.IsSpecialTerm = true;
            //    objInvoiceFromHearingResult.CanGenerateInvoice = objdetails.CanGenerateInvoice;
            //    objInvoiceFromHearingResult.DontGenerateInvoiceFlag = objdetails.DontGenerateInvoiceFlag;
            //    switch (objdetails.TermType)
            //    {
            //        case (int)Enumerators.PTXenumTermsType.Standard:
            //            objInvoiceFromHearingResult.HearingResultId = item.ID;
            //            break;
            //        case (int)Enumerators.PTXenumTermsType.Litigation:
            //            objInvoiceFromHearingResult.LitigationId = item.ID;

            //            break;
            //        case (int)Enumerators.PTXenumTermsType.Arbritration:
            //            objInvoiceFromHearingResult.ArbitrationDetilId = item.ID;
            //            break;
            //    }

            //    if (objdetails.TermType != (int)Enumerators.PTXenumTermsType.Standard)
            //    {
                    
            //        var hearingresult = GetHearingResultByType(YearlyHearingDetails.YearlyHearingDetailsId,9);
                    
            //        if (hearingresult != null)
            //        {
            //            objInvoiceFromHearingResult.HearingResultId = hearingresult.HearingResultId;
            //        }
            //    }

            //    objInvoiceFromHearingResult.TaxYear = invoiceDetails.Taxyear;
            //   Issucess = InsertInvoiceData(objInvoiceFromHearingResult, out  errorMessage); 

            //    //Added by Boopathi
            //    //Update The Invoice Status as 'DoNotGenerate Invoice'
            //    if (String.IsNullOrEmpty(errorMessage))
            //    {
            //        switch (objdetails.TermType)
            //        {
            //            case (int)Enumerators.PTXenumTermsType.Standard:
            //                //Set DontGenerate Flag For HRInvoiceStatusField
            //                if (objInvoiceFromHearingResult.DontGenerateInvoiceFlag)
            //                {
            //                    var obj = GetHearingResultByType(objInvoiceFromHearingResult.HearingResultId);
            //                    //Repository<PTXdoHearingResult>.GetQuery().FirstOrDefault(x => x.HearingResultsId == objInvoiceFromHearingResult.HearingResultId);
            //                    if (obj != null)
            //                    {
            //                        obj.HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.DoNotGenerateInvoice.GetId();
            //                        SaveOrUpdateHearingResult(obj);
                                   
            //                    }
            //                }
            //                break;
            //            case (int)Enumerators.PTXenumTermsType.Litigation:
            //                //Set DontGenerate Flag For LitigationInvoiceStatusField
            //                if (objInvoiceFromHearingResult.DontGenerateInvoiceFlag)
            //                {
            //                    var obj = GetLitigation(objInvoiceFromHearingResult.LitigationId);
            //                    //Repository<PTXboLitigation>.GetQuery().FirstOrDefault(x => x.LitigationID == objInvoiceFromHearingResult.LitigationId);
            //                    if (obj != null)
            //                    {
            //                        obj.LitigationStatusID = PTXdoenumHRInvoiceStatus.DoNotGenerateInvoice.GetId();
            //                        SaveOrUpdateLitigation(obj);
                                   
            //                    }
            //                }
            //                break;
            //            case (int)Enumerators.PTXenumTermsType.Arbritration:
            //                //Set DontGenerate Flag For ArbritrationInvoiceStatusField
            //                if (objInvoiceFromHearingResult.DontGenerateInvoiceFlag)
            //                {
            //                    var obj = GetArbitrationDetails(objInvoiceFromHearingResult.ArbitrationDetilId);
            //                    //Repository<PTXboArbitrationDetails>.GetQuery().FirstOrDefault(x => x.ArbitrationDetailsId == objInvoiceFromHearingResult.ArbitrationDetilId);
            //                    if (obj != null)
            //                    {
            //                        obj.ArbitrationStatusId = PTXdoenumHRInvoiceStatus.DoNotGenerateInvoice.GetId();
                                    
            //                        SaveOrUpdateArbitrationDetails(obj);
            //                    }
            //                }
            //                break;
            //        }
                    
            //    }
            //}
            Logger.For(this).Invoice("SubmitForInvoiceGeneration-API  ends successfully ");
            //return Issucess;
            return _invoiceCalculation.SubmitForInvoiceGeneration(currentUserId, currentUserRoleID, objdetails, out errorMessage);
        }

        //public PTXboInvoice GetInvoiceDetailsForTermLevel(int invoiceGroupID,int invoiceGroupingTypeID,int invoiceTypeID,int invoiceStatusID,int taxYear)
        //{
        //    try
        //    {
        //        Logger.For(this).Invoice("GetInvoiceDetailsForTermLevel-API  reached " + ((object)"invoiceGroupID="+ invoiceGroupID.ToString()+ "invoiceGroupingTypeID="+ invoiceGroupingTypeID.ToString() + "invoiceTypeID="+ invoiceTypeID.ToString() + "invoiceStatusID="+ invoiceStatusID.ToString() + "taxYear="+ taxYear.ToString()).ToJson(false));
        //        //Hashtable parameters = new Hashtable();
        //        PTXboInvoice invoiceDetails = new PTXboInvoice();
        //        //parameters.Add("@GroupID", invoiceGroupID);
        //        //parameters.Add("@InvoiceGroupingTypeID", invoiceGroupingTypeID);
        //        //parameters.Add("@InvoiceTypeID", invoiceTypeID);
        //        //parameters.Add("@InvoiceStatusID", invoiceStatusID);
        //        //parameters.Add("@TaxYear", taxYear);
        //        //invoiceDetails = _dapperConnection.Select<PTXboInvoice>(StoredProcedureNames.usp_get_InvoicedetailsForTermLevel, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        invoiceDetails = _invoiceCalculation.GetInvoiceDetailsForTermLevel(invoiceGroupID,  invoiceGroupingTypeID,  invoiceTypeID,  invoiceStatusID,  taxYear);
        //        Logger.For(this).Invoice("GetInvoiceDetailsForTermLevel-API  ends successfully ");
        //        return invoiceDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetInvoiceDetailsForTermLevel-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        //public PTXboInvoice GetInvoiceDetailsForProjectLevel(int invoiceGroupID, int invoiceGroupingTypeID,int projectID, int invoiceTypeID, int invoiceStatusID, int taxYear)
        //{
        //    try
        //    {
        //        Logger.For(this).Invoice("GetInvoiceDetailsForProjectLevel-API  reached " + ((object)"invoiceGroupID="+ invoiceGroupID.ToString() + "invoiceGroupingTypeID="+invoiceGroupingTypeID.ToString() + "projectID="+ projectID.ToString() + "invoiceTypeID="+ invoiceTypeID.ToString() + "invoiceStatusID="+ invoiceStatusID.ToString() + "taxYear="+ taxYear.ToString()).ToJson(false));
        //        //Hashtable parameters = new Hashtable();
        //        PTXboInvoice invoiceDetails = new PTXboInvoice();
        //        //parameters.Add("@ProjectID", projectID);
        //        //parameters.Add("@GroupID", invoiceGroupID);
        //        //parameters.Add("@InvoiceGroupingTypeID", invoiceGroupingTypeID);
        //        //parameters.Add("@InvoiceTypeID", invoiceTypeID);
        //        //parameters.Add("@InvoiceStatusID", invoiceStatusID);
        //        //parameters.Add("@TaxYear", taxYear);
        //        //invoiceDetails = _dapperConnection.Select<PTXboInvoice>(StoredProcedureNames.usp_get_InvoicedetailsForProjectLevel, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
        //        invoiceDetails = _invoiceCalculation.GetInvoiceDetailsForProjectLevel(invoiceGroupID, invoiceGroupingTypeID, projectID, invoiceTypeID, invoiceStatusID, taxYear);
        //        Logger.For(this).Invoice("GetInvoiceDetailsForProjectLevel-API  ends successfully ");
        //        return invoiceDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetInvoiceDetailsForProjectLevel-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}

        #region  Invoice generation with validation
        /// <summary>
        /// This method is used to validate and insert invoice record in Account level , Project level and Term level from HearingResult finalized
        /// </summary>
        /// <param name="objInvoiceGenerationData"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>        
        //public bool InsertInvoiceData(PTXboInvoice objInvoiceFromHearingResult, out string errorMessage)
        //{
        //    errorMessage = string.Empty;
        //    int HearingResultID = objInvoiceFromHearingResult.HearingResultId;
        //    int TaxBillAuditId = objInvoiceFromHearingResult.TaxBillAuditId;
        //    int BppRenditionId = objInvoiceFromHearingResult.BppRenditionId;
        //    List<PTXboInvoice> lstInvoiceDetails = new List<PTXboInvoice>();
        //    List<PTXboInvoice> objExceptValueHearingType = new List<PTXboInvoice>();
        //    bool isInvoiceRecordCreated = false;
        //    short valueHearingType = Enumerators.PTXenumHearingType.ValueHearing.GetId();
        //    string hearingType = string.Empty;
        //    string invoicingGroupType = string.Empty;

        //    try
        //    {
        //        Logger.For(this).Invoice("InsertInvoiceData-API  reached " + ((object)objInvoiceFromHearingResult).ToJson(false));
        //        // Edited K.Selva 
        //        if (objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //        {
        //            /** Get the Hearing Type **/
        //            var objHearingType = GetHearingResultByType(HearingResultID); 
        //            hearingType = (objHearingType!=null)? objHearingType.HearingTypeId.ToString():"";
        //        }

        //        /*Getting Account, group, term details based on the client, tax year for the standard term*/
        //        lstInvoiceDetails= _invoiceCalculation.GetInvoiceGenerationInputData(objInvoiceFromHearingResult.ClientId, objInvoiceFromHearingResult.TaxYear, objInvoiceFromHearingResult.InvoiceTypeId);
        //        List<PTXboInvoice> objInvoiceDetails = new List<PTXboInvoice>();
        //        // K.Selva - For Getting BPPRenditionID  from Invoice Details //starts

        //        //Added By Boopathi.S --Checks Null Condition
        //        if (lstInvoiceDetails != null)
        //        {
        //            if (objInvoiceFromHearingResult.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
        //            {
        //                objInvoiceDetails = lstInvoiceDetails.Where(a => a.BppRenditionId == BppRenditionId).ToList();
        //            }
        //            //For Getting TaxBillAuditID  from Invoice Details
        //            else if (objInvoiceFromHearingResult.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //            {
        //                objInvoiceDetails = lstInvoiceDetails.Where(a => a.TaxBillAuditId == TaxBillAuditId).ToList();
        //            }
        //            else
        //            {
        //                /*Select the hearing finalized record*/
        //                objInvoiceDetails = lstInvoiceDetails.Where(a => a.HearingResultId == HearingResultID).ToList();
        //            }
        //        }
        //        // Ends
        //        if (objInvoiceDetails.Count > 0)
        //        {
        //            foreach (PTXboInvoice objInvoice in objInvoiceDetails)
        //            {
        //                objInvoice.InvoiceID = (objInvoiceFromHearingResult.IsRegeneateInvoice == 1) ? 0 : objInvoice.InvoiceID; //Product Backlog Item 34480:Spartaxx | DE - Invoice re-generation - Hearing finalized                      
        //                objInvoice.CanGenerateInvoice = objInvoiceFromHearingResult.CanGenerateInvoice;
        //                objInvoice.IsSpecialTerm = objInvoiceFromHearingResult.IsSpecialTerm;
        //                objInvoice.DontGenerateInvoiceFlag = objInvoiceFromHearingResult.DontGenerateInvoiceFlag;
        //                if (objInvoice.IsSpecialTerm)
        //                {
        //                    objInvoice.InvoiceAmount = objInvoiceFromHearingResult.InvoiceAmount;
        //                    objInvoice.ContingencyPercentage = objInvoiceFromHearingResult.ContingencyPercentage;
        //                    objInvoice.FlatFee = objInvoiceFromHearingResult.FlatFee;
        //                }
        //                if (objInvoiceFromHearingResult.IsSpecialTerm && objInvoiceFromHearingResult.InvoiceID > 0)
        //                    objInvoice.InvoiceID = (objInvoiceFromHearingResult.IsRegeneateInvoice == 1) ? 0 : objInvoiceFromHearingResult.InvoiceID; //Product Backlog Item 34480:Spartaxx | DE - Invoice re-generation - Hearing finalized                        

        //                if (objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.GetId())
        //                {
        //                    invoicingGroupType = Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString();
        //                    isInvoiceRecordCreated = _invoiceCalculation.InsertAccountLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult.CreatedBy).IsInvoiceRecordCreated;
        //                }

        //                else if (objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
        //                {
        //                    invoicingGroupType = Enumerators.PTXenumInvoiceGroupingType.TermLevel.ToString();

        //                    //Added by Kishore to check whether Invoice already exist to Pervent Duplicate Invoices while Updating
        //                    if (objInvoice.IsSpecialTerm)
        //                    {
        //                        //Product Backlog Item 34480:Spartaxx | DE - Invoice re-generation - Hearing finalized
        //                        if (objInvoiceFromHearingResult.IsRegeneateInvoice == 1)
        //                        {
        //                            objInvoice.InvoiceID = 0;
        //                        }

        //                        else
        //                        {
        //                            var Invoice = GetInvoiceDetailsForTermLevel(objInvoice.GroupId, objInvoice.InvoiceGroupingTypeId, objInvoice.InvoiceTypeId, Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId(), objInvoice.TaxYear);
        //                            if (Invoice != null)
        //                                objInvoice.InvoiceID = Invoice.InvoiceID;
        //                        }
        //                    }

        //                    if (objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //                    {
        //                        //For Other HearingType consider those accounts as Account level and insert an Invoice.
        //                        objExceptValueHearingType = lstInvoiceDetails.Where(h => h.GroupId == objInvoice.GroupId && h.HearingTypeId != valueHearingType && h.HearingFinalized == true).ToList();
        //                        if (objExceptValueHearingType.Count > 0)
        //                        {
        //                            isInvoiceRecordCreated = _invoiceCalculation.InsertAccountLevelInvoiceGeneration(objInvoice, objExceptValueHearingType, hearingType, invoicingGroupType, objInvoiceFromHearingResult.CreatedBy).IsInvoiceRecordCreated;
        //                        }

        //                        //Value Hearing type Invoice Generation
        //                        lstInvoiceDetails = lstInvoiceDetails.Where(a => a.GroupId == objInvoice.GroupId && a.HearingTypeId == valueHearingType).ToList();
        //                        if (lstInvoiceDetails.Count > 0)
        //                        {
        //                            isInvoiceRecordCreated = InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult, out errorMessage);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        lstInvoiceDetails = lstInvoiceDetails.Where(a => a.GroupId == objInvoice.GroupId).ToList();
        //                        if (lstInvoiceDetails.Any())
        //                            isInvoiceRecordCreated = InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult, out errorMessage);
        //                    }

        //                }

        //                else if (objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
        //                {
        //                    invoicingGroupType = Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.ToString();
        //                    //Added by Kishore to check whether Invoice already exist to Pervent Duplicate Invoices while Updating
        //                    if (objInvoice.IsSpecialTerm)
        //                    {
        //                        //Product Backlog Item 34480:Spartaxx | DE - Invoice re-generation - Hearing finalized
        //                        if (objInvoiceFromHearingResult.IsRegeneateInvoice == 1)
        //                        {
        //                            objInvoice.InvoiceID = 0;
        //                        }

        //                        else
        //                        {
        //                            var Invoice = GetInvoiceDetailsForProjectLevel(objInvoice.GroupId, objInvoice.InvoiceGroupingTypeId, objInvoice.ProjectId, objInvoice.InvoiceTypeId, Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId(), objInvoice.TaxYear);
        //                            if (Invoice != null)
        //                                objInvoice.InvoiceID = Invoice.InvoiceID;
        //                        }
        //                    }

        //                    //For Other HearingType consider those accounts as Account level and insert an Invoice.
        //                    if (objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //                    {
        //                        objExceptValueHearingType = lstInvoiceDetails.Where(h => h.ProjectId == objInvoice.ProjectId && h.HearingTypeId != valueHearingType && h.HearingFinalized == true).ToList();
        //                        if (objExceptValueHearingType.Count > 0)
        //                        {
        //                            isInvoiceRecordCreated = _invoiceCalculation.InsertAccountLevelInvoiceGeneration(objInvoice, objExceptValueHearingType, hearingType, invoicingGroupType, objInvoiceFromHearingResult.CreatedBy).IsInvoiceRecordCreated;
        //                        }

        //                        //Value Hearing type Invoice Generation
        //                        lstInvoiceDetails = lstInvoiceDetails.Where(a => a.ProjectId == objInvoice.ProjectId && a.HearingTypeId == valueHearingType).ToList();
        //                        if (lstInvoiceDetails.Count > 0)
        //                        {
        //                            isInvoiceRecordCreated = InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult, out errorMessage);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        lstInvoiceDetails = lstInvoiceDetails.Where(a => a.ProjectId == objInvoice.ProjectId).ToList();
        //                        if (lstInvoiceDetails.Count > 0)
        //                            isInvoiceRecordCreated = InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult, out errorMessage);
        //                    }
        //                }
        //            }
        //        }
        //        Logger.For(this).Invoice("InsertInvoiceData-API  ends successfully ");
        //        return isInvoiceRecordCreated;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("InsertInvoiceData-API  error " + ((object)ex).ToJson(false));
        //        throw ex;

        //    }
        //}

        #endregion

        //public bool InsertTermOrProjectLevelInvoiceGeneration(PTXboInvoice objInvoice, List<PTXboInvoice> lstInvoiceDetails, string hearingType, string invoicingGroupingType, PTXboInvoice objInvoiceFromHearingResult, out string errorMessage)
        //{
        //    //PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
        //    //PTXboInvoice objInsertInvoiceData = new PTXboInvoice();
        //    errorMessage = string.Empty;
        //    //decimal TotalNoticedValue = 0;
        //    //decimal TotalPostHearingValue = 0;
        //    //decimal TotalReduction = 0;
        //    //double TotalPriorYearTaxRate = 0;
        //    //decimal TotalEstimatedTaxSavings = 0;
        //    //decimal? TotalInvoiceAmount = 0;
        //    //decimal Reduction = 0;
        //    //bool IsInvoiceDataValid = false;
        //    //bool IsInvoiceValidationFails = false;
        //    //bool IsReduction = false;
        //    //bool isInvoiceRecordCreated = false;
        //    //int NewlyCreatedInvoiceID = 0;
        //    //int HearingResultFinalizednotFinalizedCount = 0;
        //    //Logger.For(this).Invoice("InsertTermOrProjectLevelInvoiceGeneration-API  reached " + ((object)lstInvoiceDetails).ToJson(false));

        //    //if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
        //    //{
        //    //    HearingResultFinalizednotFinalizedCount = lstInvoiceDetails.Where(a => a.BppRenditionId == 0).ToList().Count;
        //    //}
        //    //else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //    //{
        //    //    HearingResultFinalizednotFinalizedCount = lstInvoiceDetails.Where(a => a.TaxBillAuditId == 0).ToList().Count;
        //    //}
        //    //else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
        //    //{
        //    //    HearingResultFinalizednotFinalizedCount = lstInvoiceDetails.Where(a => a.HearingFinalized == false || a.FinalAssessedValue == 0).ToList().Count;
        //    //}
        //    //else
        //    //{
        //    //    HearingResultFinalizednotFinalizedCount = lstInvoiceDetails.Where(a => a.HearingFinalized == false).ToList().Count;
        //    //}


        //    //if (HearingResultFinalizednotFinalizedCount > 0 && objInvoice.IsSpecialTerm != true)
        //    //{
        //    //    //SubmitinvoiceSummary
        //    //    if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
        //    //    {
        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingArbitrationresultsforotheraccountsinthisTerm.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    }
        //    //    else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
        //    //    {
        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingLitigationresultsforotheraccountsinthisTerm.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    }
        //    //    else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
        //    //    {
        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingBPPforotheraccountsinthisTerm.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    }
        //    //    else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //    //    {
        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingTaxBillforotheraccountsinthisTerm.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    }
        //    //    else
        //    //    {
        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingHearingResultsForOtherAccountsInthisTerm.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //    }
        //    //    _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
        //    //    IsInvoiceDataValid = false;
        //    //}
        //    //else
        //    //{
        //    //    //Added by KishoreKumar to avoid Inserting accounts which are not Qualified for invoicing.
        //    //    if (objInvoice.IsSpecialTerm)
        //    //        lstInvoiceDetails = lstInvoiceDetails.Where(q => q.HearingFinalized == true).ToList();

        //    //    foreach (PTXboInvoice objInvoiceDataCalculation in lstInvoiceDetails)
        //    //    {
        //    //        if (_invoiceCalculation.ValidateInvoiceDetails(objInvoiceDataCalculation, out errorMessage))
        //    //        {
        //    //            if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
        //    //            {
        //    //                if (objInvoice.FlatFee > 0 && objInvoice.FlatFee != null)
        //    //                {
        //    //                    objInsertInvoiceData = objInvoiceDataCalculation;
        //    //                    objInsertInvoiceData.InvoiceAmount = Convert.ToDecimal(objInvoiceDataCalculation.FlatFee);
        //    //                    IsInvoiceDataValid = true;
        //    //                }
        //    //                else
        //    //                {
        //    //                    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoicenotrequiredFlatFeenotNoticed.GetId();
        //    //                    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //                    _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
        //    //                    IsInvoiceDataValid = false;
        //    //                    break;
        //    //                }
        //    //            }
        //    //            else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
        //    //            {
        //    //                if (objInvoice.FlatFee > 0 && objInvoice.FlatFee != null)
        //    //                {
        //    //                    objInsertInvoiceData = objInvoiceDataCalculation;
        //    //                    objInsertInvoiceData.InvoiceAmount = Convert.ToDecimal(objInvoiceDataCalculation.FlatFee);
        //    //                    IsInvoiceDataValid = true;
        //    //                }
        //    //                else
        //    //                {
        //    //                    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoicenotrequiredFlatFeenotNoticed.GetId();
        //    //                    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //                    _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
        //    //                    IsInvoiceDataValid = false;
        //    //                    break;
        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                if (objInvoiceDataCalculation.HearingFinalized == true && objInvoiceDataCalculation.InvoiceGenerated == false)
        //    //                {
        //    //                    TotalNoticedValue = TotalNoticedValue + Convert.ToDecimal(objInvoiceDataCalculation.InitialAssessedValue);
        //    //                    TotalPostHearingValue = TotalPostHearingValue + Convert.ToDecimal(objInvoiceDataCalculation.FinalAssessedValue);
        //    //                    TotalPriorYearTaxRate = TotalPriorYearTaxRate + Convert.ToDouble(objInvoiceDataCalculation.PriorYearTaxRate);
        //    //                    TotalReduction = (TotalNoticedValue - TotalPostHearingValue); //Modified By Pavithra.B on 31Aug2016 - calculating Reduction value from TotalNoticedValue and TotalPostHearingValue                                
        //    //                    Reduction = Convert.ToDecimal(objInvoiceDataCalculation.InitialAssessedValue) - Convert.ToDecimal(objInvoiceDataCalculation.FinalAssessedValue);
        //    //                    if (TotalReduction > 0 || objInvoice.IsSpecialTerm)
        //    //                    {
        //    //                        IsReduction = true;
        //    //                        //objInvoiceDataCalculation.EstimatedTaxSaving = (Convert.ToDecimal(TotalPriorYearTaxRate) / 100) * Convert.ToDecimal((TotalReduction));
        //    //                        //TotalEstimatedTaxSavings = objInvoiceDataCalculation.EstimatedTaxSaving;
        //    //                        objInvoiceDataCalculation.EstimatedTaxSaving = (Convert.ToDecimal(Convert.ToDouble(objInvoiceDataCalculation.PriorYearTaxRate)) / 100) * Convert.ToDecimal((Reduction));
        //    //                        TotalEstimatedTaxSavings = TotalEstimatedTaxSavings + (Convert.ToDecimal(Convert.ToDouble(objInvoiceDataCalculation.PriorYearTaxRate)) / 100) * Convert.ToDecimal((Reduction));
        //    //                        if (objInvoice.IsSpecialTerm)
        //    //                        {
        //    //                            objInvoiceDataCalculation.ContingencyFee = Math.Round((TotalEstimatedTaxSavings) * Convert.ToDecimal((objInvoice.ContingencyPercentage)), 2);
        //    //                            objInvoiceDataCalculation.FlatFee = objInvoice.FlatFee.GetValueOrDefault();
        //    //                            //Modified By Pavithra.B on 3Nov2016 - TFS Id : 26636
        //    //                            objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
        //    //                            TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
        //    //                            //objInvoiceDataCalculation.InvoiceAmount = objInvoiceDataCalculation.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
        //    //                        }
        //    //                        else
        //    //                        {
        //    //                            objInvoiceDataCalculation.ContingencyFee = Math.Round((TotalEstimatedTaxSavings) * Convert.ToDecimal((objInvoiceDataCalculation.ContingencyPercentage)), 2);
        //    //                            if ((objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() ||
        //    //                                objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
        //    //                                && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
        //    //                            {
        //    //                                List<PTXboInvoiceLineItem> dtInvoiceLineItem = _invoiceCalculation.CheckPosttoFlatFeeInvoicing(objInvoice.GroupId, 3);
        //    //                                if (dtInvoiceLineItem.Count == 0)
        //    //                                {
        //    //                                    objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
        //    //                                    TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
        //    //                                }
        //    //                                else
        //    //                                {
        //    //                                    objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
        //    //                                    TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
        //    //                                }
        //    //                            }
        //    //                            else if ((objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() ||
        //    //                                objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
        //    //                                && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
        //    //                            {
        //    //                                List<PTXboInvoiceLineItem> dtInvoiceLineItem = _invoiceCalculation.CheckPosttoFlatFeeInvoicing(objInvoice.ProjectId, 2);
        //    //                                if (dtInvoiceLineItem.Count == 0)
        //    //                                //if (dtInvoiceLineItem == null)
        //    //                                {
        //    //                                    objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
        //    //                                    TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
        //    //                                }
        //    //                                else
        //    //                                {
        //    //                                    objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
        //    //                                    TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
        //    //                                }
        //    //                            }

        //    //                        }

        //    //                        objInsertInvoiceData = objInvoiceDataCalculation;
        //    //                        if (objInvoice.IsSpecialTerm)
        //    //                        {
        //    //                            objInsertInvoiceData.InvoiceID = objInvoice.InvoiceID;
        //    //                            objInsertInvoiceData.IsSpecialTerm = objInvoice.IsSpecialTerm;
        //    //                        }
        //    //                        //objInsertInvoiceData.EstimatedTaxSaving = objInvoiceDataCalculation.EstimatedTaxSaving;
        //    //                        objInsertInvoiceData.EstimatedTaxSaving = TotalEstimatedTaxSavings;
        //    //                        objInsertInvoiceData.ContingencyFee = objInvoiceDataCalculation.ContingencyFee;
        //    //                        //objInsertInvoiceData.InvoiceAmount = objInvoiceDataCalculation.InvoiceAmount;
        //    //                        objInsertInvoiceData.InvoiceAmount = TotalInvoiceAmount;
        //    //                        objInsertInvoiceData.InitialAssessedValue = TotalNoticedValue;
        //    //                        objInsertInvoiceData.FinalAssessedValue = TotalPostHearingValue;
        //    //                        objInsertInvoiceData.PriorYearTaxRate = TotalPriorYearTaxRate;
        //    //                        objInsertInvoiceData.Reduction = TotalReduction;
        //    //                        IsInvoiceDataValid = true;
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        if (IsReduction)
        //    //                        {
        //    //                            IsInvoiceDataValid = true;
        //    //                        }
        //    //                        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceNotRequiredReductionNotNoticed.GetId();
        //    //                        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //                        _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
        //    //                        IsInvoiceDataValid = false;
        //    //                        //break;
        //    //                    }
        //    //                }
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            objInsertInvoiceData = objInvoiceDataCalculation;
        //    //            IsInvoiceValidationFails = true;
        //    //        }
        //    //    }
        //    //}

        //    //if (invoicingGroupingType == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.ToString())
        //    //    objInsertInvoiceData.InvoiceDescription = objInvoice.ProjectName;
        //    //else if (invoicingGroupingType == Enumerators.PTXenumInvoiceGroupingType.TermLevel.ToString())
        //    //    objInsertInvoiceData.InvoiceDescription = objInvoice.GroupName;

        //    //objInsertInvoiceData.InvoiceDate = DateTime.Now;



        //    //if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
        //    //{
        //    //    objInsertInvoiceData.PaymentDueDate = DateTime.Now.AddDays(90);
        //    //}
        //    //else
        //    //{
        //    //    objInsertInvoiceData.PaymentDueDate = DateTime.Now.AddDays(30);
        //    //}
        //    ////Standard so due date will be invoice date + 30 days

        //    //objInsertInvoiceData.CreatedDateAndTime = DateTime.Now;
        //    //objInsertInvoiceData.AutoGenerated = true;
        //    //List<Int32> lstHearingResultID = _invoiceCalculation.GetDistinctHearingResultIDs(lstInvoiceDetails, objInvoice.InvoiceTypeId);

        //    //if (IsInvoiceDataValid)
        //    //{
        //    //    if (objInvoice.IsSpecialTerm)
        //    //    {
        //    //        objInsertInvoiceData.ContingencyPercentage = objInvoice.ContingencyPercentage;
        //    //        objInsertInvoiceData.FlatFee = objInvoice.FlatFee.GetValueOrDefault();
        //    //        objInsertInvoiceData.DontGenerateInvoiceFlag = objInvoice.DontGenerateInvoiceFlag;
        //    //        objInsertInvoiceData.CanGenerateInvoice = objInvoice.CanGenerateInvoice;
        //    //        //Added By Pavithra.B on 3Nov2016 - FlatFee Calculation Issue. TFS Id : 26636
        //    //        objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount + objInvoice.FlatFee.GetValueOrDefault();
        //    //    }
        //    //    else
        //    //    {
        //    //        if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
        //    //        {
        //    //            List<PTXboInvoiceLineItem> dtInvoiceLineItem = _invoiceCalculation.CheckPosttoFlatFeeInvoicing(objInvoice.GroupId, 3);
        //    //            if (dtInvoiceLineItem.Count == 0)
        //    //            {
        //    //                objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount + objInvoice.FlatFee.GetValueOrDefault();
        //    //            }
        //    //            else
        //    //            {
        //    //                objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount;
        //    //            }
        //    //        }
        //    //        else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
        //    //        {
        //    //            List<PTXboInvoiceLineItem> dtInvoiceLineItem = _invoiceCalculation.CheckPosttoFlatFeeInvoicing(objInvoice.ProjectId, 2);
        //    //            if (dtInvoiceLineItem.Count == 0)
        //    //            {
        //    //                objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount + objInvoice.FlatFee.GetValueOrDefault();
        //    //            }
        //    //            else
        //    //            {
        //    //                objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount;
        //    //            }
        //    //        }
        //    //    }
        //    //    objInsertInvoiceData.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
        //    //    objInsertInvoiceData.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.ReadyForInvoicing.GetId();
        //    //    isInvoiceRecordCreated = _invoiceCalculation.SubmitInvoice(objInsertInvoiceData, lstHearingResultID, out NewlyCreatedInvoiceID, out errorMessage);

        //    //    if (isInvoiceRecordCreated == true && NewlyCreatedInvoiceID != 0)
        //    //    {
        //    //        bool isSuccessCapValue = false;
        //    //        string errormessage = string.Empty;
        //    //        List<PTXboInvoicePaymentMap> lstInvoicePaymentMap;
        //    //        var objInvoiceid = _invoiceCalculation.GetInvoiceDetailsById(NewlyCreatedInvoiceID);
        //    //        if (objInvoiceid != null)
        //    //        {
        //    //            //Pavithra.B - Included Cap Value
        //    //            if (objInsertInvoiceData.CapValue > 0 && Convert.ToDecimal((objInvoice.ContingencyPercentage)) > 0 && Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount) > Convert.ToDecimal(objInsertInvoiceData.CapValue))
        //    //            {
        //    //                lstInvoicePaymentMap = _invoiceCalculation.GetInvoicePaymentMap(NewlyCreatedInvoiceID);
        //    //                if (lstInvoicePaymentMap != null && lstInvoicePaymentMap.Count > 0)
        //    //                {
        //    //                    PTXboPayment Objpayment = new PTXboPayment();
        //    //                    foreach (PTXboInvoicePaymentMap obiInvPay in lstInvoicePaymentMap)
        //    //                    {
        //    //                        var lstPayment = _invoiceCalculation.GetInvoicePayment(obiInvPay.PaymentId, "Cap Value Adjustment"); 
        //    //                        if (lstPayment != null)
        //    //                        {
        //    //                            Objpayment = lstPayment;
        //    //                        }
        //    //                        decimal PaymentAmount = 0;
        //    //                        PaymentAmount = Math.Round((objInvoiceid.InvoiceAmount.GetValueOrDefault()) - Convert.ToDecimal(objInsertInvoiceData.CapValue), 2);
        //    //                        Objpayment.ClientId =  objInsertInvoiceData.ClientId;
        //    //                        Objpayment.InvoicePaymentMethodId =  Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
        //    //                        Objpayment.PaymentAmount = PaymentAmount;
        //    //                        Objpayment.PaymentDescription = "Cap Value Adjustment";
        //    //                        Objpayment.CreatedBy = objInvoiceFromHearingResult.CreatedBy;
        //    //                        Objpayment.CreatedDateTime = DateTime.Now;
        //    //                        Objpayment.UpdatedBy =objInvoiceFromHearingResult.CreatedBy ;
        //    //                        Objpayment.UpdatedDateTime = DateTime.Now;
        //    //                        isSuccessCapValue = _invoiceCalculation.SubmitCapValueAdjustment(Objpayment, NewlyCreatedInvoiceID, out errormessage);
        //    //                        if (isSuccessCapValue)
        //    //                        {
        //    //                            bool isSuccess = _invoiceCalculation.UpdateAmountDueCapValueChange(NewlyCreatedInvoiceID, out errormessage);
        //    //                        }
        //    //                    }
        //    //                }
        //    //                else
        //    //                {
        //    //                    decimal PaymentAmount = 0;
        //    //                    PaymentAmount = Math.Round((objInvoiceid.InvoiceAmount.GetValueOrDefault()) - Convert.ToDecimal(objInsertInvoiceData.CapValue), 2);
        //    //                    PTXboPayment Objpayment = new PTXboPayment();
        //    //                    Objpayment.ClientId = objInsertInvoiceData.ClientId ;
        //    //                    Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId() ;
        //    //                    Objpayment.PaymentAmount = PaymentAmount;
        //    //                    Objpayment.PaymentDescription = "Cap Value Adjustment";
        //    //                    Objpayment.CreatedBy =  objInvoiceFromHearingResult.CreatedBy ;
        //    //                    Objpayment.CreatedDateTime = DateTime.Now;
        //    //                    Objpayment.UpdatedBy = objInvoiceFromHearingResult.CreatedBy;
        //    //                    Objpayment.UpdatedDateTime = DateTime.Now;
        //    //                    isSuccessCapValue = _invoiceCalculation.SubmitCapValueAdjustment(Objpayment, NewlyCreatedInvoiceID, out errormessage);
        //    //                    if (isSuccessCapValue)
        //    //                    {
        //    //                        bool isSuccess = _invoiceCalculation.UpdateAmountDueCapValueChange(NewlyCreatedInvoiceID, out errormessage);
        //    //                    }
        //    //                }
        //    //            }
        //    //            else if (Convert.ToDecimal(objInvoiceid.AmountAdjusted) != 0)
        //    //            {
        //    //                _invoiceCalculation.RemoveCapvalueAdjustment(objInvoiceid.InvoiceID);
        //    //                bool isSuccess = _invoiceCalculation.UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errormessage);
        //    //            }
        //    //        }

        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ReadyForInvoicing.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();

        //    //        invoiceSummary.InvoiceID = NewlyCreatedInvoiceID;
        //    //        _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);

        //    //        //Need to insert into Invoice Remarks
        //    //        PTXboInvoiceRemarks objInvoiceRemarks = new PTXboInvoiceRemarks();
        //    //        objInvoiceRemarks.InvoiceID = NewlyCreatedInvoiceID;
        //    //        objInvoiceRemarks.InvoiceRemarks = "Invoice created for Hearing Type : " + hearingType + " and Grouping level : " + invoicingGroupingType;
        //    //        objInvoiceRemarks.UpdatedBy = objInvoiceFromHearingResult.CreatedBy;
        //    //        _invoiceCalculation.InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objInvoiceRemarks, out errorMessage);
        //    //    }
        //    //    else
        //    //    {
        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.TachnicalErrorInInvoiceGeneration.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //        _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);
        //    //    }
        //    //}
        //    //if (IsInvoiceValidationFails)
        //    //{
        //    //    objInsertInvoiceData.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
        //    //    objInsertInvoiceData.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.WaitingInPendingResearchQueue.GetId();
        //    //    isInvoiceRecordCreated = _invoiceCalculation.SubmitInvoice(objInsertInvoiceData, lstHearingResultID, out NewlyCreatedInvoiceID, out errorMessage);
        //    //    if (isInvoiceRecordCreated == true && NewlyCreatedInvoiceID != 0)
        //    //    {
        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.WaitingInPendingResearchQueue.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();

        //    //        invoiceSummary.InvoiceID = NewlyCreatedInvoiceID;
        //    //        _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);

        //    //        //Need to insert into Invoice Remarks
        //    //        PTXboInvoiceRemarks objInvoiceRemarks = new PTXboInvoiceRemarks();
        //    //        objInvoiceRemarks.InvoiceID = NewlyCreatedInvoiceID;
        //    //        objInvoiceRemarks.InvoiceRemarks = "Invoice created for Hearing Type : " + hearingType + " and Grouping level : " + invoicingGroupingType;
        //    //        objInvoiceRemarks.UpdatedBy = objInvoiceFromHearingResult.CreatedBy;
        //    //        _invoiceCalculation.InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objInvoiceRemarks, out errorMessage);
        //    //    }
        //    //    else
        //    //    {
        //    //        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.TachnicalErrorInInvoiceGeneration.GetId();
        //    //        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    //        _invoiceCalculation.InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);
        //    //    }
        //    //}
        //    Logger.For(this).Invoice("InsertTermOrProjectLevelInvoiceGeneration-API  ends successfully ");
        //    //return isInvoiceRecordCreated;
        //    return _invoiceCalculation.InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupingType, objInvoiceFromHearingResult, out errorMessage);
        //}

        //public List<PTXboInvoiceAndHearingResultMap> GetInvoiceAndHearingResultMap(int invoiceID)
        //{
        //    try
        //    {
        //        //Hashtable parameters = new Hashtable();
        //        //parameters.Add("@InvoiceID", invoiceID);
        //        //Logger.For(this).Invoice("GetInvoiceAndHearingResultMap-  reached " + ((object)invoiceID).ToJson(false));
        //        //var result = _dapperConnection.Select<PTXboInvoiceAndHearingResultMap>(StoredProcedureNames.usp_get_InvoiceAndHearingResultMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
        //        var result = _invoiceCalculation.GetInvoiceAndHearingResultMap(invoiceID);
        //        Logger.For(this).Invoice("GetInvoiceAndHearingResultMap-  ends successfully ");
        //        return result;
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetInvoiceAndHearingResultMap-API  error " + ((object)ex).ToJson(false));
        //        throw ex;
        //    }
        //}




        /// <summary>
        /// Created by SaravananS. tfs id:61899
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool GenerateInvoice(PTXboGenerateInvoice_Request request, out string errorMessage)
        {
            Hashtable parameters = new Hashtable();
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-Invoice Calculation-GenerateInvoice-API  reached " + ((object)request).ToJson(false));
                foreach (var invoice in request.SelectedInvoices)
                {
                    try
                    {

                        List<PTXboInvoiceReport> lstInvoiceDetails = new List<PTXboInvoiceReport>();
                        List<PTXboInvoiceAccount> lstInvoiceAccount = new List<PTXboInvoiceAccount>();

                        //Added by SaravananS.tfs id:56033
                        if (request.IsDisasterInvoice)
                        {
                            PTXboInvoiceReportDetailsInput invoiceReportDetailsInput = new PTXboInvoiceReportDetailsInput()
                            {
                                InvoiceId = Convert.ToInt32(invoice),
                                InvoiceTypeId = 1,
                                IsOutOfTexas = false,
                                IsOTEntryscreen = false,
                                IsDisasterInvoice = request.IsDisasterInvoice

                            };

                            PTXboInvoiceReportDetails result = _invoiceCalculation.GetInvoiceReportDetails(invoiceReportDetailsInput);
                            lstInvoiceDetails = result.InvoiceDetails;
                            lstInvoiceAccount = result.InvoiceAccount;
                        }
                        //Ends here.
                        else if (request.IsOTEntryscreen)
                        {
                            PTXboInvoiceReportDetailsInput invoiceReportDetailsInput = new PTXboInvoiceReportDetailsInput()
                            {
                                InvoiceId = Convert.ToInt32(invoice),
                                InvoiceTypeId = 1,
                                IsOutOfTexas = true,
                                IsOTEntryscreen = true,
                                IsDisasterInvoice = request.IsDisasterInvoice

                            };
                            PTXboInvoiceReportDetails result = _invoiceCalculation.GetInvoiceReportDetails(invoiceReportDetailsInput);
                            lstInvoiceDetails = result.InvoiceDetails;
                            lstInvoiceAccount = result.InvoiceAccount;
                        }
                        else
                        {
                            PTXboInvoiceReportDetailsInput invoiceReportDetailsInput = new PTXboInvoiceReportDetailsInput()
                            {
                                InvoiceId = Convert.ToInt32(invoice),
                                InvoiceTypeId = 1
                            };

                            PTXboInvoiceReportDetails result = _invoiceCalculation.GetInvoiceReportDetails(invoiceReportDetailsInput);
                            lstInvoiceDetails = result.InvoiceDetails;
                            lstInvoiceAccount = result.InvoiceAccount;
                        }

                        var objInvoice = lstInvoiceDetails.FirstOrDefault();
                        if (objInvoice != null)
                        {    //Added by SaravananS.tfs id:56933
                            if (!string.IsNullOrEmpty(request.InvoiceDescription))
                            {
                                objInvoice.Description = request.InvoiceDescription;
                            }
                            //Ends here.
                            else if (objInvoice.InvoiceGroupType == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.GetDescription() && !request.IsOTEntryscreen)
                            {
                                objInvoice.Description = objInvoice.PropertyAddress;
                            }

                            Logger.For(this).Invoice("Invoice Calculation-Invoice Calculation-Inside GenerateInvoice -started UpdateInvoiceDetailsReport:objInvoice" + ((object)objInvoice).ToJson(false));

                            UpdateInvoiceDetailsReport(objInvoice);

                            Logger.For(this).Invoice("Invoice Calculation-Inside GenerateInvoice -ended UpdateInvoiceDetailsReport:objInvoice" + ((object)objInvoice).ToJson(false));

                            Logger.For(this).Invoice("Invoice Calculation-started Updating invoicing process status as invoice generated for invoiceid " + objInvoice.InvoiceId);

                            PTXboUpdateInvoiceProcessingStatusInput invoiceProcessingStatus = new PTXboUpdateInvoiceProcessingStatusInput()
                            {
                                ClientID = objInvoice.ClientId,
                                GroupID = objInvoice.GroupId,
                                ProjectID = objInvoice.ProjectId,
                                InvoiceID = objInvoice.InvoiceId,
                                InvoiceProcessingStatusID = Enumerators.PTXenumInvoicingPRocessingStatus.InvoiceGenerated.GetId(),
                                YearlyHearingDetailsID = objInvoice.YealyHearingDetailsId,
                                InvoiceSummaryProcessId = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceGenerated.GetId(),
                                InvoiceGeneartedForID = Enumerators.PTXenumInvoiceGeneratedFor.HearingProcessFees.GetId(),
                                CorrProcessId = Enumerators.PTXenumQueueProcessingStatus.InProgress.GetId()
                            };
                            UpdateInvoiceProcessStatus(invoiceProcessingStatus);
                            Logger.For(this).Invoice("Invoice Calculation-ended Updating invoicing process status as invoice generated for invoiceid " + objInvoice.InvoiceId);
                        }
                        else
                        {
                            Logger.For(this).Invoice("Invoice Calculation-invoice details are empty for invoiceid " + invoice);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.For(this).Invoice("Invoice Calculation-GenerateInvoice- error  selectedInvoices:" + request.SelectedInvoices + " Issue with invoiceid:: " + invoice + " ex:" + ex);
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoice-API  error " + ((object)ex).ToJson(false));
                errorMessage = ex.Message;
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public double GetTaxrateForTheGivenTaxyear(PTXboSpecialTermInvoiceTaxrate taxrateDetails, out string errorMessage)
        {
            Hashtable parameters = new Hashtable();
           double taxrate = 0;
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("GetTaxrateForTheGivenTaxyear-API  reached ".ToJson(false));
                parameters.Add("@TermId", taxrateDetails.TermsId);
                parameters.Add("@TaxYear", taxrateDetails.TaxYear);
                parameters.Add("@TermsTypeId", taxrateDetails.TermsTypeId);
                parameters.Add("@IsCurrentYearTaxrate", taxrateDetails.IsCurrentYearTaxrate);
                parameters.Add("@DecId", taxrateDetails.DecId);
                parameters.Add("@InvoiceLevelId", taxrateDetails.InvoiceLevelId);
                taxrate = Convert.ToDouble(_dapperConnection.ExecuteScalar(StoredProcedureNames.Usp_GetTaxrateForTheGivenTaxyear, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("GetTaxrateForTheGivenTaxyear-API  ends successfully");
                return taxrate;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetTaxrateForTheGivenTaxyear-API  error ".ToJson(false));
                throw ex;
            }
        }



    }
}
