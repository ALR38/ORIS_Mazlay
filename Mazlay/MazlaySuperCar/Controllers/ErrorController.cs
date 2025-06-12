using Microsoft.AspNetCore.Mvc;

namespace MazlaySuperCar.Controllers;   

public class ErrorController : Controller
{
    [Route("{statusCode:int}")]
    public IActionResult HttpStatusHandler(int statusCode)
    {
        Response.StatusCode = statusCode;
        return View("~/Views/404/404.cshtml");  
    }
    
    // [Route("Error/500")]
    // public IActionResult ServerError()
    // {
    //     Response.StatusCode = 500;
    //     return View("~/Views/Error/500.cshtml");
    // }
}
