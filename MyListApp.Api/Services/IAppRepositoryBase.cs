using System.Collections.Generic;
using System.Security.Principal;

namespace MyListApp.Api.Services
{
    public interface IAppRepositoryBase<T> where T : class
    {
        IIdentity User { set; }

        T Add(T item);
        bool Delete(int id);
        void Dispose();
        T Get(int id);
        IEnumerable<T> Get(string userIdField = "userId");
        bool Update(int id, T newItem);
    }
}