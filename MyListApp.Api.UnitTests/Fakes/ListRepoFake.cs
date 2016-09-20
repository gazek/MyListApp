using MyListApp.Api.Services;
using System;
using System.Collections.Generic;
using MyListApp.Api.Data.Entities;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace MyListApp.Api.UnitTests.Fakes
{
    class ListRepoFake : IListRepository
    {
        protected IIdentity _user;
        protected string _userId;
        private List<ListModel> _data;

        public IIdentity User
        {
            set
            {
                _user = value;
                _userId = _user.GetUserId();
            }
        }

        public ListRepoFake()
        {
            CreateFakeData();
        }

        public ListModel Add(ListModel item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ListModel> Get(string userIdField = "ownerId")
        {
            return _data;
        }

        public ListModel Get(int id)
        {
            return _data.Where(l => l.Id == id).SingleOrDefault();
        }

        public bool Update(int id, ListModel item)
        {
            throw new NotImplementedException();
        }

        private void CreateFakeData()
        {
            List<ListModel> lists = new List<ListModel>();

            // make up some  data
            ListModel list1 = new ListModel()
            {
                Id = 1,
                OwnerId = _userId,
                Name = "List1",
                Type = ListModel.ListType.ToBuy,
                Items = new List<ListItemModel>(),
                ShowCompletedItems = false,
                Position = 1,
                Sharing = new List<ListShareModel>()
            };

            ListModel list2 = new ListModel()
            {
                Id = 1,
                OwnerId = "not a valid user",
                Name = "List2",
                Type = ListModel.ListType.ToBuy,
                Items = new List<ListItemModel>(),
                ShowCompletedItems = false,
                Position = 2,
                Sharing = new List<ListShareModel>()
            };

            ListModel list3 = new ListModel()
            {
                Id = 1,
                OwnerId = _userId,
                Name = "List3",
                Type = ListModel.ListType.ToBuy,
                Items = new List<ListItemModel>(),
                ShowCompletedItems = false,
                Position = 3,
                Sharing = new List<ListShareModel>()
            };

            // add the data to the list
            lists.Add(list1);
            lists.Add(list2);
            lists.Add(list3);

            _data = lists;
        }
    }
}
