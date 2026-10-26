<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="testdetails.aspx.vb" Inherits="Admin_staffprofile" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <h2>TEST DETAILS</h2>
      
            
  

      
                      
    
                                     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1></h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    
                                                                <asp:Button ID="btnEdit" class="btn btn-white btn-xs"  runat="server" Text="Edit Test"></asp:Button><asp:Button ID="btnQuestion" class="btn btn-white btn-xs"  runat="server" Text="Questions"></asp:Button> <asp:Button ID="btnValidate" class="btn btn-white btn-xs"  runat="server" Text="Validate Test"></asp:Button> <asp:Button ID="btnReport" class="btn btn-white btn-xs"  runat="server" Text="Test Report"></asp:Button></div></div> </div> </div> 


   
                                 <br />
                           
                            <asp:DetailsView ID="DetailsView1" runat="server" class="newtable"  GridLines="None">
                                <EditRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            </asp:DetailsView>
                            


                           
                            



                          



                            
                            



                          
                            


      
    

</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 
                 

   
       
              
</asp:Content>

