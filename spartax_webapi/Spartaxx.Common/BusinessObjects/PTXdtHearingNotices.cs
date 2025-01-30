using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common
{
    //From Spartaxx.DataService --> Below classes 
    public class PTXdtHearingNotices
    {
        public int IndexedNoticeDocumentId { get; set; }
        public string BatchCode { get; set; }
        public int Countyid { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string Website { get; set; }
        public int NoticeDocumentTypeId { get; set; }
        public string NoticedDocumentType { get; set; }
        public int AccountId { get; set; }
        public int ClientId { get; set; }
        public string AccountNumber { get; set; }
        public int AccountStatusId { get; set; }
        public string Accountstatus { get; set; }
        public int AccountProcessStatusId { get; set; }
        public string AccountProcessStatus { get; set; }
        public int TaxYear { get; set; }
        public int NoticeProcessingStatusId { get; set; }
        public string NoticeProcessingStatus { get; set; }
        public int ProtestCodeId { get; set; }
        public string ProtestCodeValues { get; set; }
        public int PWImageID { get; set; }
        public int FileCabinetID { get; set; }
        public int UserEnteredVNoticeDataID { get; set; }
        public string ClientNumber { get; set; }
        public string IFileNumber { get; set; }
        public DateTime NoticeDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double LandValue { get; set; }
        public double ImprovedValue { get; set; }
        public double MarketValue { get; set; }
        public double TotalValue { get; set; }

        //Added by Balaji Task:38259
        public double RenoticedLandValue { get; set; }
        public double RenoticedImprovedValue { get; set; }
        public double RenoticedMarketValue { get; set; }
        public double RenoticeTotalValue { get; set; }

        public bool IsValueNoticeAlreadyExist { get; set; }
        public bool IsValueNoticeUpdateRequired { get; set; }
        public DateTime? OnHoldDateAndTime { get; set; } //Newly Added
        public DateTime? ScannedDateAndTime { get; set; } //Newly Added
        public int Offset { get; set; } //Newly Added
        public string TCK { get; set; } //Newly Added
        public string DocumentName { get; set; } //Newly Added
        public int YearlyHearingDetails { get; set; } //Newly Added...
        public string DocumentFileName { get; set; } //Newly Added

        public int DocumentDefectCodeId { get; set; }
        public string DocumentDefectCodes { get; set; }
        public bool DefectNotice { get; set; }
        public int AssignedQueueId { get; set; }
        public string NoticeQueue { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int RequestedUserId { get; set; }
        public string RequestedUser { get; set; }
        public DateTime RequestedDateAndTime { get; set; }
        public int RespondedUserId { get; set; }
        public string RespondedUser { get; set; }
        public DateTime RespondedDateAndTime { get; set; }
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
        public int EntryUserRoleId { get; set; }
        public int DefectAssignedUserId { get; set; }
        public int DefectAssignedUserRoleId { get; set; }
        public int AuditedUserId { get; set; }
        public int AuditedUserRoleId { get; set; }
        public bool DefectResolved { get; set; }
        public DateTime DefectRectifiedDateAndTime { get; set; }


        public int? UE_TaxYear { get; set; }
        public DateTime? EvidenceDueDate { get; set; }
        public DateTime? InformalHearingDate { get; set; }
        public TimeSpan? InfomalHearingtime { get; set; }
        public DateTime? FormalHearingDate { get; set; }
        public TimeSpan? FormalHearingTime { get; set; }
        public string PanelDocketId { get; set; }
        public string FormalPanelDocketID { get; set; }
        public string InformalPanelDocketID { get; set; }
        public string Remarks { get; set; }
        public int UserEnteredHearingNoticeDataId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        //To show the Formal & informal hearing dates if exist
        public DateTime? Existing_EvidenceDueDate { get; set; }
        public DateTime? Existing_InformalHearingDate { get; set; }
        public TimeSpan? Existing_InformalHearingTime { get; set; }
        public DateTime? Existing_FormalHearingDate { get; set; }
        public TimeSpan? Existing_FormalHearingTime { get; set; }
        public int Existing_HearingTypeId { get; set; }
        public string Existing_HearingType { get; set; }
        public int UserEnteredUpdatedBy { get; set; }
        public DateTime? UserEnteredUpdatedDateTime { get; set; }
        public string PropertyAddress { get; set; }
        //Added by Nandhini for TFS id-41386
        public bool IsFormalRescheduleHearing { get; set; }
        public bool IsInformalRescheduleHearing { get; set; }
        public DateTime? InformalNoticeLetterDate { get; set; }
        public DateTime? FormalNoticeLetterDate { get; set; }
        //end

        public int HearingModeid { get; set; }
        public int DNDCodeID { get; set; }
        public int DisasterId { get; set; }
        public bool IsDisasterProtest { get; set; }
        public bool? FormalWebex { get; set; }
        public bool? InformalWebex { get; set; }

        public double PostHearingLandValue { get; set; }
        public double PostHearingImprovedValue { get; set; }
        public double PostHearingMarketValue { get; set; }
        public double PostHearingTotalValue { get; set; }

        public int HearingResolutionId { get; set; }
        public int ReasonCodeId { get; set; }

        public DateTime? FormalFilePreppedDate { get; set; }
        public DateTime? InformalFilePreppedDate { get; set; }
    }

    public class PTXdtHearingNoticeClarification
    {
        public int NoticeClarificationId { get; set; }
        public int IndexedNoticeDocumentId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int RequestedUserId { get; set; }
        public string RequestedUser { get; set; }
        public string RequestedUserFirstName { get; set; }
        public string RequestedUserLastName { get; set; }
        public int RequestedUserRoleId { get; set; }
        public string RequestedUserRole { get; set; }
        public DateTime RequestedDateAndTime { get; set; }
        public int RespondedUserId { get; set; }
        public string RespondedUser { get; set; }
        public string RespondedUserFirstName { get; set; }
        public string RespondedUserLastName { get; set; }
        public int RespondedUserRoleId { get; set; }
        public string RespondedUserRole { get; set; }
        public DateTime? RespondedDateAndTime { get; set; }
        public string ClarificationAssignedBy { get; set; }
        public DateTime ClarificationAssignedOn { get; set; }
    }

    public class PTXdtHearingNoticeRemarks
    {
        public int UserEnteredHearingNoticeRemarksId { get; set; }
        public int UserEnteredHearingNoticeDataId { get; set; }
        public string Remarks { get; set; }
        public int UpdatedBy { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }

    // Hearing Type.
    public class PTXdtUserEnteredHearingTypeForNotice
    {
        public int UserEnteredHearingTypeForNoticeId { get; set; }
        public int HearingTypeId { get; set; }
        public int UserEnteredHearingNoticeDataId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }

    // Multiple Hearing Type Details .
    public class PTXdtHearingNoticeExistingHearingTypeDetails
    {
        public string CountyCode { get; set; }
        public int AccountId { get; set; }
        public int HearingTypeId { get; set; }
        public string HearingType { get; set; }
        public DateTime? FormalHearingDate { get; set; }
        public string FormalHearingTime { get; set; }
        public DateTime? InformalHearingDate { get; set; }
        public string InformalHearingTime { get; set; }
        public string PanelDocketId { get; set; }
        public DateTime EvidenceDueDate { get; set; }
        public int TaxYear { get; set; }
        public double? NoticeImprovedValue { get; set; }
        public double? NoticeLandValue { get; set; }
        public double? NoticeMarketValue { get; set; }
        public double? NoticeTotalValue { get; set; }

        //Added by Balaji Task:38259
        public double? RenoticedImprovedValue { get; set; }
        public double? RenoticedLandValue { get; set; }
        public double? RenoticedMarketValue { get; set; }
        public double? RenoticeTotalValue { get; set; }
        public string InformalPanelId { get; set; }
        public string FormalPanelId { get; set; }

    }

    /* Get the Hearing Details for QC Screen*/
    public class PTXdtHearingDetails
    {
        public int HearingDetailsId { get; set; }
        public int HearingTypeId { get; set; }
        public DateTime? InformalHearingDate { get; set; }
        public TimeSpan InformalHearingTime { get; set; }
        public DateTime? FormalHearingDate { get; set; }
        public TimeSpan FormalHearingTime { get; set; }
        public int InformalAssignedAgentId { get; set; }
        public DateTime? InformalInformalHearingDate { get; set; }
        public int FormalAssignedAgentId { get; set; }
        public DateTime? InformalBatchPrintDate { get; set; }
        public DateTime? FormalBatchPrintDate { get; set; }
        public string InformalPanelDocketID { get; set; }
        public string FormalPanelDocketID { get; set; }
        public DateTime? EvidenceDueDate { get; set; }
        public DateTime? HearingDateAudited { get; set; }
        public int Countyid { get; set; }
        public DateTime? HearingCompletedDate { get; set; }
        public int HearingStatusId { get; set; }
        public int YearlyHearingDetailsId { get; set; }

        /*Value Notice Columns*/
        public int ValueNoticeId { get; set; }
        public DateTime? NoticedDate { get; set; }
        public string IFileNumber { get; set; }
        public double NoticeLandValue { get; set; }
        public double NoticeImprovedValue { get; set; }
        public double NoticeMarketValue { get; set; }
        public double NoticeTotalValue { get; set; }
        public int Statusid { get; set; }
        public bool IsValueNoticeAlreadyExist { get; set; }
        public bool IsValueNoticeUpdateRequired { get; set; }
    }

    /* Get the Hearing Details Remarks for QC Screen*/
    public class PTXdtHearingDetailsRemarks
    {
        public int HearingDetailsRemarksId { get; set; }
        public int HearingDetailsId { get; set; }
        public string Remarks { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
    }
}
