using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyKoloApi.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string EncryptedPassword { get; set; }
        public List<UserClaim> UserClaims { get; set; }
        public List<Expense> UserExpenses { get; set; }

    }
}
