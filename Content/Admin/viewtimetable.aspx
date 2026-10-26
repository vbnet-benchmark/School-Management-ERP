<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/digidashboard.master" CodeFile="viewtimetable.aspx.vb" Inherits="Default2"  %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">   
    
     <h2>SCHOOL TIME TABLE</h2>


                 <h4>
                     <asp:label runat="server" ID="lblTimetable"></asp:label>
                 </h4>
    <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back" ></asp:Button>     
</div></div> </div> </div> 

     <div class="col-lg-12">
        

         <div class="comment-replay-profile" style="text-align:right;"> <asp:Label ID="lblStatus" runat="server"></asp:Label>
      <asp:LinkButton ID="btnChange" runat="server" class="btn btn-white btn-xs">Change</asp:LinkButton>     </div></div>
    <br />
    <br />
     <div class="form-group-inner">
<div class="row">
                                                                
                                                                <div class="col-lg-12">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboDay" runat="server" autopostback="true"  >
                                                                               <asp:ListItem Value="Select Day"></asp:ListItem>
                         <asp:ListItem Value="Monday"></asp:ListItem>
                         <asp:ListItem Value="Tuesday"></asp:ListItem>
                         <asp:ListItem Value="Wednesday"></asp:ListItem>
                         <asp:ListItem Value="Thursday"></asp:ListItem>
                         <asp:ListItem Value="Friday"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
      
    
     <asp:Panel runat="server" ID="pnltt" visible="false">
     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Time Table</h1>
                                       
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
    <asp:Button ID="btnCS" visible="false" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Cancel Swap" ></asp:Button></div></div> </div> </div> 

      <asp:Panel ID="Panel1" runat="server"></asp:Panel>
     <br />
      <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="Button3" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Show Staff" ></asp:Button></div></div> </div> </div> 


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

         </asp:Panel>


    <asp:Panel ID="pnlAll" runat="server"  visible="false">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                       <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        
                                            
                                        <asp:Label ID="Label1" Font-Bold="true" Font-Size="Larger" runat="server" Text="Staff List"></asp:Label>
											
                                       
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
    <asp:Button ID="Button1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Staff" ></asp:Button></div></div> </div> </div> 
         <br />
            <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">  
 <asp:GridView runat="server" AutoGenerateColumns="False" showheader="False" ID="gridview2"  GridLines="None" class="table" >
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
            
     
     
              
       

    </asp:Content> 