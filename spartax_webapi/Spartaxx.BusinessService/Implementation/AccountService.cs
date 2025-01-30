using Spartaxx.BusinessObjects;
using Spartaxx.Common.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public class AccountService : IAccountService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public AccountService()
        {
            _UnitOfWork = new UnitOfWork();
        }

        public int CreateQuickNewClientValues(PTXboNewClientQuickSetup objNewClientQuickSetup, List<PTXboPropertiesFromTaxRoll> lstPropertiesFromTaxroll)
        {
            return _UnitOfWork.AccountRepository.CreateQuickNewClientValues(objNewClientQuickSetup, lstPropertiesFromTaxroll);
        }
        public List<PTXboReactivationHistory> ReactivationHistory(int AccountId)
        {
            return _UnitOfWork.AccountRepository.ReactivationHistory(AccountId);
        }
    }
}
