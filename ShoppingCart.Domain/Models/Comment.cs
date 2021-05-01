using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public string commentDetails { get; set; }

        public DateTime date { get; set; }

        public virtual Files Files { get; set; }
       
        [ForeignKey("Files")]
        public Guid FileId { get; set; }
    }
}
