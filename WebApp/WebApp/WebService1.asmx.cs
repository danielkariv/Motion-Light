using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using BL;
namespace WebApp
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService()]

    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod]
        public int RegisterUser(string username, string password, string email, string countryName)
        {
            int flag;
            flag = BlMain.RegisterUser(username, password, email, countryName);
            return flag;
        }
        [WebMethod]
        public string GetUserInfo(string username, string password)
        {
            string str = null;
            str = BlMain.GetUserInfo(username, password);
            return str;
        }
        [WebMethod]
        public int LoginUser(string username, string password)
        {
            int flag = BlMain.LoginUser(username, password);
            return flag;
        }
        [WebMethod]
        public string[] GetCountries()
        {
            string[] strs = BlMain.GetCountries();
            return strs;
        }
        
        [WebMethod]
        public string GetServer(int id)
        {
            string str = BlMain.GetServerById(id);
            return str;
        }
        [WebMethod]
        public string[] GetAllServers()
        {
            string[] strs = BlMain.GetAllServers();
            return strs;
        }
        [WebMethod]
        public int InsertServer(string name,string hostUsername,string offerData)
        {
            int room_id = BlMain.InsertServer(name, hostUsername, offerData);
            return room_id;
        }
        [WebMethod]
        public int DeleteServer(int id)
        {
            int flag = BlMain.DeleteServer(id);
            return flag;
        }
        [WebMethod]
        public int DeleteServerByHosterName(string hosterName)
        {
            int flag = BlMain.DeleteServer(hosterName);
            return flag;
        }
        [WebMethod]
        public string[] GetAnswers(int room_id)
        {
            string[] strs = BlMain.GetAnswers(room_id);
            return strs;
        }
        [WebMethod]
        public int DeleteAnswer(int id)
        {
            int flag = BlMain.DeleteAnswer(id);
            return flag;
        }
        [WebMethod]
        public int InsertAnswer(string sender,int room_id,string answerData)
        {
            int id = BlMain.InsertAnswer(sender, room_id, answerData);
            return id;
        }
    }
}
