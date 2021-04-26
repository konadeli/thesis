using System.Linq;
using Microsoft.EntityFrameworkCore;
using Users.Models.User;

namespace Users.Services

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

