<%@ Page Title="" Language="VB" MasterPageFile="~/digicontent.master" ValidateRequest=false AutoEventWireup="false" CodeFile="addbooks.aspx.vb" Inherits="Admin_adminpage" %>
   <%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
    
                <h2>NEW LIBRARY BOOKS
                </h2>

    <br />
    <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="Button1" class="btn btn-white btn-xs" runat="server" Text="Back" ></asp:Button></div></div> </div> </div> 
 <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Title</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                     <asp:Textbox cssclass="form-control" ID="txtTitle" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>







     <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-2">
                                                          <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubject" runat="server"   >
                                                                                </asp:DropDownList>           
                                                                               
                                                                     </div>
                                                                </div>
                                                            </div></div>
                                   <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Author</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                     <asp:Textbox cssclass="form-control" ID="txtAuthor" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Publisher</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                     <asp:Textbox cssclass="form-control" ID="txtPublisher" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
 <div class="row">
          <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Upload PDF</label>
                                                                </div>
                                                 <div class="col-lg-10 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="file-upload-inner file-upload-inner-right ts-forms">
                                                                        <div class="input append-big-btn">

                                                <asp:FileUpload ID="FileUpload1" CssClass="form-control"  runat="server"   />
</div> 
                                                                        </div> </div> </div>
              

   
    <div class="login-button-pro" style="text-align:left;" >   <asp:Button ID="btnSend" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>

    
    
   </asp:Content>

