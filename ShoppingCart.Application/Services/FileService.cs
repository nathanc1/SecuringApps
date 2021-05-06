using AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Application.Services
{

    public class FileService : IFilesService
    {
        private IFilesRepository _filesRepo;
        private IMapper _autoMapper;
        public FileService(IFilesRepository filesRepo, IMapper autoMapper)
        {
            _filesRepo = filesRepo;
            _autoMapper = autoMapper;
        }
        public void AddFile(FileViewModel model)
        {
            var file = _autoMapper.Map<Files>(model);
            file.TaskId = file.task.id;
            file.task = null;

            _filesRepo.AddFile(file);
        }

        public IQueryable<FileViewModel> GetFiles(string email)
        {
            return _filesRepo.GetFiles().Where(f => f.email == email || f.task.email == email).ProjectTo<FileViewModel>(_autoMapper.ConfigurationProvider);
        }

        public FileViewModel GetFile(Guid id)
        {
            var f = _filesRepo.GetFile(id);
            if (f == null) return null;
            else
            {
                var result = _autoMapper.Map<FileViewModel>(f);
                return result;
            }
        }

 
    }
}
