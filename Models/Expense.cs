using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyKoloApi.Models
{
    public class Expense
    {

        public string Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage ="Description cannot exceed 500 characters")]
        [MinLength(10, ErrorMessage ="Cannot have less than 10 characters")]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

    }
}
