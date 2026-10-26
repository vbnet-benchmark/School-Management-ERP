<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" ValidateRequest=false AutoEventWireup="false" CodeFile="deptmembers.aspx.vb" Inherits="Admin_adminpage" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
       <h2>MANAGE DEPARTMENTS</h2>
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Department Members</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
       <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
  
                                                                  <asp:Button ID="lnkBack" OnClick="Unnamed1_Click" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back" ></asp:Button>
                                                            </div></div> </div> </div> 
     <br />
     <h3><asp:Label runat="server" id ="label1"></asp:Label></h3>
     <br />
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Add Member (Staff iD)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtID" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button2" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
     <br />
         <br />
         <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
      <asp:GridView ID="GridView1" runat="server" style="text-align: left"   GridLines="None" class="table" AutoGenerateColumns="False"  >
                   <Columns>
                       <asp:BoundField DataField="S/N" HeaderText="S/N"></asp:BoundField>
                       <asp:BoundField HeaderText="Staff ID" DataField="Staff Id"></asp:BoundField>
                       <asp:BoundField DataField="Staff Name" HeaderText="Staff Name"></asp:BoundField>
                       <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                       <asp:CommandField DeleteText="Remove" ShowDeleteButton="True">
                           <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" />
                       </asp:CommandField>
                   </Columns>
    </asp:GridView>

                                                                </div> </div> 

</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 










              
   </asp:Content>

