<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="addsubject.aspx.vb" Inherits="Admin_addsubject" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
                <h1>MANAGE SUBJECTS</h1>

 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>School Subjects</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

   
                                                            <div class="btn-group" id="grid" style="width:100%;">
                                                                 <div class="row">
        <div class="col-lg-4">
                                   <asp:GridView GridLines="Horizontal" autogeneratecolumns="false" class="newtable tableflat"  showheader="False" BorderColor="#CCCCCC"  BorderStyle="None" width="100%"  id="gridRecipients" runat="server">
                                       <Columns>
                                           <asp:BoundField DataField="name"></asp:BoundField>
                                           <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                                       </Columns>
                                    </asp:GridView>
                                    </div> 
                                        </div>  </div> 
                                                                 
                            </div>
                                                <br />
                                                <br />
     <h4>Add a new subject</h4>

     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtSubject" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Alias (3 Letters)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtAlias" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button3" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div>            


  </asp:Content>






