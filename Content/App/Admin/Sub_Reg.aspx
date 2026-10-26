<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/admin.master" CodeBehind="Sub_Reg.aspx.vb" Inherits="StaffPortal.Sub_Reg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 48px;
        }
        .style2
        {
            width: 77px;
        }
        .style4
        {
            width: 84px;
        }
        .style6
        {
            width: 46px;
        }
        .style7
        {
            width: 129px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 352px">
        <table style="width:100%;">
            <tr>
                <td class="style1">
                    Staff ID</td>
                <td class="style2">
                    <asp:DropDownList ID="DropDownList1" runat="server" Height="16px" Width="134px">
                    </asp:DropDownList>
                </td>
                <td class="style4">
                    Subject Code
                </td>
                <td class="style7">
                    <asp:DropDownList ID="DropDownList2" runat="server" Height="16px" Width="124px">
                    </asp:DropDownList>
                </td>
                <td class="style6">
                    Subject</td>
                <td>
                    <asp:DropDownList ID="DropDownList3" runat="server" Height="18px" Width="133px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Add&gt;&gt;" />
                </td>
            </tr>
        </table>
        <br />
        <div>
            <asp:GridView ID="GridView1" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
