using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VaphiAttorneys.MyPages
{
    public partial class ClientDetails : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 1. Get ID from URL: ClientDetails.aspx?ClientID=10
                string id = Request.QueryString["ClientID"];
                if (string.IsNullOrEmpty(id))
                {
                    Response.Redirect("Clients.aspx");
                    return;
                }

                // 2. Default state: View mode
                pnlView.Visible = true;
                pnlEdit.Visible = false;

                // 3. Load data ONCE
                LoadClient(id);
                LoadClientCases(id);
            }
            // On PostBack (Edit, Save, Cancel clicks), do NOTHING here.
            // Let the button click methods handle panel switching.
        }
        private void LoadClient(string clientId)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                // Use parameter - never concat ID, prevents SQL injection + type errors
                SqlCommand cmd = new SqlCommand("SELECT * FROM Tbl_Clients WHERE ClientID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", clientId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read()) // <-- THIS IS THE FIX, moves to first record
                    {
                        // VIEW - check column names exist, your table has FullName not FullNames
                        lblClientName.Text = dr["FullName"].ToString();
                        lblFullNames.Text = dr["FullName"].ToString();
                        lblIDNumber.Text = dr["IDNumber"].ToString();

                        // with null check like your CaseDetails
                        lblPhone.Text = dr["Phone"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["Phone"].ToString()) ? "Not Set" : dr["Phone"].ToString();
                        lblEmail.Text = dr["Email"] == DBNull.Value ? "Not Set" : dr["Email"].ToString();
                        lblAddress.Text = dr["Address"] == DBNull.Value ? "Not Set" : dr["Address"].ToString();
                        lblClientNo.Text = "Client #" + clientId;

                        // EDIT - so edit form is pre-filled
                        txtFullNames.Text = dr["FullName"].ToString();
                        txtIDNumber.Text = dr["IDNumber"].ToString();
                        txtPhone.Text = dr["Phone"].ToString();
                        txtEmail.Text = dr["Email"].ToString();
                        txtAddress.Text = dr["Address"].ToString();
                    }
                    else
                    {
                        // No record found for ClientID=11
                        lblClientName.Text = "Client Not Found";
                        lblEditMsg.Text = "No record found for ClientID = " + clientId;
                    }
                }
            }
        }
        protected string GetBackUrl()
        {
            string source = Request.QueryString["source"];
            if (source == "Dashboard")
                return "Dashboard.aspx";

            // check where user came from
            if (Request.UrlReferrer != null && Request.UrlReferrer.ToString().Contains("Dashboard"))
                return "Dashboard.aspx";

            // default
            if (Request.Path.Contains("CaseDetails"))
                return "Cases.aspx";
            else
                return "Clients.aspx";
        }
        private void LoadClientCases(string clientId)
        {
            //string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["VaphiConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT *, StageName 
                         FROM Tbl_Cases c 
                            left join CaseStages s on c.CurrentStageID = s.StageID
                         WHERE ClientID = " + clientId +"ORDER BY CaseID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvClientCases.DataSource = dt;
                        gvClientCases.DataBind();
                    }
                }
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            pnlView.Visible = false;
            pnlEdit.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // your save logic here
            lblEditMsg.Text = "Saved!";
            pnlView.Visible = true;
            pnlEdit.Visible = false;
            // Reload client
        }
        // COPY FROM HERE
        protected void btnBackToClients_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyPages/Clients.aspx");
        }

        protected void btnBackToDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyPages/Dashboard.aspx");
        }

        protected void btnEditClient_Click(object sender, EventArgs e)
        {
            // if you have pnlView/pnlEdit for client, use this:
            if (pnlView != null) pnlView.Visible = false;
            if (pnlEdit != null) pnlEdit.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (pnlEdit != null) pnlEdit.Visible = false;
            if (pnlView != null) pnlView.Visible = true;
        }
        // COPY TO HERE
        protected void btnBackToCases_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyPages/Cases.aspx");
        }


        protected void btnViewClient_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyPages/ClientDetails.aspx.aspx");
        }
    }


}