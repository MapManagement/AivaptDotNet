using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AivaptDotNet.Services.Database
{
    [Table("guild")]
    public class Guild
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ulong OwnerId { get; set; }
    }
}
