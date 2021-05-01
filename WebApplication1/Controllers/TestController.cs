using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var asymmetricKeys = Encryption.GenerateAsymmetricKeys();
            //signing of data
            string originaldata = "hello world";
            MemoryStream msIn = new MemoryStream(Encoding.UTF32.GetBytes(originaldata));
            msIn.Position = 0;

            string signature = Encryption.SignData(msIn, asymmetricKeys.PrivateKey);

            originaldata = "Hello world";

            MemoryStream msIn2 = new MemoryStream(Encoding.UTF32.GetBytes(originaldata));
            msIn2.Position = 0;

            bool result = Encryption.VerifyData(msIn, asymmetricKeys.PublicKey, signature);


           // string cipher = Encryption.SymmetricEncrypt("Hello World");

          //  string originalString = Encryption.SymmetricDecrypt(cipher);


           // Encryption.AsymmetricEncrypt("")

            return View();
        }
    }
}
