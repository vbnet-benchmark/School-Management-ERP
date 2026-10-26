<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="newterm.aspx.vb" Inherits="Admin_newterm" %>
<%@ Register Assembly="SlimeeLibrary" Namespace="SlimeeLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      
    </asp:content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
            
                <h2>MANAGE TERMS</h2>

            <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Terms</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph" id="focusform" runat ="server">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
<div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
                                                                <div class="row">
        <div class="col-lg-4">
 <asp:GridView  runat="server" autogeneratecolumns ="False" ID="Gridview1" showheader="False"  class="newtable tableflat" BorderColor="#CCCCCC" GridLines="Horizontal" BorderStyle="None">
                     <Columns>
                        <asp:BoundField DataField="session" HeaderText="Session" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="term" HeaderText="Term"></asp:BoundField>
                         <asp:CommandField ButtonType="Button" SelectText="View" ShowSelectButton="True">
                             <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" />
                         </asp:CommandField>
                    </Columns>

                </asp:GridView></div> </div></div> </div> 
<br />
                                                <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="linkbterm" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Term" ></asp:Button></div></div> </div> </div> 
<asp:Panel ID="Panel1" visible="false" runat="server">
    
                                                 <div class="all-form-element-inner">
                                                     <div class="form-group-inner">


    <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Session</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                        
                                                                       <asp:DropDownList ID="DropDownList1" runat="server" class = "form-control custom-select-value">
       </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div> 
                                                     
                                                 
                                                     <div class="form-group-inner">
    <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Term</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                      <asp:DropDownList ID="DropDownList2" runat="server" Class="form-control custom-select-value">
            <asp:ListItem>Select</asp:ListItem>
        <asp:ListItem Value="1st term"></asp:ListItem>
        <asp:ListItem Value="2nd term"></asp:ListItem>
        <asp:ListItem Value="3rd term"></asp:ListItem>
        </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div> 
  <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Starting date</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:TextBox ID="txtOpen" class="form-control" runat="server"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
            TargetControlID="txtOpen" Format="dd/MM/yyyy" 
            PopupButtonID="TextBox2" >
        </cc1:CalendarExtender> </div>
                                                            </div>
                                                        </div>
                       
                          
                        </div>
                    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Closing date</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:TextBox ID="txtClose" class="form-control" runat="server"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
            TargetControlID="txtClose" Format="dd/MM/yyyy" 
            PopupButtonID="TextBox2" >
        </cc1:CalendarExtender> </div>
                                                            </div>
                                                        </div>
                       
                          
                       
                                             <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Next Term's Resumption Date</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:TextBox ID="txtNext" class="form-control" runat="server"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" 
            TargetControlID="txtNext" Format="dd/MM/yyyy" 
            PopupButtonID="TextBox2" >
        </cc1:CalendarExtender> </div>
                                                            </div>
                                                        </div>
                       


                          
                       
    <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />
                    </div>              
     </asp:Panel>         
 </div>
                </div>
                                 
          </div> 
</div>
                                        </div>
                                    </div>
                                </div>
                          

                                



  
         </asp:Content>




