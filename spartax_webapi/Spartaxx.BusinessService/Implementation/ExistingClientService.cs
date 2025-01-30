using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public class ExistingClientService : IExistingClientService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public ExistingClientService()
        {
            _UnitOfWork = new UnitOfWork();
        }

        public DataSet getClientCodesDetails(int ClientID)
        {
            return _UnitOfWork.ExistingClientRepository.getClientCodesDetails(ClientID);
        }

        public PTXboSaveAddressContactAndPhoneResult SaveAddressContactAndPhone(int AddressId, int AddressTypeId, int GroupID = 0)
        {
            return _UnitOfWork.ExistingClientRepository.SaveAddressContactAndPhone(AddressId, AddressTypeId, GroupID);
        }

        public List<PTXboAccountDetails> getAccountSummary(int ClientID, int UserID)
        {
            return _UnitOfWork.ExistingClientRepository.getAccountSummary(ClientID, UserID);
        }
    }
}
