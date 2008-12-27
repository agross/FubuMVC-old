using System.Collections.Generic;
using AltOxite.Core.Domain;

namespace AltOxite.Core.Web.Controllers
{
    public class HomeController
    {
        public IndexViewModel Index(IndexSetupViewModel inModel)
        {
            return new IndexViewModel();
        }
    }

    public class IndexSetupViewModel
    {
    }

    public class IndexViewModel : ViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}