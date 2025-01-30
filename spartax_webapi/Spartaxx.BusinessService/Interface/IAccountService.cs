using Spartaxx.BusinessObjects;
using Spartaxx.Common.BusinessObjects;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public interface IAccountService
    {
        int CreateQuickNewClientValues(PTXboNewClientQuickSetup objNewClientQuickSetup, List<PTXboPropertiesFromTaxRoll> lstPropertiesFromTaxroll);
        List<PTXboReactivationHistory> ReactivationHistory(int AccountId);
    }
}
