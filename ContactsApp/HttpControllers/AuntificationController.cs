using ContactsApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.HttpControllers;

[ApiController]
[Route("api/v1/auth")]
public class AuntificationController : ControllerBase
{
    private readonly IAuntificationService _service;

    public AuntificationController(IAuntificationService service)
        => _service = service;

    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        try
        {
            var result = await _service.LoginAsync(username, password, HttpContext.RequestAborted);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string password, string fullName)
    {
        try
        {
            await _service.RegisterAsync(username, password, fullName, HttpContext.RequestAborted);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return Conflict(ex.Message);
        }
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string sessionToken)
    {
        try
        {
            await _service.DeleteUser(sessionToken, HttpContext.RequestAborted);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return RedirectToAction("Login", new {message = ex.Message});
        }
    }
}