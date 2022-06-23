using System;
using System.Data.SqlClient;

namespace T03MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {

            int villainIdAsInput = int.Parse(Console.ReadLine());
            string connection = "Server = .; Database = MinionsDB; Integrated security = true;";

            using SqlConnection sqlConnection = new SqlConnection(connection);
            {
                sqlConnection.Open();

                SqlCommand sqlCommand1 = new SqlCommand(@"SELECT Name FROM Villains
                WHERE Id = @VillainId", sqlConnection);
                sqlCommand1.Parameters.AddWithValue("@VillainId", villainIdAsInput);
                string villainResult1 = (string)sqlCommand1.ExecuteScalar();

                if (string.IsNullOrEmpty(villainResult1))
                {
                    Console.WriteLine($"No villain with ID {villainIdAsInput} exists in the database."); return;
                }
                else
                {
                    Console.WriteLine($"Villain: {villainResult1}");
                }

                SqlCommand sqlCommand2 = new SqlCommand(@"SELECT COUNT(m.Id) FROM MinionsVillains AS mv JOIN Minions AS m ON mv.MinionId = m.Id WHERE mv.VillainId = @VillainId", sqlConnection);
                sqlCommand2.Parameters.AddWithValue("@VillainId", villainIdAsInput);
                int villainResult2 = (int)sqlCommand2.ExecuteScalar();
                if (villainResult2 == 0)
                {
                    Console.WriteLine("(no minions)");
                }
                else
                {
                    SqlCommand sqlCommand3 = new SqlCommand(@"SELECT m.[Name] AS [MinionsNames], m.Age as [MinionAge] FROM MinionsVillains AS mv JOIN Minions AS m ON mv.MinionId = m.Id JOIN Villains AS v ON mv.VillainId = v.Id WHERE v.Id = @VillainId
                      ORDER BY m.[Name]", sqlConnection);
                    sqlCommand3.Parameters.AddWithValue("@VillainId", villainIdAsInput);
                    using SqlDataReader reader = sqlCommand3.ExecuteReader();

                    int count = 1;
                    while (reader.Read())
                    {
                        string villainsMinion = (string)reader["MinionsNames"];
                        int minionAge = (int)reader["MinionAge"];
                        Console.WriteLine($"{count}. {villainsMinion} {minionAge}");
                        count++;
                    }

                }
            }

        }
    }
}

