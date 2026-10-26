<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="externalintegrations.aspx.vb" Inherits="Admin_externalintegrations" %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

                 <h2>EXTERNAL INTEGRATIONS</h2>

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

     <h4>Webhook Test Console</h4>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Webhook URL</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtWebhookUrl" placeholder="https://example.com/hooks/school-events" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <asp:Button ID="btnTestWebhookDirect" runat="server" Text="Send Test Ping" CssClass="btn btn-primary" OnClick="btnTestWebhookDirect_Click" />
                                                            <asp:Button ID="btnTestWebhookDirectSafe" runat="server" Text="Send Test Ping (Safe)" CssClass="btn btn-default" OnClick="btnTestWebhookDirectSafe_Click" />
                                                        </div>

     <h4>Fee Payment Webhook</h4>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee Webhook URL</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtFeeWebhookUrl" placeholder="https://example.com/hooks/fee-payments" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <asp:Button ID="btnTestFeeWebhook" runat="server" Text="Test Fee Webhook" CssClass="btn btn-primary" OnClick="btnTestFeeWebhook_Click" />
                                                            <asp:Button ID="btnTestFeeWebhookSafe" runat="server" Text="Test Fee Webhook (Safe)" CssClass="btn btn-default" OnClick="btnTestFeeWebhookSafe_Click" />
                                                        </div>

     <h4>Attendance Alert Webhook</h4>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Attendance Webhook URL</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtAttendanceWebhookUrl" placeholder="https://example.com/hooks/attendance-alerts" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <asp:Button ID="btnSendAttendanceAlert" runat="server" Text="Send Test Attendance Alert" CssClass="btn btn-primary" OnClick="btnSendAttendanceAlert_Click" />
                                                            <asp:Button ID="btnSendAttendanceAlertSafe" runat="server" Text="Send Test Attendance Alert (Safe)" CssClass="btn btn-default" OnClick="btnSendAttendanceAlertSafe_Click" />
                                                        </div>

     <h4>Integration Channel Dispatch</h4>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Channel Type</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtChannelType" placeholder="webhook or log" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Destination</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtChannelDestination" placeholder="https://example.com/hooks/custom" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Message</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtChannelMessage" placeholder="Test message" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <asp:Button ID="btnDispatchChannel" runat="server" Text="Dispatch" CssClass="btn btn-primary" OnClick="btnDispatchChannel_Click" />
                                                            <asp:Button ID="btnDispatchChannelSafe" runat="server" Text="Dispatch (Safe)" CssClass="btn btn-default" OnClick="btnDispatchChannelSafe_Click" />
                                                        </div>

     <h4>Partner Resource Sync</h4>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Partner Host</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtPartnerHost" placeholder="https://partner-lms.example.com" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Resource Path</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtResourcePath" placeholder="curriculum/term3/syllabus.pdf" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <asp:Button ID="btnSyncResource" runat="server" Text="Sync Now" CssClass="btn btn-primary" OnClick="btnSyncResource_Click" />
                                                            <asp:Button ID="btnSyncResourceSafe" runat="server" Text="Sync Now (Safe)" CssClass="btn btn-default" OnClick="btnSyncResourceSafe_Click" />
                                                        </div>

     <h4>Partner Certificate Trust &#8212; API Connectivity Check</h4>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Partner API URL</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtPartnerCertApiUrl" placeholder="https://partner-examboard.example.com/api/status" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <asp:Button ID="btnCheckPartnerApi" runat="server" Text="Check Connectivity" CssClass="btn btn-primary" OnClick="btnCheckPartnerApi_Click" />
                                                            <asp:Button ID="btnCheckPartnerApiSafe" runat="server" Text="Check Connectivity (Safe)" CssClass="btn btn-default" OnClick="btnCheckPartnerApiSafe_Click" />
                                                        </div>

     <h4>Partner Certificate Trust &#8212; Trust Policy by Partner Code</h4>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Partner Code</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtPartnerCode" placeholder="standard or legacy partner code" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="form-group-inner">
     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Partner API URL</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtPartnerTrustApiUrl" placeholder="https://partner-examboard.example.com/api/status" runat="server"></asp:Textbox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <asp:Button ID="btnCheckPartnerCertTrust" runat="server" Text="Check Trust Policy" CssClass="btn btn-primary" OnClick="btnCheckPartnerCertTrust_Click" />
                                                        </div>

      <div class="form-group-inner">
                                                            <asp:Literal ID="litResult" runat="server"></asp:Literal>
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
