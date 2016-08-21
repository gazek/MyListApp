using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyListApp.Api.Data.Entities
{
    /*
     * This entity offers to grant access a user other than the list owner to:
     * - view the list
     * - add/modify/delete items associated with the list
     * */
    public class InvitationModel
    {
        // invitation ID
        [Required]
        public int Id { get; set; }

        // userId of user issuing invitation
        [Required]
        public string InvitorId { get; set; }

        // userId of user receiving invitation
        [Required]
        public string InviteeId { get; set; }

        // ID of list
        [Required]
        public int ListId { get; set; }

        // Status of invitation offer
        [Required]
        public StatusType Status { get; set; }

        [ForeignKey("ListId")]
        [JsonIgnore]
        public virtual ListModel List { get; set; }

        [ForeignKey("InvitorId")]
        [JsonIgnore]
        public virtual IdentityUser Invitor { get; set; }

        [ForeignKey("InviteeId")]
        [JsonIgnore]
        public virtual IdentityUser Invitee { get; set; }

        // enum of invitation status
        public enum StatusType
        {
            Open,
            Accepted,
            Rejected
        }
    }
}