using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICommentsRepository
    {
        void AddComment(Comment submission);

        IQueryable<Comment> GetComments();

        Comment GetComment(Guid id);

        void DeleteComment(Guid id);
    }
}
