using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyListApp.Api.Data.Entities
{
    public class ListItemModel
    {
        // List item ID
        [Required]
        public int Id { get; set; }

        // ID of List
        [Required]
        public int ListId { get; set; }

        // List creator userId
        [Required]
        public string CreatorId { get; set; }

        // name of item
        [Required]
        public string Name { get; set; }

        // item cost
        // used for ToBuy list type
        public decimal Price { get; set; }

        // URL of ToBuy list item
        [Url]
        [DataType(DataType.Url)]
        public string URL { get; set; }

        [ForeignKey("CreatorId")]
        [JsonIgnore]
        public virtual IdentityUser Creator { get; set; }

        [ForeignKey("ListId")]
        [JsonIgnore]
        public virtual ListModel List { get; set; }

        public ListItemModel()
        {
            Price = 0M;
    }
    }
}