<%@ Page Title="Vaphi - Cases" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cases.aspx.cs" Inherits="VaphiAttorneys.MyPages.Cases" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
 /* Force Select2 to take full width of Add New Case column */
 .select2-container { width: 100% !important; }
 .select2-selection--single { height: 38px !important; padding: 6px; }
 #ContentPlaceHolder1_ddlClients { width: 100% !important; }
</style>

<div style="display:flex; gap:20px">
 <!-- LEFT: Form -->
 <div style="width:40%; background:#fff; padding:20px; border-radius:8px">
    <h3 style="color:#0A2342">Add New Case</h3>
    
     <asp:DropDownList ID="ddlClients" runat="server" CssClass="form-control searchable-client" 
        AppendDataBoundItems="true" ClientIDMode="Static" Height="30px">
        <asp:ListItem Text="-- Select Client --" Value="0" />
     </asp:DropDownList>
     <asp:RequiredFieldValidator ControlToValidate="ddlClients" InitialValue="0" runat="server" ErrorMessage="* Select Client" ForeColor="Red" Display="Dynamic" />
     <br />
<label style="font-weight:600;color:#0A2342">Case Stage (7 Standard Litigation Stages)</label>
<asp:DropDownList ID="ddlStage" runat="server" CssClass="form-control" ClientIDMode="Static">
</asp:DropDownList>
<asp:RequiredFieldValidator ControlToValidate="ddlStage" InitialValue="0" runat="server" ErrorMessage="* Select Stage" ForeColor="Red" Display="Dynamic" />
     <br /><br />

    <%-- CaseType Dropdown --%>
    <asp:DropDownList ID="ddlCaseType" runat="server" CssClass="form-control">
        <asp:ListItem Text="-- Select Case Type --" Value="" />
        <asp:ListItem Text="Criminal" />
        <asp:ListItem Text="Civil Litigation" />
        <asp:ListItem Text="Family Law" />
        <asp:ListItem Text="Labour Law" />
        <asp:ListItem Text="Property / Conveyancing" />
        <asp:ListItem Text="Road Accident Fund" />
        <asp:ListItem Text="Divorce" />
    </asp:DropDownList>
    <br />

    <%-- Status Dropdown --%>
    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
        <asp:ListItem Text="Open" Selected="True" />
        <asp:ListItem Text="In Court" />
        <asp:ListItem Text="Pending" />
        <asp:ListItem Text="Closed" />
    </asp:DropDownList>
    <br />
     <asp:TextBox ID="txtCourtName" runat="server" CssClass="form-control" Placeholder="e.g. High Court, Mthatha"></asp:TextBox>
<asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" Placeholder="Case Description" CssClass="form-control" /><br />
    <asp:TextBox ID="txtCourtDate" runat="server" TextMode="Date" CssClass="form-control" /><br />

    <asp:Button ID="BtnSaveCase" runat="server" Text="Save Case" CssClass="btn btn-primary" 
        BackColor="#0A2342" ForeColor="White" Width="100%" OnClick="BtnSaveCase_Click" />
    <br /><asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
 </div>

 <!-- RIGHT: GridView -->
 <div style="width:60%; background:#fff; padding:20px; border-radius:8px">
    <h3 style="color:#0A2342">Listing of Cases</h3>
    <asp:GridView ID="gvCases" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" 
        HeaderStyle-BackColor="#0A2342" HeaderStyle-ForeColor="White">
        <Columns>
            

<asp:TemplateField HeaderText="Case Number">
 <ItemTemplate>
  <a href='CaseDetails.aspx?CaseID=<%# Eval("CaseID") %>'>
   <%# Eval("CaseNo") %>
  </a>
 </ItemTemplate>
</asp:TemplateField>
            <asp:BoundField DataField="CaseNo" HeaderText="Case Number" />
            <asp:BoundField DataField="FullName" HeaderText="Client" />
            <asp:BoundField DataField="CaseType" HeaderText="Type" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="DateOpened" HeaderText="Opened" DataFormatString="{0:yyyy-MM-dd}" />
        </Columns>
    </asp:GridView>
 </div>
</div>

<link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
<script>
    function initClientSearch() {
        $('#ddlClients').select2({
            placeholder: "-- Select Client --",
            allowClear: true,
            width: '100%'
        });
    }
    // Load for first time
    $(document).ready(function () { initClientSearch(); });
    // Reload after ASP.NET postback
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { initClientSearch(); });
</script>

</asp:Content>