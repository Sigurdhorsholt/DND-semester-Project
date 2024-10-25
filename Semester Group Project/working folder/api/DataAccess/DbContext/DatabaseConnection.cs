using System.Data;
using MySqlConnector;

namespace api.DataAccess.DbContext;

public class DatabaseConnection : IDisposable
{
    private MySqlConnection _connection;

    // Constructor to initialize connection
    public DatabaseConnection(string connectionString)
    {
        _connection = new MySqlConnection(connectionString);
    }

    // Open the connection
    public void OpenConnection()
    {
        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }
    }

    // Close the connection
    public void CloseConnection()
    {
        if (_connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }
    }

    // Execute a query that returns no result (INSERT, UPDATE, DELETE)
    public void ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
    {
        using (var command = new MySqlCommand(query, _connection))
        {
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            command.ExecuteNonQuery();
        }
    }

    // Execute a query that returns a result (SELECT)
    public DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
    {
        using (var command = new MySqlCommand(query, _connection))
        {
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            using (var dataAdapter = new MySqlDataAdapter(command))
            {
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }
    }

    // Dispose method to clean up the connection
    public void Dispose()
    {
        CloseConnection();
        _connection.Dispose();
    }
}
    
    