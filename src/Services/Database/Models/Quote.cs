using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AivaptDotNet.Services.Database.Models
{
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
