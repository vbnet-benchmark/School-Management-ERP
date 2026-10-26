<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="editprofile.aspx.vb" Inherits="Admin_addteacher" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td>

                      <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    

    
    

            </td>

        </tr>
        <tr style="width:100%;">
            <td>
                <h2>
                    <asp:Label ID="lblHeader" runat="server" ForeColor="Black" Text="EDIT PROFILE"></asp:Label>
                </h2>

            </td>

                    </tr>
        </table>



    <asp:Wizard ID="Wizard1" OnNextButtonClick="Wizard1_NextButtonClick" width="100%" runat="server" ActiveStepIndex="0" style="font-size: small" DisplaySideBar="False"  FinishDestinationPageUrl="~/content/staff/staffprofile.aspx">
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
                                                                   <asp:Textbox enabled="false" cssclass="form-control" ID="txtID" runat="server"  ></asp:Textbox>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Full Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" enabled ="false" ID="txtSurname" runat="server"  ></asp:TextBox>
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
                                                                        <asp:DropDownList enabled ="false" cssclass = "form-control custom-select-value" ID="cboDesignation" runat="server"  >
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

                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Salary</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <asp:Textbox enabled ="false" cssclass="form-control" ID="txtSalary" runat="server"  ></asp:Textbox>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Account Number</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <asp:Textbox enabled="false" cssclass="form-control" ID="txtAcc" runat="server"  ></asp:Textbox>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Bank</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <asp:Textbox enabled="false" cssclass="form-control" ID="txtBank" runat="server"  ></asp:Textbox>

                                                                </div>
                                                            </div>
                                                        </div>

                                                        
                                                         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Pensioned</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" enabled="false" ID="cboPension" runat="server"  >
                                                                                <asp:ListItem Value="Select"></asp:ListItem>
                                                                                <asp:ListItem Value="Yes"></asp:ListItem>
                                                                                <asp:ListItem Value="No"></asp:ListItem>
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

            
        </WizardSteps>
    </asp:Wizard>
       
    
   </asp:Content>

