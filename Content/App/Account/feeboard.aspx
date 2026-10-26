<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="feeboard.aspx.vb" Inherits="Account_Default" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
  

 <h2>TERMLY FEE SUMMARY
        
    </h2>
       <asp:Panel ID="pnlAll" runat="server">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                       <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        
                                            
                                        <asp:Label ID="Label2" Font-Bold="true" Font-Size="Larger" runat="server" Text="Students"></asp:Label>
											
                                       
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
        <div class="breadcome-heading">
                                    
                                        </div>
                                    </div>
                                </div>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
             <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList ID="DropDownList1" runat="server" class="form-control custom-select-value" autopostback="true">
                                                                                   
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
                                                        </div>
          
                             
          
     <div class="form-group-inner">
                                                   
                                                       
   
            
    

          
       
            <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">  
 <asp:GridView runat="server" AutoGenerateColumns="False" showheader="False" ID="gridview1"  GridLines="None" class="table" AllowPaging="false">
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
    
   








</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 

                    </div> 
    </asp:Panel>
            

                            
                            
 <asp:Panel runat="server" id ="panel3" visible="false">
  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1><asp:Label ID="lblClass" runat="server" ></asp:Label></h1>
                                       
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
    <asp:Button ID="Button3" class="btn btn-white btn-xs"  runat="server" Text="All Children"></asp:Button></div></div> </div> </div>    
     <br />
     <div class="row"><div class="row" id="dd" style="margin:5px;text-align:center;">
                        <span class="picspan" "  >  <asp:Image  ID="Image1" ImageUrl="~/img/noimage.jpg" CssClass="picsupload"  runat="server" /> </span> <br /> </div> 
                           </div>
 



        <table class="table">
            <tr>
                <td class="auto-style9" >
                     
                    <asp:Label ID="Label7" runat="server"  Text="Total Payed this term" style="font-weight: 700"></asp:Label>
                     
    </td>
                <td class="auto-style8"  >
                    
                   
                
                   
                
                    <asp:Label ID="lblPaid" runat="server" style="text-align: right;"></asp:Label>
                    
                   
                
            </tr>
             <tr>
                <td class="auto-style9" >
                     
                    &nbsp;</td>
                <td class="auto-style8"  >
                    
                   
                
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                   
                
                    </tr>
           
             <tr>
                <td class="auto-style9" >
                     
                    <asp:Label ID="Label3" runat="server"  Text="Unpaid Balance" style="font-weight: 700"></asp:Label>
                     
    </td>
                <td class="auto-style8"  >
                    
                   
                
                    <asp:Label ID="lblOutstanding" runat="server" style="text-align: right;"></asp:Label>
                    
                   
                
            </tr>
           <tr>
                <td class="auto-style9" >
                     
                    &nbsp;</td>
                <td class="auto-style8"  >
                    
                   
                
                    &nbsp;</tr>
           <tr>
                <td class="auto-style9" >
                     
                    <asp:Label ID="Label1" runat="server"  Text="Total Outstanding" style="font-weight: 700"></asp:Label>
                     
    </td>
                <td class="auto-style8"  >
                    
                   
                
                    <asp:Label ID="lblOut" runat="server" style="text-align: right;"></asp:Label>
                    
                   
                
            </tr>
             <tr>
                <td class="auto-style9" >
                     
                    &nbsp;</td>
                <td class="auto-style8"  >
                    
                   
                
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                   
                
                    </tr>
             <tr>
                <td class="auto-style9" >
                     
                    <asp:Label ID="Label4" runat="server"  Text="Payments made in advance" style="font-weight: 700"></asp:Label>
                     
    </td>
                <td class="auto-style8"  >
                    
                   
                
                    <asp:Label ID="lblAdvance" runat="server" style="text-align: right;"></asp:Label>
                    
                   
                
            </tr>
           <tr>
                <td class="auto-style9" >
                     
                    &nbsp;</td>
                <td class="auto-style8"  >
                    
                   
                
                    &nbsp;</tr>
          
           
           
             
           
           <tr>
                <td class="auto-style9" >
                     
                    <asp:Label ID="Label8" runat="server"  Text="Fee Status" style="font-weight: 700"></asp:Label>
                     
                </td>
                <td class="auto-style8"  >
                    
                   
                
                    <asp:Label ID="lblFStatus" runat="server" style="text-align: right;"></asp:Label>
                    
                   
                
                    </tr> <tr>
                <td class="auto-style9" >
                     
                    <div class="row">
                   <div class="col-lg-12">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnPay" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="View Schedule" ></asp:Button><asp:Button ID="btnReceipt" class="btn btn-white btn-xs" TabIndex="8"  runat="server" visible="false" Text="View Receipt" ></asp:Button><asp:Button ID="btnReregister" class="btn btn-white btn-xs" TabIndex="8" visible="false"  runat="server"  Text="Register all students" ></asp:Button><asp:Button ID="BtnReverse" class="btn btn-white btn-xs" TabIndex="8"  runat="server"  Text="Reverse Payment" ></asp:Button></div></div> </div> </div> 
                     
    </td>
                <td class="auto-style8"  >
                    
                   
                
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                   
                
                    </tr>
        </table>
    
     


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

                         
    
 </asp:Panel> 
   
  



       
</asp:Content>
