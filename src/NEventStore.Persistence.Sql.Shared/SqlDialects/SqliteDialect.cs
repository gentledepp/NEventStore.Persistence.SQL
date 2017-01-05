using System.Globalization;

namespace NEventStore.Persistence.Sql.SqlDialects
{
    using System;

    public class SqliteDialect : CommonSqlDialect
    {
        public override string InitializeStorage
        {
            get { return SqliteStatements.InitializeStorage; }
        }

        // Sqlite wants all parameters to be a part of the query
        public override string GetCommitsFromStartingRevision
        {
            get { return base.GetCommitsFromStartingRevision.Replace("\n ORDER BY ", "\n  AND @Skip = @Skip\nORDER BY "); }
        }

        public override string PersistCommit
        {
            get { return SqliteStatements.PersistCommit; }
        }

        public override bool IsDuplicate(Exception exception)
        {
            string message = exception.Message.ToUpperInvariant();
            return message.Contains("DUPLICATE") || message.Contains("UNIQUE") || message.Contains("CONSTRAINT");

        }

        public override DateTime ToDateTime(object value)
        {
#if !FRAMEWORK
            if (value is DateTime)
            {
                return ((DateTime)value).ToUniversalTime();
            }

            if (value is string)
            {
                DateTime d;
                // expecting something like 2018-01-04 15:46:32.3213822
                //                       or 2017-01-04 15:54:09.586809
                if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d))
                {
                    return d.ToUniversalTime();
                }
                if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d))
                {
                    return d.ToUniversalTime();
                }
                if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss.fffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d))
                {
                    return d.ToUniversalTime();
                }
                if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d))
                {
                    return d.ToUniversalTime();
                }
                if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d))
                {
                    return d.ToUniversalTime();
                }
                if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss.ff", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d))
                {
                    return d.ToUniversalTime();
                }
                if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss.f", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d))
                {
                    return d.ToUniversalTime();
                }
            }
            throw new InvalidOperationException($"('{value}'): the type '{value?.GetType().FullName}' cannot be converted to a valid datetime");
#else
            return ((DateTime) value).ToUniversalTime();
#endif
        }
    }
}