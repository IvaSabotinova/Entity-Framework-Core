using System;
using System.Data.SqlClient;

namespace T05ChangeTownNamesCasing
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputCountryName = Console.ReadLine();

            string connection = "Server = .; Database = MinionsDB; Integrated security = true;";
            using SqlConnection sqlconnection = new SqlConnection(connection);
            {
                sqlconnection.Open();

                SqlCommand sqlCommand1 = new SqlCommand(@"SELECT COUNT(*) FROM Countries
                WHERE [Name] = @countryName", sqlconnection);
                sqlCommand1.Parameters.AddWithValue("@countryName", inputCountryName);
                int numOfCountriesWithCurrName = (int)sqlCommand1.ExecuteScalar();

                SqlCommand sqlCommand2 = new SqlCommand(@"SELECT COUNT(*) FROM Towns AS t JOIN Countries AS c ON t.CountryCode = c.Id WHERE c.[Name] = @countryName", sqlconnection);
                sqlCommand2.Parameters.AddWithValue("@countryName", inputCountryName);
                int numOfTowns = (int)sqlCommand2.ExecuteScalar();


                if (numOfCountriesWithCurrName != 0 && numOfTowns != 0)
                {
                    SqlCommand sqlCommand3 = new SqlCommand(@"UPDATE Towns  SET[Name] = UPPER([Name])
                WHERE CountryCode = (SELECT Id FROM Countries WHERE[Name] = @countryName)", sqlconnection);
                    sqlCommand3.Parameters.AddWithValue("@countryName", inputCountryName);
                    int numOfAffectedTowns = sqlCommand3.ExecuteNonQuery();

                    SqlCommand sqlCommand4 = new SqlCommand(@"SELECT STRING_AGG(t.[Name], ', ') FROM Towns AS t JOIN Countries AS c ON t.CountryCode = c.Id WHERE c.[Name] = @countryName", sqlconnection);
                    sqlCommand4.Parameters.AddWithValue("@countryName", inputCountryName);
                    string townsAffectedOutput = (string)sqlCommand4.ExecuteScalar();
                    Console.WriteLine($"{numOfAffectedTowns} town names were affected.");
                    Console.WriteLine($"[{townsAffectedOutput}]");
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }

        }
    }
}
