<%@ Page Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="ListTables.aspx.cs" Inherits="StajOdev.Tables.ListTables" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Tabloları Listele ve Sil</h2>

    <asp:GridView ID="gvTables" runat="server" AutoGenerateColumns="false" 
        OnRowCommand="gvTables_RowCommand" 
        OnSelectedIndexChanged="gvTables_SelectedIndexChanged"
        DataKeyNames="TableName" 
        AllowPaging="true" PageSize="10" 
        AutoGenerateSelectButton="true">
        <Columns>
            <asp:BoundField DataField="TableName" HeaderText="Tablo Adı" />
            <asp:ButtonField Text="Tabloyu Sil" CommandName="DeleteTable" ButtonType="Button" />
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />

    <hr />

    <h3>Seçilen Tablo Verileri</h3>

    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="true" 
        OnRowDeleting="gvData_RowDeleting" DataKeyNames="Id" >
        <Columns>
            <asp:CommandField ShowDeleteButton="true" DeleteText="Sil" />
        </Columns>
    </asp:GridView>

</asp:Content>
