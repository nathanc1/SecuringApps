using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Member
    {
        [Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string teacherEmail { get; set; }

        public string privateKey { get; set; }

        public string publicKey { get; set; }
    }
}
