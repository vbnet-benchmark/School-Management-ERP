<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="perf.aspx.vb" Inherits="Admin_newterm" %>
<%@ Register Assembly="SlimeeLibrary" Namespace="SlimeeLibrary" TagPrefix="cc1" %>



<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
                <h2>MANAGE PERFORMANCE MONITORING</h2>
    <br />
     <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0"  style="font-size: small" DisplaySideBar="False" Width="100%" startNextButtonCss="btn-small btn-skin btn-block btn-lg" FinishDestinationPageUrl="~/content/Admin/manageperf.aspx">
       
         <NavigationButtonStyle CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" Font-Size="Small" Width="90px" />
                <WizardSteps>
           <asp:WizardStep ID="WizardStep1" runat="server" Title="Profile">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Performance Monitoring Groups</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Group Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtID" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                           <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Low Range</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtLowest" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
       <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">High Range</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtHighest" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                     <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Frequency</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cboFrequency" runat="server"  >
                                                                                  <asp:ListItem>Daily</asp:ListItem>
                                                                                   <asp:ListItem>Weekly</asp:ListItem>
                                                                                   <asp:ListItem>Monthly</asp:ListItem>
                                                                           
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
                                                        
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
                                        <h1>Performance Indicators</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     
    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
                <asp:GridView  runat="server" autogeneratecolumns ="False" ID="Gridview1" showheader="False" GridLines="None" class="newtable">
                     <Columns>
                         <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="trait" HeaderText="Indicator"></asp:BoundField>
                         
                         <asp:CommandField EditText="Edit" ShowEditButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                         <asp:CommandField DeleteText="Remove" EditText="Rename" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                    </Columns>

                </asp:GridView></div> </div> 

           <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkbterm" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Indicator" ></asp:Button></div></div> </div> </div> 
                           
    <asp:Panel ID="Panel1" visible="false" runat="server">
         <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Add Indicator</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Indicator</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtTrait" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

      <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />



</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 




  
            
  
         </asp:Panel>  
            </asp:WizardStep>


            <asp:WizardStep ID="WizardStep3" runat="server" Title="optional">
                

                 
                 <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Classes Affected</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div id ="compulsory"  runat ="server">
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

           </WizardSteps>
    </asp:Wizard>
    
    


       



  
         </asp:Content>




