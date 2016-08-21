using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyListApp.Api.Data.Entities
{
    /*
     * This entity grants access a user other than the list owner to:
     * - view the list
     * - add/modify/delete items associated with the list
     * */
    public class ListShareModel
    {
        // Access item ID
        [Required]
        public int Id { get; set; }

        // ID of List
        [Required]
        public int ListId { get; set; }

        // User whose access this describes
        [Required]
        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual IdentityUser User { get; set; }

        [ForeignKey("ListId")]
        [JsonIgnore]
        public virtual ListModel List { get; set; }
    }
}