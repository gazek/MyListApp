using MyListApp.Api.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using static MyListApp.Api.Data.Entities.InvitationModel;

namespace MyListApp.Api.Services
{
    // TODO: clean up the update method
    public class InvitationRepository : AppRepositoryBase<InvitationModel>
    {
        public InvitationRepository(IIdentity user) : base(user)
        {
        }

        public override IEnumerable<InvitationModel> Get(string userIdField = "userId")
        {
            // get all invitations based on userIdField
            IEnumerable<InvitationModel> result = base.Get(userIdField);

            // Only return open invitations to invitation recipient
            if (userIdField == "InviteeId")
            {
                return result.Where(i => i.Status == StatusType.Open);
            }
            else
            {
                return result;
            }
        }

        public override InvitationModel Add(InvitationModel item)
        {
            item.InvitorId = _userId;
            return base.Add(item);
        }

        public override bool Update(int id, InvitationModel item)
        {
            // set update fields
            _updateFields = new List<string> { "Status" };

            // update invitation status
            bool result = base.Update(id, item);

            // check result
            if (!result)
            {
                return false;
            }


            if (item.Status == StatusType.Accepted)
            {
                AddShareRecord(id, item);
            }

            return true;
        }

        protected void AddShareRecord(int id, InvitationModel item)
        {
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
        }
    }
}