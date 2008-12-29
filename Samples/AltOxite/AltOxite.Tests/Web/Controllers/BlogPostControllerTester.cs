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
    }
}