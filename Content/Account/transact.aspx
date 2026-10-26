<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="transact.aspx.vb" Inherits="Account_Default" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
    <h2>MANAGE TRANSACTIONS
  <br />

                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Transaction Reference No: <asp:Label ID="lblRef" runat="server" style="font-weight: 700"></asp:Label></h1>
                                       
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
    <asp:Button ID="lnkBack" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back to Transactions" ></asp:Button></div></div> </div> </div> 
     <br />
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Debit Account</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboDAccount" runat="server"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Credit Account</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboCAccount" runat="server"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="cboDAmount" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Description</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox textmode="multiline" cssclass="form-control" ID="cboDDetails" runat="server"  ></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="lblPost" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Post" /> <asp:Button ID="btnReverse" visible="false" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Reverse" /></div>




</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 






   
     
</asp:Content>

