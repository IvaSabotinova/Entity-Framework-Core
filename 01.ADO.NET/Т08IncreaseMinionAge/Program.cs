using System;
using System.Data.SqlClient;
using System.Linq;

namespace Т08IncreaseMinionAge
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] inputMinionsId = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            using SqlConnection sqlConnection = new SqlConnection("Server = .; Database = MinionsDB; Integrated Security = true ;");
            {
                sqlConnection.Open();


                for (int i = 0; i < inputMinionsId.Length; i++)
                {
                    using SqlCommand updateMinionsAge = new SqlCommand($"UPDATE Minions SET Age += 1 WHERE Id = @Id", sqlConnection);
                    updateMinionsAge.Parameters.AddWithValue("@Id", inputMinionsId[i]);
                    updateMinionsAge.ExecuteNonQuery();

                    using SqlCommand upperMinionsNameLetter = new SqlCommand($"UPDATE Minions SET[Name] = CONCAT(UPPER(SUBSTRING([Name], 1, 1)), SUBSTRING([Name], 2, LEN([Name]) - 1)) FROM Minions WHERE Id = @Id", sqlConnection);
                    upperMinionsNameLetter.Parameters.AddWithValue("@Id", inputMinionsId[i]);
                    upperMinionsNameLetter.ExecuteNonQuery();
                }

                using SqlCommand minionsNamesAndAges = new SqlCommand("SELECT [Name], Age FROM Minions", sqlConnection);
               using SqlDataReader sqlDataReader = minionsNamesAndAges.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Console.WriteLine($"{sqlDataReader[0]} {sqlDataReader[1]}");
                }
            }
        }
    }
}
