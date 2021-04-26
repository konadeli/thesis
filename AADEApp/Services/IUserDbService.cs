using Microsoft.EntityFrameworkCore;
using Aade.Models.User;

namespace Aade.Services
{
    public interface IUserDbService
    {
        void Save();
        DbSet<AspNetUsers> Set();
        bool Update(string id, AspNetUsers model);
    }
}
