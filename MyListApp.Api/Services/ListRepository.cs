using MyListApp.Api.Data.Context;
using MyListApp.Api.Data.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;

namespace MyListApp.Api.Services
{

    public class ListRepository : AppRepositoryBase<ListModel>
    {
        public ListRepository(IIdentity user) : base(user)
        {
        }

        public override ListModel Add(ListModel item)
        {
            item.OwnerId = _userId;
            return base.Add(item);
        }

        public override IEnumerable<ListModel> Get(string userIdField = "ownerId")
        {
            // find all lists owned by the current user and all lists that have
            // a sharing record referencing the current user
            return  _context.Lists.Where(l => l.OwnerId == _userId || l.Sharing.Any(s => s.UserId == _userId))
                .Include(l => l.Items)
                .Include(l => l.Sharing);
        }

        public override bool Update(int id, ListModel item)
        {
            _updateFields = new List<string> { "Name", "Type" };
            return base.Update(id, item);
        }
    }
}