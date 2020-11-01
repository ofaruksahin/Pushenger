using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Reflection;

namespace Pushenger.Core.Utilities
{
    public static class IQueryCollectionMapper
    {
        public static T Map<T>(this IQueryCollection queryCollection)
        {
            try
            {
                T result = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] properties = result.GetType().GetProperties();
                T instance = (T)Activator.CreateInstance(typeof(T));
                foreach (var key in queryCollection.Keys)
                {
                    StringValues queryValue;
                    queryCollection.TryGetValue(key, out queryValue);
                    if (!String.IsNullOrEmpty(queryValue))
                    {
                        foreach (PropertyInfo property in properties)
                        {
                            if (key == property.Name)
                                property.SetValue(instance, queryValue.ToString());
                        }
                    }
                }
                result = instance;
                return result;
            }
            catch (Exception)
            {
                return default(T);   
            }            
        }
    }
}
