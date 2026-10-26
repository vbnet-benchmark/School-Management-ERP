<%@ Page Title="" Language="VB" MasterPageFile="~/Student/student.master" AutoEventWireup="false" CodeFile="scorehistory.aspx.vb" Inherits="Student_scores" %>

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
        .auto-style1 {
            width: 231px;
            font-weight: bold;
            text-align: left;
        }
        .auto-style2 {
            width: 93px;
        }
        .auto-style3 {
            width: 79px;
            font-weight: 700;
        }
        .auto-style4 {
            width: 118px;
        }
        .auto-style5 {
            width: 592px;
        }
        .auto-style6 {
            width: 255px;
        }
    </style>
    <script type ="text/javascript">
        function PrintDivContent(divContainer) {
            var printContent = document.getElementById(divContainer);
            var WinPrint = window.open('', '', 'left=0,top=0,toolbar=0,sta­tus=0,buttom=0');
            WinPrint.document.write(printContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            winprint.close();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center" style=" margin-left: 129px">
         <table style="width:103%; margin-left: 0px;" align="center">
            <tr>
                <td class="auto-style3">
                    Term:</td>
                <td class="style2">
                    <asp:DropDownList ID="cboYr" runat="server" Height="26px" Width="332px" AutoPostBack="true" >
                    </asp:DropDownList>
                </td>
                <td class="style3">
                    &nbsp;</td>
                <td class="style4">
                    &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
            </tr>
        </table>

    </div>
    <div style="border-style: groove; width: 72%; margin-left: 10%;" align="center" 
        id="divContainer">
        
        <div id="divPrint">
        
        <div>
        <h1 >Score Sheet</h1>
        </div>
        <br />
        <div style="width: 614px; margin-left: 2px">
            <table style="width:100%;">
                <tr>
                    <td class="auto-style4">
                        <b>Fullname:</b></td>
                    <td class="auto-style5" align="left">
                        <asp:Label ID="lblname" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style1">
                        Admission no:</td>
                    <td class="auto-style6" align="left">
                        <asp:Label ID="lblID" runat="server"></asp:Label>
                    </td>
                    
               
                    <td class="auto-style2">
                        <b>Class:</b></td>
                    <td class="style7" align="left">
                        <asp:Label ID="lblClass" runat="server"></asp:Label>
                    </td>
                   <td rowspan="4">
                        <div style="height: 81px; width: 99px">
                            <img runat="server" alt="" src="../image/noimage.jpg" style="height: 85px; width: 98px" id="img1" /></div>
                    </td>
                </tr>
               
                <tr>
                    <td class="auto-style4">
                        &nbsp;</td>
                    <td class="auto-style5" align="left">
                        &nbsp;</td>
                    <td class="auto-style1">
                        &nbsp;</td>
                    <td class="auto-style6" align="left">
                        
                    </td>
                </tr>
               
            </table>
            
        </div>
               <br />    
        
            <asp:Chart ID="Chart1" runat="server" Palette="Bright">
                <Series>
                    <asp:Series ChartType="Line" Name="Series1" YValuesPerPoint="4">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
        <br />
        <asp:LinkButton ID="LinkButton2" runat="server"  OnClientClick="javascript:PrintDivContent('divPrint');">Print</asp:LinkButton>
        
             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        
             <asp:LinkButton ID="LinkButton1" runat="server">View Result</asp:LinkButton>
   
        <br />
        
        </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

    <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
</asp:Content>
