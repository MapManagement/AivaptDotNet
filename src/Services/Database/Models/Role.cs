using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AivaptDotNet.Services.Database.Models
{
    [Table("role")]
    public class Role
    {
        [Key]
        public ulong RoleId { get; set; }

        [Key]
        public ulong GuildId { get; set; }

        [Required]
        public bool ModPermissions { get; set; }
    }
}
