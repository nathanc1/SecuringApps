using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class TasksService : ITasksService
    {
        private ITaskRepository _taskrepo;
        private IMapper _autoMapper;
        public TasksService(ITaskRepository tasksrepo, IMapper autoMapper)
        {
            _taskrepo = tasksrepo;
            _autoMapper = autoMapper;
        }

        public void AddTask(TaskViewModel model)
        {
            _taskrepo.AddTask(_autoMapper.Map<Task>(model));
        }

        public TaskViewModel GetTask(Guid id)
        {
            var t = _taskrepo.GetTask(id);
            if (t == null) return null;
            else
            {
                var result = _autoMapper.Map<TaskViewModel>(t);
                return result;
            }
        }

        public IQueryable<TaskViewModel> GetTasks()
        {
            return _taskrepo.GetTasks().ProjectTo<TaskViewModel>(_autoMapper.ConfigurationProvider);

        }
    }
}
