

namespace api.DomainModel.UserModel;

public class User
{
    public ulong UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string UserType { get; set; }  // Updated to match database column
    public string Apartment { get; set; }
    public DateTime? LastLogin { get; set; }

    // Parameterless constructor
    public User() { }

    public User(
        ulong userId,
        string userName,
        string email,
        string password,
        string fullName,
        string userType,  // Updated
        string apartment,
        DateTime? lastLogin)
    {
        this.UserId = userId;
        this.UserName = userName;
        this.Email = email;
        this.Password = password;
        this.FullName = fullName;
        this.UserType = userType;  // Updated
        this.Apartment = apartment;
        this.LastLogin = lastLogin;
    }
}