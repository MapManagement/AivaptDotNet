using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AivaptDotNet.Services.Database.Models
{
    [Table("simple_command")]
    public class SimpleCommand
    {
        [Key]
        public string Name { get; set; }

        public string Text { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public ulong CreatorId { get; set; }

        [Required]
        public string Color { get; set; }
    }
}
