using System;
using AltOxite.Core.Domain;

namespace AltOxite.Core.Web.Controllers
{
    [Serializable]
    public class BlogPostViewModel : ViewModel
    {
        public Post Post { get; set; }
        public DateTime LocalPublishedDate { get; set; }
    }
}