<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="publishca.aspx.vb" Inherits="Admin_results" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
   <h2>PUBLISH ASSESSMENTS</h2>
    <br />
    <div class="row">
        <div class="col-lg-6">
      <table class="newtable tableflat">
          <tr runat ="server" id="trCAsd3" visible="false" >
              <td class="auto-style1"><asp:Label ID="Label6" runat="server" style="font-weight: 700" Text="CA 1"></asp:Label></td>
              <td class="auto-style2"><asp:Label ID="lblpublishCA1" runat="server"></asp:Label></td>
              <td> <asp:linkButton ID="btnCA1" runat="server" /></td>
              </tr>
           <tr runat ="server" id="trCAdd" visible="false" >
              <td class="auto-style1"><asp:Label ID="Label1" runat="server" style="font-weight: 700" Text="CA 2"></asp:Label></td>
              <td class="auto-style2"><asp:Label ID="lblPublishCA2" runat="server"></asp:Label></td>
              <td> <asp:linkButton ID="btnCA2" runat="server" /></td>
              </tr>
           <tr runat ="server" id="trCA3" visible="false" >
              <td class="auto-style1"><asp:Label ID="Label2" runat="server" style="font-weight: 700" Text="CA 3"></asp:Label></td>
              <td class="auto-style2"><asp:Label ID="lblPublishCA3" runat="server"></asp:Label></td>
              <td> <asp:linkButton ID="btnCA3" runat="server" /></td>
              </tr>
           <tr runat ="server" id="trCA4" visible ="false" >
              <td class="auto-style1"><asp:Label ID="Label3" runat="server" style="font-weight: 700" Text="Mid Term report"></asp:Label></td>
              <td class="auto-style2"><asp:Label ID="lblPublishProject" runat="server"></asp:Label></td>
              <td> <asp:linkButton ID="btnProject" runat="server" /></td>
              </tr>
          <tr>
           <td class="auto-style1"><asp:Label ID="Label4" runat="server" style="font-weight: 700" Text="Termly result"></asp:Label></td>
              <td class="auto-style2"><asp:Label ID="lblPublisgExams" runat="server"></asp:Label></td>
              <td> <asp:linkButton ID="btnExams" runat="server" /></td>
              </tr>
      </table>
        </div></div>
    <br />
    <br />
    <div class="row">
        <div class="col-lg-6">
      <table class="newtable tableflat">
          <tr>
              <td class="auto-style1">External Moderator Email</td>
              <td><asp:Textbox class="form-control" ID="txtModeratorEmail" runat="server"></asp:Textbox></td>
              <td><asp:Button ID="btnNotifyModerator" runat="server" Text="Notify Moderator" cssclass="btn btn-sm btn-primary" /></td>
          </tr>
          <tr>
              <td class="auto-style1">External Moderator Email (validated)</td>
              <td><asp:Textbox class="form-control" ID="txtModeratorEmailSafe" runat="server"></asp:Textbox></td>
              <td><asp:Button ID="btnNotifyModeratorSafe" runat="server" Text="Notify Moderator (Safe)" cssclass="btn btn-sm btn-primary" /></td>
          </tr>
          <tr>
              <td class="auto-style1">External Reviewer Email</td>
              <td><asp:Textbox class="form-control" ID="txtReviewerEmail" runat="server"></asp:Textbox></td>
              <td><asp:Button ID="btnShareWithReviewer" runat="server" Text="Share With Reviewer" cssclass="btn btn-sm btn-primary" /></td>
          </tr>
          <tr>
              <td class="auto-style1">External Reviewer Email (validated)</td>
              <td><asp:Textbox class="form-control" ID="txtReviewerEmailSafe" runat="server"></asp:Textbox></td>
              <td><asp:Button ID="btnShareWithReviewerSafe" runat="server" Text="Share With Reviewer (Safe)" cssclass="btn btn-sm btn-primary" /></td>
          </tr>
      </table>
        </div>
    </div>

         </asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="head">
    <style type="text/css">
        #span1 {
            width: 382px;
        }
        #span2 {
            width: 352px;
        }
        .auto-style1 {
            width: 115px;
        }
        .auto-style2 {
            width: 198px;
        }
    </style>
</asp:Content>
