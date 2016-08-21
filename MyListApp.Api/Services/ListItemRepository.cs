using Microsoft.AspNet.Identity;
using MyListApp.Api.Data.Context;
using MyListApp.Api.Data.Entities;
using System.Security.Principal;
using System;
using System.Collections.Generic;

namespace MyListApp.Api.Services
{
    public class ListItemRepository : AppRepositoryBase<ListItemModel>
    {
        public ListItemRepository(IIdentity user) : base(user)
        {
        }
    }
}
