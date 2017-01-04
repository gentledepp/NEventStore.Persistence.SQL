#if !FRAMEWORK
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace NEventStore.Persistence.Sql
{
    public class SqLiteFactory : DbProviderFactory
    {
        public static readonly SqLiteFactory Instance = new SqLiteFactory();

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
