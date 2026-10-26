<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Student/MyMaster.Master" CodeBehind="s_Default.aspx.vb" Inherits="StaffPortal.s_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 194px;
        }
        .style2
        {
            width: 239px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 394px">
        <table style="width: 100%; height: 295px;">
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td class="style2">
                    <div align="center">
                        <asp:ImageButton ID="ImageButton1" runat="server" Height="147px" 
                            ImageUrl="~/image/7600233_G.jpg" Width="183px" />
                        <br />
                        Terminal Report</div>
                </td>
                <td>
                    <div align="center" style="width: 304px">
                        <asp:ImageButton ID="ImageButton2" runat="server" Height="154px" 
                            ImageUrl="~/image/bills.jpg" Width="285px" />
                        <br />
                        Print Billings</div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

