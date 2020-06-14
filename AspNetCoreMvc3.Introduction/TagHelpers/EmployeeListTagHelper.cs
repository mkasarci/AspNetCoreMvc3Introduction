using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;


namespace AspNetCoreMvc3.Introduction.TagHelpers
{
    [HtmlTargetElement("employee-list")]
    public class EmployeeListTagHelper : TagHelper
    {
        private List<Employee> _employees;
        public EmployeeListTagHelper()
        {
            _employees = new List<Employee>
            {
                new Employee {Id = 1, FirstName = "Muhammet", LastName = "Kasarcı", CityId = 34},
                new Employee {Id = 2, FirstName = "Furkan", LastName = "Kasarcı", CityId = 34}
            };
        }

        private const string ListCountAttributeName = "count";
        //ListCountAttributeName it is a standard naming for using Tag helpers with parameters. It is just bind the count parameter with the property that I created down below. So this field should be like => private const string <PropertyName>AttributeName = "ParameterName"
        
        [HtmlAttributeName(ListCountAttributeName)]
        public int ListCount { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var query = _employees.Take(ListCount);
            StringBuilder stringBuilder = new StringBuilder();
            
            foreach (Employee employee in query)
            {
                stringBuilder.AppendFormat("<h2><a href='/employee/detail/{0}'>{1}</a></h2>", employee.Id, employee.FirstName);
            }

            output.Content.SetHtmlContent(stringBuilder.ToString());
            base.Process(context, output);
        }
    }
}
