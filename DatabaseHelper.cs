using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberSecurityBotPart2
{
    internal class DatabaseHelper
    {
        private string connectionString =
            "server=localhost;database=CybersecurityBotDB;uid=root;pwd=Zukhanyise2103@;";

        public void AddTask(string title, string description, DateTime? reminderDate)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Tasks 
                                (Title, Description, ReminderDate) 
                                VALUES (@Title, @Description, @ReminderDate)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", description);

                    if (reminderDate.HasValue)
                    {
                        command.Parameters.AddWithValue("@ReminderDate", reminderDate.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ReminderDate", DBNull.Value);
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetAllTasks()
        {
            List<string> tasks = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TaskID, Title, Description, ReminderDate, IsCompleted FROM Tasks";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string reminder = reader["ReminderDate"] == DBNull.Value
                            ? "No reminder"
                            : Convert.ToDateTime(reader["ReminderDate"]).ToString("yyyy-MM-dd");

                        string status = Convert.ToBoolean(reader["IsCompleted"])
                            ? "Completed"
                            : "Pending";

                        string task =
                            $"ID: {reader["TaskID"]}\n" +
                            $"Title: {reader["Title"]}\n" +
                            $"Description: {reader["Description"]}\n" +
                            $"Reminder: {reminder}\n" +
                            $"Status: {status}\n";

                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        public void MarkTaskCompleted(int taskId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Tasks SET IsCompleted = TRUE WHERE TaskID = @TaskID";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TaskID", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(int taskId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Tasks WHERE TaskID = @TaskID";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TaskID", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}