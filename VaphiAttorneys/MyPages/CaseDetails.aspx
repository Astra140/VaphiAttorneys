<%@ Page Title="Case Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CaseDetails.aspx.cs" Inherits="VaphiAttorneys.MyPages.CaseDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
  /* This is your new system color scheme */
  .page-bg { background-color:#F8F5EF; min-height:100vh; margin:-20px; padding:30px 20px; }
  .case-card { background:#ffffff; border:none; border-radius:10px; overflow:hidden; box-shadow:0 2px 12px rgba(10,35,66,0.08); }
  .case-card-header { background:#fff; text-align:center; padding:16px; border-top:4px solid #0A2342; border-bottom:1px solid #EEF2F7; }
  .case-row { margin:0; padding:6px 16px; font-size:14px; line-height:1.3;}
  .case-row:nth-child(odd) { background:#ffffff; }
  .case-row:nth-child(even) { background:#EEF2F7; } /* <-- INSIDE COLOR */
  .case-label { font-weight:600; color:#0A2342; text-align:right; padding-right:25px; }

.btnDark { background:#0f1c33; color:white; padding:8px 16px; border-radius:6px; border:none; }
.btnLight { background:white; color:#1a365d; padding:8px 16px; border-radius:6px; border:1px solid #1a365d; }
.btnYellow { background:#f6c343; color:#000; padding:8px 16px; border-radius:6px; border:none; }
</style>

<div class="page-bg">
<div class="container-fluid" style="max-width:1100px; margin:0 auto;">
 
 <asp:Panel ID="pnlView" runat="server">
  <div class="case-card">
   <div class="case-card-header">
    <h4 style="margin:0; color:#666; font-weight:400;">Case Number : <span style="font-weight:700; color:#0A2342;"><asp:Label ID="lblCaseNo" runat="server" /></span></h4>
   </div>
   <div>
    <div class="row case-row"><div class="col-md-3 case-label">Client :</div><div class="col-md-9"><asp:Label ID="lblClient" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Case Title :</div><div class="col-md-9"><asp:Label ID="lblTitle" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Stage :</div><div class="col-md-9"><asp:Label ID="lblStage" runat="server" CssClass="badge" style="background:#0A2342; color:white; padding:6px 12px;" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Court :</div><div class="col-md-9"><asp:Label ID="lblCourt" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Court Date :</div><div class="col-md-9"><asp:Label ID="lblCourtDate" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Status / Type :</div><div class="col-md-9"><asp:Label ID="lblStatus" runat="server" /> - <asp:Label ID="lblType" runat="server" /></div></div>
   </div>
  </div>

  <div class="mt-4">
    <!-- ONLY these 4, delete the other 2 -->
    <asp:Button ID="btnBackToCases" runat="server" Text="Back to Cases" OnClick="btnBackToCases_Click" CssClass="btnDark" />
    <asp:Button ID="btnBackToDashboard" runat="server" Text="Back to Dashboard" Visible="false" OnClick="btnBackToDashboard_Click" CssClass="btnDark" />
    <asp:Button ID="btnViewClient" runat="server" Text="View Full Client Profile" OnClick="btnViewClient_Click" CssClass="btnLight" />
    <asp:Button ID="btnEditCase" runat="server" Text="Edit Case" OnClick="btnEditCase_Click" CssClass="btnYellow" />
  </div>
  <!-- Documents also using same combo -->
  <div class="case-card mt-4">
   <div class="case-card-header" style="font-weight:700; color:#0A2342;">Case Documents</div>
   <asp:GridView ID="gvDocs" runat="server" CssClass="table mb-0" AutoGenerateColumns="false" GridLines="None">
    <HeaderStyle BackColor="#EEF2F7" ForeColor="#0A2342" Font-Bold="true" />
    <RowStyle BackColor="White" />
    <AlternatingRowStyle BackColor="#EEF2F7" />
    <Columns>
     <asp:BoundField DataField="DocName" HeaderText="Document Name" ItemStyle-CssClass="p-3" />
     <asp:BoundField DataField="DateUploaded" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-CssClass="p-3" />
    </Columns>
    <EmptyDataTemplate><div class="p-4 text-center text-muted" style="background:#EEF2F7;">No documents attached.</div></EmptyDataTemplate>
   </asp:GridView>
  </div>
 </asp:Panel>

    <div>
  Case Finances - 
  Trust Balance: <asp:Label ID="lblTrustBalance" runat="server" Font-Bold="true" /> 
  | 
  Business Balance: <asp:Label ID="lblBusinessBalance" runat="server" Font-Bold="true" ForeColor="Green" />
</div>

<asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="False" 
  OnRowCommand="gvTransactions_RowCommand" CssClass="table table-striped">
<Columns>
    <asp:BoundField DataField="TransDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
    <asp:BoundField DataField="Description" HeaderText="Description" />
    <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt" />
    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
    <asp:BoundField DataField="Account" HeaderText="Account" />
    <asp:BoundField DataField="TrustBalance" HeaderText="Trust Balance" DataFormatString="{0:N2}" />
    <asp:BoundField DataField="BusinessBalance" HeaderText="Business Balance" DataFormatString="{0:N2}" />
    <asp:BoundField DataField="CreatedByID" HeaderText="User" />
    
    <asp:TemplateField HeaderText="Action">
        <ItemTemplate>
            <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" 
                CommandName="CancelTrans" 
                CommandArgument='<%# Eval("TransID") %>'
                OnClientClick="return confirm('Cancel this receipt? It will be reversed.');"
                Visible='<%# Eval("IsCancelled")==DBNull.Value || !Convert.ToBoolean(Eval("IsCancelled")) %>'
                CssClass="btn btn-sm btn-danger">
            </asp:LinkButton>
            <asp:Label ID="lblCancelled" runat="server" Text="CANCELLED" ForeColor="Red" Font-Bold="true"
                Visible='<%# Eval("IsCancelled")!=DBNull.Value && Convert.ToBoolean(Eval("IsCancelled")) %>'>
            </asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>

<div class="mt-2">
    <asp:Button ID="btnAddReceipt" runat="server" Text="Add Receipt" OnClick="btnAddReceipt_Click" CssClass="btnYellow" />
    <asp:Button ID="btnShowPayment" runat="server" Text="Add Payment" CssClass="btn btn-danger" OnClick="btnShowPayment_Click" />

<asp:Panel ID="pnlAddReceipt" runat="server" Visible="false" CssClass="card p-3 mt-3" Style="border:1px solid #ffc107; background:white;">
  <h6>Add New Receipt - Case: VAPHI/2026/003</h6>
  <div class="row">
    <div class="col-md-3">
      <label>Amount (R)</label>
      <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TextMode="Number" Text="5000"></asp:TextBox>
    </div>
    <div class="col-md-3">
      <label>Account</label>
      <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control">
        <asp:ListItem Text="Trust" Value="Trust" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Business" Value="Business"></asp:ListItem>
      </asp:DropDownList>
    </div>
    <div class="col-md-6">
      <label>Description</label>
      <asp:TextBox ID="txtTransDesc" runat="server" CssClass="form-control" Text="File opening deposit"></asp:TextBox>
    </div>
  </div>
  <div class="mt-2">
    <asp:Button ID="btnSaveReceipt" runat="server" Text="Save Receipt" OnClick="btnSaveReceipt_Click" CssClass="btn btn-dark" />
    <asp:Button ID="btnCancelReceipt" runat="server" Text="Cancel" OnClick="btnCancelReceipt_Click" CssClass="btn btn-light" />
    <asp:Label ID="lblReceiptMsg" runat="server" ForeColor="Red"></asp:Label>
  </div>
</asp:Panel>
</div>

<asp:Panel ID="pnlPayment" runat="server" Visible="false" style="border:1px solid red; padding:10px; margin-top:10px;">
  Account: 
  <asp:DropDownList ID="ddlPayAccount" runat="server">
    <asp:ListItem Text="Business" Value="Business" />
    <asp:ListItem Text="Trust" Value="Trust" />
  </asp:DropDownList>
  Description: <asp:TextBox ID="txtPayDesc" runat="server" />
  Amount: <asp:TextBox ID="txtPayAmount" runat="server" TextMode="Number" />
  <asp:Button ID="btnSavePayment" runat="server" Text="Save Payment" OnClick="btnSavePayment_Click" />
</asp:Panel>

 <asp:Panel ID="pnlEdit" runat="server" Visible="false">
  <div class="case-card">
    <div class="card-header" style="text-align:center;"><b>Edit Case - </b> <asp:Label ID="lblEditCaseNo" runat="server" /> <asp:Label ID="lblEditMsg" runat="server" CssClass="text-success ms-2" /></div>
    <div class="p-4" style="background:#EEF2F7;">
    <div class="mb-3"><label>Case Title</label><asp:TextBox ID="txtEditTitle" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" /></div>
    <div class="row">
     <div class="col-md-4 mb-3"><label>Stage</label><asp:DropDownList ID="ddlEditStage" runat="server" CssClass="form-control"></asp:DropDownList></div>
     <div class="col-md-4 mb-3"><label>Type</label><asp:DropDownList ID="ddlEditType" runat="server" CssClass="form-control"><asp:ListItem>Litigation</asp:ListItem><asp:ListItem>Criminal</asp:ListItem><asp:ListItem>Family</asp:ListItem><asp:ListItem>Labour</asp:ListItem></asp:DropDownList></div>
     <div class="col-md-4 mb-3"><label>Status</label><asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="form-control"><asp:ListItem>Open</asp:ListItem><asp:ListItem>Closed</asp:ListItem><asp:ListItem>Pending</asp:ListItem></asp:DropDownList></div>
    </div>
    <div class="row">
     <div class="col-md-6 mb-3"><label>Court Name</label><asp:TextBox ID="txtEditCourt" runat="server" CssClass="form-control" /></div>
     <div class="col-md-6 mb-3"><label>Court Date</label><asp:TextBox ID="txtEditCourtDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
    </div>
   </div>
  </div>
  <div class="mt-3">
   <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-success" OnClick="btnSave_Click" />
   <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary ms-2" OnClick="btnCancel_Click" />
  </div>
 </asp:Panel>

</div>
</div>

</asp:Content>