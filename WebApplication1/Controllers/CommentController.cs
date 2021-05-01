using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

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
        public IActionResult Index(Guid id)
        {
            var list = _commentsService.GetComments(id);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create(Guid id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CommentViewModel data, Guid id, DateTime myDate)
        {
            var comments = _commentsService.GetComments(id);
            myDate = System.DateTime.Now;
            ViewBag.Comments = comments;


            if (ModelState.IsValid)
            {
                data.file = _filesService.GetFile(id);
                

                _commentsService.AddComment(data,myDate);

                TempData["Message"] = "Comment inserted successfuly";
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Error");
                return View(data);

            }
        }
    }
}
