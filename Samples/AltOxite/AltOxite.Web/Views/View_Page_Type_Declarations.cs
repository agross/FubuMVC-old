using AltOxite.Core.Domain;
using AltOxite.Core.Web;
using AltOxite.Core.Web.Controllers;
using AltOxite.Core.Web.DisplayModels;
using AltOxite.Core.Web.WebForms;

// Master Pages
public class SiteMasterView : AltOxiteMasterPage{}

// Pages
public class HomeIndexView : AltOxitePage<IndexViewModel> { }
public class PageNotFoundIndexView : AltOxitePage<PageNotFoundViewModel> { }
public class LoginIndexView : AltOxitePage<LoginViewModel> { }
public class DebugIndexView : AltOxitePage<ViewModel> { }
public class BlogPostIndexView : AltOxitePage<BlogPostViewModel> { }
public class TagIndexView : AltOxitePage<TagViewModel> { }

// User Controls
public class LoggedInStatus : AltOxiteUserControl<ViewModel> { }
public class LoggedOutStatus : AltOxiteUserControl<ViewModel> { }

public class BlogPost : AltOxiteUserControl<PostDisplay> { }
public class BlogPostComment : AltOxiteUserControl<CommentDisplay> { }
public class LoggedInCommentForm : AltOxiteUserControl<CommentFormDisplay> { }
public class LoggedOutCommentForm : AltOxiteUserControl<CommentFormDisplay> { }

public class TagLink : AltOxiteUserControl<TagDisplay> { }
