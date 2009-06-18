using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FubuMVC.Core.Runtime
{
    public class ConvertProblem
    {
        public object Item { get; set; }
        public PropertyInfo Property { get; set; }
        public object Value { get; set; }
        public Exception Exception { get; set; }

        public override string ToString()
        {
            return
                @"Item type:       {0}
Property:        {1}
Property Type:   {2}
Attempted Value: {3}
Exception:
{4} 
"
                    .ToFormat(
                    ((Item != null) ? Item.GetType().FullName : "(null)"),
                    Property.Name,
                    Property.PropertyType,
                    Value,
                    Exception);
        }
    }

    public interface IDictionaryConverter<T>
        where T : class, new()
    {
        T ConvertFrom(IDictionary<string, object> requestData, out ICollection<ConvertProblem> problems);
        T StrictConvertFrom(IDictionary<string, object> requestData);
    }

    public class DictionaryConverter<T> : IDictionaryConverter<T>
        where T : class, new()
    {
        public T ConvertFrom(IDictionary<string, object> values, out ICollection<ConvertProblem> problems) 
        {
            var item = new T();

            Populate(values, item, out problems);

            return item;
        }

        public void Populate(IDictionary<string, object> values, object item, out ICollection<ConvertProblem> problems)
        {
            problems = new List<ConvertProblem>();

            if (values == null) return;
            
            foreach (var propInfo in TypeDescriptorRegistry.GetPropertiesFor(item.GetType()).Values)
            {
                object value;
                if (values.TryGetValue(propInfo.Name, out value))
                {
                    SetPropFromValue(value, item, propInfo, problems);
                }
            }
        }

       
        private static void SetPropFromValue(object value, object item, PropertyInfo prop, ICollection<ConvertProblem> problems)
        {
            WriteToProperty(item, prop, value, problems);
        }

        private static void WriteToProperty(object item, PropertyInfo prop, object value, ICollection<ConvertProblem> problems)
        {
            try
            {

                if (value != null && !Equals(value, ""))
                {
                    var destType = prop.PropertyType;

                    if( destType == typeof(bool) && Equals(value, prop.Name) )
                    {
                        value = true;
                    }

                    if (prop.PropertyType.IsAssignableFrom(value.GetType()))
                    {
                        prop.SetValue(item, value, null);
                        return;
                    }

                    if (prop.PropertyType.IsNullableOfT())
                    {
                        destType = prop.PropertyType.GetGenericArguments()[0];
                    }

                    prop.SetValue(item, Convert.ChangeType(value, destType), null);
                }
            }
            catch(Exception ex)
            {
                problems.Add(new ConvertProblem{Item = item, Property = prop, Value = value, Exception = ex});
            }
        }

        public T StrictConvertFrom(IDictionary<string, object> requestData)
        {
            ICollection<ConvertProblem> problems;

            var item = ConvertFrom(requestData, out problems);
            
            if (problems.Count > 0)
            {
                var counter = 0;
                var builder = new StringBuilder();
                builder.Append("Could not convert all input values into their expected types:");
                builder.Append(Environment.NewLine);
                foreach( var prob in problems )
                {
                    builder.AppendFormat("-----Problem[{0}]---------------------", counter++);
                    builder.Append(Environment.NewLine);
                    builder.Append(prob);
                    builder.Append(Environment.NewLine);
                }
                throw new InvalidOperationException(builder.ToString());
            }

            return item;
        }
    }
}