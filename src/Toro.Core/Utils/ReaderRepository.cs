using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Toro.Core.Entities;

namespace Toro.Core.Utils
{
    public interface IMongoContext
    {
        public IMongoCollection<BankAccount> Contas { get; }
        public IMongoCollection<Trend> Transacoes { get; }
    }
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext()
        {
            string connectionString = "mongodb://root:example@localhost:27017/";
            string databaseName = "sales";
            IMongoClient _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public IMongoCollection<BankAccount> Contas => _database.GetCollection<BankAccount>(nameof(BankAccount));
        public IMongoCollection<Trend> Transacoes => _database.GetCollection<Trend>(nameof(Trend));
    }
}
