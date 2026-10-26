<%@ Page Title="" Language="VB" MasterPageFile="~/Admin/adminmaster.master" AutoEventWireup="false" CodeFile="Copy of newterm.aspx.vb" Inherits="Admin_newterm" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link id ="link0" href="../css/normalize.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link1" href="../css/demo.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/tabs.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link3" href="../css/tabstyles.css" rel="stylesheet" type="text/css" runat="server" />
   <p><asp:Label ID="Label12" runat="server" Text="Session:" style="font-weight: 700"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:DropDownList ID="DropDownList1" runat="server" Height="16px" Width="318px">
       </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="dropdownlist1" runat="server" ErrorMessage="Please select a session" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                </p>
    <p><asp:Label ID="Label1" runat="server" Text="Term:" style="font-weight: 700"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="DropDownList2" runat="server" Height="16px" Width="324px">
        <asp:ListItem Value="1st term"></asp:ListItem>
        <asp:ListItem Value="2nd term"></asp:ListItem>
        <asp:ListItem Value="3rd term"></asp:ListItem>
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="dropdownlist2" runat="server" ErrorMessage="Please select a term" ForeColor="#FF3300"></asp:RequiredFieldValidator>
         </p>
   <p> <asp:Label ID="Label4" runat="server" Text="Closing date:" style="font-weight: 700"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="DropDownList3" runat="server" Width="75px" Height="16px">
                    <asp:ListItem Selected="True">Day</asp:ListItem>
                </asp:DropDownList>
               
       <asp:DropDownList ID="DropDownList4" runat="server" Width="162px">
           <asp:ListItem Selected="True">Month</asp:ListItem>
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
       <asp:DropDownList ID="DropDownList5" runat="server" Height="22px" Width="101px">
           <asp:ListItem Selected="True">Year</asp:ListItem>
       </asp:DropDownList>
               
</p>
  <p> <asp:Label ID="Label2" runat="server" Text="Next term begins on:" style="font-weight: 700"></asp:Label>
                &nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="DropDownList6" runat="server" Width="75px">
                    <asp:ListItem Selected="True">Day</asp:ListItem>
                </asp:DropDownList>
               
       <asp:DropDownList ID="DropDownList7" runat="server" Width="162px">
           <asp:ListItem Selected="True">Month</asp:ListItem>
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
       <asp:DropDownList ID="DropDownList8" runat="server" Height="22px" Width="101px">
           <asp:ListItem Selected="True">Year</asp:ListItem>
       </asp:DropDownList>
               
</p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
    <asp:Button ID="Button1" runat="server" Text="Add" />
    <br />
    <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    
         </asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="head">
    <style type="text/css">
        #span1 {
            width: 382px;
        }
        #span2 {
            width: 352px;
        }
    </style>
</asp:Content>


