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
            "INSERT INTO user (userName, email, password, userType) VALUES (@userName, @Email, @Password, @UserType)";
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
            string query = "SELECT * FROM user WHERE userName = @userName";
            MySqlParameter[] parameters = { new MySqlParameter("@userName", model.UserName) };

            try
            {
                var userInfo = GetUserWithComplexInfo(model.UserName);
                if (userInfo == null)
                {
                    return Unauthorized(new { message = "User does not exist" });
                }

                // Check password
                if (!model.Password.Equals(userInfo["Password"].ToString()))
                {
                    return Unauthorized(new { message = "Incorrect password" });
                }

                // Generate JWT Token with additional claims
                var token = GenerateJwtToken(userInfo);

                Console.WriteLine("Login successful for user ID: " + userInfo["id"]);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login error: " + ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }   

        }

    private DataRow  GetUserWithComplexInfo(string userName)
    {
        string query = @"
        SELECT u.id, u.userName, u.Email, u.FullName, u.Password, u.apartment, 
               u.isAdmin, c.complexName, c.street, c.city, c.zipcode
        FROM user u
        LEFT JOIN livesin l ON u.id = l.userId
        LEFT JOIN apartmentcomplex c ON l.complexId = c.id
        WHERE u.userName = @userName";
        
        
        MySqlParameter[] parameters = { new MySqlParameter("@userName", userName) };
        _dbConnection.OpenConnection();
        DataTable result = _dbConnection.ExecuteQuery(query, parameters);
        _dbConnection.CloseConnection(); 
        
        return result.Rows.Count > 0 ? result.Rows[0] : null;
    }

    private string GenerateJwtToken(DataRow userInfo)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("ThisIsASecretKeyWithAtLeast32Characters");

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo["id"].ToString()),
                    new Claim(ClaimTypes.Name, userInfo["userName"].ToString()),
                    new Claim(ClaimTypes.Email, userInfo["Email"].ToString()),
                    new Claim("FullName", userInfo["FullName"].ToString()),
                    new Claim("Apartment", userInfo["apartment"].ToString()),
                    new Claim("ComplexName", userInfo["complexName"].ToString()),
                    new Claim("Street", userInfo["street"].ToString()),
                    new Claim("City", userInfo["city"].ToString()),
                    new Claim("ZipCode", userInfo["zipcode"].ToString()),
                    new Claim("IsAdmin", userInfo["isAdmin"].ToString() == "1" ? "true" : "false")
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _configuration["Issuer"],
                Audience = _configuration["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
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



    
