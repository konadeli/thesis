using Microsoft.EntityFrameworkCore;
using Users.Models.Messages;

namespace Users.Services
{
    public interface IMessagesDbService
    {
        void Save();
        string Create(Messages model);
    }
}
