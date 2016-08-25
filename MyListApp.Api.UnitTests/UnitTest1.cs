using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyListApp.Api.Controllers;
using MyListApp.Api.Data.Context;
using MyListApp.Api.Data.Entities;
using MyListApp.Api.Services;
using MyListApp.Api.UnitTests.Migrations;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;

/*  Required Tests:
 *  - create user (user1)
 *  - get token for user1
 *  - create a list (list1) owned by user1
 *  - add an item to list2
 *  - create another list (list2) owned by user1
 *  - add 2 items to list2
 *  - get all lists using user1 => 2 lists and that each list has items
 *  - get(list2) => 1 list with proper ID
 *  - user1 delete item from list2
 *  - get list1 => has only 1 item
 *  - get deleted item by ID => 404
 *  - update item in list1 => verify
 *  - create another user (user2)
 *  - get all lists using user2 => no lists
 *  - create a list owned by user2
 *  - get all lists using user2 => 1 list
 *  - user1 invite user2 to share list1
 *  - user2 reject invitation to list1
 *  - get all lists using user2 => 1 list
 *  - user1 invite user2 to share list1
 *  - user2 accept invitation to list1
 *  - get all lists using user2 => 2 list
 *  - get get(list1) => 1 list, proper ID
 *  - user2 add item to list1
 *  - get(list1) => 2 items with 1 item added by user2
 */

namespace MyListApp.Api.UnitTests
{
    [TestClass]
    public class UnitMyTest1
    {
        public UnitMyTest1()
        {
            Database.SetInitializer(new MyDbConfiguration());
        }

        [TestMethod]
        public void ListRepoGetTest()
        {
            // Create ClaimsIdentity needed for ListRepository
            AppDbContext context = new AppDbContext();
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            IdentityUser user = userManager.Find("testuser1", "Abc123!");
            ClaimsIdentity claimsID = userManager.CreateIdentity(user, "password");

            ListModel testValue = new ListModel()
            {
                Id = 1,
                OwnerId = user.Id,
                Name = "MyFirstList",
                Type = ListModel.ListType.ToDo,
                Items =
                    {
                        new ListItemModel
                        {
                            Id = 1,
                            ListId = 1,
                            CreatorId = user.Id,
                            Name = "Do this",
                            Price = 0M,
                            URL = null
                        },
                        new ListItemModel
                        {
                            Id = 2,
                            ListId = 1,
                            CreatorId = user.Id,
                            Name = "Do that",
                            Price = 0M,
                            URL = null
                        }
                    },
                Sharing = new List<ListShareModel>()
            };
            
            // Create instance of ListRepository
            ListRepository listRepo = new ListRepository(claimsID);

            // Call Get from repo
            ListModel result = listRepo.Get(1);

            // Do test
            foreach (string s in new List<string> { "Id", "OwnerId", "Name", "Type"})
            {
                Assert.AreEqual(result.GetType().GetProperty(s).GetValue(result, null),
                    testValue.GetType().GetProperty(s).GetValue(testValue, null));
            }
            Assert.AreEqual(result.Items.Count, testValue.Items.Count);
        }

        [TestMethod]
        public void ListControllerGetTest()
        {
            // Create UserManager
            AppDbContext context = new AppDbContext();
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));

            // Get IdentityUer
            IdentityUser user = userManager.Find("testuser1", "Abc123!");

            // Create ClaimsIdentity needed to create ClaimsPrincipal
            ClaimsIdentity claimsID = userManager.CreateIdentity(user, "password");

            // Create ClaimsPrincipal needed to set User prop in the controller
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsID);

            // Set currentPrincipal which defines controller User prop
            Thread.CurrentPrincipal = claimsPrincipal;

            // Create instance of the controller
            ListController controller = new ListController();

            // Call the controller Get method
            var result = controller.Get();

            // Do a test
            Assert.IsInstanceOfType(result, typeof(IHttpActionResult));
        }
    }
}
