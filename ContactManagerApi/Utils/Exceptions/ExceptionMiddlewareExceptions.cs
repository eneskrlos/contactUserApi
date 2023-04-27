using ContactManagerApi.Models.DTO.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Net;

namespace ContactManagerApi.Utils.Exceptions
{
    public static class ExceptionMiddlewareExceptions
    {
        //built-int Exception Handler
        public static void ConfigureBuiltInExceptionHandler(this IApplicationBuilder app ) 
        {
            app.UseExceptionHandler(appError => 
            {
                appError.Run(async context => 
                {
                    var contextFeaure = context.Features.Get<IExceptionHandlerFeature>();
                    var contextrequest = context.Features.Get<IHttpRequestFeature>();

                    context.Response.ContentType = "application/json";

                    if (contextFeaure != null)
                    {
                        var erroString = new ErrorResponseData()
                        {
                            StatusCode = (int)HttpStatusCode.InternalServerError,
                            Message = contextFeaure.Error.Message,
                            Path = contextrequest.Path
                        }.ToString();

                        await context.Response.WriteAsync(erroString);
                    }
                });
            
            });
        }
    }
}
