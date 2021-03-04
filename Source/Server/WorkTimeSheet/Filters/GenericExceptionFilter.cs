using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using WorkTimeSheet.Exceptions;

namespace WorkTimeSheet.Filters
{
    public class GenericExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var errorMessage = new ErrorMessage();
            try
            {
                throw context.Exception;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                errorMessage.Message = string.IsNullOrEmpty(invalidOperationException.Message) ? "Bad Request" : invalidOperationException.Message;
                context.HttpContext.Response.StatusCode = 400;
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                errorMessage.Message = string.IsNullOrEmpty(unauthorizedAccessException.Message) ? "Invalid User" : unauthorizedAccessException.Message;
                context.HttpContext.Response.StatusCode = 401;
            }
            catch (SecurityTokenException securityTokenException)
            {
                errorMessage.Message = string.IsNullOrEmpty(securityTokenException.Message) ? "Invalid User" : securityTokenException.Message;
                context.HttpContext.Response.StatusCode = 401;
            }
            catch (ForbiddenException forbiddenException)
            {
                errorMessage.Message = string.IsNullOrEmpty(forbiddenException.Message) ? "Access forbidden" : forbiddenException.Message;
                context.HttpContext.Response.StatusCode = 403;
            }
            catch (DataNotFoundException dataNotFoundException)
            {
                errorMessage.Message = string.IsNullOrEmpty(dataNotFoundException.Message) ? "Not able to fetch required data" : dataNotFoundException.Message;
                context.HttpContext.Response.StatusCode = 404;
            }
            catch (InternalServerException internalServerException)
            {
                errorMessage.Message = string.IsNullOrEmpty(internalServerException.Message) ? "Internal Server Exception" : internalServerException.Message;
                context.HttpContext.Response.StatusCode = 500;
            }
            catch (DbUpdateException dbUpdateException)
            {
                errorMessage.Message = "Database Exception";

                if (dbUpdateException.InnerException != null)
                {
                    try
                    {
                        throw dbUpdateException.InnerException;
                    }
                    catch (SqlException sqlException)
                    {
                        if (sqlException.Errors.Count > 0)
                            errorMessage.Message = sqlException.Errors[0].Message;
                        else
                            errorMessage.Message = sqlException.Message;
                    }
                    catch (Exception exception)
                    {
                        errorMessage.Message = exception.Message;
                    }
                }
                context.HttpContext.Response.StatusCode = 500;
            }

            context.Result = new JsonResult(errorMessage);
            base.OnException(context);
        }
    }
}
