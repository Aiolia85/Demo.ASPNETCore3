using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    public class WebAPIResult
    { 
        public bool IsException { get; set; }

        public string ExceptionMessage { get; set; }
    }

    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this._logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            this._logger.LogError(context.Exception,"GlobalException");

            WebAPIResult res = new WebAPIResult();
            res.IsException = true;
            res.ExceptionMessage = context.Exception.Message;

            ContentResult cr = new ContentResult();
            cr.Content = JsonConvert.SerializeObject(res);
            cr.StatusCode = 200;
            cr.ContentType = "application/json;charset=utf-8";

            context.Result = cr;

            context.ExceptionHandled = true;

        }
    }
}
