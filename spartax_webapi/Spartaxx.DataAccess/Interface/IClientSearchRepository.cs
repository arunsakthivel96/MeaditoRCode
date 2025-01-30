using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public interface IClientSearchRepository
    {
        List<PTXboAccountSearchDetails> getSearchedAccountDetails(PTXboClientSearch clientSearch);
        List<PTXboClientSearchDetails> getSearchedClientDetails(PTXboClientSearch clientSearch);
        List<PTXboClientSearchDetails> getCAFSearchedClientDetails(PTXboClientSearch clientSearch);
    }
}
