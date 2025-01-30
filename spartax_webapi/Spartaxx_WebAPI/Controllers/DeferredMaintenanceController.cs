using Spartaxx.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Spartaxx.Common.BusinessObjects;
using Spartaxx.BusinessObjects.Concierge;
using Spartaxx.BusinessObjects;
using Spartaxx.Common.Reports;
using Spartaxx.Utilities.PDFViewer;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PwObjectModel;

namespace Spartaxx_WebAPI.Controllers
{
    public class BoReadFromPaperwise
    {
        public string DocType { get; set; }
        public string filePath { get; set; }
        public string fileCabinetId { get; set; }
        public int imageId { get; set; }
    }

    public class DeferredMaintenanceController : ApiController
    {
        private readonly IDeferredMaintenanceService _iDeferredMaintenanceService;
        string PwImagePath = System.Configuration.ConfigurationManager.AppSettings["PwImagePath"];

        [Route("api/DeferredMaintenance/ReadFromPaperwise")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReadFromPaperwise([FromBody]BoReadFromPaperwise BoReadFromPaperwise)
        {
            var errorList = "";

            try
            {
                CPReportData objCPReportData = new CPReportData();

                List<string> storedFilePaths = new List<string>();
                string errormessage = "";

                var IsSucess = readFromPaperwise(BoReadFromPaperwise.DocType, BoReadFromPaperwise.filePath, BoReadFromPaperwise.fileCabinetId, BoReadFromPaperwise.imageId, out storedFilePaths, out errormessage);

                return Request.CreateResponse(HttpStatusCode.OK, storedFilePaths);
            }
            catch (Exception ex)
            {
                errorList = ex.Message;

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        public bool readFromPaperwise(string DocType, string filePath, string fileCabinetId, int imageId, out List<string> storedFilePaths, out string errorMessage)
        {
            errorMessage = string.Empty;
            storedFilePaths = new List<string>();
            StorageAPI storage = new StorageAPI();
            Connection connection = new Connection();
            var storedFilePath = string.Empty;
            Object ImageObj;

            string tempFileName = string.Empty;

            bool isSuccess = false;

            try
            {
                var destPath = filePath;
                var physicalPath = Path.Combine(filePath, imageId.ToString());

                filePath = physicalPath;
                string Server1 = "inpwsqlserver";
                string Database = "paperwise";
                string Password = "spartaxx";
                string UserID = "spartaxx";
                int ApplicationID = 555;

                connection.Logon(Server1, Database, UserID, Password, ApplicationID);

                ImageObj = storage.GetImageInfoByID(connection, fileCabinetId, imageId);

                string fileName = ((ImageInfo)ImageObj).Filename;
                string fileExtension = fileName.Substring(fileName.LastIndexOf(".")).ToLower();
                string annotatedFile = string.Empty;

                if (fileExtension.Contains(".tif"))
                {
                    fileExtension = fileExtension.Replace(".tif", ".jpg");
                }

                storedFilePath = filePath + fileExtension;

                if (fileExtension.Contains(".pdf"))
                {
                    var thumbnailPath = "";
                    var compressedPath = "";
                    var isCompressed = false;

                    if (!System.IO.File.Exists(storedFilePath))
                        storage.WriteImageDataToFile(connection, fileCabinetId, ImageObj, storedFilePath, PwOverlayType.PwOverlayOff);

                    var pageCount = AFPDFLibUtil.GetPDFPageCount(storedFilePath);

                    if (DocType == "Client Notes")
                        pageCount = 1;

                    for (int i = 1; i <= pageCount; i++)
                    {
                        if (!System.IO.File.Exists(PwImagePath + imageId + "_" + i + ".jpg"))
                        {
                            parameterHash parameterHash = new parameterHash();
                            parameterHash.CurrentPageNumber = i;
                            thumbnailPath = ASPPDFLib.GetPageFromPDF(storedFilePath, destPath, ref parameterHash);

                            if (pageCount == 1)
                                compressedPath = PwImagePath + imageId + ".jpg";
                            else
                                compressedPath = PwImagePath + imageId + "_" + i + ".jpg";

                            isCompressed = CompressImage(thumbnailPath, compressedPath, out errorMessage);

                            if (!isCompressed)
                            {
                                System.IO.File.Copy(thumbnailPath, PwImagePath + imageId + "_" + i + ".jpg");
                            }

                            if (System.IO.File.Exists(thumbnailPath))
                            {
                                try
                                {
                                    System.IO.File.Delete(thumbnailPath);
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            //storedFilePath = PwImagePath + imageId + ".jpg";

                            storedFilePaths.Add(compressedPath);
                        }
                        else
                        {
                            storedFilePaths.Add(PwImagePath + imageId + "_" + i + ".jpg");
                        }
                    }

                    if (System.IO.File.Exists(storedFilePath))
                    {
                        try
                        {
                            System.IO.File.Delete(storedFilePath);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                else
                {
                    if (!System.IO.File.Exists(PwImagePath + imageId + ".jpg"))
                    {
                        var uncompressedPath = filePath + "_uncompressed" + fileExtension;

                        storage.WriteImageDataToFile(connection, fileCabinetId, ImageObj, uncompressedPath, PwOverlayType.PwOverlayOff);

                        var compressedPath = PwImagePath + imageId + ".jpg";

                        var isCompressed = CompressImage(uncompressedPath, compressedPath, out errorMessage);

                        if (!isCompressed)
                        {
                            System.IO.File.Copy(uncompressedPath, PwImagePath + imageId + ".jpg");
                        }

                        if (System.IO.File.Exists(uncompressedPath))
                        {
                            try
                            {
                                System.IO.File.Delete(uncompressedPath);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    storedFilePaths.Add(PwImagePath + imageId + ".jpg");
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Log.txt"), System.Environment.NewLine + DateTime.Now + ": " + ex.StackTrace);
            }
            finally
            {
                if (connection.ConnectState != PwConnectState.PwConnClose)
                {
                    connection.Logoff();
                }
            }

            return isSuccess;
        }
        public bool CompressImage(string SourceFilePath, string DestFilePath, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                string extn = Path.GetExtension(SourceFilePath).ToLower();

                ImageFormat _imgFormat = ImageFormat.Jpeg;

                using (Bitmap bmp1 = new Bitmap(SourceFilePath))
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(_imgFormat);

                    System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;

                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, 50L);

                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp1.Save(DestFilePath, jpgEncoder, myEncoderParameters);
                    bmp1.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Log.txt"), System.Environment.NewLine + DateTime.Now + ": " + ex.Message);

                ErrorMessage = ex.Message;
                return false;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        public DeferredMaintenanceController(IDeferredMaintenanceService IDeferredMaintenanceServiced)
        {
            _iDeferredMaintenanceService = IDeferredMaintenanceServiced;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SendPaperWiseDeferredMaintenancePhoto([FromBody]PTXdtDeferredMaintenanceImageUpload PWObj)
        {
            dynamic result;
            try
            {
                string errorMessage = "";
                result = _iDeferredMaintenanceService.SendPaperWiseDeferredMaintenancePhoto(PWObj,out errorMessage);
                string jsonvalue = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> DMFinalvalueUpdate([FromBody]PTXdtPropertydetailsUpdateParam PWPropertydetail)
        {
            dynamic result;
            try
            {
                string errorMessage = "";
                result = _iDeferredMaintenanceService.DMFinalvalueUpdate(PWPropertydetail, out errorMessage);
                string jsonvalue = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> DMConciergeLinkGenerate([FromBody]PTXdtConciergeLinkGenerateParam PWPropertydetail)
        {
            
            try
            {
                string errorMessage = "";
                var result = await Task.FromResult(_iDeferredMaintenanceService.DMConciergeLinkGenerate(PWPropertydetail, out errorMessage));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SendPaperWiseConcierge([FromBody]PTXboSentToPaperwiseConcierge PWObj)
        {
            dynamic result;
            try
            {
                string errorMessage = "";
                result = _iDeferredMaintenanceService.SendPaperWiseConcierge(PWObj, out errorMessage);
                string jsonvalue = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

    }
}
