using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aade.Models.User;

namespace Aade.Services

{
    public class UserDbService<TContext> : IUserDbService where TContext : DbContext
    {
        private readonly TContext Context;

        public UserDbService(TContext context)
        {
            Context = context;
        }

        public DbSet<AspNetUsers> Set() 
        {
            return Context.Set<AspNetUsers>();
        }
    }
}

