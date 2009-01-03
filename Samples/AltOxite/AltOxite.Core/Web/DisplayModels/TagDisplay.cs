using System;
using AltOxite.Core.Domain;

namespace AltOxite.Core.Web.DisplayModels
{
    public class TagDisplay
    {
        public TagDisplay(Tag tag)
        {
            Name = tag.Name;
            CreatedDate = tag.CreatedDate;
        }

        public string Name { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}