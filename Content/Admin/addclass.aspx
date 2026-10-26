<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="addclass.aspx.vb" Inherits="Admin_addclass" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h1>ADD CLASS</h1>
    
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0"  style="font-size: small" DisplaySideBar="False" Width="100%" startNextButtonCss="btn-small btn-skin btn-block btn-lg" FinishDestinationPageUrl="~/content/Admin/manageclass.aspx">
        <NavigationButtonStyle CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew"  />
        
        <WizardSteps>
           <asp:WizardStep ID="WizardStep1" runat="server" Title="Profile">
                 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Class Information</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Class Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtId" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class type</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cbotype" runat="server"  >
                                                                                <asp:ListItem>Select Type</asp:ListItem>
                                                                              <asp:ListItem>K.G 1 Special</asp:ListItem>
                                                                                   <asp:ListItem>Nursery</asp:ListItem>
                                                                                   <asp:ListItem>Primary</asp:ListItem>
                                                                            <asp:ListItem>Primary 6</asp:ListItem>
                                                                                   <asp:ListItem>Junior Secondary</asp:ListItem>
                                                                                   <asp:ListItem>Senior Secondary</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Department</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cboDept" runat="server"  >
                                                                                <asp:ListItem>Select Type</asp:ListItem>
                                                                                   <asp:ListItem>Nursery</asp:ListItem>
                                                                                   <asp:ListItem>Primary</asp:ListItem>
                                                                                   <asp:ListItem>Junior Secondary</asp:ListItem>
                                                                                   <asp:ListItem>Senior Secondary</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">No Of CA</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cboCaNo" runat="server"  >
                                                                                  <asp:ListItem>2</asp:ListItem>
                                                                                <asp:ListItem>3</asp:ListItem>
                                                                                <asp:ListItem>4</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Next Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cboNextClass" runat="server"  >
                                                                                 
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
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Upload passport">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Subjects offerred</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
      <div id ="compulsory" style="float:left; font:small" runat ="server"><h4>Compulsory Subjects</h4>
                                <asp:CheckBoxList ID="CheckBoxList2" runat="server"  EnableViewState="true" >
                            </asp:CheckBoxList>
                               

						   </div>






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
                                        <h1>Subjects offerred</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

      <div id ="Div1"  runat="server"><h4>Optional Subjects</h4>
                               <asp:CheckBoxList ID="CheckBoxList3" runat="server" >
                            </asp:CheckBoxList>


                               <br />


                           </div>






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
                                        <h1>Allocate Periods</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
     <asp:GridView  runat="server" autogeneratecolumns ="false" ID="Gridview1" GridLines="None" class="table">
                     <Columns>
                        <asp:BoundField DataField="subject" HeaderText="Subject" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="periods" HeaderText="Periods (Weekly)"></asp:BoundField>
                        <asp:CommandField ShowEditButton="True" CausesValidation="False"> <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                    </Columns>

                </asp:GridView></div> </div> 
     
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




