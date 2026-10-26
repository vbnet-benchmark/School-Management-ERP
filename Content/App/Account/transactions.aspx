<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="transactions.aspx.vb" Inherits="Admin_allstudents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>MANAGE TRANSACTIONS</h2>
    <br />


     <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Filter Account</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboAcc" autopostback="true" runat="server"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

    <br />
     <div class="row">
                   <div class="col-lg-12">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group"> 
                                                                <asp:LinkButton ID="LinkButton2" class="btn btn-white btn-xs" runat="server">Add transaction</asp:LinkButton>        
     <asp:LinkButton ID="LinkButton3" class="btn btn-white btn-xs" runat="server">Debtors List</asp:LinkButton>
        <asp:LinkButton ID="LinkButton4" class="btn btn-white btn-xs" runat="server">Debt Reminder SMS</asp:LinkButton>
        
        <asp:lINKButton ID="btnInstt" class="btn btn-white btn-xs" runat="server" Text="Income Statement"  />
        
    <asp:LinkButton ID="LinkButton5" class="btn btn-white btn-xs" runat="server">Cummulative Income Statement</asp:LinkButton>
        
        <asp:LinkButton ID="Button4" class="btn btn-white btn-xs" runat="server" Text="Balance Sheet" /></div></div> </div> </div> 

    <div class="row">
        <div class="col-lg-9">

        </div>
        <div class="col-lg-3">
             <asp:Label ID="lblBal" runat="server" style="font-weight: 700"></asp:Label>

         <asp:Label ID="lblType" runat="server" style="font-weight: 700"></asp:Label>

        </div>

    </div>


    <asp:Panel ID="pnlAll" runat="server" DefaultButton="button3">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                       <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                      <div class="form-group-inner">
                                                            
                                                                <div class="col-lg-6">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">From</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:TextBox ID="DatePicker1" class="form-control" runat="server"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
            TargetControlID="DatePicker1" Format="dd/MM/yyyy" 
            PopupButtonID="TextBox2" >
        </cc1:CalendarExtender> </div></div> 
                                                           
                                                       
                                         <div class="col-lg-6">
                                       
                                                            
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">To</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:TextBox ID="DatePicker2" class="form-control" runat="server"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
            TargetControlID="DatePicker2" Format="dd/MM/yyyy" 
            PopupButtonID="TextBox2" >
        </cc1:CalendarExtender> </div>
                                                           
                                                        </div></div> 
                                       
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
            
             <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
  <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="table" >
             <Columns>
        <asp:HyperLinkField DataTextField="ref" DataNavigateUrlFields="ref" DataNavigateUrlFormatString="~/content/app/Account/transact.aspx?{0}" >
                     <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />  </asp:HyperLinkField>
                 <asp:boundfield datafield="date" readonly="true" headertext="Date"/>
                  <asp:boundfield datafield="account" headertext="Account"/>
                 <asp:boundfield datafield="type" headertext="Account type"/>
                 <asp:boundfield datafield="details" readonly="true" headertext="Details"/>
     
      <asp:boundfield datafield="dr" headertext="Debit"/>
                  <asp:boundfield datafield="cr" headertext="Credit"/>
                 </Columns> 
        </asp:GridView>


                                                                </div> </div> 
                                               
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnPrevious" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Newer" />
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnNext" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Older" /></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 

                    </div> 
    </asp:Panel>
            
  
   
</asp:Content>


