<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="editprofile.aspx.vb" Inherits="Admin_addteacher" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
          
                   <h2> EDIT PROFILE</h2>
               <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" OnNextButtonClick="Wizard1_NextButtonClick" style="font-size: small" DisplaySideBar="False" Width="100%" FinishDestinationPageUrl="~/content/App/App/parent/parentprofile.aspx">
                    <NavigationButtonStyle CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" Font-Size="14px" Width="90px" />
        <WizardSteps>
            <asp:WizardStep ID="WizardStep1" runat="server" Title="Profile">
                 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Parent Profile</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Parent ID</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                   <asp:Textbox cssclass="form-control" enabled ="false" ID="txtID" runat="server"  ></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtid" runat="server" ErrorMessage="Please enter a valid staff id" ForeColor="#FF3300"></asp:RequiredFieldValidator>

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

