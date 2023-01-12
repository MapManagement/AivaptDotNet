using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AivaptDotNet.Services.Database
{
    [Table("simple_command")]
    public class SimpleCommand
    {
        [Key]
        public string Name { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public ulong CreatorId { get; set; }

        [Required]
        public string Color { get; set; }
    }

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

    [Table("quote")]
    public class Quote
    {
        [Key]
        public ulong Id { get; set; }

        [Required]
        public ulong UserId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
