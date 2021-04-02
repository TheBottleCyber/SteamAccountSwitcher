using System.Collections.Generic;
using LiteDB;
using SteamSwitcher.Models;

namespace SteamSwitcher.Database
{
    public class LiteDatabaseService : IDatabaseService
    {
        private readonly ILiteCollection<SteamUser> _steamUserCollection;

        public LiteDatabaseService(IDatabaseSettings settings)
        {
            var litedbClient = new LiteDatabase(settings.ConnectionString);
            _steamUserCollection = litedbClient.GetCollection<SteamUser>(settings.DatabaseName);
        }

        public IEnumerable<SteamUser> FindAll()
        {
            return _steamUserCollection.FindAll();
        }

        public SteamUser FindById(int id)
        {
            return _steamUserCollection.FindById(id);
        }

        public void Insert(SteamUser entity)
        {
            _steamUserCollection.Insert(entity);
        }

        public void Delete(int id)
        {
            _steamUserCollection.Delete(id);
        }
    }
}