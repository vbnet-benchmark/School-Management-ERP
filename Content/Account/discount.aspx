<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="discount.aspx.vb" Inherits="Admin_studentprofile" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
  
    <h2>STUDENTS DISCOUNTS</h2>
 <asp:Panel ID="pnlAll" runat="server" DefaultButton="button2">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                       <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        
                                            
                                        <asp:Label ID="Label3" Font-Bold="true" Font-Size="Larger" runat="server" Text="Student List"></asp:Label>
											
                                       
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
        <div class="breadcome-heading">
                                       <span> <asp:Textbox class="form-control" ID="txtSearch"   placeholder="Search..." runat="server" style="width:100%;" ></asp:TextBox>
												<a href="#" id="sb" ><asp:LinkButton TabIndex="0" style="float:right; right:20; top:1; " class="fa fa-search" ID="Button2" runat="server"></asp:LinkButton> </a></span>
                                        </div>
                                    </div>
                                </div>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
            
              <br />
                                              
     <div class="form-group-inner">
                                                            <div class="row">
                                                                
                                                                     
                                                                
                                                                                   


                                                               
                                                            </div>
                                                       
   
            
    

          <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton5" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Student" ></asp:Button></div></div> </div> </div> 
         <br />
            <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">  
 <asp:GridView runat="server" AutoGenerateColumns="False" showheader="False" ID="gridview1"  GridLines="Horizontal" width="50%" class="newtable tableflat" AllowPaging="True">
            <Columns>
                <asp:ImageField DataImageUrlField="passport">
                    <controlstyle BorderStyle="None" Height="80px" Width="80px" />
                </asp:ImageField>
                <asp:BoundField DataField="staffname">
                    <ItemStyle Font-Size="16pt" />
                </asp:BoundField>
                
                
                <asp:CommandField SelectImageUrl="~/image/my-profile.png" SelectText="View" ShowSelectButton="True" ButtonType="Button">
                    <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" />
                </asp:CommandField>
            </Columns>
            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
            <sortedascendingcellstyle backcolor="#F7F7F7" />
            <sortedascendingheaderstyle backcolor="#4B4B4B" />
            <sorteddescendingcellstyle backcolor="#E5E5E5" />
            <sorteddescendingheaderstyle backcolor="#242121" />
    </asp:GridView>
            </div> </div> 
      <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnPrevious" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Previous" />
      <asp:Button ID="btnNext" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Next" /></div>
   

   








</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 

                    </div> 
    </asp:Panel>
     <asp:Panel runat="server" id ="panel3" visible="false" style="margin-top: 0px" >       
           <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1><asp:Label ID="lblName" runat="server"></asp:Label></h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

       <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="Button3" class="btn btn-white btn-xs"  runat="server" Text="All Students"></asp:Button></div></div> </div> </div> 
     <br />
      <div class="row"><div class="row" id="dd" style="margin:5px;text-align:center;width:50%;">
                        <span class="picspan" "  >  <asp:Image  ID="Image1" ImageUrl="~/img/noimage.jpg" CssClass="picsupload"  runat="server" /> </span> <br /> </div> 
                           </div>
     <br />
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
      <asp:GridView id="Gridview2" runat="server" autogeneratecolumns="False" GridLines="None" width="50%" class="newtable">
        <Columns>
<asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" Visible="False"></asp:BoundField>
            <asp:BoundField DataField="fee" HeaderText="Fee" ReadOnly="True"></asp:BoundField>
            <asp:BoundField DataField="type" HeaderText="Type"></asp:BoundField>
            <asp:BoundField DataField="amount" HeaderText="Value"></asp:BoundField>
             <asp:BoundField DataField="frequency" HeaderText="Frequency"></asp:BoundField>
           <asp:CommandField EditText="Change" SelectText="Change" ShowSelectButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:Commandfield>
            <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        </Columns>
    </asp:GridView></div> </div> 
     <br />
       <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton4" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Discount" ></asp:Button></div></div> </div> </div> 
      <asp:Panel  id="panel2" runat="server" visible="false" style="margin-left: 227px">
           <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Fee</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboHostel" runat="server"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
           <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Discount Type</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboTransport" runat="server"  >
                                                                             <asp:ListItem>Select Type</asp:ListItem>   
                                                                            <asp:ListItem>Fixed</asp:ListItem>
                                            <asp:ListItem>Percentage</asp:ListItem>
                                                                           
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

 <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Amount/Percentage/No of Months</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtValue" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

          <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Frequency</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboFreq" runat="server"  >
                                                                                                                                                                                                  <asp:ListItem>Recurring</asp:ListItem>
                                                                           <asp:ListItem>One Time</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnUpdate" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Update" /></div>




          </asp:Panel> 



</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
    
     
       
       
          

            </asp:Panel> 
       
    
<div class="login-button-pro" style="padding:10px;">
    <h4>Process Pending Concession Request</h4>
    <asp:Textbox cssclass="form-control" ID="txtConcessionStudent" runat="server"></asp:Textbox>
    <asp:HiddenField id="hdnConcessionTier" runat="server"></asp:HiddenField>
    <asp:Label ID="lblConcessionTierInfo" runat="server" Text=""></asp:Label>
    <br/>
    <asp:Button ID="btnPreviewConcessionTier" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preview Tier" />
    <asp:Button ID="btnProcessConcession" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Process Concession" />
    <asp:Button ID="btnProcessConcessionSafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Process Concession (Safe)" />
</div>
</asp:Content>
