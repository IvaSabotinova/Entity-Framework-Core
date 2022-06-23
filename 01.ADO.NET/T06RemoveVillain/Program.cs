using System;

using System.Data.SqlClient;


namespace T06RemoveVillain
{
    class Program
    {
        static void Main(string[] args)
        {
             int villainIdAsInput = int.Parse(Console.ReadLine());

            using SqlConnection sqlConnection = new SqlConnection("Server = .; Database = MinionsDB; Integrated security = true;");
            {
                sqlConnection.Open();

                SqlCommand findVillain = new SqlCommand("SELECT [Name] FROM Villains WHERE Id = @villainId", sqlConnection);
                findVillain.Parameters.AddWithValue("@villainId", villainIdAsInput);
                string villainName = (string)findVillain.ExecuteScalar();

                if (villainName is null)
                {
                    Console.WriteLine("No such villain was found.");
                }
                else
                {
                    SqlCommand deleteVillainFromMV = new SqlCommand(@"DELETE FROM MinionsVillains
                        WHERE VillainId = @villainId", sqlConnection);

                    deleteVillainFromMV.Parameters.AddWithValue("@villainId", villainIdAsInput);
                    int numberOfDeletedMinions = deleteVillainFromMV.ExecuteNonQuery();

                    SqlCommand deleteVillainFromVillains = new SqlCommand(@"DELETE FROM Villains
                        WHERE Id = @villainId", sqlConnection);

                    deleteVillainFromVillains.Parameters.AddWithValue("@villainId", villainIdAsInput);
                    deleteVillainFromVillains.ExecuteNonQuery();

                    Console.WriteLine($"{villainName} was deleted." + Environment.NewLine +
                        $"{numberOfDeletedMinions} minions were released.");
                }
            }
        }
    }
}
