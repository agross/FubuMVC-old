using System.Web;

namespace FubuMVC.Core.SessionState
{
    public interface IRequestDataProvider
    {
        void SetRequestData(object data);
        object GetRequestData();
    }

    public class RequestDataProvider : IRequestDataProvider
    {
        public const string REQUESTDATA_KEY = "__fubuRequestData";
        private readonly HttpContextBase _httpContext;

        public RequestDataProvider(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public void SetRequestData(object data)
        {
            if(_httpContext.Session == null) return;

            _httpContext.Session.Add(REQUESTDATA_KEY, data);
        }

        public object GetRequestData()
        {
            if(_httpContext.Session == null) return null;

            var requestData =  _httpContext.Session[REQUESTDATA_KEY];

            if (requestData != null)
            {
                // If we got it from Session, remove it so that no other request gets it
                _httpContext.Session.Remove(REQUESTDATA_KEY);
                return requestData;
            }
            
            return null;
        }
    }
}
