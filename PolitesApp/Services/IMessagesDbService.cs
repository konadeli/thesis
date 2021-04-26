using Microsoft.EntityFrameworkCore;
using Users.Models.Messages;

namespace Users.Services
{
    public interface IMessagesDbService
    {
        void Save();
        DbSet<Messages> Set();
        bool Update(string id, Messages model);
        string Create(Messages model);
    }
}
