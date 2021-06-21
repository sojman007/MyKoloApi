using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyKoloApi.Models
{
    public class UserClaim
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public  User User { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }

    }
}
