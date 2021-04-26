using Microsoft.EntityFrameworkCore;

namespace Users.Models.Messages
{
    public partial class MessagesContext : DbContext
    {
        public MessagesContext()
        {
        }

        public MessagesContext(DbContextOptions<MessagesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Messages> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Messages;Trusted_Connection=True;");
            }
        }

    }
}
