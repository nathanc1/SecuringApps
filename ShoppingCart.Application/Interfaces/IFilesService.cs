using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IFilesService
    {
        void AddFile(FileViewModel model);

        IQueryable<FileViewModel> GetFiles(string email);

        FileViewModel GetFile(Guid id);
    }
}
