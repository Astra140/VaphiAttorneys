using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VaphiAttorneys.MyPages
{
    public partial class Calendar : System.Web.UI.Page
    {
        private static Dictionary<DateTime, int> CourtDates = new Dictionary<DateTime, int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack)
            {
                LoadCourtDates();
                calCourt.SelectedDate = DateTime.Today;
                BindForDate(DateTime.Today);
            }
        }

        private void LoadCourtDates()
        {
            CourtDates.Clear();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand("SELECT CAST(CourtDate AS DATE) as D, COUNT(*) as Cnt FROM dbo.Tbl_Cases WHERE CourtDate IS NOT NULL GROUP BY CAST(CourtDate AS DATE)", con);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        CourtDates[Convert.ToDateTime(dr["D"]).Date] = Convert.ToInt32(dr["Cnt"]);
                }
            }
        }

        protected void calCourt_DayRender(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
        {
            if (CourtDates.ContainsKey(e.Day.Date))
            {
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#C5A880");
                e.Cell.Font.Bold = true;
                e.Cell.ToolTip = CourtDates[e.Day.Date] + " case(s)";
            }
        }

        protected void calCourt_SelectionChanged(object sender, EventArgs e)
        {
            BindForDate(calCourt.SelectedDate);
        }

        private void BindForDate(DateTime dt)
        {
            lblSelectedDate.Text = dt.ToString("dddd, dd MMM yyyy");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString))
            {
                string sql = @"SELECT c.CaseID, c.CaseNo, c.CaseType, c.Court, c.Status 
                               FROM dbo.Tbl_Cases c 
                               WHERE CAST(c.CourtDate AS DATE)=@D";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@D", dt.Date);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    gvCourt.DataSource = tbl;
                    gvCourt.DataBind();
                    lblNoData.Visible = tbl.Rows.Count == 0;
                }
            }
        }
    }
}