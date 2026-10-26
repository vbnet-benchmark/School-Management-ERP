<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeFile="~/Admin/Login.aspx.vb" Inherits="StaffPortal.Login2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 94px;
        }
        #form1
        {
            width: 595px;
            margin-left: 200px;
            margin-top: 332px;
        }
        .style2
        {
            width: 128px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" 
    style="width: 60%; margin-left: 20%; background-color: #C0C0C0; margin-right: 20%; height: 268px;">
    <div align="center" style="background-color: #669999">
    
        <table style="width:67%; margin-left: 133px; margin-top: 97px;">
            <tr>
                <td class="style1">
                    Student ID</td>
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
                    <asp:Button ID="Button1" runat="server" Text="Login" style="height: 26px" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
