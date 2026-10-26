<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" ValidateRequest=false AutoEventWireup="false" CodeFile="newmsg.aspx.vb" Inherits="Admin_adminpage" %>
   <%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

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

     <h2>PARENT MESSAGING</h2>
       <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Compose Message</h1>
                                       
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
    <asp:Button ID="Button6" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back" ></asp:Button>
                                                                                                                                <asp:Button ID="LinkButton5" runat="server" class="btn btn-white btn-xs" Text="Sent Messages"></asp:Button>

                                                            </div>

                                                        </div> 

                   </div> 

  </div> 
<br />
                                                        <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Subject</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtSubject" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    
                                                         <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Receiver Type</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboCategory" runat="server"   >
                                                                                          
                                                                                </asp:DropDownList> <asp:linkButton ID="Button1" runat="server" onclientclick="store()" Text="Add" />
                                                                     </div>
                                                                </div>
                                                            </div></div>
                                                        
                                                                     
                                                                        

                                               
                            <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0; ">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>Recipients</h1>
                                        <div class="sparkline11-outline-icon">
                                            <span class="sparkline11-collapse-link"><i class="fa fa-chevron-up"></i></span>
                                            <span><i class="fa fa-wrench"></i></span>
                                            <span class="sparkline11-collapse-close"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment" style="overflow:auto; height:200px;" id="mydiv"  >
                                    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
                                   <asp:GridView GridLines="None" autogeneratecolumns="false" class="table"  showheader="False"  id="gridRecipients" runat="server">
                                       <Columns>
                                           <asp:BoundField DataField="name"></asp:BoundField>
                                           <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                                       </Columns>
                                    </asp:GridView>
                                    </div> 
                                        </div> 

                                </div>
                                
                            </div>
                       
                                     <br />
                                                <br />                          

                                                         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-12">
                                                                     <FTB:FreeTextBox id="FreeTextBox1" runat="Server" visible="false" width="100%" EditorBorderColorDark="blue" EditorBorderColorLight="LightBlue" GutterBorderColorDark="Gray" GutterBorderColorLight="LightBlue" ToolbarStyleConfiguration="Office2003" ButtonSet="OfficeMac" DesignModeCss="" ButtonDownImage="True" ButtonOverImage="True" ButtonWidth="35" EnableHtmlMode="False" Focus="True" />

                                                                      <div class="email editor">
    <label class="control-label sr-only" for="summernote">Descriptions </label>
                                    <textarea class="form-control" id="summernote" name="editordata" rows="6" placeholder="Write Descriptions"></textarea>
         </div> 

    <asp:HiddenField id="Hidden1" runat="server"></asp:HiddenField>
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
                                                        <asp:Button ID="btnUpload" runat="server" onclientclick="store()" Text="Upload" CssClass="btn btn-sm btn-primary login-submit-cs buttonsnew" />
</asp:Panel> 
<br />
                                                         <asp:Panel ID="Panel1" runat="server" Visible="False">
        <h4>Attachments</h4>
        <table class="table" id="attachtable" style="border:none; width:100%;" >
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
            
    <asp:Button ID="btnSend" runat="server" Text="Send" onclientclick="store()" cssclass="btn btn-sm btn-primary login-submit-cs buttonsnew"  />
             







</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
    
    

    
                   
   
      </asp:Content>
