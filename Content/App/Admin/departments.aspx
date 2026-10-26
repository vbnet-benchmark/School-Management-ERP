<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="departments.aspx.vb" Inherits="Admin_staffprofile" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     <h2>MANAGE DEPARTMENTS</h2>
    <br />
      <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
  
                                                                  <asp:Button ID="LinkButton5" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Department" ></asp:Button>
                                                            </div></div> </div> </div> 
     
    <div class="all-form-element-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Department</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="DropDownList1" runat="server" autopostback="true"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
     </div>
    <asp:Panel runat="server" id="panel1" visible="false"> 
        <div class="all-form-element-inner">
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Department Details</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">

     <div class="row">
                                            <div class="col-lg-12">
                                                <div class="row" id="dd" style="margin:5px; text-align:center;">
                                                <span class="picspan"  >
                    <asp:Image ID="Image1" ImageUrl="~/img/noimage.jpg" runat="server" CssClass="picsupload"   />
                    </span></div></div> </div> 
                                          
     <br />
      <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" class="table">
                            </asp:DetailsView>

     <br />
       <asp:Label ID="Label3" runat="server" style="font-weight: 700" Text="Sub Departments"></asp:Label>
                            <br />
                            <asp:BulletedList ID="subdeptList" runat="server"  Visible="False"  CssClass="form-control" BorderStyle="None" >
                            </asp:BulletedList>
                           
                                <br />
                                   <br />           
                                <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkView" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="View Members" visible="false" ></asp:Button>
                                                                  <asp:Button ID="LinkButton1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Edit Department" ></asp:Button>
                                                            </div></div> </div> </div> 


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

        </asp:Panel>
    
    
    
        </asp:Content>