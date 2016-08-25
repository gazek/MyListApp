using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyListApp.Api.Data.Context;
using MyListApp.Api.Data.Entities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyListApp.Api.Services
{
    public class AuthRepository : IDisposable
    {
        private AppDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private bool disposedValue = false;

        public AuthRepository()
        {
            _context = new AppDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_context));
        }

        public async Task<IdentityResult> RegisterUser(RegisterModel reg)
        {
            var user = new IdentityUser
            {
                UserName = reg.UserName,
                Email = reg.EmailAddress
            };

            var result = await _userManager.CreateAsync(user, reg.Password);

            return result;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IdentityUser userModel, string authenticationType)
        {
            var userIdentity = await _userManager.CreateIdentityAsync(userModel, authenticationType);
            return userIdentity;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public string GetUserIdByUsername(string username)
        {
            IdentityUser user = _userManager.FindByName(username);
            
            if (user == null)
            {
                return "";

            }

            return user.Id;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _userManager.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }



    }
}