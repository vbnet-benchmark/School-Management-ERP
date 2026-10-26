<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" ValidateRequest=false AutoEventWireup="false" CodeFile="newtemplate.aspx.vb" Inherits="Admin_adminpage" %>

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
      
     <h2>LESSON PLAN TEMPLATES</h2>
          <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>New Template</h1>
                                       
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

                                                            </div>

                                                        </div> 

                   </div> 

  </div> 
                                                         <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Template Name</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                     <asp:Textbox cssclass="form-control" ID="txtName" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

 
<br />
                                             

         
                                  <br />       
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


                                                        <br />
                                                         <h1>Classes Affected</h1>
                                                         <div id ="compulsory"  runat ="server">
                                <asp:CheckBoxList ID="CheckBoxList2" runat="server" EnableViewState="true" >
                            </asp:CheckBoxList>


						   </div>
    <asp:Button ID="btnExportClasses" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Export Assigned Classes" ></asp:Button>
    <asp:Button ID="btnExportClassesSafe" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Export Assigned Classes (Secure)" ></asp:Button>
    <br />

    <asp:Button ID="btnSend" onclientclick="store()" runat="server" Text="Update" cssclass="btn btn-sm btn-primary login-submit-cs buttonsnew"  />
             








</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
    
   

    
                   
   
      </asp:Content>

