<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin/admin.master" CodeBehind="Stud_Enroll.aspx.vb" Inherits="StaffPortal.Stud_Enroll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 94px;
        }
        .style3
        {
            width: 128px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 353px">
    <h3 align="center" style="color: #003366">Student Registration</h3>
        <div style="width: 65%; margin-left: 30%;">
        
        <table style="width:100%;">
            <tr>
                <td class="style1">
                    Student ID</td>
                <td class="style3">
                    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                </td>
                <td rowspan="6">
                    <div style="height: 178px">
                     
                        <img alt="" height="80px" src="" width="80px" /><br />
                        <br />
                        </td>
            </tr>
            <tr>
                <td class="style1">
                    Fullname</td>
                <td class="style3">
                    <asp:TextBox ID="txtFname" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    Class</td>
                <td class="style3">
                    <asp:DropDownList ID="cboClass" runat="server">
                        <asp:ListItem>Form1</asp:ListItem>
                        <asp:ListItem>Form2</asp:ListItem>
                        <asp:ListItem>Form3</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    House</td>
                <td class="style3">
                    <asp:DropDownList ID="cboHouse" runat="server">
                        <asp:ListItem>A1</asp:ListItem>
                        <asp:ListItem>A2</asp:ListItem>
                        <asp:ListItem>A3</asp:ListItem>
                        <asp:ListItem>B1</asp:ListItem>
                        <asp:ListItem>B2</asp:ListItem>
                        <asp:ListItem>B3</asp:ListItem>
                        <asp:ListItem>C1</asp:ListItem>
                        <asp:ListItem>C2</asp:ListItem>
                        <asp:ListItem>C3</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    Term</td>
                <td class="style3">
                    <asp:DropDownList ID="cboTerm" runat="server">
                        <asp:ListItem>First Term</asp:ListItem>
                        <asp:ListItem>Second Term</asp:ListItem>
                        <asp:ListItem>Third Term</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    Year</td>
                <td class="style3">
                    <asp:DropDownList ID="cboYr" runat="server">
                        <asp:ListItem>2015</asp:ListItem>
                        <asp:ListItem>2016</asp:ListItem>
                        <asp:ListItem>2017</asp:ListItem>
                        <asp:ListItem>2018</asp:ListItem>
                        <asp:ListItem>2019</asp:ListItem>
                        <asp:ListItem>2020</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <div align="center">

            <asp:ImageButton ID="ImageButton1" runat="server" Height="46px" 
                ImageUrl="~/image/V4.jpg" Width="141px" />

            <br />
            <asp:Label ID="lblmessage" runat="server"></asp:Label>

        </div>
        </div>
    </div>
</asp:Content>
