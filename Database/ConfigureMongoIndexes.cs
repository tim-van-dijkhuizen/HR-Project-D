using FitbyteServer.Models;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace FitbyteServer.Database {

    public class ConfigureMongoIndexes : IHostedService {

        private readonly IMongoDatabase _database;

        public ConfigureMongoIndexes(IMongoDatabase database) {
            _database = database;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            IMongoCollection<Profile> collection = _database.GetCollection<Profile>("profiles");

            // Create index
            StringFieldDefinition<Profile> field = new StringFieldDefinition<Profile>("Username");
            IndexKeysDefinition<Profile> index = new IndexKeysDefinitionBuilder<Profile>().Ascending(field);

            CreateIndexOptions options = new CreateIndexOptions() { Unique = true };
            CreateIndexModel<Profile> model = new CreateIndexModel<Profile>(index, options);
        
            await collection.Indexes.CreateOneAsync(model, cancellationToken: cancellationToken);
        }


        public Task StopAsync(CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
    }

}
