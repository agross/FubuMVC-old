using System.Collections.Generic;

namespace AltOxite.Core.Domain
{
    public class User : DomainEntity
    {
        public virtual string Username { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string HashedEmail { get; set; }
        public virtual string Password { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual int Status { get; set; }
        public virtual bool IsAnonymous { get; set; }
        public virtual IList<Post> Posts { get; set; }

        // public virtual Language LanguageDefault{ get; set; }
        // public IEnumerable<Language> GetLanguages();
    }
}