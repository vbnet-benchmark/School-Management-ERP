<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="managetimetable.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       
  <h2>MANAGE TIME TABLES</h2>
    <br />

      <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnknEW" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Time Table" ></asp:Button></div></div> </div> </div> 
    <br />
      <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
                                                                <div class="row">
        <div class="col-lg-6">
                                                                   <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40" GridLines="Horizontal" class="newtable tableflat" ShowHeader="False" >
              <Columns>
                   <asp:boundfield datafield="name" headertext="Time table"/>
                                  
                   <asp:CheckBoxField DataField="default"></asp:CheckBoxField>
                                   <asp:HyperLinkField DataNavigateUrlFields="id,class,school" DataNavigateUrlFormatString="~/content/Admin/newtimetable.aspx?id={0}&amp;class={1}&amp;school={2}" Text="Rebuild" ><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:hyperlinkfield>

                   <asp:HyperLinkField DataNavigateUrlFields="id,class,school" DataNavigateUrlFormatString="~/content/Admin/viewtimetable.aspx?timetable={0}&amp;class={1}&amp;school={2}" Text="View"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:HyperLinkField>

                   <asp:CommandField ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>

                 </Columns> 
            
        </asp:GridView></div> </div> </div> </div> 
    <br />
         </asp:Content>




