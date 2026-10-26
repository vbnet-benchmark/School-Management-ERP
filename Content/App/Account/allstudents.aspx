<%@ Page Title="" Language="VB" MasterPageFile="~/Account/accountmaster.master" AutoEventWireup="false" CodeFile="allstudents.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link id ="link0" href="../css/normalize.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link1" href="../css/demo.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/tabs.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link3" href="../css/tabstyles.css" rel="stylesheet" type="text/css" runat="server" />
    
     <p>
         <asp:TextBox ID="txtSearch" runat="server" Height="24px" Width="242px"></asp:TextBox>
&nbsp;
         <asp:Button ID="Button3" runat="server" Text="Search" />
         </p>
    <p>
        <asp:GridView ID="GridView1" AutoGenerateColumns="false" runat="server" AllowPaging="True" PageSize="40" >
             <Columns>
        <asp:HyperLinkField DataTextField="admno" DataNavigateUrlFields="admno" DataNavigateUrlFormatString="~/Account/feeschedule.aspx?{0}"
            HeaderText="Admission No" ItemStyle-Width = "150" />
                 <asp:boundfield datafield="surname" readonly="true" headertext="Student's Name"/>
      <asp:boundfield datafield="sex" headertext="Sex"/>
      <asp:boundfield datafield="phone" headertext="Phone No"/>
                 </Columns> 
        </asp:GridView>
        <asp:Button ID="btnPrevious" runat="server" Text="Previous" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnNext" runat="server" Height="26px" Text="Next" Width="64px" />
        &nbsp;&nbsp;&nbsp;&nbsp;</p>
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


