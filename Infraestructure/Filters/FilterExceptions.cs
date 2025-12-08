using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infraestructure.Filters
{
    public class FilterExceptions : ExceptionFilterAttribute
    {
        private readonly ILogger<FilterExceptions> _logger;
        private readonly IAdminInterfaces _adminInterfaces;

        public FilterExceptions(ILogger<FilterExceptions> logger, IAdminInterfaces adminInterfaces)
        {
            _logger = logger;
            _adminInterfaces = adminInterfaces;
        }

        public override void OnException(ExceptionContext context)
        {
            LogApplication logApplication = new LogApplication();
            HttpStatusCode Status = HttpStatusCode.InternalServerError;
            string Title = context.Exception?.InnerException != null ? context.Exception?.InnerException?.Message : context.Exception?.Message;
            string Detail = context.Exception?.InnerException != null ? context.Exception?.InnerException?.Message : context.Exception?.Message;

            if (context.Exception.GetType() == typeof(UnauthorizedBusinessException))
            {
                var exception = (UnauthorizedBusinessException)context.Exception;
                Status = HttpStatusCode.Unauthorized;
                Title = string.IsNullOrEmpty(exception.exception?.Name) ? $"Unauthorized {context.ActionDescriptor.DisplayName}" : $"{exception.exception.Name} {context.ActionDescriptor.DisplayName}";
                Detail = string.IsNullOrEmpty(exception.Message) ? exception.exception.Message : exception.Message;
            }
            if (context.Exception.GetType() == typeof(InternalServerErrorBusinessExceprions))
            {
                var exception = (InternalServerErrorBusinessExceprions)context.Exception;
                Status = HttpStatusCode.InternalServerError;
                Title = string.IsNullOrEmpty(exception.exception?.Name) ? $"Internal Server Error {context.ActionDescriptor.DisplayName}" : $"Not name";
                Detail = string.IsNullOrEmpty(exception.Message) ? exception.exception.Message : exception.Message;
            }
            if (context.Exception.GetType() == typeof(BadRequestBusinessException))
            {
                var exception = (BadRequestBusinessException)context.Exception;
                Status = HttpStatusCode.BadRequest;
                Title = string.IsNullOrEmpty(exception.exception?.Name) ? $"BadRequest Server Error {context.ActionDescriptor.DisplayName}" : $"Not name";
                Detail = string.IsNullOrEmpty(exception.Message) ? exception.exception.Message : exception.Message;
            }
            if (context.Exception.GetType() == typeof(ConnectionBusinessException))
            {
                var exception = (ConnectionBusinessException)context.Exception;
                Status = HttpStatusCode.BadRequest;
                Title = "Connect Error ";
                Detail = string.IsNullOrEmpty(exception.Message) ? exception.exception.Message : exception.Message;
            }

            var objectException = new
            {
                Status,
                Title,
                Detail
            };

            var json = new
            {
                errors = new[] { objectException }
            };

            switch (Status)
            {
                case HttpStatusCode.Continue:
                    break;
                case HttpStatusCode.SwitchingProtocols:
                    break;
                case HttpStatusCode.Processing:
                    break;
                case HttpStatusCode.EarlyHints:
                    break;
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.Created:
                    break;
                case HttpStatusCode.Accepted:
                    break;
                case HttpStatusCode.NonAuthoritativeInformation:
                    break;
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.ResetContent:
                    break;
                case HttpStatusCode.PartialContent:
                    break;
                case HttpStatusCode.MultiStatus:
                    break;
                case HttpStatusCode.AlreadyReported:
                    break;
                case HttpStatusCode.IMUsed:
                    break;
                case HttpStatusCode.Ambiguous:
                    break;
                case HttpStatusCode.Moved:
                    break;
                case HttpStatusCode.Found:
                    break;
                case HttpStatusCode.RedirectMethod:
                    break;
                case HttpStatusCode.NotModified:
                    break;
                case HttpStatusCode.UseProxy:
                    break;
                case HttpStatusCode.Unused:
                    break;
                case HttpStatusCode.RedirectKeepVerb:
                    break;
                case HttpStatusCode.PermanentRedirect:
                    break;
                case HttpStatusCode.BadRequest:
                    logApplication.detail = objectException.Detail;
                    logApplication.status = Status;
                    logApplication.title = objectException.Title;
                    logApplication.logDate = DateTime.Now;
                    _adminInterfaces.logApplicationRepository.Add(logApplication);
                    break;
                case HttpStatusCode.Unauthorized:
                    logApplication.detail = objectException.Detail;
                    logApplication.status = Status;
                    logApplication.title = objectException.Title;
                    logApplication.logDate = DateTime.Now;
                     _adminInterfaces.logApplicationRepository.Add(logApplication);
                    break;
                case HttpStatusCode.PaymentRequired:
                    break;
                case HttpStatusCode.Forbidden:
                    break;
                case HttpStatusCode.NotFound:
                    break;
                case HttpStatusCode.MethodNotAllowed:
                    break;
                case HttpStatusCode.NotAcceptable:
                    break;
                case HttpStatusCode.ProxyAuthenticationRequired:
                    break;
                case HttpStatusCode.RequestTimeout:
                    break;
                case HttpStatusCode.Conflict:
                    break;
                case HttpStatusCode.Gone:
                    break;
                case HttpStatusCode.LengthRequired:
                    break;
                case HttpStatusCode.PreconditionFailed:
                    break;
                case HttpStatusCode.RequestEntityTooLarge:
                    break;
                case HttpStatusCode.RequestUriTooLong:
                    break;
                case HttpStatusCode.UnsupportedMediaType:
                    break;
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    break;
                case HttpStatusCode.ExpectationFailed:
                    break;
                case HttpStatusCode.MisdirectedRequest:
                    break;
                case HttpStatusCode.UnprocessableEntity:
                    break;
                case HttpStatusCode.Locked:
                    break;
                case HttpStatusCode.FailedDependency:
                    break;
                case HttpStatusCode.UpgradeRequired:
                    break;
                case HttpStatusCode.PreconditionRequired:
                    break;
                case HttpStatusCode.TooManyRequests:
                    break;
                case HttpStatusCode.RequestHeaderFieldsTooLarge:
                    break;
                case HttpStatusCode.UnavailableForLegalReasons:
                    break;
                case HttpStatusCode.InternalServerError:
                    logApplication.detail = objectException.Detail;
                    logApplication.status = Status;
                    logApplication.title = objectException.Title;
                    logApplication.logDate = DateTime.Now;
                    _adminInterfaces.logApplicationRepository.Add(logApplication);
                    break;
                case HttpStatusCode.NotImplemented:
                    break;
                case HttpStatusCode.BadGateway:
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    break;
                case HttpStatusCode.GatewayTimeout:
                    break;
                case HttpStatusCode.HttpVersionNotSupported:
                    break;
                case HttpStatusCode.VariantAlsoNegotiates:
                    break;
                case HttpStatusCode.InsufficientStorage:
                    break;
                case HttpStatusCode.LoopDetected:
                    break;
                case HttpStatusCode.NotExtended:
                    break;
                case HttpStatusCode.NetworkAuthenticationRequired:
                    break;
                default:
                    break;
            }
            context.Result = new JsonResult(json);
            _logger.LogInformation("JsonResult : " + objectException.Title);
            _logger.LogInformation("JsonResult : " + objectException.Detail);
            _logger.LogInformation("JsonResult : " + objectException.Status);
            context.HttpContext.Response.StatusCode = (int)Status;
            context.ExceptionHandled = true;
        }
    }
}
