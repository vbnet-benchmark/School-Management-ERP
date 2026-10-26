<%@ Page Title="" Language="VB" MasterPageFile="~/Staff/Main2.master" AutoEventWireup="false" CodeFile="payment.aspx.vb" Inherits="Admin_payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h3>Payment Status</h3>
                <asp:DropDownList ID="cboSession" runat="server" AutoPostBack="True" Height="20px" OnSelectedIndexChanged="cboSession_SelectedIndexChanged" Width="155px">
                    <asp:ListItem Selected="True">Select Session</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="cboTerm" runat="server" Width="132px">
                    <asp:ListItem Selected="True">Select Term</asp:ListItem>
                </asp:DropDownList>
                <br />
                <p>
                    <asp:Label ID="Label3" runat="server" Text="Amount paid:"></asp:Label> &nbsp;&nbsp;&nbsp; <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></p>
                                    <asp:Label ID="Label4" runat="server" Text="Outstanding"></asp:Label> &nbsp;&nbsp; &nbsp;&nbsp; <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></p>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    
    
    
    
    
   </asp:Content>