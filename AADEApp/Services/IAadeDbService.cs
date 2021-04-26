using Microsoft.EntityFrameworkCore;
using Aade.Models.Aade;

namespace Aade.Services
{
    public interface IAadeDbService
    {
        void Save();
        DbSet<AspNetUsers> Set();
        bool Update(string id, AspNetUsers model);
    }
}
