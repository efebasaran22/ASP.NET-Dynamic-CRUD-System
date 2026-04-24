<%@ Page Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="StajOdev.Account.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Giriş Yap</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br />

    <asp:TextBox ID="txtUsername" runat="server" Placeholder="Kullanıcı Adı"></asp:TextBox><br /><br />
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Placeholder="Şifre"></asp:TextBox><br /><br />

    <asp:CheckBox ID="chkRememberMe" runat="server" Text="Beni Hatırla" /><br /><br />

    <asp:Button ID="btnLogin" runat="server" Text="Giriş Yap" OnClick="btnLogin_Click" />
</asp:Content>
