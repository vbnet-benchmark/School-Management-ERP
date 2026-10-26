<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="questions.aspx.vb" Inherits="Admin_addteacher" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:Timer ID="Timer1" runat="server" Interval ="1000" ></asp:Timer>
                    <h2>TEST QUESTIONS</h2>

     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <asp:UpdatePanel ID="UpdatePanel100" runat="server" updatemode="conditional">
                                              <ContentTemplate>
                                <div class="sparkline12-hd" id="llk2" style="min-height: 60px;">
                                    <div class="main-sparkline12-hd" >
                                        <div class="col-lg-4">
                                        <h1>Question <asp:Label runat="server" ID="lblqno"></asp:Label></h1>
                                       </div>
                                         <div class="col-lg-4">
                                        <h1>Time Allocated: <asp:Label runat="server" ID="lblTimeAllocated"></asp:Label></h1>
                                       </div>
                                         
                                         <div class="col-lg-4">
                                        <h1>Remaining Time: <asp:Label runat="server" ID="lblTimeLeft"></asp:Label></h1>
                                       </div>
                                                  
                                    </div>
                                </div>
                                    </ContentTemplate></asp:UpdatePanel>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                   <h2> <asp:Label runat="server" ID="lblQuestion"></asp:Label></h2>
                                                                </div>
                                                                                                                           </div>
                                                        </div>
  <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                    <asp:RadioButtonList id="radOptions" runat="server">
                                                                        <asp:ListItem></asp:ListItem>
                                                                        <asp:ListItem></asp:ListItem>
                                                                        <asp:ListItem></asp:ListItem>
                                                                        <asp:ListItem></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </div>
                                                                                                                           </div>
                                                        </div>     
 
 </div>
          
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="bnNext" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Next Question" /> <asp:Button ID="btnBack" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Exit Test" />

</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      


                        </div>
                        
                    </div>
                    
         <//div> 

  
   </asp:Content>

