namespace Supercell.Laser.Server.Utils
{
    using MySql.Data.MySqlClient;
    using Supercell.Laser.Server.Settings;

    public class DatabaseHelper
    {
        private static string GetConnectionString()
        {
            return $"server={Configuration.Instance.MysqlHost};"
                + $"user={Configuration.Instance.MysqlUsername};"
                + $"database={Configuration.Instance.MysqlDatabase};"
                + $"port={Configuration.Instance.MysqlPort};"
                + $"password={Configuration.Instance.MysqlPassword}";
        }

        public static string ExecuteScalar(string query, params (string, object)[] parameters)
        {
            try
            {
                using (MySqlConnection connection = new(GetConnectionString()))
                {
                    connection.Open();

                    MySqlCommand cmd = new(query, connection);
                    foreach ((string paramName, object paramValue) in parameters)
                    {
                        cmd.Parameters.AddWithValue(paramName, paramValue);
                    }

                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "N/A";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public static bool ExecuteNonQuery(string query, params (string, object)[] parameters)
        {
            try
            {
                using (MySqlConnection connection = new(GetConnectionString()))
                {
                    connection.Open();

                    MySqlCommand cmd = new(query, connection);
                    foreach ((string paramName, object paramValue) in parameters)
                    {
                        cmd.Parameters.AddWithValue(paramName, paramValue);
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}