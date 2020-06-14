using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Entities;
using AspNetCoreMvc3.Introduction.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc3.Introduction.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee {Id = 1, FirstName = "Muhammet", LastName = "Kasarcı", CityId = 34},
                new Employee {Id = 2, FirstName = "Furkan", LastName = "Kasarcı", CityId = 34}
            };
            List<string> cities = new List<string>{"İstanbul", "Ankara", "Rize"};

            var model = new EmployeeListViewModel
            {
                Employees = employees,
                Cities = cities
            };
            return View(model);
        }
    }
}
