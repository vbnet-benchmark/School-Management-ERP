<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="customformulas.aspx.vb" Inherits="Admin_customformulas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h1>CUSTOM FORMULAS</h1>

    <div class="row">
        <div class="col-lg-12">
            <div class="sparkline12-list shadow-reset mg-t-30">
                <div class="sparkline12-hd">
                    <div class="main-sparkline12-hd">
                        <h1>Custom Grade Weighting Formula</h1>
                    </div>
                </div>
                <div class="sparkline12-graph">
                    <div class="basic-login-form-ad">
                        <div class="all-form-element-inner">
                            <div class="form-group-inner">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Formula Code</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtGradeFormulaCode" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="login-button-pro" style="text-align:right;">
                                <asp:Button ID="btnRunGradeFormula" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Compute Weighted Grade" />
                                <asp:Button ID="btnRunGradeFormulaSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Compute (Safe)" />
                            </div>
                            <asp:Label ID="lblGradeFormulaResult" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="sparkline12-list shadow-reset mg-t-30">
                <div class="sparkline12-hd">
                    <div class="main-sparkline12-hd">
                        <h1>Custom Fee Surcharge Formula</h1>
                    </div>
                </div>
                <div class="sparkline12-graph">
                    <div class="basic-login-form-ad">
                        <div class="all-form-element-inner">
                            <div class="form-group-inner">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Formula Code</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtFeeSurchargeFormulaCode" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Base Fee</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtFeeSurchargeBaseFee" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="login-button-pro" style="text-align:right;">
                                <asp:Button ID="btnRunFeeSurcharge" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Compute Surcharge" />
                                <asp:Button ID="btnRunFeeSurchargeSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Compute (Safe)" />
                            </div>
                            <asp:Label ID="lblFeeSurchargeResult" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="sparkline12-list shadow-reset mg-t-30">
                <div class="sparkline12-hd">
                    <div class="main-sparkline12-hd">
                        <h1>Custom Report-Column Formula</h1>
                    </div>
                </div>
                <div class="sparkline12-graph">
                    <div class="basic-login-form-ad">
                        <div class="all-form-element-inner">
                            <div class="form-group-inner">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Formula Code</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtReportColumnFormulaCode" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Sample Row Value</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtReportColumnSampleValue" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="login-button-pro" style="text-align:right;">
                                <asp:Button ID="btnRunReportColumn" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Compute Column" />
                                <asp:Button ID="btnRunReportColumnSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Compute (Safe)" />
                            </div>
                            <asp:Label ID="lblReportColumnResult" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="sparkline12-list shadow-reset mg-t-30">
                <div class="sparkline12-hd">
                    <div class="main-sparkline12-hd">
                        <h1>Custom Scholarship / Concession Adjustment Formula</h1>
                    </div>
                </div>
                <div class="sparkline12-graph">
                    <div class="basic-login-form-ad">
                        <div class="all-form-element-inner">
                            <div class="form-group-inner">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Evaluator Mode</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtEvaluatorMode" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Formula Text</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtScholarshipFormulaText" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Base Amount</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtScholarshipBaseAmount" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="login-button-pro" style="text-align:right;">
                                <asp:Button ID="btnRunScholarshipAdjustment" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Compute Adjustment" />
                            </div>
                            <asp:Label ID="lblScholarshipResult" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="sparkline12-list shadow-reset mg-t-30">
                <div class="sparkline12-hd">
                    <div class="main-sparkline12-hd">
                        <h1>Bulk Fee-Category Surcharge Rollout</h1>
                    </div>
                </div>
                <div class="sparkline12-graph">
                    <div class="basic-login-form-ad">
                        <div class="all-form-element-inner">
                            <div class="form-group-inner">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Category Formulas (category=formula;category=formula)</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtBulkCategoryFormulas" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label class="login2 pull-right pull-right-pro">Category Base Fees (category=amount;category=amount)</label>
                                    </div>
                                    <div class="col-lg-9">
                                        <asp:TextBox class="form-control" ID="txtBulkCategoryBaseFees" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="login-button-pro" style="text-align:right;">
                                <asp:Button ID="btnRunBulkSurcharge" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Run Batch" />
                                <asp:Button ID="btnRunBulkSurchargeSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Run Batch (Safe)" />
                            </div>
                            <asp:Label ID="lblBulkSurchargeResult" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
