<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="elscores.aspx.vb" Inherits="Admin_studentprofile" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
      <h2>PROGRESS REPORT</h2>

    

             <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList ID="DropDownList2" runat="server" class="form-control custom-select-value" autopostback="true">
                                                                                 
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
                                                        </div>

        <asp:Panel id ="panel1" visible="false"  runat="server">
              <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Aspects</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                                                 


                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div4" style="width:100%;">  
                                                 <asp:GridView ID="GridView2" AutoGenerateColumns="False" ShowHeader="False" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="table"
 >
              <Columns>
                  
                 <asp:boundfield datafield="Aspect" headertext="Aspect"/>
                         
                  
         <asp:CommandField  SelectText="Edit" ShowSelectButton="True" ButtonType="Button">
                    <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" />
                </asp:CommandField>
                   <asp:CommandField DeleteText="Remove" ShowDeleteButton="True" buttontype="button"> <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" /></asp:CommandField>
         
                 </Columns> 
        </asp:GridView></div> </div> </div></div> 
                                       
 <div class="all-form-element-inner">
       
     
           
     <div class="form-group-inner">
          <br />
                                        <br />
                                                            <div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Add Aspect</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                     <asp:Textbox cssclass="form-control" ID="txtAspect"  runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
  
      <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnTopUpdate" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add Aspect" /><asp:Button ID="btnSubAspects" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew"  Text="Show SubAspects" /></div>


         







</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                      </asp:Panel>  
                  

      

        <asp:Panel id ="panel5" visible="false"  runat="server">
              <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Sub Aspects</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                          <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnShowAspects" class="btn btn-white btn-xs"  runat="server" Text="Show Aspects"></asp:Button></div></div> </div> </div> 


                                        <br />
                                        <br />
                                        <div class="form-group-inner">
<div class="row">
                                                               
                                                                <div class="col-lg-12">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubAspect" autopostback="true" runat="server"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
      


                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div5" style="width:100%;">  
                                                 <asp:GridView ID="GridView3" AutoGenerateColumns="False" ShowHeader="False" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="table"
 >
              <Columns>
                  
                 <asp:boundfield datafield="Aspect" headertext="Aspect"/>
                           <asp:boundfield datafield="subAspect" headertext="Sub Aspect"/>
                  
         <asp:CommandField  SelectText="Edit" ShowSelectButton="True" ButtonType="Button">
                    <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" />
                </asp:CommandField>
                   <asp:CommandField DeleteText="Remove" ShowDeleteButton="True" buttontype="button"> <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" /></asp:CommandField>
         
                 </Columns> 
        </asp:GridView></div> </div> </div></div>
                                        <br />

 <div class="all-form-element-inner">
       
           
           
             <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Sub Aspect</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                     <asp:Textbox cssclass="form-control" ID="txtSubSubAspect"  runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
  
      <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnSubSubAspect" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add SubAspect" /> </div>
    <br />
    <asp:Label ID="lblTopicLookupHeading" runat="server" Text="Topic usage lookup (select mode, then enter the topic key):" style="font-weight: 700"></asp:Label>
    <br />
    <asp:DropDownList ID="ddlTopicLookupMode" runat="server">
        <asp:ListItem Text="Current (numeric Topic ID)" Value="numeric" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Legacy (free-text topic name)" Value="legacy"></asp:ListItem>
    </asp:DropDownList>
    <asp:TextBox ID="txtTopicKey" runat="server"></asp:TextBox>
    <asp:Button ID="btnTopicLookup" runat="server" Text="Look up usage" />
    <br />
    <asp:GridView ID="GridViewTopicUsage" runat="server" AutoGenerateColumns="True" GridLines="None" class="newtable"></asp:GridView>


         







</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    

         </asp:Panel>
      
      
  

      
       
    </asp:Content>