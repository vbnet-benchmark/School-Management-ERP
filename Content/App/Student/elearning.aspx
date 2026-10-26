<%@ Page Title="" Language="VB" MasterPageFile="~/digicontent.master" AutoEventWireup="false" CodeFile="elearning.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       

  <h2>E - LEARNING MATERIALS</h2>
    <br />
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
        <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40"  GridLines="None" class="table">
              <Columns>
                   <asp:HyperLinkField DataNavigateUrlFields="reference" DataNavigateUrlFormatString="~/content/Student/viewelearning.aspx?{0}" Text="View" ><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:hyperlinkfield>
                   <asp:boundfield datafield="date" headertext="Date">
                       <HeaderStyle HorizontalAlign="Left" />
                   </asp:boundfield>
                 <asp:boundfield datafield="subject" headertext="Subject">

                          <HeaderStyle HorizontalAlign="Left" />
                   </asp:boundfield>

                          <asp:boundfield datafield="title" headertext="Title">
          
                              <HeaderStyle HorizontalAlign="Left" />
                              <ItemStyle HorizontalAlign="Left" />
                   </asp:boundfield>
          
                 </Columns> 
        </asp:GridView></div> </div> 


         <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnPrevious" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Previous" /> <asp:Button ID="btnNext" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Next" /></div>

         </asp:Content>
