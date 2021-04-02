using System.Collections.Generic;
using LiteDB;
using static SteamSwitcher.Models;

namespace SteamSwitcher
{
    public class LiteDatabaseService
    {
        private readonly ILiteCollection<SteamUser> _steamUserCollection;

        public LiteDatabaseService(DatabaseSettings settings)
        {
            var litedbClient = new LiteDatabase(settings.ConnectionString);
            _steamUserCollection = litedbClient.GetCollection<SteamUser>(settings.DatabaseName);
        }

        public IEnumerable<SteamUser> FindAll()
        {
            return _steamUserCollection.FindAll();
        }

        public SteamUser FindById(BsonValue id)
        {
            return _steamUserCollection.FindById(id);
        }

        public void Insert(SteamUser entity)
        {
            _steamUserCollection.Insert(entity);
        }

        public void Delete(BsonValue id)
        {
            _steamUserCollection.Delete(id);
        }
    }
}