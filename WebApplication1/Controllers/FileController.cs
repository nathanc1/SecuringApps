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
using WebApplication1.ActionFilters;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class FileController : Controller
    {
        private readonly IFilesService _fileSerive;
        private readonly ITasksService _tasksService;
        private readonly IMembersService _memberService;
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<FileController> _logger;
        public FileController(IFilesService filesService, ITasksService tasksService, IWebHostEnvironment host, ILogger<FileController> logger, IMembersService membersService)
        {
            _logger = logger;
            _host = host;
            _fileSerive = filesService;
            _tasksService = tasksService;
            _memberService = membersService;

        }
        [StudentFile]
        public IActionResult Index()
        {
            try
            {
                string email = User.Identity.Name;
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                _logger.LogInformation("Current user accessing files section: " + remoteIpAddress + " TimeStamp: " + System.DateTime.Now + "User: " + email);
                var filesList = _fileSerive.GetFiles(email);
                return View(filesList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("File list not working" + ex);
                return RedirectToAction("Error", "home");
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("File creation page not working" + ex);
                return RedirectToAction("Error", "home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(IFormFile file, FileViewModel data,Guid id)
        {
            try
            {
                data.task = _tasksService.GetTask(id);

                if (data.task.deadline > DateTime.Now)
                {
                    if (ModelState.IsValid)
                    {
                        string uniqueFilename;
                        if (System.IO.Path.GetExtension(file.FileName) == ".pdf" && file.Length < 1048576)
                        {
                            //137 80 78 71 13 10 26 10
                            byte[] whiteList = new byte[] { 37, 80, 68, 70 };
                            if (file != null)
                            {
                                MemoryStream msIn = new MemoryStream();
                                using (var f = file.OpenReadStream())
                                {
                                    f.Position = 0;
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

                                    // f.CopyTo(msIn); //hybird encrypt

                                    //uploading the file
                                    //correctness
                                    uniqueFilename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                                    data.file = uniqueFilename;

                                    string absolutePath = @"ValuableFiles\" + uniqueFilename;
                                    try
                                    {

                                        var member = _memberService.GetMember(User.Identity.Name);

                                        file.CopyTo(msIn);
                                        var encryptedData = Encryption.HybridEncrypt(msIn, member.publicKey);
                                        System.IO.File.WriteAllBytes(absolutePath, encryptedData.ToArray());

                                        data.signature = Encryption.SignData(encryptedData, member.privateKey);

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
                        //once the file has been inserted successfully in the db

                        data.email = HttpContext.User.Identity.Name; //this is the currently logged in user

                        _fileSerive.AddFile(data);

                        var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

                        _logger.LogInformation("Current user uploading in files section: " + remoteIpAddress + " TimeStamp: " + System.DateTime.Now + " User: " + data.email + " File Id: " + data.file);


                        TempData["message"] = "File inserted successfully";
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Check your input. Operation failed");
                        return View(data);
                    }
                }
                else
                {
                    TempData["error"] = "File deadline already passes";
                    return View();
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation("File upload not working" + ex);
                return RedirectToAction("Error", "home");
            }


        }

        public IActionResult Download(string id,FileViewModel data)
        {
            try
            {
                string cipher = Encryption.SymmetricDecrypt(id);

                Guid guid = Guid.Parse(cipher);



                var file = _fileSerive.GetFile(guid);

                string absolutePath = @"ValuableFiles\" + file.file;

                FileStream fs = new FileStream(absolutePath, FileMode.Open, FileAccess.Read);

                MemoryStream toDownload = new MemoryStream();



                fs.CopyTo(toDownload);

                string email = file.email;

                var member = _memberService.GetMember(email);

                bool verifyData = Encryption.VerifyData(toDownload, member.publicKey, file.signature);

                if (verifyData)
                {
                    MemoryStream fileContent = Encryption.HybridDecrypt(toDownload, member.privateKey);

                    return File(fileContent, "application/ocet-stream", Guid.NewGuid() + ".pdf");

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("File download not working" + ex);
                return RedirectToAction("Error", "home");
            }

        }
    }
}
