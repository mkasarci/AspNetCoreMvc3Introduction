using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AspNetCoreMvc3.Introduction.Filters
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public string ViewName { get; set; }
        public Type ExceptionType { get; set; }
        public override void OnException(ExceptionContext context)
        {
            if (ExceptionType != null)
            {
                if (ExceptionType == context.Exception.GetType())
                {
                    var result = new ViewResult { ViewName = "Error" };
                    var modelDataProvider = new EmptyModelMetadataProvider();
                    result.ViewData = new ViewDataDictionary(modelDataProvider, context.ModelState);
                    result.ViewData.Add("HandleException", context.Exception);
                    context.Result = result;
                    context.ExceptionHandled = true;
                }
            }

        }
    }
}
