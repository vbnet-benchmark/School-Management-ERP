<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="publishca.aspx.vb" Inherits="Admin_results" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
   <h2>PUBLISH ASSESSMENTS</h2>
    <br />
      <table class="table">
          <tr runat ="server"   >
              <td class="auto-style1"><asp:Label ID="Label6" runat="server" style="font-weight: 700" Text="CA 1"></asp:Label></td>
              <td class="auto-style2"><asp:Label ID="lblpublishCA1" runat="server"></asp:Label></td>
              <td> <asp:linkButton ID="btnCA1" runat="server" /></td>
              </tr>
           <tr runat ="server" >
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
              <td class="auto-style1"><asp:Label ID="Label3" runat="server" style="font-weight: 700" Text="CA 4"></asp:Label></td>
              <td class="auto-style2"><asp:Label ID="lblPublishProject" runat="server"></asp:Label></td>
              <td> <asp:linkButton ID="btnProject" runat="server" /></td>
              </tr>
          <tr>
           <td class="auto-style1"><asp:Label ID="Label4" runat="server" style="font-weight: 700" Text="Examination"></asp:Label></td>
              <td class="auto-style2"><asp:Label ID="lblPublisgExams" runat="server"></asp:Label></td>
              <td> <asp:linkButton ID="btnExams" runat="server" /></td>
              </tr>
      </table>
        
    <br />
    <br />
   
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
