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
using Spartaxx.Utilities.Logging;
using Spartaxx.Utilities.Extenders;
using Spartaxx.DataObjects.NHibernate.DataObjects;

namespace Spartaxx.DataAccess
{
    public class HearingNoticeRepocitory : IHearingNoticeRepocitory
    {
        private readonly DapperConnection _Connection;
        public HearingNoticeRepocitory(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        public List<PTXboHearingNoticeAllocatedDocuments> getHearingNoticeEntryAllottedDocuments(PTXboHearingNoticeAllotmentSearchRequest objHearingNoticeAllotmentSearchRequest)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            try
            {
                Logger.For(this).Transaction("getHearingNoticeEntryAllottedDocuments-API  Reached " + ((object)objHearingNoticeAllotmentSearchRequest).ToJson(false));
                if (objHearingNoticeAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.EntryQueue)
                    SPName = StoredProcedureNames.usp_GetHearingNoticeEntryAllottedDocuments;
                else if (objHearingNoticeAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.AuditQueue)
                    SPName = StoredProcedureNames.usp_GetHearingNoticeAuditAllottedDocuments;
                else if (objHearingNoticeAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.DefectQueue)
                    SPName = StoredProcedureNames.usp_GetHearingNoticeDefectAllottedDocuments;
                else if (objHearingNoticeAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.ClarificationQueue)
                    SPName = StoredProcedureNames.usp_GetHearingNoticeClarificationAllottedDocuments;
                else if (objHearingNoticeAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.QCQueue)
                    SPName = StoredProcedureNames.usp_GetHearingNoticeQCAllottedDocuments;
                else if (objHearingNoticeAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)
                    SPName = StoredProcedureNames.usp_GetHearingNoticeMultipleEntryAllottedDocuments;

                _hashtable.Add("@UserID", objHearingNoticeAllotmentSearchRequest.User.Userid);
                _hashtable.Add("@UserRoleID", objHearingNoticeAllotmentSearchRequest.UserRole.UserRoleid);
                _hashtable.Add("@ClientNumber", objHearingNoticeAllotmentSearchRequest.ClientNumber);
                _hashtable.Add("@AccountNumber", objHearingNoticeAllotmentSearchRequest.AccountNumber);
                _hashtable.Add("@FirstName", objHearingNoticeAllotmentSearchRequest.FirstName);
                _hashtable.Add("@LastName", objHearingNoticeAllotmentSearchRequest.LastName);
                _hashtable.Add("@BatchCode", objHearingNoticeAllotmentSearchRequest.BatchCode);
                _hashtable.Add("@CADLegalName", objHearingNoticeAllotmentSearchRequest.CADLegalName);
                _hashtable.Add("@PropertyAddress", objHearingNoticeAllotmentSearchRequest.PropertyAddress);
                var result = _Connection.Select<PTXboHearingNoticeAllocatedDocuments>(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                Logger.For(this).Transaction("getHearingNoticeEntryAllottedDocuments-API  Ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getHearingNoticeEntryAllottedDocuments-API  Error " + ex);
                throw ex;
            }
        }

        public PTXboYearlyHearingDetailsList getYearlyHearingDetails(int YearlyHearingDetailsId)
        {
            Hashtable _hashtable = new Hashtable();
            List<PTXboYearlyHearingDetailsList> lstPTXboYearlyHearingDetails = null;
            PTXboYearlyHearingDetailsList pTXboYearlyHearingDetails = new PTXboYearlyHearingDetailsList();
            try
            {
                string objInput = "getYearlyHearingDetails : " + YearlyHearingDetailsId.ToString();
                Logger.For(this).Transaction("getYearlyHearingDetails-API  Reached " + ((object)objInput).ToJson(false));
                _hashtable.Add("@YearlyHearingDetailsId", YearlyHearingDetailsId);

                lstPTXboYearlyHearingDetails = _Connection.Select<PTXboYearlyHearingDetailsList>(StoredProcedureNames.usp_getYearlyHearingDetailsById, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (lstPTXboYearlyHearingDetails.Count > 0)
                {
                    pTXboYearlyHearingDetails.HearingDetailsId = lstPTXboYearlyHearingDetails[0].HearingDetailsId;
                }
                Logger.For(this).Transaction("getYearlyHearingDetails-API  Ends successfully ");
                return pTXboYearlyHearingDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getYearlyHearingDetails-API  Error " + ex);
                throw ex;
            }
        }
        public ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument objIndexedNoticeDocument, int AssignedQueue)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            int Userid = 0;
            int UserRoleid = 0;
            try
            {
                Logger.For(this).Transaction("getNextRecordList-API  Reached " + ((object)objIndexedNoticeDocument).ToJson(false));
                //_hashtable.Add("@TokenID", 11);
                //var Result = _Connection.SelectDynamicMultiple("usp_get_AllReqListDefinitions", _hashtable,
                //    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.EntryQueue)
                {
                    SPName = StoredProcedureNames.usp_getNextEntryHearingNotice;
                    Userid = objIndexedNoticeDocument.entryUser.Userid;
                    UserRoleid = objIndexedNoticeDocument.entryUserRole.UserRoleid;
                }
                else if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.AuditQueue)
                {
                    SPName = StoredProcedureNames.usp_getNextAuditHearingNotice;
                    Userid = objIndexedNoticeDocument.auditedUser.Userid;
                    UserRoleid = objIndexedNoticeDocument.auditedUserRole.UserRoleid;
                }
                else if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.DefectQueue)
                {
                    SPName = StoredProcedureNames.usp_getNextDefectHearingNotice;
                    Userid = objIndexedNoticeDocument.defectAssignedUser.Userid;
                    UserRoleid = objIndexedNoticeDocument.defectAssignedUserRole.UserRoleid;
                }
                else if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.ClarificationQueue)
                {
                    SPName = StoredProcedureNames.usp_getNextClarificationHearingNotice;
                    Userid = objIndexedNoticeDocument.clarificationList[0].RespondedUser.Userid;
                    UserRoleid = objIndexedNoticeDocument.clarificationList[0].RespondedUserRole.UserRoleid;
                }

