using System;
using System.Data.SqlClient;

namespace T04AddMinion
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputLine1 = Console.ReadLine();
            string inputLine2 = Console.ReadLine();
            string[] tokens1 = inputLine1.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] tokens2 = inputLine2.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string minionName = tokens1[1];
            int minionAge = int.Parse(tokens1[2]);
            string minionTown = tokens1[3];
            string villainName = tokens2[1];
            string connection = "Server = .; Database = MinionsDB; Integrated security = true;";
            using SqlConnection sqlconnection = new SqlConnection(connection);
            {

                sqlconnection.Open();
                SqlCommand sqlCommand1 = new SqlCommand(@"SELECT COUNT(*) FROM Towns
                WHERE [Name] = @minionTown", sqlconnection);
                sqlCommand1.Parameters.AddWithValue("@minionTown", minionTown);
                int resultMinionTown = (int)sqlCommand1.ExecuteScalar();

                if (resultMinionTown == 0)
                {
                    SqlCommand sqlCommand2 = new SqlCommand($@"INSERT INTO Towns ([Name]) Values
                    (@minionTown)", sqlconnection);
                    sqlCommand2.Parameters.AddWithValue("@minionTown", minionTown);
                    sqlCommand2.ExecuteNonQuery();
                    Console.WriteLine($"Town {minionTown} was added to the database.");
                }
                SqlCommand sqlCommand3 = new SqlCommand(@"SELECT COUNT(*) FROM Villains
                    WHERE [Name] = @villainName", sqlconnection);
                sqlCommand3.Parameters.AddWithValue("@villainName", villainName);
                int resultVillainName = (int)sqlCommand3.ExecuteScalar();

                if (resultVillainName == 0)
                {
                    SqlCommand sqlCommand4 = new SqlCommand($@"INSERT INTO Villains Values
                    (@villainName, 4)", sqlconnection);
                    sqlCommand4.Parameters.AddWithValue("@villainName", villainName);
                    sqlCommand4.ExecuteNonQuery();
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }

                SqlCommand sqlCommand5 = new SqlCommand($@"SELECT Id FROM Towns WHERE [Name] = @minionTown", sqlconnection);

                sqlCommand5.Parameters.AddWithValue("@minionTown", minionTown);

                int townId = (int)sqlCommand5.ExecuteScalar();

                SqlCommand sqlCommand6 = new SqlCommand($@"SELECT COUNT(*) FROM Minions WHERE [Name] = @minionName AND Age = @minionAge AND TownId = {townId}", sqlconnection);
                sqlCommand6.Parameters.AddWithValue("@minionName", minionName);
                sqlCommand6.Parameters.AddWithValue("@minionAge", minionAge);
                int currentMinionCount = (int)sqlCommand6.ExecuteScalar();

                if (currentMinionCount == 0)
                {
                    SqlCommand sqlCommand7 = new SqlCommand($@"INSERT INTO Minions VALUES
                    (@minionName, @minionAge , {townId})", sqlconnection);
                    sqlCommand7.Parameters.AddWithValue("@minionName", minionName);
                    sqlCommand7.Parameters.AddWithValue("@minionAge", minionAge);
                    sqlCommand7.ExecuteNonQuery();
                }


                SqlCommand sqlCommand8 = new SqlCommand(@"SELECT Id FROM Villains WHERE [Name] = @villainName", sqlconnection);
                sqlCommand8.Parameters.AddWithValue("@villainName", villainName);

                int villainId = (int)sqlCommand8.ExecuteScalar();

                SqlCommand sqlCommand9 = new SqlCommand($@"SELECT Id FROM Minions WHERE [Name] = @minionName AND Age = @minionAge AND TownId = {townId}", sqlconnection);
                sqlCommand9.Parameters.AddWithValue("@minionName", minionName);
                sqlCommand9.Parameters.AddWithValue("@minionAge", minionAge);

                int minionId = (int)sqlCommand9.ExecuteScalar();

                SqlCommand sqlCommand10 = new SqlCommand($@"SELECT COUNT(*) FROM MinionsVillains WHERE MinionId = @minionId AND VillainId = @villainId", sqlconnection);
                sqlCommand10.Parameters.AddWithValue("@minionId", minionId);
                sqlCommand10.Parameters.AddWithValue("@villainId", villainId);

                int resultMinionVillain = (int)sqlCommand10.ExecuteScalar();

                if (resultMinionVillain == 0)
                {

                    SqlCommand sqlCommand11 = new SqlCommand($@"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES ({minionId}, {villainId})", sqlconnection);
                    sqlCommand11.ExecuteNonQuery();
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }

            }
        }
    }
}

