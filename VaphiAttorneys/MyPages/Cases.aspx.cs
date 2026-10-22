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
    public partial class Cases : System.Web.UI.Page
    {
        readonly string strLegalCon = ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadClientsDropdown();
                LoadStagesDropdown();

                string filter = Request.QueryString["filter"];
                if (filter == "open")
                    LoadCases("Open");
                else if (filter == "closed")
                    LoadCases("Closed");
                else
                    LoadCases(null); // no filter = all
            }
        }


        // NEW - with status param - this fixes your red error
        private void LoadCases(string statusFilter)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetCases", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoginName", DBNull.Value);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Filter here if coming from Dashboard
                        if (!string.IsNullOrEmpty(statusFilter))
                        {
                            DataView dv = dt.DefaultView;
                            dv.RowFilter = $"Status = '{statusFilter}'";
                            gvCases.DataSource = dv;
                        }
                        else
                        {
                            gvCases.DataSource = dt;
                        }

                        gvCases.DataBind();
                    }
                }
            }
        }
        private void LoadCases()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetCases", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoginName", DBNull.Value);
                    con.Open();
                    gvCases.DataSource = cmd.ExecuteReader();
                    gvCases.DataBind();
                }
            }
        }
        private void LoadClients()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetCases", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoginName", null);
                    con.Open();
                    gvCases.DataSource = cmd.ExecuteReader();
                    gvCases.DataBind();
                }
            }
        }

        void LoadClientsDropdown()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                string sql = @"SELECT ClientID, FullName + ' - ' + ISNULL(Phone,'') as DisplayName 
                       FROM Tbl_Clients ORDER BY FullName";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    ddlClients.DataSource = dr;
                    ddlClients.DataTextField = "DisplayName";
                    ddlClients.DataValueField = "ClientID";
                    ddlClients.DataBind();
                }
            }
            ddlClients.Items.Insert(0, new ListItem("-- Select Client --", "0"));
        }

        void LoadStagesDropdown()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                string sql = "SELECT StageID, StageName FROM CaseStages ORDER BY StageOrder";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    ddlStage.DataSource = dr;
                    ddlStage.DataTextField = "StageName";
                    ddlStage.DataValueField = "StageID";
                    ddlStage.DataBind();
                }
            }
            ddlStage.Items.Insert(0, new ListItem("-- Select Case Stage --", "0"));
        }
        protected void BtnSaveCase_Click(object sender, EventArgs e)
        {
            if (ddlClients.SelectedValue == "0")
            {
                lblMessage.Text = "Please select a client"; return;
            }

            using (SqlConnection con = new SqlConnection(strLegalCon))
            using (SqlCommand cmd = new SqlCommand("sp_InsertCase", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientID", ddlClients.SelectedValue);
                cmd.Parameters.AddWithValue("@CaseType", ddlCaseType.SelectedValue);
                cmd.Parameters.AddWithValue("@Status", "Open");

                // These are the court fields you asked about
                cmd.Parameters.AddWithValue("@Court", txtCourtName.Text.Trim()); // Court Name - textbox for court name
                                                                                 // If you have a date picker called txtCourtDate
                DateTime courtDt;
                if (DateTime.TryParse(txtCourtDate.Text, out courtDt))
                    cmd.Parameters.AddWithValue("@CourtDate", courtDt);
                else
                    cmd.Parameters.AddWithValue("@CourtDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CaseTitle", txtDescription.Text.Trim()); // Your description goes to CaseTitle
                    cmd.Parameters.AddWithValue("@CurrentStageID", ddlStage.SelectedValue); // 7 stages
                    cmd.Parameters.AddWithValue("@AssignedTo", Session["LoginName"] ?? "Admin");
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["LoginName"] ?? "Admin");

                con.Open();
                // Only call ONCE
                string newNo = cmd.ExecuteScalar().ToString();
                lblMessage.Text = newNo + " created successfully!";
                con.Close();

                LoadCases(); // refresh grid
                ClearForm();
            }
        }

        void ClearForm()
        {
            ddlClients.SelectedIndex = 0;
            ddlStage.SelectedIndex = 0;
            ddlCaseType.SelectedIndex = 0;
            txtDescription.Text = "";
            txtCourtName.Text = "";
            txtCourtDate.Text = "";
        }
        private void BindClients()
        {
            // SELECT * FROM Tbl_Clients ORDER BY ClientID DESC
        }
    }
}

