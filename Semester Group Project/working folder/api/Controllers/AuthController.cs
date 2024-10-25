using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.DataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;  


// Use MySqlConnector instead of MySql.Data.MySqlClient

namespace api.Controllers;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DatabaseConnection _dbConnection;
    private readonly IConfiguration _configuration;


    public AuthController(DatabaseConnection dbConnection,
        IConfiguration configuration)
    {
        _dbConnection = dbConnection;
        _configuration = configuration;

    }


    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel model)
    {

        // SQL query to insert a new user
        string query =
            "INSERT INTO User (userName, email, password, userType) VALUES (@userName, @Email, @Password, @UserType)";
        MySqlParameter[] parameters =
        {
            new MySqlParameter("@userName", model.UserName),
            new MySqlParameter("@Email", model.Email),
            new MySqlParameter("@Password", model.Password),
            new MySqlParameter("@UserType", model.UserType)
        };
        
        try
        {
            _dbConnection.OpenConnection();
            _dbConnection.ExecuteNonQuery(query, parameters);
            _dbConnection.CloseConnection();

            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }

    }
    
    [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            
            Console.WriteLine("Login route tested " + model.UserName.ToString() + " " + model.Password.ToString());
            // Check if user exists in the database
            string query = "SELECT * FROM User WHERE userName = @userName";
            MySqlParameter[] parameters = { new MySqlParameter("@userName", model.UserName) };

            try
            {
                _dbConnection.OpenConnection();
                DataTable result = _dbConnection.ExecuteQuery(query, parameters);
                _dbConnection.CloseConnection();

                if (result.Rows.Count == 0)
                {
                    return Unauthorized(new { message = "User does not exist" });
                }

                // Get the user data
                var user = result.Rows[0];
                var storedPassword = user["password"];

                var userIdResult = user["id"].ToString();
                var isAdminresult = user["isAdmin"].ToString().ToLowerInvariant();
                // Verify password
                if (!model.Password.Equals(storedPassword))
                {
                    return Unauthorized(new { message = "Incorrect password" });
                }

                // Generate JWT Token
                var token = GenerateJwtToken(userIdResult, isAdminresult); // Pass user ID to token
                
                Console.WriteLine(user["id"].ToString() + " " + user["isAdmin"].ToString());
                Console.WriteLine(token.ToString());
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        private string GenerateJwtToken(string userId, string isAdmin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Retrieve the secret key from the configuration
            var key = Encoding.UTF8.GetBytes("ThisIsASecretKeyWithAtLeast32Characters");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId), // Add userId claim
                    new Claim("IsAdmin",
                        (isAdmin == "true" ? "true" : "false").ToString()) // Add IsAdmin claim based on userType
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration
                Issuer = "yourdomain.com",  // Add Issuer
                Audience = "yourdomain.com", // Add Audience
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public class RegisterModel
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserType { get; set; } // Enum: SystemAdmin, ComplexAdmin, DailyUser
        }

        public class LoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }



    
