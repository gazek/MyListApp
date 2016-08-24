using MyListApp.Api.Data.Entities;
using System.Collections.Generic;
using System.Security.Principal;
using static MyListApp.Api.Data.Entities.InvitationModel;

namespace MyListApp.Api.Services
{
    public class InvitationRepository : AppRepositoryBase<InvitationModel>
    {
        public InvitationRepository(IIdentity user) : base(user)
        {
        }

        // TODO: write GetToMe() and GetToMe(id) so that they only return invitations with status of open

        public override InvitationModel Add(InvitationModel item)
        {
            item.InvitorId = _userId;
            return base.Add(item);
        }

        public override bool Update(int id, InvitationModel item)
        {
            // set update fields
            _updateFields = new List<string> { "Status" };

            // get current status in case we need to roll back
            StatusType originalStatus = this.Get(id).Status;

            // update invitation status
            bool result = base.Update(id, item);

            // check result
            if (!result)
            {
                return false;
            }

            if (item.Status == StatusType.Rejected)
            {
                return true;
            }

            // create listshare item
            ListShareModel shareItem = new ListShareModel
            {
                ListId = item.ListId,
                UserId = item.InviteeId
            };

            // result for share item add
            ListShareModel shareAddResult;

            // add ListShare record
            using (ListShareRepository _shareRepo = new ListShareRepository(_user))
            {
                shareAddResult = _shareRepo.Add(shareItem);
            }

            // make sure share record was created
            if (shareAddResult == null)
            {
                item.Status = originalStatus;
                base.Update(id, item);
                return false;
            }

            return true;
        }
    }
}