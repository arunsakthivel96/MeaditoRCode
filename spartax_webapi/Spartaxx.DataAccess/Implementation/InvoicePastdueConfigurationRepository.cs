using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.DataObjects;
using Spartaxx.Utilities.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public class InvoicePastdueConfigurationRepository:IInvoicePastdueConfigurationRepository
    {
        private readonly DapperConnection _dapperConnection;

        public InvoicePastdueConfigurationRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
        }


        public List<PTXboPastdueRunningStatus> GetPastdueRunningStatus()
        {
            try
            {
                Logger.For(this).Invoice("GetPastdueRunningStatus-API  reached " );
                Hashtable parameters = new Hashtable();

                var result = _dapperConnection.Select<PTXboPastdueRunningStatus>(StoredProcedureNames.usp_getPastdueRunningStatus, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetPastdueRunningStatus-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetPastdueRunningStatus-API  error " + ex);
                throw ex;
            }
        }

        public List<PTXboPastDueGenerationHistory> GetPastDueHistory()
        {
            try
            {
                Logger.For(this).Invoice("GetPastDueHistory-API  reached " );
                Hashtable parameters = new Hashtable();

                var result = _dapperConnection.Select<PTXboPastDueGenerationHistory>(StoredProcedureNames.usp_getPastdueHistory, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetPastDueHistory-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetPastDueHistory-API  error " + ex);
                throw ex;
            }
        }
    }
}
