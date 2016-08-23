using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyListApp.Api.Controllers;
using MyListApp.Api.Data.Context;
using MyListApp.Api.Data.Entities;
using MyListApp.Api.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace MyListApp.Api.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            UserManager<IdentityUser> _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new AppDbContext()));
            
            AuthRepository authRepo = new AuthRepository();
            await authRepo.RegisterUser(new RegisterModel { UserName = "tester", Password = "Abc123!", ConfirmPassword = "Abc123!", EmailAddress = "foo@bar.com"});
            IdentityUser user = await authRepo.FindUser("tester", "Abc123!");

            //Thread.CurrentPrincipal = new ClaimsPrincipal().AddIdentity(user);
            ListController controller = new ListController();
            IHttpActionResult result = controller.Get();
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IEnumerable<ListModel>>)); 
        }
    }
}
