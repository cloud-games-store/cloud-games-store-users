using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Users.Application.DTOs;
using Users.Application.Interfaces;

namespace Users.API.Controllers;

[Route("user")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status200OK)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get a user by GUID
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid id)
    {
        var result = await _userService.GetUser(id);
       
        if (!result.Success)
        {
            return result?.Error?.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(result?.Error?.ErrorMessage),
                HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, result?.Error?.ErrorMessage),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, "")
            };
        }

        if (result.Data is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserRequestDto dto)
    {
        var result = await _userService.CreateUser(dto);

        if (!result.Success)
        {
            return result?.Error?.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(result?.Error?.ErrorMessage),
                HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, result?.Error?.ErrorMessage),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, "")
            };
        }

        return Ok(result);
    }

    /// <summary>
    /// Edit a user 
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Put([FromQuery] Guid id, [FromBody] UserRequestDto dto)
    {
        var result = await _userService.UpdateUser(dto, id);

        if (!result.Success)
        {
            return result?.Error?.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(result?.Error?.ErrorMessage),
                HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, result?.Error?.ErrorMessage),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, "")
            };
        }

        return Ok();
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        var result = await _userService.DeleteUser(id);

        if (!result.Success)
        {
            return result?.Error?.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(result?.Error?.ErrorMessage),
                HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, result?.Error?.ErrorMessage),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, "")
            };
        }

        return Ok();
    }
}
