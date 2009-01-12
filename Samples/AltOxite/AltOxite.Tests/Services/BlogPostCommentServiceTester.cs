using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using AltOxite.Core.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace AltOxite.Tests.Services
{
    [TestFixture]
    public class BlogPostCommentServiceTester
    {
        private IRepository _repository;
        private IBlogPostCommentService _blogPostCommentService;
        private string GivenBody;
        private bool GivenUserSubscriberd;
        private User GivenUser;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateStub<IRepository>();
            _blogPostCommentService = new BlogPostCommentService(_repository);
            GivenBody = "body";
            GivenUserSubscriberd = true;
            GivenUser = new User { DisplayName = "user" };
        }

        [Test]
        [Ignore("This test should be moved to UserService, or at least refactored, will do this tomorrow")]
        public void should_attempt_load_user_by_email()
        {
            //_blogPostCommentService.AddCommentToBlogPost();
            //_controller.Comment(_validInput);

            //_repository.AssertWasCalled(
            //    r => r.Query<User>(null),
            //    o => o.Constraints(Property.Value("Email", _validInput.UserEmail)));
        }

        [Test]
        public void should_add_comment_to_post()
        {
            var _post = new Post();
            _blogPostCommentService.AddCommentToBlogPost(GivenBody, GivenUserSubscriberd, GivenUser, _post);
            _post.GetComments().ShouldHaveCount(1);
        }

        [Test]
        public void should_add_comment_to_post_using_the_correct_body_and_user_subscribed_flag()
        {
            var _post = new Post();
            _blogPostCommentService.AddCommentToBlogPost(GivenBody, GivenUserSubscriberd, GivenUser, _post);

            var comment = _post.GetComments().First();

            comment.Body.ShouldEqual(GivenBody);
            comment.UserSubscribed.ShouldBeTrue();
        }
    }
}