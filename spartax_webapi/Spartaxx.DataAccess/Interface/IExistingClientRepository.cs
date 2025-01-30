using Spartaxx.BusinessObjects;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public interface IExistingClientRepository
    {
        DataSet getClientCodesDetails(int ClientID);
        PTXboSaveAddressContactAndPhoneResult SaveAddressContactAndPhone(int AddressId, int AddressTypeId, int GroupID = 0);
        List<PTXboAccountDetails> getAccountSummary(int ClientID, int UserID);
    }
}
