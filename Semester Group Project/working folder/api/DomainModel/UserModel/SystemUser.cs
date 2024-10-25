using Microsoft.AspNetCore.Identity;

namespace BackendCore.DomainModel.UserModel;

public class SystemUser : IdentityUser

{
    public string UserId { get; private set; }  // Unique ID for each user
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; private set; }
    public string FullName { get; set; }

    // Constructor to initialize user
    public SystemUser(string userId, string userName, string email, string password, string fullName)
    {
        this.UserId = userId;
        this.UserName = userName;
        this.Email = email;
        this.Password = password;
        this.FullName = fullName;
    }
    
    // Method to update password with validation
    public void SetPassword(string newPassword)
    {
        if (!string.IsNullOrEmpty(newPassword) && newPassword.Length >= 8) // Example validation
        {
            Password = newPassword;
        }
        else
        {
            throw new ArgumentException("Password must be at least 8 characters long");
        }
    }
    
    // Method to verify password (for login)
    public bool VerifyPassword(string inputPassword)
    {
        return Password == inputPassword;
    }
    
    // Method to reset password (assign temporary password)
    public void ResetPassword(string tempPassword)
    {
        Password = tempPassword; 
    }

    // Validation method for email format
    private bool ValidateEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
    
}