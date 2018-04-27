using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using DAL.Map;
namespace BL
{
    public class BlMain
    {
        // Users:
        public static int RegisterUser(string username, string password, string email, int countryIndex)
        {
            int flag = 0;
            UserDB userDB = new UserDB();
            CountryDB countryDB = new CountryDB();
            string countryName = countryDB.SelectById(countryIndex).Name;
            User newUser = new User();
            newUser.Username = username;
            newUser.Password = password;
            newUser.Email = email;
            newUser.CountryName = countryName;
            
            bool s = userDB.InsertUser(newUser);

            if (s)
                flag = 1;         
            else 
                flag = 0;

            return flag;
        }
        public static int RegisterUser(string username, string password, string email, string countryName)
        {
            int flag = 0;
            UserDB userDB = new UserDB();
            CountryDB countryDB = new CountryDB();
            Country country = countryDB.SelectByName(countryName);
            if (country != null)
            {
                User newUser = new User();
                newUser.Username = username;
                newUser.Password = password;
                newUser.Email = email;
                newUser.CountryName = countryName;

                bool s = userDB.InsertUser(newUser);
                if (s)
                    flag = 1;
                else
                    flag = 0;

            }
            return flag;
        }
        public static string GetUserInfo(string username,string password)
        {
            UserDB userDB = new UserDB ();
            User user = userDB.SelectByName(username);
            if (user == null)
            {
                return null;
            }
            else
            {
                if (password == user.Password)
                    return "{\"id\":\""+ user.Id +"\",\"username\":\"" + user.Username + "\",\"password\":\"" + user.Password + "\",\"email\":\"" + user.Email + "\",\"countryName\":\"" + user.CountryName + "\"}";
                else
                    return null;
            }
            
        }
        public static int LoginUser(string username, string password)
        {
            int flag = 0;
            UserDB userDB = new UserDB();
            User user = userDB.SelectByName(username);

            if (user != null)
                if (user.Password == password)
                {

                     flag = 1;

                }
            return flag;
        }
        // Countries:
        public static string[] GetCountries()
        {
            CountryDB countryDB = new CountryDB();
            Countries countries = countryDB.SelectCountries();
            string[] countryNames = new string[countries.Count];
            for (int i = 0; i < countries.Count; i++)
            {
                countryNames[i] = countries[i].Name;
            }
            return countryNames;
        }
        // Servers:
        public static int InsertServer(string name,string hostUsername,string offerData)
        {
            ServerDB serverDB = new ServerDB();
            Server server = new Server();
            int room_id = -1;
            if(server != null)
            {
                server.Name = name;
                server.HostUsername = hostUsername;
                server.OfferData = offerData;
                if (serverDB.InsertServer(server))
                {
                    Servers servers = serverDB.SelectByUsername(hostUsername);
                    for (int i = 0; i < servers.Count; i++)
                    {
                        Server ser = servers[i];
                        if (ser.Name == name && ser.HostUsername == hostUsername && ser.OfferData == offerData)
                        {
                            room_id = ser.ID;
                        }
                    }
                }
            }
            return room_id;
        }
        public static int DeleteServer(int ID)
        {
            int flag = 0;
            ServerDB serverDB = new ServerDB();
        
            bool s = serverDB.DeleteServer(ID);
            if (s)
                flag = 1;
            else
                flag = 0;
            return flag;
        }
        public static int DeleteServer(string HosterName)
        {
            int flag = 0;
            ServerDB serverDB = new ServerDB();

            bool s = serverDB.DeleteServer(HosterName);
            if (s)
                flag = 1;
            else
                flag = 0;
            return flag;
        }
        public static string[] GetAllServers()
        {
            
            ServerDB serverDB = new ServerDB();
            Servers servers = serverDB.SelectAll();
            string[] serversSTR = new string[servers.Count];
            if (servers == null || servers.Count == 0)
            {
                return null;
            }
            else
            {
                string str = null ;
               
                for (int i = 0; i < servers.Count; i++)
                {
                    Server server = servers[i];
                    str = "{\"ID\":\"" + server.ID + "\",\"Name\":\"" + server.Name + "\",\"HostUsername\":\"" + server.HostUsername + "\",\"OfferData\":[" + server.OfferData + "]" + "}";
                    serversSTR[i] = str;
                    str = null;
                }
            }
            return serversSTR;
        }
        public static string GetServerById(int Id)
        {
            ServerDB serverDB = new ServerDB();
            Server server = serverDB.SelectById(Id);
            string str = null;
            if(server != null){
                str = "{\"ID\":\"" + server.ID + "\",\"Name\":\"" + server.Name + "\",\"HostUsername\":\"" + server.HostUsername + "\",\"OfferData\":[" + server.OfferData + "]" +"}";
            }
            else
                return null;
            return str;
        }
        // Answers:
        public static string[] GetAnswers(int room_id)
        {
            AnswerDB answerDB = new AnswerDB();
            Answers answers = answerDB.SelectByRoomId(room_id);
            if (answers != null)
            {
                string[] answerSTR = new string[answers.Count];
                string str = null;
                for (int i = 0; i < answers.Count; i++)
                {
                    Answer answer = answers[i];
                    str = "{\"Id\":\"" + answer.Id + "\",\"Sender\":\"" + answer.Sender + "\",\"Room_Id\":\"" + answer.Room_Id + "\",\"AnswerData\":[" + answer.AnswerData + "]}";
                    answerSTR[i] = str;
                    str = null;
                }
                return answerSTR;
            }
            else
            return null;
        }
        public static int InsertAnswer(string Sender, int Room_Id, string answerData)
        {
            int id = -1;
            AnswerDB answerDB = new AnswerDB();
            Answer answer = new Answer();
            if (answer != null)
            {
                answer.Sender = Sender;
                answer.Room_Id = Room_Id;
                answer.AnswerData = answerData;

                id = answerDB.InsertAnswer(answer);
                
            }
            return id;
        }
        public static int DeleteAnswer(int Id)
        {
            int flag = 0;
            AnswerDB answerDB = new AnswerDB();

            bool s = answerDB.DeleteAnswer(Id);
            if (s)
                flag = 1;
            else
                flag = 0;
            return flag;
        }

    }
}
