using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using Spartaxx.Common;
using Spartaxx.Common.Reports;
using Spartaxx.DataObjects;
//using Spartaxx.Utilities;
using Spartaxx.Utilities.Extenders;
using Spartaxx.Utilities.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Spartaxx.DataAccess
{
    /// <summary>
    /// added by saravanans-tfs:47247
    /// </summary>
    public class InvoiceCalculationRepository:IDisposable
    {
        private readonly DapperConnection _dapperConnection;
        public InvoiceCalculationRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
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
            if(this.disposed)
            {
                disposed = true;
            }
        }

        #region Invoice Regular queue
        public string GetUserMessage(string userMessageCode)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetUserMessage-API  reached " + ((object)userMessageCode).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@UserMessageCode", userMessageCode);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_GET_UserMessage, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-GetUserMessage-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetUserMessage-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

      
        public PTXboInvoice GetSpecificInvoiceGenerationDetails(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationDetails-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                PTXboInvoice invoiceDetails = new PTXboInvoice();
                parameters.Add("@InvoiceID", invoiceID);
                invoiceDetails = _dapperConnection.Select<PTXboInvoice>(StoredProcedureNames.usp_get_specificinvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationDetails-API  returns the value " + ((object)invoiceDetails).ToJson(false));
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationDetails-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboPhone GetContactDetails(int contactID)
        {
            Hashtable parameters = new Hashtable();
            PTXboPhone phone = new PTXboPhone();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetContactDetails-API  reached " + ((object)"contactID=" + contactID.ToString()).ToJson(false));
                parameters.Add("@ContactID", contactID);

                phone = _dapperConnection.Select<PTXboPhone>(StoredProcedureNames.usp_get_clientcontact, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetContactDetails-API  ends successfully ");
                return phone;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetContactDetails-API  error " + ((object)ex).ToJson(false)); ;
                throw ex;
            }
            finally
            {
                Dispose();
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
            
            int BouncedEmailCount = 0;
            bool IsBadAddress = false;
            string ClientEmail = string.Empty;
            List<PTXboCorrQueue> lstCorrQueue = new List<PTXboCorrQueue>();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-ValidateClientEmail-API  reached " + ((object)objCorrQueueList).ToJson(false));
                foreach (var item in objCorrQueueList)
                {
                    Hashtable parameters = new Hashtable();
                    parameters.Add("@ClientID", item.ClientID);
                    var result = _dapperConnection.Select<PTXboClientEmailValidation>(StoredProcedureNames.Usp_ValidateClientEmail, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                    if (result != null)
                    {

                        BouncedEmailCount = result.BouncedEmails;
                        ClientEmail = result.ClientEmail;
                        IsBadAddress = result.IsBadAddress;
                        if (item.DeliveryMethodId == Enumerators.PTXenumDefaultDeliveryMethod.Email.GetId())
                        {
                            if (BouncedEmailCount >= 1 || string.IsNullOrEmpty(ClientEmail))
                            {
                                item.DeliveryMethodId = Enumerators.PTXenumDefaultDeliveryMethod.USMail.GetId();
                                if (IsBadAddress)
                                {
                                    item.CorrProcessingStatusID = Enumerators.PTXenumCorrProcessingStatus.BadAddress.GetId();
                                }
                            }
                        }
                        else if (item.DeliveryMethodId == Enumerators.PTXenumDefaultDeliveryMethod.USMail.GetId())
                        {
                            if (IsBadAddress)
                            {
                                item.CorrProcessingStatusID = Enumerators.PTXenumCorrProcessingStatus.BadAddress.GetId();
                            }
                        }
                        lstCorrQueue.Add(item);

                    }

                }
                Logger.For(this).Invoice("Invoice Calculation-ValidateClientEmail-API  ends successfully ");
                return lstCorrQueue;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-ValidateClientEmail-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboInvoiceContact GetInvoiceGroupContactDetails(int groupId, int clientId, int corrQId = 0)
        {
            Hashtable parameters = new Hashtable();
            PTXboInvoiceContact invoiceContact = new PTXboInvoiceContact();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGroupContactDetails-reached " + ((object)groupId).ToJson(false));
                parameters.Add("@ClientId", clientId);
                parameters.Add("@GroupId", groupId.ToString());
                //parameters.Add("@corrQId", corrQId);

                invoiceContact = _dapperConnection.Select<PTXboInvoiceContact>(StoredProcedureNames.usp_get_ClientEmailAddress_Invoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return invoiceContact;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGroupContactDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool UpdateCorrAccounts(int accountId, int corrQId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateCorrAccounts-API  reached " + ((object)"accountId=" + accountId.ToString() + "corrQId=" + corrQId.ToString()).ToJson(false));
                parameters.Add("@AccountId", accountId);
                parameters.Add("@CorrQId", corrQId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertcorrqaccounts, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-UpdateCorrAccounts-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateCorrAccounts-API  error " + ((object)ex).ToJson(false)) ;
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool UpdateCreditCorrAccounts(int accountId, int corrQId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateCorrAccounts-API  reached " + ((object)"accountId=" + accountId.ToString() + "corrQId=" + corrQId.ToString()).ToJson(false));
                parameters.Add("@AccountId", accountId);
                parameters.Add("@CorrQId", corrQId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_InsertCreditCorrqAccounts, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-UpdateCorrAccounts-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateCorrAccounts-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void UpdateInvoiceStatus(int invoiceID, int corrProcessingStatusID)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceStatus-API  reached " + ((object)"invoiceID=" + invoiceID.ToString() + "corrProcessingStatusID=" + corrProcessingStatusID.ToString()).ToJson(false));
                parameters.Add("@InvoiceID", invoiceID);
                parameters.Add("@CorrProcessingStatusId", corrProcessingStatusID);

                var result = _dapperConnection.Execute(StoredProcedureNames.usp_updateinvoicestatus, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceStatus-API  ends successfully ");
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceStatus-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public bool UpdateClientCorrQueueRecordInvoice(List<PTXboCorrQueue> objCorrQueueList, int groupId)
        {

            Hashtable parameters = new Hashtable();
            PTXboInvoiceContact invoiceContact = new PTXboInvoiceContact();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-API  reached " + ((object)objCorrQueueList).ToJson(false));
                if (objCorrQueueList != null)
                {


                    List<PTXboCorrQueue> lstCorrQueue = new List<PTXboCorrQueue>();
                    lstCorrQueue = ValidateClientEmail(objCorrQueueList);

                    foreach (PTXboCorrQueue objCorrQueue in lstCorrQueue)
                    {
                        Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-inside for loop " + ((object)objCorrQueue).ToJson(false));
                        if (!objCorrQueue.IsCustomDelivery)
                        {
                            var clientGroupContact = GetInvoiceGroupContactDetails(groupId, objCorrQueue.ClientID);

                            if (clientGroupContact != null)
                            {
                                if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.Email.GetId())
                                {
                                    objCorrQueue.DeliveryAddress = clientGroupContact.ClientEmail;
                                    objCorrQueue.CCMailAddress = clientGroupContact.AdditionalEmail;//Added by SaravananS. tfs id:63611
                                }
                                else if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.USMail.GetId())
                                {
                                    objCorrQueue.DeliveryAddress = clientGroupContact.address;
                                }
                                else if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.Fax.GetId())
                                {
                                    if (clientGroupContact.ContactId != null)
                                    {
                                        var Contact = GetContactDetails(Convert.ToInt32(clientGroupContact.ContactId));

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
                        Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-Inserting invoice details in ptaxcorrqueue table ::objCorrQueue=>" +((object)objCorrQueue).ToJson(false));
                        //Inserting invoice details in ptaxcorrqueue table
                        var corrQID = SaveOrUpdateCorrQ(objCorrQueue);
                        objCorrQueue.CorrQID = corrQID;
                        //update corrid in corracc
                        if (corrQID>0)
                        {
                            Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice- " + ((object)objCorrQueue).ToJson(false));
                            if (objCorrQueue.AccountList !=null &&  objCorrQueue.AccountList.Any())
                            {
                                foreach (var CorrAccount in objCorrQueue.AccountList.Split(','))
                                {
                                    //Inserting account details in ptaxcorraccounts table
                                    Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-going to insert corraccounts " + ((object)CorrAccount).ToJson(false));
                                    UpdateCorrAccounts(Convert.ToInt32(CorrAccount), corrQID);
                                }
                            }
                            else
                            {
                                Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-given objCorrQueue doesn't have any accounts " + ((object)objCorrQueue).ToJson(false));
                            }
                        }
                        else
                        {
                            Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-corrQID is zero.corrQID: " + ((object)corrQID).ToJson(false));
                        }

                        Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-going to start UpdateInvoiceStatus " + ((object)objCorrQueue).ToJson(false));
                        //updating invoice status in 1-ptaxhearingresult,2-ptaxlitigation,3-ptaxarbitrationdetails
                        UpdateInvoiceStatus(objCorrQueue.LinkFieldValue, objCorrQueue.CorrProcessingStatusID);
                        Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-UpdateInvoiceStatus is completed " + ((object)objCorrQueue).ToJson(false));

                        Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-API  ends successfully ");
                    }

                    return true;
                }
                else
                {
                    Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-given objCorrQueueList is null" + ((object)objCorrQueueList).ToJson(false));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboCorrQueue InsertUpdateCorrQCreditInvoice(List<PTXboCorrQueue> objCorrQueueList)
        { 
            Hashtable parameters = new Hashtable();
            PTXboInvoiceContact invoiceContact = new PTXboInvoiceContact();
            PTXboCorrQueue objCorrQ= new PTXboCorrQueue();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-API  reached " + ((object)objCorrQueueList).ToJson(false));
                if (objCorrQueueList != null)
                {


                    List<PTXboCorrQueue> lstCorrQueue = new List<PTXboCorrQueue>();
                    lstCorrQueue = ValidateClientEmail(objCorrQueueList);

                    foreach (PTXboCorrQueue objCorrQueue in lstCorrQueue)
                    {
                        if (!objCorrQueue.IsCustomDelivery)
                        {
                            var clientGroupContact = GetInvoiceGroupContactDetails(0, objCorrQueue.ClientID);

                            if (clientGroupContact != null)
                            {
                                if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.Email.GetId())
                                {
                                    objCorrQueue.DeliveryAddress = clientGroupContact.ClientEmail;
                                }
                                else if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.USMail.GetId())
                                {
                                    objCorrQueue.DeliveryAddress = clientGroupContact.address;
                                }
                                else if (objCorrQueue.DeliveryMethodId == Enumerators.PTXenumDeliveryMethod.Fax.GetId())
                                {
                                    if (clientGroupContact.ContactId != null)
                                    {
                                        var Contact = GetContactDetails(Convert.ToInt32(clientGroupContact.ContactId));

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
                        Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-Inserting invoice details in ptaxcorrqueue table ::objCorrQueue=>" + objCorrQueue);
                        //Inserting invoice details in ptaxcorrqueue table
                        var corrQID = SaveOrUpdateCorrQCreditLetter(objCorrQueue);
                        objCorrQueue.CorrQID = corrQID;
                        //update corrid in corracc
                        if (corrQID > 0)
                        {
                            if (objCorrQueue.AccountList.Any())
                            {
                                foreach (var CorrAccount in objCorrQueue.AccountList.Split(','))
                                {
                                    //Inserting account details in ptaxcorraccounts table
                                    UpdateCreditCorrAccounts(Convert.ToInt32(CorrAccount), corrQID);
                                }
                            }
                        }
                        
                        objCorrQ = objCorrQueue;

                        Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-API  ends successfully ");
                    }

                    return objCorrQ;
                }
                else
                {

                    return objCorrQ;
                }
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateClientCorrQueueRecordInvoice-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        //public bool IsOutOfTexas(int invoiceID)
        //{
        //    try
        //    {
        //        Hashtable parameters = new Hashtable();
        //        parameters.Add("@InvoiceID", invoiceID);
        //        var result = _dapperConnection.Execute(StoredProcedureNames.usp_checkinvoiceisoutoftexas, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        return Convert.ToBoolean(result);
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.For(this).Invoice("Invoice Calculation-IsOutOfTexas-API  error " + ex);
        //        throw ex;
        //    }
        //}

        //public PTXboInvoiceReportDetails GetInvoiceReportDetails(int invoiceID, int invoiceTypeId = (int)Enumerators.PTXenumInvoiceType.Standard, bool? isInvoiceDefect = null, bool isOutOfTexas = false, bool IsOTEntryscreen = false, bool isMultiyear = false, bool IsDisasterInvoice = false)
        public PTXboInvoiceReportDetails GetInvoiceReportDetails(PTXboInvoiceReportDetailsInput invoiceReportDetailsInput)
        {
            Hashtable parameters = new Hashtable();
            PTXboInvoiceReportDetails invoiceReportDetails = new PTXboInvoiceReportDetails();
            int invoiceID = invoiceReportDetailsInput.InvoiceId;
            int invoiceTypeId = invoiceReportDetailsInput.InvoiceTypeId == 0 ? (int)Enumerators.PTXenumInvoiceType.Standard : invoiceReportDetailsInput.InvoiceTypeId;
            bool? isInvoiceDefect = invoiceReportDetailsInput.IsInvoiceDefect;
            bool isOutOfTexas = invoiceReportDetailsInput.IsOutOfTexas;
            bool isOTEntryscreen = invoiceReportDetailsInput.IsOTEntryscreen;
            bool isMultiyear = invoiceReportDetailsInput.IsMultiyear;
            bool isDisasterInvoice = invoiceReportDetailsInput.IsDisasterInvoice;
            //Added by SaravananS.tfs id:55256
            var invoice = GetSpecificInvoiceGenerationDetails(invoiceID);
            if (invoice!= null && invoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId())
            {
                invoiceTypeId = invoice.InvoiceTypeId;
            }
            //Ends here.

           

            //Added by SaravananS. tfs id:60434
            bool isILHearing = IsILHearing(invoiceID);
            if(isILHearing)
            {
                isOTEntryscreen = true;
            }
            //Ends here.

            //Added by SaravananS.tfs id:56033
            if (!isDisasterInvoice)
            {
                isDisasterInvoice = IsDisasterInvoice(invoiceID);
            }
            //Ends here..

            isOutOfTexas = IsOutOfTexas(invoiceID);
            isMultiyear = IsMultiyear(invoiceID);

            //Added by SaravananS. tfs id:63335
            if (invoice != null && invoice.IsRegenerateInvoice == 1 && isOutOfTexas && !isMultiyear)
            {
                isOTEntryscreen = true;
            }
            //Ends here.

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-API  reached .invoiceReportDetailsInput:" + ((object)invoiceReportDetailsInput).ToJson(false)+";" + ((object)"invoiceID=" + invoiceID.ToString() + "invoiceTypeId=" + invoiceTypeId.ToString() + "isInvoiceDefect=" + isInvoiceDefect.ToString()).ToJson(false));
                parameters.Add("@InvoiceID", invoiceID);
                parameters.Add("@isInvoiceDefect", isInvoiceDefect);
                if(isDisasterInvoice)
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard-outoftaxes Entry screen inputs:" + parameters);
                    var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForPreview_Disaster, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                                  gr => gr.Read<PTXboInvoiceReport>(),
                                  gr => gr.Read<PTXboInvoiceAccount>());
                    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard outoftaxes Entry screen successfully finished.invoiceReportDetails.isDisasterInvoice:"+ isDisasterInvoice+".InvoiceDetails:" + ((object)invoiceReportDetails.InvoiceDetails.FirstOrDefault()).ToJson(false));
                
                }
                //Added by SaravananS. tfs id:64496
                else if(invoiceTypeId == Enumerators.PTXenumInvoiceType.Judgment.GetId())
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-Judgment inputs:" + parameters);
                    var result = _dapperConnection.SelectMultiple(StoredProcedureNames.Usp_Get_InvoiceDetailsForJudgment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                                gr => gr.Read<PTXboInvoiceReport>(),
                                gr => gr.Read<PTXboInvoiceAccount>());
                    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-Judgment successfully finished.");

                }
                //Ends here.
                else if (isOutOfTexas == false)
                {
                 if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-Arbritration inputs:"+ parameters);
                    var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReport_Arbitration, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                                gr => gr.Read<PTXboInvoiceReport>(),
                                gr => gr.Read<PTXboInvoiceAccount>());
                    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-Arbritration successfully finished.");
                }
                else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-Litigation inputs:" + parameters);
                    var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReportLitigation, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                              gr => gr.Read<PTXboInvoiceReport>(),
                              gr => gr.Read<PTXboInvoiceAccount>());
  
                    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-Litigation successfully finished.");
                }
                else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-BPP inputs:" + parameters);
                    var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReportBppRendition, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                              gr => gr.Read<PTXboInvoiceReport>(),
                              gr => gr.Read<PTXboInvoiceAccount>());
                    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-BPP successfully finished.");
                }
                else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-TaxBillAudit inputs:" + parameters);
                    var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReportTaxBill, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                              gr => gr.Read<PTXboInvoiceReport>(),
                              gr => gr.Read<PTXboInvoiceAccount>());
                    invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                    invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-TaxBillAudit successfully finished.");
                }
                //Added by SaravananS. tfs id:59604
                else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.SOAH.GetId())
                {
                        Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-Soah inputs:" + parameters);
                        parameters.Add("@IsRegeneration", invoice.IsRegenerateInvoice); //Added by SaravananS. tfs id:63342
                        var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReport_SOAH, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                                  gr => gr.Read<PTXboInvoiceReport>(),
                                  gr => gr.Read<PTXboInvoiceAccount>());
                        invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                        invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();

                        Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-Soah successfully finished.");
                }
                //Ends here.
                else
                {
                
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard inputs:" + parameters);
                    var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReport, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                          gr => gr.Read<PTXboInvoiceReport>(),
                          gr => gr.Read<PTXboInvoiceAccount>());
                invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard successfully finished.");
                }

                }
                else
               {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard-outoftaxes inputs:" + parameters);
                     //Added by saravanans..tfs id:52726
                     if (isMultiyear == true)
                    {
                        Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard-Multiyear Entry screen inputs:" + parameters);
                        var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForPreview_Multiyear, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                                      gr => gr.Read<PTXboInvoiceReport>(),
                                      gr => gr.Read<PTXboInvoiceAccount>());
                        invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                        invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                        Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard Multiyear Entry screen successfully finished.");
                    }
                    //Ends here..
                    else if(isOTEntryscreen == true)
                    {
                        Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard-outoftaxes Entry screen inputs:" + parameters);
                        var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForPreview_OT, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                                      gr => gr.Read<PTXboInvoiceReport>(),
                                      gr => gr.Read<PTXboInvoiceAccount>());
                        invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                        invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                        Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard outoftaxes Entry screen successfully finished.");
                    }
                   
                    else
                    {
                        var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceDetailsForReport_OT, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                                      gr => gr.Read<PTXboInvoiceReport>(),
                                      gr => gr.Read<PTXboInvoiceAccount>());
                        invoiceReportDetails.InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>();
                        invoiceReportDetails.InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                        Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-standard outoftaxes successfully finished.");
                    }
                }
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-API  ends successfully ");

                return invoiceReportDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceReportDetails-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool UpdateInvoiceProcessingStatus(int invoicingProcessingStatusID, PTXboInvoiceDetails invoiceDetails)
        {

            try
            {
                int invoiceID = 0;
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceProcessingStatus-API  reached " + ((object)invoiceDetails).ToJson(false));
                var ObjInvoice = GetSpecificInvoiceGenerationDetails(invoiceDetails.InvoiceId);
                if (ObjInvoice != null)
                {

                    ObjInvoice.ManuallyGeneratedUserId = invoiceDetails.ManualGeneratedUseriD;
                    ObjInvoice.ManuallyGeneratedUserRoleId = invoiceDetails.ManualGeneratedUserRoleID;
                    if (invoiceDetails.onHold)
                    {
                        ObjInvoice.OnHold = invoiceDetails.onHold;
                        ObjInvoice.OnHoldDate = invoiceDetails.onHoldDate;
                        ObjInvoice.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.InvoiceSkippedForTheCurrentDate.GetId();
                    }
                    else
                    {
                        ObjInvoice.InvoicingProcessingStatusId = invoicingProcessingStatusID;
                    }
                    UpdateInvoiceProcessStatus(ObjInvoice, out invoiceID);

                }

                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceProcessingStatus-API  ends successfully ");

                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceProcessingStatus-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool UpdateInvoiceProcessStatus(PTXboInvoice invoice, out int invoiceID)
        {
            Hashtable parameters = new Hashtable();
            invoiceID = 0;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceProcessStatus-API  reached " + ((object)invoice).ToJson(false));
                parameters.Add("@InvoiceId", invoice.InvoiceID);
                parameters.Add("@ManuallyGeneratedUserId", invoice.ManuallyGeneratedUserId);
                parameters.Add("@ManuallyGeneratedUserRoleId", invoice.ManuallyGeneratedUserRoleId);
                parameters.Add("@OnHold", invoice.OnHold);
                parameters.Add("@OnHoldDate", invoice.OnHoldDate);
                parameters.Add("@InvoicingProcessingStatusId", invoice.InvoicingProcessingStatusId);
               
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_UpdateInvoiceProcessingStatus, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceProcessStatus-API  ends successfully ");
                invoiceID = Convert.ToInt32(result);
                if (invoiceID != 0)
                    return true;
                else
                    return false;
                // return Convert.ToBoolean(result);

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceProcessStatus-API  error " + ((object)ex).ToJson(false)); ;
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public int SaveOrUpdateCorrQ(PTXboCorrQueue corrQueue)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceCorrQ-API  reached " + ((object)corrQueue).ToJson(false));
                parameters.Add("@CorrQID", corrQueue.CorrQID);
                parameters.Add("@ServicePackageID ", corrQueue.ServicePackageID);
                parameters.Add("@ClientID", corrQueue.ClientID);
                parameters.Add("@TaxYear", corrQueue.TaxYear);
                parameters.Add("@CorrProcessingStatusID", corrQueue.CorrProcessingStatusID);
                parameters.Add("@DeliveryMethodID", corrQueue.DeliveryMethodId);
                parameters.Add("@IsSpecificAccount", corrQueue.IsSpecificAccount);
                parameters.Add("@SentToContactAgent", corrQueue.SentToContactAgent);
                parameters.Add("@SentToPrimaryAgent", corrQueue.SentToPrimaryAgent);
                parameters.Add("@SentToSalesAgent", corrQueue.SentToSalesAgent);
                parameters.Add("@Createdby", corrQueue.CreatedBy);
                parameters.Add("@SentToClient", corrQueue.SentToClient);
                parameters.Add("@InvoiceID", corrQueue.LinkFieldValue);
                parameters.Add("@IsCustomDelivery", corrQueue.IsCustomDelivery);
                parameters.Add("@DeliveryAddress", corrQueue.DeliveryAddress);
                parameters.Add("@StartDateTime", DateTime.Now); 
                //Added by SaravananS. tfs id:63611
                parameters.Add("@CCMailAddress", corrQueue.CCMailAddress);
                parameters.Add("@IsResendInvoice", corrQueue.IsResendInvoice);
                //Ends here.
                parameters.Add("@EndDateTime", DateTime.Now);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertcorrq, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateCorrQ-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateINvoiceCorrQ-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public int SaveOrUpdateCorrQCreditLetter(PTXboCorrQueue corrQueue)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateCorrQ-API  reached " + ((object)corrQueue).ToJson(false));
                parameters.Add("@CorrQID", corrQueue.CorrQID);
                parameters.Add("@ServicePackageID ", corrQueue.ServicePackageID);
                parameters.Add("@ClientID", corrQueue.ClientID);
                parameters.Add("@TaxYear", corrQueue.TaxYear);
                parameters.Add("@CorrProcessingStatusID", corrQueue.CorrProcessingStatusID);
                parameters.Add("@DeliveryMethodID", corrQueue.DeliveryMethodId);
                parameters.Add("@IsSpecificAccount", corrQueue.IsSpecificAccount);
                parameters.Add("@SentToContactAgent", corrQueue.SentToContactAgent);
                parameters.Add("@SentToPrimaryAgent", corrQueue.SentToPrimaryAgent);
                parameters.Add("@SentToSalesAgent", corrQueue.SentToSalesAgent);
                parameters.Add("@Createdby", corrQueue.CreatedBy);
                parameters.Add("@SentToClient", corrQueue.SentToClient);
                parameters.Add("@LinkFieldValue", corrQueue.LinkFieldValue);
                parameters.Add("@IsCustomDelivery", corrQueue.IsCustomDelivery);
                parameters.Add("@DeliveryAddress", corrQueue.DeliveryAddress);
                parameters.Add("@StartDateTime", DateTime.Now);
                parameters.Add("@EndDateTime", corrQueue.EndDateTime);
                // parameters.Add("@DescriptionID", corrQueue.Descriptionid);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.USP_InsertCreditletterCorrq, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateCorrQ-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateCorrQ-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool UpdateInvoiceAdjustment(PTXboInvoice invoice, out int invoiceID)
        {
            Hashtable parameters = new Hashtable();
            invoiceID = 0;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceAdjustment-API  reached " + ((object)invoice).ToJson(false));
                parameters.Add("@InvoiceId", invoice.InvoiceID);
                parameters.Add("@GroupId", invoice.GroupId);
                parameters.Add("@ProjectId", invoice.ProjectId);
                parameters.Add("@YealyHearingDetailsId", (invoice.YealyHearingDetailsId == 0) ? invoice.YearlyHearingDetailsId : invoice.YealyHearingDetailsId);
                parameters.Add("@ClientId", invoice.ClientId);
                parameters.Add("@InvoiceTypeId", invoice.InvoiceTypeId);
                parameters.Add("@InvoiceDate", invoice.InvoiceDate);
                parameters.Add("@PaymentDueDate", invoice.PaymentDueDate);
                parameters.Add("@InvoiceGenerationConfigIDUsedForCalculation", invoice.InvoiceGenerationConfigIDUsedForCalculation);
                parameters.Add("@InitialAssessedValue ", invoice.InitialAssessedValue);
                parameters.Add("@FinalAssessedValue", invoice.FinalAssessedValue);
                parameters.Add("@PriorYearTaxRate", invoice.PriorYearTaxRate);
                parameters.Add("@ContingencyPercentage", invoice.ContingencyPercentage);
                parameters.Add("@ContingencyFee", invoice.ContingencyFee);
                parameters.Add("@FlatFee", invoice.FlatFee);
                parameters.Add("@InvoiceAmount", invoice.InvoiceAmount);
                parameters.Add("@CreatedDateAndTime", invoice.CreatedDateAndTime);
                parameters.Add("@SentDateAndTime", invoice.SentDateAndTime);
                parameters.Add("@DeliveryMethodId", invoice.DeliveryMethodId);
                parameters.Add("@DeliveryStatusId", (invoice.DeliveryStatusId == 0) ? (GetClientDefaultDeliveryMethod(invoice.ClientId)!=null && GetClientDefaultDeliveryMethod(invoice.ClientId).DefaultDeliveryTypeId >0?1:4) : invoice.DeliveryStatusId);
                parameters.Add("@AutoGenerated", invoice.AutoGenerated);
                parameters.Add("@ManuallyGeneratedUserId", invoice.ManuallyGeneratedUserId);
                parameters.Add("@ManuallyGeneratedUserRoleId", invoice.ManuallyGeneratedUserRoleId);
                parameters.Add("@OnHold", invoice.OnHold);
                parameters.Add("@OnHoldDate", invoice.OnHoldDate);
                parameters.Add("@TotalAmountPaid", invoice.TotalAmountPaid);
                parameters.Add("@InvoicingStatusId", invoice.InvoicingStatusId);
                parameters.Add("@InvoicingProcessingStatusId", invoice.InvoicingProcessingStatusId);
                parameters.Add("@InvoiceDescription ", invoice.InvoiceDescription);
                parameters.Add("@CompoundInterest", invoice.CompoundInterest);
                parameters.Add("@InvoiceCreditAmount", invoice.InvoiceCreditAmount);
                parameters.Add("@PaymentStatusId", invoice.PaymentStatusId);
                parameters.Add("@InvoiceGroupingTypeId ", invoice.InvoiceGroupingTypeId);
                parameters.Add("@TaxYear", invoice.TaxYear);
                parameters.Add("@Reduction", invoice.Reduction);
                parameters.Add("@TotalEstimatedTaxSavings", invoice.TotalEstimatedTaxSavings);
                parameters.Add("@AmountPaid", invoice.TotalAmountPaid);//
                parameters.Add("@AmountAdjusted", invoice.AmountAdjusted);//
                parameters.Add("@ApplicableInterest", invoice.InterestAmount);
                parameters.Add("@InterestPaid", invoice.InterestPaid);//
                parameters.Add("@InterestAdjusted", invoice.InterestAdjusted);//
                parameters.Add("@AmountDue", invoice.AmountDue);
                parameters.Add("@UpdatedBy", invoice.CreatedBy);
                parameters.Add("@UpdatedDateTime", DateTime.Now);
                parameters.Add("@InterestRateID", invoice.InterestRateID);
                parameters.Add("@IntitalLand", invoice.IntitalLand);
                parameters.Add("@IntialImproved", invoice.IntialImproved);
                parameters.Add("@InitialMarket", invoice.InitialMarket);
                parameters.Add("@FinalLand", invoice.FinalLand);
                parameters.Add("@FinalImproved", invoice.FinalImproved);
                parameters.Add("@FinalMarket", invoice.FinalMarket);
                parameters.Add("@CreditAmountApplied", invoice.InvoiceCreditAmount);
                parameters.Add("@isInvoiceDefect", invoice.isInvoiceDefect);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_UpdateInvoiceAdjustment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceAdjustment-API  ends successfully ");
                invoiceID = Convert.ToInt32(result);
                if (invoiceID != 0)
                    return true;
                else
                    return false;
                // return Convert.ToBoolean(result);

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceAdjustment-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public bool SaveOrUpdateInvoice(PTXboInvoice invoice, out int invoiceID)
        {
            Hashtable parameters = new Hashtable();
            invoiceID = 0;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoice-API  reached " + ((object)invoice).ToJson(false));
                parameters.Add("@InvoiceId", invoice.InvoiceID);
                parameters.Add("@GroupId", invoice.GroupId);
                parameters.Add("@ProjectId", invoice.ProjectId);
                parameters.Add("@YealyHearingDetailsId", (invoice.YealyHearingDetailsId == 0) ? invoice.YearlyHearingDetailsId : invoice.YealyHearingDetailsId);
                parameters.Add("@ClientId", invoice.ClientId);
                parameters.Add("@InvoiceTypeId", invoice.InvoiceTypeId);
                parameters.Add("@InvoiceDate", invoice.InvoiceDate);
                parameters.Add("@PaymentDueDate", invoice.PaymentDueDate);
                parameters.Add("@InvoiceGenerationConfigIDUsedForCalculation", invoice.InvoiceGenerationConfigIDUsedForCalculation);
                parameters.Add("@InitialAssessedValue ", invoice.InitialAssessedValue);
                parameters.Add("@FinalAssessedValue", invoice.FinalAssessedValue);
                parameters.Add("@PriorYearTaxRate", invoice.PriorYearTaxRate);
                parameters.Add("@ContingencyPercentage", invoice.ContingencyPercentage<1? invoice.ContingencyPercentage*100: invoice.ContingencyPercentage);
                parameters.Add("@ContingencyFee", invoice.ContingencyFee);
                parameters.Add("@FlatFee", invoice.FlatFee);
                parameters.Add("@InvoiceAmount",((Convert.ToDecimal(invoice.InvoiceCapValue)<invoice.InvoiceAmount&& invoice.InvoiceCapValue>0) ? Convert.ToDecimal(invoice.InvoiceCapValue): invoice.InvoiceAmount));// invoice.FlatFee>0? invoice.InvoiceAmount:
                parameters.Add("@CreatedDateAndTime", invoice.CreatedDateAndTime);
                parameters.Add("@SentDateAndTime", invoice.SentDateAndTime);
                parameters.Add("@DeliveryMethodId", invoice.DeliveryMethodId);
                parameters.Add("@DeliveryStatusId", (invoice.DeliveryStatusId == 0) ? (GetClientDefaultDeliveryMethod(invoice.ClientId)!=null && GetClientDefaultDeliveryMethod(invoice.ClientId).DefaultDeliveryTypeId>0?1:4) : invoice.DeliveryStatusId);
                parameters.Add("@AutoGenerated", invoice.AutoGenerated);
                parameters.Add("@ManuallyGeneratedUserId", invoice.ManuallyGeneratedUserId);
                parameters.Add("@ManuallyGeneratedUserRoleId", invoice.ManuallyGeneratedUserRoleId);
                parameters.Add("@OnHold", invoice.OnHold);
                parameters.Add("@OnHoldDate", invoice.OnHoldDate);
                parameters.Add("@TotalAmountPaid", invoice.TotalAmountPaid);
                parameters.Add("@InvoicingStatusId", invoice.InvoicingStatusId);
                parameters.Add("@InvoicingProcessingStatusId", invoice.InvoicingProcessingStatusId!=0? invoice.InvoicingProcessingStatusId: invoice.InvoicingProcessingStatus);
                parameters.Add("@InvoiceDescription ", invoice.InvoiceDescription);
                parameters.Add("@CompoundInterest", invoice.CompoundInterest);
                parameters.Add("@InvoiceCreditAmount", invoice.InvoiceCreditAmount);
                parameters.Add("@PaymentStatusId", invoice.PaymentStatusId);
                parameters.Add("@InvoiceGroupingTypeId ", invoice.InvoiceGroupingTypeId);
                parameters.Add("@TaxYear", invoice.TaxYear);
                parameters.Add("@Reduction", invoice.Reduction);
                parameters.Add("@TotalEstimatedTaxSavings", invoice.TotalEstimatedTaxSavings);
                parameters.Add("@AmountPaid", invoice.TotalAmountPaid);
                parameters.Add("@AmountAdjusted", invoice.AmountAdjusted);
                parameters.Add("@ApplicableInterest", invoice.InterestAmount);
                parameters.Add("@InterestPaid", invoice.InterestPaid);
                parameters.Add("@InterestAdjusted", invoice.InterestAdjustment);
                parameters.Add("@AmountDue",((invoice.TotalAmountPaid+ invoice.AmountAdjusted==null|| invoice.TotalAmountPaid + invoice.AmountAdjusted ==0? (Convert.ToDecimal(invoice.InvoiceCapValue) < invoice.InvoiceAmount && invoice.InvoiceCapValue > 0 ? Convert.ToDecimal(invoice.InvoiceCapValue):invoice.InvoiceAmount):invoice.AmountDue))); // changed by Arun// invoice.FlatFee > 0 ? invoice.InvoiceAmount : 
                parameters.Add("@UpdatedBy", invoice.CreatedBy);
                parameters.Add("@UpdatedDateTime", DateTime.Now);
                parameters.Add("@InterestRateID", invoice.InterestRateID);
                parameters.Add("@IntitalLand", invoice.IntitalLand);
                parameters.Add("@IntialImproved", invoice.IntialImproved);
                parameters.Add("@InitialMarket", invoice.InitialMarket);
                parameters.Add("@FinalLand", invoice.FinalLand);
                parameters.Add("@FinalImproved", invoice.FinalImproved);
                parameters.Add("@FinalMarket", invoice.FinalMarket);
                parameters.Add("@CreditAmountApplied", invoice.InvoiceCreditAmount);
                parameters.Add("@isInvoiceDefect", invoice.isInvoiceDefect);
                parameters.Add("@InvoiceCapValue", invoice.InvoiceCapValue);
                parameters.Add("@IsRegenerateInvoice", invoice.IsRegenerateInvoice);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoice-API  ends successfully ");
                invoiceID = Convert.ToInt32(result);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoice-Invoiceid " + result);
                if (invoiceID != 0)
                    return true;
                else
                    return false;
                // return Convert.ToBoolean(result);

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoice-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboClientDeliveryMethodId GetClientDefaultDeliveryMethod(int clientId)
        {
            Hashtable parameters = new Hashtable();
            PTXboClientDeliveryMethodId clientDeliveryMethodId = new PTXboClientDeliveryMethodId();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetClientDefaultDeliveryMethod-API  reached " + ((object)"clientId=" + clientId.ToString()).ToJson(false));
                parameters.Add("@clientID", clientId);
                clientDeliveryMethodId = _dapperConnection.Select<PTXboClientDeliveryMethodId>(StoredProcedureNames.usp_GetClientDefaultDeliveryMethod, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetClientDefaultDeliveryMethod-API  ends successfully ");

               return clientDeliveryMethodId;
                                
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetClientDefaultDeliveryMethod-API  error " + ((object)ex).ToJson(false));
                throw ex;                
            }
            finally
            {
                Dispose();
            }
        }
        public List<PTXboInvoicePaymentMap> GetInvoicePaymentMap(int invoiceID)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoicePaymentMap-API  reached " + ((object)"invoiceID=" + invoiceID.ToString()).ToJson(false));
                parameters.Add("@InvoiceID", invoiceID);
                var result = _dapperConnection.Select<PTXboInvoicePaymentMap>(StoredProcedureNames.usp_get_InvoicePaymentMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("Invoice Calculation-GetInvoicePaymentMap-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoicePaymentMap-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboPayment GetInvoicePayment(int paymentId, string paymentDescription)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoicePayment-API  reached " + ((object)"paymentId=" + paymentId.ToString() + "paymentDescription=" + paymentDescription).ToJson(false));
                parameters.Add("@PaymentId", paymentId);
                parameters.Add("@PaymentDescription", paymentDescription);
                var result = _dapperConnection.Select<PTXboPayment>(StoredProcedureNames.usp_get_InvoicePayment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetInvoicePayment-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoicePayment-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public int SaveOrUpdatePayment(PTXboPayment payment)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdatePayment-API  reached " + ((object)payment).ToJson(false));
                parameters.Add("@PaymentId", payment.PaymentId);
                parameters.Add("@ClientId", payment.ClientId);
                parameters.Add("@PaymentDescription", payment.PaymentDescription);
                parameters.Add("@PaymentAmount", payment.PaymentAmount);
                parameters.Add("@InterestAmount", payment.InterestAmount);
                parameters.Add("@PaymentReceivedDate", payment.PaymentReceivedDate);
                parameters.Add("@PostedDate", payment.PostedDate);
                parameters.Add("@InvoicePaymentMethodId", payment.InvoicePaymentMethodId);
                parameters.Add("@PaymentTypeID", payment.PaymentTypeID);
                parameters.Add("@PropertyTaxInvoiceID", payment.PropertyTaxInvoiceID);
                parameters.Add("@PropertyTaxPaymentTypeID", payment.PropertyTaxPaymentTypeID);
                parameters.Add("@CreatedBy", payment.CreatedBy);
                parameters.Add("@CreatedDateTime", payment.CreatedDateTime);
                parameters.Add("@UpdatedBy", payment.UpdatedBy);
                parameters.Add("@UpdatedDateTime", payment.UpdatedDateTime);
                parameters.Add("@CheckNumber", payment.CheckNumber);
                parameters.Add("@BatchNumber", payment.BatchNumber);
                parameters.Add("@CreditToInvoiceid", payment.CreditToInvoiceid);
                parameters.Add("@IsLegalPayment", payment.IsLegalPayment);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdatePayment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdatePayment-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdatePayment-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SaveOrUpdateInvoicePaymentMap(PTXboInvoicePaymentMap paymentInvoiceMap)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-Invoice Calculation-SaveOrUpdateInvoicePaymentMap-API  reached " + ((object)paymentInvoiceMap).ToJson(false));
                parameters.Add("@InvoicePaymentMapId", paymentInvoiceMap.InvoicePaymentMapId);
                parameters.Add("@InvoiceId", paymentInvoiceMap.InvoiceId);
                parameters.Add("@PaymentId", paymentInvoiceMap.PaymentId);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateInvoicePaymentMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-Invoice Calculation-SaveOrUpdateInvoicePaymentMap-API  ends successfully ");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoicePaymentMap-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool SubmitCapValueAdjustment(PTXboPayment ObjPayment, int invoiceID, out string errorString)
        {
            errorString = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitCapValueAdjustment-API  reached " + ((object)ObjPayment + "invoiceID=" + invoiceID.ToString()).ToJson(false));
                if (ObjPayment != null && ObjPayment.PaymentId == 0)
                {
                    PTXboPayment PrincipalAdjPayment = new PTXboPayment();
                    PrincipalAdjPayment.PostedDate = DateTime.Now;
                    PrincipalAdjPayment.PaymentDescription = ObjPayment.PaymentDescription;
                    PrincipalAdjPayment.PaymentAmount = ObjPayment.PaymentAmount;
                    PrincipalAdjPayment.InvoicePaymentMethodId = (ObjPayment.InvoicePaymentMethodId == 0) ? 0 : ObjPayment.InvoicePaymentMethodId;
                    PrincipalAdjPayment.InterestAmount = ObjPayment.InterestAmount;
                    PrincipalAdjPayment.ClientId = (ObjPayment.ClientId == 0) ? 0 : ObjPayment.ClientId;
                    PrincipalAdjPayment.CreatedBy = ObjPayment.CreatedBy;
                    PrincipalAdjPayment.CreatedDateTime = ObjPayment.CreatedDateTime;
                    PrincipalAdjPayment.UpdatedBy = ObjPayment.UpdatedBy;
                    PrincipalAdjPayment.UpdatedDateTime = ObjPayment.UpdatedDateTime;
                    ObjPayment.PaymentId= SaveOrUpdatePayment(PrincipalAdjPayment);
                    PTXboInvoicePaymentMap PaymentInvoiceMap = new PTXboInvoicePaymentMap();
                    PaymentInvoiceMap.InvoiceId = invoiceID;
                    //PaymentInvoiceMap.PaymentId = (PrincipalAdjPayment.PaymentId == 0) ? 0 : PrincipalAdjPayment.PaymentId;
                    PaymentInvoiceMap.PaymentId = (PrincipalAdjPayment.PaymentId == 0) ? ObjPayment.PaymentId : PrincipalAdjPayment.PaymentId;
                    SaveOrUpdateInvoicePaymentMap(PaymentInvoiceMap);

                }
                else
                {
                    PTXboPayment PrincipalAdjPayment = ObjPayment;
                    PrincipalAdjPayment.PostedDate = DateTime.Now;
                    PrincipalAdjPayment.PaymentDescription = ObjPayment.PaymentDescription;
                    PrincipalAdjPayment.PaymentAmount = ObjPayment.PaymentAmount;
                    PrincipalAdjPayment.InvoicePaymentMethodId = (ObjPayment.InvoicePaymentMethodId == 0) ? 0 : ObjPayment.InvoicePaymentMethodId;
                    PrincipalAdjPayment.InterestAmount = ObjPayment.InterestAmount;
                    PrincipalAdjPayment.ClientId = (ObjPayment.ClientId == 0) ? 0 : ObjPayment.ClientId;
                    PrincipalAdjPayment.CreatedBy = ObjPayment.CreatedBy;
                    PrincipalAdjPayment.CreatedDateTime = ObjPayment.CreatedDateTime;
                    PrincipalAdjPayment.UpdatedBy = ObjPayment.UpdatedBy;
                    PrincipalAdjPayment.UpdatedDateTime = ObjPayment.UpdatedDateTime;
                    PrincipalAdjPayment.PaymentId= SaveOrUpdatePayment(PrincipalAdjPayment);

                }
                Logger.For(this).Invoice("Invoice Calculation-SubmitCapValueAdjustment-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitCapValueAdjustment-API  error " + ((object)ex).ToJson(false));
                errorString = ex.Message;
                return false;
            }
            finally
            {
                Dispose();
            }
        }

       



        public bool UpdateAmountDueCapValueChange(int invoiceID, out string errorString)
        {
            Hashtable parameters = new Hashtable();
            errorString = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateAmountDueCapValueChange-API  reached " + ((object)invoiceID).ToJson(false));
                parameters.Add("@invoiceID", invoiceID);
                parameters.Add("@IsMainScreen", false);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_UpdateInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-UpdateAmountDueCapValueChange-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateAmountDueCapValueChange-API  error " + ((object)ex).ToJson(false));
                errorString = ex.Message;
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool RemoveCapvalueAdjustment(int invoiceID)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-RemoveCapvalueAdjustment-API  reached " + ((object)invoiceID).ToJson(false));
                parameters.Add("@InvoiceId", invoiceID);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_RemoveCapvalueAdjustment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-RemoveCapvalueAdjustment-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-RemoveCapvalueAdjustment-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool AccountLevelInvoiceGeneration(List<PTXboInvoiceReport> getlstInvoiceDetails, List<PTXboInvoiceAccount> lstAccountDetails, int updatedBy)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-AccountLevelInvoiceGeneration-API  reached " + ((object)getlstInvoiceDetails).ToJson(false));
                bool isReturn = false;
                string errorMessage = string.Empty;
                decimal InvoiceCapValue = 0;
                decimal InvoiceCalAmount = 0;
                foreach (PTXboInvoiceReport objInv in getlstInvoiceDetails)
                {
                    //bool isSuccessCapValue = false;
                    //List<PTXboInvoicePaymentMap> lstInvoicePaymentMap;
                    var objInvoice = GetSpecificInvoiceGenerationDetails(objInv.InvoiceId);
                    InvoiceCapValue = Convert.ToDecimal(objInv.InvoiceCapValue);
                    if (objInvoice != null)
                    {
                        int invoiceID = 0;
                        objInvoice.InitialAssessedValue = Convert.ToDecimal(objInv.Total_Initial_Assessed_Value);
                        objInvoice.FinalAssessedValue = Convert.ToDecimal(objInv.Total_Final_Assessed_Value);
                        objInvoice.Reduction = Convert.ToDecimal((objInv.Total_Initial_Assessed_Value) - (objInv.Total_Final_Assessed_Value));
                        objInvoice.PriorYearTaxRate = Math.Round(Convert.ToDouble(objInv.Prior_Year_TaxRate * 100), 8); //
                        
                        if (objInvoice.Reduction > 0)
                        {
                            objInvoice.TotalEstimatedTaxSavings = Math.Round(Convert.ToDecimal((objInvoice.PriorYearTaxRate / 100)) * Convert.ToDecimal((objInvoice.Reduction)), 2);
                            objInvoice.ContingencyPercentage = objInv.Contingency<1? (objInv.Contingency * 100): objInv.Contingency; // Added by saravanans -to change decimal to int
                            //objInvoice.ContingencyPercentage = objInv.Contingency / 100;
                            objInvoice.ContingencyFee = Math.Round(Convert.ToDecimal((objInvoice.TotalEstimatedTaxSavings) * Convert.ToDecimal((objInv.Contingency / 100))), 2);
                            //objInvoice.InvoiceAmount = Math.Round(objInvoice.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee, 2);
                            InvoiceAmountCalculation(Convert.ToDecimal(objInvoice.ContingencyFee.GetValueOrDefault()), objInv.Flat_fee, InvoiceCapValue, out InvoiceCalAmount);
                            objInvoice.InvoiceAmount = InvoiceCalAmount;// Math.Round(InvoiceCalAmount + objInv.Flat_fee, 2);
                            objInvoice.FlatFee = objInv.Flat_fee;
                            objInvoice.InvoiceCapValue = objInv.CapValue;//added by saravanans.
                            isReturn = SaveOrUpdateInvoice(objInvoice, out invoiceID);
                            objInvoice.InvoiceID = invoiceID;
                            //if (objInv.CapValue > 0 && Convert.ToDecimal((objInv.Contingency / 100)) > 0 && Math.Round(objInvoice.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee, 2) > Convert.ToDecimal(objInv.CapValue))
                            //{
                            //    lstInvoicePaymentMap = GetInvoicePaymentMap(objInv.InvoiceId);
                            //    if (lstInvoicePaymentMap != null && lstInvoicePaymentMap.Count > 0)
                            //    {
                            //        PTXboPayment Objpayment;
                            //        foreach (PTXboInvoicePaymentMap obiInvPay in lstInvoicePaymentMap)
                            //        {
                            //            var lstPayment = GetInvoicePayment(obiInvPay.PaymentId, "Cap Value Adjustment");
                            //            if (lstPayment != null)
                            //            {
                            //                Objpayment = lstPayment;
                            //            }
                            //            else
                            //            {
                            //                Objpayment = new PTXboPayment();
                            //            }
                            //            decimal PaymentAmount = 0;
                            //            PaymentAmount = Math.Round((objInvoice.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee) - Convert.ToDecimal(objInv.CapValue), 2);
                            //            Objpayment.ClientId = objInv.ClientId;
                            //            Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                            //            Objpayment.PaymentAmount = PaymentAmount;
                            //            Objpayment.PaymentDescription = "Cap Value Adjustment";
                            //            Objpayment.CreatedBy = updatedBy;
                            //            Objpayment.CreatedDateTime = DateTime.Now;
                            //            Objpayment.UpdatedBy = updatedBy;
                            //            Objpayment.UpdatedDateTime = DateTime.Now;
                            //            isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, objInv.InvoiceId, out errorMessage);
                            //            if (isSuccessCapValue)
                            //            {
                            //                bool isSuccess = UpdateAmountDueCapValueChange(objInvoice.InvoiceID, out errorMessage);
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        PTXboPayment Objpayment = new PTXboPayment();
                            //        decimal PaymentAmount = 0;
                            //        PaymentAmount = Math.Round((objInvoice.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee) - Convert.ToDecimal(objInv.CapValue), 2);
                            //        Objpayment.ClientId = objInv.ClientId;
                            //        Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                            //        Objpayment.PaymentAmount = PaymentAmount;
                            //        Objpayment.PaymentDescription = "Cap Value Adjustment";
                            //        Objpayment.CreatedBy = updatedBy;
                            //        Objpayment.CreatedDateTime = DateTime.Now;
                            //        Objpayment.UpdatedBy = updatedBy;
                            //        Objpayment.UpdatedDateTime = DateTime.Now;
                            //        isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, objInv.InvoiceId, out errorMessage);
                            //        if (isSuccessCapValue)
                            //        {
                            //            bool isSuccess = UpdateAmountDueCapValueChange(objInvoice.InvoiceID, out errorMessage);
                            //        }
                            //    }
                            //}
                            if (Convert.ToDecimal(objInvoice.AmountAdjusted) != 0)
                            {
                                RemoveCapvalueAdjustment(objInvoice.InvoiceID);
                                bool isSuccess = UpdateAmountDueCapValueChange(objInvoice.InvoiceID, out errorMessage);
                            }
                        }
                    }
                }
                Logger.For(this).Invoice("Invoice Calculation-AccountLevelInvoiceGeneration-API  ends successfully ");
                return isReturn;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-AccountLevelInvoiceGeneration-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool ProjectorTermInvoiceGeneration(List<PTXboInvoiceReport> getlstInvoiceDetails, List<PTXboInvoiceAccount> lstAccountDetails, int updatedBy)
        {

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-ProjectorTermInvoiceGeneration-API  reached " + ((object)getlstInvoiceDetails).ToJson(false));
                bool isReturn = false;
                string errorMessage = string.Empty;
                PTXboInvoice objInsertInvoiceData = new PTXboInvoice();
                decimal InvoiceCapValue = 0;
                decimal InvoiceCalAmount = 0;

                foreach (PTXboInvoiceReport objInv in getlstInvoiceDetails)
                {
                    //bool isSuccessCapValue = false;
                    //List<PTXboInvoicePaymentMap> lstInvoicePaymentMap;
                    var objInvoice = GetSpecificInvoiceGenerationDetails(objInv.InvoiceId);
                    InvoiceCapValue = Convert.ToDecimal(objInv.InvoiceCapValue);
                    if (objInvoice != null)
                    {
                        decimal TotalNoticedValue = 0;
                        decimal TotalPostHearingValue = 0;
                        decimal TotalReduction = 0;
                        double TotalPriorYearTaxRate = 0;
                        decimal TotalEstimatedTaxSavings = 0;
                        decimal? TotalInvoiceAmount = 0;
                        decimal Reduction = 0;
                        decimal? ContingencyFee = 0;
                        decimal? InvoiceAmount = 0;
                        int invoiceID = 0;
                        foreach (PTXboInvoiceAccount lstAccount in lstAccountDetails)
                        {
                            TotalNoticedValue = TotalNoticedValue + Convert.ToDecimal(lstAccount.NoticedValue);
                            TotalPostHearingValue = TotalPostHearingValue + Convert.ToDecimal(lstAccount.FinalValue);
                            TotalPriorYearTaxRate = TotalPriorYearTaxRate + Convert.ToDouble(lstAccount.PrevYearTaxRate);
                            TotalReduction = (TotalNoticedValue - TotalPostHearingValue); //Modified By Pavithra.B on 31Aug2016 - calculating Reduction value from TotalNoticedValue and TotalPostHearingValue                                
                            Reduction = Convert.ToDecimal(lstAccount.NoticedValue) - Convert.ToDecimal(lstAccount.FinalValue);
                            if (TotalReduction > 0)
                            {
                                lstAccount.Estimated_Tax_Savings = (Convert.ToDouble(Convert.ToDouble(lstAccount.PrevYearTaxRate))) * Convert.ToDouble((Reduction));
                                TotalEstimatedTaxSavings = TotalEstimatedTaxSavings + (Convert.ToDecimal(Convert.ToDouble(lstAccount.PrevYearTaxRate))) * Convert.ToDecimal((Reduction));
                                ContingencyFee = (TotalEstimatedTaxSavings) * Convert.ToDecimal((objInv.Contingency / 100));

                                if ((objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() ||
                                            objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                                            && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
                                //if (objInvoiceid.InvoiceType.Termstypeid == Enumerators.PTXenumInvoiceType.Standard.GetId() && objInvoiceid.InvoiceGroupType.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
                                {
                                    //Modified by Pavithra.B on 3Nov2016 - TFS Id-26636
                                    InvoiceAmount = (Convert.ToDecimal(lstAccount.Estimated_Tax_Savings) * Convert.ToDecimal((objInv.Contingency / 100)));
                                    TotalInvoiceAmount = TotalInvoiceAmount + InvoiceAmount;
                                }
                                else if ((objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() ||
                                                objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                                                && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
                                {
                                    //Modified by Pavithra.B on 3Nov2016 - TFS Id-26636
                                    InvoiceAmount = (Convert.ToDecimal(lstAccount.Estimated_Tax_Savings) * Convert.ToDecimal((objInv.Contingency / 100)));
                                    TotalInvoiceAmount = TotalInvoiceAmount + InvoiceAmount;
                                }
                                //else if(objInvoiceid.InvoiceType.Termstypeid == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId() ||
                                //   objInvoiceid.InvoiceType.Termstypeid == Enumerators.PTXenumInvoiceType.BPP.GetId())
                                //{
                                //    InvoiceAmount= Convert.ToDecimal(objInv.Flat_fee);
                                //    TotalInvoiceAmount = InvoiceAmount;
                                //}
                                objInsertInvoiceData.InvoiceID = objInv.InvoiceId;
                                objInsertInvoiceData.TotalEstimatedTaxSaving = Convert.ToDouble(TotalEstimatedTaxSavings);
                                objInsertInvoiceData.ContingencyFee = ContingencyFee;
                                objInsertInvoiceData.InvoiceAmount = TotalInvoiceAmount;
                                objInsertInvoiceData.InitialAssessedValue = TotalNoticedValue;
                                objInsertInvoiceData.FinalAssessedValue = TotalPostHearingValue;
                                objInsertInvoiceData.PriorYearTaxRate = TotalPriorYearTaxRate;
                                objInsertInvoiceData.Reduction = TotalReduction;
                                objInsertInvoiceData.FlatFee = objInv.Flat_fee;
                            }
                        }
                        //Added by Pavithra.B on 3Nov2016 - TFS Id-26636
                        //objInsertInvoiceData.InvoiceAmount = Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount) + Convert.ToDecimal(objInv.Flat_fee);
                        InvoiceAmountCalculation(Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount), objInv.Flat_fee, InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680

                        objInsertInvoiceData.InvoiceAmount = InvoiceCalAmount;// + Convert.ToDecimal(objInv.Flat_fee);
                        objInvoice.InitialAssessedValue = Convert.ToDecimal(objInsertInvoiceData.InitialAssessedValue);
                        objInvoice.FinalAssessedValue = Convert.ToDecimal(objInsertInvoiceData.FinalAssessedValue);
                        objInvoice.Reduction = Convert.ToDecimal(objInsertInvoiceData.Reduction);
                        objInvoice.PriorYearTaxRate = Math.Round(Convert.ToDouble(objInsertInvoiceData.PriorYearTaxRate * 100), 8);
                        if (objInvoice.Reduction > 0)
                        {
                            objInvoice.TotalEstimatedTaxSavings = Math.Round(Convert.ToDecimal(objInsertInvoiceData.TotalEstimatedTaxSaving), 2);
                            objInvoice.ContingencyFee = Math.Round(Convert.ToDecimal(objInsertInvoiceData.ContingencyFee), 2);
                            objInvoice.ContingencyPercentage = objInv.Contingency / 100;
                            objInvoice.InvoiceAmount = Math.Round(Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount), 2);
                            objInvoice.FlatFee = objInv.Flat_fee;
                            objInvoice.InvoiceCapValue = objInv.CapValue;//added by saravanans.
                            SaveOrUpdateInvoice(objInvoice, out invoiceID);
                            objInvoice.InvoiceID = invoiceID;
                            //if (objInv.CapValue > 0 && Convert.ToDecimal((objInv.Contingency / 100)) > 0 && Math.Round(objInvoice.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee, 2) > Convert.ToDecimal(objInv.CapValue))
                            //{
                            //    lstInvoicePaymentMap = GetInvoicePaymentMap(objInv.InvoiceId);
                            //    if (lstInvoicePaymentMap != null && lstInvoicePaymentMap.Count > 0)
                            //    {
                            //        PTXboPayment Objpayment;
                            //        foreach (PTXboInvoicePaymentMap obiInvPay in lstInvoicePaymentMap)
                            //        {
                            //            var lstPayment = GetInvoicePayment(obiInvPay.PaymentId, "Cap Value Adjustment");
                            //            if (lstPayment != null)
                            //            {
                            //                Objpayment = lstPayment;
                            //            }
                            //            else
                            //            {
                            //                Objpayment = new PTXboPayment();
                            //            }
                            //            decimal PaymentAmount = 0;
                            //            PaymentAmount = Math.Round((objInvoice.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee) - Convert.ToDecimal(objInv.CapValue), 2);
                            //            Objpayment.ClientId = objInv.ClientId;
                            //            Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                            //            Objpayment.PaymentAmount = PaymentAmount;
                            //            Objpayment.PaymentDescription = "Cap Value Adjustment";
                            //            Objpayment.CreatedBy = updatedBy;
                            //            Objpayment.CreatedDateTime = DateTime.Now;
                            //            Objpayment.UpdatedBy = updatedBy;
                            //            Objpayment.UpdatedDateTime = DateTime.Now;
                            //            isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, objInv.InvoiceId, out errorMessage);
                            //            if (isSuccessCapValue)
                            //            {
                            //                bool isSuccess = UpdateAmountDueCapValueChange(objInvoice.InvoiceID, out errorMessage);
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        PTXboPayment Objpayment = new PTXboPayment();
                            //        decimal PaymentAmount = 0;
                            //        PaymentAmount = Math.Round((objInvoice.ContingencyFee.GetValueOrDefault() + objInv.Flat_fee) - Convert.ToDecimal(objInv.CapValue), 2);
                            //        Objpayment.ClientId = objInv.ClientId;
                            //        Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                            //        Objpayment.PaymentAmount = PaymentAmount;
                            //        Objpayment.PaymentDescription = "Cap Value Adjustment";
                            //        Objpayment.CreatedBy = updatedBy;
                            //        Objpayment.CreatedDateTime = DateTime.Now;
                            //        Objpayment.UpdatedBy = updatedBy;
                            //        Objpayment.UpdatedDateTime = DateTime.Now;
                            //        isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, objInv.InvoiceId, out errorMessage);
                            //        if (isSuccessCapValue)
                            //        {
                            //            bool isSuccess = UpdateAmountDueCapValueChange(objInvoice.InvoiceID, out errorMessage);
                            //        }
                            //    }
                            //}
                            if (Convert.ToDecimal(objInvoice.AmountAdjusted) != 0)
                            {
                                RemoveCapvalueAdjustment(objInvoice.InvoiceID);
                                bool isSuccess = UpdateAmountDueCapValueChange(objInvoice.InvoiceID, out errorMessage);
                            }
                        }
                    }
                }
                Logger.For(this).Invoice("Invoice Calculation-ProjectorTermInvoiceGeneration-API  ends successfully ");
                return isReturn;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-ProjectorTermInvoiceGeneration-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// This method is used to bind  the Invoice summary data with the appropriate field and insert invoice summary.-added by saravanans-tfs:47247
        /// </summary>
        /// <param name="invoicesummary"></param>
        /// <param name="invoiceSummaryProcessingStatusID"></param>
        public int InsertInvoiceSummaryWithProcessingStatus(PTXboInvoiceSummary invoiceSummary, PTXboInvoice objInvoice, out string errorString)
        {
            invoiceSummary.YearlyHearingDetailsID = objInvoice.YearlyHearingDetailsId;
            invoiceSummary.GroupId = objInvoice.GroupId;
            invoiceSummary.ProjectID = objInvoice.ProjectId;// == null ? 0 : boInvoice.ProjectId ;
            invoiceSummary.ClientId = objInvoice.ClientId;
            //Added by Pavithra.B on 17Nov2015 - Arbitration
            errorString = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceSummaryWithProcessingStatus-API  reached " + ((object)invoiceSummary).ToJson(false));
                if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                {
                    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.ArbitrationLegalFees.GetId();
                }
                else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
                {
                    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.LitigationLegalFees.GetId();
                }
                else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
                {
                    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.BPPrenditionFees.GetId();
                }
                else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                {
                    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.TaxBillAuditFees.GetId();
                }
                else
                {
                    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.HearingProcessFees.GetId();
                }
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceSummaryWithProcessingStatus.objInvoice:"+ objInvoice.InvoiceID+".IsSpecialTerm:"+ objInvoice.IsSpecialTerm+ " objInvoice.CanGenerateInvoice:"+ objInvoice.CanGenerateInvoice+ " objInvoice.DontGenerateInvoiceFlag:"+ objInvoice.DontGenerateInvoiceFlag);
                if (objInvoice.IsSpecialTerm == true && objInvoice.CanGenerateInvoice == false && objInvoice.DontGenerateInvoiceFlag == true)
                {
                    invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.SpecialtermFees.GetId();
                    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.Specialterminvoicingnotgeneratedforaccount.GetId();
                    //Added by Saravanans.tfs id:54969
                    if(objInvoice.InvoiceID >0)
                    {
                        var invoice = GetInvoiceDetailsById(objInvoice.InvoiceID).FirstOrDefault();
                        invoice.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.DoNotGenerateInvoice.GetId();
                        int tempInvoiceid = 0;
                        UpdateInvoiceProcessStatus(invoice,out tempInvoiceid);
                        UpdateInvoiceStatus(objInvoice.InvoiceID, Enumerators.PTXenumInvoiceStatus.DoNotGenerateInvoice.GetId());
                    }
                   //Ends here
                }
                invoiceSummary.InvoiceGenerated = true;
                invoiceSummary.InvoiceID = objInvoice.InvoiceID;
                invoiceSummary.InvoicesummaryID = SubmitInvoiceSummary(invoiceSummary);
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceSummaryWithProcessingStatus-API  ends successfully ");
                return invoiceSummary.InvoicesummaryID;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceSummaryWithProcessingStatus-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboInvoiceSummary GetSpecificInvoiceSummary(int invoicesummaryID)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetSpecificInvoiceSummary-API  reached " + ((object)invoicesummaryID).ToJson(false));
                parameters.Add("@InvoicesummaryID", invoicesummaryID);

                var result = _dapperConnection.Select<PTXboInvoiceSummary>(StoredProcedureNames.GetSpecificInvoiceSummary, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetSpecificInvoiceSummary-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetSpecificInvoiceSummary-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public int SaveOrUpdateInvoiceSummary(PTXboInvoiceSummary invoiceSummary)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceSummary-API  reached " + ((object)invoiceSummary).ToJson(false));
                if(invoiceSummary.YearlyHearingDetailsID>0)
                {

                
                parameters.Add("@InvoiceSummaryId", invoiceSummary.InvoicesummaryID);
                parameters.Add("@YearlyHearingDetailsId", invoiceSummary.YearlyHearingDetailsID);
                parameters.Add("@InvoiceGenerated", invoiceSummary.InvoiceGenerated);
                parameters.Add("@InvoiceGeneratedForId", invoiceSummary.InvoiceGeneratedForID);
                parameters.Add("@InvoiceStatusId", invoiceSummary.InvoiceStatusID);
                parameters.Add("@InvoiceSummaryProcessingStatusId", invoiceSummary.InvoiceSummaryProcessingStatusID);
                parameters.Add("@GroupId", invoiceSummary.GroupId);
                parameters.Add("@ClientId", invoiceSummary.ClientId);
                parameters.Add("@ProjectId", invoiceSummary.ProjectID);
                parameters.Add("@CreateDateTime", DateTime.Now);//invoiceSummary.CreateDateTime
                parameters.Add("@InvoiceID", invoiceSummary.InvoiceID);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_SaveOrUpdateInvoiceSummary, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceSummary-API  ends successfully ");
                    return Convert.ToInt32(result);
                }
                else
                {
                    return Convert.ToInt32(invoiceSummary.YearlyHearingDetailsID);
                }
                
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceSummary-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        
        public int SubmitInvoiceSummary(PTXboInvoiceSummary invoiceSummary)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceSummary-API  reached " + ((object)invoiceSummary).ToJson(false));
                var objAccountInvoiceSummary = GetSpecificInvoiceSummary(invoiceSummary.InvoicesummaryID);
                if (objAccountInvoiceSummary == null)
                {
                    objAccountInvoiceSummary = new PTXboInvoiceSummary();
                }
                objAccountInvoiceSummary.YearlyHearingDetailsID = invoiceSummary.YearlyHearingDetailsID;
                objAccountInvoiceSummary.InvoiceGenerated = invoiceSummary.InvoiceGenerated;
                objAccountInvoiceSummary.InvoiceGeneratedForID = invoiceSummary.InvoiceGeneratedForID;
                objAccountInvoiceSummary.InvoiceStatusID = invoiceSummary.InvoiceStatusID;
                objAccountInvoiceSummary.InvoiceSummaryProcessingStatusID = invoiceSummary.InvoiceSummaryProcessingStatusID;
                objAccountInvoiceSummary.GroupId = invoiceSummary.GroupId;
                objAccountInvoiceSummary.ProjectID = invoiceSummary.ProjectID;
                objAccountInvoiceSummary.ClientId = invoiceSummary.ClientId;
                objAccountInvoiceSummary.InvoiceID = invoiceSummary.InvoiceID;
                objAccountInvoiceSummary.InvoicesummaryID=SaveOrUpdateInvoiceSummary(objAccountInvoiceSummary);
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceSummary-API  ends successfully ");
                return objAccountInvoiceSummary.InvoicesummaryID;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceSummary-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// This method is used to validate the Invoice data whether it is valid or not for generating a invoice.-added by saravanans-tfs:47247
        /// </summary>
        /// <param name="objInv"></param>
        /// <param name="errorstring"></param>
        /// <returns></returns>
        public bool ValidateInvoiceDetails(PTXboInvoice objInv, out string errorstring)
        {
            PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
            errorstring = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-validateInvoiceDetails-API  reached " + ((object)objInv).ToJson(false));
                if (objInv.InitialAssessedValue == 0 && (objInv.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInv.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId()))
                {
                    errorstring = "Update the status as Noticed Data not available message";

                    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ErrorTotalNoticedValueIsNotAvailable.GetId();
                    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                    invoiceSummary.InvoicesummaryID= InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInv, out errorstring);
                    return false;
                }

                if (objInv.FinalAssessedValue == 0 && (objInv.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInv.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId()))
                {
                    errorstring = "Post Hearing value is not available";

                    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ErrorTotalPostHearingValueIsNotAvailable.GetId();
                    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                    invoiceSummary.InvoicesummaryID= InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInv, out errorstring);
                    return false;
                }


                if (objInv.TermExpiryDate != null && objInv.TermExpiryDate < DateTime.Now)
                {
                    errorstring = "Terms have expired";

                    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ErrorReadyForInvoicingButTermHasExpired.GetId();
                    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInv, out errorstring);
                    return false;
                }
                if (objInv.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId() && (objInv.FinalAssessedValue == null || objInv.FinalAssessedValue == 0))
                {
                    errorstring = " Arbitration SettlementAmt is not present";

                    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ArbitrationSettlementAmtisnotpresent.GetId();
                    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInv, out errorstring);
                    return false;
                }
                Logger.For(this).Invoice("Invoice Calculation-validateInvoiceDetails-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-validateInvoiceDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<PTXboInvoice> GetInvoiceDetailsById(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsById-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                
                parameters.Add("@InvoiceID", invoiceID);
                var invoiceDetails = _dapperConnection.Select<PTXboInvoice>(StoredProcedureNames.usp_get_InvoicedetailsbyId, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsById-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsById-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public bool GetInvoiceAndHearingResultMapStatus(int invoiceID, int hearingResultId, int invoiceTypeId, int invoicingProcessingStatusId,bool isILHearing)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAndHearingResultMapStatus-API  reached " + ((object)"invoiceID=" + invoiceID.ToString() + "hearingResultId=" + hearingResultId.ToString() + "invoiceTypeId=" + invoiceTypeId.ToString() + "invoicingProcessingStatusId=" + invoicingProcessingStatusId.ToString()).ToJson(false));
                parameters.Add("@HearingResultID", hearingResultId);
                parameters.Add("@InvoiceID", invoiceID);
                parameters.Add("@InvoiceTypeID", invoiceTypeId);
                parameters.Add("@InvoicingProcessingStatusID", invoicingProcessingStatusId);
                parameters.Add("@IsILHearing", isILHearing); //Added by SaravananS. tfs id:63335
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insert_InvoiceAndHearingResultMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAndHearingResultMapStatus-API  ends successfully ");
                return Convert.ToBoolean(result);
                // return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAndHearingResultMapStatus-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public void SubmitInvoiceAndHearingResultMap(int invoiceID, int hearingResultId, int invoiceTypeId, int invoicingProcessingStatusId, PTXboInvoice objInvoice)
        {
            string errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceAndHearingResultMap-API  reached " + ((object)objInvoice).ToJson(false));
                if (invoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId() || invoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId() || invoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId())
                {
                    if (!GetInvoiceAndHearingResultMapStatus(invoiceID, hearingResultId, invoiceTypeId, invoicingProcessingStatusId, objInvoice.IsILHearing))//Added by SaravananS. tfs id:63335
                    {
                        PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
                        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceAndHearingResultMapNotMapped.GetId();
                        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
                        invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                    }
                }
                else
                {
                    if (invoicingProcessingStatusId != Enumerators.PTXenumInvoicingPRocessingStatus.WaitingInPendingResearchQueue.GetId())
                    {
                        if (!GetInvoiceAndHearingResultMapStatus(invoiceID, hearingResultId, invoiceTypeId, invoicingProcessingStatusId, objInvoice.IsILHearing))//Added by SaravananS. tfs id:63335
                        {
                            PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
                            invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceAndHearingResultMapNotMapped.GetId();
                            invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
                            invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                        }
                    }

                }

                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceAndHearingResultMap-API  ends successfully ");
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceAndHearingResultMap-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// this method is used to submit a record into Invoice table and also InvoiceHearingResultMap table
        /// </summary>

        public bool SubmitInvoice(PTXboInvoice objInvoice, List<Int32> lstHearingResultID, out int NewlyGeneratedInvoiceID, out string errorstring)
        {
            NewlyGeneratedInvoiceID = 0;
            errorstring = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoice-API  reached " + ((object)objInvoice +"hearingresultid "+(object)lstHearingResultID).ToJson(false));
                var objAccountInvoice = GetInvoiceDetailsById(objInvoice.InvoiceID).FirstOrDefault();
                if (objAccountInvoice == null)
                {
                    objAccountInvoice = new PTXboInvoice();
                    objAccountInvoice.CreatedDateAndTime = objInvoice.CreatedDateAndTime;
                }

                //Added by SaravananS. tfs id:63335
                if (objInvoice.IsRegenerateInvoice == null)
                {
                    objInvoice.IsRegenerateInvoice = 0;
                }
                objAccountInvoice.IsRegenerateInvoice = objInvoice.IsRegenerateInvoice ;
                //Ends here.


                objAccountInvoice.InvoicingStatusId = objInvoice.InvoicingStatusId;
                objAccountInvoice.AutoGenerated = objInvoice.AutoGenerated == null ? false : objAccountInvoice.AutoGenerated = objInvoice.AutoGenerated;
                objAccountInvoice.ClientId = objInvoice.ClientId;
                // objAccountInvoice.CompoundInterest = objInvoice.CompoundInterest;
                objAccountInvoice.ContingencyFee = objInvoice.ContingencyFee;
                objAccountInvoice.ContingencyPercentage = objInvoice.ContingencyPercentage;

                objAccountInvoice.DeliveryMethodId = objInvoice.DeliveryMethodId;
                //objAccountInvoice.DeliveryStatus = objInvoice.DeliveryStatusId == 0 ? null : new PTXdoDeliveryStatus() { DeliverystatusId = objInvoice.DeliveryStatusId };
                objAccountInvoice.FinalAssessedValue = objInvoice.FinalAssessedValue;
                objAccountInvoice.InitialAssessedValue = objInvoice.InitialAssessedValue;
                objAccountInvoice.FlatFee = objInvoice.FlatFee;
                //  objAccountInvoice.InterestBalance = objInvoice.InterestBalance;
                //objAccountInvoice.InterestPayments = objInvoice.InterestPayments;
                //  objAccountInvoice.InterestRate = objInvoice.InterestRate;
                objAccountInvoice.InvoiceAmount = objInvoice.InvoiceAmount;
                //objAccountInvoice.InvoiceCreditAmount = objInvoice.InvoiceCreditAmount;
                // Commented by yuvaraj.
                //objAccountInvoice.InvoiceDate = objInvoice.InvoiceDate;
                objAccountInvoice.InvoiceDescription = objInvoice.InvoiceDescription;
                objAccountInvoice.OnHold = objInvoice.OnHold == null ? false : objAccountInvoice.OnHold = objInvoice.OnHold;
                objAccountInvoice.OnHoldDate = objInvoice.OnHoldDate;
                objAccountInvoice.PaymentDueDate = null; // modified by Arunkumar S | TFS ID: 40863
                objAccountInvoice.PriorYearTaxRate = objInvoice.PriorYearTaxRate;
                objAccountInvoice.ProjectId = objInvoice.ProjectId;
                objAccountInvoice.SentDateAndTime = objInvoice.SentDateAndTime;
                objAccountInvoice.TaxYear = objInvoice.TaxYear;
                objAccountInvoice.TotalAmountPaid = objInvoice.TotalAmountPaid;
                objAccountInvoice.UpdatedBy = objInvoice.CreatedBy;
                objAccountInvoice.UpdatedDateTime = DateTime.Now;
                //    objAccountInvoice.TotalBalance = objInvoice.TotalBalance;
                // objAccountInvoice.TotalInvoiceAmount = objInvoice.TotalInvoiceAmount;
                objAccountInvoice.YearlyHearingDetailsId = objInvoice.YearlyHearingDetailsId;
                objAccountInvoice.GroupId = objInvoice.GroupId;
                objAccountInvoice.InvoiceTypeId = objInvoice.InvoiceTypeId;
                objAccountInvoice.InvoicingProcessingStatusId = objInvoice.InvoicingProcessingStatusId;
                objAccountInvoice.InvoiceGroupingTypeId = objInvoice.InvoiceGroupingTypeId;
                objAccountInvoice.InvoiceCapValue = objInvoice.InvoiceCapValue;
                objAccountInvoice.IsSpecialTerm = objInvoice.IsSpecialTerm;
                objAccountInvoice.DontGenerateInvoiceFlag = objInvoice.DontGenerateInvoiceFlag;
                objAccountInvoice.Reduction = objInvoice.Reduction;
                objAccountInvoice.TotalEstimatedTaxSavings = objInvoice.EstimatedTaxSaving;
                objAccountInvoice.InvoiceDescription = objInvoice.InvoiceDescription;//Added by SaravananS. tfs id:56933

                //Added by saravanans.tfs id:56388 //commented by SaravananS. tfs id:63335
                //if (objInvoice.InvoiceTypeId!=1)
                //{
                //    objInvoice.IsRegenerateInvoice = 0;
                //}
                //Ends here.

                objAccountInvoice.IsILHearing = objInvoice.IsILHearing;//Added by saravanans.tfs id:63335
                objAccountInvoice.IsRegenerateInvoice = objInvoice.IsRegenerateInvoice;//Added by saravanans.tfs id:55312

                if (lstHearingResultID.Count > 0)
                {
                    //if (objAccountInvoice.InvoiceAndHearingResultMap == null)
                    //{
                    //    objAccountInvoice.InvoiceAndHearingResultMap = new List<PTXboInvoiceAndHearingResultMap>();
                    //}

                    //Added by Saravanans.tfs id:54969
                    if (objAccountInvoice.DontGenerateInvoiceFlag ==true)
                    {
                        objAccountInvoice.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.DoNotGenerateInvoice.GetId();
                        objAccountInvoice.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.DoNotGenerateInvoice.GetId();
                    }
                    //Ends here

                    SaveOrUpdateInvoice(objAccountInvoice, out NewlyGeneratedInvoiceID);

                    if (objAccountInvoice.InvoiceID == 0)
                    {
                        objAccountInvoice.InvoiceID = NewlyGeneratedInvoiceID;
                    }
                    foreach (int hearingResultId in lstHearingResultID)
                    {
                        if (hearingResultId != 0)
                        {
                            SubmitInvoiceAndHearingResultMap(objAccountInvoice.InvoiceID, hearingResultId, objInvoice.InvoiceTypeId, objInvoice.InvoicingProcessingStatusId, objAccountInvoice);

                        }
                    }
                }

                
                if (objAccountInvoice.InvoiceID != 0)
                {
                    NewlyGeneratedInvoiceID = objAccountInvoice.InvoiceID;//Newly created invoice ID  
                    if(objAccountInvoice.AmountDue>0)
                    {
                        UpdateMSInvoice(NewlyGeneratedInvoiceID, false);//for update the amount due.
                    }
                    
                }

                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoice-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoice-API  error " + ((object)ex).ToJson(false));
                errorstring = ex.Message;
                return false;
            }
            finally
            {
                Dispose();
            }
        }

        public List<PTXboInvoiceLineItem> CheckPosttoFlatFeeInvoicing(int accountId, int invoicingLevel)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-CheckPosttoFlatFeeInvoicing-API  reached " + ((object)"accountId=" + accountId.ToString() + "invoicingLevel=" + invoicingLevel.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                List<PTXboInvoiceLineItem> invoiceDetails = new List<PTXboInvoiceLineItem>();
                parameters.Add("@Id", accountId);
                parameters.Add("@Invoicinglevel", invoicingLevel);
                invoiceDetails = _dapperConnection.Select<PTXboInvoiceLineItem>(StoredProcedureNames.usp_CheckPosttoFlatFeeInvoicing, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("Invoice Calculation-CheckPosttoFlatFeeInvoicing-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-CheckPosttoFlatFeeInvoicing-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public List<Int32> GetDistinctHearingResultIDs(List<PTXboInvoice> lstInvoiceGroupTypeAccounts, int InvoiceTypeId)
        {
            try
            {

                Logger.For(this).Invoice("Invoice Calculation-GetDistinctHearingResultIDs-API  reached " + ((object)lstInvoiceGroupTypeAccounts).ToJson(false));
                List<int> lstHearingResultIDs = new List<int>();
                if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                {
                    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.ArbitrationDetilId).Distinct().ToList();
                }
                else if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
                {
                    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.LitigationId).Distinct().ToList();
                }
                else if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
                {
                    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.BppRenditionId).Distinct().ToList();
                }
                else if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                {
                    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.TaxBillAuditId).Distinct().ToList();
                }
                //Added by SaravananS. tfs id:63634
                else if (InvoiceTypeId == Enumerators.PTXenumInvoiceType.SOAH.GetId())
                {
                    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.SOAHDetailsID).Distinct().ToList();
                }
                //Ends here.
                else
                {
                    lstHearingResultIDs = lstInvoiceGroupTypeAccounts.Select(h => h.HearingResultId).Distinct().ToList();
                }
                List<Int32> lstHearingResultID = new List<int>();
                foreach (int hearingResultId in lstHearingResultIDs)
                {
                    if (hearingResultId != 0)
                    {
                        lstHearingResultID.Add(hearingResultId);
                    }
                }
                Logger.For(this).Invoice("Invoice Calculation-GetDistinctHearingResultIDs-API  ends successfully ");
                return lstHearingResultID;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetDistinctHearingResultIDs-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(PTXboInvoiceRemarks objBOInvoiceRemarks, out string errorMessage)
        {

            errorMessage = string.Empty;
            string remarks = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel-API  reached " + ((object)objBOInvoiceRemarks).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceRemarksID", objBOInvoiceRemarks.InvoiceRemarksID);
                parameters.Add("@InvoiceID", objBOInvoiceRemarks.InvoiceID);
                parameters.Add("@InvoiceRemarks", objBOInvoiceRemarks.InvoiceRemarks);
                parameters.Add("@RemarksTypeId", objBOInvoiceRemarks.RemarksTypeId);
                parameters.Add("@UpdatedBy", objBOInvoiceRemarks.UpdatedBy);
                parameters.Add("@UpdatedDateTime", objBOInvoiceRemarks.UpdatedDateTime);
                parameters.Add("@IsCreditAudit", objBOInvoiceRemarks.IsCreditAudit);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceRemarks, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel-API  ends successfully ");
                return true;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel-API  error " + ((object)ex).ToJson(false));
                errorMessage = ex.Message;
                throw ex;

            }
            finally
            {
                Dispose();
            }

        }

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
            bool isInvoiceRecordCreated = false;
            PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
            PTXboInvoice objInsertInvoiceData = new PTXboInvoice();
            PTXboAccountLevelInvoiceOutput invoiceOutput = new PTXboAccountLevelInvoiceOutput();
            bool IsInvoiceDataValid = false;
            bool IsInvoiceValidationFails = false;
            int NewlyCreatedInvoiceID = 0;
            string errorMessage = string.Empty;
            decimal InvoiceCapValue = 0;
            decimal InvoiceCalAmount = 0;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertAccountLevelInvoiceGeneration-API  reached " + ((object)lstInvoiceDetails).ToJson(false));
                //Added by Kishore to prevent inserting multiple Hearing Type
                lstInvoiceDetails = lstInvoiceDetails.Where(a => a.AccountId == objInvoice.AccountId && a.HearingTypeId == objInvoice.HearingTypeId).ToList();
                if (ValidateInvoiceDetails(objInvoice, out errorMessage))
                {
                    if (objInvoice.InvoiceCapValue != null && objInvoice.InvoiceCapValue != 0)
                    {
                        InvoiceCapValue = Convert.ToDecimal(objInvoice.InvoiceCapValue);
                    }
                    //Kishore- For Special Term Invoices
                    if ((objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId()) && objInvoice.IsSpecialTerm == true)
                    {
                        objInvoice.Reduction = Convert.ToDecimal((objInvoice.InitialAssessedValue) - (objInvoice.FinalAssessedValue));
                        objInvoice.EstimatedTaxSaving = (Convert.ToDecimal(objInvoice.PriorYearTaxRate) / 100) * Convert.ToDecimal((objInvoice.Reduction));
                        objInvoice.ContingencyFee = Math.Round((objInvoice.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)), 2); //added round off by saravanans-tfs:48022
                        
                        //objInvoice.InvoiceAmount = InvoiceCalAmount + objInvoice.FlatFee.GetValueOrDefault();
                        InvoiceAmountCalculation(Convert.ToDecimal(objInvoice.ContingencyFee), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680

                        objInvoice.InvoiceAmount = InvoiceCalAmount;//Added by saravanans
                        objInsertInvoiceData = objInvoice;
                        IsInvoiceDataValid = true;

                    }
                    else
                    {
                        // K.Selva - It Checks   the flat fee   amount for this account
                        if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                        {
                            if (objInvoice.FlatFee > 0 && objInvoice.FlatFee != null)
                            {
                                objInvoice.InvoiceAmount = objInvoice.FlatFee;
                                objInsertInvoiceData = objInvoice;
                                IsInvoiceDataValid = true;
                            }
                            else
                            {
                                //invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoicenotrequiredFlatFeenotNoticed.GetId();
                                //invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                                //invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                                IsInvoiceDataValid = false;
                            }
                        }
                        else
                        {
                            objInvoice.Reduction = Convert.ToDecimal((objInvoice.InitialAssessedValue) - (objInvoice.FinalAssessedValue));
                            if (objInvoice.Reduction > 0 || objInvoice.IsOutOfTexas) //Added by SaravananS. tfs id:63335
                            {
                                
                                if (!objInvoice.IsOutOfTexas)//Added by SaravananS. tfs id:63335
                                {
                                    objInvoice.EstimatedTaxSaving = Convert.ToDecimal((objInvoice.PriorYearTaxRate / 100)) * Convert.ToDecimal((objInvoice.Reduction));
                                    objInvoice.ContingencyFee = Math.Round((objInvoice.EstimatedTaxSaving) * Convert.ToDecimal(objInvoice.ContingencyPercentage), 2);
                                }
                                else
                                {
                                    var taxrate = Convert.ToDecimal(objInvoice.PriorYearTaxRate);
                                    //Added by SaravananS. tfs id:61646
                                    if (objInvoice.IsILHearing == true)
                                    {
                                        taxrate = Convert.ToDecimal(taxrate * objInvoice.StateEvaValue);
                                    }
                                    //Ends here.

                                    objInvoice.TotalEstimatedTaxSavings = Math.Round((taxrate * Convert.ToDecimal((objInvoice.Reduction))), 2);
                                    //objInvoice.ContingencyPercentage = objInvoice.Contingency * 100;
                                    objInvoice.ContingencyPercentage = (objInvoice.ContingencyPercentage > 100 ? (objInvoice.ContingencyPercentage / 100) : objInvoice.ContingencyPercentage);// objInv.Contingency * 100;
                                    objInvoice.ContingencyPercentage = objInvoice.ContingencyPercentage < 1 ? objInvoice.ContingencyPercentage * 100 : objInvoice.ContingencyPercentage;
                                    objInvoice.ContingencyFee = Math.Round(Convert.ToDecimal((objInvoice.TotalEstimatedTaxSavings) * Convert.ToDecimal((objInvoice.ContingencyPercentage / 100))), 2);
                                }

                                if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId())
                                {
                                    List<PTXboInvoiceLineItem> dtInvoiceLineItem = CheckPosttoFlatFeeInvoicing(objInvoice.AccountId, 1);
                                    if (dtInvoiceLineItem.Count == 0)
                                    {
                                        
                                        InvoiceAmountCalculation(Convert.ToDecimal(objInvoice.ContingencyFee.GetValueOrDefault()), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680

                                        objInvoice.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                                        //objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
                                    }
                                    else
                                    {
                                        
                                        InvoiceAmountCalculation(Convert.ToDecimal(objInvoice.ContingencyFee.GetValueOrDefault()), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680
                                        objInvoice.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                                        //objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault();
                                    }
                                }
                                else
                                {
                                    
                                    InvoiceAmountCalculation(Convert.ToDecimal(objInvoice.ContingencyFee.GetValueOrDefault()), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680
                                    objInvoice.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                                    //objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
                                }

                                objInsertInvoiceData = objInvoice;
                                IsInvoiceDataValid = true;
                            }
                            else if (objInvoice.Reduction <= 0 && (objInvoice.FlatFee != null && objInvoice.FlatFee != 0))//Added By Pavithra.B on 02Feb2017 - if Reduction <=0 and flatfee is available then create Invoice based on FlatFee
                            {
                                objInvoice.EstimatedTaxSaving = Convert.ToDecimal((objInvoice.PriorYearTaxRate / 100)) * Convert.ToDecimal((objInvoice.Reduction));
                                objInvoice.ContingencyFee = Math.Round((objInvoice.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)), 2);

                                InvoiceAmountCalculation(Convert.ToDecimal(objInvoice.ContingencyFee.GetValueOrDefault()), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680
                                objInvoice.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                                //objInvoice.InvoiceAmount = objInvoice.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
                                objInsertInvoiceData = objInvoice;
                                IsInvoiceDataValid = true;
                            }
                            else
                            {
                                //invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceNotRequiredReductionNotNoticed.GetId();
                                //invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                                //invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                                IsInvoiceDataValid = false;
                            }
                        }
                    }
                }
                else
                {
                    objInsertInvoiceData = objInvoice;
                    IsInvoiceValidationFails = true;
                }

                if(objInvoice.InvoiceGroupingTypeId==3 && !string.IsNullOrEmpty(objInvoice.PropertyAddress) 
                    && !objInvoice.IsOutOfTexas)//Added by SaravananS. tfs id:63335
                {
                    objInsertInvoiceData.InvoiceDescription = objInvoice.PropertyAddress;
                }
                else
                {
                    objInsertInvoiceData.InvoiceDescription = objInvoice.InvoiceDescription;
                }
                
                objInsertInvoiceData.InvoiceDate = DateTime.Now;
                if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                {
                    objInsertInvoiceData.PaymentDueDate = DateTime.Now.AddDays(90);
                }
                else
                {
                    objInsertInvoiceData.PaymentDueDate = DateTime.Now.AddDays(30);
                }
                objInsertInvoiceData.CreatedDateAndTime = DateTime.Now;
                objInsertInvoiceData.AutoGenerated = true;
                List<Int32> lstHearingResultID = GetDistinctHearingResultIDs(lstInvoiceDetails, objInvoice.InvoiceTypeId);

                if (IsInvoiceDataValid)
                {
                    if (objInvoice.IsSpecialTerm)
                    {
                        objInsertInvoiceData.FlatFee = objInvoice.FlatFee.GetValueOrDefault();
                        if(!string.IsNullOrEmpty(objInvoice.InvoiceDescription))
                        {
                            objInsertInvoiceData.InvoiceDescription = objInvoice.InvoiceDescription;//Added by SaravananS. tfs id:56933
                        }
                        
                    }
                        

                    objInsertInvoiceData.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
                    objInsertInvoiceData.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.ReadyForInvoicing.GetId();
                    objInsertInvoiceData.InvoiceTypeId = objInvoice.InvoiceTypeId;
                    objInsertInvoiceData.IsRegenerateInvoice = objInvoice.IsRegenerateInvoice;//Added by SaravananS.tfs id:63335
                    isInvoiceRecordCreated = SubmitInvoice(objInsertInvoiceData, lstHearingResultID, out NewlyCreatedInvoiceID, out errorMessage);

                    //added by saravanans-updating invoiceid if it's 0
                    if (objInsertInvoiceData.InvoiceID == 0)
                    {
                        objInsertInvoiceData.InvoiceID = NewlyCreatedInvoiceID;
                    }
                    //ends here

                    //if (isInvoiceRecordCreated == true && NewlyCreatedInvoiceID != 0)
                    //{
                    //    bool isSuccessCapValue = false;
                    //    string errormessage = string.Empty;
                    //    List<PTXboInvoicePaymentMap> lstInvoicePaymentMap;
                    //    var objInvoiceid = GetInvoiceDetailsById(NewlyCreatedInvoiceID).FirstOrDefault();
                    //    if (objInvoiceid != null)
                    //    {
                            
                    //        //Pavithra.B - Included Cap Value
                    //        //if (objInsertInvoiceData.CapValue > 0 && Convert.ToDecimal((objInvoice.ContingencyPercentage)) > 0 && Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount) > Convert.ToDecimal(objInsertInvoiceData.CapValue))
                    //        //{
                    //        //    lstInvoicePaymentMap = GetInvoicePaymentMap(NewlyCreatedInvoiceID);
                    //        //    if (lstInvoicePaymentMap != null && lstInvoicePaymentMap.Count > 0)
                    //        //    {
                    //        //        PTXboPayment Objpayment = new PTXboPayment();
                    //        //        foreach (PTXboInvoicePaymentMap obiInvPay in lstInvoicePaymentMap)
                    //        //        {
                    //        //            var lstPayment = GetInvoicePayment(obiInvPay.PaymentId, "Cap Value Adjustment");
                    //        //            if (lstPayment != null)
                    //        //            {
                    //        //                Objpayment = lstPayment;
                    //        //            } 

                    //        //            decimal PaymentAmount = 0;
                    //        //            PaymentAmount = Math.Round((objInvoiceid.InvoiceAmount.GetValueOrDefault()) - Convert.ToDecimal(objInsertInvoiceData.CapValue), 2);
                    //        //            Objpayment.ClientId = objInsertInvoiceData.ClientId;
                    //        //            Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                    //        //            Objpayment.PaymentAmount = PaymentAmount;
                    //        //            Objpayment.PaymentDescription = "Cap Value Adjustment";
                    //        //            Objpayment.CreatedBy = createdBy;
                    //        //            Objpayment.CreatedDateTime = DateTime.Now;
                    //        //            Objpayment.UpdatedBy = createdBy;
                    //        //            Objpayment.UpdatedDateTime = DateTime.Now;
                    //        //            isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, NewlyCreatedInvoiceID, out errormessage);
                    //        //            if (isSuccessCapValue)
                    //        //            {
                    //        //                bool isSuccess = UpdateAmountDueCapValueChange(NewlyCreatedInvoiceID, out errormessage);
                    //        //            }
                    //        //        }
                    //        //    }
                    //        //    else
                    //        //    {
                    //        //        PTXboPayment Objpayment = new PTXboPayment();
                    //        //        decimal PaymentAmount = 0;
                    //        //        PaymentAmount = Math.Round((objInvoiceid.InvoiceAmount.GetValueOrDefault()) - Convert.ToDecimal(objInsertInvoiceData.CapValue), 2);
                    //        //        Objpayment.ClientId = objInsertInvoiceData.ClientId;
                    //        //        Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                    //        //        Objpayment.PaymentAmount = PaymentAmount;
                    //        //        Objpayment.PaymentDescription = "Cap Value Adjustment";
                    //        //        Objpayment.CreatedBy = createdBy;
                    //        //        Objpayment.CreatedDateTime = DateTime.Now;
                    //        //        Objpayment.UpdatedBy = createdBy;
                    //        //        Objpayment.UpdatedDateTime = DateTime.Now;
                    //        //        isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, NewlyCreatedInvoiceID, out errormessage);
                    //        //        if (isSuccessCapValue)
                    //        //        {
                    //        //            bool isSuccess = UpdateAmountDueCapValueChange(NewlyCreatedInvoiceID, out errormessage);
                    //        //        }
                    //        //    }
                    //        //}
                    //        if (Convert.ToDecimal(objInvoiceid.AmountAdjusted) != 0)
                    //        {
                    //            RemoveCapvalueAdjustment(objInvoiceid.InvoiceID);
                    //            bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errormessage);
                    //        }
                    //    }

                    //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ReadyForInvoicing.GetId();
                    //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();

                    //    invoiceSummary.InvoiceID = NewlyCreatedInvoiceID;
                    //    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);

                    //    //Need to insert into Invoice Remarks
                    //    PTXboInvoiceRemarks objInvoiceRemarks = new PTXboInvoiceRemarks();
                    //    objInvoiceRemarks.InvoiceID = NewlyCreatedInvoiceID;

                    //    if (invoicingGroupType == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString())
                    //        objInvoiceRemarks.InvoiceRemarks = "Invoice created for Grouping level : " + Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString();
                    //    else
                    //        objInvoiceRemarks.InvoiceRemarks = "Invoice created for Grouping level : " + Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString() + " but the Original Grouping level is " + invoicingGroupType;
                    //    //}

                    //    objInvoiceRemarks.UpdatedBy = createdBy;
                    //    InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objInvoiceRemarks, out errorMessage);
                    //}
                    //else
                    //{
                    //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.TachnicalErrorInInvoiceGeneration.GetId();
                    //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                    //    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);
                    //}
                }

                if (IsInvoiceValidationFails)
                {
                    objInsertInvoiceData.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
                    objInsertInvoiceData.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.WaitingInPendingResearchQueue.GetId();
                    objInsertInvoiceData.InvoiceTypeId = objInvoice.InvoiceTypeId;
                    isInvoiceRecordCreated = SubmitInvoice(objInsertInvoiceData, lstHearingResultID, out NewlyCreatedInvoiceID, out errorMessage);
                    //added by saravanans-updating invoiceid if it's 0
                    if (objInsertInvoiceData.InvoiceID == 0)
                    {
                        objInsertInvoiceData.InvoiceID = NewlyCreatedInvoiceID;
                    }
                    //ends here
                    //if (isInvoiceRecordCreated)
                    //{
                    //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.WaitingInPendingResearchQueue.GetId();
                    //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();

                    //    invoiceSummary.InvoiceID = NewlyCreatedInvoiceID;
                    //    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);

                    //    //Need to insert into Invoice Remarks
                    //    PTXboInvoiceRemarks objInvoiceRemarks = new PTXboInvoiceRemarks();
                    //    objInvoiceRemarks.InvoiceID = NewlyCreatedInvoiceID;
                    //    if (invoicingGroupType == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString())
                    //        objInvoiceRemarks.InvoiceRemarks = "Invoice created for Hearing Type : " + hearingType + " and Grouping level : " + Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString();
                    //    else
                    //        objInvoiceRemarks.InvoiceRemarks = "Invoice created for Hearing Type : " + hearingType + " and Grouping level : " + Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString() + " but the Original Grouping level is " + invoicingGroupType;
                    //    objInvoiceRemarks.UpdatedBy = createdBy;
                    //    InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objInvoiceRemarks, out errorMessage);
                    //}
                    //else
                    //{
                    //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.TachnicalErrorInInvoiceGeneration.GetId();
                    //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                    //    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);
                    //}
                }
                Logger.For(this).Invoice("Invoice Calculation-InsertAccountLevelInvoiceGeneration-API  ends successfully ");
                invoiceOutput.IsInvoiceRecordCreated = isInvoiceRecordCreated;
                invoiceOutput.InvoiceDetails = objInsertInvoiceData;
                return invoiceOutput;
                //return isInvoiceRecordCreated;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertAccountLevelInvoiceGeneration-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public List<PTXboInvoice> GetInvoiceGenerationInputData(int clientID, int taxYear, int invoiceTypeId,bool outOfTexas= false,bool IsDisasterInvoice =false)
        {
            Hashtable parameters = new Hashtable();
            string spName = string.Empty;
            List<PTXboInvoice> invoice = new List<PTXboInvoice>();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-API  reached " + ((object)"clientID=" + clientID.ToString() + "taxYear=" + taxYear.ToString() + "invoiceTypeId=" + invoiceTypeId.ToString()).ToJson(false));
                parameters.Add("@ClientID", clientID);
                parameters.Add("@TaxYear", taxYear);
                parameters.Add("@InvoiceTypeId", invoiceTypeId);
                if(IsDisasterInvoice)
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-IsDisasterInvoice");
                    spName = StoredProcedureNames.usp_GetDisasterInvoiceInputData;
                }
                else if(outOfTexas==false)
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-Texas");
                    spName = StoredProcedureNames.usp_getInvoiceInputData;
                }
                else
                {
                    Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-outOfTexas");
                    spName = StoredProcedureNames.usp_getInvoiceInputData_OT;
                }
                invoice = _dapperConnection.Select<PTXboInvoice>(spName, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-API  ends successfully ");
                //Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-returns " + ((object)invoice).ToJson(false));

                return invoice;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }


        public List<PTXboInvoice> GetCreditInvoiceGenerationInputData(int Clientid)
        {
            Hashtable parameters = new Hashtable();
            string spName = string.Empty;
            List<PTXboInvoice> invoice = new List<PTXboInvoice>();
            try
            {
                 parameters.Add("@ClientID", Clientid); 
                spName = StoredProcedureNames.usp_getCreditInvoiceInputData;
                
                invoice = _dapperConnection.Select<PTXboInvoice>(spName, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-API  ends successfully ");
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-returns " + ((object)invoice).ToJson(false));

                return invoice;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceGenerationInputData-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }


        #endregion Invoice Regular queue

        #region Invoice Adjustment
        public PTXboInvoiceAdjustmentReportDetails GetInvoiceAdjustmentReportDetails(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAdjustmentReportDetails-API  reached " + ((object)invoiceId).ToJson(false));
                parameters.Add("@InvoiceID", invoiceId);
                var result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getInvoiceGeneratedreportDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure,
                    Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXboInvoiceReport>(),
                    gr => gr.Read<PTXboInvoiceAccount>());

                PTXboInvoiceAdjustmentReportDetails invoiceAdjustmentReportDetails = new PTXboInvoiceAdjustmentReportDetails
                {
                    InvoiceDetails = result.Item1.Count() > 0 ? (List<PTXboInvoiceReport>)result.Item1 : new List<PTXboInvoiceReport>(),
                    InvoiceAccount = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>()
                };
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAdjustmentReportDetails-API  ends successfully ");
                return invoiceAdjustmentReportDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAdjustmentReportDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public PTXboAutoAdjustmentInvoiceDetails GetAutoAdjustmentInvoiceDetails(int invoiceID, int invoiceAdjustmentRequestID)
        {
            PTXboAutoAdjustmentInvoiceDetails autoAdjustmentInvoiceDetails = new PTXboAutoAdjustmentInvoiceDetails();
            try
            { 
                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  reached " + ((object)"invoiceID=" + invoiceID.ToString() + "invoiceAdjustmentRequestID=" + invoiceAdjustmentRequestID.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);
                parameters.Add("@InvoiceAdjustmentRequestID", invoiceAdjustmentRequestID);

                if(invoiceAdjustmentRequestID != 0)
                {
                   var  result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getAutoAdjustmentInvoiceDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                             gr => gr.Read<PTXboInvoice>(),
                             gr => gr.Read<PTXboInvoiceAdjustment>(),
                             gr => gr.Read<PTXboInvoiceAccount>(),
                             gr => gr.Read<PTXboInvoiceAdjustmentClarifications>()
                             , gr => gr.Read<PTXboExemptionJurisdictions>()
                             );
                    autoAdjustmentInvoiceDetails.Invoice = result.Item1.Count() > 0 ? result.Item1.FirstOrDefault() : new PTXboInvoice();
                    autoAdjustmentInvoiceDetails.Adjustment = result.Item2.Count() > 0 ? result.Item2.FirstOrDefault() : new PTXboInvoiceAdjustment();
                    autoAdjustmentInvoiceDetails.InvoiceLienItem = result.Item3.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item3 : new List<PTXboInvoiceAccount>();
                    autoAdjustmentInvoiceDetails.InvoiceAdjustmentClarification = result.Item4.Count() > 0 ? (List<PTXboInvoiceAdjustmentClarifications>)result.Item4 : new List<PTXboInvoiceAdjustmentClarifications>();
                    autoAdjustmentInvoiceDetails.Invoice.ExemptionJurisdicitonlst = result.Item5.Count() > 0 ? (List<PTXboExemptionJurisdictions>)result.Item5 : new List<PTXboExemptionJurisdictions>();

                }
                else
                {
                   var  result = _dapperConnection.SelectMultiple(StoredProcedureNames.usp_getAutoAdjustmentInvoiceDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                                                 gr => gr.Read<PTXboInvoice>(),
                                                 gr => gr.Read<PTXboInvoiceAdjustment>(),
                                                 gr => gr.Read<PTXboInvoiceAccount>(),
                                                 gr => gr.Read<PTXboInvoiceAdjustmentClarifications>()
                                                 //, gr => gr.Read<PTXboExemptionJurisdictions>()
                                                 );
                    autoAdjustmentInvoiceDetails.Invoice = result.Item1.Count() > 0 ? result.Item1.FirstOrDefault() : new PTXboInvoice();
                    autoAdjustmentInvoiceDetails.Adjustment = result.Item2.Count() > 0 ? result.Item2.FirstOrDefault() : new PTXboInvoiceAdjustment();
                    autoAdjustmentInvoiceDetails.InvoiceLienItem = result.Item3.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item3 : new List<PTXboInvoiceAccount>();
                    autoAdjustmentInvoiceDetails.InvoiceAdjustmentClarification = result.Item4.Count() > 0 ? (List<PTXboInvoiceAdjustmentClarifications>)result.Item4 : new List<PTXboInvoiceAdjustmentClarifications>();
                  
                }


               // autoAdjustmentInvoiceDetails.Invoice = result.Item1.Count() > 0 ? result.Item1.FirstOrDefault() : new PTXboInvoice();
               // autoAdjustmentInvoiceDetails.Adjustment = result.Item2.Count() > 0 ? result.Item2.FirstOrDefault() : new PTXboInvoiceAdjustment();
               // autoAdjustmentInvoiceDetails.InvoiceLienItem = result.Item3.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item3 : new List<PTXboInvoiceAccount>();
               // autoAdjustmentInvoiceDetails.InvoiceAdjustmentClarification = result.Item4.Count() > 0 ? (List<PTXboInvoiceAdjustmentClarifications>)result.Item4 : new List<PTXboInvoiceAdjustmentClarifications>();
               //autoAdjustmentInvoiceDetails.Invoice.ExemptionJurisdicitonlst = result.Item5.Count()>0 ? (List<PTXboExemptionJurisdictions>)result.Item5 : new List<PTXboExemptionJurisdictions>();
                //if(result.Item5.Count()>0)
                //{
                //    autoAdjustmentInvoiceDetails.Invoice.ExemptionJurisdicitonlst = result.Item5.Count() > 0 ? (List<PTXboExemptionJurisdictions>)result.Item5 : new List<PTXboExemptionJurisdictions>();
                //}

                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  ends successfully ");
                return autoAdjustmentInvoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetAutoAdjustmentInvoiceDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        #endregion Invoice Adjustment


        #region Invoice Special term

        public List<PTXboInvoiceAndHearingResultMap> GetInvoiceAndHearingResultMap(int invoiceID)
        {
            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAndHearingResultMap-  reached " + ((object)invoiceID).ToJson(false));
                var result = _dapperConnection.Select<PTXboInvoiceAndHearingResultMap>(StoredProcedureNames.usp_get_InvoiceAndHearingResultMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAndHearingResultMap-  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceAndHearingResultMap-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public bool SaveOrUpdateHearingResult(PTXboHearingResult hearingResult)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateHearingResult-API  reached " + ((object)hearingResult).ToJson(false));
                parameters.Add("@HearingResultId", hearingResult.HearingResultId);
                parameters.Add("@HearingDetailsId", hearingResult.HearingDetailsId);
                parameters.Add("@HearingTypeId", hearingResult.HearingTypeId);
                parameters.Add("@HearingResolutionId", hearingResult.HearingResolutionId);
                parameters.Add("@HearingValue", hearingResult.HearingValue);
                parameters.Add("@HearingResultReasonCodeId", hearingResult.HearingResultReasonCodeId);
                parameters.Add("@PostHearingLandValue", hearingResult.PostHearingLandValue);
                parameters.Add("@PostHearingImprovedValue", hearingResult.PostHearingImprovedValue);
                parameters.Add("@PostHearingMarketValue", hearingResult.PostHearingMarketValue);
                parameters.Add("@PostHearingTotalValue", hearingResult.PostHearingTotalValue);
                parameters.Add("@HearingAgentId", hearingResult.HearingAgentId);
                parameters.Add("@HearingStatusId", hearingResult.HearingStatusId);
                parameters.Add("@HearingFinalized", hearingResult.HearingFinalized);
                parameters.Add("@DismissalAuthStatusId", hearingResult.DismissalAuthStatusId);
                parameters.Add("@confirmationLetterReceivedDate", hearingResult.confirmationLetterReceivedDate);
                parameters.Add("@confirmationLetterDateTime", hearingResult.confirmationLetterDateTime);
                parameters.Add("@completionDateAndTime", hearingResult.completionDateAndTime);
                parameters.Add("@HearingResultsSentOn", hearingResult.HearingResultsSentOn);
                parameters.Add("@InvoiceGenerated", hearingResult.InvoiceGenerated);
                parameters.Add("@ReviewForArbitration", hearingResult.ReviewForArbitration);
                parameters.Add("@ClientRequestedForArbitration", hearingResult.ClientRequestedForArbitration);
                parameters.Add("@HearingProcessingStatusId", hearingResult.HearingProcessingStatusId);
                parameters.Add("@UpdatedBy", hearingResult.UpdatedBy);
                parameters.Add("@UpdatedDateTime", hearingResult.UpdatedDateTime);
                parameters.Add("@HearingTrackingCodeId", hearingResult.HearingTrackingCodeId);
                parameters.Add("@HearingResultReportId", hearingResult.HearingResultReportId);
                parameters.Add("@ExemptInvoice", hearingResult.ExemptInvoice);
                parameters.Add("@ResultGenerated", hearingResult.ResultGenerated);
                parameters.Add("@FinalizedDate", hearingResult.FinalizedDate);
                parameters.Add("@HRInvoiceStatusid", hearingResult.HRInvoiceStatusid);
                parameters.Add("@AppraiserName", hearingResult.AppraiserName);
                parameters.Add("@ReviewBindingArbitration", hearingResult.ReviewBindingArbitration);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateHearingResult, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateHearingResult-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateHearingResult-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public PTXboYearlyHearings GetYearlyHearingDetails(int yearlyHearingDetailsId)
        {
            //Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetYearlyHearingDetails-API  reached " + ((object)yearlyHearingDetailsId).ToJson(false));
                //parameters.Add("@YearlyHearingDetailsId", yearlyHearingDetailsId);
                // var result = _dapperConnection.Select<PTXboYearlyHearings>(StoredProcedureNames.usp_get_YearlyHearingDetails, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                var result = GetYearlyHearingDetailsById(yearlyHearingDetailsId).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetYearlyHearingDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetYearlyHearingDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<PTXboYearlyHearings> GetYearlyHearingDetailsById(int yearlyHearingDetailsId)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetYearlyHearingDetails-API  reached " + ((object)yearlyHearingDetailsId).ToJson(false));
                parameters.Add("@YearlyHearingDetailsId", yearlyHearingDetailsId);

                var result = _dapperConnection.Select<PTXboYearlyHearings>(StoredProcedureNames.usp_get_YearlyHearingDetails, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("Invoice Calculation-GetYearlyHearingDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetYearlyHearingDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboHearingResult GetHearingResultByType(int hearingDetailsId, int hearingTypeId = 0)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetHearingResultByType-API  reached " + ((object)"hearingDetailsId=" + hearingDetailsId.ToString() + "hearingTypeId=" + hearingTypeId.ToString()).ToJson(false));
                parameters.Add("@HearingDetailsId", hearingDetailsId);
                parameters.Add("@HearingTypeId", hearingTypeId);
                var result = _dapperConnection.Select<PTXboHearingResult>(StoredProcedureNames.usp_get_HearingResultByType, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetHearingResultByType-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetHearingResultByType-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboLitigationDetails GetLitigation(int invoiceID)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetLitigation-API  reached " + ((object)invoiceID).ToJson(false));
                parameters.Add("@LitigationID", invoiceID);

                var result = _dapperConnection.Select<PTXboLitigationDetails>(StoredProcedureNames.usp_get_LitigationDetails, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetLitigation-API  returns " + ((object)result).ToJson(false));
                Logger.For(this).Invoice("Invoice Calculation-GetLitigation-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetLitigation-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<PTXboYearlyHearings> GetYearlyHearingDetailsByAccountId(int accountID,int taxyear)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetLitigation-API  reached " + ((object)accountID).ToJson(false));
                parameters.Add("@AccountID", accountID);
                parameters.Add("@Taxyear", taxyear);
                var result = _dapperConnection.Select<PTXboYearlyHearings>(StoredProcedureNames.usp_GetYearlyHearingDetailsByAccount, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("Invoice Calculation-GetLitigation-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetLitigation-API  error " + ((object)ex).ToJson(false));

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public int SaveOrUpdateLitigation(PTXboLitigationDetails litigationDetails)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateLitigation-API  reached " + ((object)litigationDetails).ToJson(false));
                
                parameters.Add("@LitigationID", litigationDetails.LitigationID);
                parameters.Add("@CauseNo", litigationDetails.CauseNo);
                parameters.Add("@CauseName", litigationDetails.CauseName);
                parameters.Add("@TrailDate", litigationDetails.TrailDate);
                parameters.Add("@LateFiled", litigationDetails.LateFiled);
                parameters.Add("@DoNotContact", litigationDetails.DoNotContact);
                parameters.Add("@FilingDueDate", litigationDetails.FilingDueDate);
                parameters.Add("@SupplementName", litigationDetails.SupplementName);
                parameters.Add("@LitigationStatusID", litigationDetails.LitigationStatusID);
                parameters.Add("@AuthLetterDate", litigationDetails.AuthLetterDate);
                parameters.Add("@ExpertAssignedID", litigationDetails.ExpertAssignedID);
                parameters.Add("@FilingRequestDate", litigationDetails.FilingRequestDate);
                parameters.Add("@AnalystID", litigationDetails.AnalystID);
                parameters.Add("@AttorneyConfDate", litigationDetails.AttorneyConfDate);
                parameters.Add("@DocketReceivedDate", litigationDetails.DocketReceivedDate);
                parameters.Add("@InitialAgreedDate", litigationDetails.InitialAgreedDate);
                parameters.Add("@FinalAgreedDate", litigationDetails.FinalAgreedDate);
                parameters.Add("@BatchCauseNo", litigationDetails.BatchCauseNo);
                parameters.Add("@BatchCauseName", litigationDetails.BatchCauseName);
                parameters.Add("@ExpertsReportsDate", litigationDetails.ExpertsReportsDate);
                parameters.Add("@ExpertRemarks", litigationDetails.ExpertRemarks);
                parameters.Add("@SettlementDate", litigationDetails.SettlementDate);
                parameters.Add("@SupplementDate", litigationDetails.SupplementDate);
                parameters.Add("@LCLetterDate", litigationDetails.LCLetterDate);
                parameters.Add("@CourtNo", litigationDetails.CourtNo);
                parameters.Add("@CADIntroLetterDate", litigationDetails.CADIntroLetterDate);
                parameters.Add("@LitMarketValue", litigationDetails.LitMarketValue);
                parameters.Add("@Attorney", litigationDetails.Attorney);
                parameters.Add("@OIRSentDate", litigationDetails.OIRSentDate);
                parameters.Add("@LitAppraisalValue", litigationDetails.LitAppraisalValue);
                parameters.Add("@Judge", litigationDetails.Judge);
                parameters.Add("@OIRReceivedDate", litigationDetails.OIRReceivedDate);
                parameters.Add("@LitFinalized", litigationDetails.LitFinalized);
                parameters.Add("@ClientConfirmed", litigationDetails.ClientConfirmed);
                parameters.Add("@DateConfirmed", litigationDetails.DateConfirmed);
                parameters.Add("@DeedProvided", litigationDetails.DeedProvided);
                parameters.Add("@CADVerified", litigationDetails.CADVerified);
                parameters.Add("@AllottedUserID", litigationDetails.AllottedUserID);
                parameters.Add("@AllottedUserRoleID", litigationDetails.AllottedUserRoleID);
                parameters.Add("@AssignedBy", litigationDetails.AssignedBy);
                parameters.Add("@AssignedRoleID", litigationDetails.AssignedRoleID);
                parameters.Add("@AllottedDateTime", litigationDetails.AllottedDateTime);
                parameters.Add("@AssignedDateTime", litigationDetails.AssignedDateTime);
                parameters.Add("@CADVerificationUserID", litigationDetails.CADVerificationUserID);
                parameters.Add("@CADVerifiedDate", litigationDetails.CADVerifiedDate);
                parameters.Add("@LitRemarks", litigationDetails.LitRemarks);
                parameters.Add("@IsAccountMoved", litigationDetails.IsAccountMoved);
                parameters.Add("@LitReasonID", litigationDetails.LitReasonID);
                parameters.Add("@UpdatedBy", litigationDetails.UpdatedBy);
                parameters.Add("@UpdatedDate", litigationDetails.UpdatedDate);
                parameters.Add("@PESMarketValue", litigationDetails.PESMarketValue);
                parameters.Add("@PESUnequalAppraisalValue", litigationDetails.PESUnequalAppraisalValue);
                parameters.Add("@ExportedBy", litigationDetails.ExportedBy);
                parameters.Add("@IsDontCopy", litigationDetails.IsDontCopy);
                parameters.Add("@PropertyTaxYear", litigationDetails.PropertyTaxYear);
                parameters.Add("@PropertyTaxAccID", litigationDetails.PropertyTaxAccID);
                parameters.Add("@PFVerifiedDate", litigationDetails.PFVerifiedDate);
                parameters.Add("@PFVerifiedBy", litigationDetails.PFVerifiedBy);
                parameters.Add("@ClientConfirmDate", litigationDetails.ClientConfirmDate);
                parameters.Add("@NameChgSubmitDate", litigationDetails.NameChgSubmitDate);
                parameters.Add("@CovLtrSentDate", litigationDetails.CovLtrSentDate);
                parameters.Add("@DiscoverySentDate", litigationDetails.DiscoverySentDate);
                parameters.Add("@SpecialFilingAccount", litigationDetails.SpecialFilingAccount);
                parameters.Add("@LITInvoiceStatusid", litigationDetails.LITInvoiceStatusid);
                parameters.Add("@RecievedAuthStatus", litigationDetails.RecievedAuthStatus);
                parameters.Add("@ExemptInvoice", litigationDetails.ExemptInvoice);
                parameters.Add("@FloodLitTypeID", litigationDetails.FloodLitTypeID);
                parameters.Add("@Appraiserid", litigationDetails.Appraiserid);
                parameters.Add("@AppraisalOrderCreated", litigationDetails.AppraisalOrderCreated);
                parameters.Add("@AppraisalCompleted", litigationDetails.AppraisalCompleted);
                parameters.Add("@AppraisalValue", litigationDetails.AppraisalValue);
                parameters.Add("@ExpressAgentID", litigationDetails.ExpressAgentID);
                parameters.Add("@ExpressOfferNotAccepted", litigationDetails.ExpressOfferNotAccepted);
                parameters.Add("@NoExpressOffer", litigationDetails.NoExpressOffer);
                parameters.Add("@SettlememtConferenceDate", litigationDetails.SettlememtConferenceDate);
                parameters.Add("@SettlementOffer", litigationDetails.SettlementOffer);
                parameters.Add("@TwoWeekTrial", litigationDetails.TwoWeekTrial);
                parameters.Add("@HardTrialDate", litigationDetails.HardTrialDate);
                parameters.Add("@HardTrialTime", litigationDetails.HardTrialTime);
                parameters.Add("@DiscoveryDeadline", litigationDetails.DiscoveryDeadline);
                parameters.Add("@CADUEValue", litigationDetails.CADUEValue);
                parameters.Add("@CADAppraisedValue", litigationDetails.CADAppraisedValue);
                parameters.Add("@PAPESRatioStudy", litigationDetails.PAPESRatioStudy);
                parameters.Add("@TargetValue", litigationDetails.TargetValue);
                parameters.Add("@TriggerValue", litigationDetails.TriggerValue);
                parameters.Add("@Projectid", litigationDetails.Projectid);


                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateLitigation, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateLitigation-API  returns " + ((object)result).ToJson(false));
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateLitigation-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateLitigation-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboArbitration GetArbitrationDetails(int invoiceID)
        {
            Hashtable parameters = new Hashtable();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetArbitrationDetails-API  reached " + ((object)invoiceID).ToJson(false));
                parameters.Add("@ArbitrationDetailsId", invoiceID);

                var result = _dapperConnection.Select<PTXboArbitration>(StoredProcedureNames.usp_get_ArbitrationDetails, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetArbitrationDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetArbitrationDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public bool SaveOrUpdateArbitrationDetails(PTXboArbitration arbitrationDetails)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateArbitrationDetails-API  reached " + ((object)arbitrationDetails).ToJson(false));
                parameters.Add("@ArbitrationDetailsId", arbitrationDetails.ArbitrationDetailsId);
                parameters.Add("@ArbitrationId", arbitrationDetails.ArbitrationId);
                parameters.Add("@RequestDate", arbitrationDetails.RequestDate);
                parameters.Add("@RequestedByAgent", arbitrationDetails.RequestedByAgent);
                parameters.Add("@InitialArbitrationDeadLineDate", arbitrationDetails.InitialArbitrationDeadLineDate);
                parameters.Add("@FinalArbitrationDeadLineDate", arbitrationDetails.FinalArbitrationDeadLineDate);
                parameters.Add("@Approved", arbitrationDetails.Approved);
                parameters.Add("@ApprovedByAgent", arbitrationDetails.ApprovedByAgent);
                parameters.Add("@TargetValue", arbitrationDetails.TargetValue);
                parameters.Add("@AssignedAgentId", arbitrationDetails.AssignedAgentId);
                parameters.Add("@DeniedReasonId", arbitrationDetails.DeniedReasonId);
                parameters.Add("@ArbitrationOrder", arbitrationDetails.ArbitrationOrder);
                parameters.Add("@IndexedArbitrationPackageDocumentId", arbitrationDetails.IndexedArbitrationPackageDocumentId);
                parameters.Add("@GeoId", arbitrationDetails.GeoId);
                parameters.Add("@ComptrollerLetterDate", arbitrationDetails.ComptrollerLetterDate);
                parameters.Add("@FilingDate", arbitrationDetails.FilingDate);
                parameters.Add("@EvidenceDueDate", arbitrationDetails.EvidenceDueDate);
                parameters.Add("@CADEvidenceReceivedDate", arbitrationDetails.CADEvidenceReceivedDate);
                parameters.Add("@EvidenceSentDate", arbitrationDetails.EvidenceSentDate);
                parameters.Add("@RebuttalEvidenceDueDate", arbitrationDetails.RebuttalEvidenceDueDate);
                parameters.Add("@CADRebuttalEvidenceReceivedDate", arbitrationDetails.CADRebuttalEvidenceReceivedDate);
                parameters.Add("@RebuttalEvidenceSentDate", arbitrationDetails.RebuttalEvidenceSentDate);
                parameters.Add("@CADAppraiser", arbitrationDetails.CADAppraiser);
                parameters.Add("@ArbitratorName", arbitrationDetails.ArbitratorName);
                parameters.Add("@ArbitratorAddress", arbitrationDetails.ArbitratorAddress);
                parameters.Add("@ArbitratorPhone", arbitrationDetails.ArbitratorPhone);
                parameters.Add("@ArbitratorFax", arbitrationDetails.ArbitratorFax);
                parameters.Add("@ArbitratorEmail", arbitrationDetails.ArbitratorEmail);
                parameters.Add("@SettlementDate", arbitrationDetails.SettlementDate);
                parameters.Add("@FinalAmount", arbitrationDetails.FinalAmount);
                parameters.Add("@ArbitrationStatusId", arbitrationDetails.ArbitrationStatusId);
                parameters.Add("@OwnerOrAgnetNumber", arbitrationDetails.OwnerOrAgnetNumber);
                parameters.Add("@HearingDate", arbitrationDetails.HearingDate);
                parameters.Add("@HearingTime", arbitrationDetails.HearingTime);
                parameters.Add("@BatchPrintDate", arbitrationDetails.BatchPrintDate);
                parameters.Add("@Photos", arbitrationDetails.Photos);
                parameters.Add("@Finalized", arbitrationDetails.Finalized);
                parameters.Add("@InvoiceId", arbitrationDetails.InvoiceId);
                parameters.Add("@ArbitrationAgent", arbitrationDetails.ArbitrationAgent);
                parameters.Add("@ArbitrationRemarks", arbitrationDetails.ArbitrationRemarks);
                parameters.Add("@RefferalMethodID", arbitrationDetails.RefferalMethodID);
                parameters.Add("@Status", arbitrationDetails.Status);
                parameters.Add("@FeePaidByOCA", arbitrationDetails.FeePaidByOCA);
                parameters.Add("@FeePaidByClient", arbitrationDetails.FeePaidByClient);
                parameters.Add("@WelcomeLetterDate", arbitrationDetails.WelcomeLetterDate);
                parameters.Add("@ProblemAofAId", arbitrationDetails.ProblemAofAId);
                parameters.Add("@PropertyTaxYear", arbitrationDetails.PropertyTaxYear);
                parameters.Add("@PropertyTaxAccID", arbitrationDetails.PropertyTaxAccID);
                parameters.Add("@UpdatedBy", arbitrationDetails.UpdatedBy);
                parameters.Add("@UpdatedDatetime", arbitrationDetails.UpdatedDatetime);
                parameters.Add("@CADLegalFirstName", arbitrationDetails.CADLegalFirstName);
                parameters.Add("@CADLegalLastName", arbitrationDetails.CADLegalLastName);
                parameters.Add("@MiddleInitial", arbitrationDetails.MiddleInitial);
                parameters.Add("@Suffix", arbitrationDetails.Suffix);
                parameters.Add("@IsAgreementonFile", arbitrationDetails.IsAgreementonFile);
                parameters.Add("@OCALucTypeId", arbitrationDetails.OCALucTypeId);
                parameters.Add("@ARBInvoiceStatusid", arbitrationDetails.ARBInvoiceStatusid);
                parameters.Add("@InitialHearingDate", arbitrationDetails.InitialHearingDate);
                parameters.Add("@InitialHearingTime", arbitrationDetails.InitialHearingTime);
                parameters.Add("@WithdrawalDeadlineDate", arbitrationDetails.WithdrawalDeadlineDate);
                parameters.Add("@MarketValueTarget", arbitrationDetails.MarketValueTarget);
                parameters.Add("@UnequalAppraisalValueTarget", arbitrationDetails.UnequalAppraisalValueTarget);
                parameters.Add("@SoftTarget", arbitrationDetails.SoftTarget);
                parameters.Add("@MarketValue", arbitrationDetails.MarketValue);
                parameters.Add("@ArbitratorLocation", arbitrationDetails.ArbitratorLocation);
                parameters.Add("@WithdrawalDate", arbitrationDetails.WithdrawalDate);
                parameters.Add("@FileFeeAmount", arbitrationDetails.FileFeeAmount);
                parameters.Add("@FileFeeChequeNumber", arbitrationDetails.FileFeeChequeNumber);
                parameters.Add("@RefundsReceivedDate", arbitrationDetails.RefundsReceivedDate);
                parameters.Add("@RefundAmount", arbitrationDetails.RefundAmount);
                parameters.Add("@ChequeNumber", arbitrationDetails.ChequeNumber);
                parameters.Add("@Rescheduling", arbitrationDetails.Rescheduling);
                parameters.Add("@EvidenceCreated", arbitrationDetails.EvidenceCreated);
                parameters.Add("@SoftTargetUpdatedAgentId", arbitrationDetails.SoftTargetUpdatedAgentId);
                parameters.Add("@MarketValueUpdatedAgentId", arbitrationDetails.MarketValueUpdatedAgentId);
                parameters.Add("@ExemptInvoice", arbitrationDetails.ExemptInvoice);
                parameters.Add("@ArbitratorId", arbitrationDetails.ArbitratorId);
                parameters.Add("@IntialReviewGoodUE", arbitrationDetails.IntialReviewGoodUE);
                parameters.Add("@IntialReviewGoodSales", arbitrationDetails.IntialReviewGoodSales);
                parameters.Add("@OCAConfRoom", arbitrationDetails.OCAConfRoom);
                parameters.Add("@BACadConfLtrDt", arbitrationDetails.BACadConfLtrDt);
                parameters.Add("@HNRcvdDate", arbitrationDetails.HNRcvdDate);
                parameters.Add("@SettlementOffer", arbitrationDetails.SettlementOffer);
                parameters.Add("@SettlementOfferDate", arbitrationDetails.SettlementOfferDate);
                parameters.Add("@TaxesPaid", arbitrationDetails.TaxesPaid);
                parameters.Add("@AwardReceiptDate", arbitrationDetails.AwardReceiptDate);
                parameters.Add("@AssignedAgentAlternateId", arbitrationDetails.AssignedAgentAlternateId);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateArbitrationDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateArbitrationDetails-API  ends successfully ");
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateArbitrationDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        //special term Invoice generation
        public bool SubmitForInvoiceGeneration(int currentUserId, int currentUserRoleID, PTXboSpecialTermInvoiceDetails objdetails, out string errorMessage)
        {
            errorMessage = string.Empty;

            bool Issucess = true;

            try
            { 
           // List<PTXboInvoiceDetails> lstInvoiceDetails = new List<PTXboInvoiceDetails>();

           // PTXboInvoiceDetails invoiceDetails = new PTXboInvoiceDetails();
            Logger.For(this).Invoice("Invoice Calculation-SubmitForInvoiceGeneration-API  reached " + ((object)objdetails).ToJson(false));
            foreach (var item in objdetails.lstTermAccounts)
            {
                var YearlyHearingDetails = GetYearlyHearingDetails(item.YearlyHearingDetailsId);

                int projectid = 0;
                if (YearlyHearingDetails.ProjectId != 0)
                {
                    projectid = YearlyHearingDetails.ProjectId;
                }

                //invoiceDetails = new PTXboInvoiceDetails();
                //invoiceDetails.InvoiceTypeId = objdetails.TermType;
                //invoiceDetails.ClientId = Convert.ToInt32(YearlyHearingDetails.ClientId);
                //invoiceDetails.Taxyear = Convert.ToInt32(YearlyHearingDetails.TaxYear);
                //invoiceDetails.InvoiceId = Convert.ToInt32(item.InvoiceId);
                //invoiceDetails.updatedBy = currentUserId;
                //invoiceDetails.ManualGeneratedUseriD = currentUserId;
                //invoiceDetails.ManualGeneratedUserRoleID = currentUserRoleID;
                //invoiceDetails.InvoiceTypeId = objdetails.TermType;
                //invoiceDetails.ProjectId = projectid;
                    
                    //Added by saravanans.tfs id:56388
                    //if (objdetails.TermType!=1)
                    //{
                    //    objdetails.IsRegenerateInvoice = false;
                    //}
                    //Ends here.

                 //invoiceDetails.IsRegenerateInvoice = objdetails.IsRegenerateInvoice==true?1:0;//Added by saravanans.tfs id:55312
                //lstInvoiceDetails.Add(invoiceDetails);


                PTXboInvoice objInvoiceFromHearingResult = new PTXboInvoice();
                objInvoiceFromHearingResult.ClientId = Convert.ToInt32(YearlyHearingDetails.ClientId);
                objInvoiceFromHearingResult.TaxYear = Convert.ToInt32(YearlyHearingDetails.TaxYear);
                objInvoiceFromHearingResult.CreatedBy = currentUserId;
                objInvoiceFromHearingResult.InvoiceID = Convert.ToInt32(item.InvoiceId);
                objInvoiceFromHearingResult.InvoiceTypeId = objdetails.TermType;
                objInvoiceFromHearingResult.ProjectId = projectid;
                objInvoiceFromHearingResult.ContingencyPercentage = Convert.ToSingle(objdetails.Contigency);//
                objInvoiceFromHearingResult.FlatFee = objdetails.FlatFee;
                objInvoiceFromHearingResult.InvoiceAmount = objdetails.InvoiceAmount;
                objInvoiceFromHearingResult.IsSpecialTerm = true;
                objInvoiceFromHearingResult.CanGenerateInvoice = objdetails.CanGenerateInvoice;
                objInvoiceFromHearingResult.DontGenerateInvoiceFlag = objdetails.DontGenerateInvoiceFlag;
                objInvoiceFromHearingResult.IsRegenerateInvoice = objdetails.IsRegenerateInvoice==true?1:0;//Added by saravanans.tfs id:55312
                objInvoiceFromHearingResult.InvoiceDescription = objdetails.InvoiceDescription;//Added by SaravananS. tfs id:56933
                  //Added by SaravananS. tfs id:63613
                    objInvoiceFromHearingResult.CanUseCurrentYearTaxrate = objdetails.CanUseCurrentYearTaxrate;
                    if(objdetails.CanUseCurrentYearTaxrate)
                    {
                        objInvoiceFromHearingResult.PriorYearTaxRate = objdetails.Taxrate;
                    }
                    //Ends here.

                    switch (objdetails.TermType)
                {
                    case (int)Enumerators.PTXenumTermsType.Standard:
                        objInvoiceFromHearingResult.HearingResultId = item.ID;
                        break;
                    case (int)Enumerators.PTXenumTermsType.Litigation:
                        objInvoiceFromHearingResult.LitigationId = item.ID;

                        break;
                    case (int)Enumerators.PTXenumTermsType.Arbritration:
                        objInvoiceFromHearingResult.ArbitrationDetilId = item.ID;
                        break;
                }

                if (objdetails.TermType != (int)Enumerators.PTXenumTermsType.Standard)
                {

                    var hearingresult = GetHearingResultByType(YearlyHearingDetails.YearlyHearingDetailsId, 9);

                    if (hearingresult != null)
                    {
                        objInvoiceFromHearingResult.HearingResultId = hearingresult.HearingResultId;
                    }
                }

                    objInvoiceFromHearingResult.TaxYear = Convert.ToInt32(YearlyHearingDetails.TaxYear);// invoiceDetails.Taxyear;
                Issucess = InsertInvoiceData(objInvoiceFromHearingResult, out errorMessage);

                //Added by Boopathi
                //Update The Invoice Status as 'DoNotGenerate Invoice'
                if (String.IsNullOrEmpty(errorMessage))
                {
                    switch (objdetails.TermType)
                    {
                        case (int)Enumerators.PTXenumTermsType.Standard:
                            //Set DontGenerate Flag For HRInvoiceStatusField
                            if (objInvoiceFromHearingResult.DontGenerateInvoiceFlag)
                            {
                                var obj = GetHearingResultByType(objInvoiceFromHearingResult.HearingResultId);
                                //Repository<PTXdoHearingResult>.GetQuery().FirstOrDefault(x => x.HearingResultsId == objInvoiceFromHearingResult.HearingResultId);
                                if (obj != null)
                                {
                                    obj.HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.DoNotGenerateInvoice.GetId();
                                    SaveOrUpdateHearingResult(obj);

                                }
                            }
                            break;
                        case (int)Enumerators.PTXenumTermsType.Litigation:
                            //Set DontGenerate Flag For LitigationInvoiceStatusField
                            if (objInvoiceFromHearingResult.DontGenerateInvoiceFlag)
                            {
                                var obj = GetLitigation(objInvoiceFromHearingResult.LitigationId);
                                //Repository<PTXboLitigation>.GetQuery().FirstOrDefault(x => x.LitigationID == objInvoiceFromHearingResult.LitigationId);
                                if (obj != null)
                                {
                                    //obj.LitigationStatusID = PTXdoenumHRInvoiceStatus.DoNotGenerateInvoice.GetId();
                                    obj.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.DoNotGenerateInvoice.GetId();
                                    obj.LitigationID =SaveOrUpdateLitigation(obj);

                                }
                            }
                            break;
                        case (int)Enumerators.PTXenumTermsType.Arbritration:
                            //Set DontGenerate Flag For ArbritrationInvoiceStatusField
                            if (objInvoiceFromHearingResult.DontGenerateInvoiceFlag)
                            {
                                var obj = GetArbitrationDetails(objInvoiceFromHearingResult.ArbitrationDetilId);
                                //Repository<PTXboArbitrationDetails>.GetQuery().FirstOrDefault(x => x.ArbitrationDetailsId == objInvoiceFromHearingResult.ArbitrationDetilId);
                                if (obj != null)
                                {
                                    //obj.ArbitrationStatusId = PTXdoenumHRInvoiceStatus.DoNotGenerateInvoice.GetId();
                                    obj.ARBInvoiceStatusid = PTXdoenumHRInvoiceStatus.DoNotGenerateInvoice.GetId();
                                    SaveOrUpdateArbitrationDetails(obj);
                                }
                            }
                            break;
                    }

                }
            }
            Logger.For(this).Invoice("Invoice Calculation-SubmitForInvoiceGeneration-API  ends successfully ");
            return Issucess;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitForInvoiceGeneration-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboInvoice GetInvoiceDetailsForTermLevel(int invoiceGroupID, int invoiceGroupingTypeID, int invoiceTypeID, int invoiceStatusID, int taxYear)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsForTermLevel-API  reached " + ((object)"invoiceGroupID=" + invoiceGroupID.ToString() + "invoiceGroupingTypeID=" + invoiceGroupingTypeID.ToString() + "invoiceTypeID=" + invoiceTypeID.ToString() + "invoiceStatusID=" + invoiceStatusID.ToString() + "taxYear=" + taxYear.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                PTXboInvoice invoiceDetails = new PTXboInvoice();
                parameters.Add("@GroupID", invoiceGroupID);
                parameters.Add("@InvoiceGroupingTypeID", invoiceGroupingTypeID);
                parameters.Add("@InvoiceTypeID", invoiceTypeID);
                parameters.Add("@InvoiceStatusID", invoiceStatusID);
                parameters.Add("@TaxYear", taxYear);
                invoiceDetails = _dapperConnection.Select<PTXboInvoice>(StoredProcedureNames.usp_get_InvoicedetailsForTermLevel, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsForTermLevel-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsForTermLevel-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public PTXboInvoice GetInvoiceDetailsForProjectLevel(int invoiceGroupID, int invoiceGroupingTypeID, int projectID, int invoiceTypeID, int invoiceStatusID, int taxYear)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsForProjectLevel-API  reached " + ((object)"invoiceGroupID=" + invoiceGroupID.ToString() + "invoiceGroupingTypeID=" + invoiceGroupingTypeID.ToString() + "projectID=" + projectID.ToString() + "invoiceTypeID=" + invoiceTypeID.ToString() + "invoiceStatusID=" + invoiceStatusID.ToString() + "taxYear=" + taxYear.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                PTXboInvoice invoiceDetails = new PTXboInvoice();
                parameters.Add("@ProjectID", projectID);
                parameters.Add("@GroupID", invoiceGroupID);
                parameters.Add("@InvoiceGroupingTypeID", invoiceGroupingTypeID);
                parameters.Add("@InvoiceTypeID", invoiceTypeID);
                parameters.Add("@InvoiceStatusID", invoiceStatusID);
                parameters.Add("@TaxYear", taxYear);
                invoiceDetails = _dapperConnection.Select<PTXboInvoice>(StoredProcedureNames.usp_get_InvoicedetailsForProjectLevel, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsForProjectLevel-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetInvoiceDetailsForProjectLevel-API  error " + ((object)ex).ToJson(false));
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
        public bool InsertInvoiceData(PTXboInvoice objInvoiceFromHearingResult, out string errorMessage)
        {
            errorMessage = string.Empty;
            int HearingResultID = objInvoiceFromHearingResult.HearingResultId;
            int TaxBillAuditId = objInvoiceFromHearingResult.TaxBillAuditId;
            int BppRenditionId = objInvoiceFromHearingResult.BppRenditionId;
            //Added by SaravananS. tfs id:63634
            int arbitrationDetailsId = (objInvoiceFromHearingResult.InvoiceTypeId == 8) ? objInvoiceFromHearingResult.SOAHDetailsID : objInvoiceFromHearingResult.ArbitrationDetilId;
            //Ends here.
            List<PTXboInvoice> lstInvoiceDetails = new List<PTXboInvoice>();
            List<PTXboInvoice> objExceptValueHearingType = new List<PTXboInvoice>();
            bool isInvoiceRecordCreated = false;
            short valueHearingType = Enumerators.PTXenumHearingType.ValueHearing.GetId();
            string hearingType = string.Empty;
            string invoicingGroupType = string.Empty;
            //Added by SaravananS. tfs id:63335
            bool isDisasterInvoice = false;
            if (objInvoiceFromHearingResult.HearingTypeId == 9)
            {
                isDisasterInvoice = true;
            }
            //Ends here.
            //Added by Mohanapriya s for TFS Id : 62983 starts here
            if (objInvoiceFromHearingResult != null && objInvoiceFromHearingResult.InvoiceID > 0)
            {
                objInvoiceFromHearingResult.IsOutOfTexas = IsOutOfTexas(objInvoiceFromHearingResult.InvoiceID);
                objInvoiceFromHearingResult.IsILHearing = IsILHearing(objInvoiceFromHearingResult.InvoiceID);
            }

            //Added by Mohanapriya s for TFS Id : 62983 ends here
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceData-API  reached " + ((object)objInvoiceFromHearingResult).ToJson(false));
                // Edited K.Selva 
                if (objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId()
                    && !objInvoiceFromHearingResult.IsILHearing //Added by SaravananS. tfs id:63335
                    )
                {
                    /** Get the Hearing Type **/
                    var objHearingType = GetHearingResultByType(HearingResultID);
                    hearingType = (objHearingType != null) ? objHearingType.HearingTypeId.ToString() : "";
                }
                //Added by SaravananS. tfs id:63335
                else if (objInvoiceFromHearingResult.IsOutOfTexas == true && objInvoiceFromHearingResult.IsILHearing == true)
                {
                    hearingType = GetHearingType(objInvoiceFromHearingResult.HearingTypeId);
                }
                //Ends here.

                /*Getting Account, group, term details based on the client, tax year for the standard term*/
                lstInvoiceDetails = GetInvoiceGenerationInputData(objInvoiceFromHearingResult.ClientId, objInvoiceFromHearingResult.TaxYear, objInvoiceFromHearingResult.InvoiceTypeId
                    ,objInvoiceFromHearingResult.IsOutOfTexas); //Added by SaravananS. tfs id:63335

                List<PTXboInvoice> objInvoiceDetails = new List<PTXboInvoice>();

                // K.Selva - For Getting BPPRenditionID  from Invoice Details //starts

                //Added By Boopathi.S --Checks Null Condition
                if (lstInvoiceDetails != null)
                {
                    if (objInvoiceFromHearingResult.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
                    {
                        objInvoiceDetails = lstInvoiceDetails.Where(a => a.BppRenditionId == BppRenditionId).ToList();
                    }
                    //For Getting TaxBillAuditID  from Invoice Details
                    else if (objInvoiceFromHearingResult.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                    {
                        objInvoiceDetails = lstInvoiceDetails.Where(a => a.TaxBillAuditId == TaxBillAuditId).ToList();
                    }
                    //Added by SaravananS. tfs id:63335
                    else if (objInvoiceFromHearingResult.IsOutOfTexas == true && objInvoiceFromHearingResult.IsILHearing == true)
                    {
                        objInvoiceDetails = lstInvoiceDetails.Where(a => a.HearingResultId == objInvoiceFromHearingResult.ILHearingDetailsId).ToList();
                    }
                    //Ends here.
                    //Added by SaravananS. tfs id:63634
                    else if (objInvoiceFromHearingResult.InvoiceTypeId == Enumerators.PTXenumInvoiceType.SOAH.GetId())
                    {
                        objInvoiceDetails = (lstInvoiceDetails != null) ? lstInvoiceDetails.Where(a => ((objInvoiceFromHearingResult.InvoiceTypeId == 8) ? a.SOAHDetailsID : a.ArbitrationDetilId) == arbitrationDetailsId).ToList() : new List<PTXboInvoice>();
                    }
                    //Ends here.
                    else
                    {
                        /*Select the hearing finalized record*/
                        objInvoiceDetails = lstInvoiceDetails.Where(a => a.HearingResultId == HearingResultID).ToList();
                    }
                }
                // Ends
                if (objInvoiceDetails.Count > 0)
                {
                    foreach (PTXboInvoice objInvoice in objInvoiceDetails)
                    {
                        //Added by SaravananS. tfs id:63335
                        if (objInvoiceFromHearingResult.IsRegenerateInvoice == null)
                        {
                            objInvoiceFromHearingResult.IsRegenerateInvoice = 0;
                        }
                        objInvoice.IsILHearing = objInvoiceFromHearingResult.IsILHearing;
                        //Ends here.

                        //Added by saravanans.tfs id:56388
                        //if (objInvoice.InvoiceTypeId!=1)
                        //{
                        //    objInvoiceFromHearingResult.IsRegenerateInvoice = 0;
                        //}
                        //Ends here.

                        objInvoice.InvoiceID = (objInvoiceFromHearingResult.IsRegenerateInvoice == 1) ? 0 : objInvoice.InvoiceID; //Product Backlog Item 34480:Spartaxx | DE - Invoice re-generation - Hearing finalized                      
                        objInvoice.CanGenerateInvoice = objInvoiceFromHearingResult.CanGenerateInvoice;
                        objInvoice.IsSpecialTerm = objInvoiceFromHearingResult.IsSpecialTerm;
                        objInvoice.DontGenerateInvoiceFlag = objInvoiceFromHearingResult.DontGenerateInvoiceFlag;
                        objInvoice.IsRegenerateInvoice = objInvoiceFromHearingResult.IsRegenerateInvoice;

                        

                        //ends here..

                        if (objInvoice.IsSpecialTerm)
                        {
                            //Added by SaravananS. tfs id:63613
                            objInvoice.CanUseCurrentYearTaxrate = objInvoiceFromHearingResult.CanUseCurrentYearTaxrate;
                            if(objInvoiceFromHearingResult.CanUseCurrentYearTaxrate)
                            {
                                objInvoice.PriorYearTaxRate = objInvoiceFromHearingResult.PriorYearTaxRate;
                            }
                            //Ends here.

                            objInvoice.InvoiceAmount = objInvoiceFromHearingResult.InvoiceAmount;
                            objInvoice.ContingencyPercentage = objInvoiceFromHearingResult.ContingencyPercentage;
                            objInvoice.FlatFee = objInvoiceFromHearingResult.FlatFee;
                            objInvoice.InvoiceDescription = objInvoiceFromHearingResult.InvoiceDescription;//Added by SaravananS. tfs id:56933
                        }

                        //out of texas //Added by SaravananS. tfs id:63335
                        if (objInvoiceFromHearingResult.IsOutOfTexas)
                        {
                            objInvoice.IsOutOfTexas = objInvoiceFromHearingResult.IsOutOfTexas;
                            objInvoice.InitialAssessedValue = objInvoiceFromHearingResult.InitialAssessedValue;
                            objInvoice.FinalAssessedValue = objInvoiceFromHearingResult.FinalAssessedValue;
                            objInvoice.PriorYearTaxRate = objInvoiceFromHearingResult.PriorYearTaxRate;
                            objInvoice.AssessmentRatio = objInvoiceFromHearingResult.AssessmentRatio;
                        }

                        if (objInvoiceFromHearingResult.IsOutOfTexas == true && objInvoiceFromHearingResult.IsILHearing == true && objInvoice.ContingencyPercentage > 0)
                        {
                            //Added by SaravananS. tfs id:61646
                            objInvoice.StateEvaValue = objInvoiceFromHearingResult.StateEvaValue;
                            objInvoice.ContingencyPercentage = objInvoice.ContingencyPercentage < 1 ? (objInvoice.ContingencyPercentage * 100) : objInvoice.ContingencyPercentage;
                            //Ends here.
                            objInvoice.InvoiceAmount = (Convert.ToDecimal(objInvoiceFromHearingResult.Reduction) * Convert.ToDecimal(objInvoice.PriorYearTaxRate)) * Convert.ToDecimal(objInvoice.ContingencyPercentage);
                            objInvoice.ContingencyFee = (Convert.ToDecimal(objInvoiceFromHearingResult.Reduction) * Convert.ToDecimal(objInvoice.PriorYearTaxRate)) * Convert.ToDecimal(objInvoice.ContingencyPercentage);
                            objInvoice.AmountDue = (Convert.ToDecimal(objInvoiceFromHearingResult.Reduction) * Convert.ToDecimal(objInvoice.PriorYearTaxRate)) * Convert.ToDecimal(objInvoice.ContingencyPercentage);
                            objInvoice.IsILHearing = objInvoiceFromHearingResult.IsILHearing;
                        }

                        if (objInvoiceFromHearingResult.IsSpecialTerm && objInvoiceFromHearingResult.InvoiceID > 0)
                            objInvoice.InvoiceID = (objInvoiceFromHearingResult.IsRegenerateInvoice == 1) ? 0 : objInvoiceFromHearingResult.InvoiceID; 

                        //Ends here.

                        //Added by saravanans. tfs id:63335
                        if(objInvoiceFromHearingResult.IsRegenerateInvoice == 1 && objInvoiceFromHearingResult.IsOutOfTexas == true)
                        {
                            objInvoice.InvoiceDescription = objInvoiceFromHearingResult.InvoiceDescription;
                        }
                        //Ends here.

                        //if (objInvoiceFromHearingResult.IsSpecialTerm && objInvoiceFromHearingResult.InvoiceID > 0)
                        //objInvoice.InvoiceID = (objInvoiceFromHearingResult.IsRegenerateInvoice == 1) ? 0 : objInvoiceFromHearingResult.InvoiceID; //Product Backlog Item 34480:Spartaxx | DE - Invoice re-generation - Hearing finalized                        

                        if (objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.GetId())
                        {
                            invoicingGroupType = Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString();
                            isInvoiceRecordCreated = InsertAccountLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult.CreatedBy).IsInvoiceRecordCreated;
                        }

                        else if (objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
                        {
                            invoicingGroupType = Enumerators.PTXenumInvoiceGroupingType.TermLevel.ToString();

                            //Added by Kishore to check whether Invoice already exist to Pervent Duplicate Invoices while Updating
                            if (objInvoice.IsSpecialTerm)
                            {
                                
                                //if (objInvoiceFromHearingResult.IsRegenerateInvoice == 1)
                                //{
                                //    objInvoice.InvoiceID = 0;
                                //}

                                //else
                                //{
                                  var Invoice = GetInvoiceDetailsForTermLevel(objInvoice.GroupId, objInvoice.InvoiceGroupingTypeId, objInvoice.InvoiceTypeId, Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId(), objInvoice.TaxYear);
                                  if (Invoice != null)
                                      objInvoice.InvoiceID = Invoice.InvoiceID;
                               // }
                            }

                            if (objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                            {
                                //For Other HearingType consider those accounts as Account level and insert an Invoice.
                                objExceptValueHearingType = lstInvoiceDetails.Where(h => h.GroupId == objInvoice.GroupId && h.HearingTypeId != valueHearingType && h.HearingFinalized == true).ToList();
                                if (objExceptValueHearingType.Count > 0)
                                {
                                    isInvoiceRecordCreated = InsertAccountLevelInvoiceGeneration(objInvoice, objExceptValueHearingType, hearingType, invoicingGroupType, objInvoiceFromHearingResult.CreatedBy).IsInvoiceRecordCreated;
                                }

                                //Value Hearing type Invoice Generation
                                lstInvoiceDetails = lstInvoiceDetails.Where(a => a.GroupId == objInvoice.GroupId && a.HearingTypeId == valueHearingType).ToList();
                                if (lstInvoiceDetails.Count > 0)
                                {
                                    isInvoiceRecordCreated = InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult, out errorMessage);
                                }
                            }
                            else
                            {
                                lstInvoiceDetails = lstInvoiceDetails.Where(a => a.GroupId == objInvoice.GroupId).ToList();
                                if (lstInvoiceDetails.Any())
                                    isInvoiceRecordCreated = InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult, out errorMessage);
                            }

                        }

                        else if (objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
                        {
                            invoicingGroupType = Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.ToString();
                            //Added by Kishore to check whether Invoice already exist to Pervent Duplicate Invoices while Updating
                            if (objInvoice.IsSpecialTerm)
                            {
                                //Product Backlog Item 34480:Spartaxx | DE - Invoice re-generation - Hearing finalized
                                //if (objInvoiceFromHearingResult.IsRegenerateInvoice == 1)
                                //{
                                //    objInvoice.InvoiceID = 0;
                                //}

                                //else
                                //{
                                    var Invoice = GetInvoiceDetailsForProjectLevel(objInvoice.GroupId, objInvoice.InvoiceGroupingTypeId, objInvoice.ProjectId, objInvoice.InvoiceTypeId, Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId(), objInvoice.TaxYear);
                                    if (Invoice != null)
                                        objInvoice.InvoiceID = Invoice.InvoiceID;
                               // }
                            }

                            //For Other HearingType consider those accounts as Account level and insert an Invoice.
                            if (objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                            {
                                objExceptValueHearingType = lstInvoiceDetails.Where(h => h.ProjectId == objInvoice.ProjectId && h.HearingTypeId != valueHearingType && h.HearingFinalized == true).ToList();
                                if (objExceptValueHearingType.Count > 0)
                                {
                                    isInvoiceRecordCreated = InsertAccountLevelInvoiceGeneration(objInvoice, objExceptValueHearingType, hearingType, invoicingGroupType, objInvoiceFromHearingResult.CreatedBy).IsInvoiceRecordCreated;
                                }

                                //Value Hearing type Invoice Generation
                                lstInvoiceDetails = lstInvoiceDetails.Where(a => a.ProjectId == objInvoice.ProjectId && a.HearingTypeId == valueHearingType).ToList();
                                if (lstInvoiceDetails.Count > 0)
                                {
                                    isInvoiceRecordCreated = InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult, out errorMessage);
                                }
                            }
                            else
                            {
                                lstInvoiceDetails = lstInvoiceDetails.Where(a => a.ProjectId == objInvoice.ProjectId).ToList();
                                if (lstInvoiceDetails.Count > 0)
                                    isInvoiceRecordCreated = InsertTermOrProjectLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult, out errorMessage);
                            }
                        }
                    }
                }
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceData-API  ends successfully ");
                return isInvoiceRecordCreated;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceData-API  error " + ((object)ex).ToJson(false));
                throw ex;

            }
            finally
            {
                Dispose();
            }
        }

       


        public bool InsertTermOrProjectLevelInvoiceGeneration(PTXboInvoice objInvoice, List<PTXboInvoice> lstInvoiceDetails, string hearingType, string invoicingGroupingType, PTXboInvoice objInvoiceFromHearingResult, out string errorMessage)
        {
            try
            { 
            PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
            PTXboInvoice objInsertInvoiceData = new PTXboInvoice();
            errorMessage = string.Empty;
            decimal TotalNoticedValue = 0;
            decimal TotalPostHearingValue = 0;
            decimal TotalReduction = 0;
            double TotalPriorYearTaxRate = 0;
            decimal TotalEstimatedTaxSavings = 0;
            decimal? TotalInvoiceAmount = 0;
            decimal Reduction = 0;
            bool IsInvoiceDataValid = false;
            bool IsInvoiceValidationFails = false;
            bool IsReduction = false;
            bool isInvoiceRecordCreated = false;
            int NewlyCreatedInvoiceID = 0;
            int HearingResultFinalizednotFinalizedCount = 0;
            decimal InvoiceCapValue = 0;
            decimal InvoiceCalAmount = 0;

            Logger.For(this).Invoice("Invoice Calculation-InsertTermOrProjectLevelInvoiceGeneration-API  reached " + ((object)lstInvoiceDetails).ToJson(false));

            if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
            {
                HearingResultFinalizednotFinalizedCount = lstInvoiceDetails.Where(a => a.BppRenditionId == 0).ToList().Count;
            }
            else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
            {
                HearingResultFinalizednotFinalizedCount = lstInvoiceDetails.Where(a => a.TaxBillAuditId == 0).ToList().Count;
            }
            else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
            {
                HearingResultFinalizednotFinalizedCount = lstInvoiceDetails.Where(a => a.HearingFinalized == false || a.FinalAssessedValue == 0).ToList().Count;
            }
            else
            {
                HearingResultFinalizednotFinalizedCount = lstInvoiceDetails.Where(a => a.HearingFinalized == false).ToList().Count;
            }


            if (HearingResultFinalizednotFinalizedCount > 0 && objInvoice.IsSpecialTerm != true)
            {
                //SubmitinvoiceSummary
                //if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                //{
                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingArbitrationresultsforotheraccountsinthisTerm.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //}
                //else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
                //{
                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingLitigationresultsforotheraccountsinthisTerm.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //}
                //else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
                //{
                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingBPPforotheraccountsinthisTerm.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //}
                //else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                //{
                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingTaxBillforotheraccountsinthisTerm.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //}
                //else
                //{
                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.PendingHearingResultsForOtherAccountsInthisTerm.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //}
                //invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                IsInvoiceDataValid = false;
            }
            else
            {
                //Added by KishoreKumar to avoid Inserting accounts which are not Qualified for invoicing.
                if (objInvoice.IsSpecialTerm)
                    lstInvoiceDetails = lstInvoiceDetails.Where(q => q.HearingFinalized == true).ToList();

                foreach (PTXboInvoice objInvoiceDataCalculation in lstInvoiceDetails)
                {
                    if (ValidateInvoiceDetails(objInvoiceDataCalculation, out errorMessage))
                    {
                        if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
                        {
                            if (objInvoice.FlatFee > 0 && objInvoice.FlatFee != null)
                            {
                                objInsertInvoiceData = objInvoiceDataCalculation;
                                objInsertInvoiceData.InvoiceAmount = Convert.ToDecimal(objInvoiceDataCalculation.FlatFee);
                                IsInvoiceDataValid = true;
                            }
                            else
                            {
                                //invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoicenotrequiredFlatFeenotNoticed.GetId();
                                //invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                                //invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                                IsInvoiceDataValid = false;
                                break;
                            }
                        }
                        else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                        {
                            if (objInvoice.FlatFee > 0 && objInvoice.FlatFee != null)
                            {
                                objInsertInvoiceData = objInvoiceDataCalculation;
                                objInsertInvoiceData.InvoiceAmount = Convert.ToDecimal(objInvoiceDataCalculation.FlatFee);
                                IsInvoiceDataValid = true;
                            }
                            else
                            {
                                //invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoicenotrequiredFlatFeenotNoticed.GetId();
                                //invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                                //invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                                IsInvoiceDataValid = false;
                                break;
                            }
                        }
                        else
                        {
                            if (objInvoiceDataCalculation.HearingFinalized == true && objInvoiceDataCalculation.InvoiceGenerated == false)
                            {
                                TotalNoticedValue = TotalNoticedValue + Convert.ToDecimal(objInvoiceDataCalculation.InitialAssessedValue);
                                TotalPostHearingValue = TotalPostHearingValue + Convert.ToDecimal(objInvoiceDataCalculation.FinalAssessedValue);
                                TotalPriorYearTaxRate = TotalPriorYearTaxRate + Convert.ToDouble(objInvoiceDataCalculation.PriorYearTaxRate);
                                TotalReduction = (TotalNoticedValue - TotalPostHearingValue); //Modified By Pavithra.B on 31Aug2016 - calculating Reduction value from TotalNoticedValue and TotalPostHearingValue                                
                                Reduction = Convert.ToDecimal(objInvoiceDataCalculation.InitialAssessedValue) - Convert.ToDecimal(objInvoiceDataCalculation.FinalAssessedValue);
                                if (TotalReduction > 0 || objInvoice.IsSpecialTerm)
                                {
                                    IsReduction = true;
                                    //objInvoiceDataCalculation.EstimatedTaxSaving = (Convert.ToDecimal(TotalPriorYearTaxRate) / 100) * Convert.ToDecimal((TotalReduction));
                                    //TotalEstimatedTaxSavings = objInvoiceDataCalculation.EstimatedTaxSaving;
                                    objInvoiceDataCalculation.EstimatedTaxSaving = (Convert.ToDecimal(Convert.ToDouble(objInvoiceDataCalculation.PriorYearTaxRate)) / 100) * Convert.ToDecimal((Reduction));
                                    TotalEstimatedTaxSavings = TotalEstimatedTaxSavings + (Convert.ToDecimal(Convert.ToDouble(objInvoiceDataCalculation.PriorYearTaxRate)) / 100) * Convert.ToDecimal((Reduction));
                                    if (objInvoice.IsSpecialTerm)
                                    {
                                        objInvoiceDataCalculation.ContingencyFee = Math.Round((TotalEstimatedTaxSavings) * Convert.ToDecimal((objInvoice.ContingencyPercentage)), 2);
                                        objInvoiceDataCalculation.FlatFee = objInvoice.FlatFee.GetValueOrDefault();
                                        //Modified By Pavithra.B on 3Nov2016 - TFS Id : 26636
                                        objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
                                        TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
                                        //objInvoiceDataCalculation.InvoiceAmount = objInvoiceDataCalculation.ContingencyFee.GetValueOrDefault() + objInvoice.FlatFee.GetValueOrDefault();
                                    }
                                    else
                                    {
                                        objInvoiceDataCalculation.ContingencyFee = Math.Round((TotalEstimatedTaxSavings) * Convert.ToDecimal((objInvoiceDataCalculation.ContingencyPercentage)), 2);
                                        if ((objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() ||
                                            objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                                            && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
                                        {
                                            List<PTXboInvoiceLineItem> dtInvoiceLineItem = CheckPosttoFlatFeeInvoicing(objInvoice.GroupId, 3);
                                            if (dtInvoiceLineItem.Count == 0)
                                            {
                                                objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
                                                TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
                                            }
                                            else
                                            {
                                                objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
                                                TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
                                            }
                                        }
                                        else if ((objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() ||
                                            objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                                            && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
                                        {
                                            List<PTXboInvoiceLineItem> dtInvoiceLineItem = CheckPosttoFlatFeeInvoicing(objInvoice.ProjectId, 2);
                                            if (dtInvoiceLineItem.Count == 0)
                                            //if (dtInvoiceLineItem == null)
                                            {
                                                objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
                                                TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
                                            }
                                            else
                                            {
                                                objInvoiceDataCalculation.InvoiceAmount = ((objInvoiceDataCalculation.EstimatedTaxSaving) * Convert.ToDecimal((objInvoice.ContingencyPercentage)));
                                                TotalInvoiceAmount = TotalInvoiceAmount + objInvoiceDataCalculation.InvoiceAmount;
                                            }
                                        }

                                    }

                                    objInsertInvoiceData = objInvoiceDataCalculation;
                                    if (objInvoice.IsSpecialTerm)
                                    {
                                        objInsertInvoiceData.InvoiceID = objInvoice.InvoiceID;
                                        objInsertInvoiceData.IsSpecialTerm = objInvoice.IsSpecialTerm;
                                    }
                                    //objInsertInvoiceData.EstimatedTaxSaving = objInvoiceDataCalculation.EstimatedTaxSaving;
                                    objInsertInvoiceData.EstimatedTaxSaving = TotalEstimatedTaxSavings;
                                    objInsertInvoiceData.ContingencyFee = objInvoiceDataCalculation.ContingencyFee;
                                    //objInsertInvoiceData.InvoiceAmount = objInvoiceDataCalculation.InvoiceAmount;
                                    objInsertInvoiceData.InvoiceAmount = TotalInvoiceAmount;
                                    objInsertInvoiceData.InitialAssessedValue = TotalNoticedValue;
                                    objInsertInvoiceData.FinalAssessedValue = TotalPostHearingValue;
                                    objInsertInvoiceData.PriorYearTaxRate = TotalPriorYearTaxRate;
                                    objInsertInvoiceData.Reduction = TotalReduction;
                                        //Added by saravanans.tfs id:56388
                                        if (objInvoice.InvoiceTypeId!=1)
                                        {
                                            objInvoice.IsRegenerateInvoice = 0;
                                        }
                                        //Ends here..

                                    objInsertInvoiceData.IsRegenerateInvoice = objInvoice.IsRegenerateInvoice;//Added by saravanans.tfs id:55312
                                    IsInvoiceDataValid = true;
                                }
                                else
                                {
                                    if (IsReduction)
                                    {
                                        IsInvoiceDataValid = true;
                                    }
                                    //invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceNotRequiredReductionNotNoticed.GetId();
                                    //invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                                    //invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInvoice, out errorMessage);
                                    IsInvoiceDataValid = false;
                                    //break;
                                }
                            }
                        }
                    }
                    else
                    {
                        objInsertInvoiceData = objInvoiceDataCalculation;
                        IsInvoiceValidationFails = true;
                    }
                }
            }
            
                    if (invoicingGroupingType == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.ToString())
                        objInsertInvoiceData.InvoiceDescription = string.IsNullOrEmpty(objInvoice.InvoiceDescription)?objInvoice.ProjectName:objInvoice.InvoiceDescription;
                    else if (invoicingGroupingType == Enumerators.PTXenumInvoiceGroupingType.TermLevel.ToString())
                        objInsertInvoiceData.InvoiceDescription = string.IsNullOrEmpty(objInvoice.InvoiceDescription) ? objInvoice.GroupName: objInvoice.InvoiceDescription;
               
                    

            objInsertInvoiceData.InvoiceDate = DateTime.Now;



            if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId() || objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
            {
                objInsertInvoiceData.PaymentDueDate = DateTime.Now.AddDays(90);
            }
            else
            {
                objInsertInvoiceData.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            //Standard so due date will be invoice date + 30 days

            objInsertInvoiceData.CreatedDateAndTime = DateTime.Now;
            objInsertInvoiceData.AutoGenerated = true;
            List<Int32> lstHearingResultID = GetDistinctHearingResultIDs(lstInvoiceDetails, objInvoice.InvoiceTypeId);

            if (IsInvoiceDataValid)
            {
                if (objInsertInvoiceData.InvoiceCapValue != null && objInsertInvoiceData.InvoiceCapValue != 0)
                {
                    InvoiceCapValue = Convert.ToDecimal(objInsertInvoiceData.InvoiceCapValue);
                }
                if (objInvoice.IsSpecialTerm)
                {
                        if (!string.IsNullOrEmpty(objInvoice.InvoiceDescription))
                        {
                            objInsertInvoiceData.InvoiceDescription = objInvoice.InvoiceDescription;//Added by SaravananS. tfs id:56933
                        }

                     objInsertInvoiceData.ContingencyPercentage = objInvoice.ContingencyPercentage;
                    objInsertInvoiceData.FlatFee = objInvoice.FlatFee.GetValueOrDefault();
                    objInsertInvoiceData.DontGenerateInvoiceFlag = objInvoice.DontGenerateInvoiceFlag;
                    objInsertInvoiceData.CanGenerateInvoice = objInvoice.CanGenerateInvoice;
                    //Added By Pavithra.B on 3Nov2016 - FlatFee Calculation Issue. TFS Id : 26636

                    InvoiceAmountCalculation(Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680
                    objInsertInvoiceData.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                    //objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount + objInvoice.FlatFee.GetValueOrDefault();
                }
                else
                {
                    if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())//Added flatfee with invoiceamount by saravanans-tfs:48680
                    {
                        List<PTXboInvoiceLineItem> dtInvoiceLineItem = CheckPosttoFlatFeeInvoicing(objInvoice.GroupId, 3);
                        if (dtInvoiceLineItem.Count == 0)
                        {
                            InvoiceAmountCalculation(Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680
                            objInsertInvoiceData.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                            //objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount + objInvoice.FlatFee.GetValueOrDefault();
                        }
                        else
                        {
                            InvoiceAmountCalculation(Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount ), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680
                            objInsertInvoiceData.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                            //objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount;
                        }
                    }
                    else if (objInvoice.InvoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId() && objInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
                    {
                        List<PTXboInvoiceLineItem> dtInvoiceLineItem = CheckPosttoFlatFeeInvoicing(objInvoice.ProjectId, 2);
                        if (dtInvoiceLineItem.Count == 0)
                        {
                            InvoiceAmountCalculation(Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount ), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680
                            objInsertInvoiceData.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                            //objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount + objInvoice.FlatFee.GetValueOrDefault();
                        }
                        else
                        {
                            InvoiceAmountCalculation(Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount), objInvoice.FlatFee.GetValueOrDefault(), InvoiceCapValue, out InvoiceCalAmount);//Added flatfee with invoiceamount by saravanans-tfs:48680
                            objInsertInvoiceData.InvoiceAmount = InvoiceCalAmount;// + objInvoice.FlatFee.GetValueOrDefault();
                            //objInsertInvoiceData.InvoiceAmount = objInsertInvoiceData.InvoiceAmount;
                        }
                    }
                }
                objInsertInvoiceData.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
                objInsertInvoiceData.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.ReadyForInvoicing.GetId();
                isInvoiceRecordCreated = SubmitInvoice(objInsertInvoiceData, lstHearingResultID, out NewlyCreatedInvoiceID, out errorMessage);

                //if (isInvoiceRecordCreated == true && NewlyCreatedInvoiceID != 0)
                //{
                //    //bool isSuccessCapValue = false;
                //    string errormessage = string.Empty;
                //    //List<PTXboInvoicePaymentMap> lstInvoicePaymentMap;
                //    var objInvoiceid = GetInvoiceDetailsById(NewlyCreatedInvoiceID).FirstOrDefault();
                //    if (objInvoiceid != null)
                //    {
                //        //Pavithra.B - Included Cap Value
                //        //if (objInsertInvoiceData.CapValue > 0 && Convert.ToDecimal((objInvoice.ContingencyPercentage)) > 0 && Convert.ToDecimal(objInsertInvoiceData.InvoiceAmount) > Convert.ToDecimal(objInsertInvoiceData.CapValue))
                //        //{
                //        //    lstInvoicePaymentMap = GetInvoicePaymentMap(NewlyCreatedInvoiceID);
                //        //    if (lstInvoicePaymentMap != null && lstInvoicePaymentMap.Count > 0)
                //        //    {
                //        //        PTXboPayment Objpayment = new PTXboPayment();
                //        //        foreach (PTXboInvoicePaymentMap obiInvPay in lstInvoicePaymentMap)
                //        //        {
                //        //            var lstPayment = GetInvoicePayment(obiInvPay.PaymentId, "Cap Value Adjustment");
                //        //            if (lstPayment != null)
                //        //            {
                //        //                Objpayment = lstPayment;
                //        //            }
                //        //            decimal PaymentAmount = 0;
                //        //            PaymentAmount = Math.Round((objInvoiceid.InvoiceAmount.GetValueOrDefault()) - Convert.ToDecimal(objInsertInvoiceData.CapValue), 2);
                //        //            Objpayment.ClientId = objInsertInvoiceData.ClientId;
                //        //            Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                //        //            Objpayment.PaymentAmount = PaymentAmount;
                //        //            Objpayment.PaymentDescription = "Cap Value Adjustment";
                //        //            Objpayment.CreatedBy = objInvoiceFromHearingResult.CreatedBy;
                //        //            Objpayment.CreatedDateTime = DateTime.Now;
                //        //            Objpayment.UpdatedBy = objInvoiceFromHearingResult.CreatedBy;
                //        //            Objpayment.UpdatedDateTime = DateTime.Now;
                //        //            isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, NewlyCreatedInvoiceID, out errormessage);
                //        //            if (isSuccessCapValue)
                //        //            {
                //        //                bool isSuccess = UpdateAmountDueCapValueChange(NewlyCreatedInvoiceID, out errormessage);
                //        //            }
                //        //        }
                //        //    }
                //        //    else
                //        //    {
                //        //        decimal PaymentAmount = 0;
                //        //        PaymentAmount = Math.Round((objInvoiceid.InvoiceAmount.GetValueOrDefault()) - Convert.ToDecimal(objInsertInvoiceData.CapValue), 2);
                //        //        PTXboPayment Objpayment = new PTXboPayment();
                //        //        Objpayment.ClientId = objInsertInvoiceData.ClientId;
                //        //        Objpayment.InvoicePaymentMethodId = Enumerators.PTXenumInvoicePaymentMethod.Adjustment.GetId();
                //        //        Objpayment.PaymentAmount = PaymentAmount;
                //        //        Objpayment.PaymentDescription = "Cap Value Adjustment";
                //        //        Objpayment.CreatedBy = objInvoiceFromHearingResult.CreatedBy;
                //        //        Objpayment.CreatedDateTime = DateTime.Now;
                //        //        Objpayment.UpdatedBy = objInvoiceFromHearingResult.CreatedBy;
                //        //        Objpayment.UpdatedDateTime = DateTime.Now;
                //        //        isSuccessCapValue = SubmitCapValueAdjustment(Objpayment, NewlyCreatedInvoiceID, out errormessage);
                //        //        if (isSuccessCapValue)
                //        //        {
                //        //            bool isSuccess = UpdateAmountDueCapValueChange(NewlyCreatedInvoiceID, out errormessage);
                //        //        }
                //        //    }
                //        //}
                //        if (Convert.ToDecimal(objInvoiceid.AmountAdjusted) != 0)
                //        {
                //            RemoveCapvalueAdjustment(objInvoiceid.InvoiceID);
                //            bool isSuccess = UpdateAmountDueCapValueChange(objInvoiceid.InvoiceID, out errormessage);
                //        }
                //    }

                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.ReadyForInvoicing.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();

                //    invoiceSummary.InvoiceID = NewlyCreatedInvoiceID;
                //    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);

                //    //Need to insert into Invoice Remarks
                //    PTXboInvoiceRemarks objInvoiceRemarks = new PTXboInvoiceRemarks();
                //    objInvoiceRemarks.InvoiceID = NewlyCreatedInvoiceID;
                //    objInvoiceRemarks.InvoiceRemarks = "Invoice created for Hearing Type : " + hearingType + " and Grouping level : " + invoicingGroupingType;
                //    objInvoiceRemarks.UpdatedBy = objInvoiceFromHearingResult.CreatedBy;
                //    InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objInvoiceRemarks, out errorMessage);
                //}
                //else
                //{
                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.TachnicalErrorInInvoiceGeneration.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);
                //}
            }
            if (IsInvoiceValidationFails)
            {
                objInsertInvoiceData.InvoicingStatusId = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();
                objInsertInvoiceData.InvoicingProcessingStatusId = Enumerators.PTXenumInvoicingPRocessingStatus.WaitingInPendingResearchQueue.GetId();
                isInvoiceRecordCreated = SubmitInvoice(objInsertInvoiceData, lstHearingResultID, out NewlyCreatedInvoiceID, out errorMessage);
                //if (isInvoiceRecordCreated == true && NewlyCreatedInvoiceID != 0)
                //{
                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.WaitingInPendingResearchQueue.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordCreated.GetId();

                //    invoiceSummary.InvoiceID = NewlyCreatedInvoiceID;
                //    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);

                //    //Need to insert into Invoice Remarks
                //    PTXboInvoiceRemarks objInvoiceRemarks = new PTXboInvoiceRemarks();
                //    objInvoiceRemarks.InvoiceID = NewlyCreatedInvoiceID;
                //    objInvoiceRemarks.InvoiceRemarks = "Invoice created for Hearing Type : " + hearingType + " and Grouping level : " + invoicingGroupingType;
                //    objInvoiceRemarks.UpdatedBy = objInvoiceFromHearingResult.CreatedBy;
                //    InsertInvoiceRemarksAlongWithHearingTypeAndGroupingLevel(objInvoiceRemarks, out errorMessage);
                //}
                //else
                //{
                //    invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.TachnicalErrorInInvoiceGeneration.GetId();
                //    invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
                //    invoiceSummary.InvoicesummaryID = InsertInvoiceSummaryWithProcessingStatus(invoiceSummary, objInsertInvoiceData, out errorMessage);
                //}
            }
            Logger.For(this).Invoice("Invoice Calculation-InsertTermOrProjectLevelInvoiceGeneration-API  ends successfully ");
            return isInvoiceRecordCreated;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertTermOrProjectLevelInvoiceGeneration-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
             finally
            {
                Dispose();
            }
        }
        #endregion


        #region Flatfee Invoice 
        public string GetParamValue(int paramID)
        {
            Hashtable parameters = new Hashtable();
            string paramValue = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetParamValue-API  reached " + ((object)paramID).ToJson(false));
                parameters.Add("@Param_ID", paramID);
                paramValue = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_GET_ParamValue, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-GetParamValue-API  ends successfully ");
                return paramValue;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetParamValue-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        private PTXboInvoice InsertInvoice(PTXboInvoiceFlatFeePre invoiceFlatFeePre, PTXboProject project, DataTable dataTable, int currentUserId)
        {
            try
            { 
            PTXboInvoice Invoice = null;
            int invoiceID = 0;
            Logger.For(this).Invoice("Invoice Calculation-InsertInvoice-  reached " + ((object)invoiceFlatFeePre).ToJson(false));
            string currentTaxYear = string.Empty;
            currentTaxYear = GetParamValue(Convert.ToInt32(EnumConstants.PTXParameters.CurrentTaxYear));

            Invoice = new PTXboInvoice();
            Invoice.GroupId = invoiceFlatFeePre.GroupId;
            Invoice.ClientId = invoiceFlatFeePre.clientId;
            Invoice.ProjectId = project.ProjectId;
            Invoice.InvoicingStatusId = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceStatus.InvoiceGenerated.GetId();
            Invoice.InvoiceTypeId = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceType.Standard.GetId(); ;
            Invoice.ContingencyFee = Convert.ToDecimal(invoiceFlatFeePre.Contingency);
            Invoice.InvoiceAmount = Convert.ToDecimal(invoiceFlatFeePre.FlatFee);
            Invoice.CreatedDateAndTime = DateTime.Now;
            Invoice.TaxYear = Convert.ToInt32(currentTaxYear);
            Invoice.InvoiceGroupingTypeId = invoiceFlatFeePre.InvoiceGroupingTypeId; ;
            //Invoice.InvoiceDate = DateTime.Now;
            Invoice.FlatFee = Convert.ToDecimal(invoiceFlatFeePre.FlatFee);
            Invoice.AmountDue = Convert.ToDecimal(invoiceFlatFeePre.FlatFee);
            Invoice.PaymentDueDate = DateTime.Now.AddDays(30);
            Invoice.PaymentStatusId = Spartaxx.DataObjects.Enumerators.PTXenumPaymentStatus.NotPaid.GetId();
            Invoice.UpdatedBy = currentUserId;
            Invoice.UpdatedDateTime = DateTime.Now;

            SaveOrUpdateInvoice(Invoice, out invoiceID);
            Invoice.InvoiceID = invoiceID;
            InsertInvoiceListItem(Invoice, invoiceFlatFeePre, dataTable, project);
            Logger.For(this).Invoice("Invoice Calculation-InsertInvoice-  ends successfully ");
            return Invoice;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoice-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        private void InsertInvoiceListItem(PTXboInvoice Invoice, PTXboInvoiceFlatFeePre invoiceFlatFeePre, DataTable dataTable, PTXboProject Project)
        {
            try
            { 
            Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceListItem-reached " + ((object)Invoice).ToJson(false));
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (Project != null)
                {
                    if (Project.ProjectId == Convert.ToInt32(dataTable.Rows[i].ItemArray[2]))
                    {

                        PTXboInvoiceLineItem InvoiceLineItem = new PTXboInvoiceLineItem();
                        InvoiceLineItem.InvoiceID = Invoice.InvoiceID;
                        InvoiceLineItem.AccountID = Convert.ToInt32(dataTable.Rows[i].ItemArray[0]);
                        SaveOrUpdateInvoiceLineItem(InvoiceLineItem);


                    }
                }
                else
                {

                    PTXboInvoiceLineItem InvoiceLineItem = new PTXboInvoiceLineItem();
                    InvoiceLineItem.InvoiceID = Invoice.InvoiceID;
                    InvoiceLineItem.AccountID = Convert.ToInt32(dataTable.Rows[i].ItemArray[0]);
                    SaveOrUpdateInvoiceLineItem(InvoiceLineItem);


                }

            }
            Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceListItem-ends successfully ");
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceListItem-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }


        }

        public int SaveOrUpdateInvoiceLineItem(PTXboInvoiceLineItem invoiceLineItem)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceLineItem-API  reached " + ((object)invoiceLineItem).ToJson(false));
                parameters.Add("@InvoiceLineItemID	", invoiceLineItem.InvoiceLineItemID);
                parameters.Add("@InvoiceID	", invoiceLineItem.InvoiceID);
                parameters.Add("@AccountID	", invoiceLineItem.AccountID);
                parameters.Add("@InitialAssessedValue	", invoiceLineItem.InitialAssessedValue);
                parameters.Add("@FinalAssessedValue	", invoiceLineItem.FinalAssessedValue);
                parameters.Add("@PriorYearTaxRate	", invoiceLineItem.PriorYearTaxRate);
                parameters.Add("@EstimatedTaxSavings	", invoiceLineItem.EstimatedTaxSavings);
                parameters.Add("@Reduciton	", invoiceLineItem.Reduciton);
                parameters.Add("@UpdatedBy	", invoiceLineItem.UpdatedBy);
                parameters.Add("@UpdatedDateAndTime	", invoiceLineItem.UpdatedDateAndTime);
                parameters.Add("@IntitalLand	", invoiceLineItem.IntitalLand);
                parameters.Add("@IntialImproved	", invoiceLineItem.IntialImproved);
                parameters.Add("@InitialMarket	", invoiceLineItem.InitialMarket);
                parameters.Add("@FinalLand	", invoiceLineItem.FinalLand);
                parameters.Add("@FinalImproved	", invoiceLineItem.FinalImproved);
                parameters.Add("@FinalMarket	", invoiceLineItem.FinalMarket);


                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceLineItem, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceLineItem-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceLineItem-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        private void InsertInvoiceSummary(PTXboInvoice Invoice, PTXboInvoiceFlatFeePre invoiceFlatFeePre, DataTable dataTable)
        {
            try
            { 
            Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceSummary- reached " + ((object)Invoice).ToJson(false));
            PTXboInvoiceSummary InvoiceSummary = new PTXboInvoiceSummary();
            InvoiceSummary.InvoiceGenerated = true;
            InvoiceSummary.InvoiceGeneratedForID = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGeneratedFor.FlatFeePre.GetId();
            InvoiceSummary.InvoiceSummaryProcessingStatusID = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceSummaryProcessingStatus.FlatFeePre.GetId();
            InvoiceSummary.InvoiceStatusID = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
            InvoiceSummary.ClientId = invoiceFlatFeePre.clientId;
            InvoiceSummary.GroupId = invoiceFlatFeePre.GroupId;
            InvoiceSummary.CreateDateTime = DateTime.Now;
            InvoiceSummary.InvoiceID = Invoice.InvoiceID;
            InvoiceSummary.InvoicesummaryID=SaveOrUpdateInvoiceSummary(InvoiceSummary);
            //InsertInvoiceListMap(InvoiceSummary, Invoice);
            Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceSummary-ends successfully ");
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceSummary-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }
        private void InsertInvoiceListMap(PTXboInvoiceSummary invoiceSummary, PTXboInvoice invoice)
        {
            try
            { 
            Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceListMap-reached " + ((object)invoiceSummary).ToJson(false));
            PTXboInvoiceListMap InvoiceListMap = new PTXboInvoiceListMap();
            InvoiceListMap.InvoiceSummaryId = invoiceSummary.InvoicesummaryID;
            InvoiceListMap.InvoiceId = invoice.InvoiceID;
            SaveOrUpdateInvoiceListItem(InvoiceListMap);
             }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceListMap-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        public int SaveOrUpdateInvoiceListItem(PTXboInvoiceListMap invoiceListMap)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceListItem-API  reached " + ((object)invoiceListMap).ToJson(false));
                parameters.Add("@InvoiceListMapId", invoiceListMap.InvoiceListMapId);
                parameters.Add("@InvoiceId", invoiceListMap.InvoiceId);
                parameters.Add("@InvoiceSummaryId", invoiceListMap.InvoiceSummaryId);

                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceListMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceListItem-API  ends successfully ");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SaveOrUpdateInvoiceListItem-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public int GetDefaultDeliveryType(int clientID)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetDefaultDeliveryType-API  reached " + ((object)clientID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@ClientID", clientID);
                var result = Convert.ToInt32(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_get_DefaultDeliveryType, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("Invoice Calculation-GetDefaultDeliveryType-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetDefaultDeliveryType-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public int GetServicePackageID(string letterName)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetServicePackageID-API  reached " + ((object)letterName).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@letterName", letterName);
                int servicePackageID = Convert.ToInt32(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_GetServicePackageID, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("Invoice Calculation-GetServicePackageID-API  ends successfully ");
                return servicePackageID;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetServicePackageID-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        private void InsertCorrQueueAndAccounts(PTXboInvoiceFlatFeePre invoiceFlatFeePre, PTXboInvoice invoice, int currentUserId, DataTable dataTable, PTXboProject project)
        {
            try
            { 
            string errorString = string.Empty;
            int servicePackageID = 0;

            string letterName = "Invoice";
            Logger.For(this).Invoice("Invoice Calculation-InsertCorrQueueAndAccounts- reached " + ((object)invoiceFlatFeePre).ToJson(false));
            string currentTaxYear = string.Empty;
            currentTaxYear = GetParamValue(Convert.ToInt32(EnumConstants.PTXParameters.CurrentTaxYear));

            servicePackageID = GetServicePackageID(letterName);

            var delivaryMethod = GetDefaultDeliveryType(invoiceFlatFeePre.clientId);

            PTXboCorrQueue objCorrQueue = new PTXboCorrQueue();
            objCorrQueue.ClientID = invoiceFlatFeePre.clientId;
            objCorrQueue.TaxYear = Convert.ToInt32(currentTaxYear);
            if (servicePackageID != 0)
            {
                objCorrQueue.ServicePackageID = servicePackageID;
            }
            objCorrQueue.CorrProcessingStatusID = Spartaxx.DataObjects.Enumerators.PTXenumCorrProcessingStatus.Active.GetId();
            objCorrQueue.SentToClient = true;

            if (delivaryMethod != 0)
            {
                objCorrQueue.DeliveryMethodId = delivaryMethod;
            }

            objCorrQueue.LinkFieldValue = invoice.InvoiceID;
            objCorrQueue.CreatedBy = currentUserId;
            objCorrQueue.CreatedDateTime = DateTime.Now;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (project != null)
                {
                    if (project.ProjectId == Convert.ToInt32(dataTable.Rows[i].ItemArray[2]))
                    {
                        objCorrQueue.AccountList = string.Join(",", Convert.ToInt32(dataTable.Rows[i].ItemArray[0]));
                    }
                }
                else
                {
                    objCorrQueue.AccountList = string.Join(",", Convert.ToInt32(dataTable.Rows[i].ItemArray[0]));
                }
            }
            SaveOrUpdateCorrQ(objCorrQueue);
            Logger.For(this).Invoice("Invoice Calculation-InsertCorrQueueAndAccounts-ends successfully ");
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetServicePackageID-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
             }
        }

        public bool FlatFeeInvoiceGeneration(PTXboFlatFeeInvoiceGenerationInput flatFeeInvoice, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-FlatFeeInvoiceGeneration-API  reached " + ((object)flatFeeInvoice).ToJson(false));
                Hashtable parameters = new Hashtable();

                parameters.Add("@ClientId", flatFeeInvoice.InvoiceFlatFeePre.clientId);
                parameters.Add("@GroupId", flatFeeInvoice.InvoiceFlatFeePre.GroupId);
                parameters.Add("@InvoiceGenerateType", flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId);

                var dtResult = _dapperConnection.SelectDataSet(StoredProcedureNames.usp_get_FlatFeeInvoiceGenerateAccountDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                if (dtResult.Tables.Count == 0)
                {
                    if (flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId() || flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGroupingType.AccountLevel.GetId())
                    {
                        errorMessage = "Invoice already Generated";
                    }

                    return false;
                }
                else
                {
                    Hashtable parameter = new Hashtable();
                    parameter.Add("@ClientId", flatFeeInvoice.InvoiceFlatFeePre.clientId);
                    parameter.Add("@GroupId", flatFeeInvoice.InvoiceFlatFeePre.GroupId);
                    parameter.Add("@InvoiceGenerateType", flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId);
                    var output = _dapperConnection.SelectDataSet(StoredProcedureNames.usp_get_FlatFeeInvoiceGenerateProjectDetails, parameter, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    DataTable ProjectDTResult = (output != null) ? output.Tables[0] : new DataTable();
                    if (flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
                    {
                        foreach (DataRow row in ProjectDTResult.Rows)
                        {
                            PTXboProject dProject = new PTXboProject();
                            dProject.ProjectId = Convert.ToInt32(row.ItemArray[0]);

                            PTXboInvoice Invoice = InsertInvoice(flatFeeInvoice.InvoiceFlatFeePre, dProject, ProjectDTResult, flatFeeInvoice.CurrentUserId);
                            if (Invoice != null)
                            {
                                InsertInvoiceSummary(Invoice, flatFeeInvoice.InvoiceFlatFeePre, ProjectDTResult);
                                InsertCorrQueueAndAccounts(flatFeeInvoice.InvoiceFlatFeePre, Invoice, flatFeeInvoice.CurrentUserId, ProjectDTResult, dProject);
                            }
                        }
                    }
                    else if (flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
                    {
                        foreach (DataRow row in ProjectDTResult.Rows)
                        {
                            //PTXdoProject dProject = new PTXdoProject();
                            //dProject.ProjectId = Convert.ToInt32(row.ItemArray[0]);

                            PTXboInvoice Invoice = InsertInvoice(flatFeeInvoice.InvoiceFlatFeePre, null, ProjectDTResult, flatFeeInvoice.CurrentUserId);
                            if (Invoice != null)
                            {
                                InsertInvoiceSummary(Invoice, flatFeeInvoice.InvoiceFlatFeePre, ProjectDTResult);
                                InsertCorrQueueAndAccounts(flatFeeInvoice.InvoiceFlatFeePre, Invoice, flatFeeInvoice.CurrentUserId, ProjectDTResult, null);
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow row in dtResult.Tables[0].Rows)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("AccountId");
                            dataTable.Columns.Add("GroupId");
                            dataTable.ImportRow(row);
                            PTXboInvoice Invoice = InsertInvoice(flatFeeInvoice.InvoiceFlatFeePre, null, dataTable, flatFeeInvoice.CurrentUserId);
                            if (Invoice != null)
                            {
                                InsertInvoiceSummary(Invoice, flatFeeInvoice.InvoiceFlatFeePre, dataTable);
                                InsertCorrQueueAndAccounts(flatFeeInvoice.InvoiceFlatFeePre, Invoice, flatFeeInvoice.CurrentUserId, dataTable, null);
                            }
                        }
                    }
                    Logger.For(this).Invoice("Invoice Calculation-FlatFeeInvoiceGeneration-API  ends successfully ");
                    return true;
                }

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation--API  error " + ex);

                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        #endregion



        #region Pending Group Invoice

        public PTXboHearingResult GetHearingResult(int hearingResultID)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetHearingResult-API  reached " + ((object)hearingResultID).ToJson(false));
                parameters.Add("@HearingResultID", hearingResultID);

                var result = _dapperConnection.Select<PTXboHearingResult>(StoredProcedureNames.usp_get_HearingResult, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetHearingResult-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetHearingResult-API  error " + ex);
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool InsertInvoiceDataAccountLevel(PTXboInvoice objInvoiceFromHearingResult)
        {
            
            int HearingResultID = objInvoiceFromHearingResult.HearingResultId;
            int TaxBillAuditId = objInvoiceFromHearingResult.TaxBillAuditId;
            int BppRenditionId = objInvoiceFromHearingResult.BppRenditionId;
            List<PTXboInvoice> lstInvoiceDetails = new List<PTXboInvoice>();
            List<PTXboInvoice> objExceptValueHearingType = new List<PTXboInvoice>();
            bool isInvoiceRecordCreated = false;
            short valueHearingType = Enumerators.PTXenumHearingType.ValueHearing.GetId();
            string hearingType = string.Empty;
            string invoicingGroupType = string.Empty;
         
            Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceDataAccountLevel API-starts " + ((object)objInvoiceFromHearingResult).ToJson(false));
            //return PTXdsInvoice.CurrentInstance.InsertInvoiceDataAccountLevel(objInvoiceFromHearingResult, out errorMessage);
            try
            {
                // Edited K.Selva 
                if (objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.BPP.GetId() && objInvoiceFromHearingResult.InvoiceTypeId != Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                {
                    PTXboHearingResult objHearingType = new PTXboHearingResult();
                    /** Get the Hearing Type **/
                    objHearingType= GetHearingResult(HearingResultID);
                    hearingType = objHearingType.HearingTypeId.ToString();
                }

                /*Getting Account, group, term details based on the client, tax year for the standard term*/
                lstInvoiceDetails=GetInvoiceGenerationInputData(objInvoiceFromHearingResult.ClientId, objInvoiceFromHearingResult.TaxYear, objInvoiceFromHearingResult.InvoiceTypeId);
                List<PTXboInvoice> objInvoiceDetails = new List<PTXboInvoice>();
                // K.Selva - For Getting BPPRenditionID  from Invoice Details //starts

                //Added By Boopathi.S --Checks Null Condition
                if (lstInvoiceDetails != null)
                {
                    if (objInvoiceFromHearingResult.InvoiceTypeId == Enumerators.PTXenumInvoiceType.BPP.GetId())
                    {
                        objInvoiceDetails = lstInvoiceDetails.Where(a => a.BppRenditionId == BppRenditionId).ToList();
                    }
                    //For Getting TaxBillAuditID  from Invoice Details
                    else if (objInvoiceFromHearingResult.InvoiceTypeId == Enumerators.PTXenumInvoiceType.TaxBillAudit.GetId())
                    {
                        objInvoiceDetails = lstInvoiceDetails.Where(a => a.TaxBillAuditId == TaxBillAuditId).ToList();
                    }
                    else
                    {
                        /*Select the hearing finalized record*/
                        objInvoiceDetails = lstInvoiceDetails.Where(a => a.HearingResultId == HearingResultID).ToList();
                    }
                }
                // Ends
                if (objInvoiceDetails.Count > 0)
                {
                    foreach (PTXboInvoice objInvoice in objInvoiceDetails)
                    {
                        objInvoice.CanGenerateInvoice = objInvoiceFromHearingResult.CanGenerateInvoice;
                        objInvoice.IsSpecialTerm = objInvoiceFromHearingResult.IsSpecialTerm;
                        objInvoice.DontGenerateInvoiceFlag = objInvoiceFromHearingResult.DontGenerateInvoiceFlag;
                        if (objInvoice.IsSpecialTerm)
                        {
                            objInvoice.InvoiceAmount = objInvoiceFromHearingResult.InvoiceAmount;
                            objInvoice.ContingencyPercentage = objInvoiceFromHearingResult.ContingencyPercentage;
                            objInvoice.FlatFee = objInvoiceFromHearingResult.FlatFee;
                        }
                        if (objInvoiceFromHearingResult.IsSpecialTerm && objInvoiceFromHearingResult.InvoiceID > 0)
                            objInvoice.InvoiceID = objInvoiceFromHearingResult.InvoiceID;


                        objInvoice.InvoiceGroupingTypeId = Enumerators.PTXenumInvoiceGroupingType.AccountLevel.GetId();
                        invoicingGroupType = Enumerators.PTXenumInvoiceGroupingType.AccountLevel.ToString();
                        PTXboInvoice createdInvoice = new PTXboInvoice();
                        PTXboAccountLevelInvoiceOutput invoiceOutput = new PTXboAccountLevelInvoiceOutput();
                        invoiceOutput = InsertAccountLevelInvoiceGeneration(objInvoice, lstInvoiceDetails, hearingType, invoicingGroupType, objInvoiceFromHearingResult.CreatedBy);
                        isInvoiceRecordCreated = invoiceOutput.IsInvoiceRecordCreated;

                    }
                }
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceDataAccountLevel API-ends ");
                return isInvoiceRecordCreated;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InsertInvoiceDataAccountLevel API-error " + ex);
                //errorstring = ex.Message;
                return false;
            }
            finally
            {
                Dispose();
            }
        }
        #endregion

        #region Invoice Defects
        /// <summary>
        /// Get service pacakage Id 
        /// </summary>
        public int GetServicePackageIDforInvoice(string servicePackageName)
        {
            Hashtable parameters = new Hashtable();
            int servicePackageID = 0;
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetServicePackageIDforInvoice-API  reached " + ((object)servicePackageName).ToJson(false));
                servicePackageID = Convert.ToInt32(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_getInvoiceServicePackageID, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
                Logger.For(this).Invoice("Invoice Calculation-GetServicePackageIDforInvoice-API  ends successfully ");
                return servicePackageID;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetServicePackageIDforInvoice-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool UpdateInvoiceStatusinMainTable(int invoiceID, int invoiceTypeId, bool isSpecialTerm = false)
        {
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-UpdateInvoiceStatusinMainTable-API  reached.invoiceID " + ((object)invoiceID).ToJson(false));
                bool isSuccsess = false;
                List<int> lstHearingResultIDs = new List<int>();
                if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Arbritration.GetId())
                {
                    lstHearingResultIDs = GetInvoiceAndHearingResultMap(invoiceID).Select(h => h.ArbitrationDetailsId).Distinct().ToList();
                    if (lstHearingResultIDs != null && lstHearingResultIDs.Count > 0)
                    {
                        foreach (int lstDet in lstHearingResultIDs)
                        {
                            PTXboArbitration arbDetails = new PTXboArbitration();
                            arbDetails = GetArbitrationDetails(lstDet);
                            //if (isSpecialTerm == false)
                            //{
                            //    arbDetails.ARBInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceSendingInProgress.GetId();
                            //}
                            //else
                            //{
                                arbDetails.ARBInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId();
                            //}
                            SaveOrUpdateArbitrationDetails(arbDetails);
                        }
                    }
                }
                else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Litigation.GetId())
                {
                    lstHearingResultIDs = GetInvoiceAndHearingResultMap(invoiceID).Select(h => h.LitigationID).Distinct().ToList();
                    if (lstHearingResultIDs != null && lstHearingResultIDs.Count > 0)
                    {
                        foreach (int lstDet in lstHearingResultIDs)
                        {
                            PTXboLitigationDetails litDetails = new PTXboLitigationDetails();
                            litDetails = GetLitigation(lstDet);
                            //if (isSpecialTerm == false)
                            //{
                            //    litDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceSendingInProgress.GetId();
                            //}
                            //else
                            //{
                                litDetails.LITInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId();
                            //}

                            litDetails.LitigationID=SaveOrUpdateLitigation(litDetails);
                        }
                    }
                }
                else if (invoiceTypeId == Enumerators.PTXenumInvoiceType.Standard.GetId())
                {
                    lstHearingResultIDs = GetInvoiceAndHearingResultMap(invoiceID).Select(h => h.HearingResultId).Distinct().ToList();
                    if (lstHearingResultIDs != null && lstHearingResultIDs.Count > 0)
                    {
                        foreach (int lstDet in lstHearingResultIDs)
                        {
                            PTXboHearingResult HearingDetails = new PTXboHearingResult();
                            HearingDetails = GetHearingResultByType(lstDet, 0);
                            //if (isSpecialTerm == false)
                            //{
                            //    HearingDetails.HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceSendingInProgress.GetId();
                            //}
                            //else
                            //{
                                HearingDetails.HRInvoiceStatusid = PTXdoenumHRInvoiceStatus.InvoiceGenerated.GetId();
                            //}
                            SaveOrUpdateHearingResult(HearingDetails);
                        }
                    }
                }
                isSuccsess = true;
                return isSuccsess;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetServicePackageIDforInvoice-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        public bool SubmitInvoiceGenerationDefects(List<PTXboInvoiceDetails> lstInvoiceDetails)
        {
            
            try
            {
                int defaultDeliveryMethodId = 0;
                int AltDeliveryMethodId = 0;
                bool isSuccessInvoiceGenerated = false;
                //List<PTXboCorrQueue> objCorrQueueList = new List<PTXboCorrQueue>();
                //PTXdoUser user = new PTXdoUser();
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceGenerationDefects- "+((object)lstInvoiceDetails).ToJson(false));
                int servicePackageID = 0;
                /* get the Service PackageID */
                servicePackageID= GetServicePackageIDforInvoice("Invoice");
                /* loop the all invoice id  */
                foreach (PTXboInvoiceDetails objInvoiceDetails in lstInvoiceDetails)
                {
                    PTXboInvoice ObjInvoice = new PTXboInvoice();
                    PTXboInvoice Invociedetails = new PTXboInvoice();
                    List<PTXboInvoice> lstInvoiceDetailsfromDB = new List<PTXboInvoice>();
                    List<PTXboCorrQueue> objCorrQueueList = new List<PTXboCorrQueue>();
                    //  PTXboInvoiceContact clientGroupContact;
                    //Added By Pavithra.B on 26Jul2017 -Bug 33166:Spartaxx Issue
                    //int Corrqid = 0;
                    int groupId = 0;
                    Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceGenerationDefects-objInvoiceDetails " + ((object)objInvoiceDetails).ToJson(false));
                    /* get the invoice details based on the invoiceId*/
                    ObjInvoice = GetSpecificInvoiceGenerationDetails(objInvoiceDetails.InvoiceId);
                    if (ObjInvoice != null && ObjInvoice.ClientId != 0)
                    {
                        /* get the Client Default Delivery method based on invoice table clientid  */

                        PTXboClientDeliveryMethodId clientDeliveryMethodId = new PTXboClientDeliveryMethodId();
                        clientDeliveryMethodId= GetClientDefaultDeliveryMethod(ObjInvoice.ClientId);
                        defaultDeliveryMethodId = clientDeliveryMethodId.DefaultDeliveryTypeId;
                        AltDeliveryMethodId = clientDeliveryMethodId.AlternateDeliveryTypeId;

                        //added by saravanans..tfs id:52962--enabling default delivery method for both Email and Usmail
                        string accountList = string.Empty;
                        Dictionary<int, bool> deliverymethod = new Dictionary<int, bool>();
                        deliverymethod.Add(1, clientDeliveryMethodId.DefaultDeliveryTypeEmail);
                        deliverymethod.Add(3, clientDeliveryMethodId.DefaultDeliveryTypeUsMail);
                        //ends here....

                        if (servicePackageID != 0)
                        {

                            /* get the Client account details for the client and taxyear statndard term
                             -- Modified by Pavithra.B -> Included Invoice Type Id condition for all the the Terms Type 
                             */
                            Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceGenerationDefects-GetInvoiceGenerationInputData: " + ((object)ObjInvoice).ToJson(false));
                            lstInvoiceDetailsfromDB = GetInvoiceGenerationInputData(ObjInvoice.ClientId, objInvoiceDetails.Taxyear, objInvoiceDetails.InvoiceTypeId);
                            /* Invoice group type is Term select the account against the groupid*/
                            if (ObjInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
                            {

                                var accounts = lstInvoiceDetailsfromDB.Where(a => a.GroupId == ObjInvoice.GroupId).Select(d => d.AccountId.ToString()).Distinct();
                                accountList = string.Join(",", accounts);

                            }
                            /* Invoice group type is Account select the account against the Accountid*/
                            else if (ObjInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.AccountLevel.GetId())
                            {
                                var accounts = lstInvoiceDetailsfromDB.Where(a => a.AccountId == ObjInvoice.AccountId).Select(d => d.AccountId.ToString()).Distinct();
                                accountList = string.Join(",", accounts);

                            }
                            /* Invoice group type is Project select the account against the Project*/
                            else if (ObjInvoice.InvoiceGroupingTypeId == Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
                            {
                                var accounts = lstInvoiceDetailsfromDB.Where(a => a.ProjectId == ObjInvoice.ProjectId).Select(d => d.AccountId.ToString()).Distinct();
                                accountList = string.Join(",", accounts);

                            }
                            //..added by saravanans...tfs id:52962
                            foreach (var defaultDelivery in deliverymethod)
                            {
                                if (defaultDelivery.Value)
                                {
                                    //..ends here....
                             PTXboCorrQueue objCorrQueue = new PTXboCorrQueue();
                            objCorrQueue.ServicePackageID = servicePackageID;
                            objCorrQueue.ClientID = (int)ObjInvoice.ClientId;
                            objCorrQueue.SentToClient = true;
                            objCorrQueue.TaxYear = objInvoiceDetails.Taxyear;
                            objCorrQueue.CorrProcessingStatusID = Spartaxx.DataObjects.Enumerators.PTXenumCorrProcessingStatus.Active.GetId();
                            objCorrQueue.DeliveryMethodId = defaultDelivery.Key;
                            objCorrQueue.IsSpecificAccount = true;
                            objCorrQueue.CreatedBy = objInvoiceDetails.updatedBy;
                            objCorrQueue.CreatedDateTime = DateTime.Now;
                            objCorrQueue.LinkFieldValue = objInvoiceDetails.InvoiceId;
                            objCorrQueue.AccountList = accountList;
                             objCorrQueueList.Add(objCorrQueue);
                            Logger.For(this).Invoice(" SubmitInvoiceGenerationDefects inside forloop  objCorrQueue:" + objCorrQueue);
                            Logger.For(this).Invoice(" SubmitInvoiceGenerationDefects inside forloop  objCorrQueue.AccountList" + objCorrQueue.AccountList);
                            objCorrQueueList.Add(objCorrQueue);

                                }
                            }

                        }
                        /*Call the Data service method to insert the corr queue table .*/
                        //Added By Pavithra.B on 26Jul2017 -Bug 33166:Spartaxx Issue
                        if (ObjInvoice.GroupId != 0)
                        {
                            groupId = ObjInvoice.GroupId;
                        }
                        else
                        {
                            groupId = 0;
                        }


                        isSuccessInvoiceGenerated = UpdateClientCorrQueueRecordInvoice(objCorrQueueList, groupId);
                        //commented
                        //if (isSuccessInvoiceGenerated)
                        //{
                        //    UpdateInvoiceStatusinMainTable(objInvoiceDetails.InvoiceId, objInvoiceDetails.InvoiceTypeId, false);
                        //}

                        /* Added by Pavithra.B on 04Aug2016 - in order to calculate the invoice amount before genereating the invoice. */
                        //   List<PTXboInvoiceDetails> getlstInvoiceDetailsisSpecialTerm;
                        string errorMessage = string.Empty;                        

                        UpdateInvoiceProcessingStatus(Enumerators.PTXenumInvoicingPRocessingStatus.InvoiceQueued.GetId(), objInvoiceDetails);

                        PTXboInvoiceSummary invoiceSummary = new PTXboInvoiceSummary();
                        invoiceSummary.YearlyHearingDetailsID = objInvoiceDetails.YealyHearingDetailsId;
                        invoiceSummary.GroupId = objInvoiceDetails.GroupId;
                        invoiceSummary.ProjectID = objInvoiceDetails.ProjectId;
                        invoiceSummary.ClientId = objInvoiceDetails.ClientId;
                        invoiceSummary.InvoiceGenerated = false;
                        invoiceSummary.InvoiceGeneratedForID = Enumerators.PTXenumInvoiceGeneratedFor.HearingProcessFees.GetId();
                        invoiceSummary.InvoiceStatusID = Enumerators.PTXenumInvoiceStatus.InvoiceQueueInserted.GetId();
                        invoiceSummary.InvoiceSummaryProcessingStatusID = Enumerators.PTXenumInvoiceSummaryProcessingStatus.InvoiceQueued.GetId();
                        invoiceSummary.InvoicesummaryID= SubmitInvoiceSummary(invoiceSummary);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-SubmitInvoiceGenerationDefects-API  error " + ((object)ex).ToJson(false));
                throw ex;
                //return false;
            }
            finally
            {
                Dispose();
            }
        }
        #endregion

        #region Mainscreen
        public PTXboPayment GetPayment(int paymentId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetPayment-API  reached " + ((object)"paymentId=" + paymentId.ToString() ).ToJson(false));
                parameters.Add("@PaymentId", paymentId);
                var result = _dapperConnection.Select<PTXboPayment>(StoredProcedureNames.usp_get_Payment, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetPayment-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetPayment-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        /// <summary>
        /// Created by SaravananS. tfs id:
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public List<PTXboCompanydetails> GetOconnorCompanyDetails(int clientId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetOconnorCompanyDetails-API  reached " + ((object)"clientId=" + clientId.ToString()).ToJson(false));
                parameters.Add("@clientId", clientId);
                var result = _dapperConnection.Select<PTXboCompanydetails>(PTXdoStoredProcedureNames.usp_getOcnnorCompanyDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Invoice("Invoice Calculation-GetOconnorCompanyDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetOconnorCompanyDetails-API  error " + ((object)ex).ToJson(false));
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        public PTXboClientPaymentDetails GetClientById(int clientId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GetClientById-API  reached " + ((object)"GetClientById=" + clientId.ToString()).ToJson(false));
                parameters.Add("@ClientId", clientId);
                var result = _dapperConnection.Select<PTXboClientPaymentDetails>(StoredProcedureNames.usp_get_Client, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("Invoice Calculation-GetClientById-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GetClientById-API  error " + ((object)ex).ToJson(false)); ;
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        #endregion


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


        #region Report Generation 
        public PTXboNomenclature GetNomeclature(int servicePackageID, int clientId, int groupId, int taxyear)
        {
            Hashtable parameters = new Hashtable();
            PTXboInvoiceContact invoiceContact = new PTXboInvoiceContact();
            try
            {
                parameters.Add("@packageId", servicePackageID);
                parameters.Add("@clientId", clientId);
                parameters.Add("@groupId", groupId);
                parameters.Add("@taxyear", taxyear);

                var serviceDocument = _dapperConnection.Select<PTXboNomenclature>(StoredProcedureNames.usp_getdocumentNomenClature, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return serviceDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        public DataTable FlatFeeCheck(int groupId, int clientId, int invoiceId)
        {
            Hashtable parameters = new Hashtable();
            
            try
            {
                parameters.Add("@ClientId", clientId);
                parameters.Add("@GroupId", groupId);
                parameters.Add("@invoiceId", invoiceId);

                var result = _dapperConnection.Select<DataTable>(StoredProcedureNames.usp_InvoiceFlatFeeValidationForReportGeneration, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        public bool IsFinalized(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                parameters.Add("@invoiceId", invoiceId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.USP_Checkhearingfinalized, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        public bool IsOutOfTexas(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                parameters.Add("@invoiceId", invoiceId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_checkoutoftexas, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        /// <summary>
        /// Created by SaravananS. tfs id:63335
        /// </summary>
        /// <param name="hearingTypeId"></param>
        /// <returns></returns>
        public string GetHearingType(int hearingTypeId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                parameters.Add("@HearingTypeId", hearingTypeId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_GetHearingType, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }
        public bool IsILAccount(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                parameters.Add("@invoiceId", invoiceId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_IsILAccount, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        public bool IsILHearing(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                parameters.Add("@invoiceId", invoiceId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_IsILHearing, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }


        /// <summary>
        /// Added by SaravananS.tfs id:56033
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public bool IsDisasterInvoice(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                parameters.Add("@invoiceId", invoiceId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_IsDisasterInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        /// <summary>
        /// Added by SaravananS.tfs id:55925
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public bool IsMultiyear(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                parameters.Add("@invoiceId", invoiceId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_CheckinvoiceisMultiyear, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }


        /// <summary>
        /// Added by SaravananS.tfs id:55925
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public bool IsHotelInvoice(int invoiceId)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                parameters.Add("@invoiceId", invoiceId);
                var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_checkinvoiceisHotelLuc, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }


        /// <summary>
        /// Created by SaravananS. tfs id:60377
        /// </summary>
        /// <param name="objInvoiceReportInput"></param>
        /// <returns></returns>
        public PTXboInvoiceReportOutput GenerateLegalInvoicereports(PTXboInvoiceReportInput objInvoiceReportInput)
        {
            string legalInvoiceOutputPath = string.Empty;
            string errorMessage = string.Empty;
            PTXboInvoiceReportOutput objInvoiceReportOutput = new PTXboInvoiceReportOutput();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GenerateLegalInvoicereports-is called " + ((object)objInvoiceReportInput).ToJson(false));
                InvoiceReport objInvoiceReportData = new InvoiceReport();

                if (objInvoiceReportData.generateLegalInvoicereports(objInvoiceReportInput.LinkFieldValue,  objInvoiceReportInput.ReportExportPath, out legalInvoiceOutputPath,out errorMessage, objInvoiceReportInput.InvoiceTypeId))
                {
                    Logger.For(this).Invoice("Invoice Calculation-GenerateLegalInvoicereports- report generation success Invoiceid:" + Convert.ToString(objInvoiceReportInput.LinkFieldValue) + " legalInvoiceOutputPath ::" + legalInvoiceOutputPath + " errorMessage ::" + errorMessage);
                    objInvoiceReportOutput.ReportOutputPath = legalInvoiceOutputPath;
                    objInvoiceReportOutput.ErrorMessage = errorMessage;

                    if (!string.IsNullOrEmpty(objInvoiceReportOutput.ReportOutputPath))
                    {
                        objInvoiceReportOutput.IsSuccess = true;
                    }
                }
                else
                {
                    Logger.For(this).Invoice("Invoice Calculation-GenerateLegalInvoicereports-Invoice details.report generation failed. INVOICEID::" + Convert.ToString(objInvoiceReportInput.LinkFieldValue) + " errorMessage::" + errorMessage);
                    objInvoiceReportOutput.IsSuccess = false;
                    objInvoiceReportOutput.ReportOutputPath = string.Empty;
                    objInvoiceReportOutput.ErrorMessage = errorMessage;
                }

                Logger.For(this).Invoice("Invoice Calculation-GenerateLegalInvoicereports-ends successfully ");
                return objInvoiceReportOutput;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GenerateLegalInvoicereports-error " + ex);
                objInvoiceReportOutput.ErrorMessage = ex.Message;
                objInvoiceReportOutput.IsSuccess = false;
                objInvoiceReportOutput.ReportOutputPath = string.Empty;
                return objInvoiceReportOutput;
            }
            finally
            {
                Dispose();
            }


        }



        /// <summary>
        /// Created by SaravananS. tfs id:60248
        /// </summary>
        /// <param name="objInvoiceReportInput"></param>
        /// <returns></returns>
        public PTXboInvoiceReportOutput GenerateInvoiceUsmailCoverLetter(PTXboInvoiceReportInput objInvoiceReportInput)
        {
            string invoiceUsmailcoverLetterPath = string.Empty;
            string errorMessage = string.Empty;
            PTXboInvoiceReportOutput objInvoiceReportOutput = new PTXboInvoiceReportOutput();
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoiceUsmailCoverLetter-is called " + ((object)objInvoiceReportInput).ToJson(false));
                InvoiceReport objInvoiceReportData = new InvoiceReport();

                if (objInvoiceReportData.GenerateInvoiceUsmailCoverLetter(Convert.ToString(objInvoiceReportInput.LinkFieldValue), objInvoiceReportInput.Groupid, objInvoiceReportInput.ClientId, objInvoiceReportInput.Taxyear, objInvoiceReportInput.ReportExportPath, objInvoiceReportInput.InvoiceTypeId, out invoiceUsmailcoverLetterPath))
                {
                    Logger.For(this).Invoice("Invoice Calculation-GenerateInvoiceUsmailCoverLetter- report generation success Invoiceid:" + Convert.ToString(objInvoiceReportInput.LinkFieldValue) + " invoiceUsmailcoverLetterPath ::" + invoiceUsmailcoverLetterPath + " errorMessage ::" + errorMessage);
                    objInvoiceReportOutput.ReportOutputPath = invoiceUsmailcoverLetterPath;
                    objInvoiceReportOutput.ErrorMessage = errorMessage;

                    if (!string.IsNullOrEmpty(objInvoiceReportOutput.ReportOutputPath))
                    {
                        objInvoiceReportOutput.IsSuccess = true;
                    }
                }
                else
                {
                    Logger.For(this).Invoice("Invoice Calculation-GenerateInvoiceUsmailCoverLetter-Invoice details.report generation failed. INVOICEID::" + Convert.ToString(objInvoiceReportInput.LinkFieldValue) + " errorMessage::" + errorMessage);
                    objInvoiceReportOutput.IsSuccess = false;
                    objInvoiceReportOutput.ReportOutputPath = string.Empty;
                    objInvoiceReportOutput.ErrorMessage = errorMessage;
                }

                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoiceUsmailCoverLetter-ends successfully ");
                return objInvoiceReportOutput;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoiceUsmailCoverLetter-error " + ex);
                objInvoiceReportOutput.ErrorMessage = ex.Message;
                objInvoiceReportOutput.IsSuccess = false;
                objInvoiceReportOutput.ReportOutputPath = string.Empty;
                return objInvoiceReportOutput;
            }
            finally
            {
                Dispose();
            }


        }

        /// <summary>
        /// added by saravanans..invoice revamp...23/11/2019
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="linkFieldValue"></param>
        /// <param name="clientId"></param>
        /// <param name="servicePackageId"></param>
        /// <param name="taxyear"></param>
        /// <param name="reportExportPath"></param>
        /// <param name="reportOutputPath"></param>
        /// <param name="errorMessage"></param>
        /// <param name="InvoiceTypeId"></param>
        /// <param name="InvoiceAdjustmentRequestId"></param>
        /// <param name="isInvoiceDefects"></param>
        /// <returns></returns>
        public PTXboInvoiceReportOutput GenerateInvoicereports(PTXboInvoiceReportInput objInvoiceReportInput)
        {
            string reportOutputPath, errorMessage;
            errorMessage = string.Empty;
            reportOutputPath = string.Empty;
            bool isOutOfTexas = IsOutOfTexas(objInvoiceReportInput.LinkFieldValue);
            //Added by SaravananS.tfs id:56033
            if(!objInvoiceReportInput.IsDisasterInvoice)
            {
                objInvoiceReportInput.IsDisasterInvoice = IsDisasterInvoice(objInvoiceReportInput.LinkFieldValue);
            }
            //Ends here..
            InvoiceReport objInvoiceReportData = new InvoiceReport();
            ReportGenType exportType = ReportGenType.Pdf;
            PTXboInvoiceReportOutput objInvoiceReportOutput = new PTXboInvoiceReportOutput();

            //added by saravanans
            List<PTXboInvoiceReport> lstInvoiceDetails= new List<PTXboInvoiceReport>();
            List<PTXboInvoiceAccount> lstInvoiceAccount= new List<PTXboInvoiceAccount>();
            PTXboInvoiceContact objClientGroupContact= new PTXboInvoiceContact();
            PTXboNomenclature objServiceDocument= new PTXboNomenclature();
            DataTable dtFlatFeeValidation = new DataTable();
            //if (objInvoiceReportInput.ServicePackageId != 100)
            //{
            //   var objInoiceReportDetails= GetInvoiceReportDetails(objInvoiceReportInput.LinkFieldValue, objInvoiceReportInput.InvoiceTypeId==0? (int)Enumerators.PTXenumInvoiceType.Standard: objInvoiceReportInput.InvoiceTypeId, objInvoiceReportInput.IsInvoiceDefects == null ? false : objInvoiceReportInput.IsInvoiceDefects, objInvoiceReportInput.IsOutOfTexas);
            //    if(objInoiceReportDetails ==null)
            //    {
            //        Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-Invoice details.Sp does not return any value. INVOICEID" + objInvoiceReportInput.LinkFieldValue);
            //    }

            //    lstInvoiceDetails = objInoiceReportDetails.InvoiceDetails;
            //    lstInvoiceAccount = objInoiceReportDetails.InvoiceAccount;

            //    objClientGroupContact = GetInvoiceGroupContactDetails(objInvoiceReportInput.Groupid, objInvoiceReportInput.ClientId);
            //    objServiceDocument=GetNomeclature(objInvoiceReportInput.ServicePackageId, objInvoiceReportInput.ClientId, 0, objInvoiceReportInput.Taxyear);
            //    dtFlatFeeValidation=FlatFeeCheck(objInvoiceReportInput.Groupid, objInvoiceReportInput.ClientId, objInvoiceReportInput.LinkFieldValue);
            //}
            //ends here.
            try
            {
                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-is called " + ((object)objInvoiceReportInput).ToJson(false));
                #region InvoiceAdjustment ServicePackage
                if (objInvoiceReportInput.ServicePackageId == 100) //For InvoiceAdjustment ServicePackage
                {
                    PTXboInvoiceReport objInvoiceReportFromDB = new PTXboInvoiceReport();
                    //List<PTXboInvoiceReport> lstInvoiceDetails;
                    //List<PTXboInvoiceAccount> lstInvoiceAccount;
                    PTXboInvoiceAdjustment objAdjusment = new PTXboInvoiceAdjustment();
                    PTXboInvoice objInvoice = new PTXboInvoice();
                    List<PTXboInvoiceAccount> objInvoiceLienItem = new List<PTXboInvoiceAccount>();
                    List<PTXboInvoiceAdjustmentClarifications> lstInvoiceAdjustmentClarification = new List<PTXboInvoiceAdjustmentClarifications>();
                    List<PTXboExemptionJurisdictions> lstAllExemptionJurisdiction = new List<PTXboExemptionJurisdictions>();
                    //added by saravanans..invoice revamp..23/11/2019
                    PTXboInvoiceAdjustmentReportDetails objInvoiceAdjustmentReportDetails = new PTXboInvoiceAdjustmentReportDetails();
                    objInvoiceAdjustmentReportDetails = GetInvoiceAdjustmentReportDetails(objInvoiceReportInput.LinkFieldValue);
                    lstInvoiceDetails = objInvoiceAdjustmentReportDetails.InvoiceDetails;
                    lstInvoiceAccount = objInvoiceAdjustmentReportDetails.InvoiceAccount;
                    //ends here..



                    objInvoiceReportFromDB.ClientId = objInvoiceReportInput.ClientId;
                    objInvoiceReportFromDB.GroupId = objInvoiceReportInput.Groupid;
                    objInvoiceReportFromDB.Taxyear = objInvoiceReportInput.Taxyear;
                    objInvoiceReportFromDB.InvoiceId = objInvoiceReportInput.LinkFieldValue;

                    foreach (PTXboInvoiceReport objInvoiceReportFrom in lstInvoiceDetails)
                    {
                        objInvoiceReportFromDB.Amount_adjusted = objInvoiceReportFrom.Amount_adjusted;
                        objInvoiceReportFromDB.Interest_adjusted = objInvoiceReportFrom.Interest_adjusted;
                        objInvoiceReportFromDB.Amount_due = objInvoiceReportFrom.Amount_due;
                    }


                    //added by saravanans...invoice revamp...23/11/2019
                    PTXboAutoAdjustmentInvoiceDetails autoAdjustmentInvoiceDetails = new PTXboAutoAdjustmentInvoiceDetails();
                    autoAdjustmentInvoiceDetails = GetAutoAdjustmentInvoiceDetails(objInvoiceReportInput.LinkFieldValue, objInvoiceReportInput.InvoiceAdjustmentRequestId);
                    objAdjusment = autoAdjustmentInvoiceDetails.Adjustment;
                    objInvoice = autoAdjustmentInvoiceDetails.Invoice;
                    objInvoiceLienItem = autoAdjustmentInvoiceDetails.InvoiceLienItem;
                    lstInvoiceAdjustmentClarification = autoAdjustmentInvoiceDetails.InvoiceAdjustmentClarification;
                    //ends here...
                    if (objInvoice.ExemptionJurisdicitonlst != null)
                    {
                        objInvoiceReportFromDB.lstExemptionJurisdiction = objInvoice.ExemptionJurisdicitonlst;
                    }
                    //objInvoiceReportData.generateInvoiceAdjustmentReport(objInvoiceReportFromDB, exportType, objInvoiceReportInput.ReportExportPath, out reportOutputPath, out errorMessage, "InvoiceAdjustment");
                    //added by saravanans
                    var tempInvoice = lstInvoiceDetails.FirstOrDefault();
                    objClientGroupContact = GetInvoiceGroupContactDetails(tempInvoice.GroupId, tempInvoice.ClientId);

                    //int servicePackageID = 0;
                    //servicePackageID = GetServicePackageIDforInvoice("InvoiceAdjustment");

                    objServiceDocument = GetNomeclature(objInvoiceReportInput.ServicePackageId, objInvoiceReportFromDB.ClientId, 0, objInvoiceReportFromDB.Taxyear);
                    objInvoiceReportData.GenerateInvoiceAdjustmentReport(objInvoiceReportFromDB, lstInvoiceDetails, lstInvoiceAccount, objServiceDocument, objClientGroupContact, exportType, objInvoiceReportInput.ReportExportPath, out reportOutputPath, out errorMessage, objInvoiceReportInput.ServicePackageId);
                    objInvoiceReportOutput.ReportOutputPath = reportOutputPath;
                    objInvoiceReportOutput.ErrorMessage = errorMessage;
                    //ends here.
                }
                #endregion InvoiceAdjustment ServicePackage

                //else if (objInvoiceReportData.GenerateStdInvoice(objInvoiceReportInput.Groupid, objInvoiceReportInput.LinkFieldValue, objInvoiceReportInput.ClientId, objInvoiceReportInput.ServicePackageId, objInvoiceReportInput.Taxyear, exportType, objInvoiceReportInput.ReportExportPath, out reportOutputPath, out errorMessage, objInvoiceReportInput.InvoiceTypeId, objInvoiceReportInput.IsInvoiceDefects == null ? false : objInvoiceReportInput.IsInvoiceDefects))
                #region invoice service(62)
                else if (objInvoiceReportInput.ServicePackageId != 100)
                {
                    Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-inside service packageid::" + objInvoiceReportInput.ServicePackageId + " INVOICEID" + objInvoiceReportInput.LinkFieldValue);
                    //Added by SaravananS. tfs id:60377
                    if (objInvoiceReportInput.InvoiceTypeId== 7)
                    {
                        objInvoiceReportOutput = GenerateLegalInvoicereports(objInvoiceReportInput);
                    }
                    else
                    {
                    PTXboInvoiceReportDetailsInput invoiceReportDetailsInput = new PTXboInvoiceReportDetailsInput();
                    invoiceReportDetailsInput.InvoiceId = objInvoiceReportInput.LinkFieldValue;
                    invoiceReportDetailsInput.InvoiceTypeId = objInvoiceReportInput.InvoiceTypeId == 0 ? (int)Enumerators.PTXenumInvoiceType.Standard : objInvoiceReportInput.InvoiceTypeId;
                    invoiceReportDetailsInput.IsInvoiceDefect = objInvoiceReportInput.IsInvoiceDefects == null ? false : objInvoiceReportInput.IsInvoiceDefects;
                    invoiceReportDetailsInput.IsOutOfTexas = objInvoiceReportInput.IsOutOfTexas;
                    invoiceReportDetailsInput.IsOTEntryscreen = objInvoiceReportInput.IsOTEntryscreen;
                    invoiceReportDetailsInput.IsMultiyear = objInvoiceReportInput.IsMultiyear;
                    invoiceReportDetailsInput.IsDisasterInvoice = objInvoiceReportInput.IsDisasterInvoice;
                    //var objInoiceReportDetails = GetInvoiceReportDetails(objInvoiceReportInput.LinkFieldValue, objInvoiceReportInput.InvoiceTypeId == 0 ? (int)Enumerators.PTXenumInvoiceType.Standard : objInvoiceReportInput.InvoiceTypeId, objInvoiceReportInput.IsInvoiceDefects == null ? false : objInvoiceReportInput.IsInvoiceDefects, objInvoiceReportInput.IsOutOfTexas, objInvoiceReportInput.IsOTEntryscreen, objInvoiceReportInput.IsMultiyear, objInvoiceReportInput.IsDisasterInvoice);
                    var objInoiceReportDetails = GetInvoiceReportDetails(invoiceReportDetailsInput);
                    if (invoiceReportDetailsInput.InvoiceTypeId== Enumerators.PTXenumInvoiceType.Judgment.GetId() //Added by SaravananS. tfs id:64496
                            || IsFinalized(objInvoiceReportInput.LinkFieldValue) == true)
                    {
                        if (objInoiceReportDetails == null)
                        {
                            Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-Invoice details.Sp does not return any value. INVOICEID" + objInvoiceReportInput.LinkFieldValue);
                            objInvoiceReportOutput.IsSuccess = false;
                            objInvoiceReportOutput.ReportOutputPath = string.Empty;
                            objInvoiceReportOutput.ErrorMessage = "Invoice Calculation-GenerateInvoicereports-Invoice details.Sp does not return any value. INVOICEID" + objInvoiceReportInput.LinkFieldValue;
                        }
                        else
                        {
                            Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-Invoice details.Sp returns  value. INVOICEID" + objInvoiceReportInput.LinkFieldValue);

                            if (objInoiceReportDetails.InvoiceDetails[0] != null && isOutOfTexas == false && string.IsNullOrEmpty(objInoiceReportDetails.InvoiceDetails[0].Description))  //&& objInvoiceReportInput.IsDisasterInvoice==false
                            {
                                if (objInoiceReportDetails.InvoiceDetails[0].InvoiceGroupType == "Account Level")
                                {
                                    objInoiceReportDetails.InvoiceDetails[0].Description = objInoiceReportDetails.InvoiceDetails[0].PropertyAddress;
                                }
                            }

                            lstInvoiceDetails = objInoiceReportDetails.InvoiceDetails;
                            lstInvoiceAccount = objInoiceReportDetails.InvoiceAccount;

                            objClientGroupContact = GetInvoiceGroupContactDetails(objInvoiceReportInput.Groupid, objInvoiceReportInput.ClientId);

                            objClientGroupContact.LogoPath = GetOconnorCompanyDetails(objInvoiceReportInput.ClientId).FirstOrDefault().LogoPath;//Added by SaravananS. tfs id:61626
                            objClientGroupContact.Signature = GetOconnorCompanyDetails(objInvoiceReportInput.ClientId).FirstOrDefault().Signature;//Added by SaravananS. tfs id:62790
                            objServiceDocument = GetNomeclature(objInvoiceReportInput.ServicePackageId, objInvoiceReportInput.ClientId, 0, objInvoiceReportInput.Taxyear);
                            dtFlatFeeValidation = FlatFeeCheck(objInvoiceReportInput.Groupid, objInvoiceReportInput.ClientId, objInvoiceReportInput.LinkFieldValue);

                            Logger.For(this).Invoice("Dapper-Invoice Calculation-GenerateInvoicereports-Invoice details.report generation started. INVOICEID::" + objInvoiceReportInput.LinkFieldValue + ",lstInvoiceDetails" + ((object)lstInvoiceDetails.FirstOrDefault()).ToJson(false));
                            if (objInvoiceReportData.GenerateInvoiceReport(objInvoiceReportInput.LinkFieldValue.ToString(), lstInvoiceDetails, lstInvoiceAccount, objClientGroupContact, objServiceDocument, dtFlatFeeValidation, exportType, objInvoiceReportInput.ReportExportPath, out reportOutputPath, out errorMessage, objInvoiceReportInput.IsOutOfTexas, objInvoiceReportInput.IsMultiyear, objInvoiceReportInput.IsDisasterInvoice))
                            // if (GenerateInvoiceReport(objInvoiceReportInput.LinkFieldValue.ToString(), lstInvoiceDetails, lstInvoiceAccount, objClientGroupContact, objServiceDocument, dtFlatFeeValidation, exportType, objInvoiceReportInput.ReportExportPath, out reportOutputPath, out errorMessage))
                            {
                                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports- report generation success Invoiceid:" + objInvoiceReportInput.LinkFieldValue + " reportOutputPath ::" + reportOutputPath + " errorMessage ::" + errorMessage);
                                objInvoiceReportOutput.ReportOutputPath = reportOutputPath;
                                objInvoiceReportOutput.ErrorMessage = errorMessage;

                                if (!string.IsNullOrEmpty(objInvoiceReportOutput.ReportOutputPath))
                                {
                                    objInvoiceReportOutput.IsSuccess = true;
                                }

                            }
                            else
                            {
                                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-Invoice details.report generation failed. INVOICEID::" + objInvoiceReportInput.LinkFieldValue + " errorMessage::" + errorMessage);
                                objInvoiceReportOutput.IsSuccess = false;
                                objInvoiceReportOutput.ReportOutputPath = string.Empty;
                                objInvoiceReportOutput.ErrorMessage = errorMessage;
                            }
                        }
                    }


                }
                    
                }
                else
                {
                    Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-hearing not finalized. INVOICEID" + objInvoiceReportInput.LinkFieldValue);
                    objInvoiceReportOutput.IsSuccess = false;
                    objInvoiceReportOutput.ReportOutputPath = string.Empty;
                    objInvoiceReportOutput.ErrorMessage = "Invoice Calculation-GenerateInvoicereports-hearing not finalized. INVOICEID" + objInvoiceReportInput.LinkFieldValue;
                }
                #endregion  invoice service(62)
               
                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-ends successfully ");
                return objInvoiceReportOutput;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-GenerateInvoicereports-error " + ex);
                objInvoiceReportOutput.ErrorMessage = ex.Message;
                objInvoiceReportOutput.IsSuccess = false;
                objInvoiceReportOutput.ReportOutputPath = string.Empty;
                return objInvoiceReportOutput;
            }
            finally
            {
                Dispose();
            }
            

        }



        /// <summary>
        /// Added by saravanans-generating random number
        /// </summary>
        /// <returns></returns>
        public static string Get8CharacterRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path.Substring(0, 8);  // Return 8 character string
        }


        /// <summary>
        /// added by saravanans-Invoice stabilization
        /// </summary>
        /// <param name="exportType"></param>
        /// <param name="reportExportPath"></param>
        /// <param name="reportOutputPath"></param>
        /// <param name="errorMessage"></param>
        /// <param name="InvoiceTypeId"></param>
        /// <param name="isInvoiceDefects"></param>
        /// <returns></returns>
        public bool GenerateInvoiceReport(string invoiceid, List<PTXboInvoiceReport> lstInvoiceDetails, List<PTXboInvoiceAccount> lstInvoiceAccount, PTXboInvoiceContact clientGroupContact, PTXboNomenclature ServiceDocument, DataTable dtFlatFeeValidation, ReportGenType exportType, string reportExportPath, out string reportOutputPath, out string errorMessage)
        {
            errorMessage = string.Empty;
            reportOutputPath = string.Empty;
            Logger.For(this).Invoice("Reached inside GenerateInvoiceReport -Inputs:: invoiceid=" + invoiceid + "=>lstInvoiceDetails::" + lstInvoiceDetails + " lstInvoiceAccount::" + lstInvoiceAccount + " clientGroupContact::" + clientGroupContact + " ServiceDocument::" + ServiceDocument);

            bool isSucess;
            DataSet dsInvoiceDetails = new DataSet();
            DataTable dtserviceDocument = new DataTable();
            DataTable dtInvoiceReport = new DataTable();
            DataTable dtclientGroupContact = new DataTable();
            DataTable dtAccountDetails = new DataTable();
            //DataTable dtFlatFeeValidation = new DataTable();
            try
            {
                               

                dtAccountDetails = lstInvoiceAccount.ToDataTable<PTXboInvoiceAccount>();
                dtAccountDetails.TableName = "dtAccount";
                dtserviceDocument = ServiceDocument.ToDataTable<PTXboNomenclature>();
                dtserviceDocument.TableName = "dtNomenclature";
                dtInvoiceReport = lstInvoiceDetails.ToDataTable<PTXboInvoiceReport>();
                dtInvoiceReport.TableName = "dtInvoice";
                dtclientGroupContact = clientGroupContact.ToDataTable<PTXboInvoiceContact>();
                dtclientGroupContact.TableName = "dtClientContact";
                dsInvoiceDetails.Tables.Add(dtAccountDetails);
                dsInvoiceDetails.Tables.Add(dtclientGroupContact);
                dsInvoiceDetails.Tables.Add(dtInvoiceReport);
                dsInvoiceDetails.Tables.Add(dtserviceDocument);

                isSucess = true;
                int reportFilePathType;
                string InvoiceType = lstInvoiceDetails.Count > 0 ? lstInvoiceDetails[0].InvoiceType : "Standard";
                string ReportFilePath = string.Empty;
                if (InvoiceType == "Standard" || InvoiceType == "Litigation" || InvoiceType == "Arbitration")
                {


                    if (dtFlatFeeValidation != null && dtFlatFeeValidation.Rows.Count > 0 && Convert.ToInt32(dtFlatFeeValidation.Rows[0].ItemArray[0]) == 1 && InvoiceType == "Standard")
                    {
                        ReportFilePath = Path.GetDirectoryName(typeof(ReportData).Assembly.CodeBase) + @"\Reports\Invoice\mstrpt_InvoiceFlatFeePreHearing.rpt";
                        reportFilePathType = 1;//flatfee
                    }
                    else
                    {
                        if (lstInvoiceAccount.Count() > 0)
                        {
                            ReportFilePath = Path.GetDirectoryName(typeof(ReportData).Assembly.CodeBase) + @"\Reports\Invoice\" + ServiceDocument.ReportName;
                            reportFilePathType = 2;//standard
                        }
                        else
                        {
                            ReportFilePath = Path.GetDirectoryName(typeof(ReportData).Assembly.CodeBase) + @"\Reports\Invoice\mstrpt_Invoice_withoutlineitem.rpt";
                            reportFilePathType = 3;//standardwithoutlineitem
                        }
                    }

                }
                else
                {
                    ReportFilePath = Path.GetDirectoryName(typeof(ReportData).Assembly.CodeBase) + @"\Reports\Invoice\mstrpt_Invoice_TaxBill_BPP.rpt";
                    reportFilePathType = 4;//taxbill
                }

                ReportFilePath = ReportFilePath.Replace("file:\\", "");
                Logger.For(this).Invoice("Inside GenerateInvoiceReport -:: invoiceid=" + invoiceid + " ReportFilePath::" + ReportFilePath + "calling GetCrystalReportDocumentForInvoice");
                GetCrystalReportDocumentForInvoice(invoiceid, reportFilePathType, ReportFilePath, reportExportPath + "" + ServiceDocument.DocumentName, dsInvoiceDetails, ReportGenType.Pdf, out reportOutputPath, out errorMessage);
                Logger.For(this).Invoice("Inside GenerateInvoiceReport -GetCrystalReportDocumentForInvoice completed- ReportFilePath::" + ReportFilePath + " reportOutputPath::" + reportOutputPath);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error(ex);
                Logger.For(this).Invoice("Inside GenerateInvoiceReport-Inside catch block-:: invoiceid=" + invoiceid + " -Error  ex.innerException::" + ex.InnerException + " ex ::" + ex);
                errorMessage = ex.Message;
                isSucess = false;
                throw ex;
            }
            finally
            {
                dsInvoiceDetails.Dispose();
                dtserviceDocument.Dispose();
                dtInvoiceReport.Dispose();
                dtclientGroupContact.Dispose();
                dtAccountDetails.Dispose();
                GC.Collect();
            }
            return isSucess;
        }

        
        /// <summary>
        /// The below function will generate the crystal report for invoice based on the input model it receiving.--Added by saravanans
        /// </summary>
        /// <param name="ReportPath"></param>
        /// <param name="ExportPath"></param>
        /// <param name="Model"></param>
        /// <param name="ReportType"></param>
        /// <param name="SuccessMessage"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool GetCrystalReportDocumentForInvoice(string invoiceid,int ReportFilePathType, string ReportPath, string ExportPath, object Model, ReportGenType ReportType, out string reportOutputPath, out string ErrorMessage)
        {
            Logger.For("Invoice Pdf generation").Invoice("Inside GetCrystalReportDocumentForInvoice Inputs :: invoiceid=" + invoiceid + " ::ReportPath=>" + ReportPath + " ExportPath::" + ExportPath);
            ErrorMessage = string.Empty;
            reportOutputPath = string.Empty;
           // ReportDocument rd = new ReportDocument();
            FileStream fileStream = null;
            try
            {
                bool isValid = true;

                string strReportName = ReportPath;
                //The report name should not be empty
                if (string.IsNullOrEmpty(strReportName))
                {
                    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice Inputs :: invoiceid=" + invoiceid + "::strReportName=>" + strReportName + " is invalid");
                    isValid = false;
                }


                if (isValid)
                {
                    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice Inputs :: invoiceid=" + invoiceid + "strReportName=>" + strReportName + "Report template copy generation is started");

                    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice Inputs ::Loading template:: invoiceid=" + invoiceid + "strReportName=>" + strReportName + " is valid");
                   // rd.Load(ReportPath);

                    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice Inputs:: after report template loaded: invoiceid=" + invoiceid + " ::strReportName=>" + strReportName + " is valid.Report template loaded.Model :: " + Model);

                    
                    //if (Model != null && Model.GetType() != typeof(string))
                    //{
                    //    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice Inputs:: invoiceid=" + invoiceid + " ::strReportName=>" + strReportName + " is valid.Model :: " + Model);
                    //    if (Model.GetType() == typeof(DataSet))
                    //    {
                    //        if (((System.Data.DataSet)Model).Tables.Count > 0)
                    //        {
                    //            if (ReportFilePathType == 1)//flatfee
                    //            {
                    //                GlobalData.StandardInvoicewithFlatfee.SetDataSource(((System.Data.DataSet)Model));
                    //                GlobalData.StandardInvoicewithFlatfee.Refresh();
                    //            }
                    //            else if (ReportFilePathType == 2)//standard
                    //            {
                    //                GlobalData.StandardInvoice.SetDataSource(((System.Data.DataSet)Model));
                    //                GlobalData.StandardInvoice.Refresh();
                    //            }
                    //            else if (ReportFilePathType == 3)//standardwithoutlineitem
                    //            {
                    //                GlobalData.StandardInvoicewithoutlineitem.SetDataSource(((System.Data.DataSet)Model));
                    //                GlobalData.StandardInvoicewithoutlineitem.Refresh();
                    //            }
                    //            else if (ReportFilePathType == 4)//taxbill
                    //            {
                    //                GlobalData.TaxbillInvoice.SetDataSource(((System.Data.DataSet)Model));
                    //                GlobalData.TaxbillInvoice.Refresh();
                    //            }
                    //               // rd.SetDataSource(((System.Data.DataSet)Model));
                    //        }

                    //    }
                    //    else
                    //    {
                    //       // rd.SetDataSource(new[] { Model });
                    //        if (ReportFilePathType == 1)//flatfee
                    //        {
                    //            GlobalData.StandardInvoicewithFlatfee.SetDataSource(new[] { Model });
                    //            GlobalData.StandardInvoicewithFlatfee.Refresh();
                    //        }
                    //        else if (ReportFilePathType == 2)//standard
                    //        {
                    //            GlobalData.StandardInvoice.SetDataSource(new[] { Model });
                    //            GlobalData.StandardInvoice.Refresh();
                    //        }
                    //        else if (ReportFilePathType == 3)//standardwithoutlineitem
                    //        {
                    //            GlobalData.StandardInvoicewithoutlineitem.SetDataSource(new[] { Model });
                    //            GlobalData.StandardInvoicewithoutlineitem.Refresh();
                    //        }
                    //        else if (ReportFilePathType == 4)//taxbill
                    //        {
                    //            GlobalData.TaxbillInvoice.SetDataSource(new[] { Model });
                    //            GlobalData.TaxbillInvoice.Refresh();
                    //        }
                    //    }
                    //}
                    //rd.Refresh();

                    ExportPath = ExportPath + DateTime.Now.ToString("hh_mm_ss_ffffff") + "_" + Get8CharacterRandomString() + "." + ReportType;
                    reportOutputPath = ExportPath;
                    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice .File path is created.:: invoiceid=" + invoiceid + " ::reportOutputPath=>" + reportOutputPath);
                    ReportGenResult reportResult= GlobalData.SetDataSource(ReportFilePathType, Model, ReportType);
                    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice :: invoiceid=" + invoiceid + ".File is going to be generated");
                    //if (ReportType == ReportGenType.Pdf)
                    //{
                    //    if (ReportFilePathType == 1)//flatfee
                    //    {
                    //        Stream _CrystalReportObject = GlobalData.StandardInvoicewithFlatfee.ExportToStream(ExportFormatType.PortableDocFormat);
                    //        reportResult = new ReportGenResult() { MimeType = "application/pdf", FileNameExtension = "pdf", CrystalReportObject = _CrystalReportObject };
                    //    }
                    //    else if (ReportFilePathType == 2)//standard
                    //    {
                    //        Stream _CrystalReportObject = GlobalData.StandardInvoice.ExportToStream(ExportFormatType.PortableDocFormat);
                    //        reportResult = new ReportGenResult() { MimeType = "application/pdf", FileNameExtension = "pdf", CrystalReportObject = _CrystalReportObject };
                    //    }
                    //    else if (ReportFilePathType == 3)//standardwithoutlineitem
                    //    {
                    //        Stream _CrystalReportObject = GlobalData.StandardInvoicewithoutlineitem.ExportToStream(ExportFormatType.PortableDocFormat);
                    //        reportResult = new ReportGenResult() { MimeType = "application/pdf", FileNameExtension = "pdf", CrystalReportObject = _CrystalReportObject };
                    //    }
                    //    else if (ReportFilePathType == 4)//taxbill
                    //    {
                    //        Stream _CrystalReportObject = GlobalData.TaxbillInvoice.ExportToStream(ExportFormatType.PortableDocFormat);
                    //        reportResult = new ReportGenResult() { MimeType = "application/pdf", FileNameExtension = "pdf", CrystalReportObject = _CrystalReportObject };
                    //    }
                    //    else
                    //    {
                    //        reportResult = new ReportGenResult() { CrystalReportObject = null };
                    //    }
                       
                    //}
                    //else
                    //{
                    //    reportResult = new ReportGenResult() { CrystalReportObject = null };
                    //}
                    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice:: invoiceid=" + invoiceid + " ExportPath ::" + ExportPath + " ExportPath:" + ExportPath);
                    fileStream = new FileStream(ExportPath, FileMode.Create, FileAccess.Write);
                    if (reportResult.CrystalReportObject != null)
                    {
                        Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice:: invoiceid=" + invoiceid + " .File generation started from filestream. ExportPath::" + ExportPath);
                        reportResult.CrystalReportObject.CopyTo(fileStream);
                        //SuccessMessage = "Letter Generated Successfully";
                        Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice:: invoiceid=" + invoiceid + " .File was Generated Successfully.ExportPath::" + ExportPath);

                        return true;
                    }
                    else
                    {
                        Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice:: invoiceid=" + invoiceid + " reportResult.CrystalReportObject is null");

                        return false;
                    }

                }
                else
                {
                    Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice:: invoiceid=" + invoiceid + " strReportName " + strReportName + "is invalid");
                    return false;
                }


            }
            catch (Exception ex)
            {

               // Logger.For("Report Base").Error(ex);
                ErrorMessage = "Error in Invoice pdf Generation " + ex.ToString();
                Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice :: invoiceid=" + invoiceid + "ExportPath::" + ExportPath + ".inside catch block.Error in Invoice pdf Generation: " + ex.ToString());
                throw;
            }

            finally
            {
               // Logger.For("Report Base").Info("GetCrystalReportDocument End");
                Logger.For("Invoice Pdf generation").Invoice("GetCrystalReportDocumentForInvoice ends  :: invoiceid=" + invoiceid + " ExportPath::" + ExportPath + ".inside finally block.");
                fileStream.Close();
                fileStream.Dispose();
               // rd.Close();
                //rd.Dispose();
                GC.Collect();
            }
        }


        #endregion Report Generation

        #region InvoiceAmountCalculation
        public bool InvoiceAmountCalculation(decimal Contingenceyfee,decimal? flatfee, decimal InvcapValue, out decimal InvoiceAmount)
        {
            bool returnValue = true;
            InvoiceAmount = 0;
            try
            {
                Logger.For(this).Invoice("Inside Invoice Calculation-InvoiceAmountCalculation-reached ");

            if (InvcapValue > 0)
            {
                if (Contingenceyfee+ flatfee > InvcapValue)    //flatfee added after discussing with manoj.tfs id:48680,49045--saravanans
                {
                    //objInvoice.ContingencyFee = invoicecapvalue;
                    InvoiceAmount = InvcapValue;
                }
                else if (InvcapValue >= Contingenceyfee+ flatfee)
                {
                    InvoiceAmount = Contingenceyfee;
                }
            }
            else
            {
                InvoiceAmount = Contingenceyfee;
            }

                Logger.For(this).Invoice("Invoice Calculation-InvoiceAmountCalculation-ends successfully ");
                return returnValue;

            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("Invoice Calculation-InvoiceAmountCalculation-error " + ((object)ex).ToJson(false));
                return false;
                
            }
            finally
            {
                Dispose();
            }
        }

        #endregion
    }
}
