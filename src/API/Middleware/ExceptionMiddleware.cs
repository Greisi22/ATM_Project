using System.Net;
using System.Text;
using API.Models;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;

namespace API.Middleware;

public class ExceptionMiddleware
{

    private readonly RequestDelegate _next;

    readonly ICurrentUserService _tokenService;

    public ExceptionMiddleware(RequestDelegate next, ICurrentUserService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        string resposeMessage;

        switch (exception)
        {


            case ForbiddenAccessException:
                resposeMessage = exception.ToString();
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                break;

            case NotFoundException:
                var ex = (NotFoundException)exception;
                resposeMessage = $"{ex.Message}";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case ValidationException:

                var exp = (ValidationException)exception;
                resposeMessage = $"{exp}";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case BadRequestException:

                resposeMessage = $"{exception.Message}";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            default:
                resposeMessage = exception.ToString();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var bytes = Encoding.UTF8.GetBytes(new ResponseNotOKSchema()
        {
            ErrorCode = context.Response.StatusCode.ToString(),
            Message = resposeMessage
        }.ToString());

        await context.Response.Body.WriteAsync(bytes.AsMemory(0, bytes.Length));


    }

}