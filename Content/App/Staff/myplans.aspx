<%@ Page Title="" Language="VB" MasterPageFile="~/DigicontentApp.master" AutoEventWireup="false" CodeFile="myplans.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <h2>MY LESSON PLANS</h2>
    <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkNew" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Lesson Plan" ></asp:Button></div></div> </div> </div> 
    <br />
    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
        <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40" GridLines="None" class="table" >
              <Columns>
                   <asp:HyperLinkField DataNavigateUrlFields="reference" DataNavigateUrlFormatString="~/content/app/Staff/markedplan.aspx?{0}" Text="View" ><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:Hyperlinkfield>
                   <asp:boundfield datafield="date" headertext="Date"/>
                  <asp:boundfield datafield="week" headertext="Week"/>
                                  <asp:boundfield datafield="subject" headertext="Subject"/>
                  <asp:boundfield datafield="class" headertext="Class"/>
                  <asp:boundfield datafield="status" headertext="Status"/>
                
                 </Columns> 
        </asp:GridView></div> </div> 
    <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnPrevious" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Previous" /> <asp:Button ID="btnNext" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Next" /></div>

       
    
    
         </asp:Content>



