using System;
using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using FubuMVC.Core;

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

            if (post == null) return new BlogPostViewModel();

            return new BlogPostViewModel
            {
                Post = post,
                LocalPublishedDate = post.Published.Value.ToLongDateString(), //To local time
                CurrentPostOnPage = inModel.CurrentPostOnPage,
                TotalPostsOnPage = inModel.TotalPostsOnPage,
            };
        }
    }

    public class BlogPostSetupViewModel
    {
        public int PostYear { get; set; }
        public int PostMonth  { get; set; }
        public int PostDay  { get; set; }
        public string Slug { get; set; }
        public int CurrentPostOnPage { get; set; }
        public int TotalPostsOnPage { get; set; }
    }

    [Serializable]
    public class BlogPostViewModel : ViewModel
    {
        public Post Post { get; set; }
        public string LocalPublishedDate { get; set; }
        public int CurrentPostOnPage { get; set; }
        public int TotalPostsOnPage { get; set; }
    }
}