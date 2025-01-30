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
   public class InvoiceAdjustmentAuditRepository : IInvoiceAdjustmentAuditRepository
    {
        private readonly DapperConnection _dapperConnection;
        private readonly InvoiceCalculationRepository _invoiceCalculation;

        public InvoiceAdjustmentAuditRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
            _invoiceCalculation = new InvoiceCalculationRepository(dapperConnection);
        }

        public List<PTXboInvoiceAdjustmentAllotted> GetAllottedInvoiceAdjustmentRequest(PTXboInvoiceAdjustmentAllotedSearch objSearch)
        {
            
            try
            {
                Logger.For(this).Invoice("GetAllottedInvoiceAdjustmentRequest-API  reached " + ((object)objSearch).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@UserID", objSearch.UserID);
                parameters.Add("@UserRoleID", objSearch.UserRoleID);
                parameters.Add("@QueueTypeID", objSearch.AssignedQueue.GetId());
                parameters.Add("@CleintNumber", objSearch.ClientNumber);
                parameters.Add("@InvoiceID", objSearch.InvoiceId);
                parameters.Add("@InvoiceTypeID", objSearch.InvoiceType);
                List<PTXboInvoiceAdjustmentAllotted> objInvAlloted = new List<PTXboInvoiceAdjustmentAllotted>();
                objInvAlloted = _dapperConnection.Select<PTXboInvoiceAdjustmentAllotted>(StoredProcedureNames.usp_GetAllottedInvoiceAdjustmentRequest, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetAllottedInvoiceAdjustmentRequest-API  ends successfully ");
                return objInvAlloted;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetAllottedInvoiceAdjustmentRequest-API  error " + ex);
                throw ex;
            }
        }


        public bool GetInvoiceNextAdjustmentAllottedDocument(int userID, int userRoleID, int queID)
        {

            try
            {
                Logger.For(this).Invoice("GetInvoiceNextAdjustmentAllottedDocument-API  reached " + ((object)"userID="+userID.ToString()+ "userRoleID="+userRoleID.ToString()+ "queID="+ queID.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@UserID", userID);
                parameters.Add("@UserRoleID", userRoleID);
                parameters.Add("@QueueTypeID", queID);
                
                var result= _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_getInvoiceNextAdjustmentAllottedDocument, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("GetInvoiceNextAdjustmentAllottedDocument-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceNextAdjustmentAllottedDocument-API  error " + ex);
                throw ex;
            }
        }

        public PTXboInvoiceAndClientDetails GetClientAndInvoiceDetails(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetClientAndInvoiceDetails-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);

                var result = _dapperConnection.Select<PTXboInvoiceAndClientDetails>(StoredProcedureNames.usp_getClientAndInvoiceDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetClientAndInvoiceDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetClientAndInvoiceDetails-API  error " + ex);
                throw ex;
            }
        }

        public PTXboInvoiceAdjustmentRequest GetInvoiceAdjustmentRequestDetails(int invoiceAdjusmentRequestID,int requestType)
        {
            
            try
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentRequestDetails-API  reached " + ((object)invoiceAdjusmentRequestID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceAdjusmentRequestID", invoiceAdjusmentRequestID);
                parameters.Add("@RequestType", requestType);
                var result = _dapperConnection.Select<PTXboInvoiceAdjustmentRequest>(StoredProcedureNames.usp_get_InvoiceAdjustmentRequest, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetInvoiceAdjustmentRequestDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentRequestDetails-API  error " + ex);
                throw ex;
            }

        }

        public PTXboPendingAdjustmentRequestDetails GetPendingAdjustmentRequestDetails(int invoiceID)
        {
            PTXboPendingAdjustmentRequestDetails pendingAdjustmentRequestDetails = new PTXboPendingAdjustmentRequestDetails();
            try
            {
                Logger.For(this).Invoice("GetPendingAdjustmentRequestDetails-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);

                var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceAdjustmentPaymentsOrAdjustments, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                              gr => gr.Read<PTXboPendingInvoiceAdjustment>(),
                              gr => gr.Read<PTXboInvoiceRemarks>());
                pendingAdjustmentRequestDetails.PendingInvoiceAdjustment = result.Item1.Count() > 0 ? (List<PTXboPendingInvoiceAdjustment>)result.Item1 : new List<PTXboPendingInvoiceAdjustment>();
                pendingAdjustmentRequestDetails.InvoiceRemarks = result.Item2.Count() > 0 ? (List<PTXboInvoiceRemarks>)result.Item2 : new List<PTXboInvoiceRemarks>();
                Logger.For(this).Invoice("GetPendingAdjustmentRequestDetails-API  ends successfully ");
                return pendingAdjustmentRequestDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetPendingAdjustmentRequestDetails-API  error " + ex);
                throw ex;
            }

        }

        public PTXboAutoAdjustmentInvoiceDetails GetAutoAdjustmentInvoiceDetails(int invoiceID, int invoiceAdjustmentRequestID)
        {
            //PTXboAutoAdjustmentInvoiceDetails autoAdjustmentInvoiceDetails = new PTXboAutoAdjustmentInvoiceDetails();
            try
            {
                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  reached " + ((object)"invoiceID="+ invoiceID.ToString()+ "invoiceAdjustmentRequestID="+invoiceAdjustmentRequestID.ToString()).ToJson(false));
                //Hashtable parameters = new Hashtable();
                //parameters.Add("@InvoiceID", invoiceID);
                //parameters.Add("@InvoiceAdjustmentRequestID", invoiceAdjustmentRequestID);

                //var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getAutoAdjustmentInvoiceDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                //              gr => gr.Read<PTXboInvoiceAdjustment>(),
                //              gr => gr.Read<PTXboInvoice>(),
                //              gr=>gr.Read<PTXboInvoiceAccount>(),
                //              gr=>gr.Read<PTXboInvoiceAdjustmentClarifications>()
                //              //,gr=>gr.Read<PTXboExemptionJurisdictions>()
                //              );
                //autoAdjustmentInvoiceDetails.Adjustment = result.Item1.Count() > 0 ? result.Item1.FirstOrDefault() : new PTXboInvoiceAdjustment();
                //autoAdjustmentInvoiceDetails.Invoice = result.Item2.Count() > 0 ? result.Item2.FirstOrDefault() : new PTXboInvoice();
                //autoAdjustmentInvoiceDetails.InvoiceLienItem = result.Item3.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item3 : new List<PTXboInvoiceAccount>();
                //autoAdjustmentInvoiceDetails.InvoiceAdjustmentClarification = result.Item4.Count() > 0 ? (List<PTXboInvoiceAdjustmentClarifications>)result.Item4 : new List<PTXboInvoiceAdjustmentClarifications>();
                //// autoAdjustmentInvoiceDetails.Invoice.ExemptionJurisdicitonlst = result.Item5.Count() > 0 ?(List<PTXboExemptionJurisdictions >)result.Item5 : new List<PTXboExemptionJurisdictions>();
                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  ends successfully ");
                return _invoiceCalculation.GetAutoAdjustmentInvoiceDetails(invoiceID,invoiceAdjustmentRequestID);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  error " + ex);
                throw ex;
            }
        }

        public List<PTXboCollectionRemark> GetCSInvoiceComments(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetCSInvoiceComments-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);

                var result = _dapperConnection.Select<PTXboCollectionRemark>(StoredProcedureNames.usp_getCollectionRemarks, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetCSInvoiceComments-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCSInvoiceComments-API  error " + ex);
                throw ex;
            }
        }

        public List<PTXboExemptionJurisdictions> GetExemptionRemovedJurisdictions(int invoiceID, int taxYear, int invoiceAdjustmentRequestID, int propertyDetails)
        {
            try
            {
                Logger.For(this).Invoice("GetExemptionRemovedJurisdictions-API  reached " + ((object)"invoiceID="+invoiceID.ToString()+ "taxYear="+taxYear.ToString()+ "invoiceAdjustmentRequestID"+ invoiceAdjustmentRequestID.ToString()+ "propertyDetails="+propertyDetails.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceId", invoiceID);
                parameters.Add("@TaxYear", taxYear);
                parameters.Add("@InvoiceAdjustmentRequestId", invoiceAdjustmentRequestID);
                parameters.Add("@PropertyDetailsID", propertyDetails);
                var result = _dapperConnection.Select<PTXboExemptionJurisdictions>(StoredProcedureNames.usp_getExemptionRemovedJurisdiction, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetExemptionRemovedJurisdictions-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetExemptionRemovedJurisdictions-API  error " + ex);
                throw ex;
            }
        }


        public PTXboInvoiceAdjustmentLineItem GetInvoiceAdjustmentLineItem(int invoiceAdjustmentId,int accountID)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentLineItem-API  reached " + ((object)"invoiceAdjustmentId="+invoiceAdjustmentId.ToString()+ "accountID="+accountID.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceAdjustmentId", invoiceAdjustmentId);
                parameters.Add("@AccountID", accountID);
                var result = _dapperConnection.Select<PTXboInvoiceAdjustmentLineItem>(StoredProcedureNames.usp_get_InvoiceAdjustmentLineItem, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetInvoiceAdjustmentLineItem-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentLineItem-API  error " + ex);
                throw ex;
            }
        }

        public int SaveOrUpdateInvoiceAdjustmentLineItem(PTXboInvoiceAdjustmentLineItem invoiceAdjustmentLineItem)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentLineItem-API  reached " + ((object)invoiceAdjustmentLineItem).ToJson(false));
                parameters.Add("@InvoiceAdjustmentLineItemID", invoiceAdjustmentLineItem.InvoiceAdjustmentLineItemID);

                parameters.Add("@InvoiceAdjustmentId", invoiceAdjustmentLineItem.InvoiceAdjustmentId);

                parameters.Add("@AccountID", invoiceAdjustmentLineItem.AccountID);

                parameters.Add("@InitialAssessedValue", invoiceAdjustmentLineItem.InitialAssessedValue);

                parameters.Add("@FinalAssessedValue", invoiceAdjustmentLineItem.FinalAssessedValue);

                parameters.Add("@PriorYearTaxRate", invoiceAdjustmentLineItem.PriorYearTaxRate);

                parameters.Add("@EstimatedTaxSavings", invoiceAdjustmentLineItem.EstimatedTaxSavings);

                parameters.Add("@Reduciton", invoiceAdjustmentLineItem.Reduciton);

                parameters.Add("@Homestead", invoiceAdjustmentLineItem.Homestead);

                parameters.Add("@Over65", invoiceAdjustmentLineItem.Over65);

                parameters.Add("@Disability", invoiceAdjustmentLineItem.Disability);

                parameters.Add("@VetDisability", invoiceAdjustmentLineItem.VetDisability);

                parameters.Add("@VetDisabilityRatingId", invoiceAdjustmentLineItem.VetDisabilityRatingId);

                parameters.Add("@ExemptionCalculationWithPriorTaxYearTaxRate", invoiceAdjustmentLineItem.ExemptionCalculationWithPriorTaxYearTaxRate);

                parameters.Add("@ExemptionCalculationWithCurrentTaxYearTaxRate", invoiceAdjustmentLineItem.ExemptionCalculationWithCurrentTaxYearTaxRate);


              var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceAdjustmentLineItem, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentLineItem-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentLineItem-API  error " + ex);
                throw ex;
            }
        }

        public int SaveOrUpdateInvoiceAdjustmentManualLineItem(PTXboInvoiceAdjustmentManualLineItem invoiceAdjustmentManualLineItem)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentManualLineItem-API  reached " + ((object)invoiceAdjustmentManualLineItem).ToJson(false));
                parameters.Add("@InvoiceAdjustmentManualLineItemID", invoiceAdjustmentManualLineItem.InvoiceAdjustmentManualLineItemID);
                parameters.Add("@InvoiceAdjustmentId", invoiceAdjustmentManualLineItem.InvoiceAdjustmentId);
                parameters.Add("@InvoiceAdjustmentManualTypeId", invoiceAdjustmentManualLineItem.InvoiceAdjustmentManualTypeId);
                parameters.Add("@PrincipalInvoiceAmount", invoiceAdjustmentManualLineItem.PrincipalInvoiceAmount);
                parameters.Add("@InterestInvoiceAmount", invoiceAdjustmentManualLineItem.InterestInvoiceAmount);
                parameters.Add("@ManualAdjustmentReason", invoiceAdjustmentManualLineItem.ManualAdjustmentReason);
                parameters.Add("@PaymentMethodId", (invoiceAdjustmentManualLineItem.PaymentMethodId==0)?2: invoiceAdjustmentManualLineItem.PaymentMethodId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateinvoiceAdjustmentManualLineItem, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentManualLineItem-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentManualLineItem-API  error " + ex);
                throw ex;
            }
        }

        public PTXboInvoiceAdjustmentManualLineItem GetInvoiceAdjustmentManualLineItem(int invoiceAdjustmentManualLineItemID)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentManualLineItem-API  reached " + ((object)invoiceAdjustmentManualLineItemID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceAdjustmentManualLineItemID", invoiceAdjustmentManualLineItemID);
                
                var result = _dapperConnection.Select<PTXboInvoiceAdjustmentManualLineItem>(StoredProcedureNames.usp_get_InvoiceAdjustmentManualLineItem, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetInvoiceAdjustmentManualLineItem-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentManualLineItem-API  error " + ex);
                throw ex;
            }
        }


        public PTXboInvoiceExemptionJurisdiction GetInvoiceExemptionJurisdiction(int invoiceAdjustmentId ,int jurisdictionId)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceExemptionJurisdiction-API  reached " + ((object)"invoiceAdjustmentId="+invoiceAdjustmentId.ToString()+ "jurisdictionId="+jurisdictionId.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceAdjustmentId", invoiceAdjustmentId);
                parameters.Add("@JurisdictionId", jurisdictionId);
                var result = _dapperConnection.Select<PTXboInvoiceExemptionJurisdiction>(StoredProcedureNames.usp_get_InvoiceExemptionJurisdiction, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetInvoiceExemptionJurisdiction-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceExemptionJurisdiction-API  error " + ex);
                throw ex;
            }
        }


        public bool SaveOrUpdateInvoiceExemptionJurisdiction(PTXboInvoiceExemptionJurisdiction ObjAdjLineItemFromDB)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceExemptionJurisdiction-API  reached " + ((object)ObjAdjLineItemFromDB).ToJson(false));
                parameters.Add("@InvoiceExemptionJurisditctionID", ObjAdjLineItemFromDB.InvoiceExemptionJurisditctionID);
                parameters.Add("@InvoiceAdjustmentId", ObjAdjLineItemFromDB.InvoiceAdjustmentId);
                parameters.Add("@InvoiceLineItemID", ObjAdjLineItemFromDB.InvoiceLineItemID);
                parameters.Add("@AccountID", ObjAdjLineItemFromDB.AccountID);
                parameters.Add("@JurisdictionID", ObjAdjLineItemFromDB.JurisdictionID);
                parameters.Add("@TaxYear", ObjAdjLineItemFromDB.TaxYear);
                parameters.Add("@TaxRate", ObjAdjLineItemFromDB.TaxRate);
                parameters.Add("@ExemptionPercent", ObjAdjLineItemFromDB.ExemptionPercent);
                parameters.Add("@ExemptionFees", ObjAdjLineItemFromDB.ExemptionFees);
                parameters.Add("@ExemptionAmount", ObjAdjLineItemFromDB.ExemptionAmount);
                parameters.Add("@TaxableAmtForIntitalValue", ObjAdjLineItemFromDB.TaxableAmtForReducedValue);
                parameters.Add("@EstTaxForInitialValue", ObjAdjLineItemFromDB.EstTaxForInitialValue);
                parameters.Add("@TaxableAmtForReducedValue", ObjAdjLineItemFromDB.TaxableAmtForReducedValue);
                parameters.Add("@EstTaxForFinalValue", ObjAdjLineItemFromDB.EstTaxForFinalValue);
                parameters.Add("@IsAdded", ObjAdjLineItemFromDB.IsAdded);
                parameters.Add("@IsRemoved", ObjAdjLineItemFromDB.IsRemoved);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceExemptionJurisdiction, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveOrUpdateInvoiceExemptionJurisdiction-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceExemptionJurisdiction-API  error " + ex);
                throw ex;
            }
        }

        public bool SaveOrUpdateInvoiceExemptionJurisdictionhistory(PTXboInvoiceExemptionJurisdictionHistory ObjAdjLineItemFromDB)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceExemptionJurisdiction-API  reached " + ((object)ObjAdjLineItemFromDB).ToJson(false));              
                parameters.Add("@InvoiceAdjustmentId", ObjAdjLineItemFromDB.InvoiceAdjustmentId);
                parameters.Add("@InvoiceLineItemID", ObjAdjLineItemFromDB.InvoiceLineItemID);
                parameters.Add("@AccountID", ObjAdjLineItemFromDB.AccountID);
                parameters.Add("@JurisdictionID", ObjAdjLineItemFromDB.JurisdictionId);
                parameters.Add("@TaxYear", ObjAdjLineItemFromDB.TaxYear);
                parameters.Add("@TaxRate", ObjAdjLineItemFromDB.TaxRate);
                parameters.Add("@ExemptionPercent", ObjAdjLineItemFromDB.ExemptionPercent);
                parameters.Add("@ExemptionFees", ObjAdjLineItemFromDB.ExemptionFees);
                parameters.Add("@ExemptionAmount", ObjAdjLineItemFromDB.ExemptionAmount);
                parameters.Add("@TaxableAmtForIntitalValue", ObjAdjLineItemFromDB.TaxableAmtForReducedValue);
                parameters.Add("@EstTaxForInitialValue", ObjAdjLineItemFromDB.EstTaxForInitialValue);
                parameters.Add("@TaxableAmtForReducedValue", ObjAdjLineItemFromDB.TaxableAmtForReducedValue);
                parameters.Add("@EstTaxForFinalValue", ObjAdjLineItemFromDB.EstTaxForFinalValue);
                parameters.Add("@IsAdded", ObjAdjLineItemFromDB.IsAdded);
                parameters.Add("@IsRemoved", ObjAdjLineItemFromDB.IsRemoved);
                parameters.Add("@RequestType", ObjAdjLineItemFromDB.RequestType);
                parameters.Add("@PrincipleAdjustmentAmount", ObjAdjLineItemFromDB.PrincipalAdjustment);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_InsertUpdateExemptionJurisdictionHistory, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveOrUpdateInvoiceExemptionJurisdiction-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceExemptionJurisdiction-API  error " + ex);
                throw ex;
            }
        }

        public int SaveOrUpdateInvoiceAdjustment(PTXboInvoiceAdjustment invoiceadjustment)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustment-API  reached " + ((object)invoiceadjustment).ToJson(false));
                parameters.Add("@InvoiceAdjustmentId", invoiceadjustment.InvoiceAdjustmentId);
                parameters.Add("@InvoiceAdjustmentRequestTypeID", invoiceadjustment.InvoiceAdjustmentRequestTypeID);
                parameters.Add("@InitialAssessedValue", invoiceadjustment.InitialAssessedValue);
                parameters.Add("@FinalAssessedValue", invoiceadjustment.FinalAssessedValue);
                parameters.Add("@PriorYearTaxRate", invoiceadjustment.PriorYearTaxRate);
                parameters.Add("@ContingencyPercentage", invoiceadjustment.ContingencyPercentage);
                parameters.Add("@ContingencyFee", invoiceadjustment.ContingencyFee);
                parameters.Add("@FlatFee", invoiceadjustment.FlatFee);
                parameters.Add("@InvoiceAmount", invoiceadjustment.InvoiceAmount);
                parameters.Add("@CompoundInterest", invoiceadjustment.CompoundInterest);
                parameters.Add("@AmountPaid", invoiceadjustment.AmountPaid);
                parameters.Add("@InterestAmount", invoiceadjustment.InterestAmount);
                parameters.Add("@PrincipleAdjustmentAmount", invoiceadjustment.PrincipleAdjustmentAmount);
                parameters.Add("@InterestAdjustmentAmount", invoiceadjustment.InterestAdjustmentAmount);
                parameters.Add("@amountDue", invoiceadjustment.AmountDue);
                parameters.Add("@CalculateWithAllPendingAdjustment", invoiceadjustment.CalculateWithAllPendingAdjustment);
                parameters.Add("@RecalculateUsingCurrentTaxYearTaxRates", invoiceadjustment.RecalculateUsingCurrentTaxYearTaxRates);
                parameters.Add("@RecalculateUsingOtherContingencyPercentage", invoiceadjustment.RecalculateUsingOtherContingencyPercentage);
                parameters.Add("@RecalculateInterest", invoiceadjustment.RecalculateInterest);
                parameters.Add("@InvoiceAdjustmentRequestID", invoiceadjustment.InvoiceAdjustmentRequestID);
                parameters.Add("@InterestRateID", invoiceadjustment.InterestRateID);
                parameters.Add("@InterestPaid", invoiceadjustment.InterestPaid);
                parameters.Add("@WaiveSimpleInterest", invoiceadjustment.WaiveSimpleInterest);
                parameters.Add("@WaiveCompoundInterest", invoiceadjustment.WaiveCompoundInterest); 

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceAdjustment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustment-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustment-API  error " + ex);
                throw ex;
            }
        }



        public int SaveOrUpdateInvoiceAdjustmentAttachments(PTXboInvoiceAdjustmentAttachments objInvoiceAttachments)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentAttachments-API  reached " + ((object)objInvoiceAttachments).ToJson(false));
                parameters.Add("@AttachDocumentID", objInvoiceAttachments.AttachDocumentID);
                parameters.Add("@ClientDocumentTypeId", objInvoiceAttachments.ClientDocumentTypeId);
                parameters.Add("@DocumentFileName", objInvoiceAttachments.DocumentFileName);
                parameters.Add("@PWImageID", objInvoiceAttachments.PWImageID);
                parameters.Add("@UpdatedBy", objInvoiceAttachments.UpdatedBy);
                parameters.Add("@UpdatedDateTime", objInvoiceAttachments.UpdatedDateTime);
                parameters.Add("@InvoiceAdjustmentRequestID", objInvoiceAttachments.InvoiceAdjustmentRequestID);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceAdjustmentAttachments, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentAttachments-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentAttachments-API  error " + ex);
                throw ex;

            }
        }

        
        public int SaveOrUpdateInvoiceAdjustmentRequest(PTXboInvoiceAdjustmentRequest objInvoiceAdjustmentRequest)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentRequest-API  reached " + ((object)objInvoiceAdjustmentRequest).ToJson(false));
                parameters.Add("@InvoiceAdjustmentRequestID", objInvoiceAdjustmentRequest.InvoiceAdjustmentRequestID);
                parameters.Add("@InvoiceID", objInvoiceAdjustmentRequest.InvoiceID);
                parameters.Add("@PrincipleAdjustmentAmount", objInvoiceAdjustmentRequest.PrincipleAdjustmentAmount);
                parameters.Add("@InterestAdjustmentAmount", objInvoiceAdjustmentRequest.InterestAdjustmentAmount);
                parameters.Add("@UrgentRequest", objInvoiceAdjustmentRequest.UrgentRequest);
                parameters.Add("@EnableSendNewInvoice", objInvoiceAdjustmentRequest.EnableSendNewInvoice);
                parameters.Add("@EnableDefaultDeliveryMethod", objInvoiceAdjustmentRequest.EnableDefaultDeliveryMethod);
                parameters.Add("@EnableAlternateDeliveryMethod", objInvoiceAdjustmentRequest.EnableAlternateDeliveryMethod);
                parameters.Add("@EnableCustomDeliveryMethod", objInvoiceAdjustmentRequest.EnableCustomDeliveryMethod);
                parameters.Add("@EnableIncludeCopyOfCalculationGrid", objInvoiceAdjustmentRequest.EnableIncludeCopyOfCalculationGrid);
                parameters.Add("@CustomDeliveryMethodId", objInvoiceAdjustmentRequest.CustomDeliveryMethodId);
                parameters.Add("@DeliveryStatusId", objInvoiceAdjustmentRequest.DeliveryStatusId);
                parameters.Add("@InvoiceAdjustmentStatusId", objInvoiceAdjustmentRequest.InvoiceAdjustmentStatusId);
                parameters.Add("@RequestedUserId", objInvoiceAdjustmentRequest.RequestedUserId);
                parameters.Add("@RequestedUserRoleID", objInvoiceAdjustmentRequest.RequestedUserRoleID);
                parameters.Add("@RequestedDateTime", objInvoiceAdjustmentRequest.RequestedDateTime);
                parameters.Add("@AuditUserid", objInvoiceAdjustmentRequest.AuditUserid);
                parameters.Add("@AuditUserRoleId", objInvoiceAdjustmentRequest.AuditUserRoleId);
                parameters.Add("@AuditedDateandTime", objInvoiceAdjustmentRequest.AuditedDateandTime);
                parameters.Add("@DefectUserid", objInvoiceAdjustmentRequest.DefectUserid);
                parameters.Add("@DefectUserRoleID", objInvoiceAdjustmentRequest.DefectUserRoleID);
                parameters.Add("@DefectResolvedDateTime", objInvoiceAdjustmentRequest.DefectResolvedDateTime);
                parameters.Add("@InvoiceQueueTypeID", objInvoiceAdjustmentRequest.InvoiceQueueTypeID);
                parameters.Add("@CancellationNotes", objInvoiceAdjustmentRequest.CancellationNotes);
                parameters.Add("@CancelledUserID", objInvoiceAdjustmentRequest.CancelledUserID);
                parameters.Add("@CancelledUserRoleID", objInvoiceAdjustmentRequest.CancelledUserRoleID);
                parameters.Add("@CancelledDateTime", objInvoiceAdjustmentRequest.CancelledDateTime);
                parameters.Add("@UpdatedBy", objInvoiceAdjustmentRequest.UpdatedBy);
                parameters.Add("@UpdatedDateTime", objInvoiceAdjustmentRequest.UpdatedDateTime);
                parameters.Add("@CustomDeliveryMethodAddress", objInvoiceAdjustmentRequest.CustomDeliveryMethodAddress);
                parameters.Add("@RequestRefundAmount", objInvoiceAdjustmentRequest.RequestRefundAmount);
                parameters.Add("@RequestRefund", objInvoiceAdjustmentRequest.RequestRefund);
                parameters.Add("@AuditAssignedBy", objInvoiceAdjustmentRequest.AuditAssignedBy);
                parameters.Add("@DefectAssignedDateAndTime", objInvoiceAdjustmentRequest.DefectAssignedDateAndTime);
                parameters.Add("@DefectAssignedBy", objInvoiceAdjustmentRequest.DefectAssignedBy);
                parameters.Add("@TotalAmountDue", objInvoiceAdjustmentRequest.TotalAmountDue);
                parameters.Add("@AdjustmentRemarks", objInvoiceAdjustmentRequest.AdjustmentRemarks);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceAdjustmentRequest, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentRequest-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateInvoiceAdjustmentRequest-API  error " + ex);
                throw ex;
            }
        }


        public List<PTXboExemptionAccounts> GetExemptionAccountlist(int invoiceID, int invoiceAdjustmentRequestID)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("GetExemptionAccountlist-API  reached " + ((object)invoiceID).ToJson(false));
                parameters.Add("@InvoiceId", invoiceID);
                parameters.Add("@InvoiceAdjustmentRequestId", invoiceAdjustmentRequestID);


                var result = _dapperConnection.Select<PTXboExemptionAccounts>(StoredProcedureNames.usp_getExemptionAccountList, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetExemptionAccountlist-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetExemptionAccountlist-API  error " + ex);
                throw ex;

            }
        }

        public List<PTXboAdjustmentHistory> GetInvoiceAdjustmentHistory(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentHistory-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceAdjustmentRequestID", invoiceID);
                var result = _dapperConnection.Select<PTXboAdjustmentHistory>(StoredProcedureNames.usp_getInvoiceAdjustmentHistory, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetInvoiceAdjustmentHistory-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceAdjustmentHistory-API  error " + ex);

                throw ex;
            }
        }


    }
}
