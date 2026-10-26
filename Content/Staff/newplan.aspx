<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" ValidateRequest=false AutoEventWireup="false" CodeFile="newplan.aspx.vb" Inherits="Admin_adminpage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
    <style type="text/css">
        #img3 {
            text-align: center;
        }
        #img4 {
            text-align: center;
        }
        #img2 {
            text-align: center;
        }
        #img1 {
            text-align: center;
        }
        .auto-style2 {
           
            text-align: center;
            width:25%;
            height:25%;
            max-height:25%;
            max-width:25%;
        }
        .auto-style3 {
            width: 29%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:TextBox runat="server" id="att1" visible="false"></asp:TextBox><asp:TextBox id="att2" visible="false" runat="server"></asp:TextBox><asp:TextBox id="att3" visible="false" runat="server"></asp:TextBox><asp:TextBox id="att4" visible="false" runat="server"></asp:TextBox>
    
     <h2>LESSON PLANS</h2>
         <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1><asp:Label runat="server" id="lblNew" Text=""></asp:Label> Lesson Plan</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                    <div class="all-form-element-inner">

  <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="Button1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back" ></asp:Button>
                                                                                                                                

                                                            </div></div> </div> </div> 
<br />
                                                       
                                                         <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Week</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="DropDownList4" runat="server"   >
                                                                                           
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
                                                         <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">HOD</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboHOD" runat="server"   >
                                                                            </asp:DropDownList>               
                                                                               
                                                                     </div>
                                                                </div>
                                                            </div></div>
                                                         <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                          <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" onchange="store()" ID="cboSubject" runat="server" autopostback="true"  >
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
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClass" onchange="store()" autopostback="true" runat="server"   >
                                                                             </asp:DropDownList>              
                                                                               
                                                                     </div>
                                                                </div>
                                                            </div></div>

                                                         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-12">
    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                     <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    <div class="row">
                                                    <div class="col-lg-12">
                                                        <div class="text-editor-compose">
                                                            <div id="summernote5">
                                                               
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
    

    <asp:HiddenField id="Hidden1" runat="server"></asp:HiddenField>
                                                         </div>
                                                            </div>
                                                        </div>

                                                                </div>
                                                            </div>
                                                        </div>


                                                        <br />
                                                         <asp:Panel ID="Panel2" runat="server">
                                                                        <h4><asp:Label ID="Label6" runat="server" Text="Upload Files"></asp:Label></h4>

                                                          <div class="row">
                                                 <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <div class="file-upload-inner file-upload-inner-right ts-forms">
                                                                        <div class="input append-big-btn">

                                                <asp:FileUpload ID="FileUpload1" CssClass="form-control"  runat="server"   />
</div> 
                                                                        </div> </div> </div>
                                                        <asp:Button ID="btnUpload" onclientclick="store()" runat="server" Text="Upload" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" />
</asp:Panel> 
<br />
                                                         <asp:Panel ID="Panel1" runat="server" Visible="False">
        <h4>Attachments</h4>
        <table class="newtable" id="attachtable" style="border:none; width:100%;" >
            <tr>
                <td id="img1"  runat ="server" class="auto-style2" >
                    <img alt="" src="" id="icon1" style="max-height:100px; width:50%; border:none;" runat ="server"  />
        </td>   
<td id="img2" runat ="server" class="auto-style2" >
        <img alt="" src="" id="icon2" style= "max-height:100px; width:50%; border:none;" runat ="server"  />
        </td>          <td id="img3" runat ="server" class="auto-style2" >
                    <img alt="" src="" id="icon3" style="max-height:100px; width:50%; border:none;" runat ="server"  />
        </td>   
       <td id="img4" runat ="server" class="auto-style2" >
           <img alt="" src="" id="icon4" style="max-height:100px; width:50%; border:none;" runat ="server"  />
        </td>   
                </tr>
            <tr>
                <td class="auto-style2">
                    <asp:LinkButton ID="LinkButton1" runat="server" ></asp:LinkButton></td>
                <td class="auto-style2">
                    <asp:LinkButton ID="LinkButton2" runat="server"></asp:LinkButton></td>
                <td class="auto-style2">
                    <asp:LinkButton ID="LinkButton3" runat="server"></asp:LinkButton></td>
                <td class="auto-style2">
                    <asp:LinkButton ID="LinkButton4" runat="server"></asp:LinkButton></td>
            </tr>
            <tr>
                <td style="text-align: center" class="auto-style2">
                    <asp:LinkButton ID="del1" runat="server" ></asp:LinkButton></td>
                <td style="text-align: center" class="auto-style2">
                    <asp:LinkButton ID="del2" runat="server"></asp:LinkButton></td>
                <td style="text-align: center" class="auto-style2">
                    <asp:LinkButton ID="del3" runat="server"></asp:LinkButton></td>
                <td style="text-align: center" class="auto-style2">
                    <asp:LinkButton ID="del4" runat="server"></asp:LinkButton></td>
            </tr>
            </table>
    </asp:Panel>
 <br />
                                                         <asp:Panel ID="pnlRemarks" runat="server" Visible="False">
          <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Remarks</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                     <asp:Textbox cssclass="form-control" ID="txtRem" enabled="false" runat="server" textmode="multiline"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>    
                                                             </asp:Panel> 
    <asp:Button ID="btnSend" onclientclick="store()" runat="server" Text="Send" cssclass="btn btn-sm btn-primary login-submit-cs buttonsnew"  />
             







</div>
                                        </div>
                        
    
    
    </div> </div> </div> </div> </div> </div> 
    
                   
   
      </asp:Content>