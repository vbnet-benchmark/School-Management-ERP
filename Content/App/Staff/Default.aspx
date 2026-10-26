<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Staff/Main2.Master" CodeBehind="Default.aspx.vb" Inherits="StaffPortal._Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style7
        {
            width: 197px;
        }
        .style8
        {
            width: 248px;
        }
    .style9
    {
        width: 216px;
    }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
            <div style="height: 151px">
                <table style="width:100%; height: 132px; cursor: pointer;" align="center">
                    <tr>
                        <td class="style7">
                            <div style="width: 183px; height: 113px">
                                `<asp:ImageButton ID="ImageButton1" runat="server" Height="98px" 
                                    ImageUrl="~/image/students-school.jpg" Width="129px" />
                            </div>
                        </td>
                        <td class="style8">
                            <div style="width: 195px">
                                <asp:ImageButton ID="ImageButton2" runat="server" Height="120px" 
                                    ImageUrl="~/image/Thinkstock.jpg" style="margin-left: 24px" 
                                    Width="140px" />
                            </div>
                        </td>
                        <td class="style9">
                            <div style="width: 176px">
                                <asp:ImageButton ID="ImageButton3" runat="server" Height="107px" 
                                    ImageUrl="~/image/School-test.jpg" style="margin-left: 22px" 
                                    Width="136px" />
                            </div>
                        </td>
                        <td>
                            <div>
                                <asp:ImageButton ID="ImageButton4" runat="server" Height="131px" 
                                    ImageUrl="~/image/au.jpg" Width="122px" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
</asp:Content>
