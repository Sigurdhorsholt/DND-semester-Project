using api.DataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

 

    public class SettingsController : ControllerBase
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly IConfiguration _configuration;


        public SettingsController(DatabaseConnection dbConnection,
            IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }



        [HttpPost("api/settings")]
        public IActionResult SaveSettings()
        {
            
            Console.WriteLine("Settings saved route HIT");
                       
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "Unauthorized save" });
            }
            
            return Ok();
            
            
        }
    
    
    
    
}