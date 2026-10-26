<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LoginMaster.Master" CodeBehind="_Login.aspx.vb" Inherits="StaffPortal._Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 61px;
        }
        .style2
        {
            width: 128px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 540px; width: 950px;">
        <div style="width: 506px; margin-top: 309px; background-color: #FFCC00; height: 201px; margin-left: 360px;">
        
        <table style="width: 100%; background-color: #C0C0C0;">
            <tr>
                <td class="style1">
                    Username</td>
                <td class="style2">
                    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style1">
                    Password</td>
                <td class="style2">
                    <asp:TextBox ID="txtPass" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td class="style2">
                    <asp:Button ID="Button1" runat="server" Text="Login" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
        </div>
    </div>
</asp:Content>
