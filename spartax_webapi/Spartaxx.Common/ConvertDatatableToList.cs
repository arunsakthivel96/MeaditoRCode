using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common
{

    public static class ConvertDatableToList
    {
        /* Soruce: CodeProject. Link: http://www.codeproject.com/Tips/195889/Convert-Datatable-to-Collection-using-Generic */

        public static List<T> ToCollection<T>(this DataTable dt)
        {
            List<T> lst = new System.Collections.Generic.List<T>();
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(tClass);
                foreach (PropertyInfo pc in pClass)
                {
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName.ToLower().Equals(pc.Name.ToLower())); /* All the field names in the dataobject class should be same as the column name in select query. */
                        if (d != null)
                        {
                            object value = item[pc.Name];
                            if (value == System.DBNull.Value)
                            {
                                value = null;
                            }

                            pc.SetValue(cn, value, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                lst.Add(cn);
            }
            return lst;
        }
    }
}
