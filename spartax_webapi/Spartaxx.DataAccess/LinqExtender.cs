using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
   public static class LinqExtender
    {

        /*******************************************************************/
        ////Author                : SaravananS
        ////Description           : Convert Single Object into DataTable
        ////Last Modified on      : 13 May 2020
        /*******************************************************************/

        public static DataTable ToDataTable<T>(this T data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(data);
            }
            table.Rows.Add(values);
            return table;
        }

        /*******************************************************************/
        ////Author                : SaravananS
        ////Description           : Convert Single Object into DataTable
        ////Last Modified on      : 13 May 2020
        /*******************************************************************/
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

    }
}
