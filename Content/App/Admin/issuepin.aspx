<%@ Page Title="" Language="VB" MasterPageFile="~/Admin/adminmaster.master" AutoEventWireup="false" CodeFile="issuepin.aspx.vb" Inherits="Admin_issuepin" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link id ="link0" href="../css/normalize.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link1" href="../css/demo.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/tabs.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link3" href="../css/tabstyles.css" rel="stylesheet" type="text/css" runat="server" />
    
     <p><asp:Label ID="Label12" runat="server" Text="Admission No:" style="font-weight: 700"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtID" runat="server" Width="335px"></asp:TextBox>
         <asp:Button ID="Button2" runat="server" Text="Search" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtid" runat="server" ErrorMessage="Please enter a valid admission no" ForeColor="#FF3300"></asp:RequiredFieldValidator>
         </p>
    <p>
        <asp:Label ID="Label13" runat="server"></asp:Label>
         </p>
    <p>
        <asp:Label ID="Label14" runat="server"></asp:Label>
         </p>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:Button ID="Button1" runat="server" Text="Issue" Visible="False" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="Button3" runat="server" Text="Reset" Visible="False" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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