                _hashtable.Add("@UserID", Userid);
                _hashtable.Add("@UserRoleID", UserRoleid);
                _hashtable.Add("@IndexedNoticeDocumentID", objIndexedNoticeDocument.IndexedNoticeDocumentID);
                if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)
                {
                    SPName = StoredProcedureNames.usp_getNextMultipleEntryRecordList;
                    Userid = objIndexedNoticeDocument.entryUser.Userid;
                    UserRoleid = objIndexedNoticeDocument.entryUserRole.UserRoleid;

                    _hashtable = new Hashtable();
                    _hashtable.Add("@UserID", Userid);
                    _hashtable.Add("@UserRoleID", UserRoleid);
                    _hashtable.Add("@TCK", objIndexedNoticeDocument.TCK);
                    _hashtable.Add("@Offset", objIndexedNoticeDocument.Offset);
                    _hashtable.Add("@CountyCode", objIndexedNoticeDocument.countyCode);
                    _hashtable.Add("@BatchCode", objIndexedNoticeDocument.batchCode);
                    _hashtable.Add("@DocumentName", objIndexedNoticeDocument.DocumentName);
                }

                var SP_Result = _Connection.SelectMultiple(SPName, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXdtHearingNotices>(),
                    gr => gr.Read<PTXdtHearingNoticeClarification>(),
                    gr => gr.Read<PTXdtHearingNoticeRemarks>(),
                    gr => gr.Read<PTXdtUserEnteredHearingTypeForNotice>(),
                    gr => gr.Read<PTXdtHearingDetailsRemarks>()
                );
                var result = convertdtResultToIndexedNoticeDocumentObject(SP_Result, objIndexedNoticeDocument, AssignedQueue);
                Logger.For(this).Transaction("getNextRecordList-API  Ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getNextRecordList-API  Error " + ex);
                throw ex;
            }
        }

        public ExpandoObject getNext_QC_RecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                Logger.For(this).Transaction("getNext_QC_RecordList-API  Reached " + ((object)indexedNoticeList).ToJson(false));
                _hashtable.Add("@UserID", indexedNoticeList.qcAssignedUser.Userid);
                _hashtable.Add("@UserRoleID", indexedNoticeList.qcAssignedUserRole.UserRoleid);
                _hashtable.Add("@IndexedNoticeDocumentID", indexedNoticeList.IndexedNoticeDocumentID);

                var SP_Result = _Connection.SelectMultiple(StoredProcedureNames.usp_getNextQCHearingNotice, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXdtHearingNotices>(),
                    gr => gr.Read<PTXdtHearingNoticeClarification>(),
                    gr => gr.Read<PTXdtHearingNoticeRemarks>(),
                    gr => gr.Read<PTXdtUserEnteredHearingTypeForNotice>(),
                    gr => gr.Read<PTXdtHearingDetails>(),
                    gr => gr.Read<PTXdtHearingDetailsRemarks>()
                );
                var result = convertdtResultToIndexedNoticeDocumentObject(SP_Result, indexedNoticeList, AssignedQueue);
                Logger.For(this).Transaction("getNext_QC_RecordList-API  Ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getNext_QC_RecordList-API  Error " + ex);
                throw ex;
            }
        }

        private ExpandoObject convertdtResultToIndexedNoticeDocumentObject(dynamic SP_Result, PTXdoIndexedNoticeDocument objIndexedNoticeDocument, int AssignedQueue)
        {
            var hearingNoticeRemarksList = new List<PTXdoHearingDetailsRemarks>();
            var objPTXdoIndexedNoticeDocumentList = new List<PTXdoIndexedNoticeDocument>();
            var HearingDetails = new List<PTXdoHearingDetails>();   //only for QC
            dynamic result = new ExpandoObject();
            try
            {
                Logger.For(this).Transaction("convertdtResultToIndexedNoticeDocumentObject-API  Reached " + ((object)objIndexedNoticeDocument).ToJson(false));
                if (SP_Result != null)
                {
                    #region IndexedNoticeDocument Object
                    var dtEntryRecordList = (List<PTXdtHearingNotices>)SP_Result.Item1;


                    objPTXdoIndexedNoticeDocumentList = dtEntryRecordList.Select(b => new PTXdoIndexedNoticeDocument
                    {
                        IndexedNoticeDocumentID = b.IndexedNoticeDocumentId,
                        batchCode = b.BatchCode,
                        /* Included FileCabinetID in NoticeDocumentType table and removed from IndexedNoticeDocument table */
                        PWImageID = b.PWImageID,
                        //PWFileCabinetID = b.PWFileCabinetID,
                        documentType = new PTXdoNoticeDocumentType()
                        {
                            NoticeDocumentTypeId = b.NoticeDocumentTypeId,
                            NoticeDocumentTypeValue = b.NoticedDocumentType,
                            FileCabinetID = b.FileCabinetID /* Included FileCabinetID in NoticeDocumentType table */
                        },
                        taxYear = b.TaxYear,
                        countyCode = b.CountyCode,
                        client = new PTXdoClient() { clientId = b.ClientId, ClientNumber = b.ClientNumber },

                        accountNumber = b.AccountNumber,
                        account = new PTXdoAccount()
                        {
                            AccountId = b.AccountId,
                            //propertyDetails = new PTXdoPropertyDetails()
                            //{

                            //    PropertyAddress = new PTXdoAddress()
                            //    {
                            //        addressLine1 = b.PropertyAddress
                            //    }
                            //},

                            /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 Start*/
                            YearlyHearingDetails = (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.QCQueue) ? (getYearlyHearingDetailsById(b.YearlyHearingDetails))
                                : (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue) ? (getYearlyHearingDetails(b.AccountId, b.TaxYear))
                                : (new List<PTXdoYearlyHearingDetails>
                                        {
                                            new PTXdoYearlyHearingDetails()
                                            {
                                                propertyDetails = new PTXdoPropertyDetails(){PropertyAddress = new PTXdoAddress(){addressLine1 = b.PropertyAddress}},
                                                DNDCodeId = new PTXdoDNDCodes(){DNDCodeId= b.DNDCodeID},                                                
                                            }
                                        }
                                    ), //By Mano

                            /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 End */
                            
                            County = new PTXdoCounty()
                            {
                                Countyid = b.Countyid,
                                CountyName = b.CountyName,
                                CountyCode = b.CountyCode,
                                Website = b.Website
                            },
                            EndDate = b.EndDate,
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
                        userEnteredHearingNoticeData = new PTXdoUserEnteredHearingNoticeData()
                        {
                            EvidenceDueDate = b.EvidenceDueDate,
                            FormalHearingDate = b.FormalHearingDate,
                            FormalHearingTime = b.FormalHearingTime,
                            InfomalHearingTime = b.InfomalHearingtime,
                            InformalHearingDate = b.InformalHearingDate,
                            PanelDocketId = b.PanelDocketId,
                            FormalPanelDocketID = b.FormalPanelDocketID,
                            InformalPanelDocketID = b.InformalPanelDocketID,
                            TaxYear = b.TaxYear,

                            //Value Notice
                            IFileNumber = b.IFileNumber,
                            LandValue = b.LandValue,
                            ImprovedValue = b.ImprovedValue,
                            MarketValue = b.MarketValue,
                            NoticeDate = b.NoticeDate,
                            TotalValue = b.TotalValue,
                            RenoticedLandValue = b.RenoticedLandValue,
                            RenoticedImprovedValue = b.RenoticedImprovedValue,
                            RenoticedMarketValue = b.RenoticedMarketValue,
                            RenoticeTotalValue = b.RenoticeTotalValue,
                            IsValueNoticeAlreadyExist = b.IsValueNoticeAlreadyExist,
                            IsValueNoticeUpdateRequired = b.IsValueNoticeUpdateRequired,

                            //To show the Formal & informal hearing dates if exist
                            Existing_EvidenceDueDate = b.Existing_EvidenceDueDate,
                            Existing_FormalHearingDate = b.Existing_FormalHearingDate,
                            Existing_FormalHearingTime = b.Existing_FormalHearingTime,
                            Existing_InfomalHearingtime = b.Existing_InformalHearingTime,
                            Existing_InformalHearingDate = b.Existing_InformalHearingDate,
                            /* Existing Hearing Type and updated by added*/
                            Existing_HearingType = new PTXdoHearingType() { HearingTypeId = b.Existing_HearingTypeId, HearingType = b.Existing_HearingType },
                            UpdatedBy = new PTXdoUser() { Userid = b.UserEnteredUpdatedBy },
                            UpdatedDateTime = b.UserEnteredUpdatedDateTime,
                            UserEnteredHearingNoticeDataId = b.UserEnteredHearingNoticeDataId,

                            IsFormalRescheduleHearing = b.IsFormalRescheduleHearing,
                            IsInformalRescheduleHearing = b.IsInformalRescheduleHearing,
                            InformalNoticeLetterDate = b.InformalNoticeLetterDate,
                            FormalNoticeLetterDate = b.FormalNoticeLetterDate,

                            HearingResolution = new PTXdoHearingResolution() { HearingResolutionId = b.HearingResolutionId },
                            ReasonCodes = new PTXdoHearingResultsReasonCode() { HearingResultsReasonCodeId = b.ReasonCodeId }
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
                        entryUserRole = new PTXdoUserRole()
                        {
                            UserRoleid = b.EntryUserRoleId
                        },
                        EntryAssignedBy = new PTXdoUser()
                        {
                            Username = b.EntryAssignBy,
                            Userid = b.EntryAssignedBy
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
                        defectAssignedUserRole = new PTXdoUserRole()
                        {
                            UserRoleid = b.DefectAssignedUserRoleId
                        },
                        auditedUser = new PTXdoUser()
                        {
                            Userid = b.AuditedUserId
                        },
                        auditedUserRole = new PTXdoUserRole()
                        {
                            UserRoleid = b.AuditedUserRoleId
                        },
                        defectNotice = b.DefectNotice,
                        defectResolved = b.DefectResolved,
                        defectReasonCode = new PTXdoDocumentDefectCodes()
                        {
                            DocumentDefectCodeId = b.DocumentDefectCodeId,
                            DocumentDefectCodes = b.DocumentDefectCodes
                        },
                        defectRectifiedDateAndTime = b.DefectRectifiedDateAndTime,
                        OnHoldDateAndTime = b.OnHoldDateAndTime,
                        ScannedDateAndTime = b.ScannedDateAndTime,
                        TCK = b.TCK,
                        Offset = b.Offset,
                        DocumentName = (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue || AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.QCQueue)
                                ? b.DocumentName : (string.IsNullOrEmpty(b.DocumentName) ? b.DocumentFileName : b.DocumentName),   //By Mano
                        /* Existing Hearing Type and updated by added*/
                        UpdatedBy = new PTXdoUser() { Userid = b.UpdatedBy },
                        UpdatedDateTime = b.UpdatedDateTime,
                        Website = b.Website,
                        HearingModeid = b.HearingModeid,
                        FormalWebex = b.FormalWebex,
                        InformalWebex = b.InformalWebex,
                        Disaster = new PTXdoDisaster()
                        {
                            DisasterId = b.DisasterId
                        },
                        IsDisasterProtest = b.IsDisasterProtest,
                        HearingResultData = new PTXdoHearingResult()
                        {
                            PostHearingImprovedValue = b.PostHearingImprovedValue,
                            PostHearingLandValue= b.PostHearingLandValue,
                            PostHearingMarketValue = b.PostHearingMarketValue,
                            PostHearingTotalValue=b.PostHearingTotalValue
                        },
                        HearingDetailsData= new PTXdoHearingDetails()
                        {
                            FilePreppedDate= b.FormalFilePreppedDate,
                            InformalFilePreppedDate= b.InformalFilePreppedDate
                        }
                    }).ToList();

                    if (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)    //By Mano
                        objIndexedNoticeDocument = objPTXdoIndexedNoticeDocumentList.OrderByDescending(q => q.userEnteredHearingNoticeData.UpdatedDateTime).FirstOrDefault();
                    #endregion

                    #region Clarification Object
                    /* Clarification Log: First row will be the latest record. */
                    if (SP_Result.Item2 != null && SP_Result.Item2.Count > 0)
                    {
                        var dtClarificationList = (List<PTXdtHearingNoticeClarification>)SP_Result.Item2;

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
                            RequestedUserRole = new PTXdoUserRole()
                            {
                                UserRoleid = a.RequestedUserRoleId,
                                UserRole = a.RequestedUserRole
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
                            RespondedUserRole = new PTXdoUserRole()
                            {
                                UserRoleid = a.RespondedUserRoleId,
                                UserRole = a.RespondedUserRole
                            },
                            Response = a.Response,
                            RespondedDateAndTime = a.RespondedDateAndTime,
                            ClarificationAssignedBy = new PTXdoUser()
                            {
                                Username = a.ClarificationAssignedBy
                            },
                            ClarificationAssignedOn = a.ClarificationAssignedOn

                        }).ToList();

                        objIndexedNoticeDocument.clarificationList = clarificationList;

                        if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)    //By Mano
                        {
                            /* Get the Clarification list*/
                            objIndexedNoticeDocument.clarificationList = clarificationList.Where(a => a.IndexedNoticeDocument.IndexedNoticeDocumentID == objIndexedNoticeDocument.IndexedNoticeDocumentID).ToList();
                        }
                    }
                    /* End */

                    #endregion

                    #region Remarks Object
                    /* Remarks Log: By default rows displayed in UpdatedDateTime desc order */
                    if (SP_Result.Item3 != null && SP_Result.Item3.Count > 0)
                    {
                        var dtHearingNoticeRemarks = (List<PTXdtHearingNoticeRemarks>)SP_Result.Item3;

                        List<PTXdoUserEnteredHearingNoticeRemarks> UserEnteredHearingNoticeRemarksList = dtHearingNoticeRemarks.Select(c => new PTXdoUserEnteredHearingNoticeRemarks
                        {
                            UserEnteredHearingNoticeRemarksId = c.UserEnteredHearingNoticeRemarksId,
                            UserEnteredHearingNoticeData = new PTXdoUserEnteredHearingNoticeData()
                            {
                                UserEnteredHearingNoticeDataId = c.UserEnteredHearingNoticeDataId
                            },
                            Remarks = c.Remarks,
                            UpdatedBy = new PTXdoUser()
                            {
                                Userid = c.UpdatedBy,
                                Username = c.Username,
                                Firstname = c.Firstname,
                                Lastname = c.Lastname
                            },
                            UpdatedDateTime = c.UpdatedDateTime
                        }).ToList();

                        if (objIndexedNoticeDocument.userEnteredHearingNoticeData == null)
                            objIndexedNoticeDocument.userEnteredHearingNoticeData = new PTXdoUserEnteredHearingNoticeData();
                        objIndexedNoticeDocument.userEnteredHearingNoticeData.UserEnteredHearingNoticeRemarks = UserEnteredHearingNoticeRemarksList;

                        if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)    //By Mano
                        {
                            /* Get the Remarks list*/
                            objIndexedNoticeDocument.userEnteredHearingNoticeData.UserEnteredHearingNoticeRemarks = UserEnteredHearingNoticeRemarksList.Where(a => a.UserEnteredHearingNoticeData.UserEnteredHearingNoticeDataId == objIndexedNoticeDocument.userEnteredHearingNoticeData.UserEnteredHearingNoticeDataId).ToList();
                        }
                    }
                    /* End */
                    #endregion

                    #region Hearing Type Object
                    /* Hearing Type */
                    if (SP_Result.Item4 != null && SP_Result.Item4.Count > 0)
                    {
                        var dtHearingTypeForNotice = (List<PTXdtUserEnteredHearingTypeForNotice>)SP_Result.Item4;

                        List<PTXdoUserEnteredHearingTypeForNotice> UserEnteredHearingTypeForNotice = dtHearingTypeForNotice.Select(c => new PTXdoUserEnteredHearingTypeForNotice
                        {
                            HearingType = new PTXdoHearingType() { HearingTypeId = c.HearingTypeId },
                            UserEnteredHearingTypeForNoticeId = c.UserEnteredHearingTypeForNoticeId,

                            UserEnteredHearingNoticeData = new PTXdoUserEnteredHearingNoticeData()
                            {
                                UserEnteredHearingNoticeDataId = c.UserEnteredHearingNoticeDataId
                            },
                            /* Updated By and Updated Date Time*/
                            UpdatedBy = new PTXdoUser() { Userid = c.UpdatedBy },
                            UpdatedDateTime = c.UpdatedDateTime
                        }).ToList();
                        if (objIndexedNoticeDocument.userEnteredHearingNoticeData == null)
                            objIndexedNoticeDocument.userEnteredHearingNoticeData = new PTXdoUserEnteredHearingNoticeData();
                        objIndexedNoticeDocument.userEnteredHearingNoticeData.UserEnteredHearingTypeForNoticeList = UserEnteredHearingTypeForNotice;

                        if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)    //By Mano
                        {
                            /* Get the Clarification list*/
                            objIndexedNoticeDocument.userEnteredHearingNoticeData.UserEnteredHearingTypeForNoticeList = UserEnteredHearingTypeForNotice.Where(a => a.UserEnteredHearingNoticeData.UserEnteredHearingNoticeDataId == objIndexedNoticeDocument.userEnteredHearingNoticeData.UserEnteredHearingNoticeDataId).ToList();
                        }
                    }
                    /* End */
                    #endregion

                    List<PTXdtHearingDetailsRemarks> dtHearingDetailsRemarks = null;
                    bool IsValid = false;
                    if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.QCQueue)
                    {
                        #region Hearing Details Object
                        if (SP_Result.Item5 != null && SP_Result.Item5.Count > 0)
                        {
                            var dtHearingDetails = (List<PTXdtHearingDetails>)SP_Result.Item5;

                            HearingDetails = dtHearingDetails.Select(a => new PTXdoHearingDetails
                            {
                                HearingDetailsId = a.HearingDetailsId,
                                HearingType = new PTXdoHearingType()
                                {
                                    HearingTypeId = a.HearingTypeId
                                },
                                InFormalHearingDate = a.InformalHearingDate,
                                InFormalHearingTime = a.InformalHearingTime,
                                FormalHearingDate = a.FormalHearingDate,
                                FormalHearingTime = a.FormalHearingTime,
                                InformalAssignedAgent = new PTXdoAgent()
                                {
                                    AgentId = a.InformalAssignedAgentId
                                },
                                FormalAssignedAgent = new PTXdoAgent()
                                {
                                    AgentId = a.FormalAssignedAgentId
                                },
                                InformalBatchPrintDate = a.InformalBatchPrintDate,
                                FormalBatchPrintDate = a.FormalBatchPrintDate,
                                InformalPanelDocketID = a.InformalPanelDocketID,
                                FormalPanelDocketID = a.FormalPanelDocketID,
                                EvidenceDueDate = a.EvidenceDueDate,
                                InformalInformalHearingDate = a.InformalInformalHearingDate,
                                //HearingDateAudited = a.HearingDateAudited,
                                //HearingCounty = new PTXdoCounty() { Countyid = a.Countyid },
                                HearingCompletedDate = a.HearingCompletedDate,
                                HearingStatus = new PTXdoHearingStatus() { HearingStatusId = a.HearingStatusId },

                                /* Get YearlyHearingDetails and ValueNotice Details*/
                                YearlyHearingDetails = new PTXdoYearlyHearingDetails()
                                {
                                    YearlyHearingDetailsId = a.YearlyHearingDetailsId,
                                    ValueHearingNotice = new PTXdoValueNotice()
                                    {
                                        ValueNoticeId = a.ValueNoticeId,
                                        IFileNumber = a.IFileNumber,
                                        NoticedDate = a.NoticedDate,
                                        NoticeImprovedValue = a.NoticeImprovedValue,
                                        NoticeLandValue = a.NoticeLandValue,
                                        NoticeMarketValue = a.NoticeMarketValue,
                                        NoticeTotalValue = a.NoticeTotalValue,
                                        Status = new PTXdoStatus() { Statusid = a.Statusid }
                                    }
                                }

                            }).ToList();
                        }
                        #endregion

                        if (SP_Result.Item6 != null && SP_Result.Item6.Count > 0)
                        {
                            dtHearingDetailsRemarks = (List<PTXdtHearingDetailsRemarks>)SP_Result.Item6;
                            IsValid = true;
                        }
                    }
                    else
                    {
                        if (SP_Result.Item5 != null && SP_Result.Item5.Count > 0)
                        {
                            dtHearingDetailsRemarks = (List<PTXdtHearingDetailsRemarks>)SP_Result.Item5;
                            IsValid = true;
                        }
                    }

                    #region Hearing Details Remarks Object
                    /*Hearing Details Remarks*/
                    if (IsValid)
                    {
                        hearingNoticeRemarksList = dtHearingDetailsRemarks.Select(c => new PTXdoHearingDetailsRemarks
                        {
                            HearingDetailsRemarksId = c.HearingDetailsRemarksId,
                            HearingDetails = new PTXdoHearingDetails()
                            {
                                HearingDetailsId = c.HearingDetailsId
                            },
                            Remarks = c.Remarks,
                            UpdatedBy = new PTXdoUser()
                            {
                                Userid = c.UpdatedBy,
                                Username = c.Username,
                                Firstname = c.Firstname,
                                Lastname = c.Lastname
                            },
                            UpdatedDateTime = c.UpdatedDatetime
                        }).ToList();

                        if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.QCQueue)
                        {
                            /*Assign Hearing Details Remarks*/
                            foreach (PTXdoHearingDetails hearingDetail in HearingDetails)
                            {
                                hearingDetail.HearingDetailsRemarks = hearingNoticeRemarksList.Where(a => a.HearingDetails.HearingDetailsId == hearingDetail.HearingDetailsId).ToList();
                            }
                        }
                    }
                    #endregion

                    //Sampathkumar for assigning Null when no defect Reason Code. 25-Nov-2013
                    if (objIndexedNoticeDocument.defectReasonCode != null && objIndexedNoticeDocument.defectReasonCode.DocumentDefectCodeId == 0)
                        objIndexedNoticeDocument.defectReasonCode = null;

                    if (objIndexedNoticeDocument.entryUserRole != null && objIndexedNoticeDocument.entryUserRole.UserRoleid == 0)
                        objIndexedNoticeDocument.entryUserRole = null;
                    if (objIndexedNoticeDocument.auditedUserRole != null && objIndexedNoticeDocument.auditedUserRole.UserRoleid == 0)
                        objIndexedNoticeDocument.auditedUserRole = null;
                    if (objIndexedNoticeDocument.defectAssignedUserRole != null && objIndexedNoticeDocument.defectAssignedUserRole.UserRoleid == 0)
                        objIndexedNoticeDocument.defectAssignedUserRole = null;
                }

                if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue)
                    result.indexedNoticeList = objPTXdoIndexedNoticeDocumentList;
                else
                    result.indexedNoticeList = objIndexedNoticeDocument;

                if (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue)
                    result.hearingNoticeRemarksList = hearingNoticeRemarksList;
                else
                    result.HearingDetailList = HearingDetails;
                Logger.For(this).Transaction("convertdtResultToIndexedNoticeDocumentObject-API  Ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("convertdtResultToIndexedNoticeDocumentObject-API  Error " + ex);
                throw ex;
            }
        }

        private List<PTXdoYearlyHearingDetails> getYearlyHearingDetailsById(int YearlyHearingDetailsById)
        {
            Hashtable _hashtable = new Hashtable();
            var pTXdoYearlyHearingDetails = new List<PTXdoYearlyHearingDetails>();

            try
            {
                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Reached " + ((object)YearlyHearingDetailsById).ToJson(false));
                _hashtable.Add("@YearlyHearingDetailsById", YearlyHearingDetailsById);

                var SP_Result = _Connection.Select<PTXboYearlyHearingDetailsList>(StoredProcedureNames.usp_getYearlyHearingDetailsById, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                var litigationsecondary = new List<PTXdoLitigationsecondary>();
                var hearingDetails = new List<PTXdoHearingDetails>();

                foreach (var d in SP_Result)
                {
                    litigationsecondary.Add(new PTXdoLitigationsecondary { LitigationSecID = Convert.ToInt32(d.LitigationSecID) });
                    hearingDetails.Add(new PTXdoHearingDetails { HearingDetailsId = Convert.ToInt32(d.HearingDetailsId) });
                }

                if (SP_Result.Count > 0)
                {
                    pTXdoYearlyHearingDetails.Add(new PTXdoYearlyHearingDetails
                    {
                        YearlyHearingDetailsId = SP_Result[0].YearlyHearingDetailsId,
                        Account = new PTXdoAccount { AccountId = Convert.ToInt32(SP_Result[0].AccountId) },
                        TaxYear = SP_Result[0].TaxYear,
                        ValueHearingNotice = new PTXdoValueNotice { ValueNoticeId = Convert.ToInt32(SP_Result[0].ValueNoticeId) },
                        propertyDetails = new PTXdoPropertyDetails { PropertyDetailsId = Convert.ToInt32(SP_Result[0].ProtestingDetailsId) },
                        UpdatedBy = new PTXdoUser { Userid = Convert.ToInt32(SP_Result[0].UpdatedBy) },
                        UpdatedDateTime = SP_Result[0].UpdatedDateTime,
                        DNDCodeId = new PTXdoDNDCodes { DNDCodeId = Convert.ToInt32(SP_Result[0].DNDCodeId) },
                        BPPAccountstatus = new PTXdoDocumentProcessingStatus { DocumentProcessingStatusId = Convert.ToInt32(SP_Result[0].BPPAccountstatus) },
                        ArbitrationDetails = new PTXdoArbitrationDetails { ArbitrationDetailsId = Convert.ToInt32(SP_Result[0].ArbitrationDetailsID) },
                        BindingArbitration = new PTXdoBindingArbitration { BindingArbitrationId = Convert.ToInt32(SP_Result[0].BindingArbitrationID) },
                        Litigation = new PTXdoLitigation { LitigationID = Convert.ToInt32(SP_Result[0].LitigationID) },
                        LitigationSecondaries = litigationsecondary,
                        //ProtestDeadline = SP_Result[0].ProtestDeadline,
                        HearingDetails = hearingDetails
                    });
                }

                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Ends successfully ");
                return pTXdoYearlyHearingDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Error " + ex);
                throw ex;
            }
        }

        private List<PTXdoYearlyHearingDetails> getYearlyHearingDetails(int AccountID, int TaxYear)
        {
            Hashtable _hashtable = new Hashtable();
            var pTXdoYearlyHearingDetails = new List<PTXdoYearlyHearingDetails>();

            try
            {
                if(AccountID > 0)
                {
                    Logger.For(this).Transaction("getYearlyHearingDetails-API  Reached. AccountID: " + AccountID + " || TaxYear: " + TaxYear);
                    _hashtable.Add("@AccountID", AccountID);
                    _hashtable.Add("@TaxYear", TaxYear);

                    var data = _Connection.SelectMultiple(StoredProcedureNames.usp_getYearlyHearingDetails, _hashtable,
                        Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                        gr => gr.Read<PTXdoYearlyHearingDetails>(),
                        gr => gr.Read<PTXdoDNDCodes>());

                    pTXdoYearlyHearingDetails = data.Item1.ToList();
                    pTXdoYearlyHearingDetails.FirstOrDefault().DNDCodeId = data.Item2.FirstOrDefault();

                    Logger.For(this).Transaction("getYearlyHearingDetails-API  Ends successfully ");
                }
                else
                {
                    //Added by Mani.d For Bug tfs Id - 48535
                    pTXdoYearlyHearingDetails = new List<PTXdoYearlyHearingDetails>();
                    pTXdoYearlyHearingDetails.Add(new PTXdoYearlyHearingDetails());
                }
                return pTXdoYearlyHearingDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getYearlyHearingDetails-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateHearingNoticeIndexedNoticeDocument(PTXboIndexedNoticeDocumentUpdate indexedNoticeDocumentUpdate)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateHearingNoticeIndexedNoticeDocument-API  Reached " + ((object)indexedNoticeDocumentUpdate).ToJson(false));
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
                string entryDateAndTime = "01-01-0001 00:00:00";
                if (indexedNoticeDocumentUpdate.EntryDateAndTime.ToString() != entryDateAndTime)
                {
                    _hashtable.Add("@EntryDateAndTime", (indexedNoticeDocumentUpdate.EntryDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.EntryDateAndTime : null);
                }
                else
                {
                    _hashtable.Add("@EntryDateAndTime", null);
                }

                _hashtable.Add("@DefectAssignedUserId", indexedNoticeDocumentUpdate.DefectAssignedUserId);
                _hashtable.Add("@DefectAssignedUserRoleId", indexedNoticeDocumentUpdate.DefectAssignedUserRoleId);
                _hashtable.Add("@DefectAssignedDateAndTime", (indexedNoticeDocumentUpdate.DefectAssignedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.DefectAssignedDateAndTime : null);
                _hashtable.Add("@DefectRectifiedDateAndTime", (indexedNoticeDocumentUpdate.DefectRectifiedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.DefectRectifiedDateAndTime : null);
                _hashtable.Add("@AuditedUserId", indexedNoticeDocumentUpdate.AuditedUserId);
                _hashtable.Add("@AuditedUserRoleId", indexedNoticeDocumentUpdate.AuditedUserRoleId);
                _hashtable.Add("@AuditedDateAndTime", (indexedNoticeDocumentUpdate.AuditedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.AuditedDateAndTime : null);
                _hashtable.Add("@StatusId", indexedNoticeDocumentUpdate.StatusId);
                _hashtable.Add("@ProcessingStatusId", indexedNoticeDocumentUpdate.ProcessingStatusId);
                _hashtable.Add("@OnHoldDateAndTime", (indexedNoticeDocumentUpdate.OnHoldDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.OnHoldDateAndTime : null);
                _hashtable.Add("@ScannedDateAndTime", (indexedNoticeDocumentUpdate.ScannedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.ScannedDateAndTime : null);
                _hashtable.Add("@Offset", indexedNoticeDocumentUpdate.Offset);
                _hashtable.Add("@TCK", indexedNoticeDocumentUpdate.TCK);
                _hashtable.Add("@DocumentName", indexedNoticeDocumentUpdate.DocumentName);
                _hashtable.Add("@UpdatedBy", indexedNoticeDocumentUpdate.UpdatedBy);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                _hashtable.Add("@QCAssignedUserId", indexedNoticeDocumentUpdate.QCAssignedUserId);
                _hashtable.Add("@QCAssignedUserRoleId", indexedNoticeDocumentUpdate.QCAssignedUserRoleId);
                _hashtable.Add("@QCAssignedDateAndTime", (indexedNoticeDocumentUpdate.QCAssignedDateAndTime.HasValue) ? indexedNoticeDocumentUpdate.QCAssignedDateAndTime : null);
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
                Logger.For(this).Transaction("saveOrUpdateHearingNoticeIndexedNoticeDocument-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateHearingNoticeIndexedNoticeDocument-API  Error " + ex);
                throw ex;
            }
        }

        public int saveOrUpdateUserEnteredHearingNoticeDataUpdate(PTXboUserEnteredHearingNoticeDataUpdate userEnteredHearingNoticeDataUpdate)
        {
            int retval = 0;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingNoticeDataUpdate-API  Reached " + ((object)userEnteredHearingNoticeDataUpdate).ToJson(false));
                _hashtable.Add("@TaxYear", userEnteredHearingNoticeDataUpdate.TaxYear);
                _hashtable.Add("@EvidenceDueDate", userEnteredHearingNoticeDataUpdate.EvidenceDueDate);
                _hashtable.Add("@InformalHearingDate", userEnteredHearingNoticeDataUpdate.InformalHearingDate);
                _hashtable.Add("@InfomalHearingtime", userEnteredHearingNoticeDataUpdate.InfomalHearingtime);
                _hashtable.Add("@FormalHearingDate", userEnteredHearingNoticeDataUpdate.FormalHearingDate);
                _hashtable.Add("@FormalHearingTime", userEnteredHearingNoticeDataUpdate.FormalHearingTime);
                _hashtable.Add("@PanelDocketId", userEnteredHearingNoticeDataUpdate.PanelDocketId);
                _hashtable.Add("@IFileNumber", userEnteredHearingNoticeDataUpdate.IFileNumber);
                _hashtable.Add("@NoticeDate", userEnteredHearingNoticeDataUpdate.NoticeDate);
                _hashtable.Add("@LandValue", userEnteredHearingNoticeDataUpdate.LandValue);
                _hashtable.Add("@ImprovedValue", userEnteredHearingNoticeDataUpdate.ImprovedValue);
                _hashtable.Add("@MarketValue", userEnteredHearingNoticeDataUpdate.MarketValue);
                _hashtable.Add("@TotalValue", userEnteredHearingNoticeDataUpdate.TotalValue);
                _hashtable.Add("@IsValueNoticeUpdateRequired", userEnteredHearingNoticeDataUpdate.IsValueNoticeUpdateRequired);
                _hashtable.Add("@IsValueNoticeAlreadyExist", userEnteredHearingNoticeDataUpdate.IsValueNoticeAlreadyExist);
                _hashtable.Add("@UpdatedBy", userEnteredHearingNoticeDataUpdate.UpdatedBy);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                _hashtable.Add("@InformalPanelDocketID", userEnteredHearingNoticeDataUpdate.InformalPanelDocketID);
                _hashtable.Add("@FormalPanelDocketID", userEnteredHearingNoticeDataUpdate.FormalPanelDocketID);
                _hashtable.Add("@RenoticedLandValue", userEnteredHearingNoticeDataUpdate.RenoticedLandValue);
                _hashtable.Add("@RenoticedImprovedValue", userEnteredHearingNoticeDataUpdate.RenoticedImprovedValue);
                _hashtable.Add("@RenoticedMarketValue", userEnteredHearingNoticeDataUpdate.RenoticedMarketValue);
                _hashtable.Add("@RenoticeTotalValue", userEnteredHearingNoticeDataUpdate.RenoticeTotalValue);

                SPName = StoredProcedureNames.usp_InsertUserEnteredHearingNoticeData;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (Convert.ToInt32(returnValue) > 0)
                {
                    retval = Convert.ToInt32(returnValue);
                }
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingNoticeDataUpdate-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingNoticeDataUpdate-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateYearlyHearingDetails(PTXboYearlyHearingDetailsList yearlyHearingDetailsList)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateYearlyHearingDetails-API  Reached " + ((object)yearlyHearingDetailsList).ToJson(false));
                _hashtable.Add("@YearlyHearingDetailsId", yearlyHearingDetailsList.YearlyHearingDetailsId);
                _hashtable.Add("@AccountId", yearlyHearingDetailsList.AccountId);
                _hashtable.Add("@TaxYear", yearlyHearingDetailsList.TaxYear);
                _hashtable.Add("@ValueNoticeId", yearlyHearingDetailsList.ValueNoticeId);
                _hashtable.Add("@ProtestingDetailsId", yearlyHearingDetailsList.ProtestingDetailsId);
                _hashtable.Add("@UpdatedBy", yearlyHearingDetailsList.UpdatedBy);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);
                _hashtable.Add("@DNDCodeId", yearlyHearingDetailsList.DNDCodeId);
                _hashtable.Add("@BPPAccountstatus", yearlyHearingDetailsList.BPPAccountstatus);
                _hashtable.Add("@propertyDetailsId", yearlyHearingDetailsList.propertyDetailsId);
                _hashtable.Add("@ArbitrationDetailsID", yearlyHearingDetailsList.ArbitrationDetailsID);
                _hashtable.Add("@BindingArbitrationID", yearlyHearingDetailsList.BindingArbitrationID);
                _hashtable.Add("@LitigationID", yearlyHearingDetailsList.LitigationID);
                _hashtable.Add("@ClientFinancialsId", yearlyHearingDetailsList.ClientFinancialsId);
                _hashtable.Add("@LitigationSecID", yearlyHearingDetailsList.LitigationSecID);
                //_hashtable.Add("@ProtestDeadline", yearlyHearingDetailsList.ProtestDeadline);

                SPName = StoredProcedureNames.usp_UpdateYearlyHearingDetails;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateYearlyHearingDetails-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateYearlyHearingDetails-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateUserEnteredHearingNoticeRemarks(PTXboUserEnteredHearingNoticeRemarksUpdate userEnteredHearingNoticeRemarksUpdate)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingNoticeRemarks-API  Reached " + ((object)userEnteredHearingNoticeRemarksUpdate).ToJson(false));

                _hashtable.Add("@UserEnteredHearingNoticeDataId", userEnteredHearingNoticeRemarksUpdate.UserEnteredHearingNoticeDataId);
                _hashtable.Add("@Remarks", userEnteredHearingNoticeRemarksUpdate.Remarks);
                _hashtable.Add("@UpdatedBy", userEnteredHearingNoticeRemarksUpdate.UpdatedBy);
                _hashtable.Add("@UpdatedDatetime", System.DateTime.Now);

                SPName = StoredProcedureNames.usp_InsertUserEnteredHearingNoticeRemarks;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingNoticeRemarks-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingNoticeRemarks-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateUserEnteredHearingTypeForNotice(PTXboUserEnteredHearingTypeForNoticeUpdate userEnteredHearingTypeForNoticeUpdate)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingTypeForNotice-API  Reached " + ((object)userEnteredHearingTypeForNoticeUpdate).ToJson(false));

                _hashtable.Add("@HearingTypeId", userEnteredHearingTypeForNoticeUpdate.HearingTypeId);
                _hashtable.Add("@UserEnteredHearingNoticeDataId", userEnteredHearingTypeForNoticeUpdate.UserEnteredHearingNoticeDataId);
                _hashtable.Add("@UpdatedBy", userEnteredHearingTypeForNoticeUpdate.UpdatedBy);
                _hashtable.Add("@UpdatedDateTime", System.DateTime.Now);

                SPName = StoredProcedureNames.usp_InsertUserEnteredHearingTypeForNotice;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingTypeForNotice-API  Ends successfully ");
                return retval;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateUserEnteredHearingTypeForNotice-API  Error " + ex);
                throw ex;
            }
        }

        public bool saveOrUpdateNoticeClarification(PTXboNoticeClarificationUpdate noticeClarificationUpdate)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateNoticeClarification-API  Reached " + ((object)noticeClarificationUpdate).ToJson(false));

                _hashtable.Add("@NoticeClarificationId", noticeClarificationUpdate.NoticeClarificationId);
                _hashtable.Add("@IndexedNoticeDocumentId", noticeClarificationUpdate.IndexedNoticeDocumentId);
                _hashtable.Add("@Request", noticeClarificationUpdate.Request);
                _hashtable.Add("@Response", noticeClarificationUpdate.Response);
                _hashtable.Add("@RequestedUserId", noticeClarificationUpdate.RequestedUserId);
                _hashtable.Add("@RequestedUserRoleId", noticeClarificationUpdate.RequestedUserRoleId);
                _hashtable.Add("@RequestedDateAndTime", noticeClarificationUpdate.RequestedDateAndTime);
                _hashtable.Add("@RespondedUserId", noticeClarificationUpdate.RespondedUserId);
                _hashtable.Add("@RespondedUserRoleId", noticeClarificationUpdate.RespondedUserRoleId);
                _hashtable.Add("@RespondedDateAndTime", noticeClarificationUpdate.RespondedDateAndTime);
                _hashtable.Add("@ClarificationAssignedBy", noticeClarificationUpdate.ClarificationAssignedBy);
                _hashtable.Add("@ClarificationAssignedOn", noticeClarificationUpdate.ClarificationAssignedOn);


                SPName = StoredProcedureNames.usp_InsertUpdateNoticeClarification;
                var returnValue = _Connection.ExecuteScalar(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                if (Convert.ToInt32(returnValue) == 1)
                {
                    retval = true;
                }
                Logger.For(this).Transaction("saveOrUpdateNoticeClarification-API  Ends successfully ");
                return retval;

            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("saveOrUpdateNoticeClarification-API  Error " + ex);
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
                string utcDate = "01-01-0001 00:00:00";
                if (utcDate != hearingDetailsUpdate.UTCDate.ToString())
                {
                    _hashtable.Add("@UTCDate", hearingDetailsUpdate.UTCDate);
                }
                else
                {
                    _hashtable.Add("@UTCDate", null);
                }
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

        public bool saveOrUpdateValueNotice(PTXboPriorValueNotice valueNotice)
        {
            bool retval = false;
            Hashtable _hashtable = new Hashtable();
            string SPName = string.Empty;
            try
            {
                Logger.For(this).Transaction("saveOrUpdateValueNotice-API  Reached " + ((object)valueNotice).ToJson(false));
                _hashtable.Add("@ValueNoticeId", valueNotice.ValueNoticeId);
                _hashtable.Add("@NoticedDate", valueNotice.NoticedDate);
                _hashtable.Add("@IFileNumber", valueNotice.IFileNumber);
                _hashtable.Add("@NoticeLandValue", valueNotice.NoticeLandValue);
                _hashtable.Add("@NoticeImprovedValue", valueNotice.NoticeImprovedValue);
                _hashtable.Add("@NoticeMarketValue", valueNotice.NoticeMarketValue);
                _hashtable.Add("@NoticeTotalValue", valueNotice.NoticeTotalValue);
                _hashtable.Add("@Statusid", valueNotice.Statusid);
                _hashtable.Add("@UpdatedBy", valueNotice.UpdatedBy);
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
                _hashtable.Add("@RenoticedDate", valueNotice.ReNoticedDate);

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
    }
}
