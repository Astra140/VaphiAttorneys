<%@ Page Title="Vaphi - Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="VaphiAttorneys.MyPages.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    .v-card { background:#0A1931; color:white; border-radius:12px; padding:20px; border-left:5px solid #C5A880; }
    .v-card h3 { color:#C5A880; }
    .v-sidebar { background:#0A1931; min-height:80vh; padding-top:20px; }
    .v-sidebar a { color:white; display:block; padding:12px 20px; text-decoration:none; }
    .v-sidebar a.active, .v-sidebar a:hover { background:#C5A880; color:#0A1931; }
</style>

<div class="row">
  <div class="col-md-2 v-sidebar">
    <a href="Dashboard.aspx" class="active">Dashboard</a>
    <a href="Cases.aspx">Cases</a>
    <a href="Clients.aspx">Clients</a>
    <a href="CaseDocuments.aspx">Documents</a>
    <a href="Calendar.aspx">Calendar</a>
  </div>

  <div class="col-md-10 p-4">
    <h2 style="color:#0A1931;">Vaphi Attorneys Dashboard</h2>
    <div class="row mt-4">
<div class="col-md-3">
  <div class="v-card" onclick="window.location='Cases.aspx?filter=open'" style="cursor:pointer;">
    <h3><asp:Label ID="lblOpen" runat="server" Text="0" /></h3>
    <p>Open Cases</p>
  </div>
</div>

<div class="col-md-3">
  <div class="v-card" onclick="window.location='Cases.aspx?filter=closed'" style="cursor:pointer;">
    <h3><asp:Label ID="lblClosed" runat="server" Text="0" /></h3>
    <p>Closed</p>
  </div>
</div>

<div class="col-md-3">
  <div class="v-card" onclick="window.location='Calendar.aspx'" style="cursor:pointer; border-left-color: #d9534f;">
    <h3><asp:Label ID="lblCourt" runat="server" Text="0" /></h3>
    <p>Court Dates</p>
  </div>
</div>

<div class="col-md-3">
  <div class="v-card" onclick="window.location='Cases.aspx'" style="cursor:pointer;">
    <h3><asp:Label ID="lblTotal" runat="server" Text="0" /></h3>
    <p>Total Cases</p>
  </div>
</div>
    </div>
<script>
function updateClock(){
 var now = new Date();
 document.getElementById('clock').innerHTML = now.toLocaleString('en-ZA',{weekday:'short', year:'numeric', month:'short', day:'numeric', hour:'2-digit', minute:'2-digit', second:'2-digit'});
}
setInterval(updateClock,1000); updateClock();
</script>
    <!-- Your My Clients pattern -->
    <h4 class="mt-5" style="color:#0A1931;">Recent Cases</h4>
    <asp:GridView ID="gvRecentCases" runat="server" CssClass="table table-striped" AutoGenerateColumns="false">
        <Columns>
            <asp:HyperLinkField DataTextField="CaseNo" HeaderText="Case No" 
                DataNavigateUrlFields="CaseID" 
                DataNavigateUrlFormatString="CaseDetails.aspx?CaseID={0}&source=Dashboard"
                ItemStyle-Width="140px" HeaderStyle-Width="140px" />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Client" 
                DataNavigateUrlFields="ClientID" 
                DataNavigateUrlFormatString="ClientDetails.aspx?ClientID={0}&source=Dashboard"
                ItemStyle-Width="120px" />
            <asp:BoundField DataField="CaseType" HeaderText="Type" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="DateOpened" HeaderText="Date" DataFormatString="{0:yyyy/MM/dd}" />
        </Columns>
    </asp:GridView>
  </div>
</div>
</asp:Content>


