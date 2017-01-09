namespace NEventStore.Persistence.AcceptanceTests
{
    using NEventStore.Persistence.Sql;
    using NEventStore.Persistence.Sql.SqlDialects;
    using NEventStore.Serialization;

    public partial class PersistenceEngineFixture
    {
        public PersistenceEngineFixture()
        {
            var knownTypes = new[]
               {
                typeof(NEventStore.Persistence.AcceptanceTests.ExtensionMethods.SomeDomainEvent),
                typeof(NEventStore.Persistence.AcceptanceTests.SimpleMessage),
            };
            var serializer = new ProtobufSerializer(knownTypes);

            _createPersistence = pageSize =>
                new SqlPersistenceFactory(
                    //new ConfigurationConnectionFactory("NEventStore.Persistence.AcceptanceTests.Properties.Settings.SQLite"),
                    new ConfigurationConnectionFactory("NEventStore", "thisisignored", "Data Source=NEventStore.db;"),
                    //new JsonSerializer(),
                    serializer,
                    new SqliteDialect(),
                    pageSize: pageSize).Build();
        }
    }
}