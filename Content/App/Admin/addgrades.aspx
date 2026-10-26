<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="addgrades.aspx.vb" Inherits="Admin_addclass" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>ADD GRADING SYSTEM</h2>
    
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0"  style="font-size: small" DisplaySideBar="False" Width="100%" startNextButtonCss="btn-small btn-skin btn-block btn-lg" FinishDestinationPageUrl="~/content/App/Admin/managegrades.aspx">
       
         <NavigationButtonStyle CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" Font-Size="Small" Width="90px" />
                <WizardSteps>
           <asp:WizardStep ID="WizardStep1" runat="server" Title="Profile">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Grading System Details</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Grading System Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtID" runat="server"  ></asp:TextBox>
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
                                        <h1>Grades</h1>
                                       
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
                        <asp:BoundField DataField="lowest" HeaderText="Score Range" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="grade" HeaderText="Grade"></asp:BoundField>
                         <asp:BoundField DataField="subject" HeaderText="Subject Remarks"></asp:BoundField>
                          <asp:BoundField DataField="average" HeaderText="Average Remarks"></asp:BoundField>
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
    <asp:Button ID="lnkNew" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Grade" ></asp:Button></div></div> </div> </div> 

     <br />
     <asp:Panel id="panel1" runat="server" visible="false">

      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Lowest score</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtLowest" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Grade</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtGrade" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Subject Remarks</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtRemarks" runat="server"   ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>.
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Average Remarks</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtARemarks" runat="server" textmode="multiline"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
         <br />
     
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
                                <asp:CheckBoxList ID="CheckBoxList2" runat="server" EnableViewState="true" >
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










