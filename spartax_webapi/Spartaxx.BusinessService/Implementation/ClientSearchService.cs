using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public class ClientSearchService : IClientSearchService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public ClientSearchService()
        {
            _UnitOfWork = new UnitOfWork();
        }

        public List<PTXboAccountSearchDetails> getSearchedAccountDetails(PTXboClientSearch clientSearch)
        {
            return _UnitOfWork.ClientSearchRepository.getSearchedAccountDetails(clientSearch);
        }

        public List<PTXboClientSearchDetails> getSearchedClientDetails(PTXboClientSearch clientSearch)
        {
            return _UnitOfWork.ClientSearchRepository.getSearchedClientDetails(clientSearch);
        }
        public List<PTXboClientSearchDetails> getCAFSearchedClientDetails(PTXboClientSearch clientSearch)
        {
            return _UnitOfWork.ClientSearchRepository.getCAFSearchedClientDetails(clientSearch);
        }
    }
}
