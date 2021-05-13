using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);  // used to avoid build errors

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId }); // configuring the primary key for this table
            builder.Entity<UserLike>()                      // specifying the relationship
                .HasOne(s => s.SourceUser)                  // has one source user    
                .WithMany(l => l.LikedUsers)                // source user can like many users   
                .HasForeignKey(s => s.SourceUserId)         
                .OnDelete(DeleteBehavior.Cascade);          // if we delete a user, we delete the related entities
            builder.Entity<UserLike>()                      // specifying the other part of the relationship
                .HasOne(s => s.LikedUser)                     
                .WithMany(l => l.LikedByUsers)                   
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}