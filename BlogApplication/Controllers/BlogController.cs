using BlogApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace BlogApplication.Controllers
{
    public class BlogController : ApiController
    {
        public IHttpActionResult AddBlogUser(UserModel model)
        {
            CreateCommand("exec ADDUSER '" + model.Login + "','" + model.Name + "'," + model.usertype + ",'" + model.email + "','" + model.password + "'");
            return Json(new { Success = "Registeration Successful" });
        }

        public IHttpActionResult LoginUser(string username, string pwd)
        {
            var token = RandomString();
            var tokendata = gettokendata("exec FINDLOGIN '" + username + "','" + pwd + "','" + token + "'");
            return Json(new { Success = "Login Successful" , tokendata = tokendata });
        }

        public IHttpActionResult AddPost(BlogPostModel model)
        {
            var tokendata = gettokendata("GETLOGIN '"+ model.tokendata +"'");

            if (tokendata == "")
            {
                return Json(new { Success = "Your Request is invalid" });
            }
            else
            {
                var id = Convert.ToInt32(tokendata);
                CreateCommand("exec ADDpost " + id + ", '" + model.postContent + "', 0");
                return Json(new { Success = "Your Post Added successfully" });
            }
        }

        public IHttpActionResult EditPost(BlogPostModel model)
        {
            var tokendata = gettokendata("GETLOGIN '" + model.tokendata + "'");

            if (tokendata == "")
            {
                return Json(new { Success = "Your Request is invalid" });
            }
            else
            {
                var id = Convert.ToInt32(tokendata);
                CreateCommand("exec ADDpost " + id + ", '" + model.postContent + "', "+ model.id +"");
                return Json(new { Success = "Your Post Edited successfully" });
            }
        }

        public IHttpActionResult Delete(int id)
        {
            return Json(new { Success = "Your Post Deleted successfully" });
        }

        public IHttpActionResult Comment(BlogPostModel model)
        {
            var tokendata = gettokendata("GETLOGINUSER '" + model.tokendata + "'");

            if (tokendata == "")
            {
                return Json(new { Success = "Your Request is invalid" });
            }
            else
            {
                var id = Convert.ToInt32(tokendata);
                CreateCommand("exec ADDComment " + id + ", '" + model.postContent + "', " + model.id + "");
                return Json(new { Success = "Your Comment added successfully" });
            }
        }
           

        private static void CreateCommand(string queryString)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private string gettokendata(string queryString)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter(queryString, connection);
                DataTable d = new DataTable();
                sqlDA.Fill(d);
                string token = string.Empty;

                if(d.Rows.Count > 0)
                {
                    token = d.Rows[0]["Tokendata"].ToString();
                }
                else
                {
                    token = "";
                }
                
                return token;
            }
        }

        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 25)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
