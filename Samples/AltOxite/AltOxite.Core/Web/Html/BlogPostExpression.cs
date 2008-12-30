using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltOxite.Core.Domain;
using AltOxite.Core.Web.Controllers;
using AltOxite.Core.Web.WebForms;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View.WebForms;
using FubuMVC.Core;

namespace AltOxite.Core.Web.Html
{
    public class BlogPostExpression
    {
        private readonly IAltOxitePage _viewPage;
        private readonly IWebFormsViewRenderer _renderer;

        private IEnumerable<Post> _posts;

        public IRenderPartialForScope RenderExpression { get; private set; }

        public BlogPostExpression(IAltOxitePage viewPage, IWebFormsViewRenderer renderer)
        {
            _viewPage = viewPage;
            _renderer = renderer;
        }

        public BlogPostExpression ForEach(IEnumerable<Post> posts)
        {
            _posts = posts;
            return this;
        }

        public BlogPostExpression Display<USERCONTROL>()
            where USERCONTROL : AltOxiteUserControl<BlogPostViewModel>
        {
            RenderExpression = new RenderPartialExpression(_viewPage, _renderer).Using<USERCONTROL>();
            return this;
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            if (_posts != null)
            {
                var postCounter = 1;
                _posts.Each(
                    post =>
                    // TODO: Stubbed temporarily, see below
                    output.Append(
                        RenderExpression.For(new BlogPostViewModel
                            {Post = post, Class = "", LocalPublishedDate = "", TagLinksAndCommentsLink = ""}))); 

                     //TODO: Don't call controllers from views! no! bad!
                    //RenderExpression.For(new BlogPostController().Index(new BlogPostSetupViewModel { Post = post, CurrentPostOnPage = postCounter++, TotalPostsOnPage = _posts.Count() }))));
            }

            return output.ToString();
        }
    }
}