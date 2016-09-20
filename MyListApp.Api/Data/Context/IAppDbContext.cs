using System.Data.Entity;
using MyListApp.Api.Data.Entities;
using System.Linq;

namespace MyListApp.Api.Data.Context
{
    public interface IAppDbContext
    {
        DbSet<InvitationModel> Invitations { get; set; }
        DbSet<ListModel> Lists { get; set; }

        DbSet<T> Set<T>() where T : class;
        void Dispose();
    }
}