using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common.BusinessObjects
{
    public class PTXdtHearingResults
    {
        public int IndexedNoticeDocumentId { get; set; }
        public string BatchCode { get; set; }
        public int PWImageID { get; set; }
        public int PWFileCabinetID { get; set; }
        public int NoticeDocumentTypeId { get; set; }
        public string NoticedDocumentType { get; set; }
        public int Taxyear { get; set; }
        public int Countyid { get; set; }
        public string CountyName { get; set; }
        public string CountyCode { get; set; }
        public string AccountNumber { get; set; }
        public int ClientId { get; set; }
        public int AccountId { get; set; }
        public int AccountStatusId { get; set; }
        public string Accountstatus { get; set; }
        public int AccountProcessStatusId { get; set; }
        public string AccountProcessStatus { get; set; }
        public int ProtestCodeId { get; set; }
        public string ProtestCodeValues { get; set; }
        public double NoticedLandValue { get; set; }
        public double NoticedImprovedValue { get; set; }
        public double NoticedMarketValue { get; set; }
        public double NoticedTotalvalue { get; set; }

        public double RenoticedLandValue { get; set; }
        public double RenoticedImprovedValue { get; set; }
        public double RenoticedMarketValue { get; set; }
        public double RenoticeTotalvalue { get; set; }

        public bool NoChange { get; set; }
        public double PostHearingLandValue { get; set; }
        public double PostHearingImprovedValue { get; set; }
        public double PostHearingMarketValue { get; set; }
        public double PostHearingTotalValue { get; set; }
        public int HearingAgentId { get; set; }
        public DateTime ConfirmationLetterDate { get; set; }
        public DateTime confirmationLetterReceivedDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public TimeSpan CompletionTime { get; set; }
        public int HearingResolutionId { get; set; }
        public int ReasonCodeId { get; set; }
        public int DismissalAuthStatusId { get; set; }
        public int DefectReasonId { get; set; }
        public int HearingResultReportId { get; set; }
        public bool HearingFinalized { get; set; }
        public DateTime HearingResultsSentOn { get; set; }
        public bool ResultGenerated { get; set; }
        public bool InvoiceGenerated { get; set; }
        public string InvoiceId { get; set; }
        public int DocumentDefectCodeId { get; set; }
        public string DocumentDefectCodes { get; set; }
        public bool DefectNotice { get; set; }
        public int UserEnteredHearingResultsId { get; set; }
        public string EntryAssignBy { get; set; }
        public int EntryAssignedBy { get; set; }
        public string AuditAssignBy { get; set; }
        public string DefectAssignBy { get; set; }
        public string QCAssignBy { get; set; }
        public DateTime EntryAssignOn { get; set; }
        public DateTime AuditAssignOn { get; set; }
        public DateTime DefectAssignOn { get; set; }
        public DateTime QCAssignOn { get; set; }
        public int EntryUserId { get; set; }
        public int DefectAssignedUserId { get; set; }
        public int AuditedUserId { get; set; }
        public bool DefectResolved { get; set; }
        public DateTime DefectRectifiedDateAndTime { get; set; }
        public int AssignedQueueId { get; set; }
        public string NoticeQueue { get; set; }
        public int NoticeProcessingStatusId { get; set; }
        public string NoticeProcessingStatus { get; set; }
        public int HearingStatusId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int QCAssignedUserId { get; set; }
        public string DocumentName { get; set; } //Newly Added
        public string PropertyAddress { get; set; }
        public string DNDcodes { get; set; }
        public bool ExemptInvoice { get; set; }
        public virtual DateTime? ScannedDateAndTime { get; set; }
        public string ClientNumber { get; set; }
        public DateTime? EndDate { get; set; }
        public string Remarks { get; set; }
        //added by Boopathi.S
        public int? HRInvoiceStatusid { get; set; }

        public int YearlyHearingDetails { get; set; } //Newly Added
        public int HearingTrackingCodeId { get; set; }
        public int DisasterId { get; set; }
        public bool IsDisasterProtest { get; set; }
        public int HearingLevelId { get; set; } // Added by Marg

        public bool ByPassInformalHearing { get; set; } // Added by Nandhini.R
        public int? ExemptionId { get; set; }
        public int YearlyHearingDetailsId { get; set; }
        public int DNDCodeId { get; set; }
        public int? PropertyDetailsId { get; set; }
        public int TaxYear { get; set; }
        public DateTime? AffidavitFiledDate { get; set; }
        public decimal? OwnerOpinionValue { get; set; }
        public int AffidavitStatusID { get; set; }
        public int? AffidavitHearingAgentID { get; set; }
        public bool AffidavitDeliveryMethodUSmail { get; set; }
        public bool AffidavitDeliveryMethodInPerson { get; set; }
        public bool AffidavitDeliveryMethodEmail { get; set; }

    }

    public class PTXdtHearingResultsClarification
    {
        public int NoticeClarificationId { get; set; }
        public int IndexedNoticeDocumentId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int RequestedUserId { get; set; }
        public string RequestedUser { get; set; }
        public DateTime RequestedDateAndTime { get; set; }
        public int RespondedUserId { get; set; }
        public string RespondedUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RespondedDateAndTime { get; set; }
        public string RespondedUserFirstName { get; set; }
        public string RespondedUserLastName { get; set; }
        public string RequestedUserFirstName { get; set; }
        public string RequestedUserLastName { get; set; }
        public string ClarificationAssignedBy { get; set; }
        public DateTime ClarificationAssignedOn { get; set; }
    }

    public class PTXdtHearingResultRemarks
    {
        public int UserEnteredHearingResultsRemarksId { get; set; }
        public int UserEnteredHearingResultsId { get; set; }
        public string Remarks { get; set; }
        public int UpdatedBy { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }

    public class PTXdtHearingTypeForResult
    {
        public int UserEnteredHearingTypeId { get; set; }
        public int UserEnteredHearingResultsId { get; set; }
        public int HearingTypeId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }

    public class PTXdtExistingHearingDetailsForResult
    {
        public string CountyName { get; set; }
        public int AccountId { get; set; }
        public int HearingTypeId { get; set; }
        public int HearingDetailsId { get; set; }
        public string HearingType { get; set; }
        public DateTime FormalHearingDate { get; set; }
        public string FormalHearingTime { get; set; }
        public DateTime InformalHearingDate { get; set; }
        public string InformalHearingTime { get; set; }
        public string InformalPanelDocketID { get; set; }
        public string FormalPanelDocketID { get; set; }
        public DateTime EvidenceDueDate { get; set; }
        public int TaxYear { get; set; }
        public int InformalAssignedAgentId { get; set; }
        public int FormalAssignedAgentId { get; set; }
        public DateTime InformalBatchPrintDate { get; set; }
        public DateTime FormalBatchPrintDate { get; set; }
        public double NoticeLandValue { get; set; }
        public double NoticeMarketValue { get; set; }
        public double NoticeImprovedValue { get; set; }
        public double NoticeTotalValue { get; set; }
        public double PostHearingLandValue { get; set; }
        public double PostHearingImprovedValue { get; set; }
        public double PostHearingMarketValue { get; set; }
        public double PostHearingTotalValue { get; set; }
        /// <summary>
        /// Added By :Kowsalya
        /// Added on :Oct 10 2017
        /// Added Desc:To fetch Post value hearing details for correction motion
        /// </summary>
        public decimal PostValueHearingLandValue { get; set; }
        public decimal PostValueHearingImprovedValue { get; set; }
        public decimal PostValueHearingMarketValue { get; set; }
        public decimal PostValueHearingTotalValue { get; set; }
        public int IsCertifiedValues { get; set; }

        public int HearingResolutionId { get; set; }
        public int HearingResultReasonCodeId { get; set; }
        public int DismissalAuthStatusId { get; set; }
        public DateTime? confirmationLetterDateTime { get; set; }
        public DateTime? confirmationLetterReceivedDate { get; set; }
        public DateTime? completionDateAndTime { get; set; }
        public string strconfirmationLetterDateTime { get; set; }
        public string strcompletionDateAndTime { get; set; }
        public bool InvoiceGenerated { get; set; }
        public int HearingStatusId { get; set; }
        public string HearingStatus { get; set; }
        public int HearingAgentId { get; set; }
        public int InvoiceId { get; set; }
        public int HearingResultId { get; set; }
        public bool HearingFinalized { get; set; }
        public int? HearingResultReportId { get; set; }
        public int? InvoiceStatusId { get; set; }
        public DateTime? ResultSenton { get; set; }
        public bool ExemptInvoice { get; set; }

        public double RenoticedLandValue { get; set; }
        public double RenoticedImprovedValue { get; set; }
        public double RenoticedMarketValue { get; set; }
        public double RenoticeTotalValue { get; set; }
        public bool? ReviewBindingArbitration { get; set; } //Added by Preethi M  tfs:38003
        public int? ExemptionId { get; set; }
        public int HearingTrackingId { get; set; }
    }

    public class PTXdtYearlyHearingDetailForQC
    {
        public int YearlyHearingDetailsId { get; set; }
        public int AccountId { get; set; }
        //public int ValueNoticeId { get; set; }
        public int TaxYear { get; set; }
        public bool DND { get; set; }
        //public int ProtestingDetailsId { get; set; }
        //public int UpdatedBy { get; set; }
        //public DateTime UpdatedDateTime { get; set; }
    }

    public class PTXdtValueNoticeForQC
    {
        public int ValueNoticeId { get; set; }
        public double NoticeLandValue { get; set; }
        public double NoticeImprovedValue { get; set; }
        public double NoticeMarketValue { get; set; }
        public double NoticeTotalValue { get; set; }
    }

    public class PTXdtHearingDetailsForQC
    {
        public int HDHearingDetailsId { get; set; }
        public int HDHearingTypeId { get; set; }
        public int HDHearingStatusId { get; set; }
        public DateTime InformalHearingDate { get; set; }
        public TimeSpan InformalHearingTime { get; set; }
        public DateTime FormalHearingDate { get; set; }
        public TimeSpan FormalHearingTime { get; set; }
        public int InformalAssignedAgentId { get; set; }
        public int FormalAssignedAgentId { get; set; }
        public DateTime InformalBatchPrintDate { get; set; }
        public DateTime FormalBatchPrintDate { get; set; }
        public string InformalPanelDocketID { get; set; }
        public string FormalPanelDocketID { get; set; }
        public DateTime EvidenceDueDate { get; set; }

        public int YearlyHearingDetailsId { get; set; }
        public int ValueNoticeId { get; set; }
        public double NoticeLandValue { get; set; }
        public double NoticeImprovedValue { get; set; }
        public double NoticeMarketValue { get; set; }
        public double NoticeTotalValue { get; set; }
        public int StatusId { get; set; }

        public int HearingResultId { get; set; }
        public int HRHearingTypeId { get; set; }
        public int HRHearingStatusId { get; set; }
        public int HearingAgentId { get; set; }
        public DateTime confirmationLetterDateTime { get; set; }
        public DateTime confirmationLetterReceivedDate { get; set; }
        public DateTime completionDateAndTime { get; set; }
        public double PostHearingLandValue { get; set; }
        public double PostHearingImprovedValue { get; set; }
        public double PostHearingMarketValue { get; set; }
        public double PostHearingTotalValue { get; set; }
        public bool HearingFinalized { get; set; }
        public DateTime HearingResultsSentOn { get; set; }
        public bool InvoiceGenerated { get; set; }
        public bool ExemptInvoice { get; set; }
        public bool ResultGenerated { get; set; }
        public int HearingResultReasonCodeId { get; set; }
        public int DismissalAuthStatusId { get; set; }
        public int HearingResolutionId { get; set; }
        //added by Boopathi.S
        public int? HRInvoiceStatusid { get; set; }
        public int? ExemptionId { get; set; }
    }

    public class PTXdtHearingResultForQC
    {
        public int HearingResultId { get; set; }
        public int HearingDetailsId { get; set; }
        public int HearingTypeId { get; set; }
        public int HearingStatusId { get; set; }
        public int HearingAgentId { get; set; }
        public DateTime confirmationLetterDateTime { get; set; }
        public DateTime completionDateAndTime { get; set; }
        public double PostHearingLandValue { get; set; }
        public double PostHearingImprovedValue { get; set; }
        public double PostHearingMarketValue { get; set; }
        public double PostHearingTotalValue { get; set; }
        public bool HearingFinalized { get; set; }
        public DateTime HearingResultsSentOn { get; set; }
        public bool InvoiceGenerated { get; set; }
        public int HearingResolutionId { get; set; }
        public int HearingResultReasonCodeId { get; set; }
        public int DismissalAuthStatusId { get; set; }
    }

    public class PTXdtHearingResultRemarksForQC
    {
        public int HearingResultRemarksId { get; set; }
        public int HearingResultId { get; set; }
        public string Remarks { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDatetime { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }

    public class PTXdtInvoiceAndHearingResultMap
    {
        public int InvoiceAndHearingResultsMapId { get; set; }
        public int InvoiceId { get; set; }
        public int HearingResultId { get; set; }
    }
}
