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
    public class CommentsService : ICommentsService
    {
        private ICommentsRepository _commentsRepository;
        private IMapper _autoMapper;
        public CommentsService(ICommentsRepository categoriesRepo, IMapper autoMapper)
        {
            _commentsRepository = categoriesRepo;
            _autoMapper = autoMapper;
        }

        public void AddComment(CommentViewModel model, DateTime myDate)
        {
            var comment = _autoMapper.Map<Comment>(model);
            comment.date = myDate;
            _commentsRepository.AddComment(comment);

        }

        public void DeleteComment(Guid id)
        {
            if (_commentsRepository.GetComment(id) != null)
            {
                _commentsRepository.DeleteComment(id);
            }
        }

        public CommentViewModel GetComment(Guid id)
        {
            var c = _commentsRepository.GetComment(id);
            if (c == null) return null;
            else
            {
                var result = _autoMapper.Map<CommentViewModel>(c);
                return result;
            }
        }

        public IQueryable<CommentViewModel> GetComments(Guid id)
        {
            return _commentsRepository.GetComments().Where(c => c.Files.Id == id).ProjectTo<CommentViewModel>(_autoMapper.ConfigurationProvider);
        }
    }
}
