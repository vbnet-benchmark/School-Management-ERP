<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" ValidateRequest=false AutoEventWireup="false" CodeFile="downloadresource.aspx.vb" Inherits="Staff_DownloadResource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h2>RESOURCE PREVIEW</h2>
    <br />

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Learning Material File Name</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtMaterialName" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewMaterial" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview" />
                <br />
                <asp:Label ID="lblMaterialPreview" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Learning Material File Name (Validated)</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtMaterialNameSafe" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewMaterialSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview (Validated)" />
                <br />
                <asp:Label ID="lblMaterialPreviewSafe" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Supplementary Answer File Name</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtSupplementaryAnswer" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewSupplementaryAnswer" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview" />
                <br />
                <asp:Label ID="lblSupplementaryAnswerPreview" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Supplementary Answer File Name (Validated)</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtSupplementaryAnswerSafe" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewSupplementaryAnswerSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview (Validated)" />
                <br />
                <asp:Label ID="lblSupplementaryAnswerPreviewSafe" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Learning Material File Name (via Shared Service)</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtServiceMaterial" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewViaService" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview" />
                <br />
                <asp:Label ID="lblServicePreview" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Learning Material File Name (via Shared Service, Validated)</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtServiceMaterialSafe" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewViaServiceSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview (Validated)" />
                <br />
                <asp:Label ID="lblServicePreviewSafe" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Resolver Mode (teacher / student)</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtResolverMode" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Resource File Name</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtPolyResource" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewPolyResource" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview" />
                <br />
                <asp:Label ID="lblPolyPreview" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Archived Material Path Segments (comma-separated)</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtArchiveSegments" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewArchive" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview" />
                <br />
                <asp:Label ID="lblArchivePreview" runat="server"></asp:Label>
            </div>
        </div>
    </div>

    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Archived Material Path Segments (comma-separated, Validated)</label>
            </div>
            <div class="col-lg-9">
                <asp:TextBox CssClass="form-control" ID="txtArchiveSegmentsSafe" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnPreviewArchiveSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview (Validated)" />
                <br />
                <asp:Label ID="lblArchivePreviewSafe" runat="server"></asp:Label>
            </div>
        </div>
    </div>

</asp:Content>
