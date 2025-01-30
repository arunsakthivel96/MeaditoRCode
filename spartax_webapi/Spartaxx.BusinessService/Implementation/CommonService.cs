using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spartaxx.DataAccess;
using Spartaxx.BusinessObjects;
using Spartaxx.DataObjects;
using System.Dynamic;

namespace Spartaxx.BusinessService
{
    public class CommonService : ICommonService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public CommonService()
        {
            _UnitOfWork = new UnitOfWork();
        }
     

        public List<PTXboPaperwiseDocumentType> getPWDocumentTypeListByFileCabinetID(int fileCabinetID)
        {
            return _UnitOfWork.CommonRepocitory.getPWDocumentTypeListByFileCabinetID(fileCabinetID);            
        }
        public List<PTXboIndexedNoticeDocumentRouteFrom> getIndexedNoticeDocumentRouteFrom(int IndexedNoticeDocumentId)
        {
            return _UnitOfWork.CommonRepocitory.getIndexedNoticeDocumentRouteFrom(IndexedNoticeDocumentId);
        }
        public List<PTXdoCounty> getCountyURL(int CountyId)
        {
            return _UnitOfWork.CommonRepocitory.getCountyURL(CountyId);
        }
    }
}
