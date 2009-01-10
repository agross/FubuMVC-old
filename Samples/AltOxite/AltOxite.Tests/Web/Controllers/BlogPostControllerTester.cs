using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using AltOxite.Core.Web.Controllers;
using FubuMVC.Core.Controller.Config;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace AltOxite.Tests.Web.Controllers
{
    [TestFixture]
    public class BlogPostControllerTester
    {
        private IRepository _repository;
        private BlogPostController _controller;
        private IList<Post> _posts;
        private IList<User> _users;
        private IUrlResolver _resolver;
        private Post _post;
        private string _testSlug;

        [SetUp]
        public void SetUp()
        {
            _posts = new List<Post>();
            _users = new List<User>();
            _repository = MockRepository.GenerateStub<IRepository>();
            _resolver = MockRepository.GenerateStub<IUrlResolver>();
            _controller = new BlogPostController(_repository, _resolver);

            _testSlug = "TESTSLUG";

            _post = new Post { Slug = _testSlug };
            _posts.Add(_post);
        }

        [Test]
        public void index_should_do_nothing_but_just_render_the_view_if_nothing_was_supplied()
        {
            var output = _controller.Index(new BlogPostViewModel());
            
            output.Post.ShouldBeNull();
            _repository.AssertWasNotCalled(r=>r.Query<Post>());
        }

        [Test]
        public void should_do_nothing_repository_returns_no_results_for_query()
        {
            _repository.Stub(r => r.Query<Post>(null)).IgnoreArguments().Return(new Post[0].AsQueryable());

            var output = _controller.Index(new BlogPostViewModel {Slug = "badslug"});

            output.Post.ShouldBeNull();
        }
    }

    [TestFixture]
    public class BlogPostController_when_an_anonymous_user_adds_a_comment
    {
        private IRepository _repository;
        private BlogPostController _controller;
        private string _testSlug;
        private BlogPostCommentViewModel _validInput;
        private BlogPostCommentViewModel _invalidInput;
        private IList<Post> _posts;
        private IList<User> _users;
        private IUrlResolver _resolver;
        private Post _post;

        [SetUp]
        public void SetUp()
        {
            _posts = new List<Post>();
            _users = new List<User>();
            _repository = MockRepository.GenerateStub<IRepository>();
            _resolver = MockRepository.GenerateStub<IUrlResolver>();
            _controller = new BlogPostController(_repository, _resolver);
            
            _post = new Post { Slug = _testSlug };
            _posts.Add(_post);

            _repository
               .Stub(r => r.Query<Post>(null))
               .IgnoreArguments()
               .Return(_posts.AsQueryable());

            _repository
               .Stub(r => r.Query<User>(null))
               .IgnoreArguments()
               .Return(_users.AsQueryable());

            _testSlug = "TESTSLUG";

            _invalidInput = new BlogPostCommentViewModel {Slug = _testSlug};
            
            _validInput = new BlogPostCommentViewModel
            {
                UserDisplayName = "username",
                UserEmail = "email",
                Body = "body",
                UserSubscribed = true,
                Slug = _testSlug
            };
        }

        [Test]
        public void should_return_validation_error_if_name_not_specified()
        {
            var output = _controller.Comment(_invalidInput);

            output.InvalidFields.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void should_return_the_field_name_of_the_invalid_field()
        {
            var output = _controller.Comment(_invalidInput);

            output.InvalidFields[0].ShouldEqual("UserDisplayName");
        }

        [Test]
        public void should_load_the_post_by_the_given_slug()
        {
            _controller.Comment(_validInput);

            _repository.AssertWasCalled(
                r=>r.Query<Post>(null),
                o => o.Constraints(Property.Value("Slug", _testSlug)));
        }

        [Test]
        public void should_attempt_load_user_by_email()
        {
            _controller.Comment(_validInput);

            _repository.AssertWasCalled(
                r => r.Query<User>(null),
                o => o.Constraints(Property.Value("Email", _validInput.UserEmail)));
        }

        [Test]
        public void should_add_comment_to_post()
        {
            _controller.Comment(_validInput);

            _post.GetComments().ShouldHaveCount(1);
        }

        [Test]
        public void should_add_comment_to_post_using_the_correct_body_and_user_subscribed_flag()
        {
            _controller.Comment(_validInput);

            var comment = _post.GetComments().First();

            comment.Body.ShouldEqual(_validInput.Body);
            comment.UserSubscribed.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class BlogPostController_when_creating_a_new_anonymous_user_for_new_comment
    {
        private IRepository _repository;
        private BlogPostController _controller;
        private string _testSlug;
        private BlogPostViewModel _curResult;
        private User _curUser;
        private IList<Post> _posts;
        private IList<User> _users;
        private IUrlResolver _resolver;
        private Post _post;

        [SetUp]
        public void SetUp()
        {
            _posts = new List<Post>();
            _users = new List<User>();

            _repository = MockRepository.GenerateStub<IRepository>();
            _resolver = MockRepository.GenerateStub<IUrlResolver>();


            _curUser = null;
            _curResult = null;

            _controller = new BlogPostController(_repository, _resolver);
            _repository.Stub(r => r.Query<Post>(null)).IgnoreArguments().Return(_posts.AsQueryable());
            _repository.Stub(r => r.Query<User>(null)).IgnoreArguments().Return(_users.AsQueryable());

            _testSlug = "TESTSLUG";

            Given = new BlogPostCommentViewModel
            {
                UserDisplayName = "username",
                UserEmail = "email",
                Body = "body",
                Slug = _testSlug
            };

            _post = new Post { Slug = _testSlug };
            _posts.Add(_post);
        }

        public BlogPostCommentViewModel Given { get; set; }

        public User CreatedUser
        {
            get
            {
                if( _curUser == null )
                {
                    var catcher = _repository.CaptureArgumentsFor(r => r.Save<User>(null));
                    _curResult = _controller.Comment(Given);
                    _curUser = catcher.First<User>();
                }
                return _curUser;
            }
        }

        [Test]
        public void the_user_should_have_been_created()
        {
            CreatedUser.ShouldNotBeNull();
        }

        [Test]
        public void the_user_should_be_anonymous()
        {
            CreatedUser.IsAuthenticated.ShouldBeFalse();
        }

        [Test]
        public void should_have_a_hashed_gravatar_email()
        {
            CreatedUser.HashedEmail.ShouldNotBeNull();
            CreatedUser.HashedEmail.ShouldNotEqual(CreatedUser.Email);
        }
    }

    [TestFixture]
    public class BlogPostController_when_updating_an_anonymous_user_for_new_comment
    {
        private IRepository _repository;
        private BlogPostController _controller;
        private string _testSlug;
        private IList<Post> _posts;
        private IList<User> _users;
        private IUrlResolver _resolver;
        private Post _post;

        [SetUp]
        public void SetUp()
        {
            _posts = new List<Post>();
            _users = new List<User>();

            _repository = MockRepository.GenerateStub<IRepository>();
            _resolver = MockRepository.GenerateStub<IUrlResolver>();

            _controller = new BlogPostController(_repository, _resolver);
            _repository.Stub(r => r.Query<Post>(null)).IgnoreArguments().Return(_posts.AsQueryable());
            _repository.Stub(r => r.Query<User>(null)).IgnoreArguments().Return(_users.AsQueryable());

            _testSlug = "TESTSLUG";

            Given = new BlogPostCommentViewModel
            {
                UserDisplayName = "username",
                UserEmail = "email",
                Body = "body",
                UserUrl = "url",
                Slug = _testSlug
            };


            _post = new Post { Slug = _testSlug };
            _posts.Add(_post);

            _controller.Comment(Given);

        }

        public BlogPostCommentViewModel Given { get; set; }

        [Test]
        public void the_display_name_should_have_been_updated()
        {
            var user = _post.GetComments().First().User;
            user.DisplayName.ShouldEqual(Given.UserDisplayName);
        }

        [Test]
        public void the_url_should_have_been_updated()
        {
            var user = _post.GetComments().First().User;
            user.Url.ShouldEqual(Given.UserUrl);
        }
    }
}