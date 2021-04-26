using Microsoft.EntityFrameworkCore;
using Aade.Models.Messages;

namespace Aade.Services
{
    public interface IMessagesDbService
    {
        void Save();
        DbSet<Messages> Set();
        bool Update(string id, Messages model);
        string Create(Messages model);
    }
}
