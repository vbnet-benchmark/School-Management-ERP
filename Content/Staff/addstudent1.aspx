<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="addstudent1.aspx.vb" Inherits="Admin_adminpage" %>
<%@ Register Assembly="SlimeeLibrary" Namespace="SlimeeLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
                <h2>NEW STUDENT</h2>

           
                <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" OnNextButtonClick="Wizard1_NextButtonClick" style="font-size: small" DisplaySideBar="False" Width="100%" >
                    <NavigationButtonStyle CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" Font-Size="14px" Width="90px" />
        <WizardSteps>
            <asp:WizardStep ID="WizardStep1" runat="server" Title="Profile">
 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Student Profile</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Admission Number</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                   <asp:Textbox cssclass="form-control" ID="txtID" runat="server"  ></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtid" runat="server" ErrorMessage="Please enter a valid admisssion number" ForeColor="#FF3300"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Full Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtSurname" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Sex</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cboSex" runat="server"  >
                                                                                <asp:ListItem Value="Select Gender"></asp:ListItem>
                                                                                <asp:ListItem Value="Male"></asp:ListItem>
                                                                                <asp:ListItem Value="Female"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                       
                                                       <div class="form-group-inner" id="data_2">
                                                            <div class="row">
                                                                <div class="col-lg-3">   <label class="login2 pull-right pull-right-pro">Date Of Birth</label>
                                                                    </div> 
                                                                <div class="col-lg-9">
                                                           <div class="input-group date">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                                <asp:Textbox cssclass="form-control"   ID="datepicker1" runat="server"  ></asp:TextBox>
                                                                                            </div>
                                                                    </div> 
                                                            </div>
                                                                </div> 
                                                          
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Phone Number</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                   <asp:Textbox class="form-control" ID="txtPhone" runat="server"  ></asp:TextBox>

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
                     </div>
                            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Upload passport">
                  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Upload Passport</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="row" id="dd" style="margin:5px;">
                                                <span class="picspan"  >
                    <asp:Image ID="Image1" ImageUrl="~/img/noimage.jpg" runat="server" CssClass="picsupload"   />
                    </span></div>
                                                
                                               
                                                <div class="row">
                                                 <div class="col-lg-9 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="file-upload-inner file-upload-inner-right ts-forms">
                                                                        <div class="input append-big-btn">

                                                <asp:FileUpload ID="FileUpload1" CssClass="form-control"  runat="server"   />
</div> 
                                                                        </div> </div> </div>
                <asp:Button ID="Button5" runat="server" Text="Upload" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" />


                                                


</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 
                </asp:WizardStep> 
            <asp:WizardStep ID="School" runat="server" Title="School">
                
                        <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Options</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro"><asp:CheckBox ID="CheckBox1" runat="server" class="form-control-small" text ="Boarding"  Visible="False" autopostback="true"/></label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboHostel" runat="server" enabled="false"  ></asp:DropDownList> 
                                                                               
                                                                     </div>
                                                                </div>
                                                            </div></div>
     <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro"><asp:CheckBox ID="CheckBox2" runat="server"  Visible="False" text="Transport" autopostback="true"/>
</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboTransport" runat="server" enabled="false"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
     <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Admission Fee Status</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboAdm" runat="server"  >
                                                                                  <asp:ListItem Selected="True">Not Paid</asp:ListItem>
                            <asp:ListItem>Paid</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>





</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
                
                
                
              
            </asp:WizardStep>

        
            <asp:WizardStep ID="WizardStep3" runat="server" Title="parent">
             <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Parent Information</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <asp:RadioButtonList id="radParent" runat="server" autopostback="true">
     <asp:ListItem Value="Existing Parent"></asp:ListItem>
        <asp:ListItem Value="New Parent"></asp:ListItem>

</asp:RadioButtonList>                 <asp:Panel ID="Panel1" runat="server"  Visible="False">

    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Parent Phone Number</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtExistPPhone" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Check" /></div>

        </asp:Panel> 
      <asp:Panel id="panel2" runat="server" visible="false">

          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Parent ID</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                   <asp:Textbox class="form-control" ID="txtPID" runat="server"  ></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtid" runat="server" ErrorMessage="Please enter a valid staff id" ForeColor="#FF3300"></asp:RequiredFieldValidator>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Parent Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtPName" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Sex</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cboPSex" runat="server"  >
                                                                                <asp:ListItem Value="Select Gender"></asp:ListItem>
                                                                                <asp:ListItem Value="Male"></asp:ListItem>
                                                                                <asp:ListItem Value="Female"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                       
                                                      
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Phone Number</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                   <asp:Textbox class="form-control" ID="txtPPhone" runat="server"  ></asp:TextBox>

                                                                </div>
                                                            </div>
                                                        </div>
                                                       
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Email</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <asp:Textbox class="form-control" ID="txtPEmail" runat="server"  ></asp:TextBox>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Address</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <asp:Textbox class="form-control" ID="txtPAddress" runat="server" TextMode="MultiLine"  ></asp:TextBox>

                                                                </div>
                                                            </div>
                                                        </div>
            <asp:TextBox ID="txtPass" runat="server" Visible="False"></asp:TextBox>
          <h3>Upload Parent Passport</h3>
              <div class="col-lg-12">
                                                <div class="row" id="Div1" style="margin:5px;">
                                                <span class="picspan"  >
                    <asp:Image ID="Image2" ImageUrl="~/img/noimage.jpg" runat="server" CssClass="picsupload"   />
                    </span></div>
                                                
                                               
                                                <div class="row">
                                                 <div class="col-lg-9 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="file-upload-inner file-upload-inner-right ts-forms">
                                                                        <div class="input append-big-btn">

                                                <asp:FileUpload ID="FileUpload2" CssClass="form-control"  runat="server"   />
</div> 
                                                                        </div> </div> </div>
                <asp:Button ID="Button2" runat="server" Text="Upload" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" />


                                                


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

