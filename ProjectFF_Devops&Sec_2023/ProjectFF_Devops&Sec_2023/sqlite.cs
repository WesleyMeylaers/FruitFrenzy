using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using Dapper;
using System.Windows.Forms;

namespace ProjectFF_Devops_Sec_2023
{
    public class sqlite
    {
        // Create connectionstring for connection to the database
        private const string ConnectionString = "Data Source=Database_project.db;Version=3;";
        public static void AddUser(string username, string password)
        {
            // Establish a connection to the SQLite database.
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                // Open the database connection.
                connection.Open();
                // Define the SQL query to insert a new user into the Tbl_User table.
                string insertUserQuery = "INSERT INTO Tbl_User (username, password, highscore) VALUES (@username, @password, @highscore)";
                // Create a command with the insert query and the open connection.
                using (SQLiteCommand command = new SQLiteCommand(insertUserQuery, connection))
                {
                    // Set parameters for the query (username, password, and initial highscore of 0).
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@highscore", 0);
                    // Execute the query to insert the new user.
                    command.ExecuteNonQuery();
                }
            }

        }
        public static DataTable GetTop10HighScores()
        {
            // Establish a connection to the SQLite database.
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                // Open the database connection.
                connection.Open();
                // Define the SQL query to retrieve the top 10 high scores from the Tbl_User table.
                string getTop10HighScoresQuery = "SELECT username, highscore FROM Tbl_User ORDER BY highscore DESC LIMIT 10";
                // Create a command with the select query and the open connection.
                using (SQLiteCommand command = new SQLiteCommand(getTop10HighScoresQuery, connection))
                {
                    // Create a data adapter and a DataTable to store the results.
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable top10HighScoresTable = new DataTable();
                        // Fill the DataTable with the results of the query.
                        adapter.Fill(top10HighScoresTable);
                        // Return the DataTable containing the top 10 high scores.
                        return top10HighScoresTable;
                    }
                }
            }

        }
        public static DataTable GetUserDetails(string username)
        {
            // Establish a connection to the SQLite database.
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                // Open the database connection.
                connection.Open();
                // Define the SQL query to retrieve all details for a specific user from the Tbl_User table.
                string getUserDetailsQuery = "SELECT * FROM Tbl_User WHERE username = @username";
                // Create a command with the select query and the open connection.
                using (SQLiteCommand command = new SQLiteCommand(getUserDetailsQuery, connection))
                {
                    // Set parameters for the query (username).
                    command.Parameters.AddWithValue("@username", username);
                    // Create a data adapter and a DataTable to store the results.
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable userDetailsTable = new DataTable();
                        // Fill the DataTable with the results of the query.
                        adapter.Fill(userDetailsTable);
                        // Return the DataTable containing user details.
                        return userDetailsTable;
                    }
                }
            }
        }
        public static void UpdateScore(string username, int newScore)
        {
            // Establish a connection to the SQLite database.
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                // Open the database connection.
                connection.Open();
                // Retrieve the current high score for the specified user.
                int currentHighScore = GetCurrentHighScore(username);
                // Check if the new score is higher than the current high score.
                if (newScore > currentHighScore)
                {
                    // Define the SQL query to update the high score for the specified user in the Tbl_User table.
                    string updateScoreQuery = "UPDATE Tbl_User SET highscore = @newScore WHERE username = @username";
                    // Create a command with the update query and the open connection.
                    using (SQLiteCommand command = new SQLiteCommand(updateScoreQuery, connection))
                    {
                        // Set parameters for the query (newScore and username).
                        command.Parameters.AddWithValue("@newScore", newScore);
                        command.Parameters.AddWithValue("@username", username);
                        // Execute the query to update the high score.
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        public static int GetCurrentHighScore(string username)
        {
            // Establish a connection to the SQLite database.
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                // Open the database connection.
                connection.Open();
                // Define the SQL query to retrieve the current high score for the specified user from the Tbl_User table.
                string getCurrentHighScoreQuery = "SELECT highscore FROM Tbl_User WHERE username = @username";
                // Create a command with the select query and the open connection.
                using (SQLiteCommand command = new SQLiteCommand(getCurrentHighScoreQuery, connection))
                {
                    // Set parameters for the query (username).
                    command.Parameters.AddWithValue("@username", username);
                    // Execute the query to retrieve the current high score.
                    object result = command.ExecuteScalar();
                    // Return the current high score, or 0 if the result is DBNull.
                    return result == DBNull.Value ? 0 : Convert.ToInt32(result);
                }
            }
        }
    }
}