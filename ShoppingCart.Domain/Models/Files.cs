using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Files
    {
        [Key]
        public Guid Id { get; set; }

        public string email { get; set; }

        public string file { get; set; }

        public virtual Task task { get; set; }

        [ForeignKey("Task")]
        public Guid TaskId { get; set; }


         
    }
}
