<%@ Page Title="" Language="VB" MasterPageFile="~/digicontent.master" AutoEventWireup="false" CodeFile="boarding.aspx.vb" Inherits="Admin_staffprofile" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2>MANAGE HOSTELS</h2> 
  <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
  
                                                                  <asp:Button ID="LinkButton5" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Hostel" ></asp:Button>
                                                            </div></div> </div> </div> 
     
    <div class="all-form-element-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Select Hostel</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="DropDownList1" runat="server" autopostback="true"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
     </div>
    <asp:Panel runat="server" id="panel1" > 
        <div class="all-form-element-inner">
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Hostel Details</h1>
                                       
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
                                <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
                                                                  <asp:Button ID="LinkButton1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Edit Hostel" ></asp:Button>
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