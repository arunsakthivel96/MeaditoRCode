using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.Common
{
    public static class Enumerator
    {
        public enum PTXenumClientSearchMode : int
        {
            Client = 1,
            Account = 2
        }

        public enum Enum_ConnectionString
        {
            Spartaxx,
            CSDB,
            CSDBTaxRoll,
        }

        public enum Enum_CommandType
        {
            StoredProcedure,
            InlineQuery,
        }

        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

        public static short GetId(this Enum value)
        {
            return Convert.ToInt16(value);
        }
    }
}
