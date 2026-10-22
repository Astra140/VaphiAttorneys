<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VaphiAttorneys.Login" %>

<!DOCTYPE html>
<html>
<head>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body{background:#F5F5F0; display:flex; align-items:center; justify-content:center; height:100vh;}
        .login-box{background:white; padding:40px; border-radius:16px; box-shadow:0 10px 30px rgba(0,0,0,0.1); width:400px; border-top:5px solid #C5A880;}
        .btn-vaphi{background:#0A1931; color:#C5A880; width:100%;}
        .btn-vaphi:hover{background:#C5A880; color:#0A1931;}
        h3{color:#0A1931;}
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="login-box">
        <h3 class="text-center">⚖️ VAPI ATTORNEYS</h3>
        <p class="text-center text-muted">Attorneys at Law - Login</p>
        
        <label>Username</label>
        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control mb-3" Placeholder="admin" />

        <label>Password</label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" Placeholder="****" />

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-vaphi mt-2" OnClick="btnLogin_Click" />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="mt-2 d-block text-center"></asp:Label>
    </div>
</form>
</body>
</html>
