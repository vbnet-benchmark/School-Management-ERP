<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="studentbehaviour.aspx.vb" Inherits="Account_Default" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
      <h2>ATTENDANCE DETAILS
        
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
    <asp:Button ID="Button3" class="btn btn-white btn-xs"  runat="server" Text="All Students"></asp:Button></div></div> </div> </div>    
     <br />
     <div class="row"><div class="row" id="dd" style="margin:5px;text-align:center;">
                        <span class="picspan" "  >  <asp:Image  ID="Image1" ImageUrl="~/img/noimage.jpg" CssClass="picsupload"  runat="server" /> </span> <br /> </div> 
                           </div>
    <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Attendance Summary</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                    <table class="table">
            <tr>
                <td><asp:Label ID="Label11" runat="server" style="font-size: small; font-weight: 700" Text="Times Present:"></asp:Label></td>
                <td><asp:label ID="lblPresent" runat="server" ></asp:label></td>
            </tr>
            <tr>
               <td><asp:Label ID="Label21" runat="server" style="font-size: small; font-weight: 700" Text="Times Absent:"></asp:Label></td>
                <td><asp:label ID="lblAbsent" runat="server" ></asp:label></td>
            </tr>
                                         <tr>
               <td><asp:Label ID="Label1" runat="server" style="font-size: small; font-weight: 700" Text="Times Late:"></asp:Label></td>
                <td><asp:label ID="lblLate" runat="server" ></asp:label></td>
            </tr>
                                        
                                         <tr>
               <td><asp:Label ID="Label3" runat="server" style="font-size: small; font-weight: 700" Text="Percentage Lateness:"></asp:Label></td>
                <td><asp:label ID="lblLatePercent" runat="server" ></asp:label></td>
            </tr>
            <tr>
               <td><asp:Label ID="Label27" runat="server" style="font-size: small; font-weight: 700" Text="Times School Opened:"></asp:Label></td>
                <td><asp:label ID="lblOpened" runat="server" ></asp:label></td>
            </tr>
            <tr>
               <td><asp:Label ID="Label28" runat="server" style="font-size: small; font-weight: 700" Text="Attendance Percentage:"></asp:Label></td>
                <td><asp:label ID="lblPercent" runat="server" ></asp:label></td>
            </tr>
        </table>       

      





                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>

                                     </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 




    
    
 </asp:Panel> 
   




       
</asp:Content>
