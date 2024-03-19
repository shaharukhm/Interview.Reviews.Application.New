using DataModel.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Interview.Reviews.Web.App.DAL.Db_Context
{
    public class UserDbContext
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["InterviewDb"].ConnectionString);


        public List<UserModel> GetUserListToUpdate()
        {
            List<UserModel> UserList = new List<UserModel>();
            SqlCommand cmd = new SqlCommand("select UserId,FullName as 'Full Name',Email,DateOfBirth,Password,StatusId as status,LinkedInUrl,NotApprovalComment from tblUser", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                UserList.Add(new UserModel
                {
                    UserId = Convert.ToInt32(dr[0]),
                    FullName = Convert.ToString(dr[1]),
                    Email = Convert.ToString(dr[2]),
                    DateOfBirth = Convert.ToDateTime(dr[3]),
                    Password = Convert.ToString(dr[4]),
                    UserStatusId = Convert.ToInt32(dr[5]),
                    LinkedInUrl = Convert.ToString(dr[6]),
                    NotApprovalComment = Convert.ToString(dr[7])
                });
            }
            return UserList;
        }

        public List<UserModel> GetPromotionsFromDatabase()
        {
            var list = new List<UserModel>();
            string query = "select UserId,Email,StatusId,LinkedInUrl from tblUser where StatusId = 1 AND LinkedInUrl IS NOT NULL";
            SqlCommand command = new SqlCommand(query, con);
            con.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    UserModel _data = new UserModel()
                    {
                        UserId = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        UserStatusId = reader.GetInt32(2),
                        LinkedInUrl = reader.GetString(3)

                    };
                    list.Add(_data);
                    //reader.Close();
                }
            }
          
            return list;
        }

        public bool UpdatePromotionApproved(UserModel _user)
        {
            string query = "update tblUser SET StatusId = 2 where UserId = @UserId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", _user.UserId);
            cmd.Parameters.AddWithValue("@StatusId", _user.UserStatusId);

            if (con.State == ConnectionState.Closed)
                con.Open();

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close(); // Close the connection after use

            return rowsAffected > 0; // Return true if at least one row was affected
        }

        public bool UpdateRejection(UserModel _user)
        {
            string query = "update tblUser SET NotApprovalComment = @NotApprovalComment";
            if (!string.IsNullOrEmpty(_user.LinkedInUrl))
            {
                query += ", LinkedInUrl = NULL";
            }
            query += " where UserId = @UserId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", _user.UserId);
            cmd.Parameters.AddWithValue("@NotApprovalComment", _user.NotApprovalComment);

            if (con.State == ConnectionState.Closed)
                con.Open();

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close(); // Close the connection after use

            return rowsAffected > 0; // Return true if at least one row was affected
        }


        public bool UpdateUserToGraduate(UserModel _user)
        {

            string query;
            SqlCommand cmd;

            // Check if NotApprovalComment has data
            if (!string.IsNullOrEmpty(_user.NotApprovalComment))
            {
              
                query = "update tblUser SET LinkedInUrl = @LinkedInUrl, NotApprovalComment = NULL where UserId = @UserId";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", _user.UserId);
                cmd.Parameters.AddWithValue("@LinkedInUrl", _user.LinkedInUrl);
            }
            else
            {
                // If it doesn't have data, update without modifying NotApprovalComment
                query = "update tblUser SET LinkedInUrl = @LinkedInUrl where UserId = @UserId";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", _user.UserId);
                cmd.Parameters.AddWithValue("@LinkedInUrl", _user.LinkedInUrl);
            }

            if (con.State == ConnectionState.Closed)
                con.Open();

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close(); // Close the connection after use

            return rowsAffected > 0;

            //string query = "update tblUser SET LinkedInUrl = @LinkedInUrl where UserId = @UserId";

            //SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@UserId", _user.UserId);
            //cmd.Parameters.AddWithValue("@LinkedInUrl", _user.LinkedInUrl);

            //cmd.Parameters.AddWithValue("@NotApprovalComment", _user.NotApprovalComment);

            //if (con.State == ConnectionState.Closed)
            //    con.Open();

            //int rowsAffected = cmd.ExecuteNonQuery();
            //con.Close(); // Close the connection after use

            //return rowsAffected > 0; // Return true if at least one row was affected
        }

        public List<UserStatus> UserRoleList()
        {
            var list = new List<UserStatus>();
            string query = "select StatusId,Status from tblUserStatus";
            SqlCommand command = new SqlCommand(query, con);
            con.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    UserStatus type = new UserStatus()
                    {
                        UserStatusId = reader.GetInt32(0),
                        Status = reader.GetString(1)

                    };
                    list.Add(type);
                }
            }

            return list;
        }

        public bool EditUser(UserModel _user)
        {
            string query = "UPDATE tblUser SET FullName = @FullName, Email = @Email, DateOfBirth = @DateOfBirth WHERE UserId = @UserId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", _user.UserId);
            cmd.Parameters.AddWithValue("@FullName", _user.FullName);
            cmd.Parameters.AddWithValue("@Email", _user.Email);
            cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(_user.DateOfBirth)); // Assuming _user.DateOfBirth is a valid date string

            if (con.State == ConnectionState.Closed)
                con.Open();

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close(); // Close the connection after use

            return rowsAffected > 0; // Return true if at least one row was affected
        }

        public bool RegisterUser(UserModel _user)
        {

            string query = "INSERT INTO tblUser (FullName, Email, DateOfBirth, Password, StatusId) VALUES (@FullName, @Email, @DateOfBirth, @Password, @StatusId)";
            SqlCommand cmd = new SqlCommand(query, con);

            // Add parameters
            cmd.Parameters.AddWithValue("@FullName", _user.FullName);
            cmd.Parameters.AddWithValue("@Email", _user.Email);
            cmd.Parameters.AddWithValue("@DateOfBirth", _user.DateOfBirth);
            cmd.Parameters.AddWithValue("@Password", _user.Password);
            cmd.Parameters.AddWithValue("@StatusId", 1);

            if (con.State == ConnectionState.Closed)
                con.Open();

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Success
                return true;
            }
            else
            {
                // Failure
                return false;
            }
        }

        public List<UserModel> GetUserList()
        {
            List<UserModel> UserList = new List<UserModel>();
            SqlCommand cmd = new SqlCommand("select UserId,FullName as 'Full Name',Email,DateOfBirth,Password,StatusId as status from tblUser", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                UserList.Add(new UserModel
                {
                    UserId = Convert.ToInt32(dr[0]),
                    FullName = Convert.ToString(dr[1]),
                    Email = Convert.ToString(dr[2]),
                    DateOfBirth = Convert.ToDateTime(dr[3]),
                    Password = Convert.ToString(dr[4]),
                    UserStatusId = Convert.ToInt32(dr[5])
                });
            }
            return UserList;
        }

        public List<UserModel> GetUserById(int UserId)
        {
            List<UserModel> userList = new List<UserModel>();
            con.Open();
            string query = "SELECT UserId, FullName, Email, DateOfBirth,Password,StatusId FROM tblUser WHERE UserId = @UserId";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", UserId);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    userList.Add(new UserModel
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        FullName = Convert.ToString(reader["FullName"]),
                        Email = Convert.ToString(reader["Email"]),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Password = Convert.ToString(reader["Password"]),
                        UserStatusId = Convert.ToInt32(reader["StatusId"])
                    });
                }
            }

            return userList;
        }

        public UserModel GetUserDetails(int id)
        {
            UserModel _obj = new UserModel();
            string query = "select u.UserId,u.FullName,u.Email,u.DateOfBirth,u.Password,s.Status from tblUser as u join tblUserStatus s ON u.StatusId = s.StatusId where u.UserId = @UserId ";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", id);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                _obj.UserId = Convert.ToInt32(reader["UserId"]);
                _obj.FullName = Convert.ToString(reader["FullName"]);
                _obj.Email = Convert.ToString(reader["Email"]);
                _obj.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                _obj.Password = Convert.ToString(reader["Password"]);

                _obj.Status = new UserStatus
                {
                    Status = reader["Status"].ToString()
                };
            }
            reader.Close();
            return _obj;
        }

        public void DeleteUserByID(int id)
        {
            string query = "DELETE FROM tblUser WHERE UserId = @UserId";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@UserId", id);

            con.Open();
            command.ExecuteNonQuery();
        }

        /// Admin Db Context ///
        /// 
        /// Admin Dashboard
        public List<UserModel> GetGraduatesFromDatabase()
        {
            List<UserModel> graduates = new List<UserModel>();

            string query = "select u.UserId,u.FullName,u.Email,u.DateOfBirth,u.Password,u.StatusId from tblUser as u join tblUserStatus as s ON u.StatusId = s.StatusId where s.Status = 'Graduate'";
            SqlCommand command = new SqlCommand(query, con);

            con.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                UserModel graduate = new UserModel();
                graduate.UserId = Convert.ToInt32(reader["UserId"]);
                graduate.FullName = reader["FullName"].ToString();
                graduate.Email = reader["Email"].ToString();
                graduate.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                graduate.Password = reader["Password"].ToString();
                graduate.UserStatusId = Convert.ToInt32(reader["StatusId"]);
                // Populate other properties as needed
                graduates.Add(graduate);
            }

            reader.Close();
            return graduates;
        }

        public List<UserModel> GetAdminsFromDatabase()
        {
            List<UserModel> Admins = new List<UserModel>();
            string query = "select u.UserId,u.FullName,u.Email,u.DateOfBirth,u.Password,u.StatusId from tblUser as u join tblUserStatus as s ON u.StatusId = s.StatusId where s.Status = 'Admin'";
            SqlCommand command = new SqlCommand(query, con);

            con.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                UserModel admin = new UserModel();
                admin.UserId = Convert.ToInt32(reader["UserId"]);
                admin.FullName = reader["FullName"].ToString();
                admin.Email = reader["Email"].ToString();
                admin.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                admin.Password = reader["Password"].ToString();
                admin.UserStatusId = Convert.ToInt32(reader["StatusId"]);
                // Populate other properties as needed
                Admins.Add(admin);
            }
            reader.Close();
            return Admins;
        }

        public List<UserModel> GetUsersFromDatabase()
        {
            List<UserModel> Users = new List<UserModel>();
            string query = "select u.UserId,u.FullName,u.Email,u.DateOfBirth,u.Password,u.StatusId from tblUser as u join tblUserStatus as s ON u.StatusId = s.StatusId where s.Status = 'User'";
            SqlCommand command = new SqlCommand(query, con);

            con.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                UserModel user = new UserModel();
                user.UserId = Convert.ToInt32(reader["UserId"]);
                user.FullName = reader["FullName"].ToString();
                user.Email = reader["Email"].ToString();
                user.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                user.Password = reader["Password"].ToString();
                user.UserStatusId = Convert.ToInt32(reader["StatusId"]);
                // Populate other properties as needed
                Users.Add(user);
            }
            reader.Close();
            return Users;
        }

        public List<ReviewModel> GetReviewsFromDatabase()
        {
            List<ReviewModel> Reviews = new List<ReviewModel>();

            string query = "select r.ReviewId,r.CompanyName,r.Content,r.Rating,r.TimeStamb,r.fk_UserId from tblUser as u right join tblReview r ON u.UserId = r.fk_UserId";
            SqlCommand command = new SqlCommand(query, con);

            con.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReviewModel _review = new ReviewModel();
                _review.ReviewId = Convert.ToInt32(reader["ReviewId"]);
                _review.CompanyName = reader["CompanyName"].ToString();
                _review.Content = reader["Content"].ToString();
                _review.Rating = Convert.ToInt32(reader["Rating"]);
                _review.UserId = Convert.ToInt32(reader["fk_UserId"]);
                // Populate other properties as needed
                Reviews.Add(_review);
            }

            reader.Close();


            return Reviews;
        }

        public int GetUserCount(string tblUser)
        {
            int count = 0;
            con.Open();

            // Execute query to get count for the specified table
            SqlCommand command = new SqlCommand($"select COUNT(*) from {tblUser} as u inner join tblUserStatus s ON u.StatusId = s.StatusId where s.Status = 'User'", con);
            count = (int)command.ExecuteScalar();
            con.Close();
            return count;
        }

        public int GetGraduatesCount(string tblUser)
        {
            int count = 0;
            con.Open();

            // Execute query to get count for the specified table
            SqlCommand command = new SqlCommand($"select COUNT(*) from {tblUser} as u inner join tblUserStatus s ON u.StatusId = s.StatusId where s.Status = 'Graduate'", con);
            count = (int)command.ExecuteScalar();
            con.Close();
            return count;
        }

        public int GetAdminCount(string tblUser)
        {
            int count = 0;
            con.Open();

            // Execute query to get count for the specified table
            SqlCommand command = new SqlCommand($"select COUNT(*) from {tblUser} as u inner join tblUserStatus s ON u.StatusId = s.StatusId where s.Status = 'Admin'", con);
            count = (int)command.ExecuteScalar();
            con.Close();
            return count;
        }

        public int GetReviewCount(string tblReview)
        {
            int count = 0;
            con.Open();

            // Execute query to get count for the specified table
            SqlCommand command = new SqlCommand($"select COUNT(r.ReviewId) from tblUser as u inner join {tblReview} as r ON u.UserId = r.fk_UserId", con);
            count = (int)command.ExecuteScalar();
            con.Close();
            return count;
        }

        /// 

        //        update tblUser SET Password = '' where Email = ''
        public void UpdatePassword(string email, string newPassword)
        {
            string updateQuery = "UPDATE tblUser SET Password = @NewPassword WHERE Email = @Email";


            SqlCommand command = new SqlCommand(updateQuery, con);
            {
                command.Parameters.AddWithValue("@NewPassword", newPassword);
                command.Parameters.AddWithValue("@Email", email);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public int GetPromotionRequestCount(string tblUser)
        {
            int count = 0;
            con.Open();

            // Execute query to get count for the specified table
            SqlCommand command = new SqlCommand($"SELECT count(*) FROM {tblUser} WHERE LinkedInUrl IS NOT NULL and StatusId = 1", con);
            count = (int)command.ExecuteScalar();
            con.Close();
            return count;
        }
    }

}
