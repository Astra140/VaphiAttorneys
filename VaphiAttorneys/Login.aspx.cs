using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using WebGrease.Activities;
using BCrypt.Net;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VaphiAttorneys
{
    public partial class Login : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString;
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID, Username, Password FROM dbo.Tbl_Users WHERE Username=@U", con);
                cmd.Parameters.AddWithValue("@U", txtUsername.Text.Trim());
                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    string stored = dt.Rows[0]["Password"].ToString().Trim();
                    string input = txtPassword.Text.Trim();
                    bool ok = false;

                    if (stored.StartsWith("$2a$") || stored.StartsWith("$2b$"))
                    {
                        try { ok = BCrypt.Net.BCrypt.Verify(input, stored); } catch { ok = false; }
                    }
                    else
                    {
                        if (input == stored)
                        {
                            ok = true;
                            string newHash = BCrypt.Net.BCrypt.HashPassword(input);
                            SqlCommand upd = new SqlCommand("UPDATE dbo.Tbl_Users SET Password=@H WHERE ID=@ID", con);
                            upd.Parameters.AddWithValue("@H", newHash);
                            upd.Parameters.AddWithValue("@ID", dt.Rows[0]["ID"]);
                            upd.ExecuteNonQuery();
                        }
                    }

                    if (ok)
                    {
                        Session["UserID"] = dt.Rows[0]["ID"].ToString();
                        Session["Username"] = dt.Rows[0]["Username"].ToString();
                        // IMPORTANT - Your screenshot shows Dashboard is inside MyPages
                        Response.Redirect("~/MyPages/Dashboard.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                }
                lblError.Text = "Invalid login";
            }
        }
        /*
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID, Username, Password FROM dbo.Tbl_Users WHERE Username=@U", con);
                cmd.Parameters.AddWithValue("@U", txtUsername.Text.Trim());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    string stored = dt.Rows[0]["Password"].ToString().Trim();
                    bool ok = false;

                    if (stored.StartsWith("$2a$") || stored.StartsWith("$2b$"))
                    {
                        ok = BCrypt.Net.BCrypt.Verify(txtPassword.Text.Trim(), stored);
                    }
                    else // plain 1234 / vaphi2026 - your current case
                    {
                        if (txtPassword.Text.Trim() == stored)
                        {
                            ok = true;
                            // upgrade to hash
                            string newHash = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text.Trim());
                            SqlCommand upd = new SqlCommand("UPDATE dbo.Tbl_Users SET Password=@H WHERE ID=@ID", con);
                            upd.Parameters.AddWithValue("@H", newHash);
                            upd.Parameters.AddWithValue("@ID", dt.Rows[0]["ID"]);
                            upd.ExecuteNonQuery();
                        }
                    }

                    if (ok)
                    {
                        Session["UserID"] = dt.Rows[0]["ID"];
                        Session["Username"] = dt.Rows[0]["Username"];
                        Response.Redirect("~/MyPages/Dashboard.aspx");
                        return;
                    }
                }
                lblError.Text = "Invalid login";
            }
        }
        */
        protected void btnHashAll_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand sel = new SqlCommand("SELECT ID, Password FROM dbo.Tbl_Users", con);
                SqlDataReader r = sel.ExecuteReader();
                var users = new List<Tuple<int, string>>();
                while (r.Read()) users.Add(new Tuple<int, string>((int)r["ID"], r["Password"].ToString()));
                r.Close();

                foreach (var u in users)
                {
                    if (!u.Item2.StartsWith("$2"))
                    {
                        string hash = BCrypt.Net.BCrypt.HashPassword(u.Item2.Trim());
                        SqlCommand upd = new SqlCommand("UPDATE dbo.Tbl_Users SET Password=@H WHERE ID=@ID", con);
                        upd.Parameters.AddWithValue("@H", hash);
                        upd.Parameters.AddWithValue("@ID", u.Item1);
                        upd.ExecuteNonQuery();
                    }
                }
            }
            lblError.Text = "All passwords hashed! Len should be 60 now.";
        }
    }
}




