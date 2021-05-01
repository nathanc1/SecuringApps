using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string commentDetails { get; set; }

        public DateTime date { get; set; } 

        public FileViewModel file { get; set; }
    }
}
