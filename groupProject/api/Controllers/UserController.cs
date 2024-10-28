using api.DataAccess.UserQueries;
using api.DomainModel.UserModel;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;


[Route("api/[controller]")]
public class UserController : ControllerBase
{
    
    private readonly UserQuery _userQuery;

    // Inject UserQuery via constructor
    public UserController(UserQuery userQuery)
    {
        _userQuery = userQuery;
    }

    
    
    // Get all users
    [HttpGet]
    public IActionResult GetUser()
    {
        try
        {
            var users = _userQuery.GetAllUsers();
            return Ok(users);  // Return data in JSON format
        }
        catch (Exception ex)
        {
            // Handle the error (you can return an appropriate HTTP error code and message)
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    
    // Get user by id
    [HttpGet("{id}")]
    public IActionResult GetUserById(ulong id)
    {
        try
        {
            var user = _userQuery.GetSpecificUser(id);

            if (user == null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
            
        }
        catch (Exception ex)
        {
            // Handle the error (you can return an appropriate HTTP error code and message)
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    
    // Add a new user
    [HttpPost]
    public IActionResult RegisterUser([FromBody] User  user)
    {

        if (user == null || !ModelState.IsValid)
        {
            return BadRequest("invalud user data.");
        }
        
        try
        {
            _userQuery.AddUser(user);
            _userQuery.AddUser(user);
            return CreatedAtAction(nameof(RegisterUser), new { id = user.UserId }, user);
        }
        catch (Exception ex)
        {
            // Handle the error
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    
    
    
    
}