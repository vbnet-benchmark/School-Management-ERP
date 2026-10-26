<%@ Page Title="" Language="VB" MasterPageFile="~/Admin/adminmaster.master" AutoEventWireup="false" CodeFile="mngclass.aspx.vb" Inherits="Staff_mngclass"%>


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
                        <h3 style="text-align: center">
                            <asp:Label ID="Label1" runat="server"></asp:Label></h3>
                           <div id ="optional" style="float:right; background-color:transparent; font:small margin-left:29px;" runat="server"><h3 style="float:left">Class Teachers</h3>
                                                   <br />          
                                <div style="float:left">

                                    <asp:BulletedList ID="BulletedList2" runat="server"></asp:BulletedList>
                               <h3>Subject Teachers</h3>
                               <asp:BulletedList ID="BulletedList3" runat="server"></asp:BulletedList>
                               </div>
                              
                           </div>



						   <div id ="compulsory" style="float:left; background-color:transparent; font:small" runat ="server"><h3>Students</h3>
                               <asp:BulletedList ID="BulletedList1" runat="server">
                        </asp:BulletedList>
                               <h4>Add Students</h4>
                               <asp:Label ID="Label2" runat="server" Text="Admission no: "></asp:Label><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><asp:Button ID="Button1" runat="server" Text="Add" /> &nbsp;&nbsp;&nbsp; <asp:Button ID="Button2" runat="server" Text="Remove" />
                        <br />
                                <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>

                           </div>
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


