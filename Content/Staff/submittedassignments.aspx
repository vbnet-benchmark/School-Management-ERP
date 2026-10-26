<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="submittedassignments.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <h2>SUBMITTED ASSIGNMENTS</h2>
    <br />
     <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Subject and Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboSubject" autopostback="true" runat="server"  >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
    <br />
    <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnMsg" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back" ></asp:Button></div></div> </div> </div> 
    <br />
    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">

       <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="newtable" >
              <Columns>
                   <asp:HyperLinkField DataNavigateUrlFields="reference" DataNavigateUrlFormatString="~/content/Staff/markassignment.aspx?{0}" Text="View" ><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:hyperlinkfield>
                   <asp:boundfield datafield="date" headertext="Date"/>
                 <asp:boundfield datafield="student" headertext="Student"/>
                                  <asp:boundfield datafield="title" headertext="Title"/>

            <asp:boundfield datafield="score" headertext="Score"/>
                
                 </Columns> 
        </asp:GridView></div> </div> 

    <div class="login-button-pro" style="text-align:left;" >   <asp:Button ID="btnPrevious" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Previous" /> <asp:Button ID="btnNext" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Next" /></div>

         </asp:Content>




