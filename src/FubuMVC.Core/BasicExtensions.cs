using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FubuMVC.Core.Html;
using FubuMVC.Core.Util;
using FubuMVC.Core.View;

namespace FubuMVC.Core
{
    public static class BasicExtensions
    {
        public static bool IsEmpty(this string stringValue)
        {
            return string.IsNullOrEmpty(stringValue);
        }

        public static bool IsNotEmpty(this string stringValue)
        {
            return !string.IsNullOrEmpty(stringValue);
        }

        public static bool ToBool(this string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return false;

            return bool.Parse(stringValue);
        }

        public static string ToFormat(this string stringFormat, params object[] args)
        {
            return String.Format(stringFormat, args);
        }

        public static string If<MODEL>(this string html, MODEL model, Expression<Func<MODEL, bool>> modelBooleanValue)
            where MODEL : class
        {
            var prop = modelBooleanValue.Body as MemberExpression;
            if (prop != null)
            {
                var info = prop.Member as PropertyInfo;
                if (info != null)
                {
                    return modelBooleanValue.Compile().Invoke(model) ? html : string.Empty;
                }
            }
            throw new ArgumentException("The modelBooleanValue parameter should be a single property from type '{0}', validation logic is not allowed, only 'x => x.BooleanValue' usage is allowed".ToFormat(typeof(MODEL), modelBooleanValue.ToString()));
        }

        public static string ToFullUrl(this string relativeUrl, params object[] args)
        {
            var formattedUrl = (args == null) ? relativeUrl : relativeUrl.ToFormat(args);

            return UrlContext.GetFullUrl(formattedUrl);
        }

        public static VALUE Get<KEY, VALUE>(this IDictionary<KEY, VALUE> dictionary, KEY key)
        {
            return dictionary.Get(key, default(VALUE));
        }

        public static VALUE Get<KEY, VALUE>(this IDictionary<KEY, VALUE> dictionary, KEY key, VALUE defaultValue)
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            return defaultValue;
        }

        // TODO: Not used and seems not wanted anyway
        //public static string GetViewModelProperty<VIEWMODEL>(this IDictionary<string, object> dictionary, Expression<Func<VIEWMODEL, object>> expression)
        //{
        //    string key = ReflectionHelper.GetProperty(expression).Name;
        //    if (dictionary.ContainsKey(key)) return dictionary[key].ToString();
        //    return string.Empty;
        //}

        public static bool Exists<T>(this IEnumerable<T> values, Func<T, bool> evaluator)
        {
            return values.Count(evaluator) > 0;
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Each<T>(this IEnumerable<T> values, Action<T> eachAction)
        {
            foreach( var item in values )
            {
                eachAction(item);
            }

            return values;
        }

        [DebuggerStepThrough]
        public static IEnumerable Each(this IEnumerable values, Action<object> eachAction)
        {
            foreach (var item in values)
            {
                eachAction(item);
            }

            return values;
        }

        [DebuggerStepThrough]
        public static int IterateFromZero(this int maxCount, Action<int> eachAction)
        {
            for( var idx = 0; idx < maxCount; idx++ )
            {
                eachAction(idx);
            }

            return maxCount;
        }

        public static bool HasCustomAttribute<ATTRIBUTE>(this MemberInfo member)
            where ATTRIBUTE : Attribute
        {
            return member.GetCustomAttributes(typeof (ATTRIBUTE), false).Any();
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