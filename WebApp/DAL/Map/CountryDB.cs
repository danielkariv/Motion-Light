using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Map
{
    public class CountryDB
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;
        public CountryDB()
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
        }
        
        public Country SelectByName(string name)
        {
            command.CommandText = string.Format("SELECT * FROM Countries WHERE Name = '{0}' ", name);
            Country country = null;
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    country = new Country();
                    country.Id = (int)reader["Id"];
                    country.Name = (string)reader["name"];
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return country;
        }
        public Country SelectById(int Id)
        {
            command.CommandText = string.Format("SELECT * FROM Countries WHERE id = {0}", Id);
            Country country = null;
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    country = new Country();
                    country.Id = (int)reader["Id"];
                    country.Name = (string)reader["name"];
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return country;
        }
        public Countries SelectCountries()
        {
            command.CommandText = string.Format("SELECT * FROM Countries");
            Countries countries = new Countries();
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                Country country = null;
                while (reader.Read())
                {
                    country = new Country();
                    country.Id = (int)reader["Id"];
                    country.Name = (string)reader["name"];
                    countries.Add(country);
                    country = null;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return countries;
        }
        public bool InsertCountry(Country country)
        {
            bool isSuccess = false;
            if (SelectByName(country.Name) == null)
            {
                isSuccess = true;
                command.CommandText = string.Format("INSERT INTO Countries (name) VALUES ('{0}');", country.Name);
                try
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    // every loop we read line from the database
                }
                catch (Exception)
                {
                    isSuccess = false;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

            }
            return isSuccess;
        }

    }
}
