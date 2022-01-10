using Microsoft.EntityFrameworkCore;
using System.Configuration;


namespace ClientServerComponents.Models
{
    public class MessengerDbContext : DbContext
    {
        public MessengerDbContext(DbContextOptions<MessengerDbContext> options) : base(options)
        {
        }
        public MessengerDbContext()
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*            modelBuilder.Entity<Message>(entity =>
                        {
                            entity.HasOne(x => x.Sender)
                            .WithMany(x => x.Messages)
                            .HasForeignKey(x => x.MessageId);
                        });

                        modelBuilder.Entity<User>(entity =>
                        {
                            entity.HasMany(x => x.Contacts)
                            .WithMany(x => x.Contacts);
                        });*/
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MessengerConnectionString"].ConnectionString);
        }
    }
}
