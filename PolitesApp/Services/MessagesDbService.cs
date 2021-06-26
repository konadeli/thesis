using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Users.Models.Messages;

namespace Users.Services

{
    public class MessagesDbService<TContext> : IMessagesDbService where TContext : DbContext
    {
        private readonly TContext Context;

        public MessagesDbService(TContext context)
        {
            Context = context;
        }

        public string Create(Messages model)
        {
            Context.Set<Messages>().Add(model);
            return model.Id;
        }

        public void Save()
        {
            Context.SaveChanges();
            Context.Dispose();
        }
    }
}

