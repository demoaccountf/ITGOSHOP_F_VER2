using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Models
{
    public class ITGoShopContext
    {
        public string ConnectionString { get; set; }

        public ITGoShopContext(string connectionString) 
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public User getUserInfo(string email, string password)
        {
            User userInfo = new User();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "select * from User where Email = @email AND Password = @password";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("password", password);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userInfo.UserId = Convert.ToInt32(reader["UserId"]);
                        userInfo.FirstName = reader["FirstName"].ToString();
                        userInfo.LastName = reader["LastName"].ToString();
                        userInfo.Mobile = reader["Mobile"].ToString();
                        userInfo.UserImage = reader["UserImage"].ToString();
                        userInfo.Admin = Convert.ToInt32(reader["Admin"]);
                    }
                    else
                        return null;
                }
            }
            return userInfo;
        }

        public void updateLastLogin(int userId)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "UPDATE USER SET LastLogin = SYSDATE() where UserId = @userId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
