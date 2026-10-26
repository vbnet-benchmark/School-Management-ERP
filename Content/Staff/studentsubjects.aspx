<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="studentsubjects.aspx.vb" Inherits="Account_Default" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
   
    <h2>STUDENT'S SUBJECTS
        
    </h2>
   
    <asp:Panel ID="pnlAll" runat="server" DefaultButton="button2">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                       <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        
                                            
                                        <asp:Label ID="Label1" Font-Bold="true" Font-Size="Larger" runat="server" Text="Student List"></asp:Label>
											
                                       
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
        <div class="breadcome-heading">
                                       <span> <asp:Textbox class="form-control" ID="txtSearch"   placeholder="Search..." runat="server" style="width:100%;" ></asp:TextBox>
												<a href="#" id="sb" ><asp:LinkButton TabIndex="0" style="float:right; right:20; top:1; " class="fa fa-search" ID="LinkButton1" runat="server"></asp:LinkButton> </a></span>
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
                                                            <div class="row">
                                                                
                                                                     
                                                                
                                                                                   


                                                               
                                                            </div>
                                                       
    <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkclrpt" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Class Report" ></asp:Button></div></div> </div> </div> 
         <br />

            
    

        
       
            <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">  
 <asp:GridView runat="server" AutoGenerateColumns="False" showheader="False" ID="gridview1"  width = "50%" GridLines="Horizontal" class="newtable tableflat" AllowPaging="false">
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
    
   <br />

         <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkAdd" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Students" ></asp:Button></div></div> </div> </div> 
   
         <asp:Panel runat="server" id="pnlAdd" visible="false">
             <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Admission Number</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="TextBox1" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

             <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /></div>




         </asp:Panel>








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
                                        <h1><asp:Label runat="server" id="lblClass"></asp:Label>'S Subjects</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="Button3" class="btn btn-white btn-xs"  runat="server" Text="All Students"></asp:Button></div></div> </div> </div> 


    <div class="row"><div class="row" id="dd" style="margin:5px;text-align:center;">
                        <span class="picspan" "  >  <asp:Image  ID="Image1" ImageUrl="~/img/noimage.jpg" CssClass="picsupload"  runat="server" /> </span> <br /> </div> 
                           </div>
                                
                               
                                 <br />
                           
          <div class="row">
                        <div class="col-lg-6">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Subjects Offerred</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                  
     <div id ="compulsory" runat ="server"></div>







                                    
      </div>                      </div>
                        
                    </div>
                   
               


                                                 <div class="col-lg-6">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Optional Subjects</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    
<div id ="optional" runat="server"><h4>Optional Subjects</h4>
                               <asp:CheckBoxList ID="CheckBoxList1" runat="server" EnableViewState="true">
                            </asp:CheckBoxList>


    <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button2" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Update" /></div>



  
      </div>                      </div>
                        
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
                
                </asp:panel>
                
                     
 




       
</asp:Content>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        