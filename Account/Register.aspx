<%@ Page Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="StajOdev.Account.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Kayıt Ol</h2>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br />
    <asp:TextBox ID="txtFirstName" runat="server" Placeholder="Ad"></asp:TextBox><br />
    <asp:TextBox ID="txtLastName" runat="server" Placeholder="Soyad"></asp:TextBox><br />
    <asp:TextBox ID="txtEmail" runat="server" Placeholder="Email"></asp:TextBox><br />
    <asp:TextBox ID="txtUsername" runat="server" Placeholder="Kullanıcı Adı"></asp:TextBox><br />
    <asp:TextBox ID="txtPassword" runat="server" Placeholder="Şifre" TextMode="Password"></asp:TextBox><br />
    <asp:Button ID="btnRegister" runat="server" Text="Kayıt Ol" OnClick="btnRegister_Click" />
</asp:Content>


