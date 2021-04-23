using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class FileViewModel
    {
        public Guid Id { get; set; }

        public string email { get; set; }

        public string file { get; set; }

        public TaskViewModel task { get; set; }
    }
}
