using System.Collections.Generic;
using FluentNHibernate.Cfg;

namespace AltOxite.Core.Config
{
    public class SQLiteSessionSourceConfiguration : FileBasedSessionSourceConfiguration
    {
        public SQLiteSessionSourceConfiguration(string db_file_name)
            : base(db_file_name)
        {
        }

        protected override IDictionary<string, string> GetProperties(string db_file_path)
        {
            return new SQLiteConfiguration()
                    .UsingFile(db_file_path)
                    .UseOuterJoin()
                    .ToProperties();
        }
    }
}