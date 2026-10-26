<%@ Page Language="VB" AutoEventWireup="false" CodeFile="passwordresetconfirm.aspx.vb" Inherits="_PasswordResetConfirm" %>

<!DOCTYPE html>
<head id="Head1" runat="server">
    <title>Confirm Password Reset</title>
    <style type="text/css">
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
    </style>
</head>
<body>
    <form id="form1" runat="server"
    style="width: 60%; margin-left: 21%; margin-right: 20%; height: 268px;">
    <div align="center">
    <h4>Enter the confirmation code we sent you.</h4>
        <table style="width:84%; margin-left: 131px; margin-top: 15px; height: 100px;">
            <tr>
                <td class="auto-style1">
                    <strong>Email / Staff ID:</strong></td>
                <td class="style2">
                    <asp:TextBox ID="txtConfirmId" runat="server" Width="169px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <strong>Confirmation Code:</strong></td>
                <td class="style2">
                    <asp:TextBox ID="txtConfirmCode" runat="server" Width="169px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="style2">
                    <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="btn btn-primary" OnClick="btnConfirm_Click" />
                    <asp:Button ID="btnConfirmSecure" runat="server" Text="Confirm (Verified)" CssClass="btn btn-warning" OnClick="btnConfirmSecure_Click" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>

     <asp:Label ID="lblConfirmError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>

    </div>
    </form>
</body>
</html>
