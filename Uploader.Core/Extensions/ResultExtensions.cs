using Microsoft.AspNetCore.Mvc;
using System;
using Uploader.Core.Enums;
using Result = Uploader.Core.Models.Result;

namespace Uploader.Core.Extensions
{
    public static class ResultExtensions
    {
        public static ActionResult FromResult<T>(this ControllerBase controller, Result.Result<T> result)
        {
            return result.ResultType switch
            {
                ResultType.Ok => result.Data is null
                                        ? controller.NoContent()
                                        : controller.Ok(result.Data),
                ResultType.File => controller.File(
                    (result as Result.FileResult).Data.Stream,
                    (result as Result.FileResult).Data.ContentType,
                    (result as Result.FileResult).Data.FileName),
                ResultType.NotFound => controller.NotFound(result.Errors),
                ResultType.Invalid => controller.BadRequest(result.Errors),
                ResultType.Unexpected => controller.BadRequest(result.Errors),
                ResultType.Created => result.Data is null
                                        ? controller.StatusCode(201)
                                        : controller.StatusCode(201, result.Data),
                _ => throw new Exception("An unhandled result has occurred as a result of a service call."),
            };
        }
    }
}
