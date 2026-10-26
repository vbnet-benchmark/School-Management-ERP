<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="mngfees.aspx.vb" Inherits="Account_salary" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <h2>MANAGE FEES</h2>
     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Class Specific Fees</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
      <asp:GridView id="Gridview1" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>
<asp:BoundField DataField="id" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="class" HeaderText="Class"></asp:BoundField>
            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            <asp:BoundField DataField="install" HeaderText="Minimum Installment (%)"></asp:BoundField>
            <asp:CommandField SelectText="Change" ShowSelectButton="True">
                <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
            </asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True">
                <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
            </asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkClass" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Fee" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel runat="server" id="pnlClass" visible="false">
          <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="DropdownList1" runat="server"  >
                                                                              
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassFee" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassAmount" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Minimum Installment</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassInstall" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button9" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>







     </asp:Panel>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Class Specific Admission Fees</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div6" style="width:100%;">
      <asp:GridView id="Gridview7" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>
<asp:BoundField DataField="id" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="class" HeaderText="Class"></asp:BoundField>
            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            <asp:BoundField DataField="install" HeaderText="Minimum Installment (%)"></asp:BoundField>
            <asp:CommandField SelectText="Change" ShowSelectButton="True">
                <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
            </asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True">
                <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
            </asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnClassOne" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Fee" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel runat="server" id="Panelclassone" visible="false">
          <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClassone" runat="server"  >
                                                                              
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassOneFee" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassOneAmount" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Minimum Installment</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassOneMin" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnclassOneAdd" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>







     </asp:Panel>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

    
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Class Specific Sessional Fees</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div7" style="width:100%;">
      <asp:GridView id="Gridview8" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>
<asp:BoundField DataField="id" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="class" HeaderText="Class"></asp:BoundField>
            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            <asp:BoundField DataField="install" HeaderText="Minimum Installment (%)"></asp:BoundField>
            <asp:CommandField SelectText="Change" ShowSelectButton="True">
                <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
            </asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True">
                <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
            </asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkClassSession" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Fee" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel runat="server" id="pnlClassSession" visible="false">
          <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClassSession" runat="server"  >
                                                                              
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassSessionfee" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassSessionamount" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Minimum Installment</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtClassSessionMin" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnClassSessionAdd" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>







     </asp:Panel>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>General Mandatory Fees</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
      <asp:GridView id="Gridview2" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>
<asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            <asp:BoundField DataField="install" HeaderText="Minimum Installment (%)"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkMF" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Fee" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel runat="server" id="pnlMF" visible="false">
        
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtMF" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtMA" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Minimum Installment</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtMMI" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button2" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>







     </asp:Panel>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 



     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Sessional Fees</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div2" style="width:100%;">
      <asp:GridView id="Gridview3" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>
<asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            <asp:BoundField DataField="install" HeaderText="Minimum Installment (%)"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkSF" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Fee" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel runat="server" id="pnlSF" visible="false">
        
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtSF" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtSA" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Minimum Installment</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtSMI" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button4" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>







     </asp:Panel>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

                  
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Admission Fees</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div3" style="width:100%;">
      <asp:GridView id="Gridview4" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>
<asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            <asp:BoundField DataField="install" HeaderText="Minimum Installment (%)"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkAF" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Fee" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel runat="server" id="pnlAF" visible="false">
        
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtAF" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtAA" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Minimum Installment</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtAMI" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button6" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>







     </asp:Panel>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Optional Fees</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div4" style="width:100%;">
      <asp:GridView id="Gridview5" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>

            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            <asp:BoundField DataField="install" HeaderText="Minimum Installment (%)"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkOF" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Fee" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel runat="server" id="pnlOF" visible="false">
        
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtOF" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtOA" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Minimum Installment</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtOMI" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>







     </asp:Panel>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
     <asp:Panel id ="panel1" visible="false" runat="server">
     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Transport Fees</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div5" style="width:100%;">
      <asp:GridView id="Gridview6" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>

            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
            <asp:BoundField DataField="install" HeaderText="Minimum Installment (%)"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     




</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
    </asp:panel>
   
  
    
   
</asp:Content>





