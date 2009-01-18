using VsTemplate.Core.Web;
using VsTemplate.Core.Web.Controllers;
using VsTemplate.Core.Web.DisplayModels;
using VsTemplate.Core.Web.WebForms;

// Master Pages
public class SiteMasterView : FubuMvcMasterPage{}
    
// Pages
public class HomeIndexView : FubuMvcPage<IndexViewModel> { }

// User Controls
public class LoggedInStatus : FubuMvcUserControl<ViewModel> { }
public class LoggedOutStatus : FubuMvcUserControl<ViewModel> { }

// For Debug View
public class DebugIndexView : FubuMvcPage<DebugViewModel> { }
public class ControllerAction : FubuMvcUserControl<ControllerActionDisplay> { }
public class DebugSingleLine : FubuMvcUserControl<DebugSingleLineDisplay> { }
