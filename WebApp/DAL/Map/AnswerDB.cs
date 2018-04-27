using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using System.Data;

namespace DAL.Map
{
    public class AnswerDB
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;
        public AnswerDB()
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
        }

        public Answers SelectByRoomId(int room_id)
        {
            command.CommandText = string.Format("SELECT * FROM Answers WHERE Room_Id ='{0}'", room_id);
            Answers answers = new Answers();
            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();
                Answer answer = null;
                while (reader.Read())
                {
                    answer = new Answer();
                    answer.Id = (int)reader["Id"];
                    answer.Sender = (string)reader["Sender"];
                    answer.Room_Id = (int)reader["Room_Id"];
                    answer.AnswerData = (string)reader["AnswerData"];
                    answers.Add(answer);
                    answer = null;
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
            return answers;
        }
        public int InsertAnswer(Answer answer)
        {
            int id = -1;
            if (true)
            {
                
                command.CommandText = string.Format("INSERT INTO Answers (Sender,Room_Id,AnswerData) VALUES ('{0}','{1}','{2}');", answer.Sender, answer.Room_Id, answer.AnswerData);
                try
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    // every loop we read line from the database
                    Answers answers = SelectByRoomId(answer.Room_Id);
                    for (int i = 0; i < answers.Count; i++)
                    {
                        if (answers[i].Sender == answer.Sender && answers[i].AnswerData == answer.AnswerData)
                            id = answers[i].Id;
                    }
                }
                catch (Exception)
                {
                    
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

            }
            return id;
        }
        // Doesnt work, it is broken
        public bool DeleteAnswer(int Id)
        {
            bool isSuccess = false;
            if (true)
            {
                isSuccess = true;
                command.CommandText = string.Format("DELETE FROM Answers WHERE Id = '{0}';", Id);
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
