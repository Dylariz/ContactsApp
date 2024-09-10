using ContactsApp.Models;
using ContactsApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.HttpControllers;

[ApiController]
[Route("api/v1/contacts")]
public class ContactsController : ControllerBase
{
    private readonly IContactsService _service;

    public ContactsController(IContactsService service)
        => _service = service;
    
    [HttpPost("add")]
    public async Task<IActionResult> AddContact(string sessionToken, int contactId)
    {
        try
        {
            await _service.AddContactAsync(sessionToken, contactId, HttpContext.RequestAborted);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("full")]
    [ProducesResponseType(typeof(IEnumerable<Profile>), 200)]
    public async Task<IActionResult> GetUserContactsFull(string sessionToken)
    {
        try
        {
            var result = await _service.GetUserContactsFullAsync(sessionToken, HttpContext.RequestAborted);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("ids")]
    [ProducesResponseType(typeof(IEnumerable<int>), 200)]
    public async Task<IActionResult> GetUserContactsIds(string sessionToken)
    {
        try
        {
            var result = await _service.GetUserContactsIdsAsync(sessionToken, HttpContext.RequestAborted);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}