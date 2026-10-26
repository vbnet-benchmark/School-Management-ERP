<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="reviewtest.aspx.vb" Inherits="Admin_addteacher" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:Timer ID="Timer1" runat="server" Interval ="1000"></asp:Timer>
                    <h2>TEST REVIEW</h2>

     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        
                                        <h1><asp:Label runat="server" ID="lblTest"></asp:Label></h1>
                                      
                                      
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     
     <asp:PlaceHolder id="plcQuestions" runat="server"></asp:PlaceHolder>     
 
 </div>
          
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnBack" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Back" />

</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
           
      

  
   </asp:Content>

