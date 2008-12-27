using System.Collections.Generic;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;

namespace AltOxite.Core.Web.Controllers
{
    public class HomeController
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public IndexViewModel Index(IndexSetupViewModel inModel)
        {
            var posts = _repository.Query<Post>();
            return new IndexViewModel{Posts = posts};
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