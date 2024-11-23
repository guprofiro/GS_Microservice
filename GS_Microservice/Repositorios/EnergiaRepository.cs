using Dapper;
using Domain;
using Repositorios.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Repositorios
{
    public class EnergiaRepository : IEnergiaRepository
    {
        private readonly IMongoCollection<Energia> _energiaCollection;

        public EnergiaRepository(IOptions<MongoSettings> mongoSettings)
        {
            var cliente = new MongoClient(mongoSettings.Value.connectionString);
            var bancoDados = cliente.GetDatabase(mongoSettings.Value.databaseName);
            _energiaCollection = bancoDados.GetCollection<Energia>(mongoSettings.Value.collectionName);
        }

        public async Task<IEnumerable<Energia>> ObterTodosRegistros()
        {
            return await _energiaCollection.Find(energia => true).ToListAsync();
        }

        public async Task InserirRegistro(Energia novaEnergia)
        {
            await _energiaCollection.InsertOneAsync(novaEnergia);
        }
    }
}