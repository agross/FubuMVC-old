using System;

namespace VsTemplate.Core.Web.Controllers
{
    public class HomeController
    {
        //Ctor arguments will be full filled by the IoC container
        //public HomeController()
        //{
        //}

        public IndexViewModel Index(IndexSetupViewModel inModel)
        {
            return new IndexViewModel();
        }
    }

    public class IndexSetupViewModel : ViewModel
    {
    }

    [Serializable]
    public class IndexViewModel : ViewModel
    {
    }
}