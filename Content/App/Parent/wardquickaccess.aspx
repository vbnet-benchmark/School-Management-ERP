<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wardquickaccess.aspx.vb" Inherits="Parent_WardQuickAccess" %>

<!DOCTYPE html>
<head id="Head1" runat="server">
    <title></title>
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
    <h4>View all my children's portals.</h4>
        <table style="width:84%; margin-left: 131px; margin-top: 15px; height: 100px;">
            <tr>
                <td class="auto-style1">
                    <strong>Parent ID:</strong></td>
                <td class="style2">
                    <asp:TextBox ID="txtWardParentId" runat="server" Width="169px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <strong>Password:</strong></td>
                <td class="style2">
                    <asp:TextBox ID="txtWardPassword" runat="server" Width="169px" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="style2">
                    <asp:Button ID="btnQuickAccessAll" runat="server" Text="View All My Children's Portals" CssClass="btn btn-primary" OnClick="btnQuickAccessAll_Click" />
                    <asp:Button ID="btnQuickAccessAllSecure" runat="server" Text="View All My Children's Portals (Verified)" CssClass="btn btn-warning" OnClick="btnQuickAccessAllSecure_Click" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>

     <asp:Label ID="lblWardError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>

    </div>
    </form>
</body>
</html>
