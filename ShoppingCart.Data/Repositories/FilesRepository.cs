using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        ShoppingCartDbContext _context;
        public FilesRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }
        public void AddFile(Files submission)
        {
            submission.Id = Guid.NewGuid();
            _context.Files.Add(submission);
            _context.SaveChanges();

        }

        public Files GetFile(Guid id)
        {
            return _context.Files.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Files> GetFiles()
        {
            return _context.Files;
        }
    }
}
