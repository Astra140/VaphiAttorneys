using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VaphiAttorneys.MyPages
{
    public partial class Clients : System.Web.UI.Page
    {
        readonly string strLegalCon = ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoginName"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack)
            {
                LoadClients();
            }
        }
        private void LoadClients()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Tbl_Clients", con);
                con.Open();
                gvClients.DataSource = cmd.ExecuteReader();
                gvClients.DataBind();
            }
        }

        protected void BtnSaveClient_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertClient", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                cmd.Parameters.AddWithValue("@IDNumber", txtIDNumber.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@CreatedBy", "Admin");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            // THIS is the fix - rebind after save
            LoadClients();

            // clear form
            txtFullName.Text = "";
            txtIDNumber.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
        }
        private void BindClients()
        {
            // SELECT * FROM Tbl_Clients ORDER BY ClientID DESC
        }
    }
}

