using System.Data;
using MySqlConnector;

namespace api.DataAccess.DbContext
{
    public class DatabaseConnection : IDisposable
    {
        private MySqlConnection _connection;
        private MySqlTransaction _transaction; 

        // Constructor to initialize connection
        public DatabaseConnection(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
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

        // Start a new transaction
        public void BeginTransaction()
        {
            OpenConnection();
            _transaction = _connection.BeginTransaction();
        }

        // Commit the transaction
        public void CommitTransaction()
        {
            if (_connection.State == ConnectionState.Open && _transaction != null) 
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        // Rollback the transaction
        public void RollbackTransaction()
        {
            if (_connection.State == ConnectionState.Open && _transaction != null) // Ensure connection is open before rollback
            {
                _transaction.Rollback();
                _transaction = null;
            }
        }

        // Execute a query that returns no result (INSERT, UPDATE, DELETE) with optional transaction
        public void ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            using (var command = new MySqlCommand(query, _connection, _transaction)) // Use transaction if active
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                command.ExecuteNonQuery();
            }
        }

        // Execute a query that returns a result (SELECT) with optional transaction
        public DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            using (var command = new MySqlCommand(query, _connection, _transaction)) // Use transaction if active
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

        // Dispose method to clean up the connection and transaction
        public void Dispose()
        {
            if (_transaction != null)
            {
                RollbackTransaction();
            }
            CloseConnection();
            _connection.Dispose();
        }
    }
}
