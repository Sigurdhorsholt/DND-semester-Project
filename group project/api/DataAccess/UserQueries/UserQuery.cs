using api.DataAccess.DbContext;
using api.DomainModel.UserModel;
using MySqlConnector;

namespace api.DataAccess.UserQueries;

public class UserQuery
{
    private readonly DatabaseConnection _dbConnection;
    private readonly userQueryUtility _userQueryUtility;
    
    public UserQuery(DatabaseConnection dbConnection, userQueryUtility userQueryUtility)
    {
        _dbConnection = dbConnection;
        _userQueryUtility = userQueryUtility;
    }
    
    
    public List<User?> GetAllUsers()
    {
        _dbConnection.OpenConnection();
        string query = "SELECT * FROM user";
        var result = _dbConnection.ExecuteQuery(query);
        _dbConnection.CloseConnection();
        
        return _userQueryUtility.ConvertDataTableToUserList(result);
    }
    
    public void AddUser(User user)
    {
        
        _dbConnection.OpenConnection();
        
            string query = "INSERT INTO user (UserName, Email, Password, FullName, userType, Apartment, LastLogin) " +
                           "VALUES (@UserName, @Email, @Password, @FullName, @UserType, @Apartment, @LastLogin);" +
                           "SELECT LAST_INSERT_ID();";  // Retrieves the last generated ID
    
            MySqlParameter[] parameters = {
                new MySqlParameter("@UserName", user.UserName),
                new MySqlParameter("@Email", user.Email),
                new MySqlParameter("@Password", user.Password), // Consider hashing the password
                new MySqlParameter("@FullName", user.FullName),
                new MySqlParameter("@UserType", user.UserType),  // Updated
                new MySqlParameter("@Apartment", user.Apartment),
                new MySqlParameter("@LastLogin", (object)user.LastLogin ?? DBNull.Value) // Handle nullable correctly
            };
            _dbConnection.ExecuteQuery(query, parameters);
            _dbConnection.CloseConnection();
        
    }


    public User? GetSpecificUser(ulong id)
    {
        _dbConnection.OpenConnection();
        string query = "SELECT * FROM user WHERE Id = @Id";
        MySqlParameter[] parameters = { new MySqlParameter("@Id", id) };
        var result = _dbConnection.ExecuteQuery(query, parameters);
        _dbConnection.CloseConnection();
        
        return _userQueryUtility.ConvertDataTableToUserList(result).FirstOrDefault();
    }
}