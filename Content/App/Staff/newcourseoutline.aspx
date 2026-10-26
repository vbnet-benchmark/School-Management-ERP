<%@ Page Title="" Language="VB" MasterPageFile="~/DigicontentApp.master" AutoEventWireup="false" CodeFile="newcourseoutline.aspx.vb" Inherits="Admin_addteacher" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
      <h2 class="auto-style1">COURSE OUTLINES</h2>
         <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>View Course Outlines</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubject2" autopostback="true" runat="server"  >
                                                                               
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
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClass2" autopostback="true" runat="server"  >
                                                                              
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
     <br /><div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
     <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="table"
 >
              <Columns>
                   <asp:BoundField DataField="week" HeaderText="Week"></asp:BoundField>
                 <asp:boundfield datafield="topic" headertext="Topic"/>
                          <asp:boundfield datafield="content" headertext="Content"/>
         
                  
         <asp:CommandField  SelectText="Edit" ShowSelectButton="True" ButtonType="Button">
                    <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" />
                </asp:CommandField>
                   <asp:CommandField DeleteText="Remove" ShowDeleteButton="True" buttontype="button"> <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" /></asp:CommandField>
         
                 </Columns> 
        </asp:GridView></div> </div> 
     <br />
           <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkNew" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Entry" ></asp:Button></div></div> </div> </div> 
     



</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

    <asp:Panel id ="panel1" visible="false"  runat="server">
              <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Course Outline Entry</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubject" enabled="false" runat="server"  >
                                                                               
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
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClass" enabled="false" runat="server"  >
                                                                              
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
            <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Week</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboWeek" autopostback="true" runat="server"  >
                                                                              
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Topic</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtTopic"  runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Content</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtContent" runat="server"  textmode="multiline" ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnUpdate" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Update" /><asp:Button ID="btnBack" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Back" /></div>


         







</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

         </asp:Panel> 

  

    
   </asp:Content>

