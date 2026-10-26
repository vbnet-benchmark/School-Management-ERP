<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="messages.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <h2>ADMIN MESSAGING</h2>
    <asp:Panel ID="pnlAll" runat="server" DefaultButton="button3">
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                       <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        
                                       <asp:Label ID="Label1" Font-Bold="true" Font-Size="Larger" runat="server" Text="Received Messages"></asp:Label>
                                       
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
        <div class="breadcome-heading">
                                       <span> <asp:Textbox class="form-control" ID="txtSearch"   placeholder="Search..." runat="server" style="width:100%;" ></asp:TextBox>
												<a href="#" id="sb" ><asp:LinkButton TabIndex="0" style="float:right; right:20; top:1; " class="fa fa-search" ID="Button3" runat="server"></asp:LinkButton> </a></span>
                                        </div>
                                    </div>
                                </div>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
            
              <br />
                                               
          
     <div class="form-group-inner">
                                                            <div class="row">
                                                                
                                                                     
                                                                
                                                                                   


                                                               
                                                            </div>
                                                       
   
            
    

          <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnMsg" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Message" ></asp:Button>
                                                                                                                                <asp:Button ID="LinkButton1" runat="server" class="btn btn-white btn-xs" Text="Sent Messages"></asp:Button>
 <asp:Button ID="btnSMS" runat="server" class="btn btn-white btn-xs" Text="SMS Reports"></asp:Button>
                                                            </div></div> </div> </div> 
         <br />
          <div class="comment-replay-profile">
                                                            <div class="btn-group"  style="width:100%;">
          <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="newtable" >
             <Columns>
                   <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="~/Content/Admin/readmsg.aspx?{0}" Text="Read" >
                       
                       <HeaderStyle  CssClass="newtableheader" />
                   </asp:HyperLinkField>
                   <asp:boundfield datafield="date" headertext="Date">
                      <HeaderStyle CssClass="newtableheader" />
                   </asp:boundfield>
                 <asp:boundfield datafield="sender" headertext="Sender">
                      <HeaderStyle CssClass="newtableheader" />
                                      </asp:boundfield>
                                  <asp:boundfield datafield="sendertype" headertext="Sender Portfolio">
                                      <HeaderStyle CssClass="newtableheader" />
                                      </asp:boundfield>

                          <asp:boundfield datafield="subject" headertext="Subject">
                               <HeaderStyle CssClass="newtableheader" />
                                      </asp:boundfield>
            <asp:boundfield datafield="status" headertext="Status">
                 <HeaderStyle CssClass="newtableheader" />
                                      </asp:boundfield>
                
                 </Columns> 
        </asp:GridView></div> </div> 
         <br />
         <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnPrevious" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Newer" ></asp:Button>
                                                                                                                                <asp:Button ID="btnNext" runat="server" class="btn btn-white btn-xs" Text="Older"></asp:Button>

                                                            </div></div> </div> </div> 





</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 

                    </div> 
        <br />
           
        </asp:Panel> 
      
      
</asp:Content>


