using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

public static class ErrorListExtensions
{
    public static IActionResult ToActionResult(this List<Error> errors)
    {
        var statusCode = errors.Count == 0
            ? StatusCodes.Status500InternalServerError
            : errors[0].Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

        var title = errors.Count == 0 ? "An error occurred." : errors[0].Description;

        return new ObjectResult(new ProblemDetails
        {
            Title = title,
            Status = statusCode
        })
        {
            StatusCode = statusCode
        };
    }
}
