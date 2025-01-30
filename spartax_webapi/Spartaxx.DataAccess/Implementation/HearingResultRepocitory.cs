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
using Spartaxx.DataAccess;
using Spartaxx.Utilities.Logging;
using Spartaxx.Utilities.Extenders;

namespace Spartaxx.DataAccess
{
    public class HearingResultRepocitory : IHearingResultRepocitory
    {
        private readonly DapperConnection _Connection;
        public HearingResultRepocitory(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        public List<PTXboHearingResultAllottedDocument> getHearingResultEntryAllottedDocuments(PTXboHearingResultAllottedDocBasedOnSearchFilter objHearingResultAllotmentSearchRequest)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();

            try
            {
                Logger.For(this).Transaction("getHearingResultEntryAllottedDocuments-API  Reached " + ((object)objHearingResultAllotmentSearchRequest).ToJson(false));
                if (objHearingResultAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.EntryQueue)
                    SPName = StoredProcedureNames.usp_getHearingResultEntryAllottedDocuments;
                else if (objHearingResultAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.AuditQueue)
                    SPName = StoredProcedureNames.usp_getHearingResultAuditAllottedDocuments;
                else if (objHearingResultAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.DefectQueue)
                    SPName = StoredProcedureNames.usp_getHearingResultDefectAllottedDocuments;
                else if (objHearingResultAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.ClarificationQueue)
                    SPName = StoredProcedureNames.usp_getHearingResultClarificationAllottedDocuments;
                else if (objHearingResultAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.QCQueue)
                    SPName = StoredProcedureNames.usp_getHearingResultQCAllottedDocuments;

                _hashtable.Add("@UserID", objHearingResultAllotmentSearchRequest.UserID);
                _hashtable.Add("@UserRoleID", objHearingResultAllotmentSearchRequest.UserRoleID);
                _hashtable.Add("@ClientNumber", objHearingResultAllotmentSearchRequest.ClientNumber);
                _hashtable.Add("@AccountNumber", objHearingResultAllotmentSearchRequest.AccountNumber);
                _hashtable.Add("@FirstName", objHearingResultAllotmentSearchRequest.FirstName);
                _hashtable.Add("@LastName", objHearingResultAllotmentSearchRequest.LastName);
                _hashtable.Add("@BatchCode", objHearingResultAllotmentSearchRequest.BatchCode);
                _hashtable.Add("@CADLegalName", objHearingResultAllotmentSearchRequest.CADLegalName);
                _hashtable.Add("@PropertyAddress", objHearingResultAllotmentSearchRequest.PropertyAddress);

                var result = _Connection.Select<PTXboHearingResultAllottedDocument>(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Transaction("getHearingResultEntryAllottedDocuments-API  Ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getHearingResultEntryAllottedDocuments-API  Error " + ex);
                throw ex;
            }
        }

        public ExpandoObject getHearingResultNextEntryRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int Accountid, int taxYear, int HearingTypeid, int AssignedQueue)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            int Userid = 0;
            int UserRoleid = 0;
            try
            {
                Logger.For(this).Transaction("getHearingResultNextEntryRecordList-API  Reached " + ((object)indexedNoticeList).ToJson(false));
                int queueId = indexedNoticeList.assignedQueue.NoticeQueueId;
                if (queueId == Convert.ToInt32(Enumerators.PTXenumNoticeQueue.EntryQueue))
                {
                    SPName = StoredProcedureNames.usp_getNextHearingResultDocument_Defect;
                    Userid = indexedNoticeList.entryUser.Userid;
                    UserRoleid = indexedNoticeList.entryUserRole.UserRoleid;
                }
                else if (queueId == Convert.ToInt32(Enumerators.PTXenumNoticeQueue.AuditQueue))
                {
                    SPName = StoredProcedureNames.usp_getNextHearingResultDocument_Defect;
                    Userid = indexedNoticeList.auditedUser.Userid;
                    UserRoleid = indexedNoticeList.auditedUserRole.UserRoleid;
                }
                else if (queueId == Convert.ToInt32(Enumerators.PTXenumNoticeQueue.DefectQueue))
                {
                    SPName = StoredProcedureNames.usp_getNextHearingResultDocument_Defect;
                    Userid = indexedNoticeList.defectAssignedUser.Userid;
                    UserRoleid = indexedNoticeList.defectAssignedUserRole.UserRoleid;
                }
                else if (queueId == Convert.ToInt32(Enumerators.PTXenumNoticeQueue.ClarificationQueue))
                {
                    SPName = StoredProcedureNames.usp_getNextHearingResultDocument_Defect;
                    Userid = indexedNoticeList.clarificationList[0].RespondedUser.Userid;
                    UserRoleid = indexedNoticeList.clarificationList[0].RespondedUserRole.UserRoleid;
                }
                else if (queueId == Convert.ToInt32(Enumerators.PTXenumNoticeQueue.QCQueue))
                {
                    SPName = StoredProcedureNames.usp_getNextHearingResultDocument_Defect;
                    Userid = indexedNoticeList.qcAssignedUser.Userid;
                    UserRoleid = indexedNoticeList.qcAssignedUserRole.UserRoleid;
                }                    
                
                _hashtable.Add("@UserID", Userid);
                _hashtable.Add("@UserRoleID", UserRoleid);
                _hashtable.Add("@AssignedQueueID", indexedNoticeList.assignedQueue.NoticeQueueId);
                _hashtable.Add("@IndexedNoticeDocumentId", indexedNoticeList.IndexedNoticeDocumentID);
                _hashtable.Add("@AccountId", indexedNoticeList.Accountid);
                _hashtable.Add("@Val_TaxYear", indexedNoticeList.taxYear);
                _hashtable.Add("@HearingTypeid", indexedNoticeList.HearingTypeid);

                var SP_Result = _Connection.SelectMultiple(SPName, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXdtHearingResults>(),
                    gr => gr.Read<PTXdtHearingResultsClarification>(),
                    gr => gr.Read<PTXdtHearingDetailsRemarks>(),
                    gr => gr.Read<PTXdtHearingTypeForResult>()
                );

                var result = convertdtResultToIndexedNoticeDocumentObject(SP_Result, indexedNoticeList, AssignedQueue);
                Logger.For(this).Transaction("getHearingResultNextEntryRecordList-API  Ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getHearingResultNextEntryRecordList-API  Error " + ex);
                throw ex;
            }
        }

        private ExpandoObject convertdtResultToIndexedNoticeDocumentObject(dynamic SP_Result, PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue)
        {
            var lstHearingDetails = new List<PTXdoHearingDetails>();
            dynamic result = new ExpandoObject();
            PTXdoHearingDetails hearingDetail = new PTXdoHearingDetails();

            try
            {
                Logger.For(this).Transaction("convertdtResultToIndexedNoticeDocumentObject-API  Reached " + ((object)indexedNoticeList).ToJson(false));
                if (SP_Result != null)
                {
                    #region IndexedNoticeDocument Object
                    var dtEntryRecordList = (List<PTXdtHearingResults>)SP_Result.Item1;
                    indexedNoticeList = dtEntryRecordList.Select(b => new PTXdoIndexedNoticeDocument
                    {
                        IndexedNoticeDocumentID = b.IndexedNoticeDocumentId,
                        batchCode = b.BatchCode,
                        PWImageID = b.PWImageID,
                        ScannedDateAndTime = b.ScannedDateAndTime,
                        //FileCabinetID = b.PWFileCabinetID, /* FileCabinetID added in NoticeDocumentType */
                        documentType = new PTXdoNoticeDocumentType()
                        {
                            NoticeDocumentTypeId = b.NoticeDocumentTypeId,
                            NoticeDocumentTypeValue = b.NoticedDocumentType,
                            FileCabinetID = b.PWFileCabinetID
                        },
                        Remarks = b.Remarks,
                        taxYear = b.Taxyear,
                        countyCode = b.CountyName,
                        accountNumber = b.AccountNumber,
                        account = new PTXdoAccount()
                        {
                            AccountId = b.AccountId,
                            EndDate = b.EndDate,
                            //propertyDetails = new PTXdoPropertyDetails()
                            //{
                            //    PropertyAddress = new PTXdoAddress()
                            //    {
                            //        addressLine1 = b.PropertyAddress
                            //    }
                            //},
                            /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 Start*/
                            YearlyHearingDetails = new List<PTXdoYearlyHearingDetails>
                            {
                                new PTXdoYearlyHearingDetails()
                                {
                                    propertyDetails = new PTXdoPropertyDetails(){PropertyAddress = new PTXdoAddress(){addressLine1 = b.PropertyAddress}}
                                }
                            },
                            /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 End*/
                            County = new PTXdoCounty()
                            {
                                Countyid = b.Countyid,
                                CountyName = b.CountyName,
                                CountyCode = b.CountyCode
                                //Website = b.Website
                            }
                        },
                        accountStatus = new PTXdoAccountStatus()
                        {
                            Accountstatusid = b.AccountStatusId,
                            Accountstatus = b.Accountstatus
                        },
                        accountProcessingStatus = new PTXdoAccountProcessStatus()
                        {
                            AccProcessstatusid = b.AccountProcessStatusId,
                            AccProcessstatus = b.AccountProcessStatus
                        },
                        protestStatus = new PTXdoProtestCodeValues()
                        {
                            ProtestCodeValuesId = b.ProtestCodeId,
                            ProtestCodeValues = b.ProtestCodeValues
                        },
                        userEnteredHearingResult = new PTXdoUserEnteredHearingResults() /* User Entered values */
                        {
                            UserEnteredHearingResutsID = b.UserEnteredHearingResultsId,
                            TaxYear = b.Taxyear,
                            NoticedLandValue = b.NoticedLandValue,
                            NoticedImprovedValue = b.NoticedImprovedValue,
                            NoticedMarketValue = b.NoticedMarketValue,
                            NoticedTotalValue = b.NoticedTotalvalue,
                            //Added by Balaji Task:38259
                            RenoticedLandValue = b.RenoticedLandValue,
                            RenoticedImprovedValue = b.RenoticedImprovedValue,
                            RenoticedMarketValue = b.RenoticedMarketValue,
                            RenoticeTotalValue = b.RenoticeTotalvalue,
                            PostHearingLandValue = b.PostHearingLandValue,
                            PostHearingImprovedValue = b.PostHearingImprovedValue,
                            PostHearingMarketValue = b.PostHearingMarketValue,
                            PostHearingTotalValue = b.PostHearingTotalValue,
                            HearingAgent = new PTXdoAgent()
                            {
                                AgentId = b.HearingAgentId
                            },
                            ConfirmationLetterDate = b.ConfirmationLetterDate,
                            confirmationLetterReceivedDate = b.confirmationLetterReceivedDate,
                            CompletionDate = b.CompletionDate,
                            CompletionTime = b.CompletionTime,
                            HearingResolution = new PTXdoHearingResolution() { HearingResolutionId = b.HearingResolutionId },
                            ReasonCodes = new PTXdoHearingResultsReasonCode() { HearingResultsReasonCodeId = b.ReasonCodeId },
                            DismissalAuthStatus = new PTXdoHearingDismissalAuthStatus() { HearingDismissalAuthStatusId = b.DismissalAuthStatusId },
                            DefectReason = new PTXdoDocumentDefectCodes() { DocumentDefectCodeId = b.DefectReasonId },
                            HearingResultsReport = new PTXdoHearingResultReport() { HearingResultReportId = b.HearingResultReportId },
                            HearingFinalized = b.HearingFinalized,
                            HearingResultsSentOn = b.HearingResultsSentOn,
                            ResultedGenerated = b.ResultGenerated,
                            InvoiceGenerated = b.InvoiceGenerated,
                            InvoiceID = b.InvoiceId,
                            HearingStatus = new PTXdoHearingStatus() { HearingStatusId = b.HearingStatusId },
                            ExemptInvoice = b.ExemptInvoice,
                            ExemptionId = b.ExemptionId,
                            HRInvoiceStatus = new PTXdoHRInvoiceStatus() { HRInvoiceStatusid = b.HRInvoiceStatusid },
                            HearingTrackingCode = new PTXdoHearingTrackingCodes() { HearingTrackingId = b.HearingTrackingCodeId },
                            HearingLevel = new PTXdoHearingLevel() { HearingLevelID = b.HearingLevelId },
                            ByPassInformalHearing = b.ByPassInformalHearing
                        },
                        assignedQueue = new PTXdoNoticeQueue()
                        {
                            NoticeQueueId = b.AssignedQueueId,
                            NoticeQueue = b.NoticeQueue
                        },
                        processingStatus = new PTXdoNoticeProcessingStatus()
                        {
                            NoticeProcessingStatusId = b.NoticeProcessingStatusId,
                            NoticeProcessingStatus = b.NoticeProcessingStatus
                        },

                        entryUser = new PTXdoUser()
                        {
                            Userid = b.EntryUserId
                        },
                        EntryAssignedBy = new PTXdoUser()
                        {
                            Username = b.EntryAssignBy
                        },
                        entryDataAndTime = b.EntryAssignOn,
                        AuditAssignedBy = new PTXdoUser()
                        {
                            Username = b.AuditAssignBy
                        },
                        auditedDateAndTime = b.AuditAssignOn,
                        DefectAssignedBy = new PTXdoUser()
                        {
                            Username = b.DefectAssignBy
                        },
                        defectAssignedDateAndTime = b.DefectAssignOn,
                        QCAssignedBy = new PTXdoUser()
                        {
                            Username = b.QCAssignBy
                        },
                        qcAssignedDateAndTime = b.QCAssignOn,

                        defectAssignedUser = new PTXdoUser()
                        {
                            Userid = b.DefectAssignedUserId
                        },
                        auditedUser = new PTXdoUser()
                        {
                            Userid = b.AuditedUserId
                        },
                        defectNotice = b.DefectNotice,
                        defectResolved = b.DefectResolved,
                        defectReasonCode = new PTXdoDocumentDefectCodes()
                        {
                            DocumentDefectCodeId = b.DocumentDefectCodeId,
                            DocumentDefectCodes = b.DocumentDefectCodes
                        },
                        defectRectifiedDateAndTime = b.DefectRectifiedDateAndTime,
                        client = new PTXdoClient() { clientId = b.ClientId, ClientNumber = b.ClientNumber },
                        DocumentName = b.DocumentName,
                        qcAssignedUser = new PTXdoUser() { Userid = b.QCAssignedUserId },
                        Disaster = new PTXdoDisaster()
                        {
                            DisasterId = b.DisasterId
                        },
                        IsDisasterProtest=b.IsDisasterProtest
                    }).FirstOrDefault();

                    //foreach (DataRow iem in dsResult.Tables[0].Rows)
                    //{
                    //    if (iem["ExemptInvoice1"].ToString() != "")
                    //    {
                    //        System.Web.HttpContext.Current.Session["Exempt"] = Convert.ToBoolean(iem["ExemptInvoice1"]);
                    //    }

                    //    break;
                    //}

                    //foreach (DataRow itemm in dsResult.Tables[0].Rows)
                    //{
                    //    if (itemm["ReviewBindingArbitration"].ToString() != "")
                    //    {
                    //        System.Web.HttpContext.Current.Session["ReviewBindingArbitration"] = Convert.ToBoolean(itemm["ReviewBindingArbitration"]);
                    //    }
                    //    break;
                    //}

                    var DNDCodes = dtEntryRecordList.Select(x => x.DNDcodes).FirstOrDefault();
                    //List<PTXdoYearlyHearingDetails> item = new List<PTXdoYearlyHearingDetails>();
                    //if (!string.IsNullOrEmpty(DNDCodes))
                    //{
                    //    item.Add(new PTXdoYearlyHearingDetails() { DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes } });
                    //}
                    //if (indexedNoticeList.account != null && (indexedNoticeList.account.YearlyHearingDetails == null || indexedNoticeList.account.YearlyHearingDetails.Count == 0))
                    //{
                    //    indexedNoticeList.account.YearlyHearingDetails = item;
                    //}
                    //else
                    //{
                    //    indexedNoticeList.account.YearlyHearingDetails[0].DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes };
                    //}

                    List<PTXdoYearlyHearingDetails> objYealyHearingList = new List<PTXdoYearlyHearingDetails>();
                    objYealyHearingList = dtEntryRecordList.Select(b => new PTXdoYearlyHearingDetails
                    {
                        YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                        DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes },
                        TaxYear = b.YearlyHearingDetailsId == 0 ? 0 : b.TaxYear
                    }).ToList();
                    if (indexedNoticeList.account != null)
                    {
                        indexedNoticeList.account.YearlyHearingDetails = objYealyHearingList;
                    }
                    #endregion

                    #region Clarification Object
                    /* Clarification Log: First row will be the latest record. */
                    if (SP_Result.Item2 != null && SP_Result.Item2.Count > 0)
                    {
                        var dtClarificationList = (List<PTXdtHearingResultsClarification>)SP_Result.Item2;

                        List<PTXdoNoticeClarification> clarificationList = dtClarificationList.Select(a => new PTXdoNoticeClarification
                        {
                            NoticeClarificationId = a.NoticeClarificationId,
                            IndexedNoticeDocument = new PTXdoIndexedNoticeDocument()
                            {
                                IndexedNoticeDocumentID = a.IndexedNoticeDocumentId
                            },
                            RequestedUser = new PTXdoUser()
                            {
                                Userid = a.RequestedUserId,
                                Username = a.RequestedUser,
                                Firstname = a.RequestedUserFirstName,
                                Lastname = a.RequestedUserLastName
                            },
                            Request = a.Request,
                            RequestedDateAndTime = a.RequestedDateAndTime,
                            RespondedUser = new PTXdoUser()
                            {
                                Userid = a.RespondedUserId,
                                Username = a.RespondedUser,
                                Firstname = a.RespondedUserFirstName,
                                Lastname = a.RespondedUserLastName
                            },
                            Response = a.Response,
                            RespondedDateAndTime = a.RespondedDateAndTime,
                            ClarificationAssignedBy = new PTXdoUser()
                            {
                                Username = a.ClarificationAssignedBy
                            },
                            ClarificationAssignedOn = a.ClarificationAssignedOn

                        }).ToList();

                        indexedNoticeList.clarificationList = clarificationList;

                        if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)    //By Mano
                        {
                            /* Get the Clarification list*/
                            indexedNoticeList.clarificationList = clarificationList.Where(a => a.IndexedNoticeDocument.IndexedNoticeDocumentID == indexedNoticeList.IndexedNoticeDocumentID).ToList();
                        }
                    }
                    #endregion

                    #region Hearing Details Remarks
                    if (SP_Result.Item3 != null && SP_Result.Item3.Count > 0)
                    {
                        var dtHearingResultRemarks = (List<PTXdtHearingDetailsRemarks>)SP_Result.Item3;

                        List<PTXdoHearingDetailsRemarks> hearingDetailsRemarksList = dtHearingResultRemarks.OrderByDescending(c => c.UpdatedDatetime).Select(c => new PTXdoHearingDetailsRemarks
                        {
                            HearingDetailsRemarksId = c.HearingDetailsRemarksId,
                            HearingDetails = new PTXdoHearingDetails() { HearingDetailsId = c.HearingDetailsId },
                            Remarks = c.Remarks,
                            UpdatedBy = new PTXdoUser()
                            {
                                Userid = c.UpdatedBy,
                                Username = c.Username
                            },
                            UpdatedDateTime = c.UpdatedDatetime
                        }).ToList();

                        hearingDetail.HearingDetailsRemarks = hearingDetailsRemarksList;

                        ///*Assign Hearing Details Remarks*/
                        //foreach (var hearingDetails in hearingDetailsRemarksList)
                        //{
                        //    var objHearingDetails = Repository<PTXdoHearingDetails>.GetQuery().FirstOrDefault(q => q.HearingDetailsId == hearingDetails.HearingDetails.HearingDetailsId);
                        //    objHearingDetails.HearingDetailsRemarks = hearingDetailsRemarksList.Where(a => a.HearingDetails.HearingDetailsId == objHearingDetails.HearingDetailsId).ToList();
                        //    lstHearingDetails.Add(objHearingDetails);
                        //}                      


                        /*Assign Hearing Details Remarks*/
                        foreach (var hearingDetails in hearingDetailsRemarksList)
                        {
                            hearingDetail.HearingDetailsRemarks.Concat(hearingDetailsRemarksList.Where(a => a.HearingDetails.HearingDetailsId == hearingDetail.HearingDetailsId).ToList());
                        }
                    }
                    #endregion

                    #region Hearing Type
                    if (SP_Result.Item4 != null && SP_Result.Item4.Count > 0)
                    {
                        var dtHearingTypeForResult = (List<PTXdtHearingTypeForResult>)SP_Result.Item4;

                        List<PTXdoUserEnteredHearingTypeForResult> UserEnteredHearingTypeForResult = dtHearingTypeForResult.Select(c => new PTXdoUserEnteredHearingTypeForResult
                        {
                            hearingType = new PTXdoHearingType() { HearingTypeId = c.HearingTypeId },
                            userEnteredHearingTypeId = c.UserEnteredHearingTypeId,

                            userEnteredHearingResults = new PTXdoUserEnteredHearingResults()
                            {
                                UserEnteredHearingResutsID = c.UserEnteredHearingResultsId
                            },
                            UpdatedBy = new PTXdoUser() { Userid = c.UpdatedBy },
                            UpdatedDateTime = c.UpdatedDateTime
                        }).ToList();

                        indexedNoticeList.userEnteredHearingResult.UserEnteredHearingTypeForResultList = UserEnteredHearingTypeForResult;
                    }
                    #endregion
                    result.indexedNoticeList = indexedNoticeList;

                    //if (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue)
                    //    result.hearingNoticeRemarksList = hearingNoticeRemarksList;
                    //else
                    // result.HearingDetails = hearingDetail;

                    Logger.For(this).Transaction("convertdtResultToIndexedNoticeDocumentObject-API  Ends successfully ");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("convertdtResultToIndexedNoticeDocumentObject-API  Error " + ex);
                throw ex;
            }

