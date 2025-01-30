using Spartaxx.BusinessObjects;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public interface ICommonRepocitory
    {
        List<PTXboPaperwiseDocumentType> getPWDocumentTypeListByFileCabinetID(int fileCabinetID);
        List<PTXboIndexedNoticeDocumentRouteFrom> getIndexedNoticeDocumentRouteFrom(int IndexedNoticeDocumentId);
        List<PTXdoCounty> getCountyURL(int CountyId);
    }
}
