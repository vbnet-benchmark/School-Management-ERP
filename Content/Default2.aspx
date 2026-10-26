<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2"  %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     
    <title></title>
    <link rel="stylesheet" href="css/style1.css" type="text/css" media="all">

   
    <style type="text/css">
        .auto-style1 {
            text-decoration: underline;
        }
        .auto-style2 {
            text-decoration: underline;
            width: 144px;
        }
    </style>

   
</head>

    
       <body id="Body1">
           <form id="Form1" runat="server" >
<!-- START PAGE SOURCE -->
<div class="wrap">
  <header>
  </header>
  <div class="container">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
       <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <img src="image/loading36.gif" style="height: 31px; width: 36px" />
                Processing....
                </ProgressTemplate>
            </asp:UpdateProgress>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
      <table style="width:100%; margin-left: 332px;">
          <tr>
              <td>
                       <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>


              </td>

          </tr>
          <tr>
              <td>
              <h3 class="auto-style1" >User Login</h3>
              </td>
          </tr>
          <tr>
                <td>
    <h4>Enter your login details.</h4></td></tr></table>
        <table style="width:84%; margin-top: 15px; height: 100px; margin-left: 185px;">
            <tr>
                
                <td class="auto-style2">
                    <strong>User Name:</strong></td>
                <td class="style2">
                    <asp:TextBox ID="txtID" runat="server" class="form-control"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style2">
                    <strong>Password:</strong></td>
                <td class="style2">
                    <input id="txtpassword" type="password"  runat="server" class="form-control" /></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style2">
                    &nbsp;</td>
                <td class="style2">
                    <asp:Button ID="Button1" runat="server" Text="Login" style="font-weight: 700"  class="btn btn-skin btn-block btn-lg" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
        
    
    </ContentTemplate>
          </asp:UpdatePanel>
    
      
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
