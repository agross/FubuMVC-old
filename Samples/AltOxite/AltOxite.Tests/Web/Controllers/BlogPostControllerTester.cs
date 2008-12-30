using System;
using System.Linq;
using System.Collections.Generic;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using AltOxite.Core.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltOxite.Tests.Web.Controllers
{
    [TestFixture]
    public class BlogPostControllerTester
    {
        private IRepository _repository;
        private BlogPostController _controller;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateStub<IRepository>();
            _controller = new BlogPostController(_repository);
        }

        [Test]
        public void index_should_do_nothing_but_just_render_the_view_if_nothing_was_supplied()
        {
            var output = _controller.Index(new BlogPostSetupViewModel());
            
            output.Post.ShouldBeNull();
            _repository.AssertWasNotCalled(r=>r.Query<Post>());
        }

        [Test]
        public void should_do_nothing_repository_returns_no_results_for_query()
        {
            _repository.Stub(r => r.Query<Post>()).Return(new Post[0].AsQueryable());

            var output = _controller.Index(new BlogPostSetupViewModel {Slug = "badslug"});

            output.Post.ShouldBeNull();
        }

        [Test]
        public void verify_that_the_class_property_on_the_view_is_set_correctly()
        {
            var slug = "EXPECTEDSLUG";

            var post = new Post {Slug = slug, Published = DateTime.Parse("2008-12-05 09:29:03.270")};
            
            _repository.Stub(r => r.Query<Post>()).Return(new []{post}.AsQueryable());
            
            _controller.Index(new BlogPostSetupViewModel { Slug = slug, CurrentPostOnPage = 1, TotalPostsOnPage = 1 })
                .Class
                .ShouldContain("class=\"first last\"");
        }

        [Test]
        public void verify_that_the_taglinksandcommentslink_property_on_the_view_is_set_correctly()
        {
            var slug = "EXPECTEDSLUG";

            var post = new Post
            {
                Slug = slug,
                Published = DateTime.Parse("2008-12-05 09:29:03.270"),
                Comments = new[]{new Comment()},
                Tags = new[]{ new Tag { Name = "Oxite" } }
            };

            _repository.Stub(r => r.Query<Post>()).Return(new[] { post }.AsQueryable());

            _controller.Index(new BlogPostSetupViewModel{Slug = slug})
                .TagLinksAndCommentsLink
                .ShouldEqual("Filed under <a href=\"#\">Oxite</a> | <a href=\"linktopostcomments\">1 comment</a> <a href=\"linktopost\" class=\"arrow\">&raquo;</a>");
        }
    }
}