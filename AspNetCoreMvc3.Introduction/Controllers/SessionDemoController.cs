using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Entities;
using AspNetCoreMvc3.Introduction.ExtensionMethods;
using AspNetCoreMvc3.Introduction.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc3.Introduction.Controllers
{
    public class SessionDemoController : Controller
    {
        public string Index()
        {
            HttpContext.Session.SetInt32("age", 24);
            HttpContext.Session.SetString("name", "Muhammet");
            HttpContext.Session.SetObject("student", new Student
            {
                FirstName = "Furkan",
                LastName = "Kasarcı",
                Email = "fk@gmail.com"
            });
            return "Session Created";
        }

        [HandleException(ViewName = "Error", ExceptionType = typeof(ArgumentNullException))]
        public string GetSessions()
        {
            Student student = HttpContext.Session.GetObject<Student>("student");
            return string.Format("Hello {0}, you are {1} years old!{2}Hello Stundent {3} {4}! Your E-mail address is: {5}", HttpContext.Session.GetString("name"), HttpContext.Session.GetInt32("age"),
                Environment.NewLine,
                student.FirstName,
                student.LastName,
                student.Email
            );
        }
    }
}
