using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FileController : Controller
    {
        private readonly IFilesService _fileSerive;
        private readonly ITasksService _tasksService;
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<FileController> _logger;
        public FileController(IFilesService filesService, ITasksService tasksService, IWebHostEnvironment host, ILogger<FileController> logger)
        {
            _logger = logger;
            _host = host;
            _fileSerive = filesService;
            _tasksService = tasksService;

        }

        public IActionResult Index()
        {
            string email = User.Identity.Name;
            var filesList = _fileSerive.GetFiles(email);
            return View(filesList);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Create()
        { 
            return View(); 
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(IFormFile file, FileViewModel data,Guid id)
        {
            data.task = _tasksService.GetTask(id);

            if (ModelState.IsValid)
            {
                string uniqueFilename;
                if (System.IO.Path.GetExtension(file.FileName) == ".pdf" && file.Length < 1048576)
                {
                    //137 80 78 71 13 10 26 10
                    byte[] whiteList = new byte[] { 37, 80, 68, 70 };
                    if (file != null)
                    {
                        using (var f = file.OpenReadStream())
                        {
                            byte[] buffer = new byte[4];
                            f.Read(buffer, 0, 4);

                            for (int i = 0; i < whiteList.Length; i++)
                            {
                                if (whiteList[i] == buffer[i])
                                { }
                                else
                                {
                                    ModelState.AddModelError("file", "file is not valid and accapteable");
                                    return View();
                                }
                            }
                            //...other reading of bytes happening
                            f.Position = 0;

                            //uploading the file
                            //correctness
                            uniqueFilename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            data.file = uniqueFilename;

                            string absolutePath = _host.WebRootPath + @"\pictures\" + uniqueFilename;
                            try
                            {
                                using (FileStream fsOut = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                                {
                                    //throw new Exception();
                                    f.CopyTo(fsOut);
                                }

                                f.Close();
                            }
                            catch (Exception ex)
                            {
                                //log
                                _logger.LogError(ex, "Error happend while saving file");

                                return View("Error", new ErrorViewModel() { Message = "Error while saving the file. Try again later" });
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("file", "File is not valid and acceptable or size is greater than 10Mb");
                    return View();
                }
                //once the product has been inserted successfully in the db

                data.email = HttpContext.User.Identity.Name; //this is the currently logged in user

                _fileSerive.AddFile(data);


                TempData["message"] = "File inserted successfully";
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Check your input. Operation failed");
                return View(data);
            }


        }
    }
}
