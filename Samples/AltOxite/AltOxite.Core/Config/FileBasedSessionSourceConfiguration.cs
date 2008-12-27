using System.Collections.Generic;
using System.IO;
using FluentNHibernate;
using FluentNHibernate.Framework;
using FubuMVC.Core.Html;

namespace AltOxite.Core.Config
{
    public abstract class FileBasedSessionSourceConfiguration : ISessionSourceConfiguration
    {
        private readonly string _physicalDbFilePath;
        private readonly bool _isNewDatabase;

        protected FileBasedSessionSourceConfiguration(string db_file_name)
        {
            _physicalDbFilePath = UrlContext.ToPhysicalPath(db_file_name);

            _isNewDatabase = (!File.Exists(_physicalDbFilePath));
        }

        protected abstract IDictionary<string, string> GetProperties(string db_file_path);

        private void create_schema_if_it_does_not_already_exist(ISessionSource source)
        {
            if (_isNewDatabase) source.BuildSchema();
        }

        public ISessionSource CreateSessionSource(PersistenceModel model)
        {
            var properties = GetProperties(_physicalDbFilePath);

            var source = new SessionSource(properties, model);

            create_schema_if_it_does_not_already_exist(source);

            return source;
        }
    }
}