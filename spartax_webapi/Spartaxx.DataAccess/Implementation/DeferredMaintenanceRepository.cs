using Spartaxx.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spartaxx.Paperwise;
using Spartaxx.Utilities.Logging;
using Spartaxx.BusinessObjects.Concierge;
using Spartaxx.BusinessObjects;
using Spartaxx.Common.BusinessObjects;

namespace Spartaxx.DataAccess
{
    public class DeferredMaintenanceRepository: IDeferredMaintenanceRepository
    {

        private readonly DapperConnection _Connection;
        public DeferredMaintenanceRepository(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        #region Paper Wise Image Update

        public bool SendPaperWiseDeferredMaintenancePhoto(PTXdtDeferredMaintenanceImageUpload PWObj, out string errorMessage)
        {
            bool isSucess = false;
            errorMessage = string.Empty;
            StringBuilder ProcessMessage = new StringBuilder();
            double  imageId = 0;
            string FileCabinetID = ConfigurationManager.AppSettings["FileCabinet"].ToString() ?? "1015";
            try
            {
                Hashtable _hashtable = new Hashtable();
                _hashtable.Add("@UserId", 0);
                _hashtable.Add("@PropertydetailsId", PWObj.PropertydetailsId);

               var ImageUploadLS = _Connection.Select<PTXdtDeferredMaintenanceImageUpload>(StoredProcedureNames.usp_DM_GetPropertyMediaDetailList,
                       _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);




                if (ImageUploadLS.Any())
                {

                    foreach (var ImageDet in ImageUploadLS)
                    {
                        PwObjectModel.PwCollection pwCollection = new PwObjectModel.PwCollection();
                        PwObjectModel.FieldValue FieldValue = new PwObjectModel.FieldValue();


                        if (!string.IsNullOrEmpty(ImageDet.AccountNumber))
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custAccountNumber";
                            FieldValue.FieldValue = ImageDet.AccountNumber;
                            pwCollection.Add(FieldValue, "custAccountNumber");
                        }
                        if (!string.IsNullOrEmpty(ImageDet.ClientNumber))
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custClientNumber";
                            FieldValue.FieldValue = ImageDet.ClientNumber;
                            pwCollection.Add(FieldValue, "custClientNumber");
                        }


                        if (ImageDet.TaxYear>0)
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custTaxYear";
                            FieldValue.FieldValue = ImageDet.TaxYear.ToString();
                            pwCollection.Add(FieldValue, "custTaxYear");
                        }
                        if (!string.IsNullOrEmpty(ImageDet.CountyName))
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custCounty";
                            FieldValue.FieldValue = ImageDet.CountyName;
                            pwCollection.Add(FieldValue, "custCounty");
                        }
                        if (!string.IsNullOrEmpty(ImageDet.DocumentType))
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custDocType";
                            FieldValue.FieldValue = ImageDet.DocumentType;
                            pwCollection.Add(FieldValue, "custDocType");
                        }
                       
                       
                        FieldValue = new PwObjectModel.FieldValue();
                        FieldValue.FieldName = "custScanDate";
                        FieldValue.FieldValue = Convert.ToDateTime(System.DateTime.Now.ToShortDateString()).ToString("yyyy/MM/dd").Replace("-", "/");
                        pwCollection.Add(FieldValue, "custScanDate");

                        // double imageId;

                        ImageDet.MediaPath= ConfigurationManager.AppSettings["DMImagePath"].ToString()+ ImageDet.MediaPath;
                       PaperwiseInterface obj = new PaperwiseInterface();
                       obj.sendToPaperwise(ImageDet.MediaPath, Convert.ToString(FileCabinetID), 0, pwCollection, out imageId,out errorMessage);
                       
