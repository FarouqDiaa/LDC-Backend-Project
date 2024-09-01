using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using Project.BusinessDomainLayer.Exceptions.CustomerExceptions;
using Project.BusinessDomainLayer.Exceptions.OrderExceptions;
using Project.BusinessDomainLayer.Exceptions.ProductExceptions;
using System.Net;
using ErrorResponse = Project.BusinessDomainLayer.Responses.ErrorResponse;

namespace Project.PresentationLayer.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UsedEmailException ex)
            {
                _logger.LogWarning(ex, "Used email exception caught in middleware");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (EmailNotRegisteredException ex)
            {
                _logger.LogWarning(ex, "Unregistered email exception caught in middleware");
                await HandleExceptionCustomAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (AuthenticationException ex)
            {
                _logger.LogWarning(ex, "Authentication exception caught in middleware");
                await HandleExceptionCustomAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (CustomerNotFoundException ex)
            {
                _logger.LogWarning(ex, "Customer NotFound exception caught in middleware");
                await HandleExceptionCustomAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ProductNameUsedException ex)
            {
                _logger.LogWarning(ex, "Product Name Used exception caught in middleware");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogWarning(ex, "Product not found exception caught in middleware");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidProductIdException ex)
            {
                _logger.LogWarning(ex, "Product id not found exception caught in middleware");
                await HandleExceptionCustomAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidCustomerIdException ex)
            {
                _logger.LogWarning(ex, "Customer id not found exception caught in middleware");
                await HandleExceptionCustomAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (FailedCustomerActionException ex)
            {
                _logger.LogWarning(ex, "Failed customer action exception caught in middleware");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (ProductQuantityException ex)
            {
                _logger.LogWarning(ex, "Product quantity exception caught in middleware");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (OrderNotFoundException ex)
            {
                _logger.LogWarning(ex, "Order not found exception caught in middleware");
                await HandleExceptionCustomAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = "InValid Data",
                Errors = new Dictionary<string, string[]>
            {
                { "Validation Error:", new[] { message } }
            }
            };

            return context.Response.WriteAsJsonAsync(errorResponse);
        }

        private Task HandleExceptionCustomAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = message
            };

            return context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}