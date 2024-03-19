using DataModel.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Interview.Reviews.Web.App.DAL.Db_Context
{
    public class ReviewDbContext
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["InterviewDb"].ConnectionString);

        public List<ReviewModel> GetReviewsList()
        {
            List<ReviewModel> reviewList = new List<ReviewModel>();
            SqlCommand cmd = new SqlCommand("select r.ReviewId,r.CompanyName,r.Content,r.Rating,r.TimeStamb, u.FullName from tblUser as u right join tblReview r ON u.UserId = r.fk_UserId", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                reviewList.Add(new ReviewModel
                {
                    ReviewId = Convert.ToInt32(dr[0]),
                    CompanyName = Convert.ToString(dr[1]),
                    Content = Convert.ToString(dr[2]),
                    Rating = Convert.ToInt32(dr[3]),
                    TimeStamb = Convert.ToDateTime(dr[4]),

                    Author = new UserModel
                    {
                        FullName = Convert.ToString(dr[5])
                    }
                });
            }
            return reviewList;
        }


        public List<ReviewModel> SearchByCompanyName(string companyName)
        {
            List<ReviewModel> reviewList = new List<ReviewModel>();

            string query = "SELECT * FROM tblReview WHERE CompanyName LIKE @CompanyName";

            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@CompanyName", "%" + companyName + "%");

            con.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                reviewList.Add(new ReviewModel
                {
                    ReviewId = reader.GetInt32(0),
                    CompanyName = reader.GetString(1),
                    Content = reader.GetString(2),
                    Rating = reader.GetInt32(3),
                    TimeStamb = reader.GetDateTime(4),
                    UserId = Convert.ToInt32(5)
                });
            }
            return reviewList;
        }

        public bool InsertReview(ReviewModel _obj)
        {
            string query = "insert into tblReview(CompanyName,Content,Rating,fk_UserId,TimeStamb) values(@CompanyName,@Content,@Rating,@fk_UserId,GETDATE()) ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@CompanyName", _obj.CompanyName);
            cmd.Parameters.AddWithValue("@Content", _obj.Content);
            cmd.Parameters.AddWithValue("@Rating", _obj.Rating);
            cmd.Parameters.AddWithValue("@fk_UserId", _obj.UserId);

            if (con.State == ConnectionState.Closed)
                con.Open();

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close(); // Close the connection after use

            return rowsAffected > 0; // Return true if at least one row was affected
        }

        public bool UpdateReview(ReviewModel _obj)
        {
            string query = "UPDATE tblReview SET CompanyName = @CompanyName, Content = @Content, Rating = @Rating , TimeStamb = GETDATE() WHERE ReviewId = @ReviewId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@CompanyName", _obj.CompanyName);
            cmd.Parameters.AddWithValue("@Content", _obj.Content);
            cmd.Parameters.AddWithValue("@Rating", _obj.Rating);
            cmd.Parameters.AddWithValue("@ReviewId", _obj.ReviewId); // Assuming ReviewId is the primary key or unique identifier for a review

            if (con.State == ConnectionState.Closed)
                con.Open();

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close(); // Close the connection after use

            return rowsAffected > 0; // Return true if at least one row was affected
        }

        public void DeleteReviewByID(int id)
        {
            string query = "DELETE FROM tblReview WHERE ReviewId = @ReviewId";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@ReviewId", id);

            con.Open();
            command.ExecuteNonQuery();
        }
    }
}
