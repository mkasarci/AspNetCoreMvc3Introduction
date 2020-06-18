using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc3.Introduction.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")] //We have to type this attribute cause we already have Admin controller.
        public IActionResult Index()
        {
            return View();
        }
    }
}
