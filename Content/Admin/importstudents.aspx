<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="importstudents.aspx.vb" Inherits="Admin_importstudents" %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

                 <h2>IMPORT STUDENT RECORDS</h2>

  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1><asp:Label ID="lblhead" runat="server" ></asp:Label></h1>
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
                                                                    <label class="login2 pull-right pull-right-pro">Student Records (XML file)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:FileUpload ID="FileUploadXml" CssClass="form-control" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>

      <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Compatibility mode (helper)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:CheckBox ID="chkCompatibilityMode" runat="server" Text="Allow legacy DOCTYPE-bearing files" />
                                                                </div>
                                                            </div>
                                                        </div>

      <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Legacy mode (service)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:CheckBox ID="chkServiceLegacyMode" runat="server" Text="Allow legacy DOCTYPE-bearing files" />
                                                                </div>
                                                            </div>
                                                        </div>

      <div class="form-group-inner">
                                                            <asp:Button ID="btnImportLegacy" runat="server" Text="Import Now (Legacy-Compatible)" CssClass="btn btn-warning" OnClick="btnImportLegacy_Click" />
                                                            <asp:Button ID="btnImportModern" runat="server" Text="Import Now (Modern/Strict)" CssClass="btn btn-primary" OnClick="btnImportModern_Click" />
                                                            <asp:Button ID="btnImportViaHelper" runat="server" Text="Import via Helper" CssClass="btn btn-primary" OnClick="btnImportViaHelper_Click" />
                                                        </div>

      <div class="form-group-inner">
                                                            <asp:Button ID="btnUploadPreview" runat="server" Text="Upload &amp; Preview" CssClass="btn btn-default" OnClick="btnUploadPreview_Click" />
                                                            <asp:Button ID="btnConfirmImport" runat="server" Text="Confirm Import" CssClass="btn btn-warning" OnClick="btnConfirmImport_Click" />
                                                            <asp:Button ID="btnValidateOnly" runat="server" Text="Validate Only (Strict)" CssClass="btn btn-primary" OnClick="btnValidateOnly_Click" />
                                                        </div>

      <div class="form-group-inner">
                                                            <asp:Button ID="btnServiceImport" runat="server" Text="Bulk Import via Service" CssClass="btn btn-primary" OnClick="btnServiceImport_Click" />
                                                            <asp:Button ID="btnSmartImport" runat="server" Text="Smart Import (Auto-Detect Format)" CssClass="btn btn-primary" OnClick="btnSmartImport_Click" />
                                                        </div>

      <div class="form-group-inner">
                                                            <asp:Literal ID="litPreview" runat="server"></asp:Literal>
                                                        </div>
 </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
</asp:Content>
