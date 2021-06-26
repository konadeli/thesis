using Microsoft.EntityFrameworkCore;
using Aade.Models.User;

namespace Aade.Services
{
    public interface IUserDbService
    {
        DbSet<AspNetUsers> Set();
    }
}
