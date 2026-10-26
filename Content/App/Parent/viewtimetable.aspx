<%@ Page Language="VB" AutoEventWireup="false" CodeFile="viewtimetable.aspx.vb" Inherits="Default2"  %>
<!DOCTYPE html>

<html xmlns="https://www.w3.org/1999/xhtml">
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

    <h2>CLASS TIME TABLE</h2>
&nbsp;
                 <h4>
                     <asp:label runat="server" ID="lblTimetable"></asp:label>
                 </h4>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:LinkButton ID="LinkButton1" runat="server">Back to class</asp:LinkButton>
        <br />

       <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Time Table</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">

      <asp:Panel ID="Panel1" runat="server">
          <asp:GridView ID="GridView1" AutoGenerateColumns ="False"  runat="server" Height="16px" BorderStyle="None" BorderColor="#CCCCCC">
             
          </asp:GridView>
      </asp:Panel>

                                                    </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
      <br />
       <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Time Table</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
              <h4>Subject Teachers</h4>
    <br />
    <asp:GridView ID="GridView3" AutoGenerateColumns ="False"  runat="server" Height="16px" Width="40%" ShowFooter="True" BorderStyle="None" GridLines="Horizontal" BorderColor="#CCCCCC">
             <Columns>
                   <asp:BoundField DataField="S/N" HeaderText="S/N"></asp:BoundField>
                 <asp:boundfield datafield="subject" HeaderText="Subject" />
                                  <asp:boundfield datafield="name" HeaderText="Teacher" />

                                             <asp:BoundField DataField="periods" HeaderText="Periods"></asp:BoundField>
        
                 </Columns> 

        </asp:GridView>
    
      
        </div>
    
</div
        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> >
<footer>
  <div class="footerlink">
    <p class="lf">Copyright &copy; 2010 <a href="#">SiteName</a> - All Rights Reserved</p>
    <p class="rf">Design by <a href="https://www.templatemonster.com/">TemplateMonster</a></p>
    <div style="clear:both;"></div>
  </div>
</footer>
<script type="text/javascript"> Cufon.now(); </script>
<!-- END PAGE SOURCE -->
               </form>
</body>


   

