<%@ Page Title="" Language="VB" MasterPageFile="~/Staff/staffmaster.master" AutoEventWireup="false" CodeFile="profile.aspx.vb" Inherits="Staff_profile" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link id ="link0" href="../css/normalize.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link1" href="../css/demo.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/tabs.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link3" href="../css/tabstyles.css" rel="stylesheet" type="text/css" runat="server" />
    
        <section style="margin:0,0,0,0; padding:0,0,0,0;" >
				<div class="tabs tabs-style-tzoid">
					<nav>
						<ul>
<li><a id="A1" href="mngclass.aspx" class="icon icon-home"  runat="server"><span>Class Information</span></a></li>
							<li><a id="A2" href="profile.aspx" class="icon icon-box" runat="server"><span>Students' Profile</span></a></li>
							<li><a id="A3" href="subjects.aspx" class="icon icon-upload" runat="server"><span>Subjects Offerred</span></a></li>
							<li><a id="A4" href="performance.aspx" class="icon icon-display" runat="server"><span>Performance</span></a></li>
						</ul>
					</nav>
					<div class="content-wrap">
                        <br />
                        <br />
                        <asp:Label ID="Label2" runat="server" style="font-size: small" Text="Select Student:"></asp:Label>
&nbsp;
                            <span><asp:DropDownList ID="DropDownList1" AutoPostBack="true" runat="server" Height="23px" Width="193px">
                            </asp:DropDownList></span>
						


						    <br />
                            <br />
                            <span style="display:inline";>
                            <span style="display:inline; float:right;"><asp:Image  ID="Image1" runat="server" Height="117px" Width="141px" Visible="False" /></span>
                            </span>
                            



                            <br />
                            <span style="display:inline";><asp:DetailsView ID="DetailsView1" AutoGenerateRows="true" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="0px" CellPadding="4" Height="169px" Width="504px" ForeColor="Black" GridLines="none">
                                <EditRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            </asp:DetailsView>
                            



						    </span>
                            



												
					</div><!-- /content -->
				</div><!-- /tabs -->
			</section>
   
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


