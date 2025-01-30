using Spartaxx.BusinessObjects;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public interface IClientSearchService
    {
        List<PTXboAccountSearchDetails> getSearchedAccountDetails(PTXboClientSearch clientSearch);
        List<PTXboClientSearchDetails> getSearchedClientDetails(PTXboClientSearch clientSearch);
        List<PTXboClientSearchDetails> getCAFSearchedClientDetails(PTXboClientSearch clientSearch);
    }
}
