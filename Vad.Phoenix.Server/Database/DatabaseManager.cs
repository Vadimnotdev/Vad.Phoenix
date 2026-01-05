using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vad.Phoenix.Titan.Logic.Math;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Vad.Phoenix.Server.Database
{
    public class Accounts
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string passToken { get; set; }

        public int highId { get; set; } 

        public int lowId { get; set; }

        public int levelScore { get; set; }

        public int diamonds { get; set; }

        public int lastPlayedLevel { get; set; }

        public int energy { get; set; }

        public DateTime? lastEnergySpendTime { get; set; }

    }

    public class DatabaseManger
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<Accounts> collection;

        public DatabaseManger()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("Vad_Phoenix");
            collection = database.GetCollection<Accounts>("Accounts");
        }

        public async Task<Accounts> GetAccountAsync(int highId, int lowId)
        {
            var filter = Builders<Accounts>.Filter.Eq(a => a.highId, highId) &
                         Builders<Accounts>.Filter.Eq(a => a.lowId, lowId);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateLastPlayedLevelAsync(int highId, int lowId, int Newlevel)
        {
            var filter = Builders<Accounts>.Filter.Eq(a => a.highId, highId) &
                         Builders<Accounts>.Filter.Eq(a => a.lowId, lowId);

            var update = Builders<Accounts>.Update.Set(a => a.lastPlayedLevel, Newlevel);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UseDiamondsAsync(int highId, int lowId)
        {
            var filter = Builders<Accounts>.Filter.Eq(a => a.highId, highId) &
                        Builders<Accounts>.Filter.Eq(a => a.lowId, lowId);

            var update = Builders<Accounts>.Update.Inc(a => a.diamonds, -10);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task<bool> UseEnergyAsync(int highId, int lowId)
        {
            var account = await GetAccountAsync(highId, lowId);
            if (account == null)
                return false;

            await RenewEnergyByTimeAsync(account);

            var filter = Builders<Accounts>.Filter.And(
                Builders<Accounts>.Filter.Eq(a => a.Id, account.Id),
                Builders<Accounts>.Filter.Gt(a => a.energy, 0)
            );

            var update = Builders<Accounts>.Update
                .Inc(a => a.energy, -1)
                .Set(a => a.lastEnergySpendTime, DateTime.UtcNow);

            var result = await collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }


        public async Task RenewEnergyByTimeAsync(Accounts account)
        {
            if (account.energy >= 10 || account.lastEnergySpendTime == null)
                return;

            var now = DateTime.UtcNow;
            var elapsedMinutes = (now - account.lastEnergySpendTime.Value).TotalMinutes;

            int restored = (int)(elapsedMinutes / 15);
            if (restored <= 0)
                return;

            int newEnergy = Math.Min(10, account.energy + restored);

            DateTime? newTimestamp = newEnergy >= 10
                ? null
                : account.lastEnergySpendTime.Value.AddMinutes(restored * 15);

            var update = Builders<Accounts>.Update
                .Set(a => a.energy, newEnergy)
                .Set(a => a.lastEnergySpendTime, newTimestamp);

            await collection.UpdateOneAsync(
                Builders<Accounts>.Filter.Eq(a => a.Id, account.Id),
                update
            );

            account.energy = newEnergy;
            account.lastEnergySpendTime = newTimestamp;
        }

        public async Task RenewEnergyAsync(int highId, int lowId)
        {
            var filter = Builders<Accounts>.Filter.And(
                Builders<Accounts>.Filter.Eq(a => a.highId, highId),
                Builders<Accounts>.Filter.Eq(a => a.lowId, lowId)
            );

            var update = Builders<Accounts>.Update
                .Set(a => a.energy, 10)
                .Set(a => a.lastEnergySpendTime, null);
            

            await collection.UpdateOneAsync(filter, update);
        }




        public async Task UpdateLevelScoreAsync(int highId, int lowId, int level)
        {
            var filter = Builders<Accounts>.Filter.Eq(a => a.highId, highId) &
                         Builders<Accounts>.Filter.Eq(a => a.lowId, lowId);

            var update = Builders<Accounts>.Update.Set(a => a.levelScore, level);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateLevelScoreIfHigherAsync(
            int highId, int lowId, int level)
        {
            var filter = Builders<Accounts>.Filter.And(
                Builders<Accounts>.Filter.Eq(a => a.highId, highId),
                Builders<Accounts>.Filter.Eq(a => a.lowId, lowId),
                Builders<Accounts>.Filter.Lt(a => a.levelScore, level)
            );

            var update = Builders<Accounts>.Update
                .Set(a => a.levelScore, level);

            await collection.UpdateOneAsync(filter, update);
        }


        public async Task<(int highId, int lowId)> GetLastAccountIdAsync()
        {
            var lastAccount = await collection
                .Find(_ => true)
                .SortByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            return lastAccount != null ? (lastAccount.highId, lastAccount.lowId) : (0, 0);
        }


        public async Task CreateDefaultAccountAsync(int highId, int lowId)
        {
            var account = new Accounts
            {
                highId = highId,
                lowId = lowId,
                passToken = GenerateRandomToken(),
                lastPlayedLevel = 0,
                diamonds = 100,
                energy = 10,
                levelScore = 1,
                lastEnergySpendTime = null


            };


            await collection.InsertOneAsync(account);
        }


        private static string GenerateRandomToken(int length = 40)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var data = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[data[i] % chars.Length];
            }

            return new string(result);
        }

    }

}