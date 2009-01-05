using System.Web;

namespace AltOxite.Core.Config
{
    public interface ICookieHandler
    {
        string UserId { get; }
        bool IsCookieThere { get; }
        ICookieHandler ForHttpRequest(HttpRequest httpRequest);
    }
}