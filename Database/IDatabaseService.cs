using System.Collections.Generic;
using SteamSwitcher.Models;

namespace SteamSwitcher.Database
{
    public interface IDatabaseService
    {
        IEnumerable<SteamUser> FindAll();
        SteamUser FindById(int id);
        void Insert(SteamUser entity);
        void Delete(int id);
    }
}