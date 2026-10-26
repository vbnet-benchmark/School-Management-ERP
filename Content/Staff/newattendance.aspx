<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="newattendance.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        
   <h2>NEW ATTENDANCE</h2>

    <br />
     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1><asp:Label runat="server" id="lblAtt"></asp:Label></h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Term</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" enabled="false" ID="txtTerm" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Date</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" enabled="false" ID="txtDate" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Day</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" enabled="false" ID="txtDay" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Week</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" enabled="false" ID="txtWeek" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" enabled="false" ID="txtClass" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <span class="  pull-right-pro"> <asp:CheckBox ID="chkHoliday"  runat="server" Text="Holiday" AutoPostBack ="true"  OnCheckedChanged="chkHoliday_CheckedChanged" /></span>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" enabled="false" ID="TextBox1" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
         
          
                                                    </div>



     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">

     <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" GridLines="None" class="newtable" >
              <Columns>
                 <asp:boundfield datafield="serial" headertext="S/N" ReadOnly="True"/>
                          <asp:boundfield datafield="admno" headertext="Admission No" ReadOnly="True"/>
         
                   <asp:BoundField DataField="sname" HeaderText="Name" ReadOnly="True" />
                   <asp:TemplateField HeaderText="Morning">
                      
                       <ItemTemplate>
                           <asp:CheckBox ID="chkMorn" runat="server" Checked='<%# Bind("morning") %>'  />
                       </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Afternoon">
                     
                      <ItemTemplate>
                          <asp:CheckBox ID="chkAft" runat="server" Checked='<%# Bind("afternoon") %>' />
                      </ItemTemplate>
                  </asp:TemplateField>
                    <asp:TemplateField HeaderText="Punctual">
                     
                      <ItemTemplate>
                          <asp:CheckBox ID="chkPunc" runat="server" Checked='<%# Bind("punctual")%>' />
                      </ItemTemplate>
                  </asp:TemplateField>
                 </Columns> 
        </asp:GridView></div> </div> 

<div class="login-button-pro" style="text-align:left;" >   <asp:Button ID="btnUpdate" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Update" /></div>
</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 



    
         </asp:Content>




