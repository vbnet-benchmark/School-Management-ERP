<%@ Page Title="" Language="VB" MasterPageFile="~/masterpage.master" AutoEventWireup="false" CodeFile="scoursedetails.aspx.vb" Inherits="Admin_addteacher" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    
     <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    <h2>COURSE DETAILS</h2>
    
    <asp:DetailsView ID="DetailsView1"  runat="server" Height="16px" Width="623px" AutoGenerateRows="False" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
        <EditRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
        <Fields>
            <asp:BoundField DataField="Subject" HeaderText="Subject:" />
            <asp:BoundField DataField="instructor" HeaderText="Subject Teacher"></asp:BoundField>
            <asp:BoundField DataField="Phone" HeaderText="Teacher's Phone No"></asp:BoundField>
            <asp:BoundField DataField="Overview" HeaderText="Course Overview" />
            <asp:BoundField DataField="Textbooks" HeaderText="Recommended Textbooks" />
        </Fields>
        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
    </asp:DetailsView> 
    <br />
    <h4>Course Outline</h4>

    <asp:GridView ID="GridView1" AutoGenerateColumns="false" runat="server" PageSize="40" Height="16px" Width="627px" style="margin-right: 0px" >
              <Columns>
                                <asp:boundfield datafield="week" headertext="Week"/>
                                   <asp:boundfield datafield="topic" headertext="Topic"/>
                          <asp:boundfield datafield="content" headertext="Content"/>
         
                 </Columns> 
        </asp:GridView>
    <asp:Button ID="Button1" runat="server" Text="Back" class="btn-small btn-skin btn-block btn-lg"  Font-Size="14px" />
    <br />
   
   </asp:Content>

