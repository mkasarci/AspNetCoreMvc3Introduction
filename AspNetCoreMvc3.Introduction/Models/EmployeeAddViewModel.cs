using System.Collections.Generic;
using AspNetCoreMvc3.Introduction.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreMvc3.Introduction
{
    public class EmployeeAddViewModel
    {
        public Employee Employee { get; set; }
        public List<SelectListItem> Cities { get; set; }
    }
}