﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Entities;
using AspNetCoreMvc3.Introduction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreMvc3.Introduction.Controllers
{
    public class EmployeeController : Controller
    {
        private ICalculator _calculator;

        public EmployeeController(ICalculator calculator)
        {
            _calculator = calculator;
        }

        public IActionResult Add()
        {
            var employeeAddViewModel = new EmployeeAddViewModel
            {
                Employee = new Employee(),
                Cities = new List<SelectListItem>
                {
                    new SelectListItem{Text = "İstanbul", Value = "34"},
                    new SelectListItem{Text = "Rize", Value = "53"},
                    new SelectListItem{Text = "Ankara", Value = "6"},
                }
            };
            return View(employeeAddViewModel);
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            return RedirectToAction("Index", "Home");
        }

        public string Calculate()
        {
            return _calculator.Calculate(100).ToString();
        }
    }
}
