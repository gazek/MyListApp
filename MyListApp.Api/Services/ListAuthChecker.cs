using Microsoft.AspNet.Identity;
using MyListApp.Api.Data.Context;
using MyListApp.Api.Data.Entities;
using System;
using System.Linq;
using System.Security.Principal;

namespace MyListApp.Api.Services
{
    public class ListAuthChecker : IDisposable
    {
        private AppDbContext _context;
        private bool disposedValue = false;
        private IIdentity _user;
        private string _userId;

        public ListAuthChecker(IIdentity user)
        {
            _user = user;
            _userId = user.GetUserId();
            _context = new AppDbContext();
        }

        public bool HasListAccessByListId (int id)
        {
            // get ListModel
            ListModel list = _context.Lists.Find(id);

            // if list is not found return faLSE
            if (list == null)
            {
                return false;
            }

            // Check is current user is owner
            if (list.OwnerId == _userId)
            {
                return true;
            }

            // check is current user is in Sharing list
            ListShareModel share = list.Sharing.Where(s => s.UserId == _userId).FirstOrDefault();
            if (share != null)
            {
                return true;
            }


            return false;
        }

        public bool HasListAccessByItemId (int id)
        {
            ListModel parentList = _context.Lists.Where(l => l.Items.Any(i => i.Id == id)).FirstOrDefault();

            if (parentList == null)
            {
                return false;
            }
            else
            {
                return this.HasListAccessByListId(parentList.Id);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
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