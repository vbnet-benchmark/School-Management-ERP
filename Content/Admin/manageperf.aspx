<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="manageperf.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      
    

  <h2>MANAGE PERFORMANCE MONITORING</h2>
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnknEW" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Performance Group" ></asp:Button></div></div> </div> </div> 
   
    <br />
    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
        <asp:GridView ID="GridView1" AutoGenerateColumns="False" GridLines="horizontal" class="newtable tableflat" runat="server" AllowPaging="True" PageSize="40" ShowHeader="False" >
              <Columns>
                   <asp:boundfield datafield="grade" headertext="Grading System"/>
                                   <asp:HyperLinkField DataNavigateUrlFields="id" DataNavigateUrlFormatString="~/content/Admin/perf.aspx?{0}" Text="View" ><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:Hyperlinkfield>

                 </Columns> 
              
        </asp:GridView> </div> </div> 
    <br />
         </asp:Content>




