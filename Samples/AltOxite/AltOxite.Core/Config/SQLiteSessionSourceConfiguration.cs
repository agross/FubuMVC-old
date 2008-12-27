using System.Collections.Generic;
using System.IO;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Framework;
using FubuMVC.Core.Html;

namespace AltOxite.Core.Config
{
    public class SQLiteSessionSourceConfiguration : ISessionSourceConfiguration
    {
        private readonly IDictionary<string, string> _properties;
        private readonly string _physicalDbFilePath;
        private readonly bool _isNewDatabase;

        public SQLiteSessionSourceConfiguration(string sql_lite_db_file_name)
        {
            _physicalDbFilePath = UrlContext.ToPhysicalPath(sql_lite_db_file_name);

            _isNewDatabase = (!File.Exists(_physicalDbFilePath));

            _properties = new SQLiteConfiguration()
                    .UsingFile(_physicalDbFilePath)
                    .UseOuterJoin()
                    .ToProperties();
        }

        private void create_db_file_if_it_does_not_already_exist(ISessionSource source)
        {
            if (_isNewDatabase) source.BuildSchema();
        }

        public ISessionSource CreateSessionSource(PersistenceModel model)
        {
            var source = new SessionSource(_properties, model);

            create_db_file_if_it_does_not_already_exist(source);

            return source;
        }
    }
}