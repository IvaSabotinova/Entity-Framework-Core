using System;
using System.Data.SqlClient;

namespace T09IncreaseAgeStoredProcedure
{
    class Program
    {
        static void Main(string[] args)
        {

            int minionId = int.Parse(Console.ReadLine());

            using SqlConnection sqlConnection = new SqlConnection("Server=.; Database = MinionsDB; Integrated Security = true");
            sqlConnection.Open();

            using SqlCommand updateQuery = new SqlCommand(@"EXEC usp_GetOlder @minionId", sqlConnection);
            updateQuery.Parameters.AddWithValue("@minionId", minionId);
            updateQuery.ExecuteNonQuery();

            using SqlCommand selectQuery = new SqlCommand("SELECT [Name], Age FROM Minions", sqlConnection);

            using SqlDataReader sqlReader = selectQuery.ExecuteReader();

            while (sqlReader.Read())
            {
                Console.WriteLine($"{sqlReader["Name"]} – {sqlReader["Age"]} years old");
            }
        }
    }
}
