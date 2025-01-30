using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Concierge;
using Spartaxx.Common.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public class DeferredMaintenanceService: IDeferredMaintenanceService
    {

        private readonly UnitOfWork _unitOfWork = null;
        public DeferredMaintenanceService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool  SendPaperWiseDeferredMaintenancePhoto(PTXdtDeferredMaintenanceImageUpload PWObj,  out string errorMessage)
        {
            return _unitOfWork.DeferredRepository.SendPaperWiseDeferredMaintenancePhoto(PWObj, out errorMessage);
        }

        public bool DMFinalvalueUpdate(PTXdtPropertydetailsUpdateParam PWPropertydetail, out string errorMessage)
        {
            return _unitOfWork.DeferredRepository.DMFinalvalueUpdate(PWPropertydetail, out errorMessage);
        }

        public PTXdtConciergeLinkGenerateParam DMConciergeLinkGenerate(PTXdtConciergeLinkGenerateParam PWPropertydetail, out string errorMessage)
        {
            return _unitOfWork.DeferredRepository.DMConciergeLinkGenerate(PWPropertydetail, out errorMessage);
        }
        public bool SendPaperWiseConcierge(PTXboSentToPaperwiseConcierge PWObj, out string errorMessage)
        {
            return _unitOfWork.DeferredRepository.SendPaperWiseConcierge(PWObj, out errorMessage);
        }

    }
}
