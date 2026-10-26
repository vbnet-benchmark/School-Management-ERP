<%@ Page Title="" Language="VB" MasterPageFile="~/Account/accountmaster.master" AutoEventWireup="false" CodeFile="expenses.aspx.vb" Inherits="Account_expenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style7
        {
            width: 79px;
        }
        .style9
        {
            width: 80px;
        }
        .style13
        {
            width: 48px;
        }
        .style15
        {
            width: 770px;
            margin-left: 19px;
        height: 77px;
    }
        .style16
        {
            width: 62px;
        }
        .style18
        {
            width: 181px;
        }
    .style20
    {
        width: 199px;
    }
    .style21
    {
        width: 247px;
    }
    .style22
    {
        }
    .style23
    {
        width: 72px;
    }
        .style24
        {
            width: 222px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
        <h3><asp:Label ID="Label1" runat="server" Text="Expenses"></asp:Label></h3>
    <p><asp:Label ID="Label2" runat="server" Text="Expense type:"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="cboType" runat="server" Height="23px" Width="142px">
        <asp:ListItem Value="Select expense type"></asp:ListItem>
        <asp:ListItem Value="Tax"></asp:ListItem>
        <asp:ListItem Value="Purchases"></asp:ListItem>
        <asp:ListItem>Salary</asp:ListItem>
        <asp:ListItem Value="Miscellaneous"></asp:ListItem>
        </asp:DropDownList></p>
                <p><asp:Label ID="Label12" runat="server" Text="Amount:"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtAmount" runat="server" Width="328px"></asp:TextBox>
                </p>
                <p><asp:Label ID="Label9" runat="server" Text="Remarks:"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtRem" runat="server" Width="333px" Height="44px" TextMode="MultiLine"></asp:TextBox></p>
     <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
       
     <p>
            <asp:Button ID="btnComfirm" runat="server" Text="Comfirm" />
        </p>
 
      
</asp:Content>
