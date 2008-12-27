using System;
using System.Collections.Generic;
using System.Reflection;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Controller
{
    public static class TypeDescriptorRegistry
    {
        private static readonly Cache<Type, IDictionary<string, PropertyInfo>> _cache;

        static TypeDescriptorRegistry()
        {
            _cache = new Cache<Type, IDictionary<string, PropertyInfo>>(type => new Dictionary<string, PropertyInfo>());
        }

        public static IDictionary<string, PropertyInfo> GetPropertiesFor<TYPE>()
        {
            return GetPropertiesFor(typeof (TYPE));
        }

        public static IDictionary<string, PropertyInfo> GetPropertiesFor(Type itemType)
        {
            IDictionary<string, PropertyInfo> map = _cache.Retrieve(itemType);

            if( map.Count == 0 )
            {
                foreach (var propertyInfo in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance ))
                {
                    map.Add(propertyInfo.Name, propertyInfo);
                }
            }

            return map;
        }

        public static void ClearAll()
        {
            _cache.ClearAll();
        }
    }
}