using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MultiworkTest.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MultiworkTest.Controllers
{
    public class HomeController : Controller
    {
        IWebHostEnvironment _appEnvironment;

        public HomeController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FileView(FileViewData data)
        {
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, string description)
        {
            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileViewData data = new FileViewData
                {
                    Path = path,
                    Description = description
                };

                return RedirectToAction("FileView", data);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
