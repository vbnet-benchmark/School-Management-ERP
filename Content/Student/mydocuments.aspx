<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="mydocuments.aspx.vb" Inherits="Student_MyDocuments" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>MY DOCUMENTS</h2>
    <br />

    <div class="row">
        <div class="col-lg-5">
            <div class="comment-replay-profile">
                <h4>My Uploaded Documents</h4>
                <asp:TextBox ID="txtDocumentId" runat="server" placeholder="Document ID"></asp:TextBox>
                <asp:Button ID="btnViewDocument" runat="server" class="btn btn-white btn-xs" Text="View Document" />
                <asp:Button ID="btnViewDocumentSafe" runat="server" class="btn btn-white btn-xs" Text="View Document (Safe)" />
                <br />
                <asp:Label ID="lblDocumentResult" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>
    <br />

    <div class="row">
        <div class="col-lg-5">
            <div class="comment-replay-profile">
                <h4>My Private Notes</h4>
                <asp:TextBox ID="txtNoteId" runat="server" placeholder="Note ID"></asp:TextBox>
                <asp:Button ID="btnViewNote" runat="server" class="btn btn-white btn-xs" Text="View Note" />
                <asp:Button ID="btnViewNoteSafe" runat="server" class="btn btn-white btn-xs" Text="View Note (Safe)" />
                <br />
                <asp:Label ID="lblNoteResult" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>
    <br />

    <div class="row">
        <div class="col-lg-5">
            <div class="comment-replay-profile">
                <h4>My Export Jobs</h4>
                <asp:TextBox ID="txtExportJobId" runat="server" placeholder="Export Job ID"></asp:TextBox>
                <asp:Button ID="btnViewExportJob" runat="server" class="btn btn-white btn-xs" Text="View Export Job" />
                <asp:Button ID="btnViewExportJobSafe" runat="server" class="btn btn-white btn-xs" Text="View Export Job (Safe)" />
                <br />
                <asp:Label ID="lblExportResult" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>
    <br />

    <div class="row">
        <div class="col-lg-5">
            <div class="comment-replay-profile">
                <h4>View Workspace Record By Type</h4>
                <asp:DropDownList ID="ddlRecordType" runat="server">
                    <asp:ListItem Text="Document" Value="document"></asp:ListItem>
                    <asp:ListItem Text="Note" Value="note"></asp:ListItem>
                    <asp:ListItem Text="Export Job" Value="export"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtRecordId" runat="server" placeholder="Record ID"></asp:TextBox>
                <asp:Button ID="btnViewRecordByType" runat="server" class="btn btn-white btn-xs" Text="View Record" />
                <asp:Button ID="btnViewRecordByTypeSafe" runat="server" class="btn btn-white btn-xs" Text="View Record (Safe)" />
                <br />
                <asp:Label ID="lblRecordResult" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>
    <br />

    <div class="row">
        <div class="col-lg-5">
            <div class="comment-replay-profile">
                <h4>Refresh My Documents (Batch)</h4>
                <asp:TextBox ID="txtBatchIds" runat="server" placeholder="Comma-separated Document IDs"></asp:TextBox>
                <asp:Button ID="btnViewBatch" runat="server" class="btn btn-white btn-xs" Text="Refresh Batch" />
                <asp:Button ID="btnViewBatchSafe" runat="server" class="btn btn-white btn-xs" Text="Refresh Batch (Safe)" />
                <br />
                <asp:Label ID="lblBatchResult" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>

    <div class="login-button-pro" style="text-align:left;">
        <asp:Button ID="btnBack" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Back" />
    </div>
</asp:Content>
