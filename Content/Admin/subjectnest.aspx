<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="subjectnest.aspx.vb" Inherits="Admin_addsubject" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
                <h1>NEST SUBJECTS</h1>

 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Subjects</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
                                                                 <div class="row">
        <div class="col-lg-6">
      <asp:GridView  runat="server" autogeneratecolumns ="false" ID="Gridview2" ShowHeader="false" GridLines="None" class="newtable" >
                     <Columns>
                        <asp:BoundField DataField="name" HeaderText="Period"></asp:BoundField>
                         <asp:CommandField SelectText="View" ShowSelectButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>                    

                         <asp:CommandField ShowDeleteButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>

                     </Columns>

                    
                </asp:GridView></div> </div> </div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Nest" ></asp:Button></div></div> </div> </div> 

     <br />
     <asp:Panel ID="Panel2" runat="server" Visible="false">

         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Collective Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtGrp" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <br />
         <h4>Subjects</h4>     
                        <asp:CheckBoxList ID="CheckBoxList2" runat="server" class="form-control-small" EnableViewState="true" >
                            </asp:CheckBoxList>
                        <br />
                        <h4>Classes affected</h4>
                        <asp:CheckBoxList ID="chkClasses" runat="server" class="form-control-small" EnableViewState="true" >
                            </asp:CheckBoxList>

         <br />
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
               <asp:Button ID="Button2" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Cancel" />

              </div>


       



         </asp:Panel> 
</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 




  </asp:Content>






