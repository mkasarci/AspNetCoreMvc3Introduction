using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc3.Introduction.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [Route("")] //Assigned as a default route.
        [Route("save")]
        [Route("~/save")]
        public string Save()
        {
            return "Saved";
        }

        [Route("delete")]
        public string Delete()
        {
            return "Deleted";
        }

        [Route("update")]
        public string Update()
        {
            return "Updated";
        }
    }
}
