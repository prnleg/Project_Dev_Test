using Project_Dev_Test.Web.Models;
using System.Data.SQLite;

namespace Project_Dev_Test.Web.Repository
{
    public class DataRepository
    {
        private const string ConnectionString = "Data Source=database.sqlite;";

        public DataRepository() { }

        public static void InitializeDatabase()
        {
            var initSQL = @"CREATE TABLE IF NOT EXISTS Results (
                                Id	TEXT NOT NULL,
                                User    INTEGER NOT NULL,
                                Image	TEXT NOT NULL,
                                CPU	REAL NOT NULL,
                                Memory	REAL NOT NULL,
                                TimeElapsed	REAL NOT NULL,
                                Iterations	REAL NOT NULL,
                                StartOperation	TEXT NOT NULL,
                                EndOperation	TEXT NOT NULL,
                                PRIMARY KEY(Id)
                            );";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string query = initSQL;

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public void SaveResult(ResultObject resultObject)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    var query = @"INSERT INTO Results (Id, User, Image, CPU, Memory, TimeElapsed, Iterations, StartOperation, EndOperation)
                                    VALUES (@Id, @User, @Image, @CPU, @Memory, @TimeElapsed, @Iterations, @StartOperation, @EndOperation);";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", resultObject.Id.ToString());
                        command.Parameters.AddWithValue("@User", resultObject.User);
                        command.Parameters.AddWithValue("@Image", resultObject.Image);
                        command.Parameters.AddWithValue("@CPU", resultObject.CPU);
                        command.Parameters.AddWithValue("@Memory", resultObject.Memory);
                        command.Parameters.AddWithValue("@TimeElapsed", resultObject.TimeElapsed);
                        command.Parameters.AddWithValue("@Iterations", resultObject.Iterations);
                        command.Parameters.AddWithValue("@StartOperation", resultObject.StartOperation);
                        command.Parameters.AddWithValue("@EndOperation", resultObject.EndOperation);

                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public List<ResultObject> GetAllResultsFromUser(int userId)
        {
            var objects = new List<ResultObject>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    var query = @"SELECT Id, User, Image, CPU, Memory, TimeElapsed, Iterations, StartOperation, EndOperation FROM Results
                                    WHERE User = @userId;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new ResultObject()
                                {
                                    Id = reader.GetGuid(0),
                                    User = reader.GetInt16(1),
                                    Image = reader.GetString(2),
                                    CPU = reader.GetFloat(3),
                                    Memory = reader.GetFloat(4),
                                    TimeElapsed = reader.GetDouble(5),
                                    Iterations = reader.GetFloat(6),
                                    StartOperation = reader.GetDateTime(7),
                                    EndOperation = reader.GetDateTime(8)
                                };

                                objects.Add(obj);
                            }
                        }

                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return objects;
        }
    }
}
