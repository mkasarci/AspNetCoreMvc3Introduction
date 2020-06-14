using System.Collections.Generic;
using AspNetCoreMvc3.Introduction.Entities;

namespace AspNetCoreMvc3.Introduction.Models
{
    public class EmployeeListViewModel
    {
        public List<Employee> Employees { get; set; }
        public List<string> Cities { get; set; }
    }
}