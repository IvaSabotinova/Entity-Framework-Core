using System;
using System.Data.SqlClient;

namespace T02VillainNames
{
    class Program
    {
        static void Main(string[] args)
        {
            string connection = "Server = .; Database = MinionsDB; Integrated security = true;";
            using SqlConnection sqlConnection = new SqlConnection(connection);
            {
                sqlConnection.Open();

                SqlCommand sqlCmd = new SqlCommand(@"SELECT v.[Name], COUNT(mv.MinionId) AS[CountOfMinions] FROM MinionsVillains AS mv JOIN Villains AS v ON mv.VillainId = v.Id
                GROUP BY v.[Name] HAVING COUNT(mv.MinionId) > 3 ORDER BY CountOfMinions DESC", sqlConnection);

                using SqlDataReader reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"{(string)reader["Name"]} - {(int)reader["CountOfMinions"]}");
                }
            }

        }
    }
}
