using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using OpinionatedMVC.Core.Util;

namespace OpinionatedMVC.Core
{
    public static class BasicExtensions
    {
        public static bool IsNotEmpty(this string stringValue)
        {
            return !string.IsNullOrEmpty(stringValue);
        }

        public static bool ToBool(this string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return false;

            return bool.Parse(stringValue);
        }

        public static VALUE Get<KEY, VALUE>(this IDictionary<KEY, VALUE> dictionary, KEY key, VALUE defaultValue)
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            return defaultValue;
        }

        public static string GetViewModelProperty<VIEWMODEL>(this IDictionary<string, object> dictionary, Expression<Func<VIEWMODEL, object>> expression)
        {
            string key = ReflectionHelper.GetProperty(expression).Name;
            if (dictionary.ContainsKey(key)) return dictionary[key].ToString();
            return string.Empty;
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> values, Action<T> eachAction)
        {
            foreach( var item in values )
            {
                eachAction(item);
            }

            return values;
        }

        public static IEnumerable Each(this IEnumerable values, Action<object> eachAction)
        {
            foreach (var item in values)
            {
                eachAction(item);
            }

            return values;
        }

        public static bool IsNullable(this Type theType)
        {
            return (!theType.IsValueType) || theType.IsNullableOfT();
        }

        public static bool IsNullableOfT(this Type theType)
        {
            return theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static bool IsNullableOf(this Type theType, Type otherType)
        {
            return theType.IsNullableOfT() && theType.GetGenericArguments()[0].Equals(otherType);
        }

        public static IList<T> AddMany<T>(this IList<T> list, params T[] items)
        {
            return list.AddRange(items);
        }

        public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            items.Each(t => list.Add(t));
            return list;
        }

        public static U ValueOrDefault<T, U>( this T root, Expression<Func<T, U>> expression)
            where T : class
        {
            if( root == null )
            {
                return default(U);
            }

            var accessor = ReflectionHelper.GetAccessor(expression);

            object result = accessor.GetValue(root);

            return (U) (result ?? default(U));
        }
    }
}