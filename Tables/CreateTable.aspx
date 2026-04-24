<%@ Page Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="CreateTable.aspx.cs" Inherits="StajOdev.Tables.CreateTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Yeni Tablo Oluştur</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br />

    <asp:TextBox ID="txtTableName" runat="server" CssClass="input-medium" />
    <asp:Button ID="btnAddField" runat="server" Text="Alan Ekle" CssClass="btn btn-small" OnClick="btnAddField_Click" />

    <asp:PlaceHolder ID="phFields" runat="server" />

    <br /><br />
    <asp:Button ID="btnCreateTable" runat="server" Text="Tabloyu Oluştur" CssClass="btn btn-primary" OnClick="btnCreateTable_Click" />

</asp:Content>
