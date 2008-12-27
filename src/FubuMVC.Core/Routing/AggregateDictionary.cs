using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace FubuMVC.Core.Routing
{
    public class AggregateDictionary : IDictionary<string, object>
    {
        private readonly IList<Func<string, object>> _locators = new List<Func<string, object>>();
        private readonly HashSet<string> _postedFileKeys;

        public AggregateDictionary()
        {
        }

        public AggregateDictionary(HttpRequestBase request, RouteData routeData)
        {
            AddLocator(key => request[key]);
            _postedFileKeys = new HashSet<string>(request.Files.Cast<string>());
            if (_postedFileKeys.Count > 0)
            {
                AddLocator(key => _postedFileKeys.Contains(key) ? request.Files[key] : null);
            }
            AddLocator(key => { object found; routeData.Values.TryGetValue(key, out found); return found; });
        }

        public void AddLocator(Func<string, object> locator)
        {
            _locators.Add(locator);
        }

        public bool TryGetValue(string key, out object value)
        {
            value = null;

            foreach (var locator in _locators)
            {
                value = locator(key);

                if (value != null)
                {
                    return true;
                }
            }

            return false;
        }

        public object this[string key]
        {
            get
            {
                object value;
                TryGetValue(key, out value);
                return value;
            }
            set { throw new NotImplementedException(); }
        }

        #region ICollection<KeyValuePair<string,object>> Members

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<string, object>>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IDictionary<string,object> Members

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            object returnValue;
            return TryGetValue(key, out returnValue);
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)this).GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> Members

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}