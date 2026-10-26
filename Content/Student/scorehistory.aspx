<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="scorehistory.aspx.vb" Inherits="Student_scores" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
<h2>PERFORMANCE HISTORY</h2>
    
        
       <div class="charts-area mg-b-15">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="charts-single-pro shadow-reset nt-mg-b-30">
                                <div class="alert-title">
                                    <h2>Basic Line Chart</h2>
                                    <p>A bar chart provides a way of showing data values. It is sometimes used to show trend data. we create a bar chart for a single dataset and render that in our page.</p>
                                </div>
                                <div id="basic-chart">
                                    <canvas id="basiclinechart"></canvas>
                                </div>
                            </div>
                        </div>
                        </div> 
                    </div> 
           </div> 
       
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
                            <img runat="server" alt="" src="../image/noimage.jpg" style="height: 85px; width: 90px; margin-left: 0px;" id="img1" /></div>
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
    <asp:CheckBoxList ID="chkSubjects" runat="server" autopostback="true"></asp:CheckBoxList><br />    
        
            <asp:Chart ID="Chart1" runat="server" Palette="Bright" width="580px" Height="450px">
                
                <Legends>
        <asp:Legend Alignment="Center" Docking="right" IsTextAutoFit="true" Name="Default"
            LegendStyle="Row" />
    </Legends>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
       


</asp:Content>