            return null;
        }
        private List<PTXdoYearlyHearingDetails> getYearlyHearingDetailsById(int YearlyHearingDetailsById)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Reached " + ((object)YearlyHearingDetailsById).ToJson(false));
                _hashtable.Add("@YearlyHearingDetailsById", YearlyHearingDetailsById);

                var SP_Result = _Connection.Select<PTXdoYearlyHearingDetails>(StoredProcedureNames.usp_getYearlyHearingDetailsById, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Ends successfully ");
                return SP_Result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Error " + ex);
                throw ex;
            }
        }

        public List<PTXdoHearingStatus> getSelectedHearingStatusByHearingType(string hearingTypeId)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                Logger.For(this).Transaction("getSelectedHearingStatusByHearingType-API  Reached " + ((object)hearingTypeId).ToJson(false));
                _hashtable.Add("@HearingTypeID", hearingTypeId);

                var SP_Result = _Connection.Select<PTXdoHearingStatus>(StoredProcedureNames.usp_getSelectedHearingStatusByHearingType, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Transaction("getSelectedHearingStatusByHearingType-API  Ends successfully ");
                return SP_Result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getSelectedHearingStatusByHearingType-API  Error " + ex);
                throw ex;
            }
        }

        public List<PTXboExistingHearingDetailsForHearingResult> getExistingHearingDetailsForResult(PTXboExistingHearingDetailsForHearingResult objExistingHearingDetailsForHearingResult)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            List<PTXboExistingHearingDetailsForHearingResult> lstExistingHearingDetailsForHearingResult = null;
            try
            {
                Logger.For(this).Transaction("getExistingHearingDetailsForResult-API  Reached " + ((object)objExistingHearingDetailsForHearingResult).ToJson(false));
                SPName = StoredProcedureNames.usp_SEL_ExistingHearingDetailsForHearingResult;

                _hashtable.Add("@CountyName", objExistingHearingDetailsForHearingResult.CountyName);
                _hashtable.Add("@AccountId", objExistingHearingDetailsForHearingResult.AccountId);
                _hashtable.Add("@Taxyear", objExistingHearingDetailsForHearingResult.TaxYear);
                _hashtable.Add("@HearingTypeId", objExistingHearingDetailsForHearingResult.HearingType.HearingTypeId);

                var SP_Result = _Connection.SelectSingle(SPName, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXdtExistingHearingDetailsForResult>()
                );

                lstExistingHearingDetailsForHearingResult = convertdtResultToIndexedExistingHearingDetailsForHearingResultObject(SP_Result, objExistingHearingDetailsForHearingResult);
                Logger.For(this).Transaction("getExistingHearingDetailsForResult-API  Ends successfully ");
                return lstExistingHearingDetailsForHearingResult;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getExistingHearingDetailsForResult-API  Error " + ex);
                throw ex;
            }
        }
        private List<PTXboExistingHearingDetailsForHearingResult> convertdtResultToIndexedExistingHearingDetailsForHearingResultObject(dynamic SP_Result, PTXboExistingHearingDetailsForHearingResult indexedNoticeList)
        {
            List<PTXboExistingHearingDetailsForHearingResult> LstExistingHearingDetailsForResult = null;
            try
            {
                Logger.For(this).Transaction("convertdtResultToIndexedExistingHearingDetailsForHearingResultObject-API  Reached " + ((object)indexedNoticeList).ToJson(false));
                if (SP_Result != null)
                {
                    var dtEntryRecordList = (List<PTXdtExistingHearingDetailsForResult>)SP_Result.Item1;
                    LstExistingHearingDetailsForResult = dtEntryRecordList.Select(b => new PTXboExistingHearingDetailsForHearingResult
                    {
                        CountyName = b.CountyName,
                        AccountId = b.AccountId,
                        HearingType = new PTXdoHearingType() { HearingTypeId = b.HearingTypeId, HearingType = b.HearingType },
                        TaxYear = b.TaxYear,
                        FormalHearingDate = b.FormalHearingDate,
                        FormalHearingTime = b.FormalHearingTime,
                        InformalHearingDate = b.InformalHearingDate,
                        InformalHearingTime = b.InformalHearingTime,
                        InformalPanelDocketID = b.InformalPanelDocketID,
                        HearingDetailsId = b.HearingDetailsId,
                        FormalPanelDocketID = b.FormalPanelDocketID,
                        InformalAssignedAgent = new PTXdoAgent() { AgentId = b.InformalAssignedAgentId },
                        FormalAssignedAgent = new PTXdoAgent() { AgentId = b.FormalAssignedAgentId },
                        InformalBatchPrintDate = b.InformalBatchPrintDate,
                        FormalBatchPrintDate = b.FormalBatchPrintDate,
                        EvidenceDueDate = b.EvidenceDueDate,
                        NoticedLandValue = b.NoticeLandValue,
                        NoticedImprovedValue = b.NoticeImprovedValue,
                        NoticedMarketValue = b.NoticeMarketValue,
                        NoticedTotalValue = b.NoticeTotalValue,
                        PostImprovedValue = b.PostHearingImprovedValue,
                        PostLandValue = b.PostHearingLandValue,
                        PostMarketValue = b.PostHearingMarketValue,
                        PostTotalValue = b.PostHearingTotalValue,
                        PostValueHearingLandValue = b.PostValueHearingLandValue,
                        PostValueHearingImprovedValue = b.PostValueHearingImprovedValue,
                        PostValueHearingMarketValue = b.PostValueHearingMarketValue,
                        PostValueHearingTotalValue = b.PostValueHearingTotalValue,
                        IsCertifiedValues = b.IsCertifiedValues,
                        completionDateAndTime = b.completionDateAndTime,
                        confirmationLetterDateTime = b.confirmationLetterDateTime,
                        confirmationLetterReceivedDate = b.confirmationLetterReceivedDate,
                        DismissalAuthStatusId = b.DismissalAuthStatusId,
                        HearingAgentId = b.HearingAgentId,
                        HearingResolutionId = b.HearingResolutionId,
                        HearingResultReasonCodeId = b.HearingResultReasonCodeId,
                        HearingStatus = new PTXdoHearingStatus() { HearingStatusId = b.HearingStatusId, HearingStatus = b.HearingStatus },
                        strcompletionDateAndTime = b.strcompletionDateAndTime,
                        strconfirmationLetterDateTime = b.strconfirmationLetterDateTime,
                        InvoiceGenerated = b.InvoiceGenerated,
                        InvoiceId = b.InvoiceId,
                        HearingResultId = b.HearingResultId,
                        ResultSenton = b.ResultSenton,
                        HearingFinalized = b.HearingFinalized,
                        HearingResultReportId = b.HearingResultReportId,
                        InvoiceStatusId = b.InvoiceStatusId,
                        RenoticedLandValue = b.RenoticedLandValue,
                        RenoticedImprovedValue = b.RenoticedImprovedValue,
                        RenoticedMarketValue = b.RenoticedMarketValue,
                        RenoticeTotalValue = b.RenoticeTotalValue,
                        ReviewBindingArbitration = b.ReviewBindingArbitration, //Added by Preethi M tfs:38003
                        ExemptInvoice = b.ExemptInvoice, // Added by Marg on 17-03-20,
                        ExemptionId = b.ExemptionId,
                        HearingTrackingId=b.HearingTrackingId
                    }).ToList();
                }
                Logger.For(this).Transaction("convertdtResultToIndexedExistingHearingDetailsForHearingResultObject-API  Ends successfully ");
                return LstExistingHearingDetailsForResult;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("convertdtResultToIndexedExistingHearingDetailsForHearingResultObject-API  Error " + ex);
                throw ex;
            }

            return null;
        }
        public ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int Accountid, int taxYear, int HearingTypeid, int AssignedQueue)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            int Userid = 0;
            int UserRoleid = 0;
            try
            {
                Logger.For(this).Transaction("getNextRecordList-API  Reached " + ((object)indexedNoticeList).ToJson(false));
                if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.EntryQueue)
                {

                }
                else if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.AuditQueue)
                {
                    SPName = StoredProcedureNames.usp_getNextHearingResultDocument_Defect;
                    Userid = indexedNoticeList.auditedUser.Userid;
                    UserRoleid = indexedNoticeList.auditedUserRole.UserRoleid;
                }
                else if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.DefectQueue)
                {
                    //SPName = StoredProcedureNames.usp_getNextDefectHearingNotice;
                    //Userid = indexedNoticeList.defectAssignedUser.Userid;
                    //UserRoleid = indexedNoticeList.defectAssignedUserRole.UserRoleid;
                }
                else if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.ClarificationQueue)
                {
                    //SPName = StoredProcedureNames.usp_getNextClarificationHearingNotice;
                    //Userid = indexedNoticeList.clarificationList[0].RespondedUser.Userid;
                    //UserRoleid = indexedNoticeList.clarificationList[0].RespondedUserRole.UserRoleid;
                }

                _hashtable.Add("@UserID", Userid);
                _hashtable.Add("@UserRoleID", UserRoleid);
                _hashtable.Add("@AssignedQueueID", indexedNoticeList.assignedQueue.NoticeQueueId);
                _hashtable.Add("@IndexedNoticeDocumentId", indexedNoticeList.IndexedNoticeDocumentID);
                _hashtable.Add("@AccountId", indexedNoticeList.Accountid);
                _hashtable.Add("@Val_TaxYear", indexedNoticeList.taxYear);
                _hashtable.Add("@HearingTypeid", indexedNoticeList.HearingTypeid);

                var SP_Result = _Connection.SelectMultiple(SPName, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXdtHearingResults>(),
                    gr => gr.Read<PTXdtHearingResultsClarification>(),
                    gr => gr.Read<PTXdtHearingDetailsRemarks>(),
                    gr => gr.Read<PTXdtHearingTypeForResult>()
                );

                var result = convertdtResultToNextRecordObject(SP_Result, indexedNoticeList, AssignedQueue);
                Logger.For(this).Transaction("getNextRecordList-API  Ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getNextRecordList-API  Error " + ex);
                throw ex;
            }
        }
        private ExpandoObject convertdtResultToNextRecordObject(dynamic SP_Result, PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue)
        {
            var lstHearingDetails = new List<PTXdoHearingDetails>();
            dynamic result = new ExpandoObject();
            PTXdoHearingDetails hearingDetail = new PTXdoHearingDetails();

            try
            {
                Logger.For(this).Transaction("convertdtResultToNextRecordObject-API  Reached " + ((object)indexedNoticeList).ToJson(false));
                if (SP_Result != null)
                {
                    #region IndexedNoticeDocument Object
                    var dtEntryRecordList = (List<PTXdtHearingResults>)SP_Result.Item1;
                    indexedNoticeList = dtEntryRecordList.Select(b => new PTXdoIndexedNoticeDocument
                    {
                        IndexedNoticeDocumentID = b.IndexedNoticeDocumentId,
                        batchCode = b.BatchCode,
                        PWImageID = b.PWImageID,
                        ScannedDateAndTime = b.ScannedDateAndTime,
                        //FileCabinetID = b.PWFileCabinetID, /* FileCabinetID added in NoticeDocumentType */
                        documentType = new PTXdoNoticeDocumentType()
                        {
                            NoticeDocumentTypeId = b.NoticeDocumentTypeId,
                            NoticeDocumentTypeValue = b.NoticedDocumentType,
                            FileCabinetID = b.PWFileCabinetID
                        },
                        Remarks = b.Remarks,
                        taxYear = b.Taxyear,
                        countyCode = b.CountyName,
                        accountNumber = b.AccountNumber,
                        account = new PTXdoAccount()
                        {
                            AccountId = b.AccountId,
                            EndDate = b.EndDate,
                            //propertyDetails = new PTXdoPropertyDetails()
                            //{
                            //    PropertyAddress = new PTXdoAddress()
                            //    {
                            //        addressLine1 = b.PropertyAddress
                            //    }
                            //},
                            /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 Start*/

                            YearlyHearingDetails = (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue) ? (new List<PTXdoYearlyHearingDetails>
                                {
                                    new PTXdoYearlyHearingDetails()
                                    {
                                        propertyDetails = new PTXdoPropertyDetails(){PropertyAddress = new PTXdoAddress(){addressLine1 = b.PropertyAddress}}
                                    }
                                }
                            ) : (getYearlyHearingDetailsById(b.YearlyHearingDetails)), //By Prakash

                            /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 End*/
                            County = new PTXdoCounty()
                            {
                                Countyid = b.Countyid,
                                CountyName = b.CountyName,
                                CountyCode = b.CountyCode
                                //Website = b.Website
                            }
                        },
                        accountStatus = new PTXdoAccountStatus()
                        {
                            Accountstatusid = b.AccountStatusId,
                            Accountstatus = b.Accountstatus
                        },
                        accountProcessingStatus = new PTXdoAccountProcessStatus()
                        {
                            AccProcessstatusid = b.AccountProcessStatusId,
                            AccProcessstatus = b.AccountProcessStatus
                        },
                        protestStatus = new PTXdoProtestCodeValues()
                        {
                            ProtestCodeValuesId = b.ProtestCodeId,
                            ProtestCodeValues = b.ProtestCodeValues
                        },
                        userEnteredHearingResult = new PTXdoUserEnteredHearingResults() /* User Entered values */
                        {
                            UserEnteredHearingResutsID = b.UserEnteredHearingResultsId,
                            TaxYear = b.Taxyear,
                            NoticedLandValue = b.NoticedLandValue,
                            NoticedImprovedValue = b.NoticedImprovedValue,
                            NoticedMarketValue = b.NoticedMarketValue,
                            NoticedTotalValue = b.NoticedTotalvalue,
                            //Added by Balaji Task:38259
                            RenoticedLandValue = b.RenoticedLandValue,
                            RenoticedImprovedValue = b.RenoticedImprovedValue,
                            RenoticedMarketValue = b.RenoticedMarketValue,
                            RenoticeTotalValue = b.RenoticeTotalvalue,
                            PostHearingLandValue = b.PostHearingLandValue,
                            PostHearingImprovedValue = b.PostHearingImprovedValue,
                            PostHearingMarketValue = b.PostHearingMarketValue,
                            PostHearingTotalValue = b.PostHearingTotalValue,
                            HearingAgent = new PTXdoAgent()
                            {
                                AgentId = b.HearingAgentId
                            },
                            ConfirmationLetterDate = b.ConfirmationLetterDate,
                            confirmationLetterReceivedDate = b.confirmationLetterReceivedDate,
                            CompletionDate = b.CompletionDate,
                            CompletionTime = b.CompletionTime,
                            HearingResolution = new PTXdoHearingResolution() { HearingResolutionId = b.HearingResolutionId },
                            ReasonCodes = new PTXdoHearingResultsReasonCode() { HearingResultsReasonCodeId = b.ReasonCodeId },
                            DismissalAuthStatus = new PTXdoHearingDismissalAuthStatus() { HearingDismissalAuthStatusId = b.DismissalAuthStatusId },
                            DefectReason = new PTXdoDocumentDefectCodes() { DocumentDefectCodeId = b.DefectReasonId },
                            HearingResultsReport = new PTXdoHearingResultReport() { HearingResultReportId = b.HearingResultReportId },
                            HearingFinalized = b.HearingFinalized,
                            HearingResultsSentOn = b.HearingResultsSentOn,
                            ResultedGenerated = b.ResultGenerated,
                            InvoiceGenerated = b.InvoiceGenerated,
                            InvoiceID = b.InvoiceId,
                            HearingStatus = new PTXdoHearingStatus() { HearingStatusId = b.HearingStatusId },
                            ExemptInvoice = b.ExemptInvoice,
                            HRInvoiceStatus = new PTXdoHRInvoiceStatus() { HRInvoiceStatusid = b.HRInvoiceStatusid },
                            HearingTrackingCode = new PTXdoHearingTrackingCodes() { HearingTrackingId = b.HearingTrackingCodeId },
                            HearingLevel = new PTXdoHearingLevel() { HearingLevelID = b.HearingLevelId },
                            ByPassInformalHearing = b.ByPassInformalHearing,
                            ExemptionId = b.ExemptionId
                        },
                        assignedQueue = new PTXdoNoticeQueue()
                        {
                            NoticeQueueId = b.AssignedQueueId,
                            NoticeQueue = b.NoticeQueue
                        },
                        processingStatus = new PTXdoNoticeProcessingStatus()
                        {
                            NoticeProcessingStatusId = b.NoticeProcessingStatusId,
                            NoticeProcessingStatus = b.NoticeProcessingStatus
                        },

                        entryUser = new PTXdoUser()
                        {
                            Userid = b.EntryUserId
                        },
                        EntryAssignedBy = new PTXdoUser()
                        {
                            Username = b.EntryAssignBy
                        },
                        entryDataAndTime = b.EntryAssignOn,
                        AuditAssignedBy = new PTXdoUser()
                        {
                            Username = b.AuditAssignBy
                        },
                        auditedDateAndTime = b.AuditAssignOn,
                        DefectAssignedBy = new PTXdoUser()
                        {
                            Username = b.DefectAssignBy
                        },
                        defectAssignedDateAndTime = b.DefectAssignOn,
                        QCAssignedBy = new PTXdoUser()
                        {
                            Username = b.QCAssignBy
                        },
                        qcAssignedDateAndTime = b.QCAssignOn,

                        defectAssignedUser = new PTXdoUser()
                        {
                            Userid = b.DefectAssignedUserId
                        },
                        auditedUser = new PTXdoUser()
                        {
                            Userid = b.AuditedUserId
                        },
                        defectNotice = b.DefectNotice,
                        defectResolved = b.DefectResolved,
                        defectReasonCode = new PTXdoDocumentDefectCodes()
                        {
                            DocumentDefectCodeId = b.DocumentDefectCodeId,
                            DocumentDefectCodes = b.DocumentDefectCodes
                        },
                        defectRectifiedDateAndTime = b.DefectRectifiedDateAndTime,
                        client = new PTXdoClient() { clientId = b.ClientId, ClientNumber = b.ClientNumber },
                        DocumentName = b.DocumentName,
                        qcAssignedUser = new PTXdoUser() { Userid = b.QCAssignedUserId },
                        Disaster = new PTXdoDisaster()
                        {
                            DisasterId = b.DisasterId
                        },
                        IsDisasterProtest=b.IsDisasterProtest
                    }).FirstOrDefault();

                    //foreach (DataRow iem in dsResult.Tables[0].Rows)
                    //{
                    //    if (iem["ExemptInvoice1"].ToString() != "")
                    //    {
                    //        System.Web.HttpContext.Current.Session["Exempt"] = Convert.ToBoolean(iem["ExemptInvoice1"]);
                    //    }

                    //    break;
                    //}

                    //foreach (DataRow itemm in dsResult.Tables[0].Rows)
                    //{
                    //    if (itemm["ReviewBindingArbitration"].ToString() != "")
                    //    {
                    //        System.Web.HttpContext.Current.Session["ReviewBindingArbitration"] = Convert.ToBoolean(itemm["ReviewBindingArbitration"]);
                    //    }
                    //    break;
                    //}

                    var DNDCodes = dtEntryRecordList.Select(x => x.DNDcodes).FirstOrDefault();
                    //List<PTXdoYearlyHearingDetails> item = new List<PTXdoYearlyHearingDetails>();
                    //if (!string.IsNullOrEmpty(DNDCodes))
                    //{
                    //    item.Add(new PTXdoYearlyHearingDetails() { DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes } });
                    //}
                    //if (indexedNoticeList.account != null && (indexedNoticeList.account.YearlyHearingDetails == null || indexedNoticeList.account.YearlyHearingDetails.Count == 0))
                    //{
                    //    indexedNoticeList.account.YearlyHearingDetails = item;
                    //}
                    //else
                    //{
                    //    indexedNoticeList.account.YearlyHearingDetails[0].DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes };
                    //}
                    List<PTXdoYearlyHearingDetails> objYealyHearingList = new List<PTXdoYearlyHearingDetails>();
                    objYealyHearingList = dtEntryRecordList.Select(b => new PTXdoYearlyHearingDetails
                    {
                        YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                        DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes },
                        TaxYear = b.YearlyHearingDetailsId == 0 ? 0 : b.TaxYear
                    }).ToList();
                    if (indexedNoticeList.account != null)
                    {
                        indexedNoticeList.account.YearlyHearingDetails = objYealyHearingList;
                    }

                    #endregion

                    #region Clarification Object
                    /* Clarification Log: First row will be the latest record. */
                    if (SP_Result.Item2 != null && SP_Result.Item2.Count > 0)
                    {
                        var dtClarificationList = (List<PTXdtHearingResultsClarification>)SP_Result.Item2;

                        List<PTXdoNoticeClarification> clarificationList = dtClarificationList.Select(a => new PTXdoNoticeClarification
                        {
                            NoticeClarificationId = a.NoticeClarificationId,
                            IndexedNoticeDocument = new PTXdoIndexedNoticeDocument()
                            {
                                IndexedNoticeDocumentID = a.IndexedNoticeDocumentId
                            },
                            RequestedUser = new PTXdoUser()
                            {
                                Userid = a.RequestedUserId,
                                Username = a.RequestedUser,
                                Firstname = a.RequestedUserFirstName,
                                Lastname = a.RequestedUserLastName
                            },
                            Request = a.Request,
                            RequestedDateAndTime = a.RequestedDateAndTime,
                            RespondedUser = new PTXdoUser()
                            {
                                Userid = a.RespondedUserId,
                                Username = a.RespondedUser,
                                Firstname = a.RespondedUserFirstName,
                                Lastname = a.RespondedUserLastName
                            },
                            Response = a.Response,
                            RespondedDateAndTime = a.RespondedDateAndTime,
                            ClarificationAssignedBy = new PTXdoUser()
                            {
                                Username = a.ClarificationAssignedBy
                            },
                            ClarificationAssignedOn = a.ClarificationAssignedOn

                        }).ToList();

                        indexedNoticeList.clarificationList = clarificationList;

                        if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)    //By Mano
                        {
                            /* Get the Clarification list*/
                            indexedNoticeList.clarificationList = clarificationList.Where(a => a.IndexedNoticeDocument.IndexedNoticeDocumentID == indexedNoticeList.IndexedNoticeDocumentID).ToList();
                        }
                    }
                    #endregion

                    #region Hearing Details Remarks
                    if (SP_Result.Item3 != null && SP_Result.Item3.Count > 0)
                    {
                        var dtHearingResultRemarks = (List<PTXdtHearingDetailsRemarks>)SP_Result.Item3;

                        List<PTXdoHearingDetailsRemarks> hearingDetailsRemarksList = dtHearingResultRemarks.OrderByDescending(c => c.UpdatedDatetime).Select(c => new PTXdoHearingDetailsRemarks
                        {
                            HearingDetailsRemarksId = c.HearingDetailsRemarksId,
                            HearingDetails = new PTXdoHearingDetails() { HearingDetailsId = c.HearingDetailsId },
                            Remarks = c.Remarks,
                            UpdatedBy = new PTXdoUser()
                            {
                                Userid = c.UpdatedBy,
                                Username = c.Username
                            },
                            UpdatedDateTime = c.UpdatedDatetime
                        }).ToList();

                        hearingDetail.HearingDetailsRemarks = hearingDetailsRemarksList;

                        /*Assign Hearing Details Remarks*/
                        //foreach (var hearingDetails in hearingDetailsRemarksList)
                        //{
                        //    var objHearingDetails = Repository<PTXdoHearingDetails>.GetQuery().FirstOrDefault(q => q.HearingDetailsId == hearingDetails.HearingDetails.HearingDetailsId);
                        //    objHearingDetails.HearingDetailsRemarks = hearingDetailsRemarksList.Where(a => a.HearingDetails.HearingDetailsId == objHearingDetails.HearingDetailsId).ToList();
                        //    lstHearingDetails.Add(objHearingDetails);
                        //}

                        /*Assign Hearing Details Remarks*/
                        foreach (var hearingDetails in hearingDetailsRemarksList)
                        {
                            hearingDetail.HearingDetailsRemarks = hearingDetailsRemarksList.Where(a => a.HearingDetails.HearingDetailsId == hearingDetail.HearingDetailsId).ToList();
                            lstHearingDetails.Add(hearingDetail);
                        }
                    }
                    #endregion

                    #region Hearing Type
                    if (SP_Result.Item4 != null && SP_Result.Item4.Count > 0)
                    {
                        var dtHearingTypeForResult = (List<PTXdtHearingTypeForResult>)SP_Result.Item4;

                        List<PTXdoUserEnteredHearingTypeForResult> UserEnteredHearingTypeForResult = dtHearingTypeForResult.Select(c => new PTXdoUserEnteredHearingTypeForResult
                        {
                            hearingType = new PTXdoHearingType() { HearingTypeId = c.HearingTypeId },
                            userEnteredHearingTypeId = c.UserEnteredHearingTypeId,

                            userEnteredHearingResults = new PTXdoUserEnteredHearingResults()
                            {
                                UserEnteredHearingResutsID = c.UserEnteredHearingResultsId
                            },
                            UpdatedBy = new PTXdoUser() { Userid = c.UpdatedBy },
                            UpdatedDateTime = c.UpdatedDateTime
                        }).ToList();

                        indexedNoticeList.userEnteredHearingResult.UserEnteredHearingTypeForResultList = UserEnteredHearingTypeForResult;
                    }
                    #endregion
                    result.indexedNoticeList = indexedNoticeList;

                    //if (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue)
                    //    result.hearingNoticeRemarksList = hearingNoticeRemarksList;
                    //else
                    //    result.HearingDetails = HearingDetails;

                    Logger.For(this).Transaction("convertdtResultToNextRecordObject-API  Ends successfully ");

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("convertdtResultToNextRecordObject-API  Error " + ex);
                throw ex;
            }

            return null;
        }

        public List<PTXboHearingResultsPWDocumentSearch> getDocumentDetailsFromPaperwise(PTXboHearingResultsPWDocumentSearch objpwDocumentDetailsSearch)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            List<PTXboHearingResultsPWDocumentSearch> lstPTXboHearingResultsPWDocumentSearch = null;
            try
            {
                Logger.For(this).Transaction("getDocumentDetailsFromPaperwise-API  Reached " + ((object)objpwDocumentDetailsSearch).ToJson(false));
                SPName = StoredProcedureNames.usp_SEL_getDocumentDetailsFromPaperwise;

                _hashtable.Add("@DocType", objpwDocumentDetailsSearch.documentType);
                _hashtable.Add("@AccountNumber", objpwDocumentDetailsSearch.accountNumber);
                _hashtable.Add("@County", objpwDocumentDetailsSearch.countyName);
                _hashtable.Add("@TaxYear", objpwDocumentDetailsSearch.taxYear);
                _hashtable.Add("@FileCabinetID", objpwDocumentDetailsSearch.fileCabinetId);

                var SP_Result = _Connection.SelectSingle(SPName, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXboHearingResultsPWDocumentSearch>()
                );

                lstPTXboHearingResultsPWDocumentSearch = convertdtResultTopwDocumentDetailsSearchResultObject(SP_Result, objpwDocumentDetailsSearch);
                Logger.For(this).Transaction("getDocumentDetailsFromPaperwise-API  Ends successfully ");
                return lstPTXboHearingResultsPWDocumentSearch;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getDocumentDetailsFromPaperwise-API  Error " + ex);
                throw ex;
            }
        }
        private List<PTXboHearingResultsPWDocumentSearch> convertdtResultTopwDocumentDetailsSearchResultObject(dynamic SP_Result, PTXboHearingResultsPWDocumentSearch hearingResultsPWDocumentSearch)
        {
            List<PTXboHearingResultsPWDocumentSearch> lstPTXboHearingResultsPWDocumentSearch = null;
            try
            {
                Logger.For(this).Transaction("convertdtResultTopwDocumentDetailsSearchResultObject-API  Reached " + ((object)hearingResultsPWDocumentSearch).ToJson(false));
                if (SP_Result != null)
                {
                    var dtEntryRecordList = (List<PTXboHearingResultsPWDocumentSearch>)SP_Result.Item1;
                    lstPTXboHearingResultsPWDocumentSearch = dtEntryRecordList.Select(b => new PTXboHearingResultsPWDocumentSearch
                    {
                        taxYear = b.taxYear,
                        documentType = b.documentType,
                        pwImageId = b.pwImageId,
                        documentName = b.documentName,
                        accountNumber = b.accountNumber,
                        countyName = b.countyName
                    }).ToList();
                }
                Logger.For(this).Transaction("convertdtResultTopwDocumentDetailsSearchResultObject-API  Ends successfully ");
                return lstPTXboHearingResultsPWDocumentSearch;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("convertdtResultTopwDocumentDetailsSearchResultObject-API  Error " + ex);
                throw ex;
            }
            return null;
        }

        public bool updateHearingResultRecordList(PTXdoIndexedNoticeDocument objPTXdoIndexedNoticeDocument)
        {
            try
            {
                Logger.For(this).Transaction("updateHearingResultRecordList-API  Reached " + ((object)objPTXdoIndexedNoticeDocument).ToJson(false));
                bool isSuccess = false;
                string errorMessage = string.Empty;

                var indexmodel = new PTXdoIndexedNoticeDocument();
                //PTXboIndexedNoticeDocument objPTXboIndexedNoticeDocument = new PTXboIndexedNoticeDocument();

                //var objIndexedNoticeDocument = Repository<PTXdoIndexedNoticeDocument>.GetQuery().Where(a => a.IndexedNoticeDocumentID == IndexedNoticeDocumentID).FirstOrDefault();

                int IndexedNoticeDocumentID = objPTXdoIndexedNoticeDocument.IndexedNoticeDocumentID;
                List<PTXdoIndexedNoticeDocument> lstIndexedNoticeDocument = new List<PTXdoIndexedNoticeDocument>();
                lstIndexedNoticeDocument = new List<PTXdoIndexedNoticeDocument>() { objPTXdoIndexedNoticeDocument };
                var objIndexedNoticeDocument = lstIndexedNoticeDocument.Where(a => a.IndexedNoticeDocumentID == IndexedNoticeDocumentID).FirstOrDefault();

                List<PTXdoAccount> lstdoAccount = new List<PTXdoAccount>();

                //objPTXdoIndexedNoticeDocument.CopyTo(indexmodel, new string[] { });

                //var objIndexedNoticeDocument = Repository<PTXboIndexedNoticeDocument>.GetQuery().Where(a => a.IndexedNoticeDocumentID == IndexedNoticeDocumentID).FirstOrDefault();

                if (objPTXdoIndexedNoticeDocument.client.clientId != 0)
                {
                    objIndexedNoticeDocument.client = new PTXdoClient() { clientId = objPTXdoIndexedNoticeDocument.client.clientId };
                }
                if (objPTXdoIndexedNoticeDocument.account.AccountId != 0)
                {
                    objIndexedNoticeDocument.account = new PTXdoAccount() { AccountId = objPTXdoIndexedNoticeDocument.account.AccountId };
                    lstdoAccount = null;
                    lstdoAccount = new List<PTXdoAccount>() { objPTXdoIndexedNoticeDocument.account };
                }
                if (!string.IsNullOrEmpty(objPTXdoIndexedNoticeDocument.accountNumber))
                {
                    objIndexedNoticeDocument.accountNumber = objPTXdoIndexedNoticeDocument.accountNumber;
                }

                if (objIndexedNoticeDocument == null)
                {
                    /* HRDS003: IndexedNoticeDocumentID is null */
                    //PTXdsCommon.CurrentInstance.GetUserMessage("HRDS003", out errorMessage);
                    return false;
                }

                if (objIndexedNoticeDocument.assignedQueue != null)
                {
                    objIndexedNoticeDocument.PreviousNoticeQueueId = new PTXdoNoticeQueue() { NoticeQueueId = objPTXdoIndexedNoticeDocument.assignedQueue.NoticeQueueId };
                }

                //objIndexedNoticeDocument.exemptInvoice = indexedNoticeList.exemptInvoice;

                if (objPTXdoIndexedNoticeDocument.userEnteredHearingResult != null)
                {

                    //if (indexedNoticeList.assignedQueue.NoticeQueueId == 2 || indexedNoticeList.assignedQueue.NoticeQueueId == 6)
                    //{

                    //}
                    //else
                    //{
                    //    UpdateHearingDetails(ref indexedNoticeList, out errorMessage);
                    //}


                    // indexedNoticeList.

                    /* objIndexedNoticeDocument.userEnteredValueNoticeData is From DB; indexedNoticeList is User given input */
                    updateUserEnteredHearingResultData(objIndexedNoticeDocument.userEnteredHearingResult, ref objPTXdoIndexedNoticeDocument, out errorMessage);
                    objPTXdoIndexedNoticeDocument.userEnteredHearingResult = new PTXdoUserEnteredHearingResults() { UserEnteredHearingResutsID = objPTXdoIndexedNoticeDocument.userEnteredHearingResult.UserEnteredHearingResutsID };
                    //Added by Boopathi.S
                    if (objPTXdoIndexedNoticeDocument.userEnteredHearingResult.HRInvoiceStatus != null)
                    {
                        indexmodel.userEnteredHearingResult.HRInvoiceStatus = objPTXdoIndexedNoticeDocument.userEnteredHearingResult.HRInvoiceStatus;
                    }
                    else
                    {
                        indexmodel.userEnteredHearingResult.HRInvoiceStatus = null;
                    }
                    if (indexmodel.userEnteredHearingResult.HearingFinalized == true)
                    {
                        if (indexmodel.userEnteredHearingResult.ExemptInvoice == false)
                        {
                            //var InvoiceGr = Repository<PTXdoAccount>.GetQuery().Where(a => a.AccountId == objPTXdoIndexedNoticeDocument.account.AccountId).Select(x => x.Group).FirstOrDefault();

                            var InvoiceGr = lstdoAccount.Where(a => a.AccountId == objPTXdoIndexedNoticeDocument.account.AccountId).Select(x => x.Group).FirstOrDefault();

                            if (InvoiceGr != null)
                            {
                                if (InvoiceGr.termsList[0].IsSpecializedTerm == true)
                                {
                                    indexmodel.userEnteredHearingResult.HRInvoiceStatus = new PTXdoHRInvoiceStatus() { HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceinSpecialQueue.GetId() };
                                }
                            }
                        }
                    }

                }
                Logger.For(this).Transaction("updateHearingResultRecordList-API  Ends successfully ");
                return isSuccess;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("updateHearingResultRecordList-API  Error " + ex);
                throw ex;
            }
        }

        private bool updateUserEnteredHearingResultData(PTXdoUserEnteredHearingResults objUserEnteredFromDB, ref PTXdoIndexedNoticeDocument indexedNoticeList, out string errorMessage)
        {
            bool retval = false;
            errorMessage = string.Empty;

            try
            {
                Logger.For(this).Transaction("updateUserEnteredHearingResultData-API  Reached " + ((object)objUserEnteredFromDB).ToJson(false));
                PTXdoUserEnteredHearingResultsRemarks objRemarks = new PTXdoUserEnteredHearingResultsRemarks();
                PTXdoUserEnteredHearingTypeForResult objHearingType = new PTXdoUserEnteredHearingTypeForResult();

                if (objUserEnteredFromDB == null)
                {
                    /* Create new object */
                    objUserEnteredFromDB = new PTXdoUserEnteredHearingResults();
                }
                if (indexedNoticeList.userEnteredHearingResult != null)
                {
                    /* Assign the input from user values into FromDB object */

                    /* Value Data*/
                    objUserEnteredFromDB.TaxYear = indexedNoticeList.userEnteredHearingResult.TaxYear;
                    objUserEnteredFromDB.NoticedLandValue = indexedNoticeList.userEnteredHearingResult.NoticedLandValue;
                    objUserEnteredFromDB.NoticedImprovedValue = indexedNoticeList.userEnteredHearingResult.NoticedImprovedValue;
                    objUserEnteredFromDB.NoticedMarketValue = indexedNoticeList.userEnteredHearingResult.NoticedMarketValue;
                    objUserEnteredFromDB.NoticedTotalValue = indexedNoticeList.userEnteredHearingResult.NoticedTotalValue;

                    /* User Entered Hearing Results */
                    objUserEnteredFromDB.NoChange = indexedNoticeList.userEnteredHearingResult.NoChange;
                    objUserEnteredFromDB.PostHearingLandValue = indexedNoticeList.userEnteredHearingResult.PostHearingLandValue;
                    objUserEnteredFromDB.PostHearingImprovedValue = indexedNoticeList.userEnteredHearingResult.PostHearingImprovedValue;
                    objUserEnteredFromDB.PostHearingMarketValue = indexedNoticeList.userEnteredHearingResult.PostHearingMarketValue;
                    objUserEnteredFromDB.PostHearingTotalValue = indexedNoticeList.userEnteredHearingResult.PostHearingTotalValue;
                    objUserEnteredFromDB.HearingAgent = (indexedNoticeList.userEnteredHearingResult.HearingAgent == null || indexedNoticeList.userEnteredHearingResult.HearingAgent.AgentId == 0) ? null : new PTXdoAgent() { AgentId = indexedNoticeList.userEnteredHearingResult.HearingAgent.AgentId };
                    objUserEnteredFromDB.ConfirmationLetterDate = indexedNoticeList.userEnteredHearingResult.ConfirmationLetterDate != default(DateTime) ? indexedNoticeList.userEnteredHearingResult.ConfirmationLetterDate : (DateTime?)null;
                    objUserEnteredFromDB.confirmationLetterReceivedDate = indexedNoticeList.userEnteredHearingResult.confirmationLetterReceivedDate != default(DateTime) ? indexedNoticeList.userEnteredHearingResult.confirmationLetterReceivedDate : (DateTime?)null;//tfsid:25572
                    objUserEnteredFromDB.CompletionDate = indexedNoticeList.userEnteredHearingResult.CompletionDate != default(DateTime) ? indexedNoticeList.userEnteredHearingResult.CompletionDate : (DateTime?)null;
                    objUserEnteredFromDB.CompletionTime = indexedNoticeList.userEnteredHearingResult.CompletionTime;
                    objUserEnteredFromDB.HearingResolution = (indexedNoticeList.userEnteredHearingResult.HearingResolution == null || indexedNoticeList.userEnteredHearingResult.HearingResolution.HearingResolutionId == 0) ? null : new PTXdoHearingResolution() { HearingResolutionId = indexedNoticeList.userEnteredHearingResult.HearingResolution.HearingResolutionId };
                    objUserEnteredFromDB.ReasonCodes = (indexedNoticeList.userEnteredHearingResult.ReasonCodes == null || indexedNoticeList.userEnteredHearingResult.ReasonCodes.HearingResultsReasonCodeId == 0) ? null : new PTXdoHearingResultsReasonCode() { HearingResultsReasonCodeId = indexedNoticeList.userEnteredHearingResult.ReasonCodes.HearingResultsReasonCodeId };
                    objUserEnteredFromDB.DismissalAuthStatus = (indexedNoticeList.userEnteredHearingResult.DismissalAuthStatus == null || indexedNoticeList.userEnteredHearingResult.DismissalAuthStatus.HearingDismissalAuthStatusId == 0) ? null : new PTXdoHearingDismissalAuthStatus() { HearingDismissalAuthStatusId = indexedNoticeList.userEnteredHearingResult.DismissalAuthStatus.HearingDismissalAuthStatusId };
                    objUserEnteredFromDB.DefectReason = (indexedNoticeList.userEnteredHearingResult.DefectReason == null || indexedNoticeList.userEnteredHearingResult.DefectReason.DocumentDefectCodeId == 0) ? null : new PTXdoDocumentDefectCodes() { DocumentDefectCodeId = indexedNoticeList.userEnteredHearingResult.DefectReason.DocumentDefectCodeId };
                    objUserEnteredFromDB.HearingResultsReport = (indexedNoticeList.userEnteredHearingResult.HearingResultsReport == null || indexedNoticeList.userEnteredHearingResult.HearingResultsReport.HearingResultReportId == 0) ? null : new PTXdoHearingResultReport() { HearingResultReportId = indexedNoticeList.userEnteredHearingResult.HearingResultsReport.HearingResultReportId };
                    objUserEnteredFromDB.HearingFinalized = indexedNoticeList.userEnteredHearingResult.HearingFinalized;
                    objUserEnteredFromDB.HearingResultsSentOn = indexedNoticeList.userEnteredHearingResult.HearingResultsSentOn != default(DateTime) ? indexedNoticeList.userEnteredHearingResult.HearingResultsSentOn : (DateTime?)null;
                    objUserEnteredFromDB.ResultedGenerated = indexedNoticeList.userEnteredHearingResult.ResultedGenerated;
                    objUserEnteredFromDB.InvoiceGenerated = indexedNoticeList.userEnteredHearingResult.InvoiceGenerated;
                    objUserEnteredFromDB.InvoiceID = indexedNoticeList.userEnteredHearingResult.InvoiceID;
                    objUserEnteredFromDB.HearingStatus = (indexedNoticeList.userEnteredHearingResult.HearingStatus == null || indexedNoticeList.userEnteredHearingResult.HearingStatus.HearingStatusId == 0) ? null : new PTXdoHearingStatus() { HearingStatusId = indexedNoticeList.userEnteredHearingResult.HearingStatus.HearingStatusId };
                    objUserEnteredFromDB.UpdatedBy = (indexedNoticeList.userEnteredHearingResult.UpdatedBy == null || indexedNoticeList.userEnteredHearingResult.UpdatedBy.Userid == 0) ? null : new PTXdoUser() { Userid = indexedNoticeList.userEnteredHearingResult.UpdatedBy.Userid };
                    //objUserEnteredFromDB.UpdatedDateTime = indexedNoticeList.userEnteredHearingResult.UpdatedDateTime != default(DateTime) ? indexedNoticeList.userEnteredHearingResult.UpdatedDateTime : (DateTime?)null;
                    objUserEnteredFromDB.UpdatedDateTime = System.DateTime.Now;
                    objUserEnteredFromDB.IsValueNoticeUpdateRequired = indexedNoticeList.userEnteredHearingResult.IsValueNoticeUpdateRequired;
                    objUserEnteredFromDB.ExemptInvoice = indexedNoticeList.userEnteredHearingResult.ExemptInvoice;
                    objUserEnteredFromDB.ReviewBindingArbitration = indexedNoticeList.userEnteredHearingResult.ReviewBindingArbitration; //Added by Preethi M tfs:38003

                    /* Hearing Result Remarks */

                    if ((objUserEnteredFromDB.UserEnteredHearingResultRemarksList == null) || (objUserEnteredFromDB.UserEnteredHearingResultRemarksList.Count == 0))
                    {
                        if ((indexedNoticeList.userEnteredHearingResult.UserEnteredHearingResultRemarksList != null) &&
                            (indexedNoticeList.userEnteredHearingResult.UserEnteredHearingResultRemarksList.Count > 0))
                        {
                            indexedNoticeList.userEnteredHearingResult.UserEnteredHearingResultRemarksList[0].userEnteredHearingResultsRemarks = objUserEnteredFromDB;
                            objUserEnteredFromDB.UserEnteredHearingResultRemarksList = indexedNoticeList.userEnteredHearingResult.UserEnteredHearingResultRemarksList;
                        }
                    }
                    else
                    {

                        if ((indexedNoticeList.userEnteredHearingResult.UserEnteredHearingResultRemarksList != null) &&
                      (indexedNoticeList.userEnteredHearingResult.UserEnteredHearingResultRemarksList.Count > 0))
                        {
                            var inUserEnteredHearingResultRemarksList = indexedNoticeList.userEnteredHearingResult.UserEnteredHearingResultRemarksList;
                            var inUserEnteredFromDB = objUserEnteredFromDB.UserEnteredHearingResultRemarksList;

                            /* Find remarks not in DB */
                            var result = inUserEnteredHearingResultRemarksList.Where(p => !inUserEnteredFromDB.Any(p2 => p2.userEnteredHearingResultsRemarksId == p.userEnteredHearingResultsRemarksId));

                            foreach (PTXdoUserEnteredHearingResultsRemarks objUserEnteredHearingResultRemarks in result)
                            {
                                objRemarks = new PTXdoUserEnteredHearingResultsRemarks();
                                objRemarks.remarks = objUserEnteredHearingResultRemarks.remarks;
                                objRemarks.updatedBy = objUserEnteredHearingResultRemarks.updatedBy == null ? null : new PTXdoUser() { Userid = objUserEnteredHearingResultRemarks.updatedBy.Userid };
                                //objRemarks.updatedDateTime = objUserEnteredHearingResultRemarks.updatedDateTime;
                                objRemarks.updatedDateTime = System.DateTime.Now;
                                objRemarks.userEnteredHearingResultsRemarks = objUserEnteredFromDB;
                                /* Add the not in DB rows into FromDB object */
                                objUserEnteredFromDB.UserEnteredHearingResultRemarksList.Add(objRemarks);
                            }


                        }
                    }

                    /* Hearing Type */
                    if (indexedNoticeList.userEnteredHearingResult.UserEnteredHearingTypeForResultList != null)
                    {

                        if (objUserEnteredFromDB.UserEnteredHearingTypeForResultList != null)
                        {
                            foreach (PTXdoUserEnteredHearingTypeForResult objHearingTypeForResult in indexedNoticeList.userEnteredHearingResult.UserEnteredHearingTypeForResultList)
                            {
                                objHearingType = new PTXdoUserEnteredHearingTypeForResult();
                                objHearingType.UpdatedBy = (objHearingTypeForResult.UpdatedBy == null || objHearingTypeForResult.UpdatedBy.Userid == 0) ? null : new PTXdoUser() { Userid = objHearingTypeForResult.UpdatedBy.Userid };
                                //objHearingType.UpdatedDateTime = objHearingTypeForResult.UpdatedDateTime != default(DateTime) ? objHearingTypeForResult.UpdatedDateTime : (DateTime?)null;
                                objHearingType.UpdatedDateTime = System.DateTime.Now;
                                objHearingType.hearingType = (objHearingTypeForResult.hearingType == null || objHearingTypeForResult.hearingType.HearingTypeId == 0) ? null : new PTXdoHearingType() { HearingTypeId = objHearingTypeForResult.hearingType.HearingTypeId };
                                objHearingType.userEnteredHearingResults = new PTXdoUserEnteredHearingResults();
                                objHearingType.userEnteredHearingResults = objUserEnteredFromDB;
                                if (objHearingTypeForResult.userEnteredHearingTypeId != 0)
                                    objHearingType.userEnteredHearingTypeId = objHearingTypeForResult.userEnteredHearingTypeId;

                                objUserEnteredFromDB.UserEnteredHearingTypeForResultList.Add(objHearingType);
                            }
                        }
                        else
                        {
                            List<PTXdoUserEnteredHearingTypeForResult> lstHearingTypeForResult = new List<PTXdoUserEnteredHearingTypeForResult>();
                            foreach (PTXdoUserEnteredHearingTypeForResult objHearingTypeForResult in indexedNoticeList.userEnteredHearingResult.UserEnteredHearingTypeForResultList)
                            {
                                objHearingTypeForResult.userEnteredHearingResults = objUserEnteredFromDB;
                                lstHearingTypeForResult.Add(objHearingTypeForResult);
                            }
                            objUserEnteredFromDB.UserEnteredHearingTypeForResultList = lstHearingTypeForResult;
                        }


                        //added by Boopathi
                        //HR Invoice Status Update
                        if (objUserEnteredFromDB.HearingFinalized)
                        {
                            if (objUserEnteredFromDB.ExemptInvoice)
                            {
                                //ExemptInvoice
                                objUserEnteredFromDB.HRInvoiceStatus = new PTXdoHRInvoiceStatus() { HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.ExemptInvoice.GetId() };
                            }
                            else
                            {
                                if (objUserEnteredFromDB.UserEnteredHearingTypeForResultList != null)
                                {
                                    var objHearingResultFromDB = objUserEnteredFromDB.UserEnteredHearingTypeForResultList.FirstOrDefault(x => x.hearingType.HearingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumHearingType.ValueHearing.GetId()
                                        || x.hearingType.HearingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumHearingType.ExemptionHearing.GetId());
                                    if (objHearingResultFromDB == null)
                                    {
                                        objHearingResultFromDB = objUserEnteredFromDB.UserEnteredHearingTypeForResultList.FirstOrDefault(x => x.hearingType.HearingTypeId != Spartaxx.DataObjects.Enumerators.PTXenumHearingType.ValueHearing.GetId()
                                             && x.hearingType.HearingTypeId != Spartaxx.DataObjects.Enumerators.PTXenumHearingType.ExemptionHearing.GetId()
                                             );
                                    }
                                    //Correction Motion HearingTypes
                                    if (objHearingResultFromDB != null)
                                    {
                                        if ((objHearingResultFromDB.hearingType != null) && objHearingResultFromDB.hearingType.HearingTypeId >= 3 && objHearingResultFromDB.hearingType.HearingTypeId <= 8)
                                        {
                                            PTXdoHearingDetails objHD = new PTXdoHearingDetails();
                                            //PTXdoHearingResult objHR = new PTXdoHearingResult();
                                            var taxyear = objUserEnteredFromDB.TaxYear;
                                            var accid = indexedNoticeList.account != null ? indexedNoticeList.account.AccountId : 0;

                                            //var objYHD = Repository<PTXdoYearlyHearingDetails>.GetQuery().FirstOrDefault(x => x.Account.AccountId == accid && x.TaxYear == taxyear);
                                            List<PTXdoYearlyHearingDetails> lstYHD = new List<PTXdoYearlyHearingDetails>();
                                            //lstYHD = indexedNoticeList.account.YearlyHearingDetails.ToList();
                                            lstYHD = getYearlyHearingDetailsByAccountId(accid, taxyear);
                                            int YearlyHearingDetailsId = Convert.ToInt32(lstYHD[0].YearlyHearingDetailsId.ToString());


                                            //objHD = Repository<PTXdoHearingDetails>.GetQuery().FirstOrDefault(x => x.YearlyHearingDetails.YearlyHearingDetailsId == objYHD.YearlyHearingDetailsId 
                                            //                                                                    && x.HearingType.HearingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumHearingType.ValueHearing.GetId());
                                            List<PTXdoHearingDetails> lstHD = new List<PTXdoHearingDetails>();
                                            //lstHD = getHearingDetailsYearly(YearlyHearingDetailsId, Spartaxx.DataObjects.Enumerators.PTXenumHearingType.ValueHearing.GetId());
                                            int HearingDetailsId = lstHD[0].HearingDetailsId;

                                            if (objHD != null)
                                            {
                                                //objHR = Repository<PTXdoHearingResult>.GetQuery().FirstOrDefault(x => x.HearingDetails.HearingDetailsId == objHD.HearingDetailsId);
                                                List<PTXdoHearingResult> lstHR = new List<PTXdoHearingResult>();
                                                //lstHR = getHearingResultByHearingDetailsId(HearingDetailsId);
                                                int HearingTypeId = Convert.ToInt32(lstHR[0].HearingType.HearingTypeId.ToString());
                                                double PostHearingTotalValue = Convert.ToDouble(lstHR[0].PostHearingTotalValue.ToString());

                                                //if (objHR.HearingType.HearingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumHearingType.ValueHearing.GetId())
                                                if (HearingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumHearingType.ValueHearing.GetId())
                                                {
                                                    //var obj = Repository<PTXdoHearingDetails>.GetQuery().Where(x=>x.HearingType == YearlyHearingDetails1.)
                                                    double intialvalue = PostHearingTotalValue; //Convert.ToDouble(objHR.PostHearingTotalValue);//VH POSTHEARING TOTAL VALUE
                                                    double finalvalue = Convert.ToDouble(objUserEnteredFromDB.PostHearingTotalValue);
                                                    if (intialvalue - finalvalue <= 0)
                                                    {
                                                        //Added By Pavithra.B on 02Feb2016 -If Reduction <=0 and Flatfee =0 then No reduction else null.
                                                        //var InvoiceGroup = Repository<PTXdoAccount>.GetQuery().Where(a => a.AccountId == accid).Select(x => x.Group).FirstOrDefault();
                                                        List<PTXdoAccount> lstdoAccount = new List<PTXdoAccount>();
                                                        lstdoAccount = null;
                                                        lstdoAccount = new List<PTXdoAccount>() { indexedNoticeList.account };
                                                        var InvoiceGroup = lstdoAccount.Where(a => a.AccountId == accid).Select(x => x.Group).FirstOrDefault();

                                                        var termInvoice = new PTXdoTerms();
                                                        if (InvoiceGroup != null)
                                                        {
                                                            termInvoice = InvoiceGroup.termsList.Where(x => x.InvoiceFrequency.InvoiceFrequencyID == 1 && x.termsType.Termstypeid == Enumerators.PTXenumInvoiceType.Standard.GetId() && x.IsSpecializedTerm != true).FirstOrDefault();
                                                        }
                                                        if (termInvoice != null && Convert.ToDecimal(termInvoice.FlatFee) == 0) // Modified by mohan.d on 5/4/2017 Task 30382:Error Message While Trying to Update Arbitration Tax
                                                        {
                                                            //No Reduction
                                                            objUserEnteredFromDB.HRInvoiceStatus = new PTXdoHRInvoiceStatus() { HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.NoReduction.GetId() };
                                                        }
                                                        else
                                                        {
                                                            objUserEnteredFromDB.HRInvoiceStatus = null;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        objUserEnteredFromDB.HRInvoiceStatus = null;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                objUserEnteredFromDB.HRInvoiceStatus = null;
                                            }
                                        }
                                        else
                                        {
                                            var tewst = objUserEnteredFromDB.TaxYear;
                                            var tewsta = indexedNoticeList.accountNumber;
                                            /* Check value notice details are available */
                                            if (objUserEnteredFromDB.NoticedTotalValue - objUserEnteredFromDB.PostHearingTotalValue <= 0)
                                            {
                                                //Added By Pavithra.B on 02Feb2016 -If Reduction <=0 and Flatfee =0 then No reduction else null.
                                                var accid = indexedNoticeList.account != null ? indexedNoticeList.account.AccountId : 0;
                                                //var InvoiceGroup = Repository<PTXdoAccount>.GetQuery().Where(a => a.AccountId == accid).Select(x => x.Group).FirstOrDefault();
                                                List<PTXdoAccount> lstdoAccount = new List<PTXdoAccount>();
                                                lstdoAccount = null;
                                                lstdoAccount = new List<PTXdoAccount>() { indexedNoticeList.account };
                                                var InvoiceGroup = lstdoAccount.Where(a => a.AccountId == accid).Select(x => x.Group).FirstOrDefault();

                                                var termInvoice = new PTXdoTerms();
                                                if (InvoiceGroup != null)
                                                {
                                                    termInvoice = InvoiceGroup.termsList.Where(x => x.InvoiceFrequency.InvoiceFrequencyID == 1 && x.termsType.Termstypeid == Enumerators.PTXenumInvoiceType.Standard.GetId() && x.IsSpecializedTerm != true).FirstOrDefault();
                                                }
                                                if (termInvoice != null && Convert.ToDecimal(termInvoice.FlatFee) == 0) // Modified by mohan.d on 5/4/2017 Task 30382:Error Message While Trying to Update Arbitration Tax
                                                {
                                                    //No Reduction
                                                    objUserEnteredFromDB.HRInvoiceStatus = new PTXdoHRInvoiceStatus() { HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.NoReduction.GetId() };
                                                }
                                                else
                                                {
                                                    objUserEnteredFromDB.HRInvoiceStatus = null;
                                                }
                                            }
                                            else
                                            {
                                                objUserEnteredFromDB.HRInvoiceStatus = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        /* Assign the updated value to Index notice object */
                        //Repository<PTXdoUserEnteredHearingResults>.SaveOrUpdate(objUserEnteredFromDB);

                        indexedNoticeList.userEnteredHearingResult = objUserEnteredFromDB;
                        retval = true;
                    }
                    else
                    {
                        indexedNoticeList.userEnteredHearingResult = objUserEnteredFromDB;
                        retval = false;
                    }
                }
                Logger.For(this).Transaction("updateUserEnteredHearingResultData-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("updateUserEnteredHearingResultData-API  Error " + ex);
                throw ex;
            }
        }


        private List<PTXdoYearlyHearingDetails> getYearlyHearingDetailsByAccountId(int AccountId, int TaxYear)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                string objInput = "AccountId : " + AccountId.ToString() + "-" + " TaxYear : " + TaxYear.ToString();
                Logger.For(this).Transaction("getYearlyHearingDetailsByAccountId-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@AccountId", AccountId);
                _hashtable.Add("@TaxYear", TaxYear);

                var SP_Result = _Connection.Select<PTXdoYearlyHearingDetails>(StoredProcedureNames.usp_getYearlyHearingDetailsByAccountId, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Transaction("getYearlyHearingDetailsByAccountId-API  Ends successfully ");
                return SP_Result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getYearlyHearingDetailsByAccountId-API  Error " + ex);
                throw ex;
            }
        }


        public PTXboYearlyHearingDetailsList getYearlyHearingDetails(int AccountId, int TaxYear)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXboYearlyHearingDetailsList> lstPTXdoYearlyHearingDetails = null;
            PTXboYearlyHearingDetailsList pTXdoYearlyHearingDetails = new PTXboYearlyHearingDetailsList();
            try
            {
                string objInput = "AccountId : " + AccountId.ToString() + "-" + " TaxYear : " + TaxYear.ToString();
                Logger.For(this).Transaction("getYearlyHearingDetails-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@AccountId", AccountId);
                _hashtable.Add("@TaxYear", TaxYear);

                lstPTXdoYearlyHearingDetails = _Connection.Select<PTXboYearlyHearingDetailsList>(StoredProcedureNames.usp_getYearlyHearingDetailsByAccountId, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (lstPTXdoYearlyHearingDetails.Count > 0)
                {
                    pTXdoYearlyHearingDetails.YearlyHearingDetailsId = lstPTXdoYearlyHearingDetails[0].YearlyHearingDetailsId;
                    pTXdoYearlyHearingDetails.HearingDetailsId = lstPTXdoYearlyHearingDetails[0].HearingDetailsId;
                    pTXdoYearlyHearingDetails.TaxYear = lstPTXdoYearlyHearingDetails[0].TaxYear;
                    pTXdoYearlyHearingDetails.AccountId = lstPTXdoYearlyHearingDetails[0].AccountId;
                    pTXdoYearlyHearingDetails.ValueNoticeId = lstPTXdoYearlyHearingDetails[0].ValueNoticeId;
                    pTXdoYearlyHearingDetails.ProtestingDetailsId = lstPTXdoYearlyHearingDetails[0].ProtestingDetailsId;
                    pTXdoYearlyHearingDetails.UpdatedBy = lstPTXdoYearlyHearingDetails[0].UpdatedBy;
                    pTXdoYearlyHearingDetails.UpdatedDateTime = lstPTXdoYearlyHearingDetails[0].UpdatedDateTime;                    
                    pTXdoYearlyHearingDetails.DNDCodeId = lstPTXdoYearlyHearingDetails[0].DNDCodeId;
                    pTXdoYearlyHearingDetails.BPPAccountstatus = lstPTXdoYearlyHearingDetails[0].BPPAccountstatus;
                    pTXdoYearlyHearingDetails.propertyDetailsId = lstPTXdoYearlyHearingDetails[0].propertyDetailsId;
                    pTXdoYearlyHearingDetails.ArbitrationDetailsID = lstPTXdoYearlyHearingDetails[0].ArbitrationDetailsID;
                    pTXdoYearlyHearingDetails.BindingArbitrationID = lstPTXdoYearlyHearingDetails[0].BindingArbitrationID;
                    pTXdoYearlyHearingDetails.LitigationID = lstPTXdoYearlyHearingDetails[0].LitigationID;
                    pTXdoYearlyHearingDetails.ClientFinancialsId = lstPTXdoYearlyHearingDetails[0].ClientFinancialsId;
                    pTXdoYearlyHearingDetails.LitigationSecID = lstPTXdoYearlyHearingDetails[0].LitigationSecID;
                    //pTXdoYearlyHearingDetails.ProtestDeadline = lstPTXdoYearlyHearingDetails[0].ProtestDeadline;                    
                }
                Logger.For(this).Transaction("getYearlyHearingDetails-API  Ends successfully ");
                return pTXdoYearlyHearingDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getYearlyHearingDetails-API  Error " + ex);
                throw ex;
            }
        }

        public PTXboHearingDetailsList getHearingDetailsYearly(int Mode,int Id, int HearingTypeId)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXboYearlyHearingDetailsList> lstPTXboYearlyHearingDetailsList = null;
            List<PTXboHearingDetailsList> lstPTXboHearingDetailsList = null;
            PTXboHearingDetailsList pTXdoHearingDetails = new PTXboHearingDetailsList();
            try
            {
                string objInput = "Mode : " + Mode.ToString() + "-" + " Id : " + Id.ToString() + "-" + " HearingTypeId : " + HearingTypeId.ToString();
                Logger.For(this).Transaction("getHearingDetailsYearly-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@Mode", Mode);
                _hashtable.Add("@Id", Id);
                _hashtable.Add("@HearingTypeId", HearingTypeId);

                if (Mode == 1)
                {
                    lstPTXboYearlyHearingDetailsList = _Connection.Select<PTXboYearlyHearingDetailsList>(StoredProcedureNames.usp_getHearingDetailsYearly, _hashtable,
                        Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                    if (lstPTXboYearlyHearingDetailsList.Count > 0)
                    {
                        pTXdoHearingDetails.UpdateHearingDetailsId = lstPTXboYearlyHearingDetailsList[0].HearingDetailsId;
                        pTXdoHearingDetails.YearlyHearingDetailsId = lstPTXboYearlyHearingDetailsList[0].YearlyHearingDetailsId;                        
                        pTXdoHearingDetails.UpdatedBy = lstPTXboYearlyHearingDetailsList[0].UpdatedBy;
                        pTXdoHearingDetails.UpdatedDateTime = lstPTXboYearlyHearingDetailsList[0].UpdatedDateTime;
                    }
                }
                else if (Mode == 2)
                {
                    lstPTXboHearingDetailsList = _Connection.Select<PTXboHearingDetailsList>(StoredProcedureNames.usp_getHearingDetailsYearly, _hashtable,
                        Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    if (lstPTXboHearingDetailsList.Count > 0)
                    {
                        pTXdoHearingDetails.UpdateHearingDetailsId = lstPTXboHearingDetailsList[0].UpdateHearingDetailsId;
                        pTXdoHearingDetails.YearlyHearingDetailsId = lstPTXboHearingDetailsList[0].YearlyHearingDetailsId;
                        pTXdoHearingDetails.HearingTypeId = lstPTXboHearingDetailsList[0].HearingTypeId;
                        pTXdoHearingDetails.InformalInformalHearingDate = lstPTXboHearingDetailsList[0].InformalInformalHearingDate;
                        pTXdoHearingDetails.EvidenceDuoDate = lstPTXboHearingDetailsList[0].EvidenceDuoDate;
                        pTXdoHearingDetails.InformalInformalHearingDate = lstPTXboHearingDetailsList[0].InformalInformalHearingDate;
                        pTXdoHearingDetails.UpdateInformalHearingTime = lstPTXboHearingDetailsList[0].UpdateInformalHearingTime;
                        pTXdoHearingDetails.FormalHearingDate = lstPTXboHearingDetailsList[0].FormalHearingDate;
                        pTXdoHearingDetails.FormalHearingTime = lstPTXboHearingDetailsList[0].FormalHearingTime;
                        pTXdoHearingDetails.HearingCompletedDate = lstPTXboHearingDetailsList[0].HearingCompletedDate;
                        pTXdoHearingDetails.HearingStatusId = lstPTXboHearingDetailsList[0].HearingStatusId;
                        pTXdoHearingDetails.CMLetterPrinted = lstPTXboHearingDetailsList[0].CMLetterPrinted;
                        pTXdoHearingDetails.CMLetterDetailsId = lstPTXboHearingDetailsList[0].CMLetterDetailsId;
                        pTXdoHearingDetails.UpdatedBy = lstPTXboHearingDetailsList[0].UpdatedBy;
                        pTXdoHearingDetails.UpdatedDateTime = lstPTXboHearingDetailsList[0].UpdatedDateTime;
                        pTXdoHearingDetails.InformalBatchPrintDate = lstPTXboHearingDetailsList[0].InformalBatchPrintDate;
                        pTXdoHearingDetails.FormalBatchPrintDate = lstPTXboHearingDetailsList[0].FormalBatchPrintDate;
                        pTXdoHearingDetails.InformalPanelDocketID = lstPTXboHearingDetailsList[0].InformalPanelDocketID;
                        pTXdoHearingDetails.FormalPanelDocketID = lstPTXboHearingDetailsList[0].FormalPanelDocketID;
                        pTXdoHearingDetails.InformalAssignedAgentId = lstPTXboHearingDetailsList[0].InformalAssignedAgentId;
                        pTXdoHearingDetails.FormalAssignedAgentId = lstPTXboHearingDetailsList[0].FormalAssignedAgentId;
                        pTXdoHearingDetails.HearingCompletedTime = lstPTXboHearingDetailsList[0].HearingCompletedTime;
                        pTXdoHearingDetails.DocketChangeRequestSelected = lstPTXboHearingDetailsList[0].DocketChangeRequestSelected;
                        pTXdoHearingDetails.DocketChangeRequestedByUserId = lstPTXboHearingDetailsList[0].DocketChangeRequestedByUserId;
                        pTXdoHearingDetails.DocketChangeRequestedByUSerRoleId = lstPTXboHearingDetailsList[0].DocketChangeRequestedByUSerRoleId;
                        pTXdoHearingDetails.DocketChangeRequestedDateAndTime = lstPTXboHearingDetailsList[0].DocketChangeRequestedDateAndTime;
                        pTXdoHearingDetails.InformalBatchDate = lstPTXboHearingDetailsList[0].InformalBatchDate;
                        pTXdoHearingDetails.FormalBatchDate = lstPTXboHearingDetailsList[0].FormalBatchDate;
                        pTXdoHearingDetails.BatchPrintStatus = lstPTXboHearingDetailsList[0].BatchPrintStatus;
                        pTXdoHearingDetails.FilePreppedDate = lstPTXboHearingDetailsList[0].FilePreppedDate;
                        pTXdoHearingDetails.UTCDate = lstPTXboHearingDetailsList[0].UTCDate;
                        pTXdoHearingDetails.Rescheduling = lstPTXboHearingDetailsList[0].Rescheduling;
                        pTXdoHearingDetails.EvidenceCreated = lstPTXboHearingDetailsList[0].EvidenceCreated;
                        pTXdoHearingDetails.ByPassInformalHearing = lstPTXboHearingDetailsList[0].ByPassInformalHearing;
                        pTXdoHearingDetails.AttendHearing = lstPTXboHearingDetailsList[0].AttendHearing;
                        pTXdoHearingDetails.ContinuousRemarks = lstPTXboHearingDetailsList[0].ContinuousRemarks;
                        pTXdoHearingDetails.CurrentYearProtestReason = lstPTXboHearingDetailsList[0].CurrentYearProtestReason;
                        pTXdoHearingDetails.OfferValue = lstPTXboHearingDetailsList[0].OfferValue;
                        pTXdoHearingDetails.OfferDate = lstPTXboHearingDetailsList[0].OfferDate;
                        pTXdoHearingDetails.ClientHearingNotice = lstPTXboHearingDetailsList[0].ClientHearingNotice;
                        pTXdoHearingDetails.IsFormalRescheduleHearing = lstPTXboHearingDetailsList[0].IsFormalRescheduleHearing;
                        pTXdoHearingDetails.IsInformalRescheduleHearing = lstPTXboHearingDetailsList[0].IsInformalRescheduleHearing;
                        pTXdoHearingDetails.OwnerOpinionValue = lstPTXboHearingDetailsList[0].OwnerOpinionValue;
                        pTXdoHearingDetails.CorrectionMotionStatusid = lstPTXboHearingDetailsList[0].CorrectionMotionStatusid;
                    }           
                }
                Logger.For(this).Transaction("getHearingDetailsYearly-API  Ends successfully ");
                return pTXdoHearingDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getHearingDetailsYearly-API  Error " + ex);
                throw ex;
            }
        }
        public PTXboHearingResult getHearingResultByHearingDetailsId(int Mode, int Id)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXboHearingResult> lstPTXdoHearingResult = null;
            PTXboHearingResult pTXdoHearingResult = new PTXboHearingResult();
            try
            {
                string objInput = "Mode : " + Mode.ToString() + "-" + "Id : " + Id.ToString();

                Logger.For(this).Transaction("getHearingResultByHearingDetailsId-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@Mode", Mode);
                _hashtable.Add("@Id", Id);

                lstPTXdoHearingResult = _Connection.Select<PTXboHearingResult>(StoredProcedureNames.usp_getHearingResultByHearingDetailsId, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (lstPTXdoHearingResult.Count > 0)
                {
                    pTXdoHearingResult.HearingResultId = lstPTXdoHearingResult[0].HearingResultId;
                    pTXdoHearingResult.AppraiserName = lstPTXdoHearingResult[0].AppraiserName;
                    pTXdoHearingResult.ClientRequestedForArbitration = lstPTXdoHearingResult[0].ClientRequestedForArbitration;
                    pTXdoHearingResult.completionDateAndTime = lstPTXdoHearingResult[0].completionDateAndTime;
                    pTXdoHearingResult.confirmationLetterDateTime = lstPTXdoHearingResult[0].confirmationLetterDateTime;
                    pTXdoHearingResult.confirmationLetterReceivedDate = lstPTXdoHearingResult[0].confirmationLetterReceivedDate;
                    pTXdoHearingResult.DismissalAuthStatusId = lstPTXdoHearingResult[0].DismissalAuthStatusId;
                    pTXdoHearingResult.ExemptInvoice = lstPTXdoHearingResult[0].ExemptInvoice;
                    pTXdoHearingResult.FinalizedDate = lstPTXdoHearingResult[0].FinalizedDate;
                    pTXdoHearingResult.HearingAgentId = lstPTXdoHearingResult[0].HearingAgentId;
                    pTXdoHearingResult.HearingDetailsId = lstPTXdoHearingResult[0].HearingDetailsId;
                    pTXdoHearingResult.HearingFinalized = lstPTXdoHearingResult[0].HearingFinalized;
                    pTXdoHearingResult.HearingProcessingStatusId = lstPTXdoHearingResult[0].HearingProcessingStatusId;
                    pTXdoHearingResult.HearingResolutionId = lstPTXdoHearingResult[0].HearingResolutionId;
                    pTXdoHearingResult.HearingResultReasonCodeId = lstPTXdoHearingResult[0].HearingResultReasonCodeId;
                    pTXdoHearingResult.HearingResultReportId = lstPTXdoHearingResult[0].HearingResultReportId;
                    pTXdoHearingResult.HearingResultsSentOn = lstPTXdoHearingResult[0].HearingResultsSentOn;
                    pTXdoHearingResult.HearingStatusId = lstPTXdoHearingResult[0].HearingStatusId;
                    pTXdoHearingResult.HearingTrackingCodeId = lstPTXdoHearingResult[0].HearingTrackingCodeId;
                    pTXdoHearingResult.HearingTypeId = lstPTXdoHearingResult[0].HearingTypeId;
                    pTXdoHearingResult.HearingValue = lstPTXdoHearingResult[0].HearingValue;
                    pTXdoHearingResult.HRInvoiceStatusid = lstPTXdoHearingResult[0].HRInvoiceStatusid;
                    pTXdoHearingResult.InvoiceGenerated = lstPTXdoHearingResult[0].InvoiceGenerated;
                    pTXdoHearingResult.PostHearingImprovedValue = lstPTXdoHearingResult[0].PostHearingImprovedValue;
                    pTXdoHearingResult.PostHearingLandValue = lstPTXdoHearingResult[0].PostHearingLandValue;
                    pTXdoHearingResult.PostHearingMarketValue = lstPTXdoHearingResult[0].PostHearingMarketValue;
                    pTXdoHearingResult.PostHearingTotalValue = lstPTXdoHearingResult[0].PostHearingTotalValue;
                    pTXdoHearingResult.ResultGenerated = lstPTXdoHearingResult[0].ResultGenerated;
                    pTXdoHearingResult.ReviewBindingArbitration = lstPTXdoHearingResult[0].ReviewBindingArbitration;
                    pTXdoHearingResult.ReviewForArbitration = lstPTXdoHearingResult[0].ReviewForArbitration;
                    pTXdoHearingResult.UpdatedBy = lstPTXdoHearingResult[0].UpdatedBy;
                    pTXdoHearingResult.UpdatedDateTime = lstPTXdoHearingResult[0].UpdatedDateTime;                    
                }
                Logger.For(this).Transaction("getHearingResultByHearingDetailsId-API  Ends successfully ");
                return pTXdoHearingResult;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getHearingResultByHearingDetailsId-API  Error " + ex);
                throw ex;
            }
        }
        public PTXdoInvoiceAndHearingResultMap getInvoiceAndHearingResultMap(int HearingResultId)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXdoInvoiceAndHearingResultMap> lstPTXdoInvoiceAndHearingResultMap = null;
            PTXdoInvoiceAndHearingResultMap pTXdoInvoiceAndHearingResultMap = new PTXdoInvoiceAndHearingResultMap();
            try
            {
                string objInput = "HearingResultId : " + HearingResultId.ToString();
                Logger.For(this).Transaction("getInvoiceAndHearingResultMap-API  Reached " + ((object)objInput).ToJson(false));

                _hashtable.Add("@HearingResultId", HearingResultId);

                lstPTXdoInvoiceAndHearingResultMap = _Connection.Select<PTXdoInvoiceAndHearingResultMap>(StoredProcedureNames.usp_getInvoiceAndHearingResultMap, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (lstPTXdoInvoiceAndHearingResultMap.Count > 0)
                {
                    pTXdoInvoiceAndHearingResultMap.InvoiceAndHearingResultMapID = lstPTXdoInvoiceAndHearingResultMap[0].InvoiceAndHearingResultMapID;
                }
                Logger.For(this).Transaction("getInvoiceAndHearingResultMap-API  Ends successfully ");
                return pTXdoInvoiceAndHearingResultMap;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getInvoiceAndHearingResultMap-API  Error " + ex);
                throw ex;
            }
        }

        public PTXboValueNotice getValueHearingNotice(int YearlyHearingDetailsId)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXboValueNotice> lstPTXdoValueNotice = null;
            PTXboValueNotice pTXdoValueNotice = new PTXboValueNotice();
            try
            {
                string objInput = "YearlyHearingDetailsId : " + YearlyHearingDetailsId.ToString();
                Logger.For(this).Transaction("getValueHearingNotice-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@YearlyHearingDetailsId", YearlyHearingDetailsId);

                lstPTXdoValueNotice = _Connection.Select<PTXboValueNotice>(StoredProcedureNames.usp_getValueNotice, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (lstPTXdoValueNotice.Count > 0)
                {
                    pTXdoValueNotice.ValueNoticeId = lstPTXdoValueNotice[0].ValueNoticeId;
                    pTXdoValueNotice.NoticedDate = lstPTXdoValueNotice[0].NoticedDate;
                    pTXdoValueNotice.IFileNumber = lstPTXdoValueNotice[0].IFileNumber;
                    pTXdoValueNotice.NoticeLandValue = lstPTXdoValueNotice[0].NoticeLandValue;
                    pTXdoValueNotice.NoticeImprovedValue = lstPTXdoValueNotice[0].NoticeImprovedValue;
                    pTXdoValueNotice.NoticeMarketValue = lstPTXdoValueNotice[0].NoticeMarketValue;
                    pTXdoValueNotice.NoticeTotalValue = lstPTXdoValueNotice[0].NoticeTotalValue;
                    pTXdoValueNotice.Statusid = lstPTXdoValueNotice[0].Statusid;
                    pTXdoValueNotice.UpdatedBy = lstPTXdoValueNotice[0].UpdatedBy;
                    pTXdoValueNotice.UpdatedDateTime = lstPTXdoValueNotice[0].UpdatedDateTime;
                    pTXdoValueNotice.CertifiedValueSelected = lstPTXdoValueNotice[0].CertifiedValueSelected;
                    pTXdoValueNotice.CertifiedLandValue = lstPTXdoValueNotice[0].CertifiedLandValue;
                    pTXdoValueNotice.CertifiedImprovedValue = lstPTXdoValueNotice[0].CertifiedImprovedValue;
                    pTXdoValueNotice.CertifiedMarketValue = lstPTXdoValueNotice[0].CertifiedMarketValue;
                    pTXdoValueNotice.CertifiedTotalValue = lstPTXdoValueNotice[0].CertifiedTotalValue;
                    pTXdoValueNotice.PendingNoticedSelected = lstPTXdoValueNotice[0].PendingNoticedSelected;
                    pTXdoValueNotice.PropertytaxAccountID = lstPTXdoValueNotice[0].PropertytaxAccountID;
                    pTXdoValueNotice.Propertytax_TaxYear = lstPTXdoValueNotice[0].Propertytax_TaxYear;
                    pTXdoValueNotice.TargetValue = lstPTXdoValueNotice[0].TargetValue;
                    pTXdoValueNotice.TargetUEValue = lstPTXdoValueNotice[0].TargetUEValue;
                    pTXdoValueNotice.ReNoticedLandValue = lstPTXdoValueNotice[0].ReNoticedLandValue;
                    pTXdoValueNotice.ReNoticedMarketValue = lstPTXdoValueNotice[0].ReNoticedMarketValue;
                    pTXdoValueNotice.ReNoticedImprovedValue = lstPTXdoValueNotice[0].ReNoticedImprovedValue;
                    pTXdoValueNotice.ReNoticeTotalValue = lstPTXdoValueNotice[0].ReNoticeTotalValue;
                    pTXdoValueNotice.ReNoticedDate = lstPTXdoValueNotice[0].ReNoticedDate;                  
                }
                Logger.For(this).Transaction("getValueHearingNotice-API  Ends successfully ");
                return pTXdoValueNotice;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getValueHearingNotice-API  Error " + ex);
                throw ex;
            }
        }
        public PTXdoInvoice getInvoiceWithInvoiceAndHearingResultMapInvoice(int Mode, int HearingResultsId)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXdoInvoice> lstPTXdoInvoice = null;
            PTXdoInvoice pTXdoInvoice = new PTXdoInvoice();

            try
            {
                string objInput = "Mode : " + Mode.ToString() + "-" + "HearingResultsId : " + HearingResultsId.ToString();
                Logger.For(this).Transaction("getInvoiceWithInvoiceAndHearingResultMapInvoice-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@Mode", Mode);
                _hashtable.Add("@Id", HearingResultsId);

                lstPTXdoInvoice = _Connection.Select<PTXdoInvoice>(StoredProcedureNames.usp_getInvoiceWithInvoiceAndHearingResultMap, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (lstPTXdoInvoice.Count > 0)
                {
                    pTXdoInvoice.InvoiceID = lstPTXdoInvoice[0].InvoiceID;
                    pTXdoInvoice.Group.groupID = lstPTXdoInvoice[0].Group.groupID;
                    pTXdoInvoice.Project.ProjectId = lstPTXdoInvoice[0].Project.ProjectId;
                    pTXdoInvoice.YearlyHearingDetails.YearlyHearingDetailsId = lstPTXdoInvoice[0].YearlyHearingDetails.YearlyHearingDetailsId;
                    pTXdoInvoice.Client.clientId = lstPTXdoInvoice[0].Client.clientId;
                    pTXdoInvoice.InvoiceType.Termstypeid = lstPTXdoInvoice[0].InvoiceType.Termstypeid;
                    pTXdoInvoice.InvoiceDate = lstPTXdoInvoice[0].InvoiceDate;
                    pTXdoInvoice.PaymentDueDate = lstPTXdoInvoice[0].PaymentDueDate;
                    pTXdoInvoice.InvoiceGenerationConfigIDUsedForCalculation = lstPTXdoInvoice[0].InvoiceGenerationConfigIDUsedForCalculation;
                    pTXdoInvoice.InitialAssessedValue = lstPTXdoInvoice[0].InitialAssessedValue;
                    pTXdoInvoice.FinalAssessedValue = lstPTXdoInvoice[0].FinalAssessedValue;
                    pTXdoInvoice.PriorYearTaxRate = lstPTXdoInvoice[0].PriorYearTaxRate;
                    pTXdoInvoice.ContingencyPercentage = lstPTXdoInvoice[0].ContingencyPercentage;
                    pTXdoInvoice.ContingencyFee = lstPTXdoInvoice[0].ContingencyFee;
                    pTXdoInvoice.FlatFee = lstPTXdoInvoice[0].FlatFee;
                    pTXdoInvoice.InvoiceAmount = lstPTXdoInvoice[0].InvoiceAmount;
                    pTXdoInvoice.CreatedDateAndTime = lstPTXdoInvoice[0].CreatedDateAndTime;
                    pTXdoInvoice.SentDateAndTime = lstPTXdoInvoice[0].SentDateAndTime;
                    pTXdoInvoice.DeliveryMethod.id = lstPTXdoInvoice[0].DeliveryMethod.id; //DeliveryMethodId
                    pTXdoInvoice.DeliveryStatus.DeliverystatusId = lstPTXdoInvoice[0].DeliveryStatus.DeliverystatusId; //DeliveryStatusId
                    pTXdoInvoice.AutoGenerated = lstPTXdoInvoice[0].AutoGenerated;
                    pTXdoInvoice.ManuallyGeneratedUser.Userid = lstPTXdoInvoice[0].ManuallyGeneratedUser.Userid; //ManuallyGeneratedUserId
                    pTXdoInvoice.ManuallyGeneratedUserRole.UserRoleid = lstPTXdoInvoice[0].ManuallyGeneratedUserRole.UserRoleid; //ManuallyGeneratedUserRoleId
                    pTXdoInvoice.OnHold = lstPTXdoInvoice[0].OnHold;
                    pTXdoInvoice.OnHoldDate = lstPTXdoInvoice[0].OnHoldDate;
                    pTXdoInvoice.TotalAmountPaid = lstPTXdoInvoice[0].TotalAmountPaid;
                    pTXdoInvoice.InvoicingStatusId = lstPTXdoInvoice[0].InvoicingStatusId;
                    pTXdoInvoice.InvoicingProcessingStatus.InvoicingProcessingStatusID = lstPTXdoInvoice[0].InvoicingProcessingStatus.InvoicingProcessingStatusID;
                    pTXdoInvoice.InvoiceDescription = lstPTXdoInvoice[0].InvoiceDescription;
                    pTXdoInvoice.CompoundInterest = lstPTXdoInvoice[0].CompoundInterest;
                    pTXdoInvoice.InvoiceCreditAmount = lstPTXdoInvoice[0].InvoiceCreditAmount;
                    pTXdoInvoice.PaymentStatus = lstPTXdoInvoice[0].PaymentStatus;
                    pTXdoInvoice.InvoiceGroupType.InvoiceGroupingTypeId = lstPTXdoInvoice[0].InvoiceGroupType.InvoiceGroupingTypeId;
                    pTXdoInvoice.TaxYear = lstPTXdoInvoice[0].TaxYear;
                    pTXdoInvoice.Reduction = lstPTXdoInvoice[0].Reduction;
                    pTXdoInvoice.TotalEstimatedTaxSavings = lstPTXdoInvoice[0].TotalEstimatedTaxSavings;
                    pTXdoInvoice.AmountPaid = lstPTXdoInvoice[0].AmountPaid;
                    pTXdoInvoice.AmountAdjusted = lstPTXdoInvoice[0].AmountAdjusted;
                    pTXdoInvoice.ApplicableInterest = lstPTXdoInvoice[0].ApplicableInterest;
                    pTXdoInvoice.InterestPaid = lstPTXdoInvoice[0].InterestPaid;
                    pTXdoInvoice.InterestAdjusted = lstPTXdoInvoice[0].InterestAdjusted;
                    pTXdoInvoice.AmountDue = lstPTXdoInvoice[0].AmountDue;
                    pTXdoInvoice.UpdatedBy = lstPTXdoInvoice[0].UpdatedBy;
                    pTXdoInvoice.UpdatedDateTime = lstPTXdoInvoice[0].UpdatedDateTime;
                    pTXdoInvoice.InterestRate.InterestRateID = lstPTXdoInvoice[0].InterestRate.InterestRateID;
                    pTXdoInvoice.IntitalLand = lstPTXdoInvoice[0].IntitalLand;
                    pTXdoInvoice.IntialImproved = lstPTXdoInvoice[0].IntialImproved;
                    pTXdoInvoice.InitialMarket = lstPTXdoInvoice[0].InitialMarket;
                    pTXdoInvoice.FinalLand = lstPTXdoInvoice[0].FinalLand;
                    pTXdoInvoice.FinalImproved = lstPTXdoInvoice[0].FinalImproved;
                    pTXdoInvoice.FinalMarket = lstPTXdoInvoice[0].FinalMarket;
                    pTXdoInvoice.CreditAmountApplied = lstPTXdoInvoice[0].CreditAmountApplied;
                    pTXdoInvoice.AttorneyFee = lstPTXdoInvoice[0].AttorneyFee;
                    pTXdoInvoice.ServiceFee = lstPTXdoInvoice[0].ServiceFee;
                    pTXdoInvoice.CourtCost = lstPTXdoInvoice[0].CourtCost;
                    pTXdoInvoice.isInvoiceDefect = lstPTXdoInvoice[0].isInvoiceDefect;
                }
                Logger.For(this).Transaction("getInvoiceWithInvoiceAndHearingResultMapInvoice-API  Ends successfully ");
                return pTXdoInvoice;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getInvoiceWithInvoiceAndHearingResultMapInvoice-API  Error " + ex);
                throw ex;
            }
        }

        public PTXdoInvoiceAndHearingResultMap getInvoiceWithInvoiceAndHearingResultMapPHRMAP(int Mode, int HearingResultsId)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXdoInvoiceAndHearingResultMap> lstPTXdoInvoiceAndHearingResultMap = null;
            PTXdoInvoiceAndHearingResultMap pTXdoInvoiceAndHearingResultMap = new PTXdoInvoiceAndHearingResultMap();
            try
            {
                string objInput = "Mode : " + Mode.ToString() + "-" + "HearingResultsId : " + HearingResultsId.ToString();
                Logger.For(this).Transaction("getInvoiceWithInvoiceAndHearingResultMapPHRMAP-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@Mode", Mode);
                _hashtable.Add("@Id", HearingResultsId);

                lstPTXdoInvoiceAndHearingResultMap = _Connection.Select<PTXdoInvoiceAndHearingResultMap>(StoredProcedureNames.usp_getInvoiceWithInvoiceAndHearingResultMap, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (lstPTXdoInvoiceAndHearingResultMap.Count > 0)
                {
                    pTXdoInvoiceAndHearingResultMap.InvoiceAndHearingResultMapID = lstPTXdoInvoiceAndHearingResultMap[0].InvoiceAndHearingResultMapID;
                    pTXdoInvoiceAndHearingResultMap.Invoice.InvoiceID = lstPTXdoInvoiceAndHearingResultMap[0].Invoice.InvoiceID;
                    pTXdoInvoiceAndHearingResultMap.HearingResult.HearingResultsId = lstPTXdoInvoiceAndHearingResultMap[0].HearingResult.HearingResultsId;
                    pTXdoInvoiceAndHearingResultMap.ArbitrationDetails.ArbitrationDetailsId = lstPTXdoInvoiceAndHearingResultMap[0].ArbitrationDetails.ArbitrationDetailsId;
                    pTXdoInvoiceAndHearingResultMap.LitigationDetails.LitigationID = lstPTXdoInvoiceAndHearingResultMap[0].LitigationDetails.LitigationID;
                    pTXdoInvoiceAndHearingResultMap.TaxBillAuidtId = lstPTXdoInvoiceAndHearingResultMap[0].TaxBillAuidtId;
                    pTXdoInvoiceAndHearingResultMap.BPPRenditionsID = lstPTXdoInvoiceAndHearingResultMap[0].BPPRenditionsID;
                }
                Logger.For(this).Transaction("getInvoiceWithInvoiceAndHearingResultMapPHRMAP-API  Ends successfully ");
                return pTXdoInvoiceAndHearingResultMap;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getInvoiceWithInvoiceAndHearingResultMapPHRMAP-API  Error " + ex);
                throw ex;
            }
        }

        public List<PTXdoInvoiceAndHearingResultMap> getInvoiceAndHearingResultMapList(int Mode, int InvoiceID)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXdoInvoiceAndHearingResultMap> lstPTXdoInvoiceAndHearingResultMap = null;
            try
            {
                string objInput = "Mode : " + Mode.ToString() + "-" + " InvoiceID : " + InvoiceID.ToString();
                Logger.For(this).Transaction("getInvoiceAndHearingResultMapList-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@Mode", Mode);
                _hashtable.Add("@Id", InvoiceID);

                lstPTXdoInvoiceAndHearingResultMap = _Connection.Select<PTXdoInvoiceAndHearingResultMap>(StoredProcedureNames.usp_getInvoiceWithInvoiceAndHearingResultMap, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Transaction("getInvoiceAndHearingResultMapList-API  Ends successfully ");
                return lstPTXdoInvoiceAndHearingResultMap;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getInvoiceAndHearingResultMapList-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateUserEnteredHearingResults(PTXboUserEnteredHearingResultsInsert userEnteredHearingResults)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;

            try
            {
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingResults-API  Reached " + ((object)userEnteredHearingResults).ToJson(false));
                _hashtable.Add("@Taxyear", userEnteredHearingResults.Taxyear);
                _hashtable.Add("@NoticedLandValue", (userEnteredHearingResults.NoticedLandValue.HasValue) ? userEnteredHearingResults.NoticedLandValue : null);
                _hashtable.Add("@NoticedImprovedValue", (userEnteredHearingResults.NoticedImprovedValue.HasValue) ? userEnteredHearingResults.NoticedImprovedValue : null);
                _hashtable.Add("@NoticedMarketValue", (userEnteredHearingResults.NoticedMarketValue.HasValue) ? userEnteredHearingResults.NoticedMarketValue : null);
                _hashtable.Add("@NoticedTotalvalue", (userEnteredHearingResults.NoticedTotalvalue.HasValue) ? userEnteredHearingResults.NoticedTotalvalue : null);
                int NoChange = 0;
                if (userEnteredHearingResults.NoChange == true)
                {
                    NoChange = 1;
                }
                _hashtable.Add("@NoChange", NoChange);
                _hashtable.Add("@PostHearingLandValue", (userEnteredHearingResults.PostHearingLandValue.HasValue) ? userEnteredHearingResults.PostHearingLandValue : null);
                _hashtable.Add("@PostHearingImprovedValue", (userEnteredHearingResults.PostHearingImprovedValue.HasValue) ? userEnteredHearingResults.PostHearingImprovedValue : null);
                _hashtable.Add("@PostHearingMarketValue", (userEnteredHearingResults.PostHearingMarketValue.HasValue) ? userEnteredHearingResults.PostHearingMarketValue : null);
                _hashtable.Add("@PostHearingTotalValue", (userEnteredHearingResults.PostHearingTotalValue.HasValue) ? userEnteredHearingResults.PostHearingTotalValue : null);
                _hashtable.Add("@HearingAgentId", (userEnteredHearingResults.HearingAgentId.HasValue) ? userEnteredHearingResults.HearingAgentId : null);
                _hashtable.Add("@ConfirmationLetterDate", (userEnteredHearingResults.ConfirmationLetterDate.HasValue) ? userEnteredHearingResults.ConfirmationLetterDate : null);
                _hashtable.Add("@CompletionDate", (userEnteredHearingResults.CompletionDate.HasValue) ? userEnteredHearingResults.CompletionDate : null);
                _hashtable.Add("@CompletionTime", (userEnteredHearingResults.CompletionTime.HasValue) ? userEnteredHearingResults.CompletionTime : null);
                _hashtable.Add("@HearingResolutionId", userEnteredHearingResults.HearingResolutionId);
                _hashtable.Add("@ReasonCodeId", userEnteredHearingResults.ReasonCodeId);
                _hashtable.Add("@DismissalAuthStatusId", userEnteredHearingResults.DismissalAuthStatusId);
                _hashtable.Add("@HearingResultReportId", userEnteredHearingResults.HearingResultReportId);
                int HearingFinalized = 0;
                if (userEnteredHearingResults.HearingFinalized == true)
                {
                    HearingFinalized = 1;
                }
                _hashtable.Add("@HearingFinalized", HearingFinalized);
                _hashtable.Add("@HearingResultSentOn", (userEnteredHearingResults.HearingResultSentOn.HasValue) ? userEnteredHearingResults.HearingResultSentOn : null);
                int ResultGenerated = 0;
                if (userEnteredHearingResults.ResultGenerated == true)
                {
                    ResultGenerated = 1;
                }
                _hashtable.Add("@ResultGenerated", ResultGenerated);
                int InvoiceGenerated = 0;
                if (userEnteredHearingResults.InvoiceGenerated == true)
                {
                    InvoiceGenerated = 1;
                }
                _hashtable.Add("@InvoiceGenerated", InvoiceGenerated);
                _hashtable.Add("@InvoiceId", userEnteredHearingResults.InvoiceId);
                int IsValueNoticeAudited = 0;
                if (userEnteredHearingResults.IsValueNoticeAudited == true)
                {
                    IsValueNoticeAudited = 1;
                }
                _hashtable.Add("@IsValueNoticeAudited", IsValueNoticeAudited);
                int IsValueNoticeUpdateRequired = 0;
                if (userEnteredHearingResults.IsValueNoticeUpdateRequired == true)
                {
                    IsValueNoticeUpdateRequired = 1;
                }
                _hashtable.Add("@IsValueNoticeUpdateRequired", IsValueNoticeUpdateRequired);
                _hashtable.Add("@HearingStatusId", userEnteredHearingResults.HearingStatusId);
                _hashtable.Add("@UpdatedBy", userEnteredHearingResults.UpdatedBy);
                //_hashtable.Add("@UpdatedDateTime", (userEnteredHearingResults.UpdatedDateTime.HasValue) ? userEnteredHearingResults.UpdatedDateTime : null);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                _hashtable.Add("@DefectReasonId", userEnteredHearingResults.DefectReasonId);
                int ExemptInvoice = 0;
                if (userEnteredHearingResults.ExemptInvoice == true)
                {
                    ExemptInvoice = 1;
                }
                _hashtable.Add("@ExemptInvoice", ExemptInvoice);
                _hashtable.Add("@confirmationLetterReceivedDate", (userEnteredHearingResults.confirmationLetterReceivedDate.HasValue) ? userEnteredHearingResults.confirmationLetterReceivedDate : null);
                _hashtable.Add("@HRInvoiceStatusid", (userEnteredHearingResults.HRInvoiceStatusid.HasValue) ? userEnteredHearingResults.HRInvoiceStatusid : null);
                int ReviewBindingArbitration = 0;
                if (userEnteredHearingResults.ReviewBindingArbitration == true)
                {
                    ReviewBindingArbitration = 1;
                }
                _hashtable.Add("@ReviewBindingArbitration", ReviewBindingArbitration);


                SPName = StoredProcedureNames.usp_InsertUserEnteredHearingResults;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);               

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingResults-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingResults-API  Error " + ex);
                throw ex;
                return false;
            }
        }

        public bool saveOrUpdateIndexedNoticeDocument(PTXboIndexedNoticeDocumentUpdate indexedNoticeDocumentUpdate)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateIndexedNoticeDocument-API  Reached " + ((object)indexedNoticeDocumentUpdate).ToJson(false));
                _hashtable.Add("@IndexedNoticeDocumentId", indexedNoticeDocumentUpdate.IndexedNoticeDocumentId);                
                _hashtable.Add("@UserEnteredHearingTypeId", indexedNoticeDocumentUpdate.UserEnteredHearingTypeId);
                _hashtable.Add("@UserEnteredVNoticeDataId", indexedNoticeDocumentUpdate.UserEnteredVNoticeDataId);
                _hashtable.Add("@UserEnteredHearingNoticeDataId", indexedNoticeDocumentUpdate.UserEnteredHearingNoticeDataId);
                _hashtable.Add("@UserEnteredHearingResultsId", indexedNoticeDocumentUpdate.UserEnteredHearingResultsId);
                _hashtable.Add("@BatchCode", indexedNoticeDocumentUpdate.BatchCode);
                _hashtable.Add("@PWImageID", indexedNoticeDocumentUpdate.PWImageID);
                _hashtable.Add("@NoticeDocumentTypeId", indexedNoticeDocumentUpdate.NoticeDocumentTypeId);
                _hashtable.Add("@TaxYear", indexedNoticeDocumentUpdate.TaxYear);
                _hashtable.Add("@CountyCode", indexedNoticeDocumentUpdate.CountyCode);
                _hashtable.Add("@AccountNumber", indexedNoticeDocumentUpdate.AccountNumber);
                _hashtable.Add("@ClientId", indexedNoticeDocumentUpdate.ClientId);
                _hashtable.Add("@AccountId", indexedNoticeDocumentUpdate.AccountId);
                _hashtable.Add("@AccountStatusId", indexedNoticeDocumentUpdate.AccountStatusId);
                _hashtable.Add("@AccountProcessStatusId", indexedNoticeDocumentUpdate.AccountProcessStatusId);
                _hashtable.Add("@ProtestCodeId", indexedNoticeDocumentUpdate.ProtestCodeId);

                int DefectNotice = 0;
                if (indexedNoticeDocumentUpdate.DefectNotice == true)
                {
                    DefectNotice = 1;
                }
                _hashtable.Add("@DefectNotice", DefectNotice);

                _hashtable.Add("@HearingResolutionId", indexedNoticeDocumentUpdate.HearingResolutionId);
                int DefectResolved = 0;
                if (indexedNoticeDocumentUpdate.DefectResolved == true)
                {
                    DefectResolved = 1;
                }
                _hashtable.Add("@DefectResolved", DefectResolved);

                int ExemptInvoice = 0;
                if (indexedNoticeDocumentUpdate.ExemptInvoice == true)
                {
                    ExemptInvoice = 1;
                }
                _hashtable.Add("@ExemptInvoice", ExemptInvoice);

                _hashtable.Add("@AssignedQueueId", indexedNoticeDocumentUpdate.AssignedQueueId);
                _hashtable.Add("@EntryUserId", indexedNoticeDocumentUpdate.EntryUserId);
                _hashtable.Add("@EntryUserRoleId", indexedNoticeDocumentUpdate.EntryUserRoleId);
                string dateAndTime = "01-01-0001 00:00:00";
                if (indexedNoticeDocumentUpdate.EntryDateAndTime.ToString() != dateAndTime)
                {
                    _hashtable.Add("@EntryDateAndTime", (indexedNoticeDocumentUpdate.EntryDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.EntryDateAndTime : null);
                }
                else
                {
                    _hashtable.Add("@EntryDateAndTime", null);
                }

                _hashtable.Add("@DefectAssignedUserId", indexedNoticeDocumentUpdate.DefectAssignedUserId);
                _hashtable.Add("@DefectAssignedUserRoleId", indexedNoticeDocumentUpdate.DefectAssignedUserRoleId);
                if (indexedNoticeDocumentUpdate.DefectAssignedDateAndTime.ToString() != dateAndTime)
                {
                    _hashtable.Add("@DefectAssignedDateAndTime", (indexedNoticeDocumentUpdate.DefectAssignedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.DefectAssignedDateAndTime : null);
                }
                else
                {
                    _hashtable.Add("@DefectAssignedDateAndTime", null);
                }
                if (indexedNoticeDocumentUpdate.DefectRectifiedDateAndTime.ToString() != dateAndTime)
                {
                    _hashtable.Add("@DefectRectifiedDateAndTime", (indexedNoticeDocumentUpdate.DefectRectifiedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.DefectRectifiedDateAndTime : null);
                }
                else
                {
                    _hashtable.Add("@DefectRectifiedDateAndTime", null);
                }
                
                _hashtable.Add("@AuditedUserId", indexedNoticeDocumentUpdate.AuditedUserId);
                _hashtable.Add("@AuditedUserRoleId", indexedNoticeDocumentUpdate.AuditedUserRoleId);

                if (indexedNoticeDocumentUpdate.AuditedDateAndTime.ToString() != dateAndTime)
                {
                    _hashtable.Add("@AuditedDateAndTime", (indexedNoticeDocumentUpdate.AuditedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.AuditedDateAndTime : null);
                }
                else
                {
                    _hashtable.Add("@AuditedDateAndTime", null);
                }
                

                _hashtable.Add("@StatusId", indexedNoticeDocumentUpdate.StatusId);
                _hashtable.Add("@ProcessingStatusId", indexedNoticeDocumentUpdate.ProcessingStatusId);

                if (indexedNoticeDocumentUpdate.OnHoldDateAndTime.ToString() != dateAndTime)
                {
                    _hashtable.Add("@OnHoldDateAndTime", (indexedNoticeDocumentUpdate.OnHoldDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.OnHoldDateAndTime : null);
                }
                else
                {
                    _hashtable.Add("@OnHoldDateAndTime", null);
                }

                if (indexedNoticeDocumentUpdate.ScannedDateAndTime.ToString() != dateAndTime)
                {
                    _hashtable.Add("@ScannedDateAndTime", (indexedNoticeDocumentUpdate.ScannedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.ScannedDateAndTime : null);
                }
                else
                {
                    _hashtable.Add("@ScannedDateAndTime", null);
                }
                
                _hashtable.Add("@Offset", indexedNoticeDocumentUpdate.Offset);
                _hashtable.Add("@TCK", indexedNoticeDocumentUpdate.TCK);
                _hashtable.Add("@DocumentName", indexedNoticeDocumentUpdate.DocumentName);
                _hashtable.Add("@UpdatedBy", indexedNoticeDocumentUpdate.UpdatedBy);
                //_hashtable.Add("@UpdatedDateTime", (indexedNoticeDocumentUpdate.UpdatedDateTime.HasValue) ? indexedNoticeDocumentUpdate.UpdatedDateTime : null);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                _hashtable.Add("@QCAssignedUserId", indexedNoticeDocumentUpdate.QCAssignedUserId);
                _hashtable.Add("@QCAssignedUserRoleId", indexedNoticeDocumentUpdate.QCAssignedUserRoleId);

                if (indexedNoticeDocumentUpdate.QCAssignedDateAndTime.ToString() != dateAndTime)
                {
                    _hashtable.Add("@QCAssignedDateAndTime", (indexedNoticeDocumentUpdate.QCAssignedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.QCAssignedDateAndTime : null);
                }
                else
                {
                    _hashtable.Add("@QCAssignedDateAndTime", null);
                }
                

                _hashtable.Add("@DocumentDefectCodeId", indexedNoticeDocumentUpdate.DocumentDefectCodeId);
                _hashtable.Add("@NoticeDefectLetterTypeId", indexedNoticeDocumentUpdate.NoticeDefectLetterTypeId);
                int Donotassignauto = 0;
                if (indexedNoticeDocumentUpdate.Donotassignauto == true)
                {
                    Donotassignauto = 1;
                }
                _hashtable.Add("@Donotassignauto", Donotassignauto);

                _hashtable.Add("@EntryAssignedBy", indexedNoticeDocumentUpdate.EntryAssignedBy);
                _hashtable.Add("@DefectAssignedBy", indexedNoticeDocumentUpdate.DefectAssignedBy);
                _hashtable.Add("@AuditAssignedBy", indexedNoticeDocumentUpdate.AuditAssignedBy);
                _hashtable.Add("@QCAssignedBy", indexedNoticeDocumentUpdate.QCAssignedBy);
                _hashtable.Add("@PreviousNoticeQueueId", indexedNoticeDocumentUpdate.PreviousNoticeQueueId);

                SPName = StoredProcedureNames.usp_updateIndexedNoticeDocument;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);               

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateIndexedNoticeDocument-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateIndexedNoticeDocument-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateHearingDetails(PTXboHearingDetailsList hearingDetailsUpdate)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateHearingDetails-API  Reached " + ((object)hearingDetailsUpdate).ToJson(false));
                _hashtable.Add("@HearingDetailsId", hearingDetailsUpdate.UpdateHearingDetailsId);
                _hashtable.Add("@YearlyHearingDetailsId", hearingDetailsUpdate.YearlyHearingDetailsId);
                _hashtable.Add("@HearingTypeId", hearingDetailsUpdate.HearingTypeId);
                _hashtable.Add("@InformalInformalHearingDate", hearingDetailsUpdate.InformalInformalHearingDate);
                _hashtable.Add("@EvidenceDuoDate", hearingDetailsUpdate.EvidenceDuoDate);
                _hashtable.Add("@InformalHearingDate", hearingDetailsUpdate.UpdateInformalHearingDate);
                _hashtable.Add("@InformalHearingTime", hearingDetailsUpdate.UpdateInformalHearingTime);
                _hashtable.Add("@FormalHearingDate", hearingDetailsUpdate.UpdateFormalHearingDate);
                _hashtable.Add("@FormalHearingTime", hearingDetailsUpdate.UpdateFormalHearingTime);
                _hashtable.Add("@HearingCompletedDate", hearingDetailsUpdate.HearingCompletedDate);
                _hashtable.Add("@HearingStatusId", hearingDetailsUpdate.HearingStatusId);
                _hashtable.Add("@CMLetterPrinted", hearingDetailsUpdate.CMLetterPrinted);
                _hashtable.Add("@CMLetterDetailsId", hearingDetailsUpdate.CMLetterDetailsId);
                _hashtable.Add("@UpdatedBy", hearingDetailsUpdate.UpdatedBy);
                //_hashtable.Add("@UpdatedDateTime", hearingDetailsUpdate.UpdatedDateTime);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                _hashtable.Add("@InformalBatchPrintDate", hearingDetailsUpdate.InformalBatchPrintDate);
                _hashtable.Add("@FormalBatchPrintDate", hearingDetailsUpdate.FormalBatchPrintDate);
                _hashtable.Add("@InformalPanelDocketID", hearingDetailsUpdate.UpdateInformalPanelDocketID);
                _hashtable.Add("@FormalPanelDocketID", hearingDetailsUpdate.UpdateFormalPanelDocketID);
                _hashtable.Add("@InformalAssignedAgentId", hearingDetailsUpdate.InformalAssignedAgentId);
                _hashtable.Add("@FormalAssignedAgentId", hearingDetailsUpdate.FormalAssignedAgentId);
                _hashtable.Add("@HearingCompletedTime", hearingDetailsUpdate.HearingCompletedTime);
                _hashtable.Add("@DocketChangeRequestSelected", hearingDetailsUpdate.DocketChangeRequestSelected);
                _hashtable.Add("@DocketChangeRequestedByUserId", hearingDetailsUpdate.DocketChangeRequestedByUserId);
                _hashtable.Add("@DocketChangeRequestedByUSerRoleId", hearingDetailsUpdate.DocketChangeRequestedByUSerRoleId);
                _hashtable.Add("@DocketChangeRequestedDateAndTime", hearingDetailsUpdate.DocketChangeRequestedDateAndTime);
                _hashtable.Add("@InformalBatchDate", hearingDetailsUpdate.InformalBatchDate);
                _hashtable.Add("@FormalBatchDate", hearingDetailsUpdate.FormalBatchDate);
                _hashtable.Add("@BatchPrintStatus", hearingDetailsUpdate.BatchPrintStatus);
                _hashtable.Add("@FilePreppedDate", hearingDetailsUpdate.FilePreppedDate);
                _hashtable.Add("@UTCDate", hearingDetailsUpdate.UTCDate);
                _hashtable.Add("@Rescheduling", hearingDetailsUpdate.Rescheduling);
                _hashtable.Add("@EvidenceCreated", hearingDetailsUpdate.EvidenceCreated);
                _hashtable.Add("@ByPassInformalHearing", hearingDetailsUpdate.ByPassInformalHearing);
                _hashtable.Add("@AttendHearing", hearingDetailsUpdate.AttendHearing);
                _hashtable.Add("@ContinuousRemarks", hearingDetailsUpdate.ContinuousRemarks);
                _hashtable.Add("@CurrentYearProtestReason", hearingDetailsUpdate.CurrentYearProtestReason);
                _hashtable.Add("@OfferValue", hearingDetailsUpdate.OfferValue);
                _hashtable.Add("@OfferDate", hearingDetailsUpdate.OfferDate);
                _hashtable.Add("@ClientHearingNotice", hearingDetailsUpdate.ClientHearingNotice);
                _hashtable.Add("@IsFormalRescheduleHearing", hearingDetailsUpdate.IsFormalRescheduleHearing);
                _hashtable.Add("@IsInformalRescheduleHearing", hearingDetailsUpdate.IsInformalRescheduleHearing);
                _hashtable.Add("@OwnerOpinionValue", hearingDetailsUpdate.OwnerOpinionValue);
                // _hashtable.Add("@IsCorrectionMotionFiled", hearingDetailsUpdate.IsCorrectionMotionFiled);
                _hashtable.Add("@IsCorrectionMotionFiled", hearingDetailsUpdate.CorrectionMotionStatusid);

                SPName = StoredProcedureNames.usp_updateHearingDetails;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);               

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateHearingDetails-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateHearingDetails-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateHearingDetailsRemarks(PTXboHearingDetailsRemarks hearingDetailsRemarks)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateHearingDetailsRemarks-API  Reached " + ((object)hearingDetailsRemarks).ToJson(false));
                _hashtable.Add("@HearingDetailsId", hearingDetailsRemarks.HearingDetailsId);
                _hashtable.Add("@Remarks", hearingDetailsRemarks.Remarks);
                _hashtable.Add("@UpdatedBy", hearingDetailsRemarks.UpdatedBy);
                //_hashtable.Add("@UpdatedDatetime", hearingDetailsRemarks.UpdatedDatetime);
                _hashtable.Add("@UpdatedDatetime", System.DateTime.Now);

                SPName = StoredProcedureNames.usp_InsertHearingDetailsRemarks;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);                

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateHearingDetailsRemarks-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateHearingDetailsRemarks-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateHearingResult(PTXboHearingResults hearingResults)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            string dateAndTime = "01-01-0001 00:00:00";
            try
            {
                Logger.For(this).Transaction("saveOrUpdateHearingResult-API  Reached " + ((object)hearingResults).ToJson(false));
                _hashtable.Add("@HearingResultId", hearingResults.HearingResultId);
                _hashtable.Add("@HearingDetailsId", hearingResults.HearingDetailsId);
                _hashtable.Add("@HearingTypeId", hearingResults.HearingTypeId);
                _hashtable.Add("@HearingResolutionId", hearingResults.HearingResolutionId);
                _hashtable.Add("@HearingValue", hearingResults.HearingValue);
                _hashtable.Add("@HearingResultReasonCodeId", hearingResults.HearingResultReasonCodeId);
                _hashtable.Add("@PostHearingLandValue", hearingResults.PostHearingLandValue);
                _hashtable.Add("@PostHearingImprovedValue", hearingResults.PostHearingImprovedValue);
                _hashtable.Add("@PostHearingMarketValue", hearingResults.PostHearingMarketValue);
                _hashtable.Add("@PostHearingTotalValue", hearingResults.PostHearingTotalValue);
                _hashtable.Add("@HearingAgentId", hearingResults.HearingAgentId);
                _hashtable.Add("@HearingStatusId", hearingResults.HearingStatusId);
                int HearingFinalized = 0;
                if (hearingResults.HearingFinalized == true)
                {
                    HearingFinalized = 1;
                }
                _hashtable.Add("@HearingFinalized", HearingFinalized);
                _hashtable.Add("@DismissalAuthStatusId", hearingResults.DismissalAuthStatusId);
                _hashtable.Add("@confirmationLetterReceivedDate", hearingResults.confirmationLetterReceivedDate);
                if (dateAndTime != hearingResults.confirmationLetterDateTime.ToString())
                {
                    _hashtable.Add("@confirmationLetterDateTime", hearingResults.confirmationLetterDateTime);
                }
                else
                {
                    _hashtable.Add("@confirmationLetterDateTime", null);
                }
                if (dateAndTime != hearingResults.completionDateAndTime.ToString())
                {
                    _hashtable.Add("@completionDateAndTime", hearingResults.completionDateAndTime);
                }
                else
                {
                    _hashtable.Add("@completionDateAndTime", null);
                }
                if (dateAndTime != hearingResults.HearingResultsSentOn.ToString())
                {
                    _hashtable.Add("@HearingResultsSentOn", hearingResults.HearingResultsSentOn);
                }
                else
                {
                    _hashtable.Add("@HearingResultsSentOn", null);
                }
                int InvoiceGenerated = 0;
                if (hearingResults.InvoiceGenerated == true)
                {
                    InvoiceGenerated = 1;
                }

                _hashtable.Add("@InvoiceGenerated", InvoiceGenerated);
                int ReviewForArbitration = 0;
                if (hearingResults.ReviewForArbitration == true)
                {
                    ReviewForArbitration = 1;
                }
                _hashtable.Add("@ReviewForArbitration", ReviewForArbitration);
                int ClientRequestedForArbitration = 0;
                if (hearingResults.ClientRequestedForArbitration == true)
                {
                    ClientRequestedForArbitration = 1;
                }
                _hashtable.Add("@ClientRequestedForArbitration", ClientRequestedForArbitration);
                _hashtable.Add("@HearingProcessingStatusId", hearingResults.HearingProcessingStatusId);
                _hashtable.Add("@UpdatedBy", hearingResults.UpdatedBy);
                //_hashtable.Add("@UpdatedDateTime", hearingResults.UpdatedDateTime);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                _hashtable.Add("@HearingTrackingCodeId", hearingResults.HearingTrackingCodeId);
                _hashtable.Add("@HearingResultReportId", hearingResults.HearingResultReportId);
                int ExemptInvoice = 0;
                if (hearingResults.ExemptInvoice == true)
                {
                    ExemptInvoice = 1;
                }
                _hashtable.Add("@ExemptInvoice", ExemptInvoice);
                int ResultGenerated = 0;
                if (hearingResults.ResultGenerated == true)
                {
                    ResultGenerated = 1;
                }
                _hashtable.Add("@ResultGenerated", ResultGenerated);
                if (dateAndTime != hearingResults.FinalizedDate.ToString())
                {
                    _hashtable.Add("@FinalizedDate", hearingResults.FinalizedDate);
                }
                else
                {
                    _hashtable.Add("@FinalizedDate", null);
                }
                _hashtable.Add("@HRInvoiceStatusid", hearingResults.HRInvoiceStatusid);
                _hashtable.Add("@AppraiserName", hearingResults.AppraiserName);
                int ReviewBindingArbitration = 0;
                if (hearingResults.ReviewBindingArbitration == true)
                {
                    ReviewBindingArbitration = 1;
                }
                _hashtable.Add("@ReviewBindingArbitration", ReviewBindingArbitration);

                SPName = StoredProcedureNames.usp_UpdateHearingResult;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);                

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateHearingResult-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateHearingResult-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateValueNotice(PTXboPriorValueNotice valueNotice)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            string dateAndTime = "01-01-0001 00:00:00";
            try
            {
                Logger.For(this).Transaction("saveOrUpdateValueNotice-API  Reached " + ((object)valueNotice).ToJson(false));
                _hashtable.Add("@ValueNoticeId", valueNotice.ValueNoticeId);
                if (dateAndTime != valueNotice.NoticedDate.ToString())
                {
                    _hashtable.Add("@NoticedDate", valueNotice.NoticedDate);
                }
                else
                {
                    _hashtable.Add("@NoticedDate", null);
                }                
                _hashtable.Add("@IFileNumber", valueNotice.IFileNumber);
                _hashtable.Add("@NoticeLandValue", valueNotice.NoticeLandValue);
                _hashtable.Add("@NoticeImprovedValue", valueNotice.NoticeImprovedValue);
                _hashtable.Add("@NoticeMarketValue", valueNotice.NoticeMarketValue);
                _hashtable.Add("@NoticeTotalValue", valueNotice.NoticeTotalValue);
                _hashtable.Add("@Statusid", valueNotice.Statusid);
                _hashtable.Add("@UpdatedBy", valueNotice.UpdatedBy);
                //_hashtable.Add("@UpdatedDateTime", valueNotice.UpdatedDateTime);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                int CertifiedValueSelected = 0;
                if (valueNotice.CertifiedValueSelected == true)
                {
                    CertifiedValueSelected = 1;
                }
                _hashtable.Add("@CertifiedValueSelected", CertifiedValueSelected);
                _hashtable.Add("@CertifiedLandValue", valueNotice.CertifiedLandValue);
                _hashtable.Add("@CertifiedImprovedValue", valueNotice.CertifiedImprovedValue);
                _hashtable.Add("@CertifiedMarketValue", valueNotice.CertifiedMarketValue);
                _hashtable.Add("@CertifiedTotalValue", valueNotice.CertifiedTotalValue);
                int PendingNoticedSelected = 0;
                if (valueNotice.PendingNoticedSelected == true)
                {
                    PendingNoticedSelected = 1;
                }
                _hashtable.Add("@PendingNoticedSelected", PendingNoticedSelected);
                _hashtable.Add("@PropertytaxAccountID", valueNotice.PropertytaxAccountID);
                _hashtable.Add("@Propertytax_TaxYear", valueNotice.Propertytax_TaxYear);
                _hashtable.Add("@TargetValue", valueNotice.TargetValue);
                _hashtable.Add("@TargetUEValue", valueNotice.TargetUEValue);
                _hashtable.Add("@RenoticedLandValue", valueNotice.RenoticedLandValue);
                _hashtable.Add("@RenoticedMarketValue", valueNotice.RenoticedMarketValue);
                _hashtable.Add("@RenoticedImprovedValue", valueNotice.RenoticedImprovedValue);
                _hashtable.Add("@RenoticeTotalValue", valueNotice.RenoticeTotalValue);
                if (dateAndTime != valueNotice.ReNoticedDate.ToString())
                {
                    _hashtable.Add("@RenoticedDate", valueNotice.ReNoticedDate);
                }
                else
                {
                    _hashtable.Add("@RenoticedDate", null);
                }

                SPName = StoredProcedureNames.usp_UpdateValueNotice;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
               
                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateValueNotice-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateValueNotice-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateInvoice(PTXboInvoice invoice)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            string dateAndTime = "01-01-0001 00:00:00";
            try
            {
                Logger.For(this).Transaction("saveOrUpdateInvoice-API  Reached " + ((object)invoice).ToJson(false));
                _hashtable.Add("@GroupId", invoice.GroupId);
                _hashtable.Add("@ProjectId", invoice.ProjectId);
                _hashtable.Add("@YealyHearingDetailsId", invoice.YealyHearingDetailsId);
                _hashtable.Add("@ClientId", invoice.ClientId);
                _hashtable.Add("@InvoiceTypeId", invoice.InvoiceTypeId);
                if (dateAndTime != invoice.InvoiceDate.ToString())
                {
                    _hashtable.Add("@InvoiceDate", invoice.InvoiceDate);
                }
                else
                {
                    _hashtable.Add("@InvoiceDate", null);
                }
                if (dateAndTime != invoice.PaymentDueDate.ToString())
                {
                    _hashtable.Add("@PaymentDueDate", invoice.PaymentDueDate);
                }
                else
                {
                    _hashtable.Add("@PaymentDueDate", null);
                }
                _hashtable.Add("@InvoiceGenerationConfigIDUsedForCalculation", invoice.InvoiceGenerationConfigIDUsedForCalculation);
                _hashtable.Add("@InitialAssessedValue", invoice.InitialAssessedValue);
                _hashtable.Add("@FinalAssessedValue", invoice.FinalAssessedValue);
                _hashtable.Add("@PriorYearTaxRate", invoice.PriorYearTaxRate);
                _hashtable.Add("@ContingencyPercentage", invoice.ContingencyPercentage);
                _hashtable.Add("@ContingencyFee", invoice.ContingencyFee);
                _hashtable.Add("@FlatFee", invoice.FlatFee);
                _hashtable.Add("@InvoiceAmount", invoice.InvoiceAmount);
                if (dateAndTime != invoice.CreatedDateAndTime.ToString())
                {
                    _hashtable.Add("@CreatedDateAndTime", invoice.CreatedDateAndTime);
                }
                else
                {
                    _hashtable.Add("@CreatedDateAndTime", null);
                }
                if (dateAndTime != invoice.SentDateAndTime.ToString())
                {
                    _hashtable.Add("@SentDateAndTime", invoice.SentDateAndTime);
                }
                else
                {
                    _hashtable.Add("@SentDateAndTime", null);
                }
                _hashtable.Add("@DeliveryMethodId", invoice.DeliveryMethodId);
                _hashtable.Add("@DeliveryStatusId", invoice.DeliveryStatusId);
                int AutoGenerated = 0;
                if (invoice.AutoGenerated == true)
                {
                    AutoGenerated = 1;
                }
                _hashtable.Add("@AutoGenerated", AutoGenerated);
                _hashtable.Add("@ManuallyGeneratedUserId", invoice.ManuallyGeneratedUserId);
                _hashtable.Add("@ManuallyGeneratedUserRoleId", invoice.ManuallyGeneratedUserRoleId);
                int OnHold = 0;
                if (invoice.OnHold == true)
                {
                    OnHold = 1;
                }
                _hashtable.Add("@OnHold", OnHold);
                if (dateAndTime != invoice.OnHoldDate.ToString())
                {
                    _hashtable.Add("@OnHoldDate", invoice.OnHoldDate);
                }
                else
                {
                    _hashtable.Add("@OnHoldDate", null);
                }
                _hashtable.Add("@TotalAmountPaid", invoice.TotalAmountPaid);
                _hashtable.Add("@InvoicingStatusId", invoice.InvoicingStatusId);
                _hashtable.Add("@InvoicingProcessingStatusId", invoice.InvoicingProcessingStatusId);
                _hashtable.Add("@InvoiceDescription", invoice.InvoiceDescription);
                _hashtable.Add("@CompoundInterest", invoice.CompoundInterest);
                _hashtable.Add("@InvoiceCreditAmount", invoice.InvoiceCreditAmount);
                _hashtable.Add("@PaymentStatusId", invoice.PaymentStatusId);
                _hashtable.Add("@InvoiceGroupingTypeId", invoice.InvoiceGroupingTypeId);
                _hashtable.Add("@TaxYear", invoice.TaxYear);
                _hashtable.Add("@Reduction", invoice.Reduction);
                _hashtable.Add("@TotalEstimatedTaxSavings", invoice.TotalEstimatedTaxSavings);
                _hashtable.Add("@AmountPaid", invoice.AmountPaid);
                _hashtable.Add("@AmountAdjusted", invoice.AmountAdjusted);
                _hashtable.Add("@ApplicableInterest", invoice.ApplicableInterest);
                _hashtable.Add("@InterestPaid", invoice.InterestPaid);
                _hashtable.Add("@InterestAdjusted", invoice.InterestAdjusted);
                _hashtable.Add("@AmountDue", invoice.AmountDue);
                _hashtable.Add("@UpdatedBy", invoice.UpdatedBy);
                //_hashtable.Add("@UpdatedDateTime", invoice.UpdatedDateTime);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                _hashtable.Add("@InterestRateID", invoice.InterestRateID);
                _hashtable.Add("@IntitalLand", invoice.IntitalLand);
                _hashtable.Add("@IntialImproved", invoice.IntialImproved);
                _hashtable.Add("@InitialMarket", invoice.InitialMarket);
                _hashtable.Add("@FinalLand", invoice.FinalLand);
                _hashtable.Add("@FinalImproved", invoice.FinalImproved);
                _hashtable.Add("@FinalMarket", invoice.FinalMarket);
                _hashtable.Add("@CreditAmountApplied", invoice.CreditAmountApplied);
                _hashtable.Add("@AttorneyFee", invoice.AttorneyFee);
                _hashtable.Add("@ServiceFee", invoice.ServiceFee);
                _hashtable.Add("@CourtCost", invoice.CourtCost);
                int isInvoiceDefect = 0;
                if (invoice.isInvoiceDefect == true)
                {
                    isInvoiceDefect = 1;
                }
                _hashtable.Add("@isInvoiceDefect", isInvoiceDefect);


                SPName = StoredProcedureNames.usp_InsertInvoice;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);               

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateInvoice-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateInvoice-API  Error " + ex);
                throw ex;
            }
        }
    }
}