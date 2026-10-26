<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="adminoptions.aspx.vb" Inherits="Admin_addteacher" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
                <h2>
                    <asp:Label ID="lblHeader" runat="server" ForeColor="Black" Text="PORTAL OPTIONS"></asp:Label>
                </h2>
       <br />
    <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">School Fee Payment</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboFees" runat="server"  >
                                                                              <asp:ListItem Value="Automated" Selected="True">Automated</asp:ListItem>
        <asp:ListItem Value="Manual">Manual</asp:ListItem>                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
       
    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">School Email</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtEmail" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>



     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">SMTP Address</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtSMTP" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Password</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtPassword" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Port</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtPort" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">School Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtSMSname" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">SMS API</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtSMS" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>     
     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtAcc" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>              
     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Public Key</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtPub" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Secret Key</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtSec" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro"><asp:CheckBox ID="ChkBoard" runat="server" text="Boarding"  />
</label>
                                                                </div>
                                                               
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro"><asp:CheckBox ID="chkTrans" runat="server"  text ="Transport"  />
</label>
                                                                </div>
                                                               
                                                            </div>
                                                        </div>
     <div class="row">
          <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Upload Signature</label>
                                                                </div>
                                                 <div class="col-lg-9 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="file-upload-inner file-upload-inner-right ts-forms">
                                                                        <div class="input append-big-btn">

                                                <asp:FileUpload ID="FileUpload1" CssClass="form-control"  runat="server"   />
</div> 
                                                                        </div> </div> </div>
    <br />
     <div class="row" id="dd" style="margin:5px;">
         <div class="col-lg-3">

         </div>
               <div class="col-lg-9">

                                      <span   >
                    <asp:Image ID="Image1" runat="server" height="100px" width="150px"  />
                    </span></div>
          </div>   
    <asp:Label runat="server" id="lblsign" visible="false"></asp:Label>
    <div class="row">
          <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Upload School Logo</label>
                                                                </div>
                                                 <div class="col-lg-9 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="file-upload-inner file-upload-inner-right ts-forms">
                                                                        <div class="input append-big-btn">

                                                <asp:FileUpload ID="FileUpload2" CssClass="form-control"  runat="server"   />
</div> 
                                                                        </div> </div> </div>

    <div class="row" id="Div1" style="margin:5px;">
         <div class="col-lg-3">

         </div>
               <div class="col-lg-9">

                                      <span   >
                    <asp:Image ID="Image2" runat="server" height="90px" width="370px"  />
                    </span></div>
          </div>   
    <asp:Label runat="server" id="lblLogo" visible="false"></asp:Label>
           <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button2" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Update" />  <asp:Button ID="btnWelcome" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Staff Welcome Message" /><asp:Button ID="btnPWelcome" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Parent Welcome Message" /></div>

    <br />
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Report Templates</label>
            </div>
            <div class="col-lg-9">
                <asp:DropDownList cssclass="form-control custom-select-value" ID="cboReportTemplateKind" runat="server">
                    <asp:ListItem Value="Transcript">Transcript</asp:ListItem>
                    <asp:ListItem Value="Receipt">Receipt</asp:ListItem>
                </asp:DropDownList>
                <asp:FileUpload ID="FileUploadReportTemplate" CssClass="form-control" runat="server" />
                <asp:Button ID="btnImportReportTemplate" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Import Template" />
                <br />
                <asp:DropDownList cssclass="form-control custom-select-value" ID="cboReportTemplateKindSafe" runat="server">
                    <asp:ListItem Value="Transcript">Transcript</asp:ListItem>
                    <asp:ListItem Value="Receipt">Receipt</asp:ListItem>
                </asp:DropDownList>
                <asp:FileUpload ID="FileUploadReportTemplateSafe" CssClass="form-control" runat="server" />
                <asp:Button ID="btnImportReportTemplateSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Import Template (Validated)" />
            </div>
        </div>
    </div>
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">System Diagnostics</label>
            </div>
        </div>
    </div>
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Database Connection</label>
            </div>
            <div class="col-lg-9">
                <asp:Button ID="btnTestDbConnection" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Test Database Connection" />
                <asp:Button ID="btnVerifyDbConfig" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Verify Database Configuration" />
                <br />
                <asp:Label ID="lblDbTestResult" runat="server"></asp:Label>
                <asp:Label ID="lblDbTestResult2" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">SMTP Connection</label>
            </div>
            <div class="col-lg-9">
                <asp:Button ID="btnTestSmtp" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Test SMTP Connection" />
                <asp:Button ID="btnTestSmtpSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Verify SMTP Settings" />
                <br />
                <asp:Label ID="lblSmtpResult" runat="server"></asp:Label>
                <asp:Label ID="lblSmtpResult2" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Backup Database</label>
            </div>
            <div class="col-lg-9">
                <asp:Button ID="btnTestBackupDb" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Test Backup Database Connection" />
                <asp:Button ID="btnTestBackupDbSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Verify Backup Database" />
                <br />
                <asp:Label ID="lblBackupResult" runat="server"></asp:Label>
                <asp:Label ID="lblBackupResult2" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Diagnostics Report</label>
            </div>
            <div class="col-lg-9">
                <asp:Button ID="btnExportReport" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Export Diagnostics Report" />
                <br />
                <asp:Label ID="lblExportResult" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">System Health Check</label>
            </div>
            <div class="col-lg-9">
                <asp:Button ID="btnRunHealthCheck" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Run System Health Check" />
                <br />
                <asp:Label ID="lblHealthCheckResult" runat="server"></asp:Label>
            </div>
        </div>
    </div>

   </asp:Content>

