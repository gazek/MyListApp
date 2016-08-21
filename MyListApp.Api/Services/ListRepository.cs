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

        public override bool Delete(int id)
        {
            _item = _context.Lists.Where(l => l.OwnerId == _userId).Where(l => l.Id == id).SingleOrDefault();

            return base.Delete(id);
        }

        public override IEnumerable<ListModel> Get()
        {
            _set = _context.Lists.Where(l => l.OwnerId == _userId || l.Sharing.Any(s => s.UserId == _userId))
                .Include(l => l.Items)
                .Include(l => l.Sharing);

            return base.Get();
        }

        public override ListModel Get(int id, string ifField = "ownerId")
        {
            _set = _context.Lists.Where(l => l.OwnerId == _userId || l.Sharing.Any(s => s.UserId == _userId))
                .Include(l => l.Items)
                .Include(l => l.Sharing);

            return base.Get(id);
        }

        public override bool Update(int id, ListModel item, string idField = "Id")
        {
            _set = _context.Lists.Where(l => l.OwnerId == _userId);
            _updateFields = new List<string> { "Name", "Type" };
            return base.Update(id, item);
        }
    }
}