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
    public class UserDB
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;
        private CountryDB countryDB;
        public UserDB()
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
            countryDB = new CountryDB();
        }
        public Users SelectAllByName(string name)
        {
            command.CommandText = string.Format("SELECT * FROM Users WHERE username LIKE '{0}%' ", name);
            Users users = new Users();
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                User user = null;
                while (reader.Read())
                {
                    user = new User();
                    user.Id = (int)reader["Id"];
                    user.Username = (string)reader["username"];
                    user.Password = (string)reader["password"];
                    user.Email = (string)reader["email"];
                    user.CountryName = countryDB.SelectById((int)reader["countryindex"]).Name;
                    users.Add(user);
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
            return users;
        }
        public User SelectByName(string name)
        {
            command.CommandText = string.Format("SELECT * FROM Users WHERE username ='{0}'", name);
            User user = null;
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    user = new User();
                    user.Id = (int)reader["Id"];
                    user.Username = (string)reader["username"];
                    user.Password = (string)reader["password"];
                    user.Email = (string)reader["email"];
                    user.CountryName = countryDB.SelectById((int)reader["countryindex"]).Name;
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
            return user;
        }
        public User SelectById(int Id)
        {
            command.CommandText = string.Format("SELECT * FROM Users WHERE id = {0}", Id);
            User user = null;
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user = new User();
                    user.Id = (int)reader["Id"];
                    user.Username = (string)reader["username"];
                    user.Password = (string)reader["password"];
                    user.Email = (string)reader["email"];
                    user.CountryName = countryDB.SelectById((int)reader["countryindex"]).Name;
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
            return user;
        }

        public bool InsertUser(User user)
        {
            bool isSuccess = false;
            if (SelectByName(user.Username) == null)
            {
                isSuccess = true;
                command.CommandText = string.Format("INSERT INTO Users (username,password,email,countryindex) VALUES ('{0}','{1}','{2}','{3}');", user.Username, user.Password, user.Email,countryDB.SelectByName(user.CountryName).Id,"offline");
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
        public bool UpdateByName(string name, User user)
        {
            bool isSuccess = false;
            if (SelectByName(name) != null)
            {
                isSuccess = true;
                command.CommandText = string.Format("UPDATE Users SET username='{0}', password='{1}', email='{2}',countryindex='{3}' WHERE username='{4}'", user.Username, user.Password, user.Email, countryDB.SelectByName(user.CountryName).Id,name);
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
        public bool UpdateById(int Id, User user)
        {
            bool isSuccess = false;
            if (SelectById(Id) != null)
            {
                isSuccess = true;
                command.CommandText = string.Format("UPDATE Users SET username='{0}', password='{1}', email='{2}',countryindex='{3}' WHERE id='{4}'", user.Username, user.Password, user.Email, countryDB.SelectByName(user.CountryName).Id, Id);
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
