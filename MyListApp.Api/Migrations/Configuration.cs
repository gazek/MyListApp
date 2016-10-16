namespace MyListApp.Api.Migrations
{
    using Data.Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.Context.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.Context.AppDbContext context)
        {
            var _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            var user1 = new IdentityUser
            {
                UserName = "Greg",
                Email = "greg@email.com"
            };
            _userManager.Create(user1, "Abc123!");

            var user2 = new IdentityUser
            {
                UserName = "otherguy",
                Email = "other@email.com"
            };
            _userManager.Create(user2, "Abc123!");

            var userId1 = context.Users.Where(u => u.UserName == "Greg").FirstOrDefault().Id;
            var userId2 = context.Users.Where(u => u.UserName == "otherguy").FirstOrDefault().Id;
            
            var lists = new List<ListModel>
            {
                new ListModel
                {
                    Name = "MyFirstList",
                    OwnerId = userId1,
                    Type = ListModel.ListType.ToDo,
                    Items = new List<ListItemModel>
                    {
                        new ListItemModel
                        {
                            Name = "Do this",
                            CreatorId = userId1
                        },
                        new ListItemModel
                        {
                            Name = "Do that",
                            CreatorId = userId1
                        }
                    }
                },
                
                new ListModel
                {
                    Name = "MysecondList",
                    OwnerId = userId1,
                    Type = ListModel.ListType.ToBuy,
                    Items = new List<ListItemModel>
                    {
                        new ListItemModel
                        {
                            Name = "Buy this",
                            CreatorId = userId1,
                            Price = 1.99M
                        },
                        new ListItemModel
                        {
                            Name = "Buy that",
                            CreatorId = userId1,
                            Price = 29.99M
                        }
                    }
                },

                new ListModel
                {
                    Name = "My Shared List",
                    OwnerId = userId1,
                    Type = ListModel.ListType.ToBuy,
                    Items = new List<ListItemModel>
                    {
                        new ListItemModel
                        {
                            Name = "Buy this",
                            CreatorId = userId1,
                            Price = 1.99M
                        },
                        new ListItemModel
                        {
                            Name = "Buy that",
                            CreatorId = userId1,
                            Price = 29.99M
                        }
                    },
                    Sharing = new List<ListShareModel>
                    {
                        new ListShareModel
                        {
                            UserId = userId2,
                        }
                    }
                },

                new ListModel
                {
                    Name = "MyFirstList",
                    OwnerId = userId2,
                    Type = ListModel.ListType.ToDo,
                    Items = new List<ListItemModel>
                    {
                        new ListItemModel
                        {
                            Name = "Do the other thing",
                            CreatorId = userId2
                        },
                        new ListItemModel
                        {
                            Name = "Do some stuff",
                            CreatorId = userId2
                        }
                    }
                }
            };

            lists.ForEach(x => context.Lists.AddOrUpdate(n => n.Name, x));
            context.SaveChanges();
        }
    }
}
