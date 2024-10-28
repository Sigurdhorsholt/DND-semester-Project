using System.Data;
using api.DomainModel.UserModel;

namespace api.DataAccess.UserQueries;

public class userQueryUtility
{
    
    
    public List<User?> ConvertDataTableToUserList(DataTable dataTable)
    {
        var users = new List<User?>();

        foreach (DataRow row in dataTable.Rows)
        {
            var user = new User
            {
                UserId = row.Field<ulong>("ID"),  // Using Field<T>() for safer type conversion
                UserName = row["Username"].ToString(),
                Email = row["Email"].ToString(),
                FullName = row["FullName"].ToString(),
                Password = row["Password"].ToString(),
                UserType = row["UserType"].ToString(),
                Apartment = row["Apartment"].ToString(),
                LastLogin = row.IsNull("LastLogin") ? (DateTime?)null : Convert.ToDateTime(row["LastLogin"])
            };

            users.Add(user);
        }

        return users;
    }
    
    
}