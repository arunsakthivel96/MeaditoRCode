using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Concierge;
using Spartaxx.Common.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public interface IDeferredMaintenanceService
    {
        #region Paperwise Image Push
        bool SendPaperWiseDeferredMaintenancePhoto(PTXdtDeferredMaintenanceImageUpload PWObj, out string errorMessage);
        #endregion

        #region DM Propertydetails Update
        bool DMFinalvalueUpdate(PTXdtPropertydetailsUpdateParam PWPropertydetail, out string errorMessage);
        #endregion

        #region DM Concierge Link Generate
        PTXdtConciergeLinkGenerateParam DMConciergeLinkGenerate(PTXdtConciergeLinkGenerateParam PWPropertydetail, out string errorMessage);
        #endregion

        #region Concierge
        bool SendPaperWiseConcierge(PTXboSentToPaperwiseConcierge PWObj, out string errorMessage);
        #endregion
    }
}
