using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.ActionFilters;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentsService _commentsService;
        private readonly IFilesService _filesService;
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<CommentController> _logger;
        public CommentController(ICommentsService comments, IWebHostEnvironment host, ILogger<CommentController> logger, IFilesService files)
        {
            _commentsService = comments;
            _host = host;
            _logger = logger;
            _filesService = files;
        }
        public IActionResult Index(string id)
        {
            try
            {
                var cipher = Encryption.SymmetricDecrypt(id);

                Guid val = Guid.Parse(cipher);

                string email = User.Identity.Name;

                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

                _logger.LogInformation("Current user accessing cooments section: " + remoteIpAddress + " TimeStamp: " + System.DateTime.Now + " User: " + email);

                var list = _commentsService.GetComments(val);
                return View(list);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Comments list not working" + ex);
                return RedirectToAction("Error", "home");
            }
        }
        [HttpGet]
        public IActionResult Create(Guid id)
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "home");
            }
        }

        [HttpPost]
        [StudentComment]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommentViewModel data, string id, DateTime myDate)
        {
            try
            {
                string email = User.Identity.Name;
                var cipher = Encryption.SymmetricDecrypt(id);

                Guid val = Guid.Parse(cipher);

                var comments = _commentsService.GetComments(val);
                myDate = System.DateTime.Now;
                ViewBag.Comments = comments;

                var allErrors = ModelState.Values.SelectMany(x => x.Errors);

                data.file = _filesService.GetFile(val);


                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

                _logger.LogInformation("Current user uploading in comments section: " + remoteIpAddress + " TimeStamp: " + System.DateTime.Now + " User: " + email + " Comment Details: " + data.commentDetails);


                _commentsService.AddComment(data, myDate);

                TempData["Message"] = "Comment inserted successfuly";
                return View();
            } catch (Exception)
            {
                return RedirectToAction("Error", "home");
            }
            
            
            
            
        }
    }
}
