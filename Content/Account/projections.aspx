<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="projections.aspx.vb" Inherits="Account_salary" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <h2>FINANCIAL PROJECTIONS</h2>

    
    <div class="row">
                        <div class="col-lg-6">
                            <div class="sparkline12-list shadow-reset mg-t-30" >
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Financial Variables</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">No of Terms</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtTerms" runat="server" autopostback="true"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Salary Months Per Term</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtSalary" runat="server" autopostback="true"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                       </div> 
                               
                                <div class="panel-group adminpro-custon-design" id="accordion2">
                                    <div class="panel panel-default">
                                        <div class="panel-heading accordion-head">
                                            <h1 class="panel-title">
                                 <a data-toggle="collapse" data-parent="#accordion2" href="#collapse4">
                                 School Statistics</a>
                              </h1>
                                        </div>
                                        <div id="collapse4"   class="panel-collapse panel-ic collapse in">
                                            <div class="panel-body admin-panel-content animated flash">
                                               
                                                <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div6" style="width:100%;">
                                                                <asp:GridView id="gridSchool" autogeneratecolumns="false" GridLines="None" class="table" runat="server">
                                                                     <Columns>

            <asp:BoundField DataField="PARAMETER" readonly="true" HeaderText="PARAMETER"></asp:BoundField>
            <asp:BoundField DataField="NUMBER" HeaderText="NUMBER"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>

<EditRowStyle CssClass="GridViewEditRow" />
                                                                </asp:GridView>
                                                                </div> 
                                                    </div> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading accordion-head">
                                            <h1 class="panel-title">
                                 <a data-toggle="collapse" data-parent="#accordion2" href="#collapse5">
                                 Class Specific Fees</a>
                              </h1>
                                        </div>
                                        <div id="collapse5" class="panel-collapse panel-ic collapse in">
                                            <div class="panel-body admin-panel-content animated flash">
                                               <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div7" style="width:100%;">
                                                                <asp:GridView id="gridClassfees" autogeneratecolumns="false" GridLines="None" class="table" runat="server">
        <Columns>

            <asp:BoundField DataField="class" readonly="true" HeaderText="Class"></asp:BoundField>
            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            
            <asp:CommandField EditText="Change" ShowEditButton="True">
                <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
            </asp:CommandField>
           
        </Columns>
<EditRowStyle CssClass="GridViewEditRow" />



                                                                </asp:GridView>
                                                                
                                                            </div> 
                                                    </div> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading accordion-head">
                                            <h1 class="panel-title">
                                 <a data-toggle="collapse" data-parent="#accordion2" href="#Div9">
                                Other Fees</a>
                              </h1>
                                        </div>
                                        <div id="Div9" class="panel-collapse panel-ic collapse in">
                                            <div class="panel-body admin-panel-content animated flash">
                                               <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div10" style="width:100%;">
                                                                <asp:GridView id="gridOtherfees" Autogeneratecolumns="false" GridLines="None" class="table" runat="server">
                                                                    <Columns>

            <asp:BoundField DataField="FEE" readonly="true" HeaderText="FEE"></asp:BoundField>
            <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>

<EditRowStyle CssClass="GridViewEditRow" />


                                                                </asp:GridView>
                                                                </div> 
                                                    </div> 
                                            </div>
                                        </div>
                                    </div>
                                     <div class="panel panel-default">
                                        <div class="panel-heading accordion-head">
                                            <h1 class="panel-title">
                                 <a data-toggle="collapse" data-parent="#accordion2" href="#Div11">
                                Stock</a>
                              </h1>
                                        </div>
                                        <div id="Div11" class="panel-collapse panel-ic collapse in">
                                            <div class="panel-body admin-panel-content animated flash">
                                               <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div12" style="width:100%;">
                                                                <asp:GridView id="gridStock" Autogeneratecolumns="false" GridLines="None" class="table" runat="server">
                                                                     <Columns>

            <asp:BoundField DataField="ACCOUNT" readonly="true" HeaderText="ACCOUNT"></asp:BoundField>
            <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>

<EditRowStyle CssClass="GridViewEditRow" />


                                                                </asp:GridView>
                                                                </div> 
                                                    </div> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel panel-default">
                                        <div class="panel-heading accordion-head">
                                            <h1 class="panel-title">
                                 <a data-toggle="collapse" data-parent="#accordion2" href="#collapse6">
                                 Expenses</a>
                              </h1>
                                        </div>
                                        <div id="collapse6" class="panel-collapse panel-ic collapse in">
                                            <div class="panel-body admin-panel-content animated flash">
                                               <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div8" style="width:100%;">
                                                                <asp:GridView id="gidExpenses" Autogeneratecolumns="false" GridLines="None" class="table" runat="server">
                                                                     <Columns>

            <asp:BoundField DataField="EXPENSE" readonly="true" HeaderText="EXPENSE"></asp:BoundField>
            <asp:BoundField DataField="AMOUNT" HeaderText="AMOUNT"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>

<EditRowStyle CssClass="GridViewEditRow" />




                                                                </asp:GridView>
                                                                </div> 
                                                    </div> 
                                                </div> 
                                            </div> 
                                        </div> 
                                    
                                            </div>
                                         </div>
                                            </div> 
                                    </div>
                                </div>
                            </div>
                        </div>
                         <div class="col-lg-6">
                            <div class="sparkline12-list shadow-reset mg-t-30" style="height:50%; position:fixed; right:20px; top:150px; width:40%;" >
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Income Projection</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
  <asp:GridView GridLines="None" id="gridIncome" showheader="false" class="table" runat="server"></asp:GridView>

                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                    </div>
     
  
    
   
</asp:Content>





