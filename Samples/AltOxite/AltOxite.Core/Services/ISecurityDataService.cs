using System;

namespace AltOxite.Core.Services
{
    public interface ISecurityDataService
    {
        Guid? AuthenticateForUserId(string username, string password);
    }
}