<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="newtimetable.aspx.vb" Inherits="Admin_addclass" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
   <h2><asp:Label runat="server" Text="NEW TIME TABLE"></asp:Label></h2>
    <br />
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0"  style="font-size: small" DisplaySideBar="False" Width="100%" startNextButtonCss="btn-small btn-skin btn-block btn-lg" FinishCompleteButtonText="Generate">
        <NavigationButtonStyle CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew"  />
        <SideBarButtonStyle Font-Size="14px" />
        <WizardSteps>
           <asp:WizardStep ID="WizardStep1" runat="server" Title="Profile">
               
               <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Time Table Details</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Time Table Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtID" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    <div class="row">
                                                                <div class="col-lg-3">

                                                                     </div>
                                                                <div class="col-lg-9">
                                                                    <h4>Generation Method</h4>
                                                                     <asp:RadioButtonList id="radType" runat="server" autopostback="true">
     <asp:ListItem Value="Automated"></asp:ListItem>
        <asp:ListItem Value="Manual"></asp:ListItem>

</asp:RadioButtonList>   
                                                                    </div> 
                                                        </div> 
                                                    <br />

                                                                  <asp:Panel ID="pnlSchool" runat="server"  Visible="False">
<div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">School</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboDept" runat="server"  >
                                                                              
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
                                                   </asp:Panel> 
                                                        <asp:Panel ID="pnlClass" runat="server"  Visible="False">
                                                             <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList ID="DropDownList1" runat="server" cssclass="form-control custom-select-value" autopostback="true">
                                                                                 
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
                                                        </div>
            <asp:Panel ID="pnlManual" runat="server"  Visible="False">
                  <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                     <label class="login2 pull-right pull-right-pro">
                                                                     <asp:CheckBox ID = "chkCopy" runat="server" Checked="False"  Text="Copy Time Table From Class" AutoPostBack="true"   /></label> 
                                                                </div>
                                                                 
                                                                <div class="col-lg-9">
                                                                   <div class="form-select-list">
                                                                       <asp:DropDownList enabled="false" cssclass = "form-control custom-select-value" ID="cboCopyClass" runat="server"  autopostback="true" >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
                                                        </div>


                <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Day</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboDay3" runat="server"  autopostback="true" >
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

     <br />
                <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Start Time (24 hour format)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtStartManual" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">End Time (24 hour format)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtEndManual" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                     <label class="login2 pull-right pull-right-pro">
                                                                     <asp:CheckBox ID = "ChkManualTutorial" runat="server" Checked="True"  Text="Tutorial" AutoPostBack="true"   /></label> 
                                                                </div>
                                                                 
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtManualTutorial" runat="server" enabled="false" text="Tutorial"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                 <asp:Panel ID="pnlManualDetails" runat="server" >

                     <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboManualSubjects" runat="server"  >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>


                     </asp:Panel> 
 <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="BtnManualAdd" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" /><asp:Button ID="btnManualremove" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Remove Period" />  </div>
              <br />
     <asp:Panel ID="pnlManualRemove" visible="false" runat="server" >
          <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Period</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboManualRemove" runat="server"  >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>

        <div class="login-button-pro" style="text-align:right;" >  <asp:Button ID="btnRemovePeriod" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Remove Period" /></div> 
         <br />
         </asp:Panel> 
            
                                                  <asp:GridView ID="GridView6" AutoGenerateColumns ="False"   runat="server" GridLines="vertical" class="table">
             
          </asp:GridView>
                </asp:Panel> 
                                                           
                                                                                                                        </asp:Panel> 
                                                   </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 
               
                       
                
    
           </asp:WizardStep>

            <asp:WizardStep ID="sgroupsname" runat="server" Title="Group Subjects">
                 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Group Subjects</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
      <asp:GridView  runat="server" autogeneratecolumns ="false" ID="Gridview2" ShowHeader="false" GridLines="None" class="table" >
                     <Columns>
                        <asp:BoundField DataField="name" HeaderText="Period"></asp:BoundField>
                         <asp:CommandField SelectText="View" ShowSelectButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>                    

                         <asp:CommandField ShowDeleteButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>

                     </Columns>

                    
                </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Group" ></asp:Button></div></div> </div> </div> 

     <br />
     <asp:Panel ID="Panel2" runat="server" Visible="false">

         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Group Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtGrp" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <br />
         <h4>Subjects</h4>     
                        <asp:CheckBoxList ID="CheckBoxList2" runat="server" class="form-control-small" EnableViewState="true" >
                            </asp:CheckBoxList>
                        
                        <h4>Classes affected</h4>
                        <asp:CheckBoxList ID="chkClasses" runat="server" class="form-control-small" EnableViewState="true" >
                            </asp:CheckBoxList>

         <br />
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
               <asp:Button ID="Button2" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Cancel" />

              </div>


         </asp:Panel> 




</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 



            </asp:WizardStep>
           <asp:WizardStep ID="WizardStep4" runat="server" Title="Group Classes">
                 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Group Classes</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
     <asp:GridView  runat="server" autogeneratecolumns ="false" ID="Gridview3" ShowHeader="false" GridLines="None" class="table"  >
                     <Columns>
                        <asp:BoundField DataField="name" HeaderText="Period"></asp:BoundField>
                         <asp:CommandField SelectText="View" ShowSelectButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>                    

                         <asp:CommandField ShowDeleteButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>

                     </Columns>

                    

                </asp:GridView></div> </div> 
     <br />
       <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton2" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Group" ></asp:Button></div></div> </div> </div> 

     <br />
     <asp:Panel ID="Panel3" runat="server" Visible="false">

         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class Group Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtCGrp" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <br />
          <h4>Classes</h4>     
                        <asp:CheckBoxList ID="chkCGroup" runat="server" class="form-control-small" EnableViewState="true" CssClass="accordion" Font-Size="Small" AutoPostBack="true" >
                            </asp:CheckBoxList>
                        
                        <h4>Subjects offerred together</h4>
                        <asp:CheckBoxList ID="chkCSSgroup" runat="server" class="form-control-small" EnableViewState="true" >
                            </asp:CheckBoxList>
                        <asp:CheckBoxList ID="chkCSgroup" runat="server" class="form-control-small" EnableViewState="true">
                            </asp:CheckBoxList>

         <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button3" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
               <asp:Button ID="Button4" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Cancel" />

              </div>

         </asp:Panel> 
</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

                
              
            </asp:WizardStep>

            <asp:WizardStep ID="periods" runat="server" Title="Period allocate">
                 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Periods</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Day</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboDay" runat="server"  autopostback="true" >
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

     <br />
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div2" style="width:100%;">
     <asp:GridView  runat="server" autogeneratecolumns ="false" ID="Gridview1" GridLines="None" class="table" >
                     <Columns>
                        <asp:BoundField DataField="period" HeaderText="Period"></asp:BoundField>
                        <asp:BoundField DataField="time" HeaderText="Time Range" ReadOnly="True"></asp:BoundField>
                         <asp:BoundField DataField="activity" HeaderText="Activity"></asp:BoundField>
                         <asp:CommandField SelectText="Change" ShowSelectButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>                    

                         <asp:CommandField ShowDeleteButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>

                     </Columns>

                </asp:GridView></div> </div> 


     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkNew" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Period" ></asp:Button></div></div> </div> </div> 


     <asp:Panel id="panel1" runat="server" visible="false">

         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Start Time (24 hour format)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtStart" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">End Time (24 hour format)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtEnd" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                     <label class="login2 pull-right pull-right-pro">
                                                                     <asp:CheckBox ID = "chkTutorial" runat="server" Checked="True"  Text="Tutorial" AutoPostBack="true"   /></label> 
                                                                </div>
                                                                 
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtActivity" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                     <label class="login2 pull-right pull-right-pro">
                                                                     <asp:CheckBox ID = "chkClassPeriods" runat="server" visible="false" Checked="False"  Text="Class Specific Activity" AutoPostBack="true"   /></label> 
                                                                </div>
                                                                 
                                                              
                                                            </div>
               <asp:Panel ID="pnlClassActivity" runat="server"  Visible="False">
<div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:CheckBoxList ID="Chkclasss" runat="server"  EnableViewState="true" >
                            </asp:CheckBoxList>
                                                                </div>
                                                            </div></div>
                                                   </asp:Panel> 


                                                        </div>
         <br />
          <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="bnAdd" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
              

              </div>


         </asp:Panel> 


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

                 
            </asp:WizardStep>
           
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Upload passport">
                  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Double Periods</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div3" style="width:100%;">
     <asp:GridView  runat="server" autogeneratecolumns ="false" ID="Gridview4" GridLines="None" class="table" >
                     <Columns>
                        <asp:BoundField DataField="subject" HeaderText="Subject"></asp:BoundField>
                        <asp:BoundField DataField="class" HeaderText="Class" ReadOnly="True"></asp:BoundField>
                         <asp:BoundField DataField="amount" HeaderText="Amount"></asp:BoundField>
  <asp:CommandField SelectText="View" ShowSelectButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " />
                         </asp:CommandField>   
                         <asp:CommandField ShowDeleteButton="True"> <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>                    

                     </Columns>

                </asp:GridView></div> </div> 
     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton3" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Double Period" ></asp:Button></div></div> </div> </div> 
     <br />
      <asp:Panel id="panel4" runat="server" visible="false">
           <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubject" runat="server" autopostback="true" >
                                                                               
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
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClasses" runat="server"  >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
<div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">No of Double Periods</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtDouble" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
          <br />
          
         <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button5" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
               <asp:Button ID="Button6" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Cancel" />

              </div>





          </asp:Panel> 


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 


            </asp:WizardStep>


            <asp:WizardStep ID="WizardStep3" runat="server" Title="optional">
                  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Morning Subjects</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
   <div class="sparkline11-graph dashone-comment dashtwo-comment" style="overflow:auto; height:200px;" id="mydiv"  >
                                    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div5" style="width:100%;">
                                   <asp:GridView GridLines="None" autogeneratecolumns="false" class="table"  showheader="False"  id="gridMorning" runat="server">
                                       <Columns>
                                           <asp:BoundField DataField="name"></asp:BoundField>
                                           <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                                       </Columns>
                                    </asp:GridView>
                                    </div> 
                                        </div> 

                                </div>
                                
                            </div>
                                                <br />
                                                <br />
     <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Add Morning Subjects (6 Max)</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubject2" runat="server"  >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
     <br />
      <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button7" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
               <asp:Button ID="Button8" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Remove" />

              </div>




</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
                
            </asp:WizardStep>

            <asp:WizardStep runat="server" Title="Exemption">
                  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Teachers Exemption</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div4" style="width:100%;">
      <asp:GridView  runat="server" autogeneratecolumns ="false" ID="Gridview5" GridLines="None" class="table">
                     <Columns>
                        <asp:BoundField DataField="teacher" HeaderText="Teacher"></asp:BoundField>
                        <asp:BoundField DataField="day" HeaderText="Day" ReadOnly="True"></asp:BoundField>
                         <asp:CommandField ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>                    

                     </Columns>

                </asp:GridView></div> </div> 
     <br />
      <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton4" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Exemption" ></asp:Button></div></div> </div> </div> 
     <br />
      <asp:Panel id="panel5" runat="server" visible="false">

           <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Teacher</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboTeacher" runat="server"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
           <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Day</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboDay2" runat="server"  >
                                                                                <asp:ListItem>Select Day</asp:ListItem>
                                                                                  <asp:ListItem>Monday</asp:ListItem>
                                                                                  <asp:ListItem>Tuesday</asp:ListItem>
                                                                                  <asp:ListItem>Wednesday</asp:ListItem>
                                                                                  <asp:ListItem>Thursday</asp:ListItem>
                                                                                  <asp:ListItem>Friday</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
          <br />
            <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button9" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
               <asp:Button ID="Button10" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Cancel" />

              </div>




          </asp:Panel> 




</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 



            </asp:WizardStep>

           </WizardSteps>
    </asp:Wizard>
    
    
   </asp:Content>










