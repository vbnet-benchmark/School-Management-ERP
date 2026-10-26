<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" ValidateRequest=false AutoEventWireup="false" CodeFile="doassignment.aspx.vb" Inherits="Admin_adminpage" %>
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
    
     <h2>ASSIGNMENTS</h2>
         <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>DO ASSIGNMENT</h1>
                                       
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
                                                                    <label class="login2 pull-right pull-right-pro">Title</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtTitle" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
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
                                                                    <label class="login2 pull-right pull-right-pro">Deadline</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox cssclass="form-control" ID="txtDeadline" runat="server" enabled="false"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
       
                                                           <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0; ">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>Question</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment" style="overflow:auto; height:400px;" id="mydiv"  >
                                    <div class="comment-replay-profile">
                                                                                                                  <asp:PlaceHolder id ="plcQuestion" runat="server"></asp:PlaceHolder>
             
                                        </div> 

                                </div>
                                
                            </div>
                                                           
                                                        </div>

                                                <br />


<h3>Your Answer</h3>


                            <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-12">
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
            
    <asp:Button ID="btnSend" runat="server" onclientclick="store()" Text="Submit" cssclass="btn btn-sm btn-primary login-submit-cs buttonsnew"  />


    <br />
    <div class="form-group-inner">
        <div class="row">
            <div class="col-lg-3">
                <label class="login2 pull-right pull-right-pro">Supplementary Material</label>
            </div>
            <div class="col-lg-9">
                <asp:FileUpload ID="FileUploadSupplementary" CssClass="form-control" runat="server" />
                <br />
                <asp:Button ID="btnUploadSupplementary" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Attach Supplementary File" />
                <asp:Button ID="btnUploadSupplementarySafe" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Attach Supplementary File (Validated)" />
                <br />
                <asp:Label ID="lblSupplementaryResult" runat="server"></asp:Label>
                <asp:Label ID="lblSupplementaryResult2" runat="server"></asp:Label>
            </div>
        </div>
    </div>





</div>
                                        </div>
                        
    
    
    </div> </div> </div> </div> </div> </div> 
    
                   
   
      </asp:Content>