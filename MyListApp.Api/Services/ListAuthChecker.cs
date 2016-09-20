using Microsoft.AspNet.Identity;
using MyListApp.Api.Data.Context;
using MyListApp.Api.Data.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;

namespace MyListApp.Api.Services
{
    public class ListAuthChecker : IDisposable
    {
        private IAppDbContext _context;
        private bool disposedValue = false;
        private IIdentity _user;
        private string _userId;

        public IIdentity User
        {
            set
            {
                _user = value;
                _userId = _user.GetUserId();
            }
        }

        public ListAuthChecker()
        {
            _context = new AppDbContext();
        }

        public ListAuthChecker(IAppDbContext context)
        {
            _context = context;
        }

        public bool HasListAccessByListId (int id)
        {
            // get ListModel
            ListModel list = _context.Lists.Where(l => l.Id == id).Include(l => l.Sharing).SingleOrDefault();

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

        public bool IsListOwnerByShareId (int id)
        {
            // Find share item
            ListShareModel shareItem = _context.Set<ListShareModel>().Find(id);

            if (shareItem == null)
            {
                return false;
            }

            // Find parent list
            ListModel listItem = _context.Lists.Find(shareItem.Id);

            // if list is found check if owner is the current user
            if (listItem != null && listItem.OwnerId == _userId)
            {
                return true;
            }

            return false;
        }

        public bool IsListOwnerByListId(int id)
        {
            // Find list
            ListModel listItem = _context.Lists.Find(id);

            // if list is found check if owner is the current user
            if (listItem != null && listItem.OwnerId == _userId)
            {
                return true;
            }

            return false;
        }

        public bool IsInvitationSenderByInvitationId(int id)
        {
            InvitationModel invitation = _context.Set<InvitationModel>().Find(id);

            if (invitation != null && invitation.InvitorId == _userId)
            {
                return true;
            }

            return false;
        }

        public bool IsInvitationReceiverByInvitationId(int id)
        {
            InvitationModel invitation = _context.Set<InvitationModel>().Find(id);

            if (invitation != null && invitation.InviteeId == _userId)
            {
                return true;
            }

            return false;
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