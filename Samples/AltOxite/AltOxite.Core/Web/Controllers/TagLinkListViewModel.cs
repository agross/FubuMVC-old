using System;
using System.Collections.Generic;
using AltOxite.Core.Domain;

namespace AltOxite.Core.Web.Controllers
{
    [Serializable]
    public class TagLinkViewModel : ViewModel
    {
        public Tag Tag { get; set; }
    }
}