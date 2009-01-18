using System;
using VsTemplate.Core.Domain;
using FubuMVC.Core;

namespace VsTemplate.Core.Config
{
    [Serializable]
    public class SiteConfigDTO
    {
        public SiteConfigDTO()
        {
            // Defaults
            pageTitleSeparator = " - ";
            themeDefault = "default";
            scriptsPath = "~/scripts";
            cssFilePath = "~/content/skins";
            imagesPath = "~/content/images";
            commentAnonymousStateDefault = "Normal";
            emailUsername = "Admin";
            includeOpenSearch = true;
            authorAutoSubscribe = true;
            postEditTimeout = 24D;
            seoRobots = "index,follow";
            trackbacksEnabled = true;
        }

        public AliasDTO[] aliases { get; set; }

        public Guid id { get; set; }
        public string name { get; set; }
        public string host { get; set; }
        public string scriptsPath { get; set; }
        public string cssFilePath { get; set; }
        public string imagesPath { get; set; }
        public string languageDefault { get; set; }
        public string gravatarDefault { get; set; }
        public string pageTitleSeparator { get; set; }
        public string themeDefault { get; set; }
        public string favIconUrl { get; set; }
        public string commentAnonymousStateDefault { get; set; }
        public string emailUsername { get; set; }
        public bool includeOpenSearch { get; set; }
        public bool authorAutoSubscribe { get; set; }
        public double postEditTimeout { get; set; }
        public string seoRobots { get; set; }
        public bool trackbacksEnabled { get; set; }

        public void ToSiteConfiguration(SiteConfiguration config)
        {
            config.ID = id;
            config.Name = name;
            config.Host = host;
            config.LanguageDefault = languageDefault;
            config.PageTitleSeparator = pageTitleSeparator;
            config.ThemeDefault = themeDefault;
            config.FavIconUrl = favIconUrl;
            config.ScriptsPath = scriptsPath;
            config.CssPath = cssFilePath;
            config.ImagesPath = imagesPath;
            config.CommentAnonymousStateDefault = commentAnonymousStateDefault;
            config.EmailUsername = emailUsername;
            config.IncludeOpenSearch = includeOpenSearch;
            config.AuthorAutoSubscribe = authorAutoSubscribe;
            config.PostEditTimeout = postEditTimeout;
            config.SEORobots = seoRobots;
            config.GravatarDefault = gravatarDefault;
            config.TrackbacksEnabled = trackbacksEnabled;

            if (aliases != null)
            {
                aliases.Each(a => config.AddAlias(a.ToAlias()));
            }
        }
    }
}
