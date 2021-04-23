using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class TaskViewModel
    {
        public Guid id { get; set; }
        public string descrption { get; set; }
        public DateTime deadline { get; set; }
        public string email { get; set; }
    }
}