                        if (imageId > 0)
                        {
                            Hashtable _hashtableImageIDUpdate = null;
                            _hashtableImageIDUpdate = new Hashtable();
                            _hashtableImageIDUpdate.Add("@MediaId", ImageDet.MediaId);
                            _hashtableImageIDUpdate.Add("@ImageId", imageId);
                            var result = _Connection.Execute(StoredProcedureNames.usp_DM_MediaDetailMapPaperwise,
                                _hashtableImageIDUpdate, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx, true);
                        }

                    }



                }

               

                isSucess = true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SendPaperWiseDeferredMaintenancePhoto-API  error " + ex);
                errorMessage = ex.Message;
                isSucess = false;
                throw ex;
            }
            return isSucess;
        }

        #endregion


        #region Paper Wise Image Update

        public bool DMFinalvalueUpdate(PTXdtPropertydetailsUpdateParam PWPropertydetail, out string errorMessage)
        {
            bool isSucess = false;
            errorMessage = string.Empty;
         
            try
            {
                Hashtable _hashtable = new Hashtable();
               
                _hashtable.Add("@AccountNumber", PWPropertydetail.AccountNumber);
                _hashtable.Add("@County", PWPropertydetail.County);
                _hashtable.Add("@Taxyear", PWPropertydetail.TaxYear);
                _hashtable.Add("@FinalValue", PWPropertydetail.FinalValue);
                _hashtable.Add("@UpdatedUser", PWPropertydetail.UpdatedUser);

                var result = _Connection.Execute(StoredProcedureNames.usp_DM_UpdateDMFinalValue,
                               _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx, true);
                isSucess = true;
            }
            catch (Exception ex)
            {
              
                Logger.For(this).Invoice("DMFinalvalueUpdate-API  error " + ex);
                errorMessage = ex.Message;
                isSucess = false;
                throw ex;
            }
            return isSucess;
        }

        #endregion


        #region Concierge Link Generate

        public PTXdtConciergeLinkGenerateParam DMConciergeLinkGenerate(PTXdtConciergeLinkGenerateParam PWPropertydetail, out string errorMessage)
        {
            bool isSucess = false;
            errorMessage = string.Empty;

            try
            {
                Hashtable _hashtable = new Hashtable();

                PTXdtConciergeLinkGenerateParam Propertydetails = new PTXdtConciergeLinkGenerateParam();

                _hashtable.Add("@AccountNumber", PWPropertydetail.AccountNumber);
                _hashtable.Add("@County", PWPropertydetail.County);
                _hashtable.Add("@Taxyear", 0);
                _hashtable.Add("@UserId", PWPropertydetail.UserId);
                _hashtable.Add("@ConciergeAgentID", PWPropertydetail.ConciergeAgentID);

                Propertydetails = _Connection.Select<PTXdtConciergeLinkGenerateParam>(StoredProcedureNames.USP_DM_ConciergeLinkGenerate, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();


                Propertydetails.DMLink = ConfigurationManager.AppSettings["DMLink"].ToString() + Propertydetails.DMLink;

                return Propertydetails;
            }
            catch (Exception ex)
            {

                Logger.For(this).Invoice("DMFinalvalueUpdate-API  error " + ex);
                errorMessage = ex.Message;
                isSucess = false;
                throw ex;
               
            }
            return new PTXdtConciergeLinkGenerateParam();
        }

        #endregion

        public bool SendPaperWiseConcierge(PTXboSentToPaperwiseConcierge PWObj, out string errorMessage)
        {
            bool isSucess = false;
            errorMessage = string.Empty;
            StringBuilder ProcessMessage = new StringBuilder();
            double imageId = 0;
            try
            {
                //Hashtable _hashtable = new Hashtable();
                //_hashtable.Add("@UserId", 0);
                //_hashtable.Add("@PropertydetailsId", PWObj.PropertydetailsId);

                //var ImageUploadLS = _Connection.Select<PTXdtDeferredMaintenanceImageUpload>(StoredProcedureNames.usp_DM_GetPropertyMediaDetailList,
                //        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);




                //if (ImageUploadLS.Any())
                //{

                //    foreach (var ImageDet in ImageUploadLS)
                //    {
                        PwObjectModel.PwCollection pwCollection = new PwObjectModel.PwCollection();
                        PwObjectModel.FieldValue FieldValue = new PwObjectModel.FieldValue();


                        if (!string.IsNullOrEmpty(PWObj.custAccountNumber))
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custAccountNumber";
                            FieldValue.FieldValue = PWObj.custAccountNumber;
                            pwCollection.Add(FieldValue, "custAccountNumber");
                        }
                        if (!string.IsNullOrEmpty(PWObj.custClientNumber))
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custClientNumber";
                            FieldValue.FieldValue = PWObj.custClientNumber;
                            pwCollection.Add(FieldValue, "custClientNumber");
                        }


                        if (Convert.ToInt32(PWObj.custTaxYear) > 0)
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custTaxYear";
                            FieldValue.FieldValue = PWObj.custTaxYear.ToString();
                            pwCollection.Add(FieldValue, "custTaxYear");
                        }
                        if (!string.IsNullOrEmpty(PWObj.custCounty))
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custCounty";
                            FieldValue.FieldValue = PWObj.custCounty;
                            pwCollection.Add(FieldValue, "custCounty");
                        }
                        if (!string.IsNullOrEmpty(PWObj.custDocType))
                        {
                            FieldValue = new PwObjectModel.FieldValue();
                            FieldValue.FieldName = "custDocType";
                            FieldValue.FieldValue = PWObj.custDocType;
                            pwCollection.Add(FieldValue, "custDocType");
                        }


                        FieldValue = new PwObjectModel.FieldValue();
                        FieldValue.FieldName = "custScanDate";
                        FieldValue.FieldValue = Convert.ToDateTime(System.DateTime.Now.ToShortDateString()).ToString("yyyy/MM/dd").Replace("-", "/");
                        pwCollection.Add(FieldValue, "custScanDate");

                // double imageId;

                        //PWObj.MediaPath = ConfigurationManager.AppSettings["ConciergeUploadPath"] + PWObj.MediaPath;
                        PaperwiseInterface obj = new PaperwiseInterface();
                        obj.sendToPaperwise(PWObj.ImagePath, Convert.ToString(PWObj.FileCabinetId), 0, pwCollection, out imageId, out errorMessage);

                        if (imageId > 0)
                        {
                            Hashtable _hashtableImageIDUpdate = null;
                            _hashtableImageIDUpdate = new Hashtable();
                            _hashtableImageIDUpdate.Add("@Userid", PWObj.UserId);
                            _hashtableImageIDUpdate.Add("@AccountId", PWObj.AccountId);
                            _hashtableImageIDUpdate.Add("@Taxyear", PWObj.custTaxYear);
                            _hashtableImageIDUpdate.Add("@DocumentType", PWObj.DocumentType);
                            _hashtableImageIDUpdate.Add("@Imageid", imageId);
                            _hashtableImageIDUpdate.Add("@MediaDetailsid", PWObj.MediaDetailsid);

                    var result = _Connection.Execute(StoredProcedureNames.usp_CG_SavePWImageId,
                                _hashtableImageIDUpdate, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx, true);
                        }

                isSucess = true;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("SendPaperWiseDeferredMaintenancePhoto-API  error " + ex);
                errorMessage = ex.Message;
                isSucess = false;
                throw ex;
            }
            return isSucess;
        }

    }
}
