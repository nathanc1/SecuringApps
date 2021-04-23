using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IFilesRepository
    {
        void AddFile(Files submission);

        IQueryable<Files> GetFiles();

        Files GetFile(Guid id);
    }
}
