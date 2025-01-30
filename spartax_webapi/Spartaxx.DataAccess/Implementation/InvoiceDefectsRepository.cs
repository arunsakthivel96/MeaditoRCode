using Spartaxx.BusinessObjects;
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
   public class InvoiceDefectsRepository:IInvoiceDefectsRepository
    {
        private readonly DapperConnection _dapperConnection;
        private readonly InvoiceCalculationRepository _invoiceCalculation;
        public InvoiceDefectsRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
            _invoiceCalculation = new InvoiceCalculationRepository(dapperConnection);

        }

        public bool UpdateInvoiceStatusinMainTable(int invoiceID, int invoiceTypeId, bool isSpecialTerm = false)
        {
            try
            {
                bool isSuccsess = false;
                //List<int> lstHearingResultIDs = new List<int>();
                //if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                //{
                //    lstHearingResultIDs = _invoiceSpecialTermsRepository.GetInvoiceAndHearingResultMap(invoiceID).Select(h => h.ArbitrationDetailsId).Distinct().ToList();
                //    if (lstHearingResultIDs != null && lstHearingResultIDs.Count > 0)
                //    {
                //        foreach (int lstDet in lstHearingResultIDs)
                //        {
                //            PTXboArbitration arbDetails = new PTXboArbitration();
                //            arbDetails = _invoiceSpecialTermsRepository.GetArbitrationDetails(lstDet);
                //            if (isSpecialTerm == false)
                //            {
                //                arbDetails.ARBInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceSendingInProgress.GetId();
                //            }
                //            else
                //            {
                //                arbDetails.ARBInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId();
                //            }
                //            _invoiceSpecialTermsRepository.SaveOrUpdateArbitrationDetails(arbDetails);
                //        }
                //    }
                //}
                //else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
                //{
                //    lstHearingResultIDs = _invoiceSpecialTermsRepository.GetInvoiceAndHearingResultMap(invoiceID).Select(h => h.LitigationID).Distinct().ToList();
                //    if (lstHearingResultIDs != null && lstHearingResultIDs.Count > 0)
                //    {
                //        foreach (int lstDet in lstHearingResultIDs)
                //        {
                //            PTXboLitigationDetails litDetails = new PTXboLitigationDetails();
                //            litDetails = _invoiceSpecialTermsRepository.GetLitigation(lstDet);
                //            if (isSpecialTerm == false)
                //            {
                //                litDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceSendingInProgress.GetId();
                //            }
                //            else
                //            {
                //                litDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId();
                //            }

                //            _invoiceSpecialTermsRepository.SaveOrUpdateLitigation(litDetails);
                //        }
                //    }
                //}
                //else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId())
                //{
                //    lstHearingResultIDs = _invoiceSpecialTermsRepository.GetInvoiceAndHearingResultMap(invoiceID).Select(h => h.HearingResultId).Distinct().ToList();
                //    if (lstHearingResultIDs != null && lstHearingResultIDs.Count > 0)
                //    {
                //        foreach (int lstDet in lstHearingResultIDs)
                //        {
                //            PTXboHearingResult HearingDetails = new PTXboHearingResult();
                //            HearingDetails = _invoiceSpecialTermsRepository.GetHearingResultByType(lstDet, 0);
                //            if (isSpecialTerm == false)
                //            {
                //                HearingDetails.HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceSendingInProgress.GetId();
                //            }
                //            else
                //            {
                //                HearingDetails.HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId();
                //            }
                //            _invoiceSpecialTermsRepository.SaveOrUpdateHearingResult(HearingDetails);
                //        }
                //    }
                //}
                //isSuccsess = true;
                isSuccsess = _invoiceCalculation.UpdateInvoiceStatusinMainTable(invoiceID, invoiceTypeId, isSpecialTerm);
                return isSuccsess;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        public bool SubmitInvoiceGenerationDefects(List<PTXboInvoiceDetails> lstInvoiceDetails)
        {
            try
            {
                return _invoiceCalculation.SubmitInvoiceGenerationDefects(lstInvoiceDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PTXboInvoiceDetails> GetInvoiceGenerationDefects(PTXboInvoiceSearchCriteria objSearchCriteria)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceGenerationDefects-API  reached " + ((object)objSearchCriteria).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", objSearchCriteria.InvoiceId);
                parameters.Add("@AccountNumber", objSearchCriteria.AccountNumber);
                parameters.Add("@ClientNumber", objSearchCriteria.ClientNumber);
                parameters.Add("@County", objSearchCriteria.County);
                parameters.Add("@FromValue", objSearchCriteria.FromValue);
                parameters.Add("@HearingAgent", objSearchCriteria.HearingAgent);
                parameters.Add("@HearingType", objSearchCriteria.HearingType);
                parameters.Add("@Project", objSearchCriteria.Project);
                parameters.Add("@PropertyType", objSearchCriteria.PropertyType);
                parameters.Add("@SelectedValueType", objSearchCriteria.SelectedValueType);
                parameters.Add("@TaxYear", objSearchCriteria.TaxYear);
                parameters.Add("@ToValue", objSearchCriteria.TotalValue);
                parameters.Add("@IsQ", objSearchCriteria.IsQ);
                parameters.Add("@InvoiceProcessingStatusId", objSearchCriteria.InvoiceProcessingStatusId);

                List<PTXboInvoiceDetails> lstInvoiceDetails = new List<PTXboInvoiceDetails>();
                if (objSearchCriteria.InvoiceType == Enumerators.PTXenumInvoiceType.Litigation.GetId())
                {
                    //lstInvoiceDetails = _dapperConnection.Select<PTXboInvoiceDetails>(StoredProcedureNames.usp_getInvoiceDetailsForDefects_Litigation, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                    lstInvoiceDetails = _dapperConnection.Select<PTXboInvoiceDetails>(StoredProcedureNames.usp_invoicedefectqueue_litigation, parameters,Enumerator.Enum_CommandType.StoredProcedure,Enumerator.Enum_ConnectionString.Spartaxx ).ToList();
                }
                else if (objSearchCriteria.InvoiceType == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                {
                    lstInvoiceDetails = _dapperConnection.Select<PTXboInvoiceDetails>(StoredProcedureNames.usp_invoicedefectqueue_arbitration, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                    
                }
                else if (objSearchCriteria.InvoiceType == Enumerators.PTXenumInvoiceType.BPP.GetId())
                {
                    lstInvoiceDetails = _dapperConnection.Select<PTXboInvoiceDetails>(StoredProcedureNames.usp_getInvoiceDetailsForDefects_BppRendition, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                    

                }
                else if (objSearchCriteria.InvoiceType == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                {
                    lstInvoiceDetails = _dapperConnection.Select<PTXboInvoiceDetails>(StoredProcedureNames.usp_getInvoiceDetailsForDefects_TaxBill, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                    

                }
                else
                {
                    lstInvoiceDetails = _dapperConnection.Select<PTXboInvoiceDetails>(StoredProcedureNames.usp_getInvoiceDetailsForDefects_Standard, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                    
                }
                return lstInvoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceGenerationDefects-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public List<PTXboAccountDetails> GetInvoiceDefectsAccountDetails(int invoiceId)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceDefectsAccountDetails-API  reached " + ((object)invoiceId).ToJson(false));
                Hashtable parameters = new Hashtable();
               var invoice = _invoiceCalculation.GetInvoiceDetailsById(invoiceId).FirstOrDefault();
               parameters.Add("@InvoiceID", invoiceId);
               parameters.Add("@InvoiceTypeId", invoice.InvoiceTypeId);
               var result = _dapperConnection.Select<PTXboAccountDetails>(StoredProcedureNames.usp_getInvoiceDefectAccountDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
               return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceDefectsAccountDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public List<PTXboAccountJurisdiction> GetInvoiceDefectsAccountJurisdictionDetails(PTXboInvoiceDefectJurisdictionInput invoiceDefectJurisdiction)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceDefectsAccountJurisdictionDetails-API  reached " + ((object)invoiceDefectJurisdiction).ToJson(false));
                Hashtable parameters = new Hashtable();
                var invoice = _invoiceCalculation.GetInvoiceDetailsById(invoiceDefectJurisdiction.InvoiceId).FirstOrDefault();
                parameters.Add("@AccountId", invoiceDefectJurisdiction.AccountId);
                parameters.Add("@InvoiceTypeId", invoice.InvoiceTypeId);
                parameters.Add("@Taxyear", invoiceDefectJurisdiction.Taxyear);
                var result = _dapperConnection.Select<PTXboAccountJurisdiction>(StoredProcedureNames.usp_getInvoiceDefectAccountJurisdictionDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceDefectsAccountJurisdictionDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }



        public PTXboTermsRemark GetTermsDetails(int groupId,int termstypeid)
        {
            try
            {
                Logger.For(this).Invoice("GetTermsDetails-API  reached " + ((object)groupId).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@GroupId", groupId);
                parameters.Add("@TermstypeId", termstypeid);
                var result = _dapperConnection.Select<PTXboTermsRemark>(StoredProcedureNames.usp_get_ptaxterms, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetTermsDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public PTXboGroup GetGroupDetails(int groupId)
        {
            try
            {
                Logger.For(this).Invoice("GetGroupDetails-API  reached " + ((object)groupId).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@GroupId", groupId);
                var result = _dapperConnection.Select<PTXboGroup>(StoredProcedureNames.usp_get_ptaxgroup, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetGroupDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public PTXboInvoiceDefectDetails GetInvoiceDefectsTermDetails(int invoiceId)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceDefectsTermDetails-API  reached .invoiceId" + ((object)invoiceId).ToJson(false));
                Hashtable parameters = new Hashtable();
                PTXboInvoiceDefectDetails invoiceDefectDetails = new PTXboInvoiceDefectDetails();
                var invoice = _invoiceCalculation.GetInvoiceDetailsById(invoiceId).FirstOrDefault();
                int InvoiceTypeid = 0;
                int GroupId = 0;
                if (invoice != null)
                {
                    InvoiceTypeid = invoice.InvoiceTypeId;// invoice.TermsTypeID;
                    GroupId = invoice.GroupId;
                }
                if (GroupId != 0)
                {
                    invoiceDefectDetails.GroupName = GetGroupDetails(GroupId).groupName;
                    var objTerms = GetTermsDetails(GroupId, InvoiceTypeid); 
                    if (objTerms != null)
                    {
                        invoiceDefectDetails.ContingencyPercent = (objTerms.Contingency* 100);
                        invoiceDefectDetails.FlatFee = objTerms.FlatFee;
                        invoiceDefectDetails.TermsRemarks = objTerms.remarks;
                    }

                }
                return invoiceDefectDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceDefectsTermDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

        public int SaveOrUpdateTerms(PTXboTerms terms)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateCorrQ-API  reached " + ((object)terms).ToJson(false));
                parameters.Add("@TermsId", terms.termsID);
                parameters.Add("@GroupId", terms.groupID);
                parameters.Add("@TermsTypeId", terms.termsTypeID);
                parameters.Add("@EffectiveDate", terms.EffectiveDate);
                parameters.Add("@ExpiryDate", terms.ExpiryDate);
                parameters.Add("@Remarks", terms.remarks);
                parameters.Add("@UpdatedBy", terms.UpdatedByID > 0 ? terms.UpdatedByID : 0);
                parameters.Add("@UpdatedDateTime", terms.UpdatedDateTime);
                parameters.Add("@Contingency", terms.Contingency);
                parameters.Add("@FlatFee", terms.FlatFee);
                parameters.Add("@ContingencyAfterExpiry", terms.ContingencyAfterExpiry);
                parameters.Add("@FlatFeeAfterExpiry", terms.FlatFeeAfterExpiry);
                parameters.Add("@ExpiryRemarks", terms.ExpiryRemarks);
                parameters.Add("@TermExpiryActionId", terms.TermExpiryActionId);
                parameters.Add("@InvoiceGroupingTypeId", terms.InvoiceGroupingTypeId);
                parameters.Add("@IsSpecializedTerm", terms.IsSpecializedTerm);
                parameters.Add("@InvoiceFrequencyId", terms.InvoiceFrequencyID);
                parameters.Add("@FrequencyDate", terms.FrequencyDate);
                parameters.Add("@CapValue", terms.CapValue);
                parameters.Add("@InvoiceQueueDate", terms.InvoiceQueueDate);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateTerms, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                
                Logger.For(this).Invoice("SaveOrUpdateCorrQ-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateCorrQ-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }


        public bool UpdateInvoiceDefectsTermsDetails(PTXboInvoiceDefectDetails invoiceDefectDetails)
        {
            try
            {
                Logger.For(this).Invoice("UpdateInvoiceDefectsTermsDetails-API  reached .invoiceDefectDetails" + ((object)invoiceDefectDetails).ToJson(false));
                bool isSuccess = false;
                Hashtable parameters = new Hashtable();
                var invoice = _invoiceCalculation.GetInvoiceDetailsById(invoiceDefectDetails.InvoiceId).FirstOrDefault();

                if (invoiceDefectDetails.GroupId != 0)
                {
                    invoiceDefectDetails.GroupName = GetGroupDetails(invoiceDefectDetails.GroupId).groupName;
                    var objTerms = GetTermsDetails(invoiceDefectDetails.GroupId, invoice.TermsTypeID);
                    if (objTerms != null)
                    {
                       // objTerms.ContingencyPercent = invoiceDefectDetails.ContingencyPercent;
                        //objTerms.FlatFee =Convert.ToDecimal(invoiceDefectDetails.FlatFee);
                        //objTerms.TermsRemarks = invoiceDefectDetails.TermsRemarks;
                        //update terms
                        PTXboTerms terms = new PTXboTerms()
                        {
                            Contingency = invoiceDefectDetails.ContingencyPercent,
                            FlatFee= Convert.ToDecimal(invoiceDefectDetails.FlatFee),
                            remarks= invoiceDefectDetails.TermsRemarks,
                            //Added by SaravananS.
                            UpdatedByID = invoiceDefectDetails.UpdatedBy,
                            termsID=objTerms.TermsId,
                            groupID=objTerms.GroupId,
                            termsTypeID = objTerms.TermsTypeId,
                            EffectiveDate=objTerms.EffectiveDate,
                            ExpiryDate=objTerms.ExpiryDate,
                            ExpiryRemarks=objTerms.ExpiryRemarks,
                            ContingencyAfterExpiry =objTerms.ContingencyAfterExpiry,
                            FlatFeeAfterExpiry=objTerms.FlatFeeAfterExpiry,
                            IsSpecializedTerm=objTerms.IsSpecializedTerm,
                            CapValue=objTerms.CapValue,
                            InvoiceGroupingTypeId=objTerms.InvoiceGroupingTypeId
                            //Ends here.
                        };
                        SaveOrUpdateTerms(terms);
                        isSuccess = true;
                    }

                }
                else
                {
                    isSuccess = false;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateInvoiceDefectsTermsDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
        }

    }
}
