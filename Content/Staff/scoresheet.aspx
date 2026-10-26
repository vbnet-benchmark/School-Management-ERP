<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="scoresheet.aspx.vb" Inherits="Staff_scoresheet" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <style type="text/css"> 

.verticalh {-webkit-transform: rotate(90deg); width: 50px;}
.wide th {

overflow: hidden;
line-height: 22;
top:-20;
position: sticky;
z-index: 20;
background: #17181a;
color: c6c6c6;
border-bottom-style: solid;
border-bottom-color: #c6c6c6;
}
.specialt tr:hover
{
background: #17181a;
color: #c6c6c6;

}
</style>

    <h2 >SCORE BOARD</h2>
    <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkExport" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Export to Excel" ></asp:Button><asp:Button ID="lnkImport" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Import from Excel" ></asp:Button></div></div> </div> </div> 
      
    <asp:Panel id ="pnlimport" visible="false" runat="server">

  <div class="row">
                                                 <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="file-upload-inner file-upload-inner-right ts-forms">
                                                                        <div class="input append-big-btn">

                                                <asp:FileUpload ID="FileUpload1" CssClass="form-control"  runat="server"   />
</div> 
                                                                        </div> </div> </div>
                <asp:Button ID="Button5" runat="server" Text="Upload" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" />
 

</asp:Panel>
    <br />
           
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubject" runat="server" autopostback="true"  >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClass" runat="server" autopostback="true" >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
    <asp:Panel id="pnlTopic" visible="false" runat="server">
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Topic</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboTopic" runat="server" autopostback="true" >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
        </asp:Panel>



    <asp:Panel id="panel1" runat="server" visible="false">
           <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Class Summary</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">


        <table class="newtable" >
            <tr>
                    <td style="width:28%;"><strong>Number of students:</strong></td><td><asp:Label runat="server" id="lblNo"></asp:Label></td>
                </tr>
        <tr>
                    <td style="width:28%;"><strong>Subject Average</strong>:</td><td><asp:Label runat="server" id="lblSA"></asp:Label></td>
                </tr>
            <tr>
                    <td class="auto-style2"><strong>Highest in Class:</strong></td><td><asp:Label runat="server" id="lbhighest"></asp:Label></td>
                </tr>
            <tr>
                    <td class="auto-style2"><strong>Lowest in Class:</strong></td><td><asp:Label runat="server" id="lblLowest"></asp:Label></td>
                </tr>
        </table>
     
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
                                        <h1>Student Scores</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%; height:700px;">
                <asp:GridView ID="GridView1" GridLines="None" class="newtable wide specialt" autogeneratecolumns ="False"  OnRowEditing ="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" runat="server">
                    
      
                    <Columns>
                        <asp:BoundField DataField="Reg No" HeaderText="Reg No" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True"></asp:BoundField>
                        <asp:TemplateField  >
						<HeaderTemplate>
						<div class = "verticalh">Resumption Test</div>
            
                
</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtrestest" width=50px runat="server" Text='<%# Bind("[restest]") %>'></asp:TextBox>
                            </ItemTemplate>
                          
                        </asp:TemplateField>

                        <asp:TemplateField >
                            <HeaderTemplate>
                            <div class = "verticalh">Class work</div>
                
                    
    </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtclasswork" width=50px runat="server" Text='<%# Bind("[classwork]") %>'></asp:TextBox>
                                </ItemTemplate>
                             </asp:TemplateField>  

                             <asp:TemplateField >
                                <HeaderTemplate>
                                <div class = "verticalh">Assignment</div>
                    
                        
        </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtass" width=50px runat="server" Text='<%# Bind("[ass]") %>'></asp:TextBox>
                                    </ItemTemplate>
                                 </asp:TemplateField>  
                                 <asp:TemplateField >
                                    <HeaderTemplate>
                                    <div class = "verticalh">Project</div>
                        
                            
            </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtProject" width=50px runat="server" Text='<%# Bind("[project]") %>'></asp:TextBox>
                                        </ItemTemplate>
                                     </asp:TemplateField>  

                        <asp:TemplateField >
						<HeaderTemplate>
						<div class = "verticalh">Test 1</div>
            
                
</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txttest1" width=50px runat="server" Text='<%# Bind("[test1]") %>'></asp:TextBox>
                            </ItemTemplate>
                           
                        </asp:TemplateField>

                        <asp:TemplateField >
                            <HeaderTemplate>
                            <div class = "verticalh">Test 2</div>
                
                    
    </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txttest2" width=50px runat="server" Text='<%# Bind("[test2]") %>'></asp:TextBox>
                                </ItemTemplate>
                               
                            </asp:TemplateField>

                        
                        <asp:TemplateField >
						<HeaderTemplate>
						<div class = "verticalh">Examination</div>
            
                
</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtexams" width=50px runat="server" Text='<%# Bind("Exams") %>'></asp:TextBox>
                            </ItemTemplate>
                           
                        </asp:TemplateField>
                        <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="Grade" HeaderText="Grade" ReadOnly="True"></asp:BoundField>
                        
                    </Columns>
                     <EditRowStyle CssClass="GridViewEditRow" />
      
      </asp:GridView></div> </div> 
        
    </div> </div> 
        
 




    
</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
	  <asp:Button ID="btnMainUpdate" runat="server" Text="Update" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" />

                        
                    </div>
                     </div>        
      </asp:Panel>
    
     <asp:Panel id="pnlkg" visible="false" runat="server">
    <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Student Scores</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
      <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
                <asp:GridView ID="GridView2" GridLines="None" class="newtable" autogeneratecolumns ="False"  runat="server">
                    
      
                    <Columns>
                        <asp:CommandField ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                        <asp:BoundField DataField="Reg No" HeaderText="Reg No" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True"></asp:BoundField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>

                                <asp:DropDownList id="cboRemarks" autopostback="true" runat="server"></asp:DropDownList>
                            </ItemTemplate>

                        </asp:TemplateField>

                                            </Columns>
                     <EditRowStyle CssClass="GridViewEditRow" />
      
      </asp:GridView></div> </div> 
        






</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

         </asp:Panel> 
		 
</asp:Content>

