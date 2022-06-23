using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace T07PrintAllMinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection("Server = .; Database = MinionsDB; Integrated security = true;");
            {
                sqlConnection.Open();
                using SqlCommand listMinions = new SqlCommand("SELECT STRING_AGG([Name], ', ') FROM Minions", sqlConnection);
                string listOfMinions = (string)listMinions.ExecuteScalar();
                List<string> listOfAllMinions = listOfMinions.Split(", ").ToList();

                using SqlCommand countMinions = new SqlCommand("SELECT COUNT(*) FROM Minions", sqlConnection);
                int countOfMinions = (int)countMinions.ExecuteScalar();

                for (int i = 0; i < countOfMinions / 2; i++)
                {
                    Console.WriteLine(listOfAllMinions[i] + Environment.NewLine + listOfAllMinions[countOfMinions - 1 - i]);
                }
                if (countOfMinions % 2 != 0)
                {
                    Console.WriteLine(listOfAllMinions[countOfMinions / 2]);
                }
            }
        }
    }
}
