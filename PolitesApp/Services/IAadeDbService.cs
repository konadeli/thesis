using Microsoft.EntityFrameworkCore;
using Users.Models.Aade;

namespace Users.Services
{
    public interface IAadeDbService
    {
        DbSet<AspNetUsers> Set();
    }
}
