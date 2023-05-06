using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharedUtilities.Utilties
{
    public static class CollectionUtilities
    {

        /// <summary>
        /// List members and their values from a dictionary
        /// </summary>
        /// <param name="atype"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionaryProperties(this object atype, BindingFlags flags)
        {
            if (atype == null) return new Dictionary<string, object>();
            var t = atype.GetType();
            var props = t.GetProperties(flags);
            var dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }
    }
}
