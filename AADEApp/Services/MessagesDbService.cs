using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aade.Models.Messages;

namespace Aade.Services

{
    public class MessagesDbService<TContext> : IMessagesDbService where TContext : DbContext
    {
        private readonly TContext Context;

        public MessagesDbService(TContext context)
        {
            Context = context;
        }

        public DbSet<Messages> Set() 
        {
            return Context.Set<Messages>();
        }

        public string Create(Messages model)
        {
            Context.Set<Messages>().Add(model);
            return model.Id;
        }


        public bool Update(string id, Messages model) 
        {
            var entity = Context.Set<Messages>().FirstOrDefault(t => t.Id == id);
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

