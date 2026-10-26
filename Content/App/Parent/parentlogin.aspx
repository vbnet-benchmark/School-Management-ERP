<%@ Page Language="VB" AutoEventWireup="false" CodeFile="parentlogin.aspx.vb" Inherits="Staff_stafflogin" %>

<!DOCTYPE html>
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
        .auto-style1 {
            width: 155px;
        }
        #Password1 {
            width: 170px;
        }
        #txtpassword {
            width: 170px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" 
        
    style="width: 60%; margin-left: 21%; margin-right: 20%; height: 268px;">
    <div align="center">
    <h4>Enter Staff details.</h4>
        <table style="width:84%; margin-left: 131px; margin-top: 15px; height: 100px;">
            <tr>
                
                <td class="auto-style1">
                    <strong>Staff ID:</strong></td>
                <td class="style2">
                    <asp:TextBox ID="txtID" runat="server" Width="169px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <strong>Password:</strong></td>
                <td class="style2">
                    <input id="txtpassword" type="password"  runat="server" /></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="style2">
                    <asp:Button ID="Button1" runat="server" Text="Login" style="font-weight: 700" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>

     <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>



   &nbsp;

    <h4>New parent? Create your portal account.</h4>
        <table style="width:84%; margin-left: 131px; margin-top: 15px;">
            <tr>
                <td class="auto-style1"><strong>Your Name:</strong></td>
                <td class="style2"><asp:TextBox ID="txtRegName" runat="server" Width="169px"></asp:TextBox></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1"><strong>Email (Parent ID):</strong></td>
                <td class="style2"><asp:TextBox ID="txtRegEmail" runat="server" Width="169px"></asp:TextBox></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1"><strong>Choose Password:</strong></td>
                <td class="style2"><asp:TextBox ID="txtRegPassword" runat="server" Width="169px" TextMode="Password"></asp:TextBox></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="style2">
                    <asp:Button ID="btnRegisterQuick" runat="server" Text="Create Account" CssClass="btn btn-primary" OnClick="btnRegisterQuick_Click" />
                    <asp:Button ID="btnRegisterSecure" runat="server" Text="Create Account (Verified)" CssClass="btn btn-warning" OnClick="btnRegisterSecure_Click" />
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <asp:Label ID="lblRegError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>

    </div>
    </form>
</body>
</html>
