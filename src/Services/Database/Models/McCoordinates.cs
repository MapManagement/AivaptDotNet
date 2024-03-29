using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AivaptDotNet.Services.Database.Models
{
    [Table("mc_coordinates")]
    public class McCoordinates
    {
        [Key]
        public int CoordinatesId { get; set; }

        [Required]
        public long X { get; set; }

        public long Y { get; set; }

        [Required]
        public long Z { get; set; }

        public List<McLocation> Locations { get; set; }

        [Required]
        public ulong SubmitterId { get; set; }

        public string Description { get; set; }
    }
}
