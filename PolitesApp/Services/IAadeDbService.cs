using Microsoft.EntityFrameworkCore;
using Users.Models.Aade;

namespace Users.Services
{
    public interface IAadeDbService
    {
        void Save();
        DbSet<AspNetUsers> Set();
        bool Update(string id, AspNetUsers model);
    }
}
