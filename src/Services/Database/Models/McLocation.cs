using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AivaptDotNet.Services.Database.Models
{
    [Table("mc_location")]
    public class McLocation
    {
        [Key]
        public uint LocationId { get; set; }

        [Required]
        public string LocationName { get; set; }

        public List<McCoordinates> LinkedMcCoordinates { get; set; }

    }
}
