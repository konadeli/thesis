using Microsoft.EntityFrameworkCore;
using Users.Models.User;

namespace Users.Services
{
    public interface IUserDbService
    {
        void Save();
        DbSet<AspNetUsers> Set();
        bool Update(string id, AspNetUsers model);
    }
}
