using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SCERP.API
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        private static readonly Dictionary<string, List<ParameterInfo>> CachedParameters = new Dictionary<string, List<ParameterInfo>>();
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Type[] types = actionContext.ActionArguments.Select(x => x.GetType()).ToArray();
            string actionName = actionContext.ActionDescriptor.ActionName;

            if (types.Length == 0)
            {
                return;
            }

            Type contorller = actionContext.ControllerContext.Controller.GetType();

            MethodInfo methodInfo = contorller.GetMethod(actionName);

            if (methodInfo == null)
            {
                return;
            }

            List<ParameterInfo> parameters;
            // get parameters from cache
            CachedParameters.TryGetValue(actionName, out parameters);

            if (parameters == null)
            {
                // parameters not cached yet, need to get from method
                parameters = methodInfo.GetParameters().Where(x => !x.IsOut).ToList();

                // cache parameters
                CachedParameters.Add(actionName, parameters);
            }

            Dictionary<string, object> data = new Dictionary<string, object>();

            ValidationContext validationContext = new ValidationContext(this);
            List<ValidationResult> errors = new List<ValidationResult>();

            for (int i = 0; i < parameters.Count; i++)
            {
                ParameterInfo parameter = parameters[i];

                string name = parameter.Name;
                object value = actionContext.ActionArguments[name];

                // add parameter name and value to dictionary
                data.Add(parameters[i].Name, parameters[i]);

                // validate parameter
                validationContext.DisplayName = name;
                List<ValidationAttribute> validations = parameter.GetCustomAttributes<ValidationAttribute>().ToList();

                if (validations.Any())
                {
                    Validator.TryValidateValue(value, validationContext, errors, validations);
                }
            }
            string parameterMessage = string.Empty;
            if (errors.Any())
            {
                parameterMessage = string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage));
            }


            if (!string.IsNullOrWhiteSpace(parameterMessage))
            {
                ErrorMessage errorMessage = new ErrorMessage();

                actionContext.Response = actionContext.Request.CreateResponse<ErrorMessage>(HttpStatusCode.BadRequest, errorMessage);
            }
        }
    }
}
