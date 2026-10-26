<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Student/MyMaster.Master" CodeBehind="Bills.aspx.vb" Inherits="StaffPortal.Bills" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 552px">
        <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
            ContextTypeName="StaffPortal.myClassDataContext" EntityTypeName="" 
            TableName="tblSubjects">
        </asp:LinqDataSource>
    </div>
</asp:Content>

