<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="accsettings.aspx.vb" Inherits="Account_salary" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>ACCOUNT BALANCES</h2>
          <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Cash Accounts</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
      <asp:GridView id="Gridview1" runat="server" autogeneratecolumns="False" GridLines="None" class="newtable" width="50%">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Balance"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
       <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkCash" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Cash Account" ></asp:Button></div></div> </div> </div> 
<br />
     <asp:Panel id="pnlCash" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox9" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Balance</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="Textbox10" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button9" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
              

     </div>  </asp:Panel>
     
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
                                        <h1>Stock Accounts</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
 <asp:GridView id="Gridview2" runat="server" autogeneratecolumns="False" GridLines="None" class="newtable" width="50%">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Balance"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView>
   </div> </div> 
     <br />

     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkStock" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Stock Account" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel id="pnlStock" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox1" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Balance</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="Textbox2" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />


     </div>  </asp:Panel>
     
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
                                        <h1>Receivables Accounts</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div2" style="width:100%;">
 <asp:GridView id="Gridview3" runat="server" autogeneratecolumns="False" GridLines="None" class="newtable" width="50%">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Balance"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView>
   </div> </div> 
     <br />

     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkReceive" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Receivable Account" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel id="pnlreceivables" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox11" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Balance</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="Textbox12" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button11" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />


     </div>  </asp:Panel>
     
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
                                        <h1>Fixed Assets Accounts</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div3" style="width:100%;">
 <asp:GridView id="Gridview4" runat="server" autogeneratecolumns="False" GridLines="None" class="newtable" width="50%">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Balance"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView>
   </div> </div> 
     <br />

     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkFixed" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Fixed Asset Account" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel id="pnlfixed" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox3" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Balance</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="Textbox4" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button3" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />


     </div>  </asp:Panel>
     
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
                                        <h1>Liability Accounts</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div4" style="width:100%;">
 <asp:GridView id="Gridview5" runat="server" autogeneratecolumns="False" GridLines="None" class="newtable" width="50%">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Balance"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView>
   </div> </div> 
     <br />

     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkLiable" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Liability Account" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel id="pnllLiable" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox5" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Balance</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="Textbox6" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button5" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />


     </div>  </asp:Panel>
     
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
                                        <h1>Equity Accounts</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div5" style="width:100%;">
 <asp:GridView id="Gridview6" runat="server" autogeneratecolumns="False" GridLines="None" class="newtable" width="50%">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Balance"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView>
   </div> </div> 
     <br />

     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkEquity" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Equity Account" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel id="pnlEquity" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox7" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Balance</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="Textbox8" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button7" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />


     </div>  </asp:Panel>
     
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
                                        <h1>Expense Accounts</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div6" style="width:100%;">
 <asp:GridView id="Gridview7" runat="server" autogeneratecolumns="False" GridLines="None" class="newtable" width="50%">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Balance"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView>
   </div> </div> 
     <br />

     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkExpense" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Expense Account" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel id="pnlExpense" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox13" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Balance</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="Textbox14" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button13" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />


     </div>  </asp:Panel>
     
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
                                        <h1>Income Accounts</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div7" style="width:100%;">
 <asp:GridView id="Gridview8" runat="server" autogeneratecolumns="False" GridLines="None" class="newtable" width="50%">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Balance"></asp:BoundField>
            <asp:CommandField EditText="Change" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView>
   </div> </div> 
     <br />

     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkIncome" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Income Account" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel id="pnlIncome" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox15" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Balance</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="Textbox16" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button15" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />


     </div>  </asp:Panel>
     
</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 


           

    
    
   </asp:Content>




