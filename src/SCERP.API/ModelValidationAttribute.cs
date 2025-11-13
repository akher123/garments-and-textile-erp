using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SCERP.API
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                var errors = new List<string>();
                foreach (var modelStateVal in actionContext.ModelState.Values.Select(d => d.Errors))
                {
                    errors.AddRange(modelStateVal.Select(error => error.ErrorMessage));
                }
                ErrorMessage errorMessage = new ErrorMessage
                {
                    Message = string.Join(Environment.NewLine, errors.Select(x => x))
                };

                if (!string.IsNullOrWhiteSpace(errorMessage.Message))
                {
                    actionContext.Response = actionContext.Request.CreateResponse<ErrorMessage>(HttpStatusCode.BadRequest, errorMessage);
                }
            }
        }
    }
}
