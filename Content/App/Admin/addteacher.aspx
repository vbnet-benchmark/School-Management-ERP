<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="addteacher.aspx.vb" Inherits="Admin_addteacher" %>
<%@ Register Assembly="SlimeeLibrary" Namespace="SlimeeLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td>


    
    

            </td>

        </tr>
        <tr style="width:100%;">
            <td>
                <h2>
                    <asp:Label ID="lblHeader" runat="server" ForeColor="Black" Text="NEW STAFF"></asp:Label>
                </h2>

            </td>

                    </tr>
        </table>



   <asp:Wizard ID="Wizard1" OnNextButtonClick="Wizard1_NextButtonClick"  runat="server" ActiveStepIndex="0" style="width:100%;" DisplaySideBar="False" FinishDestinationPageUrl="~/content/App/Admin/staffprofile.aspx">
        <NavigationButtonStyle CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" Font-Size="14px" Width="90px" />
        <WizardSteps>
            <asp:WizardStep ID="WizardStep1" runat="server" Title="Profile">
                 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Staff Profile</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Staff ID</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                   <asp:Textbox cssclass="form-control" ID="txtID" runat="server"  ></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtid" runat="server" ErrorMessage="Please enter a valid staff id" ForeColor="#FF3300"></asp:RequiredFieldValidator>

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
                                                       
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Email</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <asp:Textbox class="form-control" ID="txtEmail" runat="server"  ></asp:TextBox>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Address</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <asp:Textbox class="form-control" ID="txtAddress" runat="server" TextMode="MultiLine"  ></asp:TextBox>

                                                                </div>
                                                            </div>
                                                        </div>

                                                         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Designation</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                        <asp:DropDownList class = "form-control custom-select-value" ID="cboDesignation" runat="server"  >
                                                                        <asp:ListItem Value="Select Designation"></asp:ListItem>
                                                                        <asp:ListItem Value="Management"></asp:ListItem>
                                                                        <asp:ListItem Value="Teacher"></asp:ListItem>
                                                                                    <asp:ListItem Value="Driver"></asp:ListItem>
                                                                                    <asp:ListItem Value="Cleaner"></asp:ListItem>
                                                                                    <asp:ListItem Value="Minder"></asp:ListItem>
                                                                                    <asp:ListItem Value="Hostel Master/Mistress"></asp:ListItem>
                                                                        </asp:DropDownList>
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
           

             
 
            <asp:WizardStep ID="WizardStep3" runat="server" Title ="Subjects taught">
                <asp:CheckBox ID="CheckBox2" runat="server"  autopostback="true" text ="Subject Teacher" class="i-checks iCheck-helper" />
                <asp:Panel id="panel1" runat="server" visible="false">
                            <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Subjects Taught</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">

                                                <div class="sparkline11-graph dashone-comment dashtwo-comment" style="overflow:scroll; height:200px;" id="mydiv"  >
                                    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
                                   <asp:GridView GridLines="None" autogeneratecolumns="false" class="table"  showheader="False"  id="gridSubjects" runat="server">
                                       <Columns>
                                           <asp:BoundField DataField="name"></asp:BoundField>
                                           <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                                       </Columns>
                                    </asp:GridView>
                                    </div> 
                                        </div> 

                                </div>
                                
                            </div>
                                            </div>
                                                <br />
                                                <br />
                                        <h4>ADD SUBJECT</h4>
                                                  <div class="form-group-inner">
                                                            <div class="row">
                                                                   <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                                        <asp:DropDownList class = "form-control custom-select-value" ID="cboSubject" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                                      </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                  <div class="form-group-inner">
                                                            <div class="row">
                                                                   <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                                        <asp:DropDownList class = "form-control custom-select-value" ID="cboClass" AutoPostBack="true" runat="server" ></asp:DropDownList>

                                                                      </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
                <asp:Button ID="Button2" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" runat="server" Text="Remove" />




</>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 



                           </asp:Panel>
                    </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Classes Managed" ID="classmanaged">
                <asp:CheckBox ID="CheckBox1" autopostback="true" runat="server"  text ="Class Teacher" class="i-checks iCheck-helper " />
                <asp:Panel id="panel2" runat="server" visible="false">
                  <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Classes Managed</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                 <div class="sparkline11-graph dashone-comment dashtwo-comment" style="overflow:scroll; height:200px;" id="Div1"  >
                                    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div2" style="width:100%;">
                                   <asp:GridView GridLines="None" autogeneratecolumns="false" class="table"  showheader="False"  id="gridClass" runat="server">
                                       <Columns>
                                           <asp:BoundField DataField="name"></asp:BoundField>
                                           <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                                       </Columns>
                                    </asp:GridView>
                                    </div> 
                                        </div> 

                                </div>
                                
                            </div>
                                            </div>
                                                <br />
                                                <br />
                                        <h4>ADD CLASS</h4>
                                                  <div class="form-group-inner">
                                                            <div class="row">
                                                                   <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                                        <asp:DropDownList class = "form-control custom-select-value" ID="cboClass1" AutoPostBack="true" runat="server" ></asp:DropDownList>

                                                                      </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                                <asp:Button ID="Button3" runat="server" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
                <asp:Button ID="Button4" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" runat="server" Text="Remove" />




</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 
            </asp:Panel>
                    </asp:WizardStep>
            <asp:WizardStep ID="Roles" runat="server" Title="Roles">
                <h4>Staff Roles</h4>
                <table style="width:100%;">
                        
                   <tr><td class="auto-style2">
<asp:CheckBox ID="ChkAccount" runat="server"  text ="School Accountant" CssClass="accordion" Font-Size="14px" autopostback="true"/>

            </td></tr>
                    <tr><td class="auto-style2">
<asp:CheckBox ID="chkAdmin" runat="server"  text ="School Admin" CssClass="accordion" Font-Size="14px" autopostback="true"/>

            </td></tr>
                  <tr><td class="auto-style2">
<asp:CheckBox ID="chkLib" runat="server"  text ="School Librarian" CssClass="accordion" Font-Size="14px" autopostback="true"/>

            </td></tr>   
    </table>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       
    
   </asp:Content>

