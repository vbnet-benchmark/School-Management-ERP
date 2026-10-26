<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="stockmnage.aspx.vb" Inherits="Account_salary" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>MANAGE STOCK ITEMS</h2>
       
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Stock Items</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="row">
                   <div class="col-lg-12">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group"> 
                                                                <asp:LinkButton ID="lnkBack" class="btn btn-white btn-xs" runat="server">Back</asp:LinkButton>        
     
        </div></div> </div> </div> 
     <br />
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
 <asp:GridView id="Gridview2" runat="server" autogeneratecolumns="False" GridLines="None" class="table">
        <Columns>
            <asp:BoundField DataField="acc" HeaderText="Account Name" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="init" HeaderText="Initial Quantity"></asp:BoundField>
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
    <asp:Button ID="lnkStock" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Stock Item" ></asp:Button></div></div> </div> </div> 
     <br />
     <asp:Panel id="pnlStock" runat="server" visible="false">
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Stock Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox1" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Initial Quantity</label>
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


      

           

    
    
   </asp:Content>




