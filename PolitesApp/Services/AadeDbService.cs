using System.Linq;
using Microsoft.EntityFrameworkCore;
using Users.Models.Aade;

namespace Users.Services

{
    public class AadeDbService<TContext> : IAadeDbService where TContext : DbContext
    {
        private readonly TContext Context;

        public AadeDbService(TContext context)
        {
            Context = context;
        }

        public DbSet<AspNetUsers> Set() 
        {
            return Context.Set<AspNetUsers>();
        }
    }
}

