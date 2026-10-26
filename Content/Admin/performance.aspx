<%@ Page Title="" Language="VB" MasterPageFile="~/Staff/staffmaster.master" AutoEventWireup="false" CodeFile="performance.aspx.vb" Inherits="Staff_performance" %>

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
                            </asp:DropDownList><asp:Label ID="Label1" runat="server" style="font-size: small" Text="Select Term:"></asp:Label><asp:DropDownList ID="cboYr" runat="server" Height="26px" Width="332px" AutoPostBack="true" >
                    </asp:DropDownList></span>
						

						
						     <span style="display:inline;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <br />
                            <br />
                        </span>



						    <asp:GridView ID="GridView2" AutoGenerateColumns="false" runat="server" Height="16px" Width="436px">
                                <columns>
      <asp:boundfield datafield="subject" readonly="true" headertext="Subject"/>
      <asp:boundfield datafield="CA1" headertext="1st CA"/>
      <asp:boundfield datafield="CA2" headertext="2nd CA"/>
      <asp:boundfield datafield="CA3" headertext="3rd CA"/>
      <asp:boundfield datafield="Project" headertext="Project"/>
      <asp:boundfield datafield="testtotal" readonly="true" headertext="Total CA"/>
      <asp:boundfield datafield="Examination" headertext="Exams"/>
      <asp:boundfield datafield="Total" readonly="true" headertext="Term Total"/>
      <asp:boundfield datafield="avg" readonly="true" headertext="Subject Average"/>
      <asp:boundfield datafield="highest" readonly="true" headertext="Highest"/>
      <asp:boundfield datafield="lowest" readonly="true" headertext="Lowest"/>
      <asp:boundfield datafield="Grade" readonly="true" headertext="Grade"/>
      <asp:boundfield datafield="Remarks" readonly="true" headertext="Remarks"/>
      <asp:boundfield datafield="pos" readonly="true" headertext="Position"/>
      </columns>
                            </asp:GridView>



						    <br />
                            <asp:Label ID="Label3" runat="server" style="font-size: small; font-weight: 700" Text="Hand Writing:"></asp:Label>
&nbsp;
                            <asp:TextBox ID="txtHandwriting" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;  <asp:Label ID="Label13" runat="server" style="font-size: small; font-weight: 700" Text="Punctuality:"></asp:Label>
&nbsp;&nbsp;
                            <asp:TextBox ID="txtPunctual" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label30" runat="server" style="font-size: small; font-weight: 700" Text="Self Control:"></asp:Label>
&nbsp;&nbsp;
                            <asp:TextBox ID="txtSelfControl" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;<br />
                        <asp:Label ID="Label4" runat="server" style="font-size: small; font-weight: 700" Text="Fluency:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;
                            <asp:TextBox ID="txtFluency" runat="server" Width="18px"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:Label ID="Label14" runat="server" style="font-size: small; font-weight: 700" Text="Attendance:"></asp:Label>
&nbsp;&nbsp;
                            <asp:TextBox ID="txtAttendance" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;  <asp:Label ID="Label29" runat="server" style="font-size: small; font-weight: 700" Text="Cooperation:"></asp:Label>
&nbsp;&nbsp;
                            <asp:TextBox ID="txtCooperate" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;<br />
                         <asp:Label ID="Label5" runat="server" style="font-size: small; font-weight: 700" Text="Games:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;
                            <asp:TextBox ID="txtGames" runat="server" Width="17px"></asp:TextBox>
                         &nbsp;&nbsp;
                         <asp:Label ID="Label15" runat="server" style="font-size: small; font-weight: 700" Text="Reliability:"></asp:Label>
&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtReliability" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label28" runat="server" style="font-size: small; font-weight: 700" Text="Responsibility:"></asp:Label>
&nbsp;
                            <asp:TextBox ID="txtResponsible" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;<br />
                        <asp:Label ID="Label6" runat="server" style="font-size: small; font-weight: 700" Text="Sports:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtSports" runat="server" Width="17px"></asp:TextBox>
                         &nbsp;&nbsp;
                         <asp:Label ID="Label16" runat="server" style="font-size: small; font-weight: 700" Text="Neatness:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtNeatness" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label27" runat="server" style="font-size: small; font-weight: 700" Text="Attentiveness:"></asp:Label>
&nbsp;
                            <asp:TextBox ID="txtAttentive" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;<br />
                        <asp:Label ID="Label7" runat="server" style="font-size: small; font-weight: 700" Text="Gymnastics:"></asp:Label>
&nbsp;&nbsp;
                            <asp:TextBox ID="txtGymnastics" runat="server" Width="17px"></asp:TextBox>
                         &nbsp;&nbsp;
                         <asp:Label ID="Label17" runat="server" style="font-size: small; font-weight: 700" Text="Politeness:"></asp:Label>
&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPolite" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label26" runat="server" style="font-size: small; font-weight: 700" Text="Initiative:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtInitiative" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;<br />
                        <asp:Label ID="Label8" runat="server" style="font-size: small; font-weight: 700" Text="Tools:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txttools" runat="server" Width="17px"></asp:TextBox>
                         &nbsp;&nbsp;
                         <asp:Label ID="Label18" runat="server" style="font-size: small; font-weight: 700" Text="Honesty:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtHonest" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label19" runat="server" style="font-size: small; font-weight: 700" Text="Organization:"></asp:Label>
&nbsp;
                            <asp:TextBox ID="txtOrganized" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;<br />
                        <asp:Label ID="Label9" runat="server" style="font-size: small; font-weight: 700" Text="Drawing:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtDrawing" runat="server" Width="17px"></asp:TextBox>
                         &nbsp;&nbsp;
                         <asp:Label ID="Label20" runat="server" style="font-size: small; font-weight: 700" Text="Relationship:"></asp:Label>
&nbsp;
                            <asp:TextBox ID="txtRelate" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp; <asp:Label ID="Label21" runat="server" style="font-size: small; font-weight: 700" Text="Perseerance:"></asp:Label>
&nbsp;
                            <asp:TextBox ID="txtPersevere" runat="server" Width="17px"></asp:TextBox>
&nbsp;&nbsp;<br />
                        <asp:Label ID="Label10" runat="server" style="font-size: small; font-weight: 700" Text="Crafts:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtCrafts" runat="server" Width="17px"></asp:TextBox>
                         &nbsp;&nbsp;
                         &nbsp;
                            &nbsp;&nbsp; <br />
                        <asp:Label ID="Label11" runat="server" style="font-size: small; font-weight: 700" Text="Musical skills"></asp:Label>
&nbsp;
                            <asp:TextBox ID="txtMusical" runat="server" Width="17px"></asp:TextBox>
                        &nbsp;&nbsp;
                        &nbsp;
                            &nbsp;&nbsp; 
&nbsp;
                            &nbsp;&nbsp; <br />
                        <br />
                         <asp:Label ID="Label12" runat="server" style="font-size: small; font-weight: 700" Text="Class Teacher's Remarks"></asp:Label>
&nbsp;<asp:TextBox ID="txtRem" runat="server" Width="533px"></asp:TextBox>
      &nbsp;<br />
                                      
                           <asp:Button ID="Button1" runat="server" Text="Update" />



						 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button2" runat="server" Text="Class Result" />
&nbsp;<br />
                                <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700; font-size: medium;"></asp:Label>
    
    
    
   &nbsp;
    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700; font-size: medium;"></asp:Label>

					    <br />



						
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


