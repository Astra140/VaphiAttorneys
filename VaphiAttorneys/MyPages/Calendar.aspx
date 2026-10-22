<%@ Page Title="Calendar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="VaphiAttorneys.MyPages.Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div style="display:flex;">
  <!-- Left sidebar same as Dashboard -->
  <div style="width:200px; background:#0A1931; min-height:80vh; padding-top:20px;">
    <a href="Dashboard.aspx" style="display:block; padding:10px 15px; color:white; text-decoration:none;">Dashboard</a>
    <a href="Cases.aspx" style="display:block; padding:10px 15px; color:white; text-decoration:none;">Cases</a>
    <a href="Clients.aspx" style="display:block; padding:10px 15px; color:white; text-decoration:none;">Clients</a>
    <a href="CaseDocuments.aspx" style="display:block; padding:10px 15px; color:white; text-decoration:none;">Documents</a>
    <a href="Calendar.aspx" style="display:block; padding:10px 15px; background:#C5A880; color:#0A1931; font-weight:bold;">Calendar</a>
  </div>

  <div style="flex:1; padding:20px;">
    <h3 style="color:#0A1931;">Court Dates Calendar</h3>
    
    <div style="display:flex; gap:20px;">
      <div>
        <asp:Calendar ID="calCourt" runat="server" 
            OnDayRender="calCourt_DayRender" 
            OnSelectionChanged="calCourt_SelectionChanged"
            BackColor="White" BorderColor="#0A1931" BorderWidth="1px"
            DayNameFormat="Shortest" Font-Names="Segoe UI" Font-Size="10pt"
            NextPrevFormat="FullMonth" TitleFormat="MonthYear">
            <SelectedDayStyle BackColor="#0A1931" ForeColor="White" Font-Bold="true" />
            <TodayDayStyle BackColor="#C5A880" ForeColor="#0A1931" Font-Bold="true" />
            <DayHeaderStyle BackColor="#0A1931" ForeColor="White" Font-Bold="true" />
            <TitleStyle BackColor="#0A1931" ForeColor="#C5A880" Font-Bold="true" />
        </asp:Calendar>
        <br />
        <div style="background:#0A1931; color:#C5A880; padding:10px; border-radius:6px; font-size:12px;">
          Gold date = Court date
        </div>
      </div>

      <div style="flex:1;">
        <h5>Cases on <asp:Label ID="lblSelectedDate" runat="server" Text="Today" Font-Bold="true"></asp:Label></h5>
        <asp:GridView ID="gvCourt" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" GridLines="None">
        <Columns>
            <asp:BoundField DataField="CaseNo" HeaderText="Case No" />
            <asp:BoundField DataField="CaseType" HeaderText="Type" />
            <asp:BoundField DataField="Court" HeaderText="Court" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:TemplateField HeaderText="View">
            <ItemTemplate>
                <a href='CaseDetails.aspx?CaseID=<%# Eval("CaseID") %>'>View</a>
                </ItemTemplate>
                </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <asp:Label ID="lblNoData" runat="server" Text="No court dates on this day." Visible="false"></asp:Label>
      </div>
    </div>
  </div>
</div>
</asp:Content>
