using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using Spartaxx.Common;
using static Dapper.SqlMapper;
using Newtonsoft.Json;
using Spartaxx.Utilities.Logging;

namespace Spartaxx.DataAccess
{
    public class DapperConnection
    {
        private IDbConnection _dbConn = null;
        private IDbTransaction _transaction = null;

        private void BeginTransaction()
        {
            _dbConn.Open();
            _transaction = _dbConn.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();

            _transaction.Dispose();
            _dbConn.Close();

            if (_dbConn.State == ConnectionState.Open)
                Logger.For(this).Transaction("dbconnection status " + _dbConn.State);

            _dbConn.Dispose();
            _dbConn = null;
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();

            _transaction.Dispose();
            _dbConn.Close();

            if (_dbConn.State == ConnectionState.Open)
                Logger.For(this).Transaction("dbconnection status " + _dbConn.State);

            _dbConn.Dispose();
            _dbConn = null;
            _transaction = null;
        }

        private IDbConnection GetConnection(Enumerator.Enum_ConnectionString Enum_SqlConnection, bool IsUseTransaction = false)
        {
            IDbConnection _dbConnection = null;
            try
            {
                if (IsUseTransaction)
                {
                    if (_dbConn == null)
                    {
                       // _dbConn = GetConnection(Enum_SqlConnection); //commented by saravanans
                        //added by saravanans
                        if (Enum_SqlConnection.ToString().ToLower() == "spartaxx")
                        {
                            _dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Spartaxx"].ConnectionString);
                        }
                        else if (Enum_SqlConnection.ToString().ToLower() == "csdbtaxroll")
                        {
                            _dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CSDBTaxRoll"].ConnectionString);
                        }
                        //ends here.
                        BeginTransaction();
                    }
                    _dbConnection = _dbConn;
                }
                else
                {
                    if (Enum_SqlConnection.ToString().ToLower() == "spartaxx")
                    {
                        _dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Spartaxx"].ConnectionString);
                    }
                    else if (Enum_SqlConnection.ToString().ToLower() == "csdbtaxroll")
                    {
                        _dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CSDBTaxRoll"].ConnectionString);
                    }
                }
                return _dbConnection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DynamicParameters GetParameters(Hashtable _parameters)
        {
            var DParameters = new DynamicParameters();
            try
            {
                foreach (DictionaryEntry parameter in _parameters)
                {
                    DParameters.Add(Convert.ToString(parameter.Key), parameter.Value);
                }
                return DParameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> Select<T>(string SqlQuery, Hashtable _parameters, Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString)
        {
            IDbConnection db = null;
            List<T> _data;
            try
            {
                db = GetConnection(_ConnectionString);
                var _DParameters = GetParameters(_parameters);
                if (_CommandType == Enumerator.Enum_CommandType.StoredProcedure)
                    _data = db.Query<T>(SqlQuery, _DParameters, commandType: CommandType.StoredProcedure, commandTimeout: 0).ToList();
                else
                    _data = db.Query<T>(SqlQuery, _DParameters, commandTimeout: 0).ToList();
                return _data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Close();

                if (db.State == ConnectionState.Open)
                    Logger.For(this).Transaction("dbconnection status " + db.State);

                db.Dispose();
            }
        }

        public Tuple<IEnumerable<T1>> SelectSingle<T1>(string SqlQuery, Hashtable _parameters,
           Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString,
           Func<GridReader, IEnumerable<T1>> func1)
        {
            var objs = getMultiple(SqlQuery, _parameters, _CommandType, _ConnectionString, func1);
            return Tuple.Create(objs[0] as IEnumerable<T1>);
        }
        public Tuple<IEnumerable<T1>, IEnumerable<T2>> SelectMultiple<T1, T2>(string SqlQuery, Hashtable _parameters,
            Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString,
            Func<GridReader, IEnumerable<T1>> func1, Func<GridReader, IEnumerable<T2>> func2)
        {
            var objs = getMultiple(SqlQuery, _parameters, _CommandType, _ConnectionString, func1, func2);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> SelectMultiple<T1, T2, T3>(string SqlQuery, Hashtable _parameters,
            Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString,
            Func<GridReader, IEnumerable<T1>> func1, Func<GridReader, IEnumerable<T2>> func2, Func<GridReader, IEnumerable<T3>> func3)
        {
            var objs = getMultiple(SqlQuery, _parameters, _CommandType, _ConnectionString, func1, func2, func3);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> SelectMultiple<T1, T2, T3, T4>(string SqlQuery, Hashtable _parameters,
            Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString,
            Func<GridReader, IEnumerable<T1>> func1, Func<GridReader, IEnumerable<T2>> func2, Func<GridReader, IEnumerable<T3>> func3, Func<GridReader, IEnumerable<T4>> func4)
        {
            var objs = getMultiple(SqlQuery, _parameters, _CommandType, _ConnectionString, func1, func2, func3, func4);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>> SelectMultiple<T1, T2, T3, T4, T5>(string SqlQuery, Hashtable _parameters,
            Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString,
            Func<GridReader, IEnumerable<T1>> func1, Func<GridReader, IEnumerable<T2>> func2, Func<GridReader, IEnumerable<T3>> func3, Func<GridReader, IEnumerable<T4>> func4,
            Func<GridReader, IEnumerable<T5>> func5)
        {
            var objs = getMultiple(SqlQuery, _parameters, _CommandType, _ConnectionString, func1, func2, func3, func4, func5);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>, objs[4] as IEnumerable<T5>);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>> SelectMultiple<T1, T2, T3, T4, T5, T6>(string SqlQuery, Hashtable _parameters,
            Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString,
            Func<GridReader, IEnumerable<T1>> func1, Func<GridReader, IEnumerable<T2>> func2, Func<GridReader, IEnumerable<T3>> func3, Func<GridReader, IEnumerable<T4>> func4,
            Func<GridReader, IEnumerable<T5>> func5, Func<GridReader, IEnumerable<T6>> func6)
        {
            var objs = getMultiple(SqlQuery, _parameters, _CommandType, _ConnectionString, func1, func2, func3, func4, func5, func6);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>, objs[4] as IEnumerable<T5>, objs[5] as IEnumerable<T6>);
        }

        private List<object> getMultiple(string SqlQuery, Hashtable _parameters, Enumerator.Enum_CommandType _CommandType,
            Enumerator.Enum_ConnectionString _ConnectionString, params Func<GridReader, object>[] readerFuncs)
        {
            IDbConnection db = null;
            var _data = new List<object>();
            GridReader gridReader = null;
            try
            {
                db = GetConnection(_ConnectionString);
                var _DParameters = GetParameters(_parameters);
                if (_CommandType == Enumerator.Enum_CommandType.StoredProcedure)
                    gridReader = db.QueryMultiple(SqlQuery, _DParameters, commandType: CommandType.StoredProcedure, commandTimeout: 0);
                else
                    gridReader = db.QueryMultiple(SqlQuery, _DParameters, commandTimeout: 0);

                foreach (var readerFunc in readerFuncs)
                {
                    var obj = readerFunc(gridReader);
                    _data.Add(obj);
                }
                return _data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Close();

                if (db.State == ConnectionState.Open)
                    Logger.For(this).Transaction("dbconnection status " + db.State);

                db.Dispose();
                gridReader.Dispose();
            }
        }

        //public dynamic SelectDynamicMultiple(string SqlQuery, Hashtable _parameters, Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString)
        //{
        //    IDbConnection db = null;
        //    List<dynamic> _data = null;
        //    GridReader gridReader = null;
        //    try
        //    {
        //        db = GetConnection(_ConnectionString);
        //        var _DParameters = GetParameters(_parameters);
        //        if (_CommandType == Enumerator.Enum_CommandType.StoredProcedure)
        //            gridReader = db.QueryMultiple(SqlQuery, _DParameters, commandType: CommandType.StoredProcedure, commandTimeout: 0);
        //        else
        //            gridReader = db.QueryMultiple(SqlQuery, _DParameters, commandTimeout: 0);

        //        while (gridReader.IsConsumed == false)
        //        {
        //            var result = gridReader?.ReadAsync<dynamic>().Result; //Working by Mano
        //            JsonConvert.SerializeObject(result, Formatting.Indented);
        //            //_data.Add(result.ToList());
        //        }
        //        return _data;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        db.Close();
        //        db.Dispose();
        //        gridReader.Dispose();
        //    }
        //}


        /// <summary>
        /// Added by SaravananS
        /// Description:Receiving multiple table from db and returning it as a dataset
        /// </summary>
        /// <param name="SqlQuery"></param>
        /// <param name="_parameters"></param>
        /// <param name="_CommandType"></param>
        /// <param name="_ConnectionString"></param>
        /// <returns></returns>
        public DataSet SelectDataSet(string SqlQuery, Hashtable _parameters, Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString)
        {
            IDbConnection db = null;

            DataSet dsResult = new DataSet();
            GridReader gridReader = null;
            try
            {
                db = GetConnection(_ConnectionString);
                var _DParameters = GetParameters(_parameters);

                    if (_CommandType == Enumerator.Enum_CommandType.StoredProcedure)
                        gridReader = db.QueryMultiple(SqlQuery, _DParameters, commandType: CommandType.StoredProcedure, commandTimeout: 0);
                    else
                        gridReader = db.QueryMultiple(SqlQuery, _DParameters, commandTimeout: 0);

                    while (gridReader.IsConsumed == false)
                    {
                        var result = gridReader?.ReadAsync<dynamic>().Result;
                        var json = JsonConvert.SerializeObject(result);
                        DataTable dt = new DataTable();
                        dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                        dsResult.Tables.Add(dt);

                    }
                    return dsResult;


            }
            catch (Exception ex)
            {
                if (ex.Message.ToString() == "No columns were selected")//empty results from db
                {
                    DataTable dt = new DataTable();
                    dsResult.Tables.Add(dt);
                    return dsResult;
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                db.Close();

                if (db.State == ConnectionState.Open)
                    Logger.For(this).Transaction("dbconnection status " + db.State);

                db.Dispose();
                gridReader.Dispose();
            }
        }

        /// <summary>
        /// Added by SaravananS
        /// Description:Receiving multiple table from db and returning it as a dataset
        /// </summary>
        /// <param name="SqlQuery"></param>
        /// <param name="_parameters"></param>
        /// <param name="_CommandType"></param>
        /// <param name="_ConnectionString"></param>
        /// <returns></returns>
        //public DataSet GetDataSet(string SqlQuery, Hashtable _parameters, Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString)
        //{
        //    IDbConnection db = null;

        //    DataSet dsResult = new DataSet();
        //    GridReader gridReader = null;
        //    IDataReader reader;
        //    try
        //    {
        //        db = GetConnection(_ConnectionString);
        //        var _DParameters = GetParameters(_parameters);
        //        if (_CommandType == Enumerator.Enum_CommandType.StoredProcedure)
        //        {
        //            //  gridReader = db.QueryMultiple(SqlQuery, _DParameters, commandType: CommandType.StoredProcedure, commandTimeout: 0);
        //            reader = db.ExecuteReader(SqlQuery, _DParameters, commandType: CommandType.StoredProcedure, commandTimeout: 0);
        //            if (reader.RecordsAffected > 0)
        //            {
        //                // var result = gridReader?.ReadAsync<dynamic>().Result;
        //                //var json = JsonConvert.SerializeObject(result);
        //                DataTable dt = new DataTable();
        //                dt.Load(reader);
        //                //dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
        //                dsResult.Tables.Add(dt);

        //            }
        //        }

        //        else
        //        {
        //            gridReader = db.QueryMultiple(SqlQuery, _DParameters, commandTimeout: 0);

        //            while (!gridReader.IsConsumed)
        //            {

        //                var result = gridReader?.ReadAsync<dynamic>().Result;
        //                var json = JsonConvert.SerializeObject(result);
        //                DataTable dt = new DataTable();

        //                dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
        //                dsResult.Tables.Add(dt);


        //            }
        //            gridReader.Dispose();
        //        }
        //        return dsResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        db.Close();
        //        db.Dispose();
        //        //gridReader.Dispose();
        //    }
        //}

        public int Execute(string SqlQuery, Hashtable _parameters, Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString, bool IsManual_RollBack_Commit_Transaction = false)
        {
            int RowsAffected = 0;
            try
            {
                _dbConn = GetConnection(_ConnectionString, IsManual_RollBack_Commit_Transaction);
                var _DParameters = GetParameters(_parameters);
                if (_CommandType == Enumerator.Enum_CommandType.StoredProcedure)
                    RowsAffected = _dbConn.Execute(SqlQuery, _DParameters, commandType: CommandType.StoredProcedure, transaction: _transaction, commandTimeout: 0);
                else
                    RowsAffected = _dbConn.Execute(SqlQuery, _DParameters, transaction: _transaction, commandTimeout: 0);
                return RowsAffected;
            }
            catch (Exception ex)
            {
                if (IsManual_RollBack_Commit_Transaction)
                {
                    RollbackTransaction();
                }
                throw ex;
            }
            finally
            {
                if (IsManual_RollBack_Commit_Transaction)
                {
                    CommitTransaction();
                }
            }
        }

        public string ExecuteScalar(string SqlQuery, Hashtable _parameters, Enumerator.Enum_CommandType _CommandType, Enumerator.Enum_ConnectionString _ConnectionString)
        {
            IDbConnection db = null;
            string strValue = string.Empty;
            try
            {
                db = GetConnection(_ConnectionString);
                var _DParameters = GetParameters(_parameters);
                if (_CommandType == Enumerator.Enum_CommandType.StoredProcedure)
                {
                    var result = db.ExecuteScalar(SqlQuery, _DParameters, commandType: CommandType.StoredProcedure, commandTimeout: 0);
                    strValue = (result == null )? string.Empty : result.ToString();
                }
                    
                else
                {
                    var result =  db.ExecuteScalar(SqlQuery, _DParameters, commandTimeout: 0).ToString();
                    strValue = result == null ? string.Empty : result.ToString();
                }
                   
                return strValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Close();

                if (db.State == ConnectionState.Open)
                    Logger.For(this).Transaction("dbconnection status " + db.State);

                db.Dispose();
            }
        }
    }
}
