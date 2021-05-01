using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ICommentsService
    {
        void AddComment(CommentViewModel model,DateTime myDate);

        IQueryable<CommentViewModel> GetComments(Guid id);

        CommentViewModel GetComment(Guid id);

        void DeleteComment(Guid id);
    }
}
