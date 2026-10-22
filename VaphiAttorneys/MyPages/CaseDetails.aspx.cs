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
    public partial class CaseDetails : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["VaphiConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStages();
                string id = Request.QueryString["CaseID"];
                if (!string.IsNullOrEmpty(id))
                {
                    LoadCase(id);
                    LoadTransactions(id);
                }
            }
            // CEO logic - null-safe
            string source = Request.QueryString["source"];
            if (btnBackToCases != null) btnBackToCases.Visible = (source != "Dashboard");
            if (btnBackToDashboard != null) btnBackToDashboard.Visible = (source == "Dashboard");
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
        void LoadStages()
        {
            ddlEditStage.Items.Clear();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT StageID, StageName FROM CaseStages ORDER BY StageOrder", con);
                var dr = cmd.ExecuteReader();
                ddlEditStage.DataSource = dr;
                ddlEditStage.DataTextField = "StageName";
                ddlEditStage.DataValueField = "StageID";
                ddlEditStage.DataBind();
            }
        }

        void LoadCase(string caseId)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"SELECT c.CaseID, c.CaseNo, c.ClientID, cl.FullName, c.CaseTitle, c.CaseType, s.StageName, c.CurrentStageID,
                                      c.Court, c.CourtDate, c.Status, c.DateOpened
                               FROM Tbl_Cases c LEFT JOIN Tbl_Clients cl ON c.ClientID=cl.ClientID
                               LEFT JOIN CaseStages s ON c.CurrentStageID=s.StageID WHERE c.CaseID=@ID";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@ID", caseId);
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lblCaseNo.Text = dr["CaseNo"].ToString();
                        lblEditCaseNo.Text = dr["CaseNo"].ToString();
                        lblClient.Text = dr["FullName"].ToString();
                        lblTitle.Text = dr["CaseTitle"].ToString();
                        lblStage.Text = dr["StageName"].ToString();
                        lblCourt.Text = dr["Court"].ToString();
                        lblCourtDate.Text = dr["CourtDate"] != DBNull.Value ? Convert.ToDateTime(dr["CourtDate"]).ToString("yyyy-MM-dd") : "Not Set";
                        lblStatus.Text = dr["Status"].ToString();
                        lblType.Text = dr["CaseType"].ToString();

                        // Fill edit fields
                        txtEditTitle.Text = dr["CaseTitle"].ToString();
                        txtEditCourt.Text = dr["Court"].ToString();
                       // txtEditTitle.Text = lblTitle.Text;
                       // txtEditCourt.Text = lblCourt.Text;
                        if (dr["CourtDate"] != DBNull.Value) txtEditCourtDate.Text = Convert.ToDateTime(dr["CourtDate"]).ToString("yyyy-MM-dd");
                        ddlEditType.SelectedValue = dr["CaseType"].ToString();
                        ddlEditStatus.SelectedValue = dr["Status"].ToString();
                        ddlEditStage.SelectedValue = dr["CurrentStageID"].ToString();

                        //  lnkClient.NavigateUrl = "~/MyPages/ClientDetails.aspx?ClientID=" + dr["ClientID"].ToString();
                        ViewState["CurrentClientID"] = dr["ClientID"].ToString();
                        ViewState["CurrentCaseID"] = caseId;
                        //ViewState["CurrentClientID"] = dr["ClientID"].ToString();
                        //ViewState["CurrentCaseID"] = caseId;

                    }
                }
            }
        }
        protected void btnViewClient_Click(object sender, EventArgs e)
        {
            if (ViewState["CurrentClientID"] != null)
            {
                string cId = ViewState["CurrentClientID"].ToString();
                Response.Redirect($"ClientDetails.aspx?ClientID={cId}&source=CaseDetails");
            }
            else
            {
                Response.Redirect("~/MyPages/Clients.aspx");
            }
        }

        protected void btnEditCase_Click(object sender, EventArgs e)
        {
            string caseId = Request.QueryString["CaseID"];
            Response.Redirect($"EditCase.aspx?CaseID={caseId}");
        }

        protected void btnBackToCases_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyPages/Cases.aspx");
        }
        protected void btnBackToDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyPages/Dashboard.aspx");
        }
        /*protected void btnEditCase_Click(object sender, EventArgs e)
        {
            pnlView.Visible = false;
            pnlEdit.Visible = true;
        }
        */
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = false;
            pnlView.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // your UPDATE query here using ViewState["CurrentCaseID"]
            // then reload:
            pnlEdit.Visible = false;
            pnlView.Visible = true;
            LoadCase(ViewState["CurrentCaseID"].ToString());
            lblEditMsg.Text = "Saved.";
        }
        private void LoadTransactions(string caseId)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"SELECT TransID, TransDate, Description, ReceiptNo, Amount, Account, TrustBalance, BusinessBalance, 
                                CreatedByID, IsCancelled, CancelledByID 
                       FROM dbo.Transactions WHERE CaseID = @CaseID AND ISCANCELLED = 0 ORDER BY TransDate DESC, TransID DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@CaseID", caseId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTransactions.DataSource = dt;
                gvTransactions.DataBind();

                if (dt.Rows.Count > 0)
                {
                    // Last trust balance
                    //var bal = dt.Rows.Cast<DataRow>().Max(r => Convert.ToDecimal(r["TrustBalance"]));
                    //lblTrustBalance.Text = "R" + bal.ToString("N2");
                    //decimal trustBal = dt.Rows.Count > 0 ? dt.AsEnumerable().Max(x => x["TrustBalance"] != DBNull.Value ? Convert.ToDecimal(x["TrustBalance"]) : 0) : 0;
                    //decimal busBal = dt.Rows.Count > 0 ? dt.AsEnumerable().Max(x => x["BusinessBalance"] != DBNull.Value ? Convert.ToDecimal(x["BusinessBalance"]) : 0) : 0;

                    var lastRow = dt.Rows[0]; // if your query is ORDER BY TransDate DESC, TransID DESC
                    decimal trustBal = lastRow["TrustBalance"] != DBNull.Value ? Convert.ToDecimal(lastRow["TrustBalance"]) : 0;
                    decimal busBal = lastRow["BusinessBalance"] != DBNull.Value ? Convert.ToDecimal(lastRow["BusinessBalance"]) : 0;

                    if (lblTrustBalance != null)
                        lblTrustBalance.Text = "R" + trustBal.ToString("N2");

                    if (lblBusinessBalance != null)
                        lblBusinessBalance.Text = "R" + busBal.ToString("N2");
                }
            }
        }
        protected void btnAddReceipt_Click(object sender, EventArgs e)
        {
            pnlAddReceipt.Visible = true;
        }

        protected void btnCancelReceipt_Click(object sender, EventArgs e)
        {
            pnlAddReceipt.Visible = false;
        }

        protected void btnSaveReceipt_Click(object sender, EventArgs e)
        {
            int caseId = Convert.ToInt32(Request.QueryString["CaseID"]);
            try
            {
                decimal amount = 5000;
                Decimal.TryParse(txtAmount.Text, out amount);

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Get ClientID
                    SqlCommand cmdCli = new SqlCommand("SELECT ClientID FROM dbo.Tbl_Cases WHERE CaseID=@CaseID", con);
                    cmdCli.Parameters.AddWithValue("@CaseID", caseId);
                    object cli = cmdCli.ExecuteScalar();
                    if (cli == null) throw new Exception("Case not found");
                    int clientId = Convert.ToInt32(cli);

                    // Get current trust balance
                    SqlCommand cmdBal = new SqlCommand("SELECT TOP 1 ISNULL(TrustBalance,0), ISNULL(BusinessBalance,0) FROM dbo.Transactions WHERE CaseID=@CaseID ORDER BY TransID DESC", con);
                    cmdBal.Parameters.AddWithValue("@CaseID", caseId);
                    decimal currentTrust = 0; decimal currentBusiness = 0;
                    
                    using (var reader = cmdBal.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentTrust = reader.GetDecimal(0);
                            currentBusiness = reader.GetDecimal(1);
                        }
                    }
                    //reader.Close();

                    decimal newTrust = currentTrust;
                    decimal newBusiness = currentBusiness;
                    string acc = ddlAccount.SelectedValue;
                    if (acc == "Trust") newTrust = currentTrust + amount;
                    else if (acc == "Business") newBusiness = currentBusiness + amount;

                    string receiptNo = "REC-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                    string userName = Session["UserName"] != null ? Session["UserName"].ToString() : User.Identity.Name;
                    int userId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;

                    SqlCommand cmdIns = new SqlCommand(
                        "INSERT INTO dbo.Transactions (ClientID, CaseID, TransDate, Description, Amount, Type, Account, TrustBalance, BusinessBalance, ReceiptNo) " +
                        " VALUES (@CID, @CaseID, GETDATE(), @Desc, @Amt, 'Receipt', @Acc, @TBal, @BBal, @RNo, @CB, @CBID)", con);
                        cmdIns.Parameters.AddWithValue("@CID", clientId);
                        cmdIns.Parameters.AddWithValue("@CaseID", caseId);
                        cmdIns.Parameters.AddWithValue("@Desc", txtTransDesc.Text);
                        cmdIns.Parameters.AddWithValue("@Amt", amount);
                        cmdIns.Parameters.AddWithValue("@Acc", acc);
                        cmdIns.Parameters.AddWithValue("@TBal", newTrust);
                        cmdIns.Parameters.AddWithValue("@BBal", newBusiness);
                        cmdIns.Parameters.AddWithValue("@RNo", receiptNo);
                        cmdIns.Parameters.AddWithValue("@CB", userName);
                        cmdIns.Parameters.AddWithValue("@CBID", userId);
                        cmdIns.ExecuteNonQuery();
                }

                pnlAddReceipt.Visible = false;
                lblReceiptMsg.Text = "";
                LoadTransactions(caseId.ToString());
            }       
            catch (Exception ex)
            {
                lblReceiptMsg.Text = "Error: " + ex.Message;
                pnlAddReceipt.Visible = true;
            }
        }

        protected void gvTransactions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelTrans")
            {

                int transId = Convert.ToInt32(e.CommandArgument);
                int currentUserID = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;
                string currentUserName = Session["UserName"] != null ? Session["UserName"].ToString() : User.Identity.Name;

                // Prevent reversing a reversal
                SqlConnection con = new SqlConnection(connStr);
                SqlCommand cmdCheck = new SqlCommand("SELECT Type, IsCancelled FROM dbo.Transactions WHERE TransID=@ID", con);
                cmdCheck.Parameters.AddWithValue("@ID", transId);
                using (var chk = cmdCheck.ExecuteReader())
                {
                    if (chk.Read())
                    {
                        string type = chk["Type"].ToString();
                        bool isCan = chk["IsCancelled"] != DBNull.Value && Convert.ToBoolean(chk["IsCancelled"]);
                        if (type == "Reversal" || isCan)
                        {
                            chk.Close();
                            return; // already cancelled or is a reversal - do nothing
                        }
                    }
                    chk.Close();
                }

                //string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection con1 = new SqlConnection(connStr))
                {
                    con1.Open();

                    // 1. Get original
                    SqlCommand cmdGet = new SqlCommand("SELECT ClientID, CaseID, Amount, Account, Description, ReceiptNo FROM dbo.Transactions WHERE TransID=@ID", con1);
                    cmdGet.Parameters.AddWithValue("@ID", transId);
                    SqlDataReader dr = cmdGet.ExecuteReader();
                    if (!dr.Read()) return;
                    int clientId = Convert.ToInt32(dr["ClientID"]);
                    int caseId = Convert.ToInt32(dr["CaseID"]);
                    decimal amt = Convert.ToDecimal(dr["Amount"]);
                    string acc = dr["Account"].ToString();
                    string desc = dr["Description"].ToString();
                    string recNo = dr["ReceiptNo"].ToString();
                    dr.Close();

                    // 2. Flag original as cancelled
                    SqlCommand cmdFlag = new SqlCommand("UPDATE dbo.Transactions SET IsCancelled=1, CancelledByID=@CBID, CancelledDate=GETDATE() WHERE TransID=@ID", con1);
                    cmdFlag.Parameters.AddWithValue("@CBID", currentUserID);
                    cmdFlag.Parameters.AddWithValue("@ID", transId);
                    cmdFlag.ExecuteNonQuery();

                    // 3. Get current LIVE balances (exclude cancelled)
                    SqlCommand cmdBal = new SqlCommand("SELECT TOP 1 ISNULL(TrustBalance,0), ISNULL(BusinessBalance,0) FROM dbo.Transactions WHERE CaseID=@CID AND IsCancelled=0 ORDER BY TransID DESC", con1);
                    cmdBal.Parameters.AddWithValue("@CID", caseId);
                    decimal curTrust = 0, curBus = 0;
                    using (SqlDataReader r = cmdBal.ExecuteReader())
                    {
                        if (r.Read()) { curTrust = r.GetDecimal(0); curBus = r.GetDecimal(1); }
                    }

                    // 4. Calculate reversal
                    decimal newTrust = curTrust;
                    decimal newBus = curBus;
                    decimal revAmt = -amt; // opposite

                    if (acc == "Trust") newTrust = curTrust + revAmt;
                    else newBus = curBus + revAmt;

                    // 5. Insert reversal entry
                    SqlCommand cmdRev = new SqlCommand(@"INSERT INTO dbo.Transactions 
                (ClientID, CaseID, TransDate, Description, Amount, Type, Account, TrustBalance, BusinessBalance, ReceiptNo, CreatedBy, CreatedByID, IsCancelled) 
                VALUES (@CID, @CaseID, GETDATE(), @Desc, @Amt, 'Reversal', @Acc, @TBal, @BBal, @RNo, @CB, @CBID, 0)", con1);
                    cmdRev.Parameters.AddWithValue("@CID", clientId);
                    cmdRev.Parameters.AddWithValue("@CaseID", caseId);
                    cmdRev.Parameters.AddWithValue("@Desc", "REVERSED: " + desc);
                    cmdRev.Parameters.AddWithValue("@Amt", revAmt);
                    cmdRev.Parameters.AddWithValue("@Acc", acc);
                    cmdRev.Parameters.AddWithValue("@TBal", newTrust);
                    cmdRev.Parameters.AddWithValue("@BBal", newBus);
                    cmdRev.Parameters.AddWithValue("@RNo", "REV-" + recNo);
                    cmdRev.Parameters.AddWithValue("@CB", currentUserName);
                    cmdRev.Parameters.AddWithValue("@CBID", currentUserID);
                    cmdRev.ExecuteNonQuery();
                }

                // Refresh grid
                string caseIdStr = Request.QueryString["CaseID"];
                if (!string.IsNullOrEmpty(caseIdStr))
                    LoadTransactions(caseIdStr);
            }
        }
        protected void btnShowPayment_Click(object sender, EventArgs e)
        {
            pnlPayment.Visible = true;
        }

        protected void btnSavePayment_Click(object sender, EventArgs e)
        {
            int caseId = Convert.ToInt32(Request.QueryString["CaseID"]);
            decimal payAmount = Convert.ToDecimal(txtPayAmount.Text);
            string acc = ddlPayAccount.SelectedValue;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmdCli = new SqlCommand("SELECT ClientID FROM dbo.Tbl_Cases WHERE CaseID=@CaseID", con);
                cmdCli.Parameters.AddWithValue("@CaseID", caseId);
                int clientId = Convert.ToInt32(cmdCli.ExecuteScalar());

                // SqlCommand cmdBal = new SqlCommand("SELECT ISNULL(MAX(TrustBalance),0), ISNULL(MAX(BusinessBalance),0) FROM dbo.Transactions WHERE CaseID=@CaseID", con);
                SqlCommand cmdBal = new SqlCommand("SELECT TOP 1 ISNULL(TrustBalance,0), ISNULL(BusinessBalance,0) FROM dbo.Transactions WHERE CaseID=@CaseID ORDER BY TransID DESC", con);
                cmdBal.Parameters.AddWithValue("@CaseID", caseId);
                decimal curTrust = 0, curBus = 0;
                using (SqlDataReader r = cmdBal.ExecuteReader())
                {
                    if (r.Read()) { curTrust = r.GetDecimal(0); curBus = r.GetDecimal(1); }
                }

                decimal newTrust = curTrust; decimal newBus = curBus;

                if (acc == "Trust")
                {
                    if (payAmount > curTrust) throw new Exception("INSUFFICIENT TRUST FUNDS - LPC Violation!");
                    newTrust = curTrust - payAmount;
                }
                else
                {
                    newBus = curBus - payAmount;
                }

                SqlCommand cmdIns = new SqlCommand(
                  "INSERT INTO dbo.Transactions (ClientID, CaseID, TransDate, Description, Amount, Type, Account, TrustBalance, BusinessBalance, ReceiptNo) VALUES (@CID, @CaseID, GETDATE(), @Desc, @Amt, 'Payment', @Acc, @TBal, @BBal, @RNo)", con);
                cmdIns.Parameters.AddWithValue("@CID", clientId);
                cmdIns.Parameters.AddWithValue("@CaseID", caseId);
                cmdIns.Parameters.AddWithValue("@Desc", txtPayDesc.Text);
                cmdIns.Parameters.AddWithValue("@Amt", -payAmount); // Negative for outflow
                cmdIns.Parameters.AddWithValue("@Acc", acc);
                cmdIns.Parameters.AddWithValue("@TBal", newTrust);
                cmdIns.Parameters.AddWithValue("@BBal", newBus);
                cmdIns.Parameters.AddWithValue("@RNo", "PAY-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"));
                cmdIns.ExecuteNonQuery();
            }
            pnlPayment.Visible = false;
            LoadTransactions(caseId.ToString());
        }
    }
}