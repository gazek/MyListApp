using System.Collections.Generic;
using MyListApp.Api.Data.Entities;
using System.Security.Principal;

namespace MyListApp.Api.Services
{
    public interface IListRepository
    {
        IIdentity User { set; }
        ListModel Add(ListModel item);
        ListModel Get(int id);
        IEnumerable<ListModel> Get(string userIdField = "ownerId");
        bool Update(int id, ListModel item);
        bool Delete(int id);
        void Dispose();
    }
}