using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyListApp.Api.Data.Entities
{
    public class ListModel
    {
        // App list ID
        [Required]
        [Display(Name = "List ID")]
        public int Id { get; set; }

        // ID of list owner
        [Required]
        [Display(Name = "List Owner ID")]
        public string OwnerId { get; set; }

        // name of list
        [Required]
        [Display(Name = "List Name")]
        public string Name { get; set; }

        // Type of list ToDo or ToBuy
        [Required]
        [Display(Name = "List Type")]
        public ListType Type { get; set; }

        // Collection of items
        [Display(Name = "List Items")]
        public ICollection<ListItemModel> Items { get; set; }

        // Should GUI show of hide completed items
        public bool ShowCompletedItems { get; set; }

        // position - sort param for list display
        [Required]
        public int Position { get; set; }

        // List access info
        [Display(Name = "Sharing Info")]
        public ICollection<ListShareModel> Sharing { get; set; }

        // force foreign key
        [ForeignKey("OwnerId")]
        [JsonIgnore]
        public virtual IdentityUser Owner { get; set; }

        // list type enum
        public enum ListType
        {
            ToDo,
            ToBuy
        }

        public ListModel()
        {
            Items = new List<ListItemModel>();
            Sharing = new List<ListShareModel>();
        }
    }
}