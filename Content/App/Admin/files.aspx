<%@ Page Title="" Language="VB" MasterPageFile="~/masterpage.master" AutoEventWireup="false" CodeFile="files.aspx.vb" Inherits="Admin_allstudents" %>
<%@ Register Assembly="SlimeeLibrary" Namespace="SlimeeLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <h2>RECEIVED FILES</h2>
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <img src="../image/loading36.gif" style="height: 31px; width: 36px" />
                Processing....
                </ProgressTemplate>
            </asp:UpdateProgress>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
     <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    
   <asp:LinkButton runat="server" ID="lnkDashBoard" OnClick="Unnamed1_Click">Back to Dashboard</asp:LinkButton>
         <br /><p><asp:Label ID="Label1" runat="server" style="font-size: large; font-weight: 700" Text="Search file: "></asp:Label>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:TextBox ID="txtSearch" runat="server" class="form-control-small" Width="201px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
         <asp:Button ID="bnBack"  style="padding:0px 0px 0px 0px; font-size: 14px;" runat="server" class="btn-small btn-skin btn-block btn-lg" Text="Search" />
 </p>       
    <h4>Filter by Date</h4>
   
   <table >
       <tr ><td>
         <asp:Label ID="Label3" runat="server" style="font-size: large; font-weight: 700" Text="From:   "></asp:Label>&nbsp; </td>
   <td>  <cc1:DatePicker ID="DatePicker1" runat="server" Width="100px"  PaneWidth="150px" style="border-radius:2px;">
        <PaneTableStyle BorderColor="#707070" BorderWidth="1px" BorderStyle="Solid" />
        <PaneHeaderStyle BackColor="#0099FF" />
        <TitleStyle ForeColor="White" Font-Bold="true" />
        <NextPrevMonthStyle ForeColor="White" Font-Bold="true" />
        <NextPrevYearStyle ForeColor="#E0E0E0" Font-Bold="true" />
        <DayHeaderStyle BackColor="#E8E8E8" />
        <TodayStyle BackColor="#FFFFCC" ForeColor="#000000" Font-Underline="false" BorderColor="#FFCC99"/>
        <AlternateMonthStyle BackColor="#F0F0F0" ForeColor="#707070" Font-Underline="false"/>
        <MonthStyle BackColor="" ForeColor="#000000" Font-Underline="false"/>
    </cc1:DatePicker></td>
          
      <td>
          &nbsp;<asp:Label ID="Label2" runat="server" style="font-size: large; font-weight: 700" Text="To:   "></asp:Label>&nbsp; </td>
   <td> <cc1:DatePicker ID="DatePicker2" runat="server" Width="100px" PaneWidth="150px" style="border-radius:2px;">
        <PaneTableStyle BorderColor="#707070" BorderWidth="1px" BorderStyle="Solid" />
        <PaneHeaderStyle BackColor="#0099FF" />
        <TitleStyle ForeColor="White" Font-Bold="true" />
        <NextPrevMonthStyle ForeColor="White" Font-Bold="true" />
        <NextPrevYearStyle ForeColor="#E0E0E0" Font-Bold="true" />
        <DayHeaderStyle BackColor="#E8E8E8" />
        <TodayStyle BackColor="#FFFFCC" ForeColor="#000000" Font-Underline="false" BorderColor="#FFCC99"/>
        <AlternateMonthStyle BackColor="#F0F0F0" ForeColor="#707070" Font-Underline="false"/>
        <MonthStyle BackColor="" ForeColor="#000000" Font-Underline="false"/>
    </cc1:DatePicker></td>
           <td>           <asp:Button ID="btnDate"  style="padding:0px 0px 0px 0px; font-size: 14px;" runat="server" class="btn-small btn-skin btn-block btn-lg" Text="Search" />
</td>
          </tr>  
       </table>
    
    
    <br />
    <asp:LinkButton id="lnkNew" runat="server">New File</asp:LinkButton>
        &nbsp;&nbsp;&nbsp;
    <asp:LinkButton ID="LinkButton3" runat="server">Receive File</asp:LinkButton>
         
        <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" width="100%" PageSize="40" AllowSorting="True" BorderStyle="None" GridLines="Horizontal" >
             <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="File No" DataNavigateUrlFormatString="~/trackfiles.aspx?{0}" ItemStyle-Width = "150" Text="Track" >
<ItemStyle Width="150px"></ItemStyle>
                 </asp:HyperLinkField>
                  <asp:boundfield datafield="File No" headertext="File No"/>
                 <asp:boundfield datafield="File Name" headertext="File Name"/>
                 <asp:boundfield datafield="date" readonly="true" headertext="Date"/>
                 </Columns> 
        </asp:GridView>
        <asp:Button ID="btnPrevious" style="padding:0px 0px 0px 0px; font-size: 14px;" runat="server" class="btn-small btn-skin btn-block btn-lg" Text="Newer" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnNext" style="padding:0px 0px 0px 0px; font-size: 14px;" runat="server" class="btn-small btn-skin btn-block btn-lg"  Text="Older" Width="76px" />
        
    <br />
    </ContentTemplate>
            </asp:UpdatePanel>
 
         </asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="head">
    <style type="text/css">
        #span1 {
            width: 382px;
        }
        #span2 {
            width: 352px;
        }
    </style>
</asp:Content>


