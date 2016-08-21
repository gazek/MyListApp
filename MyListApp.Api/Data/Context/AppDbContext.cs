using Microsoft.AspNet.Identity.EntityFramework;
using MyListApp.Api.Data.Entities;
using System.Data.Entity;

namespace MyListApp.Api.Data.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ListModel> Lists { get; set; }
        public DbSet<InvitationModel> Invitations { get; set; }

        public AppDbContext()
             : base("AppDbContext")
        {

        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InvitationModel>()
                .HasRequired<IdentityUser>(c => c.Invitee)
                .WithMany()
                .HasForeignKey(c => c.InviteeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvitationModel>()
                .HasRequired<IdentityUser>(c => c.Invitor)
                .WithMany()
                .HasForeignKey(c => c.InvitorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ListShareModel>()
                .HasRequired<IdentityUser>(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ListItemModel>()
                .HasRequired<IdentityUser>(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorId)
                .WillCascadeOnDelete(false);
        }
        
    }
}