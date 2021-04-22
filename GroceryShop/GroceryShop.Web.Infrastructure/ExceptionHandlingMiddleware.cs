namespace GroceryShop.Web.Infrastructure
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using GroceryShop.Common;
    using GroceryShop.Web.Infrastructure.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var problemDetails = GenerateProblemDetails(ex);
            var jsonSetting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var result = JsonConvert.SerializeObject(problemDetails, jsonSetting);

            context.Response.ContentType = GlobalConstants.ApplicationProblemJson;
            context.Response.StatusCode = (int)problemDetails.Status;

            return context.Response.WriteAsync(result);
        }

        private static ProblemDetails GenerateProblemDetails(Exception ex)
        {
            string type;
            string title;
            HttpStatusCode code;

            switch (ex)
            {
                case ObjectNotFoundException objectNotFoundException:
                    code = HttpStatusCode.NotFound;
                    type = GlobalConstants.NotFoundUri;
                    title = GlobalConstants.NotFoundTitle;
                    break;
                case InvalidParameterException invalidParameterException:
                case ObjectExistsException objectExistsException:
                    code = HttpStatusCode.BadRequest;
                    type = GlobalConstants.BadRequestUri;
                    title = GlobalConstants.BadRequestTitle;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    type = GlobalConstants.InternalServerErrorUri;
                    title = GlobalConstants.InternalServerErrorTitle;
                    break;
            }

            return new ProblemDetails()
            {
                Type = type,
                Title = title,
                Detail = ex.Message,
                Status = (int)code
            };
        }
    }
}