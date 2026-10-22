<%@ Page Title="Client Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientDetails.aspx.cs" Inherits="VaphiAttorneys.MyPages.ClientDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div style="margin: 0 0 12px 0; font-size: 14px;">
    <a href="<%= GetBackUrl() %>" style="color:#1a365d; font-weight:600; text-decoration:none;">← Back</a>
    <span style="margin-left:15px; color:#888;">|</span>
    <a href="Dashboard.aspx" style="margin-left:15px;">Back to Dashboard</a>
    <a href="Cases.aspx" style="margin-left:10px;">Back to Cases</a>
    <a href="Clients.aspx" style="margin-left:10px;">Back to Clients</a>
</div>
<style>
  .page-bg { background-color:#F8F5EF; min-height:100vh; margin:-20px; padding:30px 20px; }
  .case-card { background:#ffffff; border:none; border-radius:10px; overflow:hidden; box-shadow:0 2px 12px rgba(10,35,66,0.08); }
  .case-card-header { background:#fff; text-align:center; padding:12px; border-top:4px solid #0A2342; border-bottom:1px solid #EEF2F7; }
  .case-row { margin:0; padding:8px 16px; font-size:14px; }
  .case-row:nth-child(odd) { background:#ffffff; }
  .case-row:nth-child(even) { background:#EEF2F7; }
  .case-label { font-weight:600; color:#0A2342; text-align:right; padding-right:25px; }
</style>

<div class="page-bg">
<div class="container-fluid" style="max-width:1100px; margin:0 auto;">

 <asp:Panel ID="pnlView" runat="server">
  <div class="case-card">
   <div class="case-card-header">
    <h4 style="margin:0; color:#666; font-weight:400;">Client : <span style="font-weight:700; color:#0A2342;"><asp:Label ID="lblClientName" runat="server" /></span></h4>
    <asp:Label ID="lblClientNo" runat="server" CssClass="text-muted" style="font-size:13px;" />
    <asp:Label ID="lblEditMsg" runat="server" CssClass="text-success ms-2" style="font-size:13px;" />
   </div>
   <div>
    <div class="row case-row"><div class="col-md-3 case-label">Full Names :</div><div class="col-md-9"><asp:Label ID="lblFullNames" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">ID / Passport :</div><div class="col-md-9"><asp:Label ID="lblIDNumber" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Phone :</div><div class="col-md-9"><asp:Label ID="lblPhone" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Email :</div><div class="col-md-9"><asp:Label ID="lblEmail" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Physical Address :</div><div class="col-md-9"><asp:Label ID="lblAddress" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Client Type :</div><div class="col-md-9"><asp:Label ID="lblClientType" runat="server" /></div></div>
    <div class="row case-row"><div class="col-md-3 case-label">Status / Date Created :</div><div class="col-md-9"><asp:Label ID="lblStatus" runat="server" /> - <asp:Label ID="lblDateCreated" runat="server" /></div></div>
   </div>
  </div>

<!-- CORRECT for ClientDetails page -->
<div class="mt-4">
<asp:Button ID="btnBackToCases" runat="server" Text="Back to Cases" OnClick="btnBackToCases_Click" CssClass="btnDark" />
<asp:Button ID="btnBackToClients" runat="server" Text="Back to Clients" OnClick="btnBackToClients_Click" CssClass="btnDark" />
<asp:Button ID="btnBackToDashboard" runat="server" Text="Back to Dashboard" Visible="false" OnClick="btnBackToDashboard_Click" CssClass="btnDark" />
<asp:Button ID="btnEditClient" runat="server" Text="Edit Client" OnClick="btnEditClient_Click" CssClass="btnYellow" />
</div>
  <!-- Client Cases - same style -->
  <div class="case-card mt-4">
   <div class="case-card-header" style="font-weight:700; color:#0A2342;">Cases for this Client</div>
   <asp:GridView ID="gvClientCases" runat="server" CssClass="table mb-0" AutoGenerateColumns="false" GridLines="None">
    <HeaderStyle BackColor="#EEF2F7" ForeColor="#0A2342" Font-Bold="true" />
    <RowStyle BackColor="White" />
    <AlternatingRowStyle BackColor="#EEF2F7" />
    <Columns>
     <asp:BoundField DataField="CaseNo" HeaderText="Case No" ItemStyle-CssClass="p-3" />
     <asp:BoundField DataField="CaseTitle" HeaderText="Title" ItemStyle-CssClass="p-3" />
     <asp:BoundField DataField="StageName" HeaderText="Stage" ItemStyle-CssClass="p-3" />
     <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"><ItemTemplate><a href='<%# "CaseDetails.aspx?CaseID=" + Eval("CaseID") %>' class="btn btn-sm btn-outline-dark">View</a></ItemTemplate></asp:TemplateField>
    </Columns>
    <EmptyDataTemplate><div class="p-4 text-center text-muted" style="background:#EEF2F7;">No cases for this client.</div></EmptyDataTemplate>
   </asp:GridView>
  </div>
 </asp:Panel>

 <asp:Panel ID="pnlEdit" runat="server" Visible="false">
  <div class="case-card">
   <div class="case-card-header" style="border-top-color:#FFC107;"><b>Edit Client - </b> <asp:Label ID="lblEditClientName" runat="server" /> <asp:Label ID="lblEditClientMsg" runat="server" CssClass="text-success ms-2" /></div>
   <div class="p-4" style="background:#EEF2F7;">
    <div class="row">
     <div class="col-md-6 mb-3"><label>Full Names</label><asp:TextBox ID="txtFullNames" runat="server" CssClass="form-control" /></div>
     <div class="col-md-6 mb-3"><label>ID / Passport</label><asp:TextBox ID="txtIDNumber" runat="server" CssClass="form-control" /></div>
    </div>
    <div class="row">
     <div class="col-md-6 mb-3"><label>Phone</label><asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" /></div>
     <div class="col-md-6 mb-3"><label>Email</label><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" /></div>
    </div>
    <div class="mb-3"><label>Address</label><asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" /></div>
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
