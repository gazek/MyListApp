using MyListApp.Api.Data.Context;
using MyListApp.Api.Data.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;

namespace MyListApp.Api.Services
{

    public class ListRepository : AppRepositoryBase<ListModel>, IListRepository
    {
        public ListRepository() : base()
        {
        }
        /*
        public ListRepository(IIdentity user) : base(user)
        {
        }
        */
        public override ListModel Add(ListModel item)
        {
            item.OwnerId = _userId;
            return base.Add(item);
        }

        public override ListModel Get(int id)
        {
            var result = _context.Lists.Where(l => l.Id == id).Include("Items").Include("Sharing").FirstOrDefault();
            result.Items = result.Items.OrderBy(i => i.Position).ToList();
            return result;
        }

        public override IEnumerable<ListModel> Get(string userIdField = "ownerId")
        {
            // find all lists owned by the current user and all lists that have
            // a sharing record referencing the current user
            var result = _context.Lists.Where(l => l.OwnerId == _userId || l.Sharing.Where(s => s.UserId == _userId).FirstOrDefault() != null)
                .Include(l => l.Items)
                .Include(l => l.Sharing)
                .OrderBy(l => l.Position)
                .ToList();

            // Not sure if I can do this through linq
            foreach (var l in result)
            {
                l.Items = l.Items.OrderBy(i => i.Position).ToList();
            }

            return result;    
        }

        public override bool Update(int id, ListModel item)
        {
            _updateFields = new List<string> { "Name", "Type", "ShowCompletedItems", "Position" };
            return base.Update(id, item);
        }
    }
}