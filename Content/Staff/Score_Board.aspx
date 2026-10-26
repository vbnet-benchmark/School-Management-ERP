<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Staff/Main.Master" CodeBehind="Score_Board.aspx.vb" Inherits="StaffPortal.Score_Board" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style7
        {
            width: 79px;
        }
        .style9
        {
            width: 80px;
        }
        .style13
        {
            width: 48px;
        }
        .style15
        {
            width: 770px;
            margin-left: 19px;
        height: 77px;
    }
        .style16
        {
            width: 62px;
        }
        .style18
        {
            width: 181px;
        }
    .style20
    {
        width: 199px;
    }
    .style21
    {
        width: 247px;
    }
    .style22
    {
        }
    .style23
    {
        width: 72px;
    }
        .style24
        {
            width: 222px;
        }
        .auto-style3 {
            width: 103px;
        }
        .auto-style4 {
            width: 109px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 652px" align="center">
    <h4 align="center" style="color: #000080">SCORE BOARD</h4>
        <div align="center" style="width: 785px">

            <table class="style15" align="center">
                <tr>
                    <td bgcolor="#CC6699" class="auto-style4" style="font-weight: bold">
                        Session</td>
                    <td bgcolor="#CC6699" style="font-weight: bold" class="auto-style3">
                        Term</td>
                    <td bgcolor="#CC6699" class="style7" style="font-weight: bold">
                        Subject</td>
                    <td bgcolor="#CC6699" class="auto-style3" style="font-weight: bold">
                        Class</td>
                    </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:DropDownList ID="DropDownList1" runat="server" 
                            DataSourceID="SqlDataSource1" DataTextField="StudentID" 
                            DataValueField="StudentID">
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style3">
                        <asp:DropDownList ID="DropDownList2" runat="server" Height="16px" Width="104px">
                          
                             <asp:ListItem>Economics </asp:ListItem>
 <asp:ListItem>Geography </asp:ListItem>
 <asp:ListItem>Government </asp:ListItem>
 <asp:ListItem>History </asp:ListItem>
 <asp:ListItem>Christian Rel Studies </asp:ListItem>
 <asp:ListItem>Islamic Rel Studies </asp:ListItem>
 <asp:ListItem>Literature in English </asp:ListItem>
 <asp:ListItem>French </asp:ListItem>
 <asp:ListItem>Gagaare </asp:ListItem>
 <asp:ListItem>Physics </asp:ListItem>
 <asp:ListItem>Chemistry </asp:ListItem>
 <asp:ListItem>Biology </asp:ListItem>
 <asp:ListItem>Mathematics (Elective) </asp:ListItem>
 <asp:ListItem>Music </asp:ListItem>
 <asp:ListItem>Twi(Asante) </asp:ListItem>
 <asp:ListItem>Twi(Akuapem) </asp:ListItem>
 <asp:ListItem>Nzema </asp:ListItem>
 <asp:ListItem>Kasem </asp:ListItem>
 <asp:ListItem>Gonja </asp:ListItem>
 <asp:ListItem>Ga </asp:ListItem>
 <asp:ListItem>Fante </asp:ListItem>
 <asp:ListItem>Ewe </asp:ListItem>
 <asp:ListItem>Dangme </asp:ListItem>
 <asp:ListItem>Dagbani </asp:ListItem>
 <asp:ListItem>Clothing & Textiles </asp:ListItem>
 <asp:ListItem>Food & Nutrition </asp:ListItem>
 <asp:ListItem>Management in Living </asp:ListItem>
 <asp:ListItem>Basketry </asp:ListItem>
 <asp:ListItem>Ceramics </asp:ListItem>
 <asp:ListItem>General Knowledge in Art </asp:ListItem>
 <asp:ListItem>Graphic Design </asp:ListItem>
 <asp:ListItem>Jewellery </asp:ListItem>
 <asp:ListItem>Leather </asp:ListItem>
 <asp:ListItem>Picture Making </asp:ListItem>
 <asp:ListItem>Sculpture </asp:ListItem>
 <asp:ListItem>Textiles </asp:ListItem>
 <asp:ListItem>General Agric </asp:ListItem>
 <asp:ListItem>Animal Husbandry </asp:ListItem>
 <asp:ListItem>Crop Husbandry and Hort </asp:ListItem>
 <asp:ListItem>Fisheries </asp:ListItem>
 <asp:ListItem>Forestry </asp:ListItem>
 <asp:ListItem>Business Management </asp:ListItem>
 <asp:ListItem>Clerical Office Duties </asp:ListItem>
 <asp:ListItem>Financial Accounting </asp:ListItem>
 <asp:ListItem>Principles of Cost Accounting </asp:ListItem>
 <asp:ListItem>Typewriting </asp:ListItem>
 <asp:ListItem>Applied Electricity </asp:ListItem>
 <asp:ListItem>Auto Mechanics </asp:ListItem>
 <asp:ListItem>Building Construction </asp:ListItem>
 <asp:ListItem>Electronics </asp:ListItem>
 <asp:ListItem>Metal Works </asp:ListItem>
 <asp:ListItem>Technical Drawing </asp:ListItem>
 <asp:ListItem>Wood Work </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style7">
                        <asp:TextBox ID="txtClass" runat="server" Width="40%">0</asp:TextBox>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtExam" runat="server" Width="40%" AutoPostBack="True">0</asp:TextBox>
                    </td>
                    </tr>
                </table>
                        <asp:Button ID="Button2" runat="server" Text="Search" />
                    <br />
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <img src="../image/loading36.gif" style="height: 31px; width: 36px" />
                Processing....
                </ProgressTemplate>
            </asp:UpdateProgress>
            <h3 align="center" style="background-color: #FFFFFF; color: #000080;">Report Summary</h3>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" Height="184px" Width="100%" 
                DataSourceID="SqlDataSource2" AllowPaging="True" 
                AutoGenerateColumns="False" DataKeyNames="SubID" Font-Size="Small" 
                    ForeColor="#003366">
                <Columns>
                    <asp:BoundField DataField="Fullname" HeaderText="Fullname" 
                        SortExpression="Fullname" />
                    <asp:BoundField DataField="Term" HeaderText="Term" SortExpression="Term" />
                    <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" />
                    <asp:BoundField DataField="Yr" HeaderText="Year" SortExpression="Yr" />
                    <asp:BoundField DataField="Subject" HeaderText="Subject" 
                        SortExpression="Subject" />
                    <asp:BoundField DataField="ClassScore" HeaderText="Class Score" 
                        SortExpression="ClassScore" />
                    <asp:BoundField DataField="Exams" HeaderText="Exams" SortExpression="Exams" />
                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                    <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" 
                        SortExpression="Remarks" />
                </Columns>
        </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:PortalDBConnectionString %>" 
                SelectCommand="SELECT * FROM [tblSubject] WHERE ([StaffID] = @StaffID) ORDER BY [Subject]">
                <SelectParameters>
                    <asp:SessionParameter Name="StaffID" SessionField="StaffID" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:PortalDBConnectionString %>" 
            SelectCommand="SELECT [StudentID] FROM [tblEnroll]">
        </asp:SqlDataSource>
        </ContentTemplate>
            </asp:UpdatePanel>
    </div>
    
</asp:Content>
