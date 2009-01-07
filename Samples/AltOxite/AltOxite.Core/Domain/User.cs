using System.Collections.Generic;
using System.Linq;

namespace AltOxite.Core.Domain
{
    public class User : DomainEntity
    {
        public virtual IList<Post> _posts { get; set; } // TODO: Make private

        public virtual string Username { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string HashedEmail { get; set; }
        public virtual string Email { get; set; }
        public virtual string Url { get; set; }
        public virtual string Password { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual int Status { get; set; }
        public virtual bool Remember { get; set; }
        public virtual bool IsAuthenticated { get; set; }
        public virtual UserRoles UserRole { get; set; }

        public virtual void AddPost(Post post)
        {
            _posts.Add(post);
        }
        public virtual void RemovePost(Post post)
        {
            _posts.Remove(post);
        }
        public virtual IEnumerable<Post> GetPosts()
        {
            return _posts.AsEnumerable();
        }

        // public virtual Language LanguageDefault{ get; set; }
        // public IEnumerable<Language> GetLanguages();
    }

    public enum UserRoles
    {
        SiteUser,
        Visitor,
        NotAuthenticated,
    }
}