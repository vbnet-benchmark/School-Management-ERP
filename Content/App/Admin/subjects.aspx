<%@ Page Title="" Language="VB" MasterPageFile="~/Staff/staffmaster.master" AutoEventWireup="false" CodeFile="subjects.aspx.vb" Inherits="Staff_classmng" %>

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
                                                   <div id ="optional" style="float:right; font:small margin-left:29px;" runat="server"><h3>Optional Subjects</h3>
                               <asp:CheckBoxList ID="CheckBoxList1" runat="server" EnableViewState="true">
                            </asp:CheckBoxList>


                               <br />
                               <asp:Button ID="Button2" runat="server" Text="Update" />


    <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    
    
    
    
    
                           </div>



						   <div id ="compulsory" style="float:left; font:small" runat ="server"><h3>Subjects Offerred</h3></div>



						    



						
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


