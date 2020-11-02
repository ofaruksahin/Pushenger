using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Reflection;

namespace Pushenger.Core.Utilities
{
    public static class IHeaderDictionaryMapper
    {
        public static T Map<T>(this IHeaderDictionary headers)
        {
            try
            {
                T result = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] properties = result.GetType().GetProperties();
                foreach (var key in headers.Keys)
                {
                    StringValues queryValue;
                    headers.TryGetValue(key, out queryValue);
                    if (!String.IsNullOrEmpty(queryValue))
                    {
                        foreach (PropertyInfo property in properties)
                        {
                            if (key == property.Name)
                                property.SetValue(result, queryValue.ToString());
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
