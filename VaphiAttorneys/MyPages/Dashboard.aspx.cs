using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VaphiAttorneys.MyPages
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["LoginName"] == null) Response.Redirect("~/Login.aspx");
            if (Session["UserID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack)
            {
                LoadStats();
                BindRecent();
            }

        }

        private void LoadStats()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                con.Open();
                lblTotal.Text = new SqlCommand("SELECT COUNT(*) FROM dbo.Tbl_Cases", con).ExecuteScalar().ToString();
                lblOpen.Text = new SqlCommand("SELECT COUNT(*) FROM dbo.Tbl_Cases WHERE Status='Open'", con).ExecuteScalar().ToString();
                lblClosed.Text = new SqlCommand("SELECT COUNT(*) FROM dbo.Tbl_Cases WHERE Status='Closed'", con).ExecuteScalar().ToString();
                //lblCourt.Text = new SqlCommand("SELECT COUNT(*) FROM dbo.Tbl_Cases WHERE CourtDate >= CAST(GETDATE() AS DATE)", con).ExecuteScalar().ToString();
                // OLD - counts everything
                // SELECT COUNT(*) FROM Tbl_Cases

                // NEW - only future open cases - will match your Calendar
                lblCourt.Text = new SqlCommand("SELECT COUNT(*) FROM Tbl_Cases WHERE CourtDate >= CAST(GETDATE() AS DATE) AND Status = 'Open'", con).ExecuteScalar().ToString();
            }
        }
        private void BindRecent()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                string sql = @"
            SELECT TOP 5 
                c.CaseID,
                c.CaseNo,
                c.ClientID,
                cl.FullName, 
                c.CaseType, 
                c.Status, 
                c.DateOpened
            FROM Tbl_Cases c
            INNER JOIN Tbl_Clients cl ON c.ClientID = cl.ClientID
            ORDER BY c.DateOpened DESC";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvRecentCases.DataSource = dt;
                    gvRecentCases.DataBind();
                }
            }
        }

    }
}


