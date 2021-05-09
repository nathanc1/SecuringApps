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
    public class StudentFile : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var currentLoggedInUser = context.HttpContext.User.Identity.Name;

                IFilesService filesService = (IFilesService)context.HttpContext.RequestServices.GetService(typeof(IFilesService));
                var file = filesService.GetFiles(currentLoggedInUser);

                foreach (var sub in file)
                {
                    if (sub.email != currentLoggedInUser)
                    {
                        context.Result = new UnauthorizedObjectResult("Access Denied");
                    }
                }
              
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult("Bad Request");
            }
        }
    }
}
