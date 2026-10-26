<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="newquestion.aspx.vb" Inherits="Admin_addteacher" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
                    <h2>CREATE QUESTION</h2>
     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Number <asp:Label runat="server" ID="lblqno"></asp:Label></h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     
      <div class="form-group-inner">
          <div class="row">
              <div class="col-lg-3">
                  <label class="login2 pull-right pull-right-pro">Question</label>
              </div> 

          </div>
                                                             <div class="row">
                                                    <div class="col-lg-12">
                                                        <div class="text-editor-compose">
                                                            <div id="summernote5">
                                                               
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
    
    <asp:HiddenField id="Hidden1" runat="server"></asp:HiddenField>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Option A</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtopta" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Option B</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtoptb" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Option C</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtoptc" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Option D</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtoptd" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Correct Answer</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboCorrect" runat="server"  >
                                                                           <asp:ListItem>Select</asp:ListItem>     
                                                                           <asp:ListItem>A</asp:ListItem>
                                                                           <asp:ListItem>B</asp:ListItem>
                                                                           <asp:ListItem>C</asp:ListItem>
                                                                           <asp:ListItem>D</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
          
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="bnCreate" onclientclick="store()" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Create" /> <asp:Button ID="btnBack" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Back" />

</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
           
         </div> 


  
   </asp:Content>

