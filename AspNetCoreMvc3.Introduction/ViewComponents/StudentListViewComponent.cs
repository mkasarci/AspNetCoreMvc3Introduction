using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AspNetCoreMvc3.Introduction.ViewComponents
{
    public class StudentListViewComponent : ViewComponent
    {
        private readonly SchoolContext _context;

        public StudentListViewComponent(SchoolContext context)
        {
            _context = context;
        }

        public ViewViewComponentResult Invoke(string filter)
        {
            filter = HttpContext.Request.Query["filter"]; // we can read query strings with that way; 
            return View(new StudentListViewModel
            {
                Students = string.IsNullOrEmpty(filter) 
                    ? _context.Students.ToList() 
                    : _context.Students.Where(s => s.FirstName.ToLower().Contains(filter.ToLower())).ToList()
            }); 
        }
    }
}
