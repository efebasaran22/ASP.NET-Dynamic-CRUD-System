<%@ Page Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="InsertData.aspx.cs" Inherits="StajOdev.Tables.InsertData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Veri Ekle</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br />

    <asp:DropDownList ID="ddlTables" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTables_SelectedIndexChanged" />

    <asp:PlaceHolder ID="phInputs" runat="server" />

    <br />
    <asp:Button ID="btnInsert" runat="server" Text="Ekle" OnClick="btnInsert_Click" CssClass="btn btn-primary" />

</asp:Content>

