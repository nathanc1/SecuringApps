using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingCart.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Utility;

namespace WebApplication1.ActionFilters
{
    public class StudentComment : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var id = new Guid(Encryption.SymmetricDecrypt(context.ActionArguments["id"].ToString()));

                var currentLoggedInUser = context.HttpContext.User.Identity.Name;

                IFilesService filesService = (IFilesService)context.HttpContext.RequestServices.GetService(typeof(IFilesService));
                var file = filesService.GetFile(id);
                if (file.email != currentLoggedInUser && file.task.email != currentLoggedInUser)
                {
                    context.Result = new UnauthorizedObjectResult("Access Denied");
                }
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult("Bad Request");
            }
        }
    }
}
