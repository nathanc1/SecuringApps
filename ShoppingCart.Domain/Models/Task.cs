using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Task
    {
        [Key]
        public Guid id { get; set; }

        public string descrption { get; set; }

        public DateTime deadline { get; set; }

        public string email { get; set; }
    }
}
