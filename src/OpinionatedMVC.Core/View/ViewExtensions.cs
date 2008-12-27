using System;
using System.Web.UI;

namespace OpinionatedMVC.Core.View
{
    public static class ViewExtensions
    {
        public static string ToVirtualPath(this Type viewType)
        {
            string[] nameParts = viewType.FullName.Split('.');
            string path = "~";
            for (int i = 2; i < nameParts.Length; i++)
            {
                string part = nameParts[i];
                path += "/" + part;
            }

            if (typeof(UserControl).IsAssignableFrom(viewType))
            {
                path += ".ascx";
            }
            else if (typeof(MasterPage).IsAssignableFrom(viewType))
            {
                path += ".master";
            }
            else
            {
                path += ".aspx";
            }

            return path;
        }
    }
}