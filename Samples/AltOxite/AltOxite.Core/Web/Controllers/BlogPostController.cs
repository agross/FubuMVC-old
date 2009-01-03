using System;
using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using AltOxite.Core.Web.DisplayModels;
using FubuMVC.Core;
using FubuMVC.Core.Controller.Config;

namespace AltOxite.Core.Web.Controllers
{
    public class BlogPostController
    {
        private readonly IRepository _repository;

        public BlogPostController(IRepository repository)
        {
            _repository = repository;
        }

        public BlogPostViewModel Index(BlogPostSetupViewModel inModel)
        {
            if (inModel.Slug.IsEmpty()) return new BlogPostViewModel();

            var post = _repository.Query<Post>().Where(p => p.Slug == inModel.Slug).SingleOrDefault();

            return new BlogPostViewModel
            {
                Post = post == null ? null : new PostDisplay(post),
            };
        }
    }

    public class BlogPostSetupViewModel
    {
        [Required] public int PostYear { get; set; }
        [Required] public int PostMonth  { get; set; }
        [Required] public int PostDay  { get; set; }
        [Required] public string Slug { get; set; }
    }

    [Serializable]
    public class BlogPostViewModel : ViewModel
    {
        public PostDisplay Post { get; set; }
    }
}