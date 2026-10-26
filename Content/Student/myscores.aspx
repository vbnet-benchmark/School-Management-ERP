<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="myscores.aspx.vb" Inherits="Account_Default" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <h2>MY SCORES
        
    </h2>
   
   

                   <asp:Panel runat="server" id ="panel3" visible="false">
   
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1><asp:Label ID="lblClass" runat="server" ></asp:Label></h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

    


    <div class="row"><div class="row" id="dd" style="margin:5px;text-align:center;">
                        <span class="picspan" "  >  <asp:Image  ID="Image1" ImageUrl="~/img/noimage.jpg" CssClass="picsupload"  runat="server" /> </span> <br /> </div> 
                           </div>

     <br />
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
                <asp:GridView ID="GridView2" GridLines="None" class="table" runat="server">
                    
      
                   
      
      </asp:GridView></div> </div> 
        
      <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="Button1" class="btn btn-white btn-xs" TabIndex="8"  runat="server"  Text="View Result" ></asp:Button></div></div> </div> </div> 



</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

               
        <asp:Panel id="panel4" runat="server" >
                  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Statistics</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">


      <table class="table" runat="server" id="tblstats">
            <tr>
                <td><asp:Label ID="Label11" runat="server" style="font-size: small; font-weight: 700" Text="Average:"></asp:Label></td>
                <td><asp:label ID="lblAverage" runat="server" ></asp:label></td>
            </tr>
           
          </table>
          <table class="table">
            <tr>
               <td><asp:Label ID="Label27" runat="server" style="font-size: small; font-weight: 700" Text="Comments:"></asp:Label></td>
                <td><asp:label ID="lblComments" runat="server" ></asp:label></td>
            </tr>
            
        </table>




</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 





        </asp:Panel>
                        
 </asp:Panel> 
   
  





       
</asp:Content>
