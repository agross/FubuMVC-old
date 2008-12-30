using System;
using System.Collections.Generic;
using AltOxite.Core.Domain;
using AltOxite.Core.Web.Controllers;
using NUnit.Framework;

namespace AltOxite.Tests.Web.Controllers
{
    [TestFixture]
    public class BlogPostControllerTester
    {
        [Test]
        public void index_should_do_nothing_but_just_render_the_view_if_nothing_was_supplied()
        {
            new BlogPostController().Index(new BlogPostSetupViewModel()).ShouldBeOfType<BlogPostViewModel>();
        }

        [Test]
        public void verify_that_the_class_property_on_the_view_is_set_correctly()
        {
            new BlogPostController().Index(new BlogPostSetupViewModel { Post = new Post { Published = DateTime.Parse("2008-12-05 09:29:03.270"), }, CurrentPostOnPage = 1, TotalPostsOnPage = 1 })
                .Class
                .ShouldContain("class=\"first last\"");
        }

        [Test]
        public void verify_that_the_taglinksandcommentslink_property_on_the_view_is_set_correctly()
        {
            var post = new Post
            {
                Published = DateTime.Parse("2008-12-05 09:29:03.270"),
                Comments = new List<Comment>
                {
                    new Comment()
                }, 
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = "Oxite"
                    }
                }
            };
            new BlogPostController().Index(new BlogPostSetupViewModel { Post = post })
                .TagLinksAndCommentsLink
                .ShouldEqual("Filed under <a href=\"#\">Oxite</a> | <a href=\"linktopostcomments\">1 comment</a> <a href=\"linktopost\" class=\"arrow\">&raquo;</a>");
        }
    }
}