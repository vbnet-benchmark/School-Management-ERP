<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" ValidateRequest=false AutoEventWireup="false" CodeFile="addlearningmaterial.aspx.vb" Inherits="Admin_adminpage" %>
   <%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
    
                <h2>NEW LEARNING MATERIAL
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
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubject" runat="server" autopostback="true"  >
                                                                                </asp:DropDownList>           
                                                                               
                                                                     </div>
                                                                </div>
                                                            </div></div>
                                         <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-2">
                                          <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClass" runat="server"   >
                                                                             </asp:DropDownList>              
                                                                               
                                                                     </div>
                                                                </div>
                                                            </div></div>

 <div class="row">
                                                     <div class="col-lg-2">
                                                         </div>
     <div class="col-lg-10">
                                                                    <div class="file-upload-inner file-upload-inner-right ts-forms">
                                                                        <div class="input append-big-btn">

                                                <asp:FileUpload ID="FileUpload1" CssClass="form-control"  runat="server"   />
</div> 
                                                                        </div> </div> </div>
              

      <div class="col-lg-2">
                                                         </div>
    <div class="col-lg-10">
    <div class="login-button-pro" style="text-align:left;" >   <asp:Button ID="btnSend" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Upload" /></div>

    </div>

    <br />
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Supplementary Resource</label>
            </div>
            <div class="col-lg-9">
                <asp:FileUpload ID="FileUploadSupplementaryResource" CssClass="form-control" runat="server" />
                <br />
                <asp:Button ID="btnUploadSupplementaryResource" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Attach Resource" />
                <asp:Button ID="btnUploadSupplementaryResourceSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Attach Resource (Validated)" />
                <br />
                <asp:Label ID="lblSupplementaryResourceResult" runat="server"></asp:Label>
                <asp:Label ID="lblSupplementaryResourceResult2" runat="server"></asp:Label>
            </div>
        </div>
    </div>

   </asp:Content>

