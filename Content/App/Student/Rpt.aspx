<%@ Page Title="" Language="vb" AutoEventWireup="true" CodeFile="~/Student/Rpt.aspx.vb" MasterPageFile="~/Student/MyMaster.Master" Inherits="StaffPortal.Rpt" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 54px;
        }
        .style3
        {
            width: 32px;
        }
        .style4
        {
            width: 103px;
        }
        .style5
        {
            width: 30px;
        }
        .style6
        {
            width: 203px;
        }
        .style7
        {
            width: 243px;
        }
        .style8
        {
            width: 82px;
            font-weight: bold;
        }
    </style>
    <script type ="text/javascript">
        function PrintDivContent(divContainer) {
            var printContent = document.getElementById(divContainer);
            var WinPrint = window.open('', '', 'left=0,top=0,toolbar=0,sta­tus=0,buttom=0');
            WinPrint.document.write(printContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            return false;
            WinPrint.print();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" style="width: 508px; margin-left: 129px">

        <table style="width:100%;" align="center">
            <tr>
                <td class="style5">
                    Year</td>
                <td class="style2">
                    <asp:DropDownList ID="cboYr" runat="server">
                    <asp:ListItem>2010</asp:ListItem>
                     <asp:ListItem>2011</asp:ListItem>
                        <asp:ListItem>2012</asp:ListItem>
                        <asp:ListItem>2013</asp:ListItem>
                        <asp:ListItem>2014</asp:ListItem>
                    <asp:ListItem>2015</asp:ListItem>
                        <asp:ListItem>2016</asp:ListItem>
                        <asp:ListItem>2017</asp:ListItem>
                        <asp:ListItem>2018</asp:ListItem>
                        <asp:ListItem>2019</asp:ListItem>
                        <asp:ListItem>2020</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style3">
                    Term</td>
                <td class="style4">
                    <asp:DropDownList ID="cboTerm" runat="server">
                     <asp:ListItem>First Term</asp:ListItem>
                        <asp:ListItem>Second Term</asp:ListItem>
                        <asp:ListItem>Third Term</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="left">
                    <asp:Button ID="Button1" runat="server" Text="Generate" OnClick ="Button1_Click1" />
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    <asp:Button ID="Button2" runat="server" Text="Button" />
                </td>
            </tr>
        </table>

    </div>
    <div style="border-style: groove; height: 652px; width: 72%; margin-left: 10%;" align="center" 
        id="divContainer">
        
        <div id="divPrint">
        
        <div>
        <h1 >Prempeh College</h1>
        Terminal Report
        </div>
        <br />
        <div style="width: 614px; margin-left: 2px">
            <table style="width:100%;">
                <tr>
                    <td>
                        <b>Fullname:</b></td>
                    <td class="style7" align="left">
                        <asp:Label ID="lblname" runat="server"></asp:Label>
                    </td>
                    <td class="style8">
                        Year:</td>
                    <td class="style6" align="left">
                        <asp:Label ID="lblyr" runat="server"></asp:Label>
                    </td>
                    <td rowspan="4">
                        <div style="height: 81px; width: 99px">
                            <img alt="" src="../image/noimage.jpg" style="height: 85px; width: 98px" /></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Class:</b></td>
                    <td class="style7" align="left">
                        <asp:Label ID="lblClass" runat="server"></asp:Label>
                    </td>
                    <td class="style8">
                        Student ID</td>
                    <td class="style6" align="left">
                        <asp:Label ID="lblID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Term:</b></td>
                    <td class="style7" align="left">
                        <asp:Label ID="lblterm" runat="server"></asp:Label>
                    </td>
                    <td class="style8">
                        &nbsp;</td>
                    <td class="style6" align="left">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="style7" align="left">
                        &nbsp;</td>
                    <td class="style8">
                        &nbsp;</td>
                    <td class="style6" align="left">
                        &nbsp;</td>
                </tr>
            </table>
        </div>
            <hr />
        
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="SubID" DataSourceID="LinqDataSource1" Height="170px" 
            Width="616px">
            <Columns>
                <asp:BoundField DataField="Subject" HeaderText="Subject" 
                    SortExpression="Subject" />
                <asp:BoundField DataField="ClassScore" HeaderText="ClassScore" 
                    SortExpression="ClassScore" />
                <asp:BoundField DataField="Exams" HeaderText="Exams" SortExpression="Exams" />
                <asp:BoundField DataField="Total" HeaderText="Total" 
                    SortExpression="Total" />
                <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" 
                    SortExpression="Remarks" />
                <asp:BoundField DataField="Fullname" HeaderText="Fullname" 
                    SortExpression="Fullname" />
            </Columns>
        </asp:GridView>
            <asp:GridView ID="GridView2" runat="server">
            </asp:GridView>
        <div style="border: thin solid #C0C0C0; width: 618px">
            <table style="width:100%;">
                <tr>
                    <td colspan="3">
                        REMARKS</td>
                </tr>
                <tr>
                    <td align="left">
                        <b>Attentance</b></td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                        <b>House Master&#39;s Remarks</b></td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                        <b>Headmaster&#39;s Remarks</b></td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                        <b>Conduct:</b></td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </div>
        <br />
        <asp:LinkButton ID="LinkButton2" runat="server"  OnClientClick="javascript:PrintDivContent('divPrint');">Print</asp:LinkButton>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:PortalDBConnectionString %>" 
            
            SelectCommand="SELECT * FROM [tblSubject] WHERE (([Term] = @Term) AND ([Yr] = @Yr) AND ([StudID] = @StudID))">
            <SelectParameters>
                <asp:ControlParameter ControlID="cboTerm" Name="Term" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="cboYr" Name="Yr" PropertyName="SelectedValue" 
                    Type="String" />
                <asp:ControlParameter ControlID="Label1" Name="StudID" PropertyName="Text" 
                    Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
            <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
                ContextTypeName="StaffPortal.DataClassesDataContext" EntityTypeName="" 
                TableName="tblSubjects" 
                Where="Term == @Term &amp;&amp; Yr == @Yr &amp;&amp; StudID == @StudID">
                <WhereParameters>
                    <asp:ControlParameter ControlID="cboTerm" Name="Term" 
                        PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="cboYr" Name="Yr" PropertyName="SelectedValue" 
                        Type="String" />
                    <asp:SessionParameter Name="StudID" SessionField="StudentID" Type="String" />
                </WhereParameters>
            </asp:LinqDataSource>
        
        
        <br />
        
        </div>
</div>
</asp:Content>
