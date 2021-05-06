using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace WebApplication1.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITasksService _taskService;
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<ProductsController> _logger;
        public TaskController(ITasksService tasksService, IWebHostEnvironment host, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _host = host;
            _taskService = tasksService;
        }

        public IActionResult Index()
        {
            var list = _taskService.GetTasks();
            return View(list);
        }

        [HttpGet]
        [Authorize(Roles = "teacher")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskViewModel data)
        {
            data.email = User.Identity.Name;

            if (ModelState.IsValid)
            {
                _taskService.AddTask(data);

                TempData["Message"] = "Task inserted successfuly";
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Error");
                return View(data);

            }
        }

        public IActionResult ViewSubmission(Guid id)
        {
            try
            {
                var myTask = _taskService.GetTask(id);
                return View(myTask);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "home");
            }

        }
    }
}
