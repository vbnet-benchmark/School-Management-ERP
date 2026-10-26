<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="courseoverview.aspx.vb" Inherits="Admin_addteacher" %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
      <h2>COURSE DETAILS</h2>

    <br />
    <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="Button1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back" ></asp:Button><asp:Button ID="lnkScore" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Score History" ></asp:Button></div></div> </div> </div> 
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Course Summary</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

     <asp:DetailsView ID="DetailsView1"  runat="server" autogeneratecolumns="true" GridLines="None" class="table" >
            </asp:DetailsView> 





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 


  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Course Outline</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
     <asp:GridView ID="GridView1" AutoGenerateColumns="false" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="table" >
              <Columns>
                                <asp:boundfield datafield="week" headertext="Week"/>
                                   <asp:boundfield datafield="topic" headertext="Topic"/>
                          <asp:boundfield datafield="content" headertext="Content"/>
         
                 </Columns> 
        </asp:GridView></div> </div> 





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
   </asp:Content>

