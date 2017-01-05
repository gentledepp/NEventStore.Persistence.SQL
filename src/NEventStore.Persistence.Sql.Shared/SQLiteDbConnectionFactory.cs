#if !FRAMEWORK
using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace NEventStore.Persistence.Sql
{
    public class SqLiteFactory : DbProviderFactory
    {
        public static readonly SqLiteFactory Instance = new SqLiteFactory();

#endif
#if ANDROID || __IOS__
        static SqLiteFactory()
        {
            // we need to set this for android and ios
            // see here: https://github.com/aspnet/Microsoft.Data.Sqlite/blob/dev/src/Microsoft.Data.Sqlite/SqliteConnection.cs
            Environment.SetEnvironmentVariable("ADONET_DATA_DIR",
                Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        }
#endif
#if !FRAMEWORK

        public override DbCommand CreateCommand()
        {
            return new SqliteCommand();
        }

        public override DbConnection CreateConnection()
        {
            return new SqliteConnection();
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new SqliteConnectionStringBuilder();
        }

        public override DbParameter CreateParameter()
        {
            return new SqliteParameter();
        }
    }
}
#endif

