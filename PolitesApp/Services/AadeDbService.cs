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


        public bool Update(string id, AspNetUsers model) 
        {
            var entity = Context.Set<AspNetUsers>().FirstOrDefault(t => t.Id == id);
            if (entity == null) return false;

            model.Id = id;

            Context.Entry(entity).CurrentValues.SetValues(model);

            return true;
        }

        public void Save()
        {
            Context.SaveChanges();
            Context.Dispose();
        }
    }
}

