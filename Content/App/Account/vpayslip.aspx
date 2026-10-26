<%@ Page Title="" Language="VB" MasterPageFile="~/Account/accountmaster.master" AutoEventWireup="false" CodeFile="vpayslip.aspx.vb" Inherits="Account_salary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3><asp:Label ID="Label1" runat="server" Text="Select Month and year" style="font-size: large"></asp:Label></h3>
                  <p> 
                      
       <asp:DropDownList ID="DropDownList2" runat="server" Width="162px">
           <asp:ListItem>Month</asp:ListItem>
           <asp:ListItem>January</asp:ListItem>
           <asp:ListItem>Febuary</asp:ListItem>
           <asp:ListItem>March</asp:ListItem>
           <asp:ListItem>April</asp:ListItem>
           <asp:ListItem>May</asp:ListItem>
           <asp:ListItem>June</asp:ListItem>
           <asp:ListItem>July</asp:ListItem>
           <asp:ListItem>August</asp:ListItem>
           <asp:ListItem>September</asp:ListItem>
           <asp:ListItem>October</asp:ListItem>
           <asp:ListItem>November</asp:ListItem>
           <asp:ListItem>December</asp:ListItem>
       </asp:DropDownList>
       <asp:DropDownList ID="DropDownList3" runat="server" Height="22px" Width="101px">
           <asp:ListItem>Year</asp:ListItem>
       </asp:DropDownList>
               
</p>
                <p>
                    <asp:Button ID="Button1" runat="server" Text="View Payslip" />
    </p>
                <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    
    
    
    
    
   </asp:Content>




