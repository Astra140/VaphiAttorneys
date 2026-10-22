<%@ Page Title="Vapi - Clients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clients.aspx.cs" Inherits="VaphiAttorneys.MyPages.Clients" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div style="display:flex; gap:20px">
 <!-- LEFT: Form -->
 <div style="width:40%; background:#fff; padding:20px; border-radius:8px">
    <h3 style="color:#0A2342">Add New Client</h3>
    <asp:TextBox ID="txtFullName" runat="server" Placeholder="Full Name" CssClass="form-control" />
    <asp:RequiredFieldValidator ControlToValidate="txtFullName" runat="server" ErrorMessage="* Required" ForeColor="Red" />
    <br />
    <asp:TextBox ID="txtIDNumber" runat="server" Placeholder="ID Number" CssClass="form-control" /><br />
    <asp:TextBox ID="txtPhone" runat="server" Placeholder="Phone" CssClass="form-control" /><br />
    <asp:TextBox ID="txtEmail" runat="server" Placeholder="Email" CssClass="form-control" /><br />
    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Placeholder="Address" CssClass="form-control" /><br />
    <asp:Button ID="btnSaveClient" runat="server" Text="Save Client" CssClass="btn btn-primary" 
        BackColor="#0A2342" OnClick="BtnSaveClient_Click" />
    <br /><asp:Label ID="lblMessage" runat="server" />
 </div>

 <!-- RIGHT: GridView -->
 <div style="width:60%; background:#fff; padding:20px; border-radius:8px">
    <h3 style="color:#0A2342">My Clients</h3>
    <asp:GridView ID="gvClients" runat="server" AutoGenerateColumns="false" CssClass="table" 
        HeaderStyle-BackColor="#0A2342" HeaderStyle-ForeColor="White">
        <Columns>
<asp:TemplateField HeaderText="ID">
 <ItemTemplate>
  <a href='ClientDetails.aspx?ClientID=<%# Eval("ClientID") %>' style="color:#0A2342; font-weight:bold; text-decoration:underline;">
   <%# Eval("ClientID") %>
  </a>
 </ItemTemplate>
</asp:TemplateField>
            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="CreatedDate" HeaderText="Date Added" DataFormatString="{0:yyyy-MM-dd}" />
        </Columns>
    </asp:GridView>
 </div>
</div>
</asp:Content>
