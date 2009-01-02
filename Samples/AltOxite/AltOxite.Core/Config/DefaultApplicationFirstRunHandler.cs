using System;
using System.Collections.Generic;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;

namespace AltOxite.Core.Config
{
    public class DefaultApplicationFirstRunHandler : IApplicationFirstRunHandler
    {
        private readonly IRepository _repository;
        private readonly ISessionSourceConfiguration _sourceConfig;
        private readonly IUnitOfWork _unitOfWork;
        private static bool _isInitialized;

        public DefaultApplicationFirstRunHandler(ISessionSourceConfiguration sourceConfig, IUnitOfWork unitOfWork, IRepository repository)
        {
            _sourceConfig = sourceConfig;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        public void InitializeIfNecessary()
        {
            if (!_sourceConfig.IsNewDatabase || IsInitialized) return;

            var user = setup_admin_user();
            setup_sample_post(user);
            _unitOfWork.Commit();

            IsInitialized = true;
        }

        private User setup_admin_user()
        {
            var defaultUser = new User
                {
                    Username = "Admin",
                    Password = "pa$$w0rd",
                    DisplayName = "Oxite Administrator",
                    HashedEmail = "01d418308faffa0d07f34ace68b686ad"
                };

            _repository.Save(defaultUser);
            return defaultUser;
        }

        private void setup_sample_post(User user)
        {
            var oxiteTag = new Tag {Name = "Oxite"};

            var defaultPost = new Post
            {
                Title = "World.Hello()",
                Slug = "World_Hello",
                BodyShort = "Welcome to Oxite! &nbsp;This is a sample application targeting developers built on <a href=\"http://asp.net/mvc\">ASP.NET MVC</a>. &nbsp;Make any changes you like. &nbsp;If you build a feature you think other developers would be interested in and would like to share your code go to the <a href=\"http://www.codeplex.com/oxite\">Oxite Code Plex project</a> to see how you can contribute.<br /><br />To get started, sign in with \"Admin\" and \"pa$$w0rd\" and click on the Admin tab.<br /><br />For more information about <a href=\"http://oxite.net\">Oxite</a> visit the default <a href=\"/About\">About</a> page.",
                Body = "body text",
                Published = DateTime.Parse("2008-12-05 09:29:03.270"),
                Tags = new List<Tag> {oxiteTag},
                Comments = new List<Comment>(),
                User = user
            };

            _repository.Save(defaultPost);

            var defaultPost1 = new Post
            {
                Title = "World.Hello()",
                Slug = "World_Hello2",
                BodyShort = "Welcome to Oxite! &nbsp;This is a sample application targeting developers built on <a href=\"http://asp.net/mvc\">ASP.NET MVC</a>. &nbsp;Make any changes you like. &nbsp;If you build a feature you think other developers would be interested in and would like to share your code go to the <a href=\"http://www.codeplex.com/oxite\">Oxite Code Plex project</a> to see how you can contribute.<br /><br />To get started, sign in with \"Admin\" and \"pa$$w0rd\" and click on the Admin tab.<br /><br />For more information about <a href=\"http://oxite.net\">Oxite</a> visit the default <a href=\"/About\">About</a> page.",
                Body = "body text",
                Published = DateTime.Parse("2008-12-05 09:29:03.270"),
                Tags = new List<Tag> { oxiteTag, new Tag { Name = "AltOxite" } },
                Comments = new List<Comment>{ new Comment() }, 
                User = user
            };
            _repository.Save(defaultPost1);
        }
    }
}