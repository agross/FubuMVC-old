namespace AltOxite.Core.Web.Controllers
{
    public class PageNotFoundController
    {
        public PageNotFoundViewModel Index(PageNotFoundSetupViewModel inModel)
        {
            return new PageNotFoundViewModel { Description = inModel.Description, ShowDescription = !string.IsNullOrEmpty(inModel.Description) };
        }
    }

    public class PageNotFoundSetupViewModel : ViewModel
    {
        public string Description { get; set; }
    }

    public class PageNotFoundViewModel : ViewModel
    {
        public string Description { get; set; }
        public bool ShowDescription { get; set; }
    }
}