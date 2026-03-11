using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Api.controllers;

public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> error)
    {
        var statusCode = error[0].Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(title: error[0].Description, statusCode: statusCode);
    }
}