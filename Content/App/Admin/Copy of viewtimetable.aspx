<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Copy of viewtimetable.aspx.vb" Inherits="Default2"  %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     
    <title></title>
    <link rel="stylesheet" href="../css/style1.css" type="text/css" media="all">

   
    </head>

    
       <body id="Body1">
           <form id="Form1" runat="server" >
<!-- START PAGE SOURCE -->
<div class="wrap">
  <header>
  </header>
  <div class="container">
      <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>

    <h2>SCHOOL TIME TABLE</h2>
&nbsp;
                 
        <asp:DropDownList ID="cboDay" runat="server" style="width:254px;" class="form-control-small" AutoPostBack="true" >
                               
                         <asp:ListItem Value="Select Day"></asp:ListItem>
                         <asp:ListItem Value="Monday"></asp:ListItem>
                         <asp:ListItem Value="Tuesday"></asp:ListItem>
                         <asp:ListItem Value="Wednesday"></asp:ListItem>
                         <asp:ListItem Value="Thursday"></asp:ListItem>
                         <asp:ListItem Value="Friday"></asp:ListItem>
                               
       </asp:DropDownList>
        <br />
             <br />

      <asp:Panel ID="Panel1" runat="server"></asp:Panel>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              
    
    
    
      
        </div>
    
</div>
<footer>
  <div class="footerlink">
    <p class="lf">Copyright &copy; 2010 <a href="#">SiteName</a> - All Rights Reserved</p>
    <p class="rf">Design by <a href="http://www.templatemonster.com/">TemplateMonster</a></p>
    <div style="clear:both;"></div>
  </div>
</footer>
<script type="text/javascript"> Cufon.now(); </script>
<!-- END PAGE SOURCE -->
               </form>
</body>


   

