<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="parentprofile.aspx.vb" Inherits="Admin_staffprofile" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
        <h2>YOUR PROFILE</h2>
  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Profile</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="row"><div class="row" id="dd" style="margin:5px;text-align:center;">
                        <span class="picspan" "  >  <asp:Image  ID="Image1" ImageUrl="~/img/noimage.jpg" CssClass="picsupload"  runat="server" /> </span> <br /> </div> 
                           </div>
     <div class="row">
                                                    <div class="col-lg-5">
                                                        <div class="comment-replay-profile" >
                                                            <div class="btn-group">
                                                               
                                                                <asp:Button ID="lnkPassport" runat="server" class="btn btn-white btn-xs" Text="Change Passport"></asp:Button>
                                                               
                                                               
                                                            </div>
                                                        </div>
                                                    </div>
                                                   
                                                </div>

 <asp:DetailsView ID="DetailsView1" runat="server" class="table"  GridLines="None">
                                <EditRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            </asp:DetailsView>

<div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton1" class="btn btn-white btn-xs"  runat="server" Text="Change Password"></asp:Button> <asp:Button ID="lnkEdit" class="btn btn-white btn-xs"  runat="server" Text="Edit Profile"></asp:Button></div></div> </div> </div> 

<asp:Panel ID="Panel1" runat="server" Visible="False">
     <div class="all-form-element-inner">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Password</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtPassword" runat="server" textmode="password"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

<div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Re enter password</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtPassword0" runat="server" textmode="password"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div> <div class="login-button-pro" style="text-align:right;" >  
         <asp:Button ID="Button1"  class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Update" runat="server" />

                     <asp:Button ID="btnCancel" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Cancel" /></div> 






         </div> 
             </asp:Panel> 
  


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
        <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Children/Wards</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

       <div id ="taught" class="table" runat ="server" ></div>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div>                   

  
      
    
    </asp:Content>