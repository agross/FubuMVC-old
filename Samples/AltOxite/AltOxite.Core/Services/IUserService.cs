using AltOxite.Core.Domain;

namespace AltOxite.Core.Services
{
    public interface IUserService 
    {
        User AddOrUpdateUser(string userEmail, string userDisplayName, string userUrl);
    }
}