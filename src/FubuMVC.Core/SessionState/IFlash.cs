using System.Web;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.SessionState
{
    public interface IFlash
    {
        void Flash(object flashObject);
        T Retrieve<T>();
    }

    public class FlashProvider :IFlash
    {
        private HttpSessionStateBase _session;
        public const string FLASH_KEY = "fubuFlash";

        public HttpSessionStateBase Session
        {
            get { return _session ?? (_session = new HttpSessionStateWrapper(HttpContext.Current.Session)); }
            set { _session = value; }
        }

        public void Flash(object flashObject)
        {
            var json = JsonUtil.ToJson(flashObject);
            Session[FLASH_KEY] = json;
        }

        public T Retrieve<T>()
        {
            var json = Session[FLASH_KEY] as string;
            Session.Remove(FLASH_KEY);

            return (json != null)
                ? JsonUtil.Get<T>(json)
                : default(T);
        }
    }
}