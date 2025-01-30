using PTaxOnWeb.DataObj.Models;
using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Litigation;
using Spartaxx.BusinessObjects.ViewModels;
using Spartaxx.Common;
using Spartaxx.Common.Reports;
using Spartaxx.DataAccess;
using Spartaxx.DataObjects;
using Spartaxx.Utilities.Extenders;
using Spartaxx.Utilities.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    /// <summary>
    /// added by saravanans-tfs:47247
    /// </summary>
    public class MainscreenInvoiceRepository : IMainscreenInvoiceRepository, IDisposable
    {
        private readonly DapperConnection _dapperConnection;
        private readonly InvoiceCalculationRepository _invoiceCalculation;
        private readonly InvoiceRepository _invoice;

        public MainscreenInvoiceRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
            _invoiceCalculation = new InvoiceCalculationRepository(dapperConnection);
            _invoice = new InvoiceRepository(dapperConnection);

        }

        #region  Dispose all used resources..Added by saravanans
        private bool disposed = false;
        public void Dispose()
        {
            GC.Collect();
        }
        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                disposed = true;
            }
        }
        #endregion 

        public PTXvmMainScreenInvoiceDetail MainScreenInvoiceStatusCollection(int clientID)
        {
            try
            {
                PTXvmMainScreenInvoiceDetail mainScreenInvoiceDetail = new PTXvmMainScreenInvoiceDetail();
                Logger.For(this).Invoice("SubmitInvoiceGenerationDefects-API  reached " + ((object)clientID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ClientID", clientID);
                mainScreenInvoiceDetail = _dapperConnection.Select<PTXvmMainScreenInvoiceDetail>(StoredProcedureNames.usp_get_MainScreenInvoiceStatusCollection, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("SubmitInvoiceGenerationDefects-API  ends successfully ");
                return mainScreenInvoiceDetail;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SubmitInvoiceGenerationDefects-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
        }

        public List<PTXboCSInvoiceComments> GetInvoiceComments(PTXboInvoiceCommentsInput invoiceComments)
        {
            try
            {
                List<PTXboCSInvoiceComments> objCSInvoiceComments = new List<PTXboCSInvoiceComments>();
                Logger.For(this).Invoice("GetCSInvoiceComments-API  reached " + ((object)invoiceComments).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@Invoiceid", invoiceComments.InvoiceID);
                parameters.Add("@CommentsType", invoiceComments.CommentsType);
                objCSInvoiceComments = _dapperConnection.Select<PTXboCSInvoiceComments>(StoredProcedureNames.usp_getInvoiceComments, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetCSInvoiceComments-API  ends successfully ");
                return objCSInvoiceComments;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCSInvoiceComments-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public bool SaveCollectionComments(PTXboInvoiceRemarks objInvoiceRemarks)
        {
            try
            {
                Logger.For(this).Invoice("SaveCollectionComments-API  reached " + ((object)objInvoiceRemarks).ToJson(false));
                Hashtable parameters = new Hashtable();

                parameters.Add("@InvoiceID", objInvoiceRemarks.InvoiceID);
                parameters.Add("@InvoiceRemarks", objInvoiceRemarks.InvoiceRemarks);
                parameters.Add("@UpdatedBy", objInvoiceRemarks.UpdatedBy);
                parameters.Add("@UpdatedDateTime", objInvoiceRemarks.UpdatedDateTime);
                parameters.Add("@IsClientLevelComment", objInvoiceRemarks.IsClientLevelComment);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveCollectionComments, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                Logger.For(this).Invoice("SaveCollectionComments-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveCollectionComments-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public bool SaveorUpdateInvoiceRemarks(PTXboInvoiceRemarks objboInvoiceRemarks)
        {
            string errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("SaveorUpdateInvoiceRemarks-API  reached " + ((object)objboInvoiceRemarks).ToJson(false));
                var result = _invoiceCalculation.InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objboInvoiceRemarks, out errorMessage);
                Logger.For(this).Invoice("SaveorUpdateInvoiceRemarks-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveorUpdateInvoiceRemarks-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboInvoiceRemarks GetInvoiceRemarks(int invoiceRemarksID)
        {
            string errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("GetInvoiceRemarks-API  reached " + ((object)invoiceRemarksID).ToJson(false));
                Hashtable parameters = new Hashtable();

                parameters.Add("@InvoiceRemarksID", invoiceRemarksID);
                var result = _dapperConnection.Select<PTXboInvoiceRemarks>(StoredProcedureNames.usp_get_InvoiceRemarks, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetInvoiceRemarks-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceRemarks-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public List<PTXboCSInvoiceComments> DeleteCSInvoiceComments(PTXboDeletecsInvoiceCommentsInput commentsInput)
        {
            string errorMessage = string.Empty;
            try
            {
                List<PTXboCSInvoiceComments> objCSInvoiceComments = new List<PTXboCSInvoiceComments>();
                int invoiceId = 0;
                Logger.For(this).Invoice("DeleteCSInvoiceComments-API  reached " + ((object)commentsInput).ToJson(false));
                if (commentsInput.Entity != null && commentsInput.Entity.InvoiceRemarksID > 0)
                {
                    var InvoiceRemarks = GetInvoiceRemarks(commentsInput.Entity.InvoiceRemarksID);
                    if (InvoiceRemarks != null)
                    {
                        int remarksuserid = InvoiceRemarks.UpdatedBy != null ? Convert.ToInt32(InvoiceRemarks.UpdatedBy) : 0;
                        invoiceId = Convert.ToInt32(InvoiceRemarks.InvoiceID);
                        if (InvoiceRemarks.UpdatedDateTime != null)
                        {
                            DateTime rmkdateposted = (DateTime)InvoiceRemarks.UpdatedDateTime;
                            if (rmkdateposted.Date == DateTime.Now.Date && remarksuserid == commentsInput.Userid)
                            {
                                //delete payment in InvoiceRemarks table
                                Hashtable parameters = new Hashtable();
                                parameters.Add("@InvoiceRemarksID", commentsInput.Entity.InvoiceRemarksID);
                                var result = _dapperConnection.Execute(StoredProcedureNames.usp_delete_InvoiceRemarks, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                            }
                            else
                            {
                                if (rmkdateposted.Date != DateTime.Now.Date)
                                {
                                    // throw new Exception("Remarks PostedDate and RemarksPosted User are not valid");
                                    errorMessage = _invoiceCalculation.GetUserMessage("INVTRANS001");
                                }
                                else if (remarksuserid != commentsInput.Userid)
                                {
                                    // throw new Exception("Remarks PostedDate and RemarksPosted User are not valid");
                                    errorMessage = _invoiceCalculation.GetUserMessage("INVTRANS002");
                                }
                            }
                        }
                        PTXboInvoiceCommentsInput invoiceComments = new PTXboInvoiceCommentsInput()
                        {
                            InvoiceID = invoiceId,
                            CommentsType = 1
                        };
                        objCSInvoiceComments = GetInvoiceComments(invoiceComments);
                    }
                }
                Logger.For(this).Invoice("DeleteCSInvoiceComments-API  ends successfully ");
                return objCSInvoiceComments;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("DeleteCSInvoiceComments-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Gets the Invoice Transaction against the Invoice Id
        /// </summary>
        /// <param name="Invoiceid"></param>
        /// <param name="objInvoiceHistory"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<PTXboCSInvoiceHistory> GetCSInvoiceTrasactions(PTXboInvoiceCommentsInput invoiceComments)
        {
            try
            {
                Logger.For(this).Invoice("GetCSInvoiceTrasactions-API  reached " + ((object)invoiceComments).ToJson(false));
                List<PTXboCSInvoiceHistory> objInvoiceHistory = new List<PTXboCSInvoiceHistory>();
                Hashtable parameters = new Hashtable();
                parameters.Add("@Invoiceid", invoiceComments.InvoiceID);
                parameters.Add("@InvoiceType", invoiceComments.InvoiceType);
                objInvoiceHistory = _dapperConnection.Select<PTXboCSInvoiceHistory>(StoredProcedureNames.usp_getInvoiceTransactions, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetCSInvoiceTrasactions-API  ends successfully ");
                return objInvoiceHistory;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCSInvoiceTrasactions-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<PTXboCSInvoiceDetails> GetInvoicecollectionDetails(PTXboInvoiceFilter gridvalues)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoicecollectionDetails-API  reached " + ((object)gridvalues).ToJson(false));
                List<PTXboCSInvoiceDetails> objCSInvoiceDetails = new List<PTXboCSInvoiceDetails>();
                Hashtable parameters = new Hashtable();
                parameters.Add("@Clientid", gridvalues.ClientID);
                parameters.Add("@All", gridvalues.chkAllinvoice);
                parameters.Add("@Collectioninv", gridvalues.chkCollections);
                parameters.Add("@Openinv", gridvalues.chkOpne);
                parameters.Add("@Invcredit", gridvalues.chkCredit);
                parameters.Add("@Closedinv", gridvalues.chkClosed);
                parameters.Add("@Invoicetype", gridvalues.CmbInvoiceType);
                objCSInvoiceDetails = _dapperConnection.Select<PTXboCSInvoiceDetails>(StoredProcedureNames.usp_getinvoicecollectiondetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetInvoicecollectionDetails-API  ends successfully ");
                return objCSInvoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoicecollectionDetails-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public PTXboInvoiceFilter GetInvoicecounts(int clientid)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoicecounts-API  reached " + ((object)clientid).ToJson(false));
                PTXboInvoiceFilter objinvoicefilter = new PTXboInvoiceFilter();
                Hashtable parameters = new Hashtable();
                parameters.Add("@Clientid", clientid);
                objinvoicefilter = _dapperConnection.Select<PTXboInvoiceFilter>(StoredProcedureNames.usp_getinvoicecollectiondetailscounts, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetInvoicecounts-API  ends successfully");
                return objinvoicefilter;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoicecounts-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        //added by saravanans-tfs:48546
        public int? GetInvoiceCheckPaymentStatus(int invoiceId)
        {
            try
            {
                Logger.For(this).Transaction("GetInvoiceCheckPaymentStatus-API  reached " + ((object)invoiceId).ToJson(false));

                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceId", invoiceId);
                var result = _dapperConnection.Select<int?>(StoredProcedureNames.usp_getinvoicecheckpaymentstatus, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Transaction("GetInvoiceCheckPaymentStatus-API  ends successfully");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("GetInvoiceCheckPaymentStatus-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// To retrieve user message based on user message ID
        /// </summary>
        /// <param name="UserMessageCode"></param>
        /// <returns></returns>
        public string GetUserMessage(string userMessageCode)
        {
            try
            {
                Logger.For(this).Invoice("GetUserMessage-API  reached " + ((object)userMessageCode).ToJson(false));

                var result = _invoiceCalculation.GetUserMessage(userMessageCode);
                Logger.For(this).Invoice("GetUserMessage-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetUserMessage-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public List<PTXboPastDueNotices> GetPastDueNotices(int invoiceId)
        {
            try
            {
                Logger.For(this).Invoice("GetPastDueNotices-API  reached " + ((object)invoiceId).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceId", invoiceId);
                var pastDueNoticeList = _dapperConnection.Select<PTXboPastDueNotices>(StoredProcedureNames.Usp_GetPastDueNotices, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetPastDueNotices-API  ends successfully ");
                return pastDueNoticeList;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetPastDueNotices-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<PTXboFlagCollectionDetails> GetFlagCollectionDetails(int clientId)
        {
            try
            {
                Logger.For(this).Invoice("GetFlagCollectionDetails-API  reached " + ((object)clientId).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ClientId", clientId);
                var objFlagCollection = _dapperConnection.Select<PTXboFlagCollectionDetails>(StoredProcedureNames.usp_getCollectionCodeAndFlagDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetFlagCollectionDetails-API  ends successfully ");
                return objFlagCollection;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetFlagCollectionDetails-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<PTXboInvoice> GetCCPaymentInvoiceDetails(int ccpaymentId)
        {
            try
            {
                Logger.For(this).Invoice("GetCCPaymentInvoiceDetails-API  reached " + ((object)ccpaymentId).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ccpaymentid", ccpaymentId);
                var invoiceDetails = _dapperConnection.Select<PTXboInvoice>(StoredProcedureNames.usp_GetCCPaymentInvoiceDetailsBasedOnInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetCCPaymentInvoiceDetails-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCCPaymentInvoiceDetails-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SaveManualPaymentPosting(PTXboManualPaymentPostingInput manualPaymentPosting)
        {
            try
            {
                Logger.For(this).Invoice("SaveManualPaymentPosting-API  reached " + ((object)manualPaymentPosting).ToJson(false));
                var invoice = _invoiceCalculation.GetInvoiceDetailsById(manualPaymentPosting.InvoiceId).FirstOrDefault();
                var ApplicableInterest = (invoice.ApplicableInterest != null ? invoice.ApplicableInterest : 0);
                decimal creditAmount = 0;
                decimal creditAdjAmount = 0;
                decimal creditIntAmount = 0;
                bool isDataInsert = true;
                bool isIntDataInsert = true;

                decimal? compondInterest = invoice.CompoundInterest;
                var CompoundInterest = (compondInterest != null ? compondInterest : 0);

                var InterestPaid = (invoice.InterestPaid != null ? invoice.InterestPaid : 0);
                var InterestAdjusted = (invoice.InterestAdjusted != null ? invoice.InterestAdjusted : 0);
                //Modified By Pavithra.B on 10Feb2017 -TFS ID - Task 29196:Check Over Credit
                var InteresttotalAmount = Math.Round(Convert.ToDecimal((ApplicableInterest + CompoundInterest) - (InterestPaid + InterestAdjusted)), 2);
                var invoiceAmountTotal = Math.Round(Convert.ToDecimal(invoice.InvoiceAmount), 2);

                PTXboPayment objboPayment = new PTXboPayment();
                objboPayment.ClientId = manualPaymentPosting.ClientId;

                var BalanceDue = Math.Round(Convert.ToDecimal((invoice.ApplicableInterest + invoice.InvoiceAmount + invoice.CompoundInterest) - (invoice.InterestPaid + invoice.AmountPaid + invoice.InterestAdjusted + invoice.AmountAdjusted)), 2);

                if (invoice.AmountPaid == null)
                    invoice.AmountPaid = 0;
                if((manualPaymentPosting.PaymentAmount > BalanceDue || manualPaymentPosting.InterestAmount > BalanceDue) 
                    && manualPaymentPosting.PaymentTypeId!= 8 //Added by SaravananS. tfs id:63530
                    && manualPaymentPosting.PaymentTypeId != 11 //Added by SaravananS. tfs id:64167
                    && manualPaymentPosting.PaymentTypeId != 25 //Added by SaravananS. tfs id:63857
                    )
                {
                    if (manualPaymentPosting.PaymentMethod == 1 && manualPaymentPosting.PaymentAmount > BalanceDue)
                    {
                        creditAmount = manualPaymentPosting.PaymentAmount - BalanceDue;
                        manualPaymentPosting.PaymentAmount = manualPaymentPosting.PaymentAmount - creditAmount;
                    }
                    if (manualPaymentPosting.PaymentMethod == 2 && manualPaymentPosting.PaymentAmount > BalanceDue)
                    {
                        creditAdjAmount = manualPaymentPosting.PaymentAmount - BalanceDue;
                        manualPaymentPosting.PaymentAmount = manualPaymentPosting.PaymentAmount - creditAdjAmount;
                    }
                    if (manualPaymentPosting.PaymentMethod == 3 && manualPaymentPosting.InterestAmount > BalanceDue)
                    {
                        creditIntAmount = manualPaymentPosting.InterestAmount - BalanceDue;
                        manualPaymentPosting.InterestAmount = manualPaymentPosting.InterestAmount - creditIntAmount;
                    }
                }
               
                if (  manualPaymentPosting.PaymentAmount==0 && manualPaymentPosting.InterestAmount==0)
                {
                    if (creditAmount > 0)
                        isDataInsert = false;
                    if (creditAdjAmount > 0)
                        isDataInsert = false;
                    if (creditIntAmount > 0)
                        isIntDataInsert = false;
                }
                 
                if (manualPaymentPosting.PaymentMethod == 1 
                    && manualPaymentPosting.PaymentTypeId != 8 //Added by SaravananS. tfs id:63530
                    && manualPaymentPosting.PaymentTypeId != 11 //Added by SaravananS. tfs id:64167
                    && manualPaymentPosting.PaymentTypeId != 25 //Added by SaravananS. tfs id:63857
                    )
                {

                    // Added by Arunkumar sakthivel | TFS ID : 42060
                    if (manualPaymentPosting.PaymentAmount <= invoiceAmountTotal && ((invoice.AmountPaid + manualPaymentPosting.PaymentAmount) <= invoiceAmountTotal))
                    {
                        objboPayment.PaymentAmount = manualPaymentPosting.PaymentAmount;
                        objboPayment.InterestAmount = 0;

                        invoice.AmountPaid = (invoice.AmountPaid != null ? invoice.AmountPaid : 0) + manualPaymentPosting.PaymentAmount;
                        invoice.InterestPaid = (invoice.InterestPaid != null ? invoice.InterestPaid : 0) + 0;
                    }
                    else
                    {
                        if (invoice.AmountPaid >= invoice.InvoiceAmount &&  InteresttotalAmount == manualPaymentPosting.PaymentAmount)
                        {                            
                            objboPayment.InterestAmount = (InteresttotalAmount > 0 ? InteresttotalAmount : 0);
                            objboPayment.PaymentAmount = (manualPaymentPosting.PaymentAmount - Convert.ToDecimal((InteresttotalAmount > 0 ? InteresttotalAmount : 0)));

                            invoice.AmountPaid = (invoice.AmountPaid != null ? invoice.AmountPaid : 0) + (manualPaymentPosting.PaymentAmount - Convert.ToDecimal((InteresttotalAmount > 0 ? InteresttotalAmount : 0)));
                            invoice.InterestPaid = (invoice.InterestPaid != null ? invoice.InterestPaid : 0) + (InteresttotalAmount > 0 ? InteresttotalAmount : 0);

                        }
                        else
                        {
                            var intersetBy = (manualPaymentPosting.PaymentAmount + invoice.AmountPaid) - invoiceAmountTotal;
                            objboPayment.InterestAmount = (manualPaymentPosting.PaymentAmount + invoice.AmountPaid) - invoiceAmountTotal;
                            objboPayment.PaymentAmount = Convert.ToDecimal(manualPaymentPosting.PaymentAmount - objboPayment.InterestAmount);

                            invoice.AmountPaid = (invoice.AmountPaid != null ? invoice.AmountPaid : 0) + (manualPaymentPosting.PaymentAmount - (intersetBy != null ? intersetBy : 0));
                            invoice.InterestPaid = (invoice.InterestPaid != null ? invoice.InterestPaid : 0) + (intersetBy != null ? intersetBy : 0);
                        }

                    }

                    objboPayment.PaymentTypeID = manualPaymentPosting.PaymentTypeId;
                }
                else if (manualPaymentPosting.PaymentMethod == 4 || manualPaymentPosting.PaymentMethod == 5)
                {
                    objboPayment.PaymentAmount = manualPaymentPosting.PaymentAmount;
                    objboPayment.PaymentTypeID = manualPaymentPosting.PaymentTypeId;
                }
                else if (manualPaymentPosting.PaymentMethod == 6 )
                {
                    objboPayment.InterestAmount = manualPaymentPosting.InterestAmount;
                    objboPayment.PaymentTypeID = manualPaymentPosting.PaymentTypeId;
                }
                else
                {
                    objboPayment.InterestAmount = manualPaymentPosting.InterestAmount;
                    objboPayment.PaymentAmount = manualPaymentPosting.PaymentAmount;
                }

                objboPayment.PostedDate = manualPaymentPosting.PaymentDate;
                objboPayment.PaymentDescription = manualPaymentPosting.PaymentDescription;
                objboPayment.InvoicePaymentMethodId = manualPaymentPosting.PaymentMethod;
                objboPayment.CheckNumber = manualPaymentPosting.CheckNumber;
                objboPayment.BatchNumber = manualPaymentPosting.BatchNumber;

                //Added by Boopathi.S taskid-22004
                objboPayment.CreatedBy = manualPaymentPosting.UserId;
                objboPayment.CreatedDateTime = DateTime.Now;
                objboPayment.UpdatedBy = manualPaymentPosting.UserId;
                objboPayment.UpdatedDateTime = DateTime.Now;
                objboPayment.CreditToInvoiceid = manualPaymentPosting.CreditToInvoiceid;

                if (manualPaymentPosting.PaymentMethod == 1 ||  manualPaymentPosting.PaymentMethod == 2 )
                {
                    if (isDataInsert)
                        objboPayment.PaymentId = _invoiceCalculation.SaveOrUpdatePayment(objboPayment);
                }
               else if ( manualPaymentPosting.PaymentMethod == 3)
                {
                    if (isIntDataInsert)
                        objboPayment.PaymentId = _invoiceCalculation.SaveOrUpdatePayment(objboPayment);
                }
               else if (manualPaymentPosting.PaymentMethod == 4 || manualPaymentPosting.PaymentMethod == 5 || manualPaymentPosting.PaymentMethod == 6)
                {                   
                        objboPayment.PaymentId = _invoiceCalculation.SaveOrUpdatePayment(objboPayment);
                }

                PTXboInvoicePaymentMap objboInvoicePaymentMap = new PTXboInvoicePaymentMap();
                objboInvoicePaymentMap.InvoiceId = manualPaymentPosting.InvoiceId;
                objboInvoicePaymentMap.PaymentId = objboPayment.PaymentId;

                if (objboPayment.PaymentId >0)
                    _invoiceCalculation.SaveOrUpdateInvoicePaymentMap(objboInvoicePaymentMap);

                if (manualPaymentPosting.PaymentMethod == 2)
                {
                    invoice.AmountAdjusted = (invoice.AmountAdjusted != null ? invoice.AmountAdjusted : 0) + manualPaymentPosting.PaymentAmount;
                    invoice.InterestAdjusted = (invoice.InterestAdjusted != null ? invoice.InterestAdjusted : 0) + manualPaymentPosting.InterestAmount;
                }
                else if (manualPaymentPosting.PaymentMethod == 3)
                {
                    invoice.AmountAdjusted = (invoice.AmountAdjusted != null ? invoice.AmountAdjusted : 0) + manualPaymentPosting.PaymentAmount;
                    invoice.InterestAdjusted = (invoice.InterestAdjusted != null ? invoice.InterestAdjusted : 0) + manualPaymentPosting.InterestAmount;
                }
                int newlyCreatedInvoiceId = 0;
                _invoiceCalculation.UpdateInvoiceAdjustment(invoice, out newlyCreatedInvoiceId);

                if (manualPaymentPosting.PaymentMethod == 1 && manualPaymentPosting.PaymentTypeId == 15)
                {
                    SaveCreditPaymentMapping(objboPayment);
                }
                //if(BalanceDue <=0 && (manualPaymentPosting.PaymentMethod == 2 || manualPaymentPosting.PaymentMethod == 3))
                //{
                //    SaveCreditAdjusmentMapping(manualPaymentPosting.InvoiceId,objboPayment.PaymentId, manualPaymentPosting.PaymentMethod);
                //}
                //added by Boopathi.S
                //Save CorrPaymentMapping
                //Modified By Pavithra.B on 14Jun2017 - Bug 32106
                if (objboPayment.PaymentId>0 && manualPaymentPosting.PaymentMethod == 1 && (manualPaymentPosting.PaymentTypeId != 15 && manualPaymentPosting.PaymentTypeId != 8 && manualPaymentPosting.PaymentTypeId != 11 && manualPaymentPosting.PaymentTypeId != 17 && manualPaymentPosting.PaymentTypeId != 12))
                {
                    SaveCorrPaymentMapping(objboPayment.PaymentId);
                }
                if (creditAmount>0 || creditAdjAmount > 0|| creditIntAmount>0)
                {
                    objboPayment.PaymentTypeID = manualPaymentPosting.PaymentTypeId;
                    if (creditAmount > 0)
                    SaveManualCreditPayment(4,manualPaymentPosting.InvoiceId, manualPaymentPosting.ClientId, objboPayment.PaymentId, objboPayment.PaymentTypeID, objboPayment, creditAmount);
                    if (creditAdjAmount > 0)
                        SaveManualCreditPayment(5,manualPaymentPosting.InvoiceId, manualPaymentPosting.ClientId, objboPayment.PaymentId, objboPayment.PaymentTypeID, objboPayment, creditAdjAmount);
                    if (creditIntAmount > 0)
                    SaveManualCreditPayment(6,manualPaymentPosting.InvoiceId, manualPaymentPosting.ClientId, objboPayment.PaymentId, objboPayment.PaymentTypeID, objboPayment, creditIntAmount);
                }
                Logger.For(this).Invoice("SaveManualPaymentPosting-API  ends successfully ");
                if(manualPaymentPosting.PaymentMethod == 4 && manualPaymentPosting.PaymentTypeId == 15)
                {
                    if(objboPayment.CreditToInvoiceid!=0 && objboPayment.PaymentId>0 && creditAmount ==0)
                    SaveManualClosedCreditPayment(objboPayment.CreditToInvoiceid,objboPayment.PaymentId);
                }
                if (manualPaymentPosting.PaymentMethod == 4 || manualPaymentPosting.PaymentMethod == 5 || manualPaymentPosting.PaymentMethod == 6)
                {
                    return UpdateCreditInvoiceunderClient(manualPaymentPosting.InvoiceId, manualPaymentPosting.ClientId, objboPayment.PaymentId, objboPayment.PaymentTypeID, objboPayment);
                }
                else
                {
                    return UpdateMSInvoice(manualPaymentPosting.InvoiceId);
                }


            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveManualPaymentPosting-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SaveCreditManualPaymentPosting(PTXboManualCreditPaymentPostingInput creditPaymentPosting)
        {
            try
            {
                Logger.For(this).Invoice("SaveManualPaymentPosting-API  reached " + ((object)creditPaymentPosting).ToJson(false));
                return UpdateMSCreditInvoice(creditPaymentPosting);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveManualPaymentPosting-API  error " + ex);

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


       

        public bool UpdateMSInvoice(int invoiceID, bool isMainScreen = true)
        {
            try
            {
                Logger.For(this).Invoice("UpdateMSInvoice-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();

                parameters.Add("@invoiceID", invoiceID);
                parameters.Add("@IsMainScreen", isMainScreen);
                var result = _dapperConnection.Execute(StoredProcedureNames.usp_UpdateInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("UpdateMSInvoice-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateMSInvoice-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool UpdateMSCreditInvoice(PTXboManualCreditPaymentPostingInput creditPaymentPosting, bool isMainScreen = true)
        {
            try
            {
                Logger.For(this).Invoice("UpdateMSCreditInvoice-API  reached " + ((object)creditPaymentPosting).ToJson(false));
                Hashtable parameters = new Hashtable();

                parameters.Add("@InvoiceID", creditPaymentPosting.InvoiceId);
                parameters.Add("@ClientID", creditPaymentPosting.ClientId);
                parameters.Add("@PaymentID", creditPaymentPosting.PaymentId);
                var result = _dapperConnection.Execute(StoredProcedureNames.usp_InsertUpdateCreditInvoicePayment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("UpdateMSCreditInvoice-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateMSCreditInvoice-API  error " + ex);

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public bool UpdateCreditInvoiceunderClient(int invoiceID, int clientid, int paymentid, int PaymentTypeID, PTXboPayment payment)
        {
            try
            {
                Logger.For(this).Invoice("UpdateMSInvoice-API  reached " + ((object)clientid).ToJson(false));
                Hashtable parameters = new Hashtable();

                parameters.Add("@invoiceID", invoiceID);
                parameters.Add("@clientid", clientid);
                parameters.Add("@paymentid", paymentid);
                parameters.Add("@PaymentAmount", payment.PaymentAmount);
                parameters.Add("@PaymentTypeID", PaymentTypeID);
                var result = _dapperConnection.Execute("USP_InsertCreditPaymentDetails", parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("UpdateMSInvoice-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateMSInvoice-API  error " + ex);

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SaveManualCreditPayment(int PaymentMethodID, int invoiceID, int clientid, int paymentid, int PaymentTypeID, PTXboPayment payment,decimal creditAmount)
        {
            try
            {
                Logger.For(this).Invoice("SaveManualCreditPayment-API  reached.invoiceID: " + invoiceID+((object)payment).ToJson(false));
                Hashtable parameters = new Hashtable();

                parameters.Add("@PaymentMethodID", PaymentMethodID);
                parameters.Add("@invoiceID", invoiceID);
                parameters.Add("@clientid", clientid);
                parameters.Add("@paymentid", paymentid);
                parameters.Add("@PaymentDescription", payment.PaymentDescription);
                parameters.Add("@PaymentReceivedDate", payment.PaymentReceivedDate);
                parameters.Add("@PostedDate", payment.PostedDate);
                parameters.Add("@CreatedBy", payment.CreatedBy);
                parameters.Add("@CheckNumber", payment.CheckNumber);
                parameters.Add("@BatchNumber", payment.BatchNumber);
                parameters.Add("@creditAmount", creditAmount); 
                parameters.Add("@PaymentTypeID", PaymentTypeID);
                var result = _dapperConnection.Execute("USP_InsertManualCreditPaymentDetails", parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveManualCreditPayment-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveManualCreditPayment-API  error " + ex);

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SaveManualClosedCreditPayment(int invoiceID, int paymentid)
        {
            try
            {
               
                Hashtable parameters = new Hashtable(); 
                parameters.Add("@invoiceID", invoiceID); 
                parameters.Add("@paymentid", paymentid); 
                var result = _dapperConnection.Execute("usp_UpdateDebitClientCreditPayment", parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveManualCreditPayment-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveManualCreditPayment-API  error " + ex); 
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SaveCorrPaymentMapping(int paymentID)
        {
            try
            {
                Logger.For(this).Invoice("SaveCorrPaymentMapping-API  reached " + ((object)paymentID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@PaymentID", paymentID);
                bool result = Convert.ToBoolean(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insert_CorrPaymentMapping, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("SaveCorrPaymentMapping-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveCorrPaymentMapping-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public bool SaveCreditAdjusmentMapping(int InvoiceId ,int paymentID,int PaymentMethod )
        {
            try
            {
                Logger.For(this).Invoice("SaveCorrPaymentMapping-API  reached " + ((object)paymentID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@invoiceID", InvoiceId);
                parameters.Add("@PaymentId", paymentID);
                parameters.Add("@PaymentMethod", PaymentMethod);
                var result =_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_InsertUpdateClientCreditAdjustment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveCorrPaymentMapping-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveCorrPaymentMapping-API  error " + ex);

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        private bool SaveCreditPaymentMapping(PTXboPayment objboPayment)
        {
            try
            {
                Logger.For(this).Invoice("SaveCreditPaymentMapping-API  reached " + ((object)objboPayment).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@invoiceID", objboPayment.CreditToInvoiceid);
                parameters.Add("@PaymentId", objboPayment.PaymentId);
              var result= _dapperConnection.Execute(StoredProcedureNames.usp_UpdateDebitClientCreditPayment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("SaveCreditPaymentMapping-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveCorrPaymentMapping-API  error " + ex);

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public bool CheckLegalInvoice(string invoiceIds)
        {
            try
            {
                Logger.For(this).Invoice("CheckLegalInvoice-API  reached " + ((object)invoiceIds).ToJson(false));
                bool isAvailable = false;
                if (!string.IsNullOrEmpty(invoiceIds))
                {
                    string[] strInvoice = invoiceIds.Split(',');
                    foreach (string item in strInvoice)
                    {
                        isAvailable = _invoiceCalculation.GetInvoiceDetailsById(Convert.ToInt32(item)).Where(x => x.TermsTypeID == Enumerators.PTXenumTermsType.Legal.GetId()).Any();
                    }
                }
                Logger.For(this).Invoice("CheckLegalInvoice-API  ends successfully ");
                return isAvailable;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("CheckLegalInvoice-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool UpdatePaymentOrSettlementPlan(PTXboPaymentSettlementPlanInput paymentSettlement)
        {
            try
            {
                Logger.For(this).Invoice("updatePaymentOrSettlementPlan-API  reached " + ((object)paymentSettlement).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@PaymentSettlementPlanID", paymentSettlement.PaymentSettlementPlanID);
                parameters.Add("@PlanStatusId", paymentSettlement.Status.GetId());
                if (paymentSettlement.CollectionLetter.GetId() == Enumerators.PTXenumCollectionLetters.InstallmentAgreement.GetId())
                {
                    parameters.Add("@Option", 1);//payment plan
                }
                else if (paymentSettlement.CollectionLetter.GetId() == Enumerators.PTXenumCollectionLetters.Settlement.GetId())
                {
                    parameters.Add("@Option", 2);//settlement plan
                }

                var result = _dapperConnection.Select<PTXboInvoiceDetails>(StoredProcedureNames.usp_update_PaymentSettlementPlan, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("updatePaymentOrSettlementPlan-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("updatePaymentOrSettlementPlan-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public bool InsertLegalInvoice(PTXboLegalInvoice legalInvoiceMap)
        {
            try
            {
                Logger.For(this).Invoice("InsertLegalInvoice-API  reached " + ((object)legalInvoiceMap).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", legalInvoiceMap.InvoiceID);
                parameters.Add("@SelectedInvoiceID", legalInvoiceMap.SelectedInvoiceID);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insert_LegalInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("InsertLegalInvoice-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }



        public bool LegalInvoiceGeneration(PTXboLegalInvoiceGenerationInput legalInvoice)
        {
            try
            {
                bool isSuccess = false;
                Logger.For(this).Invoice("LegalInvoiceGeneration-API  reached " + ((object)legalInvoice).ToJson(false));
                if (string.IsNullOrEmpty(legalInvoice.CCPayment.InvoiceList) == false || legalInvoice.CCPayment.CCPaymentID > 0)
                {
                    int newlyCreatedInvoiceId = 0;
                    //Insert into PTAX_Invoice table with Invoice Type as "Legal"
                    PTXboInvoice objboInvoice = new PTXboInvoice();
                    objboInvoice.CreatedDateAndTime = DateTime.Now;
                    objboInvoice.InvoiceDate = DateTime.Now;
                    objboInvoice.ClientId = legalInvoice.CCPayment.ClientID;
                    objboInvoice.InvoiceTypeId = Enumerators.PTXenumTermsType.Legal.GetId();
                    objboInvoice.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.LegalInvoiceGenerated.GetId();
                    objboInvoice.InvoicingProcessingStatus = Enumerators.PTXenumInvoicingPRocessingStatus.LegalInvoiceGenerated.GetId();
                    objboInvoice.InvoiceAmount = Math.Round(Convert.ToDecimal(legalInvoice.CCPayment.ServiceFee + legalInvoice.CCPayment.AttorneyFee + legalInvoice.CCPayment.CourtCost), 2);
                    objboInvoice.AmountDue = Math.Round(Convert.ToDecimal(legalInvoice.CCPayment.ServiceFee + legalInvoice.CCPayment.AttorneyFee + legalInvoice.CCPayment.CourtCost), 2);
                    objboInvoice.AttorneyFee = legalInvoice.CCPayment.AttorneyFee;
                    objboInvoice.ServiceFee = legalInvoice.CCPayment.ServiceFee;
                    objboInvoice.CourtCost = legalInvoice.CCPayment.CourtCost;
                    objboInvoice.UpdatedBy = legalInvoice.UserId;
                    _invoiceCalculation.SaveOrUpdateInvoice(objboInvoice, out newlyCreatedInvoiceId);

                    //Inserting into PTAX_LegalInvoice               
                    string[] strInvoice = legalInvoice.CCPayment.InvoiceList.Split(',');
                    foreach (string item in strInvoice)
                    {
                        PTXboLegalInvoice objLegalInvoiceMap = new PTXboLegalInvoice();
                        objLegalInvoiceMap.InvoiceID = newlyCreatedInvoiceId;
                        objLegalInvoiceMap.SelectedInvoiceID = Convert.ToInt32(item);
                        InsertLegalInvoice(objLegalInvoiceMap);

                    }

                    isSuccess = true;
                }

                Logger.For(this).Invoice("LegalInvoiceGeneration-API  ends successfully ");
                return isSuccess;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("LegalInvoiceGeneration-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public List<PTXboCSInvoiceHistory> UpdateCSInvoiceTransaction(PTXboUpdateCSInvoiceTranscationInput objUpdateCSInvoice)
        {
            try
            {
                string errorMessage = string.Empty;
                Logger.For(this).Invoice("UpdateCSInvoiceTransaction-API  reached " + ((object)objUpdateCSInvoice).ToJson(false));
                List<PTXboCSInvoiceHistory> objInvoiceHistory = new List<PTXboCSInvoiceHistory>();
                int invoiceId = 0;
                if (objUpdateCSInvoice.InvoiceHistory != null && objUpdateCSInvoice.InvoiceHistory.PaymentId > 0 && objUpdateCSInvoice.InvoiceHistory.InvoiceId > 0)
                {
                    var payment = _invoiceCalculation.GetPayment(objUpdateCSInvoice.InvoiceHistory.PaymentId);
                    invoiceId = objUpdateCSInvoice.InvoiceHistory.InvoiceId;
                    bool isposteddatevalid = true;
                    // DateTime dtpostdate = Convert.ToDateTime(objUpdateCSInvoice.InvoiceHistory.PostedDate);
                    DateTime dtpostdate = DateTime.ParseExact(objUpdateCSInvoice.InvoiceHistory.PostedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (dtpostdate.Date == DateTime.Now.Date || isposteddatevalid)
                    {
                        //payment.PaymentAmount = objUpdateCSInvoice.InvoiceHistory.PrincipalAmount ?? (decimal)objUpdateCSInvoice.InvoiceHistory.PrincipalAmount;
                        payment.PaymentAmount = (objUpdateCSInvoice.InvoiceHistory.PrincipalAmount == null ? 0 : (decimal)objUpdateCSInvoice.InvoiceHistory.PrincipalAmount);//Added by SaravananS.tfs :55509
                        payment.InterestAmount = objUpdateCSInvoice.InvoiceHistory.InterestAmount;
                        payment.UpdatedBy = objUpdateCSInvoice.UserId;
                        payment.UpdatedDateTime = DateTime.Now;
                        payment.PostedDate = dtpostdate;
                        payment.IsLegalPayment = objUpdateCSInvoice.InvoiceHistory.IsLegalPayment;//Added by SaravananS.tfs :55509
                        payment.PaymentId = _invoiceCalculation.SaveOrUpdatePayment(payment);

                        //added by boopathi -Save CorrPaymentMapping
                        //Modified By Pavithra.B on 14Jun2017 - Bug 32106
                        if (payment.InvoicePaymentMethodId == 1 && (payment.PaymentTypeID != 15
                            && payment.PaymentTypeID != 8 && payment.PaymentTypeID != 11 && payment.PaymentTypeID != 17
                            && payment.PaymentTypeID != 12))
                        {
                            SaveCorrPaymentMapping(payment.PaymentId);
                        }

                        PTXboInvoiceCommentsInput objinvoice = new PTXboInvoiceCommentsInput()
                        {
                            InvoiceID = invoiceId,
                            InvoiceType = null
                        };
                        UpdateMSInvoice(invoiceId, objUpdateCSInvoice.IsMainScreen);
                        objInvoiceHistory = GetCSInvoiceTrasactions(objinvoice);
                    }
                    else
                    {
                        PTXboInvoiceCommentsInput invoiceComments = new PTXboInvoiceCommentsInput()
                        {
                            InvoiceID = invoiceId,
                            InvoiceType = null
                        };
                        objInvoiceHistory = GetCSInvoiceTrasactions(invoiceComments);
                        errorMessage = _invoiceCalculation.GetUserMessage("INV015");
                    }
                }

                // isSucess = string.IsNullOrEmpty(errorMessage) ? true : false;

                Logger.For(this).Invoice("UpdateCSInvoiceTransaction-API  ends successfully ");
                return objInvoiceHistory;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateCSInvoiceTransaction-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public decimal WaiveCompoundInterest(string invoiceId)
        {
            try
            {
                decimal compoundInterest = 0;
                decimal? invoiceAmount = 0;
                decimal? amountDue = 0;
                decimal waiveInterest = 0;
                Logger.For(this).Invoice("WaiveCompoundInterest-API  reached " + ((object)invoiceId).ToJson(false));
                string[] strInvoice = invoiceId.Split(',');
                foreach (string item in strInvoice)
                {
                    var invoiceDetails = _invoiceCalculation.GetInvoiceDetailsById(Convert.ToInt32(item)).FirstOrDefault();

                    if (invoiceDetails != null)
                    {
                        decimal compoundInterestNew = 0;
                        decimal? invoiceAmountNew = 0;
                        decimal? amountDueNew = 0;
                        if (invoiceDetails.CompoundInterest == 0)
                        {
                            decimal? SimpleInterest = invoiceDetails.ApplicableInterest == null ? 0 : invoiceDetails.ApplicableInterest;
                            compoundInterestNew = Convert.ToDecimal(SimpleInterest);
                        }
                        else
                        {
                            decimal? SimpleInterest = invoiceDetails.ApplicableInterest;
                            compoundInterestNew = invoiceDetails.CompoundInterest + Convert.ToDecimal(SimpleInterest);
                        }
                        invoiceAmountNew = invoiceDetails.InvoiceAmount;
                        amountDueNew = invoiceDetails.AmountDue;
                        compoundInterest = compoundInterest + compoundInterestNew;
                        invoiceAmount = invoiceAmount + invoiceAmountNew;
                        amountDue = amountDue + amountDueNew;
                    }
                }
                waiveInterest = Convert.ToDecimal(amountDue) - compoundInterest;

                Logger.For(this).Invoice("WaiveCompoundInterest-API  ends successfully ");
                return waiveInterest;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("WaiveCompoundInterest-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public decimal? CheckCompoundInterestAvailability(string invoiceId)
        {
            try
            {
                Logger.For(this).Invoice("CheckCompoundInterestAvailability-API  reached " + ((object)invoiceId).ToJson(false));
                decimal? compoundInterest = 0;

                //bool isSucess = false;
                string[] strInvoice = invoiceId.Split(',');
                foreach (string item in strInvoice)
                {
                    var invoiceDetails = _invoiceCalculation.GetInvoiceDetailsById(Convert.ToInt32(item)).FirstOrDefault();

                    if (invoiceDetails != null)
                    {
                        decimal? compountInterestNew = invoiceDetails.CompoundInterest;
                        compoundInterest = compoundInterest + compountInterestNew;
                    }
                }

                //  isSucess = true;
                Logger.For(this).Invoice("CheckCompoundInterestAvailability-API  ends successfully ");
                return compoundInterest;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("CheckCompoundInterestAvailability-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public decimal? WaiveSimpleInterest(string invoiceId)
        {
            try
            {
                Logger.For(this).Invoice("WaiveSimpleInterest-API  reached " + ((object)invoiceId).ToJson(false));
                decimal? waiveInterest = 0;
                decimal? simpleInterest = 0;
                decimal? invoiceAmount = 0;
                decimal? amountDue = 0;

                string[] strInvoice = invoiceId.Split(',');
                foreach (string item in strInvoice)
                {
                    var invoiceDetails = _invoiceCalculation.GetInvoiceDetailsById(Convert.ToInt32(item)).FirstOrDefault();

                    if (invoiceDetails != null)
                    {
                        decimal? simpleInterestNew = 0;
                        decimal? invoiceAmountNew = 0;
                        decimal? amountDueNew = 0;
                        simpleInterestNew = invoiceDetails.ApplicableInterest;
                        invoiceAmountNew = invoiceDetails.InvoiceAmount;
                        amountDueNew = invoiceDetails.AmountDue;
                        simpleInterest = simpleInterest + simpleInterestNew;
                        invoiceAmount = invoiceAmount + invoiceAmountNew;
                        amountDue = amountDue + amountDueNew;
                    }
                }
                waiveInterest = Convert.ToDecimal(amountDue) - simpleInterest;

                Logger.For(this).Invoice("WaiveSimpleInterest-API  ends successfully ");
                return waiveInterest;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("WaiveSimpleInterest-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public bool SaveOrUpdateSettlementPlan(PTXboSettlementPlan objSettlementPlan)
        {
            try
            {
                Logger.For(this).Invoice("SaveOrUpdateSettlementPlan-API  reached " + ((object)objSettlementPlan).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@SettlementPlanId", objSettlementPlan.SettlementPlanId);
                parameters.Add("@ClientId", objSettlementPlan.ClientId);
                parameters.Add("@PlanLevel", objSettlementPlan.PlanLevel);
                parameters.Add("@StartDate", objSettlementPlan.StartDate);
                parameters.Add("@EndDate", objSettlementPlan.EndDate);
                parameters.Add("@TotalBalanceDue", objSettlementPlan.TotalBalanceDue);
                parameters.Add("@SettlementAmount", objSettlementPlan.SettlementAmount);
                parameters.Add("@SavingInPercentage", objSettlementPlan.SavingInPercentage);
                parameters.Add("@SettlementDueDate", objSettlementPlan.SettlementDueDate);
                parameters.Add("@PlanStatusId", objSettlementPlan.PlanStatusId);
                parameters.Add("@UpdatedBy", objSettlementPlan.UpdatedBy);
                parameters.Add("@IsActive", objSettlementPlan.IsActive);
                parameters.Add("@UpdatedDateTime ", objSettlementPlan.UpdatedDateTime);
                var result = Convert.ToBoolean(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateSETTLEMENTPLAN, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("SaveOrUpdateSettlementPlan-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateSettlementPlan-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SaveOrUpdateSettlementPlanInvoices(PTXboSettlementPlanInvoices objSettlementPlanInvoices)
        {
            try
            {
                Logger.For(this).Invoice("SaveOrUpdateSettlementPlanInvoices-API  reached " + ((object)objSettlementPlanInvoices).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@SettlementPlanInvoicesId", objSettlementPlanInvoices.SettlementPlanInvoicesId);
                parameters.Add("@SettlementPlanId", objSettlementPlanInvoices.SettlementPlanId);
                parameters.Add("@InvoiceId", objSettlementPlanInvoices.InvoiceId);
                var result = Convert.ToBoolean(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateSETTLEMENTPLANInvoices, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList());
                Logger.For(this).Invoice("SaveOrUpdateSettlementPlanInvoices-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateSettlementPlanInvoices-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }



        public bool UpdateSettlementDetails(PTXboUpdateSettlementDetailsInput objUpdateSettlement)
        {
            try
            {
                Logger.For(this).Invoice("UpdateSettlementDetails-API  reached " + ((object)objUpdateSettlement).ToJson(false));

                string[] invoiceID = objUpdateSettlement.InvoiceIdList.Split(',').ToArray();
                if (objUpdateSettlement.SettlementPlan.SettlementPlanId == 0)
                {
                    SaveOrUpdateSettlementPlan(objUpdateSettlement.SettlementPlan);

                    foreach (string objInvoiceID in invoiceID)
                    {
                        PTXboSettlementPlanInvoices objSettlementPlanInvoices = new PTXboSettlementPlanInvoices()
                        {
                            SettlementPlanId = objUpdateSettlement.SettlementPlan.SettlementPlanId,
                            InvoiceId = Convert.ToInt32(objInvoiceID)

                        };
                        SaveOrUpdateSettlementPlanInvoices(objSettlementPlanInvoices);

                    }
                }
                else
                    SaveOrUpdateSettlementPlan(objUpdateSettlement.SettlementPlan);
                Logger.For(this).Invoice("UpdateSettlementDetails-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateSettlementDetails-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public PTXboAgentCollectionSummary GetCollectionDetailsSummary(int userId)
        {
            try
            {
                Logger.For(this).Invoice("GetCollectionDetailsSummary-API  reached " + ((object)userId).ToJson(false));

                Hashtable parameters = new Hashtable();
                parameters.Add("@userId", userId);
                var result = _dapperConnection.Select<PTXboAgentCollectionSummary>(StoredProcedureNames.usp_getCollectionDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetCollectionDetailsSummary-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCollectionDetailsSummary-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<PTXboCCPayInvoiceList> GetCCPaymentInvoiceListDetails(int clientID)
        {
            try
            {
                Logger.For(this).Invoice("GetCCPaymentInvoiceListDetails-API  reached " + ((object)clientID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ClientID", clientID);
                var result = _dapperConnection.Select<PTXboCCPayInvoiceList>(StoredProcedureNames.usp_GetCCPaymentInvoiceList, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetCCPaymentInvoiceListDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCCPaymentInvoiceListDetails-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboCCPayment GetCCPaymentDetailsForInvoice(int ccpaymentID)
        {
            try
            {
                Logger.For(this).Invoice("GetCCPaymentDetailsForInvoice-API  reached " + ((object)ccpaymentID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@CCPaymentID", ccpaymentID);
                var result = _dapperConnection.Select<PTXboCCPayment>(StoredProcedureNames.usp_GetCCPaymentDetailsForInvoices, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetCCPaymentDetailsForInvoice-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCCPaymentDetailsForInvoice-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public List<ClientDetails> GetClientDetails(string clientNo)
        {
            try
            {
                Logger.For(this).Invoice("GetClientDetails-API  reached " + ((object)clientNo).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ClientNumber", clientNo);
                var result = _dapperConnection.Select<ClientDetails>(StoredProcedureNames.USP_POW_GETCLIENTDETAILS, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetClientDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetClientDetails-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SaveOrUpdateCCPayment(PTXboCCPayment objCCPayment)
        {
            try
            {
                Logger.For(this).Invoice("SaveOrUpdateCCPayment-API  reached " + ((object)objCCPayment).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@CCPaymentID", objCCPayment.CCPaymentID);
                parameters.Add("@AmountDue", objCCPayment.AmountDue);
                parameters.Add("@AmountCharged", objCCPayment.AmountCharged);
                parameters.Add("@CCNumber", objCCPayment.CCNumber);
                parameters.Add("@CCType", objCCPayment.CCType);
                parameters.Add("@ConfirmationNo", objCCPayment.ConfirmationNo);
                parameters.Add("@CCStatus", objCCPayment.CCStatus);
                parameters.Add("@DateProcessed", objCCPayment.DateProcessed);
                parameters.Add("@DateSubmitted", objCCPayment.DateSubmitted);
                parameters.Add("@ReturnRecieptRequest", objCCPayment.ReturnRecieptRequest);
                parameters.Add("@CCExpDate", objCCPayment.CCExpDate);
                parameters.Add("@CCName", objCCPayment.CCName);
                parameters.Add("@VerificationCode", objCCPayment.VerificationCode);
                parameters.Add("@PaymentPosted", objCCPayment.PaymentPosted);
                parameters.Add("@Mail_Rcvd_Date", objCCPayment.Mail_Rcvd_Date);
                parameters.Add("@Mail_Rcvd_Flag", objCCPayment.Mail_Rcvd_Flag);
                parameters.Add("@FirstName", objCCPayment.FirstName);
                parameters.Add("@LastName", objCCPayment.LastName);
                parameters.Add("@Address", objCCPayment.Address);
                parameters.Add("@City", objCCPayment.City);
                parameters.Add("@State", objCCPayment.State);
                parameters.Add("@Zip", objCCPayment.Zip);
                parameters.Add("@EmailAddress", objCCPayment.EmailAddress);
                parameters.Add("@SendReceipt", objCCPayment.SendReceipt);
                parameters.Add("@ClientID", objCCPayment.ClientID);
                parameters.Add("@CreatedBy", objCCPayment.CreatedBy);
                parameters.Add("@CreatedDate", DateTime.Now);//objCCPayment.CreatedDate
                parameters.Add("@StatusID", objCCPayment.StatusID);
                parameters.Add("@OverCreditReason", objCCPayment.OverCreditReason);
                parameters.Add("@TransactionID", objCCPayment.TranscationId);
                parameters.Add("@Merchant", objCCPayment.Merchant);
                var result = Convert.ToBoolean(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateCCPayment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("SaveOrUpdateCCPayment-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateCCPayment-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }



        private bool InsertDetails(string[] responseArray, List<ClientDetails> clientDetailslst, string clientNumber, PTXboCCPayment objCCPayment, List<PTXboCCPaymentInvoiceDetails> lstCCPaymentDetails)
        {
            try
            {
                string _ccPaymentId = String.Empty;

                Logger.For(this).Invoice("InsertDetails-API  reached.responseArray " + ((object)responseArray).ToJson(false));
                Logger.For(this).Invoice("InsertDetails-API  reached.clientDetailslst " + ((object)clientDetailslst).ToJson(false));
                Logger.For(this).Invoice("InsertDetails-API  reached.clientDetailslst " + ((object)clientDetailslst).ToJson(false));
                Logger.For(this).Invoice("InsertDetails-API  reached.objCCPayment " + ((object)objCCPayment).ToJson(false));
                Logger.For(this).Invoice("InsertDetails-API  reached.lstCCPaymentDetails " + ((object)lstCCPaymentDetails).ToJson(false));

                decimal AmountCharged = objCCPayment.AmountCharged.GetValueOrDefault();
                List<PTXboInvoice> InvoicesFromDB = lstCCPaymentDetails.Select(i => i.Invoice).ToList();

                InvoicesFromDB.ForEach(q =>
                {
                    decimal InterestAmount = lstCCPaymentDetails.FirstOrDefault(i => i.Invoice.InvoiceID == q.InvoiceID).InterestAmount.GetValueOrDefault();
                    var invoice = _invoiceCalculation.GetInvoiceDetailsById(q.InvoiceID).FirstOrDefault();
                    q.AmountDue = Math.Round(invoice.AmountDue.GetValueOrDefault() - (objCCPayment.CanWaiveInterest ? Math.Round(InterestAmount, 2) : 0), 2);
                });
                InvoicesFromDB = InvoicesFromDB.OrderBy(o => o.AmountDue).ToList();

                decimal InvoiceAmountdue = InvoicesFromDB.Sum(q => q.AmountDue.GetValueOrDefault());

                decimal minimumAmounttobeappliedforallinvoices = Math.Round(AmountCharged / InvoicesFromDB.Count, 2);

                List<KeyValuePair<int, decimal>> Invoices = new List<KeyValuePair<int, decimal>>();

                decimal remainingamountdues = 0;

                InvoicesFromDB.ForEach(q =>
                {
                    if ((minimumAmounttobeappliedforallinvoices + remainingamountdues) > q.AmountDue.GetValueOrDefault())
                    {
                        Invoices.Add(new KeyValuePair<int, decimal>(q.InvoiceID, q.AmountDue.GetValueOrDefault()));
                    }
                    else if ((minimumAmounttobeappliedforallinvoices + remainingamountdues) == q.AmountDue)
                    {
                        Invoices.Add(new KeyValuePair<int, decimal>(q.InvoiceID, q.AmountDue.GetValueOrDefault()));
                    }
                    else if ((minimumAmounttobeappliedforallinvoices + remainingamountdues) < q.AmountDue)
                    {
                        Invoices.Add(new KeyValuePair<int, decimal>(q.InvoiceID, (minimumAmounttobeappliedforallinvoices + remainingamountdues)));
                    }
                    remainingamountdues = Math.Round(((minimumAmounttobeappliedforallinvoices + remainingamountdues) - q.AmountDue.GetValueOrDefault()) <= 0 ? 0 : (minimumAmounttobeappliedforallinvoices + remainingamountdues) - q.AmountDue.GetValueOrDefault(), 2);
                });


                int? objCCPaymentId = null;
                for (int j = 0; j < Invoices.Count; j++)
                {
                    //OnlineOneTimePaymentDetails param = new OnlineOneTimePaymentDetails();
                    PTXboOneTimePayment param = new PTXboOneTimePayment();
                    param.Address = clientDetailslst[0].Address ?? string.Empty;
                    param.CcExpDate = objCCPayment.CCExpDate ?? string.Empty;
                    //Modified by prabhakaran.v on aug 3 2016
                    //param.CcName = objCCPayment.FirstName ?? string.Empty;
                    param.CcName = objCCPayment.CCName ?? string.Empty;
                    //End
                    param.CcNumber = responseArray[50].ToString();
                    param.CardType = responseArray[51].ToString();
                    param.CcPaymentId = _ccPaymentId ?? string.Empty;
                    param.CcStatus = "This transaction has been approved." == responseArray[3].ToString() ? "Pending" : responseArray[3].ToString();
                    param.City = clientDetailslst[0].CityName ?? string.Empty;
                    param.ClientNumber = clientNumber ?? string.Empty;
                    param.ConfirmationNo = responseArray[1].ToString();
                    param.EmailAddress = string.IsNullOrEmpty(clientDetailslst[0].Email) ? string.Empty : clientDetailslst[0].Email;
                    param.FirstName = clientDetailslst[0].FirstName ?? string.Empty;
                    param.LastName = clientDetailslst[0].LastName ?? string.Empty;
                    //Modified by prabhakaran.v on aug 3 2016
                    //param.InvoiceAmount = Convert.ToString(Invoices[j].Value);
                    param.InvoiceAmount = "0";
                    //End
                    param.InvoiceId = Convert.ToInt32(Invoices[j].Key);
                    param.PwImageId = "0";
                    param.State = clientDetailslst[0].StateName ?? string.Empty;
                    //param.TotalInvoiceAmount = Invoices[j].Value.ToString();//responseArray[9].ToString() ?? string.Empty; //Invoices[j].Value added by saravanans.
                    param.TotalInvoiceAmount = responseArray[9].ToString() ?? string.Empty;
                    param.TransactionId = responseArray[6].ToString();

                    param.VerificationCode = objCCPayment.VerificationCode ?? string.Empty;
                    param.Zip = clientDetailslst[0].ZipCode ?? string.Empty;
                    param.DateProcessed = string.Empty;
                    param.DateSubmitted = string.Empty;

                    Logger.For(this).Invoice("Before start InsertOneTimePaymentDetails" + ((object)param).ToJson(false));
                    ///This method is called WebAPI   
                    //PTaxOnWeb.BAL.InvoiceBAL _invoiceBal = new PTaxOnWeb.BAL.InvoiceBAL();
                    //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    //_ccPaymentId = _invoiceBal.InsertOneTimePaymentDetails(param);

                    _ccPaymentId = InsertOneTimePaymentDetails(param);
                    Logger.For(this).Invoice("after start InsertOneTimePaymentDetails._ccPaymentId" + ((object)_ccPaymentId).ToJson(false));
                    if (string.IsNullOrEmpty(_ccPaymentId))
                    {

                        return false;
                    }
                    else
                    {
                        var ccPayment = GetCCPaymentDetailsForInvoice(Convert.ToInt32(_ccPaymentId));
                        if (ccPayment != null)
                        {
                            ccPayment.CreatedBy = objCCPayment.CreatedBy;
                            objCCPaymentId = ccPayment.CCPaymentID;
                            ccPayment.TranscationId = string.IsNullOrEmpty(ccPayment.TranscationId) ? objCCPayment.TranscationId : ccPayment.TranscationId;//added by saravanans
                            Logger.For(this).Invoice("SaveOrUpdateCCPayment" + ((object)ccPayment).ToJson(false));
                            SaveOrUpdateCCPayment(ccPayment);

                        }
                    }


                }
                if (objCCPaymentId != null)
                {
                    Logger.For(this).Invoice("UpdateCCPartialPayment" + ((object)objCCPaymentId.Value).ToJson(false));
                    UpdateCCPartialPayment(objCCPaymentId.Value);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateCCPartialPayment-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public string InsertOneTimePaymentDetails(PTXboOneTimePayment objOneTimePayment)
        {
            try
            {
                Logger.For(this).Invoice("InsertOneTimePaymentDetails-API  reached " + ((object)objOneTimePayment).ToJson(false));
                Hashtable parameters = new Hashtable();


                parameters.Add("@ClientNumber", objOneTimePayment.ClientNumber);
                parameters.Add("@TotalInvoiceAmount", objOneTimePayment.TotalInvoiceAmount);
                parameters.Add("@CcNumber", objOneTimePayment.CcNumber);
                parameters.Add("@ConfirmationNo", objOneTimePayment.ConfirmationNo);
                parameters.Add("@CcStatus", objOneTimePayment.CcStatus);
                parameters.Add("@CardType", objOneTimePayment.CardType);
                parameters.Add("@TransactionId", objOneTimePayment.TransactionId);
                parameters.Add("@CcName", objOneTimePayment.CcName);
                parameters.Add("@CcExpDate", objOneTimePayment.CcExpDate);
                parameters.Add("@VerificationCode", objOneTimePayment.VerificationCode);
                parameters.Add("@FirstName", objOneTimePayment.FirstName);
                parameters.Add("@LastName", objOneTimePayment.LastName);
                parameters.Add("@Address", objOneTimePayment.Address);
                parameters.Add("@City", objOneTimePayment.City);
                parameters.Add("@State", objOneTimePayment.State);
                parameters.Add("@Zip", objOneTimePayment.Zip);
                parameters.Add("@EmailAddress", objOneTimePayment.EmailAddress);
                parameters.Add("@CcPaymentId", objOneTimePayment.CcPaymentId);
                parameters.Add("@InvoiceId", objOneTimePayment.InvoiceId);
                parameters.Add("@PwImageId", objOneTimePayment.PwImageId);
                parameters.Add("@InvoiceAmount", objOneTimePayment.InvoiceAmount);


                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_InsertOneTimePaymentDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                Logger.For(this).Invoice("InsertOneTimePaymentDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("InsertOneTimePaymentDetails-API  error "+ ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        private bool UpdateCCPartialPayment(int ccPaymentId)
        {
            try
            {
                Logger.For(this).Invoice("UpdateCCPartialPayment-API  reached " + ((object)ccPaymentId).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@CCPaymentID", ccPaymentId);
                var result = _dapperConnection.Execute(StoredProcedureNames.USP_UpdateCCPartialPayment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("UpdateCCPartialPayment-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("UpdateCCPartialPayment-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public int SaveOrUpdateContactLog(PTXboContactLog objContactLog)
        {
            try
            {
                Logger.For(this).Invoice("SaveOrUpdateContactLog-API  reached " + ((object)objContactLog).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@Contactlogid", objContactLog.ContactLogID == 0 ? null : (object)objContactLog.ContactLogID);
                parameters.Add("@ClientId", objContactLog.ClientId);
                parameters.Add("@AccountId", objContactLog.AccountId == 0 ? null : (object)objContactLog.AccountId);
                parameters.Add("@TopicID", objContactLog.TopicId);
                parameters.Add("@ContactId", objContactLog.ContactId == 0 ? null : (object)objContactLog.ContactId);
                parameters.Add("@AltContact", objContactLog.AltContact == 0 ? null : (object)objContactLog.AltContact);
                parameters.Add("@ContactMode", objContactLog.ContactMode);
                parameters.Add("@ContactUserId", objContactLog.LogUserID);
                parameters.Add("@ContactUserRoleId", objContactLog.LogUserroleID == 0 ? null : (object)objContactLog.LogUserroleID);
                parameters.Add("@LogDateTime", objContactLog.LogDateTime);
                parameters.Add("@Reason", objContactLog.Reason);
                parameters.Add("@Disposition", objContactLog.Disposition);
                parameters.Add("@Remarks", objContactLog.Remarks);
                parameters.Add("@UpdatedBy", objContactLog.UpdatedBy == 0 ? null : (object)objContactLog.UpdatedBy);
                parameters.Add("@UpdatedDateTime", DateTime.Now);//objContactLog.UpdatedDateTime
                parameters.Add("@CSCallTypeId", objContactLog.CSCallTypeId);
                parameters.Add("@EscallationDateTime", (objContactLog.EscallationDateTime.Year == 1) ? null : (object)objContactLog.EscallationDateTime);
                parameters.Add("@CallDispositionTypeId", objContactLog.CallDispositionTypeId == 0 ? null : (object)objContactLog.CallDispositionTypeId);
                parameters.Add("@PropertyTaxLog_CallId", objContactLog.PropertyTaxLog_CallId);
                parameters.Add("@InvoiceID", objContactLog.InvoiceID);
                parameters.Add("@ClientCallDetailId", objContactLog.ClientCallDetailId);
                var result = Convert.ToInt32(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateContactLog, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("SaveOrUpdateContactLog-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateContactLog-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public bool SaveOrUpdateContactLogRemarks(PTXboContactLogRemarks objcontactlogremarks)
        {
            try
            {
                Logger.For(this).Invoice("SaveOrUpdateContactLogRemarks-API  reached " + ((object)objcontactlogremarks).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ContactLogRemarksId", objcontactlogremarks.ContactLogRemarksId);
                parameters.Add("@ContactlogId", objcontactlogremarks.ContactLogId);
                parameters.Add("@Remarks", objcontactlogremarks.Remarks);
                var result = Convert.ToBoolean(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateContactLogRemarks, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("SaveOrUpdateContactLogRemarks-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveOrUpdateContactLogRemarks-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        /// <summary>
        /// Save CC Payment Details along with its invoice details
        /// </summary>
        /// <param name="objCCPayment"></param>
        /// <param name="objCCInvoiceDetails"></param>
        /// <param name="SuccessMessage"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public PTXboCCPaymentStatus SaveCCPaymentDetails(PTXboSaveCCPaymentsInput objSaveCCPayments)
        {
            try
            {
                Logger.For(this).Invoice("SaveCCPaymentDetails-API  reached " + ((object)objSaveCCPayments).ToJson(false));
                PTXboCCPaymentStatus objCCPaymentStatus = new PTXboCCPaymentStatus();
                string errorMessage = string.Empty;
                string successMessage = string.Empty;
                PaymentGateWayDetails details = new PaymentGateWayDetails();
                //PTaxOnWeb.Utilities.Paymentgateway paymentGateway = new PTaxOnWeb.Utilities.Paymentgateway();

                var Client = _invoiceCalculation.GetClientById(objSaveCCPayments.CCPayment.ClientID);
                string clientNumber = Client != null ? Client.ClientNumber : string.Empty;
                var Clientdetails = GetClientDetails(clientNumber);
                details.CardNumber = objSaveCCPayments.CCPayment.CCNumber;
                details.CCV_Code = objSaveCCPayments.CCPayment.VerificationCode;

                details.ClientNo = clientNumber;
                details.Expiry = objSaveCCPayments.CCPayment.CCExpDate;
                details.InvoiceNo = string.Join(",", objSaveCCPayments.CCInvoiceDetails.Select(i => i.Invoice.InvoiceID));
                details.FirstName = Clientdetails[0].FirstName;

                PTaxOnWeb.Utilities.Paymentgateway paymentGateway = new PTaxOnWeb.Utilities.Paymentgateway();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;


                Logger.For(this).Invoice("paymentGateway.CreateoneTimePayment starts : details" + ((object)details + " +Clientdetails" + (object)Clientdetails + "objSaveCCPayments:" + (object)objSaveCCPayments).ToJson(false));
                string[] responseArray = paymentGateway.CreateoneTimePayment(details, Clientdetails, objSaveCCPayments.CCPayment.AmountCharged.GetValueOrDefault());

                Logger.For(this).Invoice("SaveCCPaymentDetails-paymentGateway.CreateoneTimePayment completed .responseArray  " + ((object)responseArray).ToJson(false));
                if (responseArray.Length > 0)
                {
                    if (responseArray[0] == "1")
                    {
                        //Modified by prabhakaran.v on Aug 05 2016
                        //Before insert we can adjust interest amount for calculation
                        #region [WaiveInterest]
                        if (objSaveCCPayments.CCPayment.CanWaiveInterest)
                        {
                            var invoicelist = objSaveCCPayments.CCInvoiceDetails.Select(q => new { q.Invoice.InvoiceID, q.InterestAmount });

                            foreach (var invoice in invoicelist)
                            {
                                if (invoice.InterestAmount.GetValueOrDefault() > 0)
                                {
                                    PTXboPayment payment = new PTXboPayment();
                                    payment.InterestAmount = invoice.InterestAmount;
                                    payment.InvoicePaymentMethodId = Spartaxx.DataObjects.Enumerators.PTXenumInvoicePaymentMethod.InterestAdjustment.GetId();
                                    //payment.PaymentDescription = "Interest Charge Off";
                                    payment.PaymentDescription = "Interest Agent Waiver/Discount"; //Added by SaravananS. tfs id:59413
                                    payment.PostedDate = DateTime.Now;
                                    payment.ClientId = objSaveCCPayments.CCPayment.ClientID;

                                    //Added by Boopathi.S taskid-22004
                                    payment.CreatedBy = objSaveCCPayments.Userid;
                                    payment.CreatedDateTime = DateTime.Now;
                                    Logger.For(this).Invoice("SaveOrUpdatePayment" + ((object)payment).ToJson(false));
                                    payment.PaymentId = _invoiceCalculation.SaveOrUpdatePayment(payment);

                                    PTXboInvoicePaymentMap paymap = new PTXboInvoicePaymentMap();
                                    paymap.InvoiceId = invoice.InvoiceID;
                                    paymap.PaymentId = payment.PaymentId;
                                    _invoiceCalculation.SaveOrUpdateInvoicePaymentMap(paymap);

                                    UpdateMSInvoice(invoice.InvoiceID);
                                }
                            }
                        }
                        #endregion
                        //End
                        objSaveCCPayments.CCPayment.CCStatus = responseArray[3] == "This transaction has been approved." ? "Pending" : responseArray[3];//Response Text
                        objSaveCCPayments.CCPayment.TranscationId = responseArray[6].ToString();//TranscationId
                        Logger.For(this).Invoice("InsertDetails:responseArray-" + ((object)responseArray + " Clientdetails-" + (object)Clientdetails + " CCPayment-" + objSaveCCPayments.CCPayment + " CCInvoiceDetails-" + objSaveCCPayments.CCInvoiceDetails).ToJson(false));
                        if (InsertDetails(responseArray, Clientdetails, clientNumber, objSaveCCPayments.CCPayment, objSaveCCPayments.CCInvoiceDetails))
                        {
                            successMessage = "The payment is processed successfully and will be posted for the invoice once it is settled in bank, this may take 1 to 2 business days.";

                        }
                        else
                        {
                            errorMessage = "Transaction completed sucessfully.Payment details inserting failed";
                        }
                        Logger.For(this).Invoice("SaveCCPaymentDetails-condition(responseArray[0] == 1) .successMessage:" + successMessage + "errorMessage:" + errorMessage);
                    }
                    else
                    {
                        if (responseArray[2] == "6")
                        {
                            errorMessage = "The payment is not successful. The credit card number is invalid. Please check the card details and try again.";
                        }
                        else if (responseArray[2] == "8")
                        {
                            errorMessage = "The payment is not successful. The credit card has expired. Please check the card details and try again.";
                        }
                        else
                        {
                            errorMessage = "The payment is not successful. " + responseArray[3].ToString() + " Please check the card details and try again.";
                        }
                        Logger.For(this).Invoice("SaveCCPaymentDetails-condition(responseArray[0] != 1) .successMessage:" + successMessage + "errorMessage:" + errorMessage);
                    }

                }
                else
                {
                    errorMessage = "Payment Failed";
                }

                if (!string.IsNullOrEmpty(successMessage))
                {

                    #region [Save Contact log]
                    //Added by Kishorekumar on 06/29/2016
                    //To Save Contact Log on Credit Card  Payment

                    string invoices = string.Join(",", objSaveCCPayments.CCInvoiceDetails.Select(i => i.Invoice.InvoiceID));
                    string ccNumber = objSaveCCPayments.CCPayment.CCNumber != null && objSaveCCPayments.CCPayment.CCNumber.Length > 4 ? string.Format("************{0}", objSaveCCPayments.CCPayment.CCNumber.Substring(objSaveCCPayments.CCPayment.CCNumber.Length - 4)) : objSaveCCPayments.CCPayment.CCNumber;

                    PTXboContactLog objContactLog = new PTXboContactLog();
                    objContactLog.ClientId = objSaveCCPayments.CCPayment.ClientID;
                    objContactLog.TopicId = EnumConstants.PTXContactLogTopic.Collections.GetId();
                    objContactLog.LogDateTime = DateTime.Now;
                    objContactLog.LogUserID = Convert.ToInt32(objSaveCCPayments.CCPayment.CreatedBy);
                    objContactLog.Reason = "Credit Card Request";
                    objContactLog.ContactLogID = SaveOrUpdateContactLog(objContactLog);


                    PTXboContactLogRemarks objcontactlogremarks = new PTXboContactLogRemarks();
                    objcontactlogremarks.ContactLogId = objContactLog.ContactLogID;
                    objcontactlogremarks.Remarks = string.Format("Credit Card request has been submitted for invoices({0}) and amount({1})-({2})", invoices, Math.Round(objSaveCCPayments.CCPayment.AmountCharged.GetValueOrDefault(), 2), ccNumber);
                    SaveOrUpdateContactLogRemarks(objcontactlogremarks);


                    #endregion
                }

                objCCPaymentStatus.ErrorMessage = errorMessage;
                objCCPaymentStatus.SuccessMessage = successMessage;
                objCCPaymentStatus.IsSuccess = true;
                Logger.For(this).Invoice("SaveCCPaymentDetails-API  ends successfully .successMessage:"+ successMessage+ "errorMessage:"+ errorMessage);
                return objCCPaymentStatus;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SaveCCPaymentDetails-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Created by Saravanan.S .tfs id:58666
        /// Waiving invoice interest from Collection module
        /// </summary>
        /// <returns></returns>
        public bool WaiveInvoiceInterest(List<PTXboPayment> paymentList)
        {
            foreach(var payment in paymentList)
            {
                try
                {
                Logger.For(this).Invoice("WaiveInvoiceInterest" + ((object)payment).ToJson(false));
                    if (payment.PaymentAmount > 0 || payment.InterestAmount > 0)
                    {
                        payment.PaymentId = _invoiceCalculation.SaveOrUpdatePayment(payment);
                        PTXboInvoicePaymentMap paymap = new PTXboInvoicePaymentMap();
                        paymap.InvoiceId = payment.Invoiceid;
                        paymap.PaymentId = payment.PaymentId;
                        _invoiceCalculation.SaveOrUpdateInvoicePaymentMap(paymap);
                        UpdateMSInvoice(payment.Invoiceid, false);
                    }
                }
                catch (Exception e)
                {
                    string error = e.Message;
                    Logger.For(this).Invoice("WaiveInvoiceInterest: error:"+ error + ((object)payment).ToJson(false));
                }
                finally
                {
                    Dispose();
                }

            }
            return true;
        }


        public string RemoveCCPaymentDetails(PTXboCCPayment objCCPayment)
        {
            try
            {
                Logger.For(this).Invoice("RemoveCCPaymentDetails-API  reached " + ((object)objCCPayment).ToJson(false));
                Hashtable parameters = new Hashtable();
                SaveOrUpdateCCPayment(objCCPayment);
                var result = _invoiceCalculation.GetUserMessage("CSM007");
                Logger.For(this).Invoice("RemoveCCPaymentDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("RemoveCCPaymentDetails-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        /// <summary>
        /// This method is used to validate and insert invoice record in Account level , Project level and Term level from HearingResult finalized
        /// </summary>
        /// <param name="objInvoiceGenerationData"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>        
        public bool InsertInvoiceData(PTXboInvoice objInvoiceFromHearingResult)
        {
            string errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("InsertInvoiceData-API  reached " + ((object)objInvoiceFromHearingResult).ToJson(false));
                Hashtable parameters = new Hashtable();

                var result = _invoiceCalculation.InsertInvoiceData(objInvoiceFromHearingResult, out errorMessage);
                Logger.For(this).Invoice("InsertInvoiceData-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("InsertInvoiceData-API  error "+ ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }

        }


        /// <summary>
        /// Created by SaravananS. tfs id:61797
        /// </summary>
        /// <param name="ccPaymentId"></param>
        /// <param name="errorstring"></param>
        /// <returns></returns>
        public string DownloadCCPaymentReceipt(int ccPaymentId)
        {
           string errorMessage = string.Empty;
           string reportOutputPath = string.Empty;
            string reportExportPath=ConfigurationManager.AppSettings["ReportGenExportPath"].ToString();
            InvoiceReport objInvoiceReport = new InvoiceReport();
            try
            {
                PTXboInvoiceCCReceipt ccreceipt = new PTXboInvoiceCCReceipt();
                ccreceipt= GetCCInvoicePaymentReceiptDetails(ccPaymentId);
                objInvoiceReport.GenerateCCInvoicePaymentReciept(ccPaymentId, reportExportPath, ccreceipt, out reportOutputPath, out errorMessage);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error(ex);
                errorMessage = ex.Message;
            }
            finally
            {
                Dispose();
            }
            return reportOutputPath;
        }

        /// <summary>
        ///  Created by SaravananS. tfs id:61797
        /// </summary>
        /// <param name="ccPaymentId"></param>
        /// <returns></returns>
        public PTXboInvoiceCCReceipt GetCCInvoicePaymentReceiptDetails(int ccPaymentId)
        {
            Hashtable parameters = new Hashtable();
            PTXboInvoiceCCReceipt ObjCCPaymentReceipt = new PTXboInvoiceCCReceipt();
            try
            {
                parameters.Add("@CCPaymentId", ccPaymentId);
                Logger.For(this).Invoice("GetCCInvoicePaymentReceiptDetails-API  reached.ccPaymentId: " + ccPaymentId.ToJson(false));
                ObjCCPaymentReceipt = _dapperConnection.Select<PTXboInvoiceCCReceipt>(PTXdoStoredProcedureNames.Usp_GetInvoiceCreditCardReceipt, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetCCInvoicePaymentReceiptDetails-API  ends successfully ");
                return ObjCCPaymentReceipt;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetCCInvoicePaymentReceiptDetails.ccPaymentId: " + ccPaymentId + "- API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        #region Mainscreen-Litigation
        //// <summary>
        /// Created by- Poornima
        /// Creted on - 18 Sep 2015
        /// This method is used to Save Details to send letters
        /// </summary>
        //public bool UpdateLitStatus(PTXboLitigationAccountDetails LitigationDetails, out string ErrorString, out int? LitInvoiceStatus, out int? LitigationId)
        //{
        //    LitInvoiceStatus = 0;
        //    LitigationId = 0;
        //    PTXboInvoiceReportOutput objInvoiceReportOutput = new PTXboInvoiceReportOutput();
        //    PTXboInvoiceReportInput objInvoiceReportInput = new PTXboInvoiceReportInput();

        //    //For generate Invoice Copy--added by saravanan
        //    int tempInvoiceId = 0;
        //    int tempGroupId = 0;
        //    int tempClientId = 0;
        //    int tempTaxyear = 0;
        //    int tempInvoiceTypeid = 0;
        //    //ends here.
        //    try
        //    {
        //        ErrorString = string.Empty;
        //        int flag = 0;
        //        bool invStatusUpdate = false;
        //        String SupplementName = string.Empty;
        //        PTXboLitigationDetails objLitigation = new PTXboLitigationDetails();
        //        objLitigation =_invoiceCalculation.GetLitigation(LitigationDetails.LitigationID);
        //        if (objLitigation == null)
        //        {
        //            objLitigation = new PTXboLitigationDetails();
        //        }
        //        else
        //        {
        //            if (LitigationDetails.SupplementName != null)
        //            {
        //                if (!LitigationDetails.SupplementName.Equals(objLitigation.SupplementName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    flag = 1;
        //                    SupplementName = objLitigation.SupplementName;
        //                }
        //            }

        //        }
        //        objLitigation.AllottedDateTime = LitigationDetails.AllottedTime;
        //        if (LitigationDetails.AllottedUserID.HasValue)
        //        {
        //            objLitigation.AllottedUserID =  Convert.ToInt32(LitigationDetails.AllottedUserID) ;
        //        }
        //        if (LitigationDetails.AllottedUserRoleID.HasValue)
        //        {
        //            objLitigation.AllottedUserRoleID =  LitigationDetails.AllottedUserRoleID.GetValueOrDefault() ;
        //        }
        //        if (LitigationDetails.AnalystID > 0)
        //        {
        //            objLitigation.AnalystID =  Convert.ToInt32(LitigationDetails.AnalystID) ;
        //        }
        //        objLitigation.AssignedRoleID = LitigationDetails.AssignedRoleID.GetValueOrDefault() > 0 ? LitigationDetails.AssignedRoleID.GetValueOrDefault()  : 0;
        //        objLitigation.AssignedDateTime = LitigationDetails.AssignedTime;
        //        objLitigation.AssignedBy = LitigationDetails.AssignedUserID.GetValueOrDefault() > 0 ?  Convert.ToInt32(LitigationDetails.AssignedUserID)  : 0;
        //        objLitigation.Attorney = LitigationDetails.Attorney;
        //        objLitigation.AttorneyConfDate = LitigationDetails.AttorneyConfDate;
        //        objLitigation.AuthLetterDate = LitigationDetails.AuthLetter;
        //        objLitigation.BatchCauseName = LitigationDetails.BatchCauseName;
        //        objLitigation.BatchCauseNo = LitigationDetails.BatchCauseNo;
        //        objLitigation.CADIntroLetterDate = LitigationDetails.CADIntroLetterDate;
        //        objLitigation.CADVerificationUserID = LitigationDetails.CADVerificationUserID.GetValueOrDefault() > 0 ?  Convert.ToInt32(LitigationDetails.CADVerificationUserID) : 0;
        //        objLitigation.CADVerified = LitigationDetails.CADVerified;
        //        objLitigation.CADVerifiedDate = LitigationDetails.CADVerifiedDate;
        //        objLitigation.CauseName = LitigationDetails.CauseName;
        //        objLitigation.CauseNo = LitigationDetails.CauseNo;
        //        objLitigation.ClientConfirmed = LitigationDetails.ClientConfirmed;
        //        objLitigation.CourtNo = LitigationDetails.CourtNo;
        //        objLitigation.DateConfirmed = LitigationDetails.DateConfirmed;
        //        objLitigation.DeedProvided = LitigationDetails.DeedProvided;
        //        objLitigation.DocketReceivedDate = LitigationDetails.DocketReceivedDate;
        //        objLitigation.DoNotContact = LitigationDetails.DoNotContact;
        //        // added by Preethi 38928 and 38929
        //        // objLitigation.Appraiserid = LitigationDetails.Appraiserid;
        //        //objLitigation.FloodLitTypeID = LitigationDetails.FloodLitTypeID;
        //        objLitigation.AppraisalOrderCreated = LitigationDetails.AppraisalOrderCreated;
        //        objLitigation.AppraisalCompleted = LitigationDetails.AppraisalCompleted;
        //        objLitigation.AppraisalValue = LitigationDetails.AppraisalValue;
        //        objLitigation.FloodLitTypeID = LitigationDetails.FloodLitTypeID.GetValueOrDefault() > 0 ? Convert.ToInt32(LitigationDetails.FloodLitTypeID)  : 0;
        //        objLitigation.Appraiserid = LitigationDetails.Appraiserid.GetValueOrDefault() > 0 ? Convert.ToInt32(LitigationDetails.Appraiserid)  : 0;
        //        //end
        //        objLitigation.ExpertAssignedID = LitigationDetails.ExpertAssignedID.GetValueOrDefault() > 0 ? Convert.ToInt32(LitigationDetails.ExpertAssignedID)  : 0;
        //        if (((Spartaxx.DataObjects.Enumerators.PTXdoUserRole)LitigationDetails.UserRoleId == Spartaxx.DataObjects.Enumerators.PTXdoUserRole.Admin) ||
        //          ((Spartaxx.DataObjects.Enumerators.PTXdoUserRole)LitigationDetails.UserRoleId == Spartaxx.DataObjects.Enumerators.PTXdoUserRole.ExecutiveSupreme) ||
        //      ((Spartaxx.DataObjects.Enumerators.PTXdoUserRole)LitigationDetails.UserRoleId == Spartaxx.DataObjects.Enumerators.PTXdoUserRole.LitSupervisor) ||
        //          ((Spartaxx.DataObjects.Enumerators.PTXdoUserRole)LitigationDetails.UserRoleId == Spartaxx.DataObjects.Enumerators.PTXdoUserRole.PESExpert) ||
        //      ((Spartaxx.DataObjects.Enumerators.PTXdoUserRole)LitigationDetails.UserRoleId == Spartaxx.DataObjects.Enumerators.PTXdoUserRole.PESSuperVisor) ||
        //          ((Spartaxx.DataObjects.Enumerators.PTXdoUserRole)LitigationDetails.UserRoleId == Spartaxx.DataObjects.Enumerators.PTXdoUserRole.Litigation))
        //        {
        //            objLitigation.ExpertRemarks = LitigationDetails.ExpertRemarks;
        //            objLitigation.PESMarketValue = LitigationDetails.PESMarketValue;
        //            objLitigation.PESUnequalAppraisalValue = LitigationDetails.PESUnequalAppraisalValue;
        //        }
        //        //var yhdetails = Repository<PTXdoUserRole>.GetQuery().FirstOrDefault(q => q. == LitigationDetails.AccountID && q.TaxYear == LitigationDetails.TaxYear);
        //        //objLitigation.ExpertRemarks = LitigationDetails.ExpertRemarks;

        //        objLitigation.ExpertsReportsDate = LitigationDetails.ExpertsReportsDate;
        //        objLitigation.FilingDueDate = LitigationDetails.FilingDueDate;
        //        objLitigation.FilingRequestDate = LitigationDetails.FilingRequestDate;
        //        objLitigation.FinalAgreedDate = LitigationDetails.FinalAgreedDate;
        //        objLitigation.InitialAgreedDate = LitigationDetails.InitialAgreedDate;
        //        objLitigation.Judge = LitigationDetails.Judge;
        //        objLitigation.LateFiled = LitigationDetails.LateFiled;
        //        objLitigation.LCLetterDate = LitigationDetails.LCLetterDate;
        //        objLitigation.LitAppraisalValue = LitigationDetails.LitAppraisalValue;
        //        if (objLitigation.LitAppraisalValue.HasValue)
        //        {
        //            objLitigation.LitFinalized = LitigationDetails.LitFinalized;
        //        }
        //        else
        //        {
        //            objLitigation.LitFinalized = false;
        //        }

        //        objLitigation.LitigationStatusID = LitigationDetails.LitigationStatusID.HasValue ?  LitigationDetails.LitigationStatusID.GetValueOrDefault()  : 0;
        //        objLitigation.LitMarketValue = LitigationDetails.LitMarketValue;
        //        objLitigation.LitRemarks = LitigationDetails.LitRemarks;
        //        objLitigation.OIRReceivedDate = LitigationDetails.OIRReceivedDate;
        //        objLitigation.OIRSentDate = LitigationDetails.OIRSentDate;
        //        objLitigation.SettlementDate = LitigationDetails.SettlementDate;
        //        objLitigation.SupplementDate = LitigationDetails.SupplementDate;
        //        objLitigation.SupplementName = LitigationDetails.SupplementName;
        //        objLitigation.TrailDate = LitigationDetails.TrailDate;
        //        //objLitigation.PESMarketValue = LitigationDetails.PESMarketValue;
        //        //objLitigation.PESUnequalAppraisalValue = LitigationDetails.PESUnequalAppraisalValue;
        //        objLitigation.UpdatedBy = Convert.ToInt32(LitigationDetails.UserID);
        //        objLitigation.UpdatedDate = DateTime.Now;
        //        if (LitigationDetails.ExpertSettlementDate.HasValue)
        //        {
        //            objLitigation.SettlementDate = LitigationDetails.ExpertSettlementDate;
        //        }
        //        if (LitigationDetails.ExpertLitAppraisalValue.GetValueOrDefault() > 0)
        //        {
        //            objLitigation.LitAppraisalValue = LitigationDetails.ExpertLitAppraisalValue.GetValueOrDefault();
        //        }

        //        if (LitigationDetails.ExpertLitMarketValue.GetValueOrDefault() > 0)
        //        {
        //            objLitigation.LitMarketValue = LitigationDetails.ExpertLitMarketValue.GetValueOrDefault();
        //        }

        //        //if (LitigationDetails.ResolutionID.GetValueOrDefault() > 0)
        //        //{
        //        //    objLitigation.LitigationStatus = new PTXdoLitigationStatus() { LitStatusID = LitigationDetails.ResolutionID.GetValueOrDefault() };
        //        //}


        //        //Add New columns
        //        objLitigation.CovLtrSentDate = LitigationDetails.CovLtrSentDate;
        //        //objLitigation.DeedProvided = LitigationDetails.DeedProvided ?? LitigationDetails.DeedProvided;                
        //        objLitigation.PFVerifiedDate = LitigationDetails.PFVerifiedDate;
        //        objLitigation.ClientConfirmDate = LitigationDetails.ClientConfirmDate;
        //        objLitigation.NameChgSubmitDate = LitigationDetails.NameChgSubmitDate;
        //        //objLitigation.PFVerifiedBy = !string.IsNullOrEmpty(LitigationDetails.PFVerifiedBy) ? LitigationDetails.PFVerifiedBy : "";
        //        objLitigation.PFVerifiedBy = LitigationDetails.PFVerifiedBy;
        //        objLitigation.SpecialFilingAccount = LitigationDetails.SpecialFilingAccount;
        //        objLitigation.DiscoverySentDate = LitigationDetails.DiscoverySentDate;
        //        //Added By Pavithra.B - TFS Id 29601
        //        objLitigation.ExemptInvoice = LitigationDetails.ExemptInvoice;
        //        //Added by Boopathi.S
        //        //Get posthearingtotalvalue HearingResult table
        //        bool isFinalized = objLitigation.LitFinalized.HasValue ? objLitigation.LitFinalized.Value : false;

        //        if (isFinalized && objLitigation.LitAppraisalValue.HasValue)
        //        {
        //            var yhdetails = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID,LitigationDetails.TaxYear).FirstOrDefault();

        //            PTXboHearingDetails objHearingDetails = new PTXboHearingDetails();
        //            PTXboHearingResult objHearingResult = new PTXboHearingResult();

        //            var ListObj = Repository<PTXdoHearingResult>.GetQuery().Join(Repository<PTXdoHearingDetails>.GetQuery()
        //              , HearingRes => HearingRes.HearingDetails.HearingDetailsId, HearingDet => HearingDet.HearingDetailsId
        //                , (HearingRes, HearingDet) => new
        //                {
        //                    HearingRes = HearingRes,
        //                    HearingDet = HearingDet
        //                })
        //            .Where(
        //                   b => b.HearingDet.YearlyHearingDetails.YearlyHearingDetailsId == yhdetails.YearlyHearingDetailsId
        //                );


        //            objHearingResult = ListObj.Select(a => a.HearingRes).Where(a => a.HearingType.HearingTypeId != 9).OrderByDescending(a => a.FinalizedDate).FirstOrDefault();
        //            objHearingDetails = ListObj.Select(a => a.HearingDet).FirstOrDefault();

        //            if (objHearingResult != null)
        //            {
        //                if ((objLitigation.LITInvoiceStatusid == 0 )|| (objLitigation.LITInvoiceStatusid != 0 && objLitigation.LITInvoiceStatusid != PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId()))
        //                {
        //                    if (objLitigation.ExemptInvoice != null && objLitigation.ExemptInvoice == true)
        //                    {
        //                        objLitigation.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.ExemptInvoice.GetId() ;
        //                        invStatusUpdate = true;
        //                    }
        //                    else
        //                    {
        //                        double intialvalue = Convert.ToDouble(objHearingResult.PostHearingTotalValue);//VH POSTHEARING TOTAL VALUE
        //                        double finalvalue = Convert.ToDouble(objLitigation.LitAppraisalValue.Value);
        //                        //Added By Pavithra.B on 29Dec2016 -TFS ID-27726 --START
        //                        if (intialvalue == 0)
        //                        {
        //                            objLitigation.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.Initialassessmentvalue0.GetId() ;
        //                        }
        //                        if (finalvalue == 0)
        //                        {
        //                            objLitigation.LITInvoiceStatusid =   PTXdoenumHRInvoiceStatus.Finalassessmentvalue0.GetId() ;
        //                        }
        //                        //Added By Pavithra.B on 29Dec2016 -TFS ID-27726--END
        //                        if (((objHearingResult.PostHearingTotalValue - Convert.ToDouble(objLitigation.LitAppraisalValue.Value)) <= 0) && (intialvalue != 0 && finalvalue != 0))
        //                        {
        //                            //Added By Pavithra.B on 02Feb2016 -If Reduction <=0 and Flatfee =0 then No reduction else null.
        //                            var InvoiceGroup = Repository<PTXdoAccount>.GetQuery().Where(a => a.AccountId == LitigationDetails.AccountID).Select(x => x.Group).FirstOrDefault();
        //                            var termInvoice = new PTXdoTerms();
        //                            if (InvoiceGroup != null)
        //                            {
        //                                termInvoice = InvoiceGroup.termsList.Where(x => x.InvoiceFrequency.InvoiceFrequencyID == 1 && x.termsType.Termstypeid == Enumerators.PTXenumInvoiceType.Litigation.GetId() && x.IsSpecializedTerm != true).FirstOrDefault();
        //                            }
        //                            if (termInvoice != null)
        //                            {
        //                                if (termInvoice != null && Convert.ToDecimal(termInvoice.FlatFee) == 0) // Modified by mohan.d on 5/4/2017 Task 30382:Error Message While Trying to Update Arbitration Tax
        //                                {
        //                                    //No Reduction
        //                                    objLitigation.LITInvoiceStatusid =  PTXdoenumHRInvoiceStatus.NoReduction.GetId();
        //                                    invStatusUpdate = true;
        //                                }
        //                                else
        //                                {
        //                                    if (objLitigation.LITInvoiceStatusid == 0 || objLitigation.LITInvoiceStatusid == 0)
        //                                    {
        //                                        objLitigation.LITInvoiceStatusid = 0;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        var yhdetail = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID ,LitigationDetails.TaxYear).Where(p=>p.TermsTypeId==1).FirstOrDefault();
        //                        if (yhdetail.GroupId != 0 && yhdetail.IsSpecializedTerm == true)
        //                        {
        //                            objLitigation.LITInvoiceStatusid =  PTXdoenumHRInvoiceStatus.InvoiceinSpecialQueue.GetId();
        //                        }
        //                        else
        //                        {
        //                            if (objLitigation.LITInvoiceStatusid == 0 || objLitigation.LITInvoiceStatusid == 0)
        //                                objLitigation.LITInvoiceStatusid = 0;
        //                        }
        //                    }
        //                    LitInvoiceStatus = objLitigation.LITInvoiceStatusid != 0 ? objLitigation.LITInvoiceStatusid : 0;
        //                }
        //                else
        //                {
        //                    LitInvoiceStatus = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId();
        //                }
        //            }
        //        }
        //        if (objLitigation.LitFinalized == false)
        //        {
        //            objLitigation.LITInvoiceStatusid = 0;
        //            LitInvoiceStatus = 0;
        //        }
        //        objLitigation.ExpressAgentID = LitigationDetails.ExpressAgentId.GetValueOrDefault() > 0 ?  Convert.ToInt32(LitigationDetails.ExpressAgentId) : 0;
        //        objLitigation.ExpressOfferNotAccepted = LitigationDetails.ExpressOfferNotAccepted;
        //        objLitigation.NoExpressOffer = LitigationDetails.NoExpressOffer;
        //        objLitigation.SettlememtConferenceDate = LitigationDetails.SettlememtConferenceDate;
        //        objLitigation.SettlementOffer = LitigationDetails.SettlementOffer;
        //        objLitigation.TwoWeekTrial = LitigationDetails.TwoWeekTrial;
        //        objLitigation.HardTrialDate = LitigationDetails.HardTrialDate;
        //        objLitigation.HardTrialTime = LitigationDetails.HardTrialTime;
        //        objLitigation.DiscoveryDeadline = LitigationDetails.DiscoveryDeadline;
        //        objLitigation.CADUEValue = LitigationDetails.CADUEValue;
        //        objLitigation.CADAppraisedValue = LitigationDetails.CADAppraisedValue;
        //        objLitigation.PAPESRatioStudy = LitigationDetails.PAPESRatioStudy;
        //        objLitigation.TargetValue = LitigationDetails.TargetValue;
        //        objLitigation.TriggerValue = LitigationDetails.TriggerValue;

        //       _invoiceCalculation.SaveOrUpdateLitigation(objLitigation);

        //        LitigationDetails.LitigationID = objLitigation.LitigationID;
        //        LitigationId = LitigationDetails.LitigationID;


        //        if (flag == 1)
        //        {
        //            PTXboSupplementNameHistory objSupplHistory = new PTXdoSupplementNameHistory();
        //            objSupplHistory.Litigation = objLitigation;
        //            objSupplHistory.SupplementName = SupplementName;
        //            objSupplHistory.UpdatedBy = new PTXdoUser() { Userid = Convert.ToInt32(LitigationDetails.UserID) };
        //            objSupplHistory.UpdatedDateTime = DateTime.Now;
        //            Repository<PTXdoSupplementNameHistory>.SaveOrUpdate(objSupplHistory);
        //        }



        //        if (LitigationDetails.objLitigationDocuments.Count > 0)
        //        {
        //            updateDocuments(LitigationDetails, out ErrorString);
        //        }


        //        var ObjYearlyHearingDetails = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID ,LitigationDetails.TaxYear).FirstOrDefault();
        //        if (ObjYearlyHearingDetails == null)
        //        {
        //            string currenttaxyear;
        //            currenttaxyear=_invoiceCalculation.GetParamValue(Spartaxx.DataObjects.EnumConstants.PTXParameters.CurrentTaxYear.GetId());
        //            var YearlyHearingDetails = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID,Convert.ToInt32(currenttaxyear));
        //            if (YearlyHearingDetails != null)
        //            {
        //                var PropertyDetails = new PTXdoPropertyDetails();
        //                var ValueHearingNotice = new PTXdoValueNotice();
        //                var PropertyJurisdiction = new List<PTXdoPropertysjurisdiction>();

        //                if (YearlyHearingDetails.propertyDetails != null)
        //                {
        //                    YearlyHearingDetails.propertyDetails.CopyTo(PropertyDetails, new string[] { "PropertyDetailsId" });

        //                    PropertyDetails.PropertysJurisdiction = null;
        //                    PropertyDetails.PropertyRemarkslst = null;



        //                    Repository<PTXdoPropertyDetails>.SaveOrUpdate(PropertyDetails);
        //                }
        //                if (YearlyHearingDetails.ValueHearingNotice != null)
        //                {
        //                    YearlyHearingDetails.ValueHearingNotice.CopyTo(ValueHearingNotice, new string[] { "ValueNoticeId" });
        //                    ValueHearingNotice.ValueNoticeRemarksList = null;
        //                    ValueHearingNotice.Yearlyhearingdetails = null;

        //                    Repository<PTXdoValueNotice>.SaveOrUpdate(ValueHearingNotice);

        //                }

        //                ObjYearlyHearingDetails = new PTXboYearlyHearingDetails();
        //                ObjYearlyHearingDetails.Account = YearlyHearingDetails.Account;
        //                ObjYearlyHearingDetails.propertyDetails = PropertyDetails;
        //                ObjYearlyHearingDetails.ValueHearingNotice = ValueHearingNotice;
        //                ObjYearlyHearingDetails.TaxYear = LitigationDetails.TaxYear;
        //                ObjYearlyHearingDetails.UpdatedBy = new PTXdoUser() { Userid = LitigationDetails.UserID };
        //                ObjYearlyHearingDetails.UpdatedDateTime = DateTime.Now;
        //            }

        //        }


        //        if (ObjYearlyHearingDetails.Litigation == null)
        //        {
        //            ObjYearlyHearingDetails.Litigation = objLitigation;
        //            _invoiceCalculation.SaveOrUpdateYearlyHearingDetails(ObjYearlyHearingDetails);
        //        }

        //        if (ObjYearlyHearingDetails.Litigation.LitigationStatus != null && ObjYearlyHearingDetails.LitigationStatus.LitStatusID == 0)
        //        {
        //            ObjYearlyHearingDetails.Litigation.LitigationStatus = null;
        //        }

        //        List<PTXboInvoice> lstInvoice = new List<PTXboInvoice>();
        //        var IsInvoiceExists =_invoiceCalculation.GetInvoiceAndHearingResultMap(objLitigation.LitigationID).Where(q=>q.InvoiceDate == null || q=>q.SettlementDate < q => q.InvoiceDate);
        //        if ((objLitigation.LitFinalized.HasValue ? objLitigation.LitFinalized.Value : false)
        //            &&
        //            ((objLitigation.LitAppraisalValue.HasValue ? objLitigation.LitAppraisalValue.Value : 0) > 0)
        //           )
        //        {
        //            var yearlyhearingDetails = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID ,LitigationDetails.TaxYear);

        //            PTXboInvoice objInvoice = new PTXboInvoice();
        //            objInvoice.InvoiceID = LitigationDetails.InvoiceID.GetValueOrDefault();
        //            objInvoice.TaxYear = Convert.ToInt16(yearlyhearingDetails.TaxYear);
        //            objInvoice.ClientId = yearlyhearingDetails.Account.Client.clientId;
        //            objInvoice.CreatedBy =  yearlyhearingDetails.UpdatedBy;
        //            objInvoice.InvoiceTypeId = Enumerators.PTXenumInvoiceType.Litigation.GetId();
        //            objInvoice.LitigationId = objLitigation.LitigationID;
        //            objInvoice.CreatedBy = Convert.ToInt32(LitigationDetails.UserID);
        //            //objInvoice.HearingResultId = objHearingResult.HearingResultsId;
        //            //objInvoice.CreatedBy = indexedNoticeList.userEnteredHearingResult.UpdatedBy == null ? 0 : indexedNoticeList.userEnteredHearingResult.UpdatedBy.Userid;
        //            lstInvoice.Add(objInvoice);

        //            var Acc = Repository<PTXdoAccount>.GetQuery().Where(x => x.Client.clientId == objInvoice.ClientId
        //                     && x.AccountId == yearlyhearingDetails.Account.AccountId).FirstOrDefault();

        //            var gropid = Acc == null ? 0 : (Acc.Group == null ? 0 : Acc.Group.groupID);

        //            if (gropid > 0)
        //            {
        //                var Term = Repository<PTXdoTerms>.GetQuery().Where(x => x.group.groupID == gropid && x.termsType.Termstypeid == Enumerators.PTXenumInvoiceType.Litigation.GetId()).FirstOrDefault();

        //                //Added by Boopathi.S
        //                //IsSpecializedTerm == true only Insert Invoice Details
        //                if (Term != null && !Term.IsSpecializedTerm)
        //                {
        //                    bool isInvGenerated = false;
        //                    if (!IsInvoiceExists && !invStatusUpdate)
        //                    {
        //                        isInvGenerated = SubmitForInvoiceGenerationLitigation(lstInvoice, ErrorString);
        //                    }
        //                    var LitDetails = _invoiceCalculation.GetLitigation(objLitigation.LitigationID);
        //                    PTXdoInvoiceAndHearingResultMap objInvoicePHRMAP = new PTXdoInvoiceAndHearingResultMap();
        //                    PTXdoInvoice objInvocie = new PTXdoInvoice();

        //                    var ListObj = Repository<PTXdoInvoice>.GetQuery().Join(Repository<PTXdoInvoiceAndHearingResultMap>.GetQuery()
        //                        , Invoicelst => Invoicelst.InvoiceID, InvoiceHRMaplst => InvoiceHRMaplst.Invoice.InvoiceID
        //                        , (Invoicelst, InvoiceHRMaplst) => new
        //                        {
        //                            Invoicelst = Invoicelst,
        //                            InvoiceHRMaplst = InvoiceHRMaplst
        //                        })
        //                    .Where(
        //                           b => b.InvoiceHRMaplst.LitigationDetails.LitigationID == LitDetails.LitigationID
        //                        );

        //                    objInvoicePHRMAP = ListObj.Select(a => a.InvoiceHRMaplst).FirstOrDefault();
        //                    objInvocie = ListObj.Select(a => a.Invoicelst).FirstOrDefault();

        //                    if (isInvGenerated)
        //                    {
        //                        if (objInvocie != null)
        //                        {
        //                            //Added By Pavithra.B on 29Dec2016 -TFS ID-27726
        //                            decimal intialvalue = Convert.ToDecimal(objInvocie.InitialAssessedValue);
        //                            decimal finalvalue = Convert.ToDecimal(objInvocie.FinalAssessedValue);
        //                            if (intialvalue == 0)
        //                            {
        //                                LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.Initialassessmentvalue0.GetId();

        //                            }
        //                            if (finalvalue == 0)
        //                            {
        //                                LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.Finalassessmentvalue0.GetId();

        //                            }
        //                            if (objInvocie.InvoiceAmount <= 5 && (intialvalue != 0 && finalvalue != 0))
        //                            {
        //                                LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.AmountLessthanorEqual5.GetId();

        //                            }
        //                            if ((objInvocie.InvoiceAmount > 5 && (intialvalue != 0 && finalvalue != 0)) || objInvocie.PriorYearTaxRate == 0)
        //                            {
        //                                LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceInQueue.GetId();
        //                                //HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId()


        //                            }
        //                            else if ((intialvalue != 0 && finalvalue != 0) && (LitDetails.LITInvoiceStatusid == 0 ))
        //                            {
        //                                LitDetails.LITInvoiceStatusid = 0;
        //                            }

        //                            LitInvoiceStatus = (LitDetails.LITInvoiceStatusid != 0 ) ? LitDetails.LITInvoiceStatusid : 0;
        //                        }
        //                        if (objLitigation.ExemptInvoice == false)
        //                        {
        //                            var yhdetail = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID,LitigationDetails.TaxYear).Where(p=>p.TermsTypeId==1).FirstOrDefault();
        //                            if (yhdetail.IsSpecializedTerm == true)
        //                            {
        //                                LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceinSpecialQueue.GetId();
        //                            }
        //                        }
        //                        _invoiceCalculation.SaveOrUpdateLitigation(LitDetails);

        //                    }
        //                    else if (isInvGenerated == false && (LitInvoiceStatus == 0 || LitDetails.LITInvoiceStatusid == 0 ) && !invStatusUpdate && !IsInvoiceExists && (Term.InvoiceGroupingType.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId()
        //                     || Term.InvoiceGroupingType.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId()))
        //                    {
        //                        LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.PendinggroupInvoice.GetId();

        //                        if (objLitigation.ExemptInvoice == false)
        //                        {
        //                            var yhdetail = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID ,LitigationDetails.TaxYear).Where(p=>p.TermsTypeId==1).FirstOrDefault();
        //                            if (yhdetail.IsSpecializedTerm == true)
        //                            {
        //                                LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceinSpecialQueue.GetId();
        //                            }
        //                        }
        //                        _invoiceCalculation.SaveOrUpdateLitigation(LitDetails);

        //                    }
        //                    //Added By Pavithra.B on 05Jan2017 - If invoice is created for the project level, then  update the HR Invoice Status for all the accounts under that project.
        //                    if (isInvGenerated)
        //                    {
        //                        //added by saravanans.Invoice copy  generation
        //                        tempInvoiceId = objInvocie.InvoiceID;
        //                        tempGroupId = objInvocie.Group.groupID;
        //                        tempClientId = objInvocie.Client.clientId;
        //                        tempTaxyear = objInvocie.TaxYear;
        //                        tempInvoiceTypeid = objInvocie.InvoiceType.Termstypeid;
        //                        //ends here.

        //                        if (objInvoicePHRMAP != null)
        //                        {
        //                            if (LitDetails.LITInvoiceStatusid == PTXdoenumHRInvoiceStatus.InvoiceInQueue.GetId() &&
        //                                (objInvocie.InvoiceGroupType.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId() ||
        //                                            objInvocie.InvoiceGroupType.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId()))
        //                            {
        //                                List<PTXboInvoiceAndHearingResultMap> inhrObj = new List<PTXboInvoiceAndHearingResultMap>();
        //                                inhrObj = _invoiceCalculation.GetInvoiceAndHearingResultMap(objInvocie.InvoiceID);
        //                                foreach (PTXboInvoiceAndHearingResultMap inLitigationId in inhrObj)
        //                                {
        //                                    if (inLitigationId.LitigationID != 0)
        //                                    {
        //                                        PTXboLitigationDetails objLitigationIdPro =_invoiceCalculation.GetLitigation(inLitigationId.LitigationID);
        //                                        if (objLitigationIdPro != null)
        //                                        {
        //                                            objLitigationIdPro.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceInQueue.GetId();

        //                                        }
        //                                        if (objLitigation.ExemptInvoice == false)
        //                                        {
        //                                            var yhdetail = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID ,LitigationDetails.TaxYear).Where(p=>p.TermsTypeId==1).FirstOrDefault();
        //                                            if (yhdetail.IsSpecializedTerm == true)
        //                                            {
        //                                                objLitigationIdPro.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceinSpecialQueue.GetId();
        //                                            }
        //                                        }
        //                                        _invoiceCalculation.SaveOrUpdateLitigation(objLitigationIdPro);

        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //Added by Boopathi.S
        //                    //Invoice amount Calculation
        //                    else if ((LitDetails.LITInvoiceStatusid == 0) && !invStatusUpdate && IsInvoiceExists)
        //                    {
        //                        var objInvoiceid = new PTXboInvoice();
        //                        PTXboInvoiceDetails objInvoiceDetails = new PTXboInvoiceDetails();
        //                        if (IsInvoiceExists)
        //                        {
        //                            if (objInvocie != null)
        //                            {
        //                                objInvoiceDetails.InvoiceId = objInvocie.InvoiceID;
        //                                objInvoiceDetails.InvoiceTypeId = objInvocie.InvoiceType.Termstypeid; // added by mohan.d  - Bug 34446:Litigation tab settled accounts- time consuming  



        //                                /* Added by Pavithra.B on 04Aug2016 - in order to calculate the invoice amount before genereating the invoice. */
        //                                //List<PTXboInvoiceDetails> getlstInvoiceDetailsisSpecialTerm;
        //                                List<PTXboInvoiceReport> getlstInvoiceDetails;
        //                                List<PTXboInvoiceAccount> lstInvoiceAccount;
        //                                string errorString = string.Empty;
        //                                string errorMessage = string.Empty;
        //                                PTXboInvoiceSearchCriteria objSearchCriteria = new PTXboInvoiceSearchCriteria();
        //                                objSearchCriteria.InvoiceId = objInvoiceDetails.InvoiceId;
        //                                objSearchCriteria.IsQ = true;
        //                                objSearchCriteria.InvoiceType = objInvoiceDetails.InvoiceTypeId;

        //                                // Commented purposly - Bug 34446:Litigation tab settled accounts- time consuming  -- below getInvoiceGenerationDetails output not used anywhere
        //                                //DataService.PTXdsInvoice.CurrentInstance.getInvoiceGenerationDetails(objSearchCriteria, out getlstInvoiceDetailsisSpecialTerm, out errorMessage);
        //                                PTXboInvoiceReportDetails objInvoiceReportDetails = new PTXboInvoiceReportDetails();
        //                                objInvoiceReportDetails=_invoiceCalculation.GetInvoiceReportDetails(objInvoiceDetails.InvoiceId, objInvoiceDetails.InvoiceTypeId);
        //                                getlstInvoiceDetails = objInvoiceReportDetails.InvoiceDetails;
        //                                lstInvoiceAccount = objInvoiceReportDetails.InvoiceAccount;
        //                                PTXboInvoice ObjInvoice = new PTXboInvoice();
        //                                PTXboInvoice Invociedetails = new PTXboInvoice();
        //                                List<PTXboInvoice> lstInvoiceDetailsfromDB = new List<PTXboInvoice>();
        //                                /* get the invoice details based on the invoiceId*/
        //                                ObjInvoice= _invoiceCalculation.GetInvoiceGenerationDetails(objInvoiceDetails.InvoiceId);

        //                                if (ObjInvoice != null && ObjInvoice.InvoiceDate != null)
        //                                {
        //                                    LitDetails.LITInvoiceStatusid =  PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId();
        //                                }
        //                                else
        //                                {
        //                                    // if (getlstInvoiceDetailsisSpecialTerm != null && getlstInvoiceDetailsisSpecialTerm.Count > 0)
        //                                    //{
        //                                    if (getlstInvoiceDetails != null && getlstInvoiceDetails.Count > 0)
        //                                    {
        //                                        if (ObjInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.GetId())
        //                                        {
        //                                            _invoiceCalculation.AccountLevelInvoiceGeneration(getlstInvoiceDetails, lstInvoiceAccount, objInvoiceDetails.updatedBy);
        //                                            //PTXdsMainScreen.CurrentInstance.AccountLevelInvoiceGeneration(getlstInvoiceDetails, lstInvoiceAccount, objInvoiceDetails.updatedBy, out errorMessage, out objInvoiceid);
        //                                        }
        //                                        else if (ObjInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId() || ObjInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
        //                                        {
        //                                            _invoiceCalculation.ProjectorTermInvoiceGeneration(getlstInvoiceDetails, lstInvoiceAccount, objInvoiceDetails.updatedBy);
        //                                            //PTXdsMainScreen.CurrentInstance.ProjectorTermInvoiceGeneration(getlstInvoiceDetails, lstInvoiceAccount, objInvoiceDetails.updatedBy, out errorMessage, out objInvoiceid);
        //                                        }
        //                                    }

        //                                    PTXboInvoice objCalculatedInvoice = _invoiceCalculation.GetInvoiceDetailsById(ObjInvoice.InvoiceID).FirstOrDefault();
        //                                    if (objCalculatedInvoice != null)
        //                                    {
        //                                        //Added By Pavithra.B on 29Dec2016 -TFS ID-27726
        //                                        decimal intialvalue = Convert.ToDecimal(objCalculatedInvoice.InitialAssessedValue);
        //                                        decimal finalvalue = Convert.ToDecimal(objCalculatedInvoice.FinalAssessedValue);
        //                                        if (intialvalue == 0)
        //                                        {
        //                                            LitDetails.LITInvoiceStatusid =  PTXdoenumHRInvoiceStatus.Initialassessmentvalue0.GetId();
        //                                        }
        //                                        if (finalvalue == 0)
        //                                        {
        //                                            LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.Finalassessmentvalue0.GetId();
        //                                        }
        //                                        if (objCalculatedInvoice.InvoiceAmount <= 5 && (intialvalue != 0 && finalvalue != 0))
        //                                        {
        //                                            LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.AmountLessthanorEqual5.GetId();

        //                                        }
        //                                        if ((objCalculatedInvoice.InvoiceAmount > 5 && (intialvalue != 0 && finalvalue != 0)) || objCalculatedInvoice.PriorYearTaxRate == 0)
        //                                        {
        //                                            LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceInQueue.GetId();
        //                                            //HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId()

        //                                            if (objCalculatedInvoice.InvoicingProcessingStatus == Enumerators.PTXenumInvoicingPRocessingStatus.WaitingInPendingResearchQueue.GetId())
        //                                            {
        //                                                objCalculatedInvoice.InvoicingProcessingStatus =  Enumerators.PTXenumInvoicingPRocessingStatus.ReadyForInvoicing.GetId();
        //                                            }
        //                                        }
        //                                        else if ((intialvalue != 0 && finalvalue != 0) && (LitDetails.LITInvoiceStatusid == 0 ))
        //                                        {
        //                                            LitDetails.LITInvoiceStatusid = 0;
        //                                        }
        //                                        int newInvoiceId = 0;
        //                                        _invoiceCalculation.SaveOrUpdateInvoice(objCalculatedInvoice,out newInvoiceId);

        //                                    }
        //                                }
        //                                if (objLitigation.ExemptInvoice == false)
        //                                {
        //                                    var yhdetail = _invoiceCalculation.GetYearlyHearingDetailsByAccountId(LitigationDetails.AccountID,LitigationDetails.TaxYear).Where(p=>p.TermsTypeId==1).FirstOrDefault();

        //                                    if (yhdetail.IsSpecializedTerm == true)
        //                                    {
        //                                        LitDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceinSpecialQueue.GetId() ;
        //                                    }
        //                                }
        //                                LitInvoiceStatus = (LitDetails.LITInvoiceStatusid != 0 ) ? LitDetails.LITInvoiceStatusid : 0;
        //                               _invoiceCalculation.SaveOrUpdateLitigation(LitDetails);
        //                            }
        //                        }
        //                    }
        //                }

        //            }

        //            //SubmitForInvoiceGenerationLitigation(lstInvoice, ErrorString);
        //        }


        //        //Invoice File generation-added by saravanan
        //        if (tempInvoiceId > 0)
        //        {

        //            string FileLocation = string.Empty;
        //            string errorMessage = string.Empty;
        //            string ReportOutputPath = string.Empty;
        //            FileLocation=_invoice.GetLatestInvoiceFile(tempInvoiceId);

        //            string ReportExportpath = System.Configuration.ConfigurationManager.AppSettings["ReportGenExportPath"].ToString();

        //            if (FileLocation != "")
        //            {
        //                if (System.IO.File.Exists(FileLocation))
        //                {
        //                    System.IO.File.Delete(FileLocation);//Need to rename this file instead of deleting in future..saravanans
        //                    FileLocation = string.Empty;
        //                }
        //            }
        //            objInvoiceReportInput.Groupid = tempGroupId;
        //            objInvoiceReportInput.LinkFieldValue = tempInvoiceId;
        //            objInvoiceReportInput.ClientId = tempClientId;
        //            objInvoiceReportInput.ServicePackageId = 62;
        //            objInvoiceReportInput.Taxyear = tempTaxyear;
        //            objInvoiceReportInput.ReportExportPath = ReportExportpath;
        //            objInvoiceReportInput.InvoiceTypeId = tempInvoiceTypeid;

        //            objInvoiceReportOutput = _invoiceCalculation.GenerateInvoicereports(objInvoiceReportInput);
        //            ReportOutputPath = objInvoiceReportOutput.ReportOutputPath;
        //            errorMessage = objInvoiceReportOutput.ErrorMessage;
        //           _invoice.SubmitInvoiceFiles(tempInvoiceId, 0, ReportOutputPath, 5639);

        //        }

        //        //ends here.				

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Error(ex);
        //        ErrorString = ex.Message;
        //        return false;
        //    }
        //}


        #endregion Mainscreen-Litigation


    }
}
