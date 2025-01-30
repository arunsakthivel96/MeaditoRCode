using Spartaxx.BusinessObjects;
using Spartaxx.Common.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public interface IAccountRepository
    {
        int CreateQuickNewClientValues(PTXboNewClientQuickSetup objNewClientQuickSetup, List<PTXboPropertiesFromTaxRoll> lstPropertiesFromTaxroll);

        List<PTXboReactivationHistory> ReactivationHistory(int AccountId);
    }
}
