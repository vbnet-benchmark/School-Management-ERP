<%@ Page Title="" Language="VB" MasterPageFile="~/DigicontentApp.master" AutoEventWireup="false" CodeFile="smessages.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <h2>RECEIVED MESSAGES</h2>

    <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Subordinate</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="DropDownList1" runat="server" autopostback="true" >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

   <asp:Panel ID="pnlAll" runat="server" DefaultButton="button3">
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                       <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        
                                            
                                        <asp:Label ID="Label1" Font-Bold="true" Font-Size="Larger" runat="server" Text="Sent Messages"></asp:Label>
											
                                       
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
                 <asp:Button ID="LinkButton1" runat="server" class="btn btn-white btn-xs" Text="Sent Messages"></asp:Button>

                                                            </div></div> </div> </div> 
         <br />
          <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
          <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" GridLines="None" class="table" >
              <Columns>
                   <asp:HyperLinkField DataNavigateUrlFields="ID,receiver" DataNavigateUrlFormatString="~/content/app/Staff/sreadmsg.aspx?id={0}&amp;receiver={1}" Text="Read" ><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:hyperlinkfield>
                   <asp:boundfield datafield="date" headertext="Date"/>
                   <asp:BoundField DataField="receiver" HeaderText="Receiver"></asp:BoundField>
                 <asp:boundfield datafield="sender" headertext="Sender"/>
                                  <asp:boundfield datafield="sendertype" headertext="Sender Portfolio"/>

                          <asp:boundfield datafield="subject" headertext="Subject"/>
            <asp:boundfield datafield="status" headertext="Status"/>
                
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




