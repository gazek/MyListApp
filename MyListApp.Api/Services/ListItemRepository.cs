using MyListApp.Api.Data.Entities;
using System.Security.Principal;
using System.Collections.Generic;

namespace MyListApp.Api.Services
{
    public class ListItemRepository : AppRepositoryBase<ListItemModel>
    {
        public ListItemRepository() : base()
        {
        }

        public override ListItemModel Add(ListItemModel item)
        {
            item.CreatorId = _userId;
            return base.Add(item);
        }

        public override bool Update(int id, ListItemModel item)
        {
            // set update fields
            _updateFields = new List<string> { "Name", "Price", "URL", "Position", "IsComplete"};
            return base.Update(id, item);
        }
    }
}
