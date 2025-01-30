using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.DataObjects;
using Spartaxx.Utilities.Extenders;
using Spartaxx.Utilities.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    /// <summary>
    /// added by saravanans-tfs:47247
    /// </summary>
   public class InvoiceFlatFeePreRepository:IInvoiceFlatFeePreRepository
    {
        private readonly DapperConnection _dapperConnection;
        private readonly InvoiceCalculationRepository _invoiceCalculation;
        public InvoiceFlatFeePreRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
            _invoiceCalculation = new InvoiceCalculationRepository(dapperConnection);
        }

        public List<PTXboInvoiceFlatFeePreAccountDetails> GetInvoiceFlatFeePreAccountDetail(PTXboInvoiceFlatFeeInput invoiceFlatFeeInput)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceFlatFeePreAccountDetail-API  reached " + ((object)invoiceFlatFeeInput).ToJson(false));
                Hashtable parameters = new Hashtable();
                List<PTXboInvoiceFlatFeePreAccountDetails> invoiceDetails = new List<PTXboInvoiceFlatFeePreAccountDetails>();
                parameters.Add("@ClientId", invoiceFlatFeeInput.ClientId);
                parameters.Add("@GroupID", invoiceFlatFeeInput.GroupId);
                parameters.Add("@InvoiceGroupingTypeId", invoiceFlatFeeInput.InvoiceGroupingTypeId);
                invoiceDetails = _dapperConnection.Select<PTXboInvoiceFlatFeePreAccountDetails>(StoredProcedureNames.usp_getInvoiceFlatFeePreHearingAccountDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetInvoiceFlatFeePreAccountDetail-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceFlatFeePreAccountDetail-API  error " + ex);
                throw ex;
            }
        }

        public List<PTXboInvoiceFlatFeePre> GetInvoiceFlatFeePreClientDetail(PTXboFlatFeeClientDetail flatFeeClientDetail)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoiceFlatFeePreClientDetail-API  reached " + ((object)flatFeeClientDetail).ToJson(false));
                Hashtable parameters = new Hashtable();
                List<PTXboInvoiceFlatFeePre> invoiceDetails = new List<PTXboInvoiceFlatFeePre>();

                 parameters.Add("@CLIENTNUMBER", (flatFeeClientDetail.ClientNumber != "" && flatFeeClientDetail.ClientNumber != null ? flatFeeClientDetail.ClientNumber : null));
                 parameters.Add("@CLIENTNAME", (flatFeeClientDetail.ClientName != "" && flatFeeClientDetail.ClientName != null ? flatFeeClientDetail.ClientName : null));
                 parameters.Add("@ACCOUNTNUMBER", (!string.IsNullOrEmpty(flatFeeClientDetail.AccountNumber) ? flatFeeClientDetail.AccountNumber : null));
                 parameters.Add("@PROJECTNAME", (!string.IsNullOrEmpty(flatFeeClientDetail.ProjectName) ? flatFeeClientDetail.ProjectName : null));
                invoiceDetails = _dapperConnection.Select<PTXboInvoiceFlatFeePre>(StoredProcedureNames.usp_get_InvoiceFlatFeePreClietDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetInvoiceFlatFeePreClientDetail-API  ends successfully ");
                return invoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoiceFlatFeePreClientDetail-API  error " + ex);
                throw ex;
            }
        }

        public bool FlatFeeInvoiceGeneration(PTXboFlatFeeInvoiceGenerationInput flatFeeInvoice,out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                Logger.For(this).Invoice("FlatFeeInvoiceGeneration-API  reached " + ((object)flatFeeInvoice).ToJson(false));
                var result=  _invoiceCalculation.FlatFeeInvoiceGeneration(flatFeeInvoice,out errorMessage);
                //Hashtable parameters = new Hashtable();

                // parameters.Add("@ClientId", flatFeeInvoice.InvoiceFlatFeePre.clientId);
                // parameters.Add("@GroupId", flatFeeInvoice.InvoiceFlatFeePre.GroupId);
                // parameters.Add("@InvoiceGenerateType", flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId);

                //var dtResult = _dapperConnection.SelectDataSet(StoredProcedureNames.usp_get_FlatFeeInvoiceGenerateAccountDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                //if(dtResult.Tables.Count==0)
                //{
                //    if (flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId() || flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGroupingType.AccountLevel.GetId())
                //    {
                //        errorMessage = "Invoice already Generated";
                //    }

                //    return false;
                //}
                //else
                //{
                //    Hashtable parameter = new Hashtable();
                //    parameter.Add("@ClientId", flatFeeInvoice.InvoiceFlatFeePre.clientId);
                //    parameter.Add("@GroupId", flatFeeInvoice.InvoiceFlatFeePre.GroupId);
                //    parameter.Add("@InvoiceGenerateType", flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId);
                //    var output = _dapperConnection.SelectDataSet(StoredProcedureNames.usp_get_FlatFeeInvoiceGenerateProjectDetails, parameter, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                //    DataTable ProjectDTResult =(output!=null)? output.Tables[0]:new DataTable();
                //    if (flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGroupingType.ProjectLevel.GetId())
                //    {
                //        foreach (DataRow row in ProjectDTResult.Rows)
                //        {
                //            PTXboProject dProject = new PTXboProject();
                //            dProject.ProjectId = Convert.ToInt32(row.ItemArray[0]);

                //            PTXboInvoice Invoice =InsertInvoice(flatFeeInvoice.InvoiceFlatFeePre, dProject, ProjectDTResult, flatFeeInvoice.CurrentUserId);
                //            if (Invoice != null)
                //            {
                //                InsertInvoiceSummary(Invoice, flatFeeInvoice.InvoiceFlatFeePre, ProjectDTResult);
                //                InsertCorrQueueAndAccounts(flatFeeInvoice.InvoiceFlatFeePre, Invoice, flatFeeInvoice.CurrentUserId, ProjectDTResult, dProject);
                //            }
                //        }
                //    }
                //    else if (flatFeeInvoice.InvoiceFlatFeePre.InvoiceGroupingTypeId == Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGroupingType.TermLevel.GetId())
                //    {
                //        foreach (DataRow row in ProjectDTResult.Rows)
                //        {
                //            //PTXdoProject dProject = new PTXdoProject();
                //            //dProject.ProjectId = Convert.ToInt32(row.ItemArray[0]);

                //            PTXboInvoice Invoice = InsertInvoice(flatFeeInvoice.InvoiceFlatFeePre, null, ProjectDTResult, flatFeeInvoice.CurrentUserId);
                //            if (Invoice != null)
                //            {
                //                InsertInvoiceSummary(Invoice, flatFeeInvoice.InvoiceFlatFeePre, ProjectDTResult);
                //                InsertCorrQueueAndAccounts(flatFeeInvoice.InvoiceFlatFeePre, Invoice, flatFeeInvoice.CurrentUserId, ProjectDTResult, null);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        foreach (DataRow row in dtResult.Tables[0].Rows)
                //        {
                //            DataTable dataTable = new DataTable();
                //            dataTable.Columns.Add("AccountId");
                //            dataTable.Columns.Add("GroupId");
                //            dataTable.ImportRow(row);
                //            PTXboInvoice Invoice = InsertInvoice(flatFeeInvoice.InvoiceFlatFeePre, null, dataTable, flatFeeInvoice.CurrentUserId);
                //            if (Invoice != null)
                //            {
                //                InsertInvoiceSummary(Invoice, flatFeeInvoice.InvoiceFlatFeePre, dataTable);
                //                InsertCorrQueueAndAccounts(flatFeeInvoice.InvoiceFlatFeePre, Invoice, flatFeeInvoice.CurrentUserId, dataTable, null);
                //            }
                //        }
                //    }
                Logger.For(this).Invoice("FlatFeeInvoiceGeneration-API  ends successfully ");
                    return true;
               // }
            

        }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("-API  error " + ex);

                throw ex;
            }
        }

        //public int GetDefaultDeliveryType(int clientID)
        //{
        //    try
        //    {
        //        Logger.For(this).Invoice("GetDefaultDeliveryType-API  reached " + ((object)clientID).ToJson(false));
        //        Hashtable parameters = new Hashtable();
        //        parameters.Add("@ClientID", clientID);
        //        var result = Convert.ToInt32(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_get_DefaultDeliveryType, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
        //        Logger.For(this).Invoice("GetDefaultDeliveryType-API  ends successfully ");
        //        return result;
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetDefaultDeliveryType-API  error " + ex);
        //        throw ex;
        //    }
        //}

        //public int GetServicePackageID(string letterName)
        //{
        //    try
        //    {
        //        Logger.For(this).Invoice("GetServicePackageID-API  reached " + ((object)letterName).ToJson(false));
        //        Hashtable parameters = new Hashtable();
        //        parameters.Add("@letterName", letterName);
        //        int servicePackageID= Convert.ToInt32(_dapperConnection.ExecuteScalar(StoredProcedureNames.usp_GetServicePackageID, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx));
        //        Logger.For(this).Invoice("GetServicePackageID-API  ends successfully ");
        //        return servicePackageID;
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.For(this).Invoice("GetServicePackageID-API  error " + ex);
        //        throw ex;
        //    }
        //}

        //private void InsertCorrQueueAndAccounts(PTXboInvoiceFlatFeePre invoiceFlatFeePre, PTXboInvoice invoice, int currentUserId, DataTable dataTable, PTXboProject project)
        //{
        //    string errorString = string.Empty;
        //    int servicePackageID = 0;

        //    string letterName = "Invoice";
        //    Logger.For(this).Invoice("InsertCorrQueueAndAccounts- reached " + ((object)invoiceFlatFeePre).ToJson(false));
        //    string currentTaxYear = string.Empty;
        //    currentTaxYear = _invoiceRepository.GetParamValue(Convert.ToInt32(EnumConstants.PTXParameters.CurrentTaxYear));

        //    servicePackageID = GetServicePackageID(letterName);

        //    var delivaryMethod = GetDefaultDeliveryType(invoiceFlatFeePre.clientId);

        //    PTXboCorrQueue objCorrQueue = new PTXboCorrQueue();
        //    objCorrQueue.ClientID = invoiceFlatFeePre.clientId ;
        //    objCorrQueue.TaxYear = Convert.ToInt32(currentTaxYear);
        //    if (servicePackageID != 0)
        //    {
        //        objCorrQueue.ServicePackageID =  servicePackageID;
        //    }
        //    objCorrQueue.CorrProcessingStatusID = Spartaxx.DataObjects.Enumerators.PTXenumCorrProcessingStatus.Active.GetId();
        //    objCorrQueue.SentToClient = true;

        //    if (delivaryMethod != 0)
        //    {
        //        objCorrQueue.DeliveryMethodId = delivaryMethod;
        //    }

        //    objCorrQueue.LinkFieldValue = invoice.InvoiceID;
        //    objCorrQueue.CreatedBy = currentUserId;
        //    objCorrQueue.CreatedDateTime = DateTime.Now;
        //    for (int i = 0; i < dataTable.Rows.Count; i++)
        //    {
        //        if (project != null)
        //        {
        //            if (project.ProjectId == Convert.ToInt32(dataTable.Rows[i].ItemArray[2]))
        //            {
        //                objCorrQueue.AccountList =string.Join(",", Convert.ToInt32(dataTable.Rows[i].ItemArray[0]));
        //            }
        //        }
        //        else
        //        {
        //            objCorrQueue.AccountList = string.Join(",", Convert.ToInt32(dataTable.Rows[i].ItemArray[0]));
        //        }
        //    }
        //    _invoiceRepository.SaveOrUpdateCorrQ(objCorrQueue);
        //    Logger.For(this).Invoice("InsertCorrQueueAndAccounts-ends successfully ");
        //}



        //private void InsertInvoiceSummary(PTXboInvoice Invoice, PTXboInvoiceFlatFeePre invoiceFlatFeePre, DataTable dataTable)
        //{
        //    Logger.For(this).Invoice("InsertInvoiceSummary- reached " + ((object)Invoice).ToJson(false));
        //    PTXboInvoiceSummary InvoiceSummary = new PTXboInvoiceSummary();
        //    InvoiceSummary.InvoiceGenerated = true;
        //    InvoiceSummary.InvoiceGeneratedForID = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceGeneratedFor.FlatFeePre.GetId();
        //    InvoiceSummary.InvoiceSummaryProcessingStatusID = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceSummaryProcessingStatus.FlatFeePre.GetId();
        //    InvoiceSummary.InvoiceStatusID = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceStatus.InvoiceRecordNotCreated.GetId();
        //    InvoiceSummary.ClientId = invoiceFlatFeePre.clientId;
        //    InvoiceSummary.GroupId = invoiceFlatFeePre.GroupId;
        //    InvoiceSummary.CreateDateTime = DateTime.Now;

        //    _invoiceRepository.SaveOrUpdateInvoiceSummary(InvoiceSummary);
        //    InsertInvoiceListMap(InvoiceSummary, Invoice);
        //    Logger.For(this).Invoice("InsertInvoiceSummary-ends successfully ");

        //}
        //private void InsertInvoiceListMap(PTXboInvoiceSummary invoiceSummary, PTXboInvoice invoice)
        //{
        //    Logger.For(this).Invoice("InsertInvoiceListMap-reached " + ((object)invoiceSummary).ToJson(false));
        //    PTXboInvoiceListMap InvoiceListMap = new PTXboInvoiceListMap();
        //    InvoiceListMap.InvoiceSummaryId = invoiceSummary.InvoicesummaryID;
        //    InvoiceListMap.InvoiceId = invoice.InvoiceID;
        //    SaveOrUpdateInvoiceListItem(InvoiceListMap);

        //}
        //public int SaveOrUpdateInvoiceListItem(PTXboInvoiceListMap invoiceListMap)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceListItem-API  reached " + ((object)invoiceListMap).ToJson(false));
        //        parameters.Add("@InvoiceListMapId", invoiceListMap.InvoiceListMapId);
        //        parameters.Add("@InvoiceId", invoiceListMap.InvoiceId);
        //        parameters.Add("@InvoiceSummaryId", invoiceListMap.InvoiceSummaryId);

        //        var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceListMap, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceListItem-API  ends successfully ");
        //        return Convert.ToInt32(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceListItem-API  error " + ex);
        //        throw ex;
        //    }
        //}
        //private PTXboInvoice InsertInvoice(PTXboInvoiceFlatFeePre invoiceFlatFeePre, PTXboProject project, DataTable dataTable, int currentUserId)
        //{
        //    PTXboInvoice Invoice = null;
        //    int invoiceID = 0;
        //    Logger.For(this).Invoice("InsertInvoice-  reached " + ((object)invoiceFlatFeePre).ToJson(false));
        //    string currentTaxYear = string.Empty;
        //    currentTaxYear= _invoiceRepository.GetParamValue(Convert.ToInt32(EnumConstants.PTXParameters.CurrentTaxYear));

        //    Invoice = new PTXboInvoice();
        //    Invoice.GroupId = invoiceFlatFeePre.GroupId; 
        //    Invoice.ClientId = invoiceFlatFeePre.clientId;
        //    Invoice.ProjectId = project.ProjectId;
        //    Invoice.InvoicingStatusId = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceStatus.InvoiceGenerated.GetId();
        //    Invoice.InvoiceTypeId = Spartaxx.DataObjects.Enumerators.PTXenumInvoiceType.Standard.GetId(); ;
        //    Invoice.ContingencyFee = Convert.ToDecimal(invoiceFlatFeePre.Contingency);
        //    Invoice.InvoiceAmount = Convert.ToDecimal(invoiceFlatFeePre.FlatFee);
        //    Invoice.CreatedDateAndTime = DateTime.Now;
        //    Invoice.TaxYear = Convert.ToInt32(currentTaxYear);
        //    Invoice.InvoiceGroupingTypeId = invoiceFlatFeePre.InvoiceGroupingTypeId; ;
        //    //Invoice.InvoiceDate = DateTime.Now;
        //    Invoice.FlatFee = Convert.ToDecimal(invoiceFlatFeePre.FlatFee);
        //    Invoice.AmountDue = Convert.ToDecimal(invoiceFlatFeePre.FlatFee);
        //    Invoice.PaymentDueDate = DateTime.Now.AddDays(30);
        //    Invoice.PaymentStatusId = Spartaxx.DataObjects.Enumerators.PTXenumPaymentStatus.NotPaid.GetId();
        //    Invoice.UpdatedBy = currentUserId;
        //    Invoice.UpdatedDateTime = DateTime.Now;
            
        //   _invoiceRepository.SaveOrUpdateInvoice(Invoice,out invoiceID);
        //   Invoice.InvoiceID = invoiceID;
        //   InsertInvoiceListItem(Invoice, invoiceFlatFeePre, dataTable, project);
        //   Logger.For(this).Invoice("InsertInvoice-  ends successfully ");
        //   return Invoice;
        //}



        //public int SaveOrUpdateInvoiceLineItem(PTXboInvoiceLineItem invoiceLineItem)
        //{
        //    Hashtable parameters = new Hashtable();

        //    try
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceLineItem-API  reached " + ((object)invoiceLineItem).ToJson(false));
        //        parameters.Add("@InvoiceLineItemID	", invoiceLineItem.InvoiceLineItemID);
        //        parameters.Add("@InvoiceID	", invoiceLineItem.InvoiceID);
        //        parameters.Add("@AccountID	", invoiceLineItem.AccountID);
        //        parameters.Add("@InitialAssessedValue	", invoiceLineItem.InitialAssessedValue);
        //        parameters.Add("@FinalAssessedValue	", invoiceLineItem.FinalAssessedValue);
        //        parameters.Add("@PriorYearTaxRate	", invoiceLineItem.PriorYearTaxRate);
        //        parameters.Add("@EstimatedTaxSavings	", invoiceLineItem.EstimatedTaxSavings);
        //        parameters.Add("@Reduciton	", invoiceLineItem.Reduciton);
        //        parameters.Add("@UpdatedBy	", invoiceLineItem.UpdatedBy);
        //        parameters.Add("@UpdatedDateAndTime	", invoiceLineItem.UpdatedDateAndTime);
        //        parameters.Add("@IntitalLand	", invoiceLineItem.IntitalLand);
        //        parameters.Add("@IntialImproved	", invoiceLineItem.IntialImproved);
        //        parameters.Add("@InitialMarket	", invoiceLineItem.InitialMarket);
        //        parameters.Add("@FinalLand	", invoiceLineItem.FinalLand);
        //        parameters.Add("@FinalImproved	", invoiceLineItem.FinalImproved);
        //        parameters.Add("@FinalMarket	", invoiceLineItem.FinalMarket);


        //        var result = _dapperConnection.ExecuteScalar(StoredProcedureNames.usp_insertorupdateInvoiceLineItem, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceLineItem-API  ends successfully ");
        //        return Convert.ToInt32(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Invoice("SaveOrUpdateInvoiceLineItem-API  error " + ex);
        //        throw ex;
        //    }
        //}
        //private void InsertInvoiceListItem(PTXboInvoice Invoice, PTXboInvoiceFlatFeePre invoiceFlatFeePre, DataTable dataTable, PTXboProject Project)
        //{
        //    Logger.For(this).Invoice("InsertInvoiceListItem-reached " + ((object)Invoice).ToJson(false));
        //    for (int i = 0; i < dataTable.Rows.Count; i++)
        //    {
        //        if (Project != null)
        //        {
        //            if (Project.ProjectId == Convert.ToInt32(dataTable.Rows[i].ItemArray[2]))
        //            {

        //                PTXboInvoiceLineItem InvoiceLineItem = new PTXboInvoiceLineItem();
        //                InvoiceLineItem.InvoiceID = Invoice.InvoiceID;
        //                InvoiceLineItem.AccountID = Convert.ToInt32(dataTable.Rows[i].ItemArray[0]);
        //                SaveOrUpdateInvoiceLineItem(InvoiceLineItem);
                        
                        
        //            }
        //        }
        //        else
        //        {

        //            PTXboInvoiceLineItem InvoiceLineItem = new PTXboInvoiceLineItem();
        //            InvoiceLineItem.InvoiceID = Invoice.InvoiceID;
        //            InvoiceLineItem.AccountID = Convert.ToInt32(dataTable.Rows[i].ItemArray[0]);
        //            SaveOrUpdateInvoiceLineItem(InvoiceLineItem);
                    
                    
        //        }

        //    }
        //    Logger.For(this).Invoice("InsertInvoiceListItem-ends successfully ");
        //}

    }
}
