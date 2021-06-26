using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aade.Models.Aade;

namespace Aade.Services

{
    public class AadeDbService<TContext> : IAadeDbService where TContext : DbContext
    {
        private readonly TContext _context;

        public AadeDbService(TContext context)
        {
            _context = context;
        }

        public DbSet<AspNetUsers> Set() 
        {
            return _context.Set<AspNetUsers>();
        }


        public bool Update(string id, AspNetUsers model) 
        {
            var entity = _context.Set<AspNetUsers>().FirstOrDefault(t => t.Id == id);
            if (entity == null) return false;

            model.Id = id;

            _context.Entry(entity).CurrentValues.SetValues(model);

            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}

