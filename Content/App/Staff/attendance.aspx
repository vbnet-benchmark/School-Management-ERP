<%@ Page Title="" Language="VB" MasterPageFile="~/DigicontentApp.master" AutoEventWireup="false" CodeFile="attendance.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <h2>STUDENTS ATTENDANCE</h2>
    <br />
          <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-1">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-11">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboClass" runat="server" autopostback="true" >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-1">
                                                                    <label class="login2 pull-right pull-right-pro">Week</label>
                                                                </div>
                                                                <div class="col-lg-11">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboWeek" runat="server" autopostback="true" >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
         <br />
    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
        <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="table" >
              <Columns>
                   <asp:HyperLinkField DataTextField="date" HeaderText ="Date" DataNavigateUrlFields="date" DataNavigateUrlFormatString="~/content/app/Staff/newattendance.aspx?{0}" ><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /> </asp:HyperLinkField>
                          <asp:boundfield datafield="day" headertext="Day"/>
         
                 <asp:boundfield datafield="morning" headertext="Morning"/>
                          <asp:BoundField DataField="afternoon" HeaderText="Afternoon" />
                   <asp:BoundField DataField="percent" HeaderText="Percentage" />
         
                   <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
         
                 </Columns> 
        </asp:GridView></div> </div> 
    <br />
    <h4><asp:Label runat="server" id="lblweekly"></asp:Label></h4>
    <br />
      <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnTake" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Take Attendance" ></asp:Button></div></div> </div> </div> 
    
    
         </asp:Content>





