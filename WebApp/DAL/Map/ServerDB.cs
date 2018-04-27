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
    public class ServerDB
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;

        public ServerDB()
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
        }
        public Servers SelectAll()
        {
            command.CommandText = string.Format("SELECT * FROM Servers");
            Servers servers = new Servers();
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                Server server = null;
                while (reader.Read())
                {
                    server = new Server();
                    server.ID = (int)reader["Id"];
                    server.Name = (string)reader["Name"];
                    server.HostUsername = (string)reader ["HostUsername"];
                    server.OfferData = (string)reader["OfferData"];
                    servers.Add(server);
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
            return servers;
        }

        public Server SelectById(int Id)
        {
            command.CommandText = string.Format("SELECT * FROM Servers WHERE id = {0}", Id);
            Server server = null;
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    server = new Server();
                    server.ID = (int)reader["Id"];
                    server.Name = (string)reader["Name"];
                    server.HostUsername = (string)reader["HostUsername"];
                    server.OfferData = (string)reader["OfferData"];
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
            return server;
        }
        public Servers SelectByUsername(string Username)
        {
            command.CommandText = string.Format("SELECT * FROM Servers WHERE HostUsername = '{0}'", Username);
            Servers servers = new Servers();
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                Server server = null ;
                while (reader.Read())
                {
                    server = new Server();
                    server.ID = (int)reader["Id"];
                    server.Name = (string)reader["Name"];
                    server.HostUsername = (string)reader["HostUsername"];
                    server.OfferData = (string)reader["OfferData"];
                    servers.Add(server);
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
            return servers;
        }

        public bool InsertServer(Server server)
        {
            bool isSuccess = false;
            if (true)
            {
                isSuccess = true;
                command.CommandText = string.Format("INSERT INTO Servers (Name,HostUsername,OfferData) VALUES ('{0}','{1}','{2}');", server.Name, server.HostUsername, server.OfferData);
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
        public bool DeleteServer(int ID)
        {
            bool isSuccess = false;
            if (true)
            {
                isSuccess = true;
                command.CommandText = string.Format("DELETE FROM Servers WHERE Id = '{0}';", ID);
                try
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    // every loop we read line from the database
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: Insert failed! info: '{0}'", ID);
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
        public bool DeleteServer(string HostUsername)
        {
            bool isSuccess = false;
            if (true)
            {
                isSuccess = true;
                command.CommandText = string.Format("DELETE FROM Servers WHERE HostUsername = '{0}';", HostUsername);
                try
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    // every loop we read line from the database
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: Insert failed! info: '{0}'", HostUsername);
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
