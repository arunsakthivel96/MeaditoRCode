using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spartaxx.Common;
using System.Collections;
using Spartaxx.DataObjects;
using System.Dynamic;
using Spartaxx.Common.BusinessObjects;

namespace Spartaxx.DataAccess
{
    public class CommonRepocitory : ICommonRepocitory
    {
        private readonly DapperConnection _Connection;
        public CommonRepocitory(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        public List<PTXboPaperwiseDocumentType> getPWDocumentTypeListByFileCabinetID(int fileCabinetID)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            List<PTXboPaperwiseDocumentType> paperwiseDocumentType = new List<PTXboPaperwiseDocumentType>();
            paperwiseDocumentType = null;

            try
            {
                SPName = StoredProcedureNames.usp_getPWDocumentTypesByFileCabinetID;               

                _hashtable.Add("@FileCabinetID", fileCabinetID);                

                paperwiseDocumentType = _Connection.Select<PTXboPaperwiseDocumentType>(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return paperwiseDocumentType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboIndexedNoticeDocumentRouteFrom> getIndexedNoticeDocumentRouteFrom(int IndexedNoticeDocumentId)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            List<PTXboIndexedNoticeDocumentRouteFrom> paperwiseDocumentType = new List<PTXboIndexedNoticeDocumentRouteFrom>();
            paperwiseDocumentType = null;

            try
            {
                SPName = StoredProcedureNames.usp_getIndexedNoticeDocumentRouteFrom;

                _hashtable.Add("@IndexedNoticeDocumentId", IndexedNoticeDocumentId);

                paperwiseDocumentType = _Connection.Select<PTXboIndexedNoticeDocumentRouteFrom>(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return paperwiseDocumentType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXdoCounty> getCountyURL(int CountyId)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            
            List<PTXdoCounty> county = new List<PTXdoCounty>();
            county = null;

            try
            {
                SPName = StoredProcedureNames.usp_getCountyURL;

                _hashtable.Add("@CountyID", CountyId);

                county = _Connection.Select<PTXdoCounty>(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return county;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private ExpandoObject convertdtResultToIndexedNoticeDocumentObject(dynamic SP_Result, PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue)
        //{
        //    var lstHearingDetails = new List<PTXdoHearingDetails>();
        //    dynamic result = new ExpandoObject();
        //    PTXdoHearingDetails hearingDetail = new PTXdoHearingDetails();

        //    try
        //    {
        //        if (SP_Result != null)
        //        {
        //            #region IndexedNoticeDocument Object
        //            var dtEntryRecordList = (List<PTXdtHearingResults>)SP_Result.Item1;
        //            indexedNoticeList = dtEntryRecordList.Select(b => new PTXdoIndexedNoticeDocument
        //            {
        //                IndexedNoticeDocumentID = b.IndexedNoticeDocumentId,
        //                batchCode = b.BatchCode,
        //                PWImageID = b.PWImageID,
        //                ScannedDateAndTime = b.ScannedDateAndTime,
        //                //FileCabinetID = b.PWFileCabinetID, /* FileCabinetID added in NoticeDocumentType */
        //                documentType = new PTXdoNoticeDocumentType()
        //                {
        //                    NoticeDocumentTypeId = b.NoticeDocumentTypeId,
        //                    NoticeDocumentTypeValue = b.NoticedDocumentType,
        //                    FileCabinetID = b.PWFileCabinetID
        //                },
        //                Remarks = b.Remarks,
        //                taxYear = b.Taxyear,
        //                countyCode = b.CountyName,
        //                accountNumber = b.AccountNumber,
        //                account = new PTXdoAccount()
        //                {
        //                    AccountId = b.AccountId,
        //                    EndDate = b.EndDate,
        //                    //propertyDetails = new PTXdoPropertyDetails()
        //                    //{
        //                    //    PropertyAddress = new PTXdoAddress()
        //                    //    {
        //                    //        addressLine1 = b.PropertyAddress
        //                    //    }
        //                    //},
        //                    /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 Start*/

        //                    YearlyHearingDetails = (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue) ? (new List<PTXdoYearlyHearingDetails>
        //                        {
        //                            new PTXdoYearlyHearingDetails()
        //                            {
        //                                propertyDetails = new PTXdoPropertyDetails(){PropertyAddress = new PTXdoAddress(){addressLine1 = b.PropertyAddress}}
        //                            }
        //                        }
        //                    ) : (getYearlyHearingDetailsById(b.YearlyHearingDetails)), //By Prakash

        //                    /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 End*/
        //                    County = new PTXdoCounty()
        //                    {
        //                        Countyid = b.Countyid,
        //                        CountyName = b.CountyName,
        //                        CountyCode = b.CountyCode
        //                        //Website = b.Website
        //                    }
        //                },
        //                accountStatus = new PTXdoAccountStatus()
        //                {
        //                    Accountstatusid = b.AccountStatusId,
        //                    Accountstatus = b.Accountstatus
        //                },
        //                accountProcessingStatus = new PTXdoAccountProcessStatus()
        //                {
        //                    AccProcessstatusid = b.AccountProcessStatusId,
        //                    AccProcessstatus = b.AccountProcessStatus
        //                },
        //                protestStatus = new PTXdoProtestCodeValues()
        //                {
        //                    ProtestCodeValuesId = b.ProtestCodeId,
        //                    ProtestCodeValues = b.ProtestCodeValues
        //                },
        //                userEnteredHearingResult = new PTXdoUserEnteredHearingResults() /* User Entered values */
        //                {
        //                    UserEnteredHearingResutsID = b.UserEnteredHearingResultsId,
        //                    TaxYear = b.Taxyear,
        //                    NoticedLandValue = b.NoticedLandValue,
        //                    NoticedImprovedValue = b.NoticedImprovedValue,
        //                    NoticedMarketValue = b.NoticedMarketValue,
        //                    NoticedTotalValue = b.NoticedTotalvalue,
        //                    //Added by Balaji Task:38259
        //                    RenoticedLandValue = b.RenoticedLandValue,
        //                    RenoticedImprovedValue = b.RenoticedImprovedValue,
        //                    RenoticedMarketValue = b.RenoticedMarketValue,
        //                    RenoticeTotalValue = b.RenoticeTotalvalue,
        //                    PostHearingLandValue = b.PostHearingLandValue,
        //                    PostHearingImprovedValue = b.PostHearingImprovedValue,
        //                    PostHearingMarketValue = b.PostHearingMarketValue,
        //                    PostHearingTotalValue = b.PostHearingTotalValue,
        //                    HearingAgent = new PTXdoAgent()
        //                    {
        //                        AgentId = b.HearingAgentId
        //                    },
        //                    ConfirmationLetterDate = b.ConfirmationLetterDate,
        //                    confirmationLetterReceivedDate = b.confirmationLetterReceivedDate,
        //                    CompletionDate = b.CompletionDate,
        //                    CompletionTime = b.CompletionTime,
        //                    HearingResolution = new PTXdoHearingResolution() { HearingResolutionId = b.HearingResolutionId },
        //                    ReasonCodes = new PTXdoHearingResultsReasonCode() { HearingResultsReasonCodeId = b.ReasonCodeId },
        //                    DismissalAuthStatus = new PTXdoHearingDismissalAuthStatus() { HearingDismissalAuthStatusId = b.DismissalAuthStatusId },
        //                    DefectReason = new PTXdoDocumentDefectCodes() { DocumentDefectCodeId = b.DefectReasonId },
        //                    HearingResultsReport = new PTXdoHearingResultReport() { HearingResultReportId = b.HearingResultReportId },
        //                    HearingFinalized = b.HearingFinalized,
        //                    HearingResultsSentOn = b.HearingResultsSentOn,
        //                    ResultedGenerated = b.ResultGenerated,
        //                    InvoiceGenerated = b.InvoiceGenerated,
        //                    InvoiceID = b.InvoiceId,
        //                    HearingStatus = new PTXdoHearingStatus() { HearingStatusId = b.HearingStatusId },
        //                    ExemptInvoice = b.ExemptInvoice,
        //                    HRInvoiceStatus = new PTXdoHRInvoiceStatus() { HRInvoiceStatusid = b.HRInvoiceStatusid }
        //                },
        //                assignedQueue = new PTXdoNoticeQueue()
        //                {
        //                    NoticeQueueId = b.AssignedQueueId,
        //                    NoticeQueue = b.NoticeQueue
        //                },
        //                processingStatus = new PTXdoNoticeProcessingStatus()
        //                {
        //                    NoticeProcessingStatusId = b.NoticeProcessingStatusId,
        //                    NoticeProcessingStatus = b.NoticeProcessingStatus
        //                },

        //                entryUser = new PTXdoUser()
        //                {
        //                    Userid = b.EntryUserId
        //                },
        //                EntryAssignedBy = new PTXdoUser()
        //                {
        //                    Username = b.EntryAssignBy
        //                },
        //                entryDataAndTime = b.EntryAssignOn,
        //                AuditAssignedBy = new PTXdoUser()
        //                {
        //                    Username = b.AuditAssignBy
        //                },
        //                auditedDateAndTime = b.AuditAssignOn,
        //                DefectAssignedBy = new PTXdoUser()
        //                {
        //                    Username = b.DefectAssignBy
        //                },
        //                defectAssignedDateAndTime = b.DefectAssignOn,
        //                QCAssignedBy = new PTXdoUser()
        //                {
        //                    Username = b.QCAssignBy
        //                },
        //                qcAssignedDateAndTime = b.QCAssignOn,

        //                defectAssignedUser = new PTXdoUser()
        //                {
        //                    Userid = b.DefectAssignedUserId
        //                },
        //                auditedUser = new PTXdoUser()
        //                {
        //                    Userid = b.AuditedUserId
        //                },
        //                defectNotice = b.DefectNotice,
        //                defectResolved = b.DefectResolved,
        //                defectReasonCode = new PTXdoDocumentDefectCodes()
        //                {
        //                    DocumentDefectCodeId = b.DocumentDefectCodeId,
        //                    DocumentDefectCodes = b.DocumentDefectCodes
        //                },
        //                defectRectifiedDateAndTime = b.DefectRectifiedDateAndTime,
        //                client = new PTXdoClient() { clientId = b.ClientId, ClientNumber = b.ClientNumber },
        //                DocumentName = b.DocumentName,
        //                qcAssignedUser = new PTXdoUser() { Userid = b.QCAssignedUserId },
        //            }).FirstOrDefault();

        //            //foreach (DataRow iem in dsResult.Tables[0].Rows)
        //            //{
        //            //    if (iem["ExemptInvoice1"].ToString() != "")
        //            //    {
        //            //        System.Web.HttpContext.Current.Session["Exempt"] = Convert.ToBoolean(iem["ExemptInvoice1"]);
        //            //    }

        //            //    break;
        //            //}

        //            //foreach (DataRow itemm in dsResult.Tables[0].Rows)
        //            //{
        //            //    if (itemm["ReviewBindingArbitration"].ToString() != "")
        //            //    {
        //            //        System.Web.HttpContext.Current.Session["ReviewBindingArbitration"] = Convert.ToBoolean(itemm["ReviewBindingArbitration"]);
        //            //    }
        //            //    break;
        //            //}

        //            var DNDCodes = dtEntryRecordList.Select(x => x.DNDcodes).FirstOrDefault();
        //            List<PTXdoYearlyHearingDetails> item = new List<PTXdoYearlyHearingDetails>();
        //            if (!string.IsNullOrEmpty(DNDCodes))
        //            {
        //                item.Add(new PTXdoYearlyHearingDetails() { DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes } });
        //            }
        //            if (indexedNoticeList.account != null && (indexedNoticeList.account.YearlyHearingDetails == null || indexedNoticeList.account.YearlyHearingDetails.Count == 0))
        //            {
        //                indexedNoticeList.account.YearlyHearingDetails = item;
        //            }
        //            else
        //            {
        //                indexedNoticeList.account.YearlyHearingDetails[0].DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes };
        //            }
        //            #endregion

        //            #region Clarification Object
        //            /* Clarification Log: First row will be the latest record. */
        //            if (SP_Result.Item2 != null && SP_Result.Item2.Count > 0)
        //            {
        //                var dtClarificationList = (List<PTXdtHearingResultsClarification>)SP_Result.Item2;

        //                List<PTXdoNoticeClarification> clarificationList = dtClarificationList.Select(a => new PTXdoNoticeClarification
        //                {
        //                    NoticeClarificationId = a.NoticeClarificationId,
        //                    IndexedNoticeDocument = new PTXdoIndexedNoticeDocument()
        //                    {
        //                        IndexedNoticeDocumentID = a.IndexedNoticeDocumentId
        //                    },
        //                    RequestedUser = new PTXdoUser()
        //                    {
        //                        Userid = a.RequestedUserId,
        //                        Username = a.RequestedUser,
        //                        Firstname = a.RequestedUserFirstName,
        //                        Lastname = a.RequestedUserLastName
        //                    },
        //                    Request = a.Request,
        //                    RequestedDateAndTime = a.RequestedDateAndTime,
        //                    RespondedUser = new PTXdoUser()
        //                    {
        //                        Userid = a.RespondedUserId,
        //                        Username = a.RespondedUser,
        //                        Firstname = a.RespondedUserFirstName,
        //                        Lastname = a.RespondedUserLastName
        //                    },
        //                    Response = a.Response,
        //                    RespondedDateAndTime = a.RespondedDateAndTime,
        //                    ClarificationAssignedBy = new PTXdoUser()
        //                    {
        //                        Username = a.ClarificationAssignedBy
        //                    },
        //                    ClarificationAssignedOn = a.ClarificationAssignedOn

        //                }).ToList();

        //                indexedNoticeList.clarificationList = clarificationList;

        //                if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)    //By Mano
        //                {
        //                    /* Get the Clarification list*/
        //                    indexedNoticeList.clarificationList = clarificationList.Where(a => a.IndexedNoticeDocument.IndexedNoticeDocumentID == indexedNoticeList.IndexedNoticeDocumentID).ToList();
        //                }
        //            }
        //            #endregion

        //            #region Hearing Details Remarks
        //            if (SP_Result.Item3 != null && SP_Result.Item3.Count > 0)
        //            {
        //                var dtHearingResultRemarks = (List<PTXdtHearingDetailsRemarks>)SP_Result.Item3;

        //                List<PTXdoHearingDetailsRemarks> hearingDetailsRemarksList = dtHearingResultRemarks.OrderByDescending(c => c.UpdatedDatetime).Select(c => new PTXdoHearingDetailsRemarks
        //                {
        //                    HearingDetailsRemarksId = c.HearingDetailsRemarksId,
        //                    HearingDetails = new PTXdoHearingDetails() { HearingDetailsId = c.HearingDetailsId },
        //                    Remarks = c.Remarks,
        //                    UpdatedBy = new PTXdoUser()
        //                    {
        //                        Userid = c.UpdatedBy,
        //                        Username = c.Username
        //                    },
        //                    UpdatedDateTime = c.UpdatedDatetime
        //                }).ToList();

        //                hearingDetail.HearingDetailsRemarks = hearingDetailsRemarksList;

        //                /*Assign Hearing Details Remarks*/
        //                //foreach (var hearingDetails in hearingDetailsRemarksList)
        //                //{
        //                //    var objHearingDetails = Repository<PTXdoHearingDetails>.GetQuery().FirstOrDefault(q => q.HearingDetailsId == hearingDetails.HearingDetails.HearingDetailsId);
        //                //    objHearingDetails.HearingDetailsRemarks = hearingDetailsRemarksList.Where(a => a.HearingDetails.HearingDetailsId == objHearingDetails.HearingDetailsId).ToList();
        //                //    lstHearingDetails.Add(objHearingDetails);
        //                //}

        //                /*Assign Hearing Details Remarks*/
        //                foreach (var hearingDetails in hearingDetailsRemarksList)
        //                {
        //                    hearingDetail.HearingDetailsRemarks = hearingDetailsRemarksList.Where(a => a.HearingDetails.HearingDetailsId == hearingDetail.HearingDetailsId).ToList();
        //                }
        //            }
        //            #endregion

        //            #region Hearing Type
        //            if (SP_Result.Item4 != null && SP_Result.Item4.Count > 0)
        //            {                        
        //                var dtHearingTypeForResult = (List<PTXdtHearingTypeForResult>)SP_Result.Item4;

        //                List<PTXdoUserEnteredHearingTypeForResult> UserEnteredHearingTypeForResult = dtHearingTypeForResult.Select(c => new PTXdoUserEnteredHearingTypeForResult
        //                {
        //                    hearingType = new PTXdoHearingType() { HearingTypeId = c.HearingTypeId },
        //                    userEnteredHearingTypeId = c.UserEnteredHearingTypeId,

        //                    userEnteredHearingResults = new PTXdoUserEnteredHearingResults()
        //                    {
        //                        UserEnteredHearingResutsID = c.UserEnteredHearingResultsId
        //                    },
        //                    UpdatedBy = new PTXdoUser() { Userid = c.UpdatedBy },
        //                    UpdatedDateTime = c.UpdatedDateTime
        //                }).ToList();

        //                indexedNoticeList.userEnteredHearingResult.UserEnteredHearingTypeForResultList = UserEnteredHearingTypeForResult;
        //            }
        //            #endregion
        //            result.indexedNoticeList = indexedNoticeList;

        //            //if (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue)
        //            //    result.hearingNoticeRemarksList = hearingNoticeRemarksList;
        //            //else
        //            //    result.HearingDetails = HearingDetails;

        //            return result;
        //        }                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return null;
        //}
        //private List<PTXdoYearlyHearingDetails> getYearlyHearingDetailsById(int YearlyHearingDetailsById)
        //{
        //    Hashtable _hashtable = new Hashtable();
        //    try
        //    {
        //        _hashtable.Add("@YearlyHearingDetailsById", YearlyHearingDetailsById);

        //        var SP_Result = _Connection.Select<PTXdoYearlyHearingDetails>(StoredProcedureNames.usp_getYearlyHearingDetailsById, _hashtable,
        //            Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        return SP_Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}      
    }
}