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
using WebApplication1.Utility;

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
            try
            {
                var list = _taskService.GetTasks();
                return View(list);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Task list not working" + ex);
                return RedirectToAction("Error", "home");
            }
       
        }

        [HttpGet]
        [Authorize(Roles = "teacher")]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Task creation page not working" + ex);
                return RedirectToAction("Error", "home");
            }
        }
        [HttpPost]
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskViewModel data)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation("Task creation not working" + ex);
                return RedirectToAction("Error", "home");
            }
        }

        public IActionResult ViewSubmission(string id)
        {
            try
            {

                var cipher = Encryption.SymmetricDecrypt(id);

                Guid val = Guid.Parse(cipher);

                var myTask = _taskService.GetTask(val);
                return View(myTask);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "home");
            }

        }
    }
}
