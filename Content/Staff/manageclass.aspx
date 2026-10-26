<%@ Page Title="" Language="VB" MasterPageFile="~/masterpage.master" AutoEventWireup="false" CodeFile="manageclass.aspx.vb" Inherits="Admin_newsession" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td>

                     <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
     <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    

            </td>

        </tr>
        <tr>
            <td>
                <h2 class="auto-style1">MANAGE CLASSES</h2>

            </td>

                    </tr>
        <tr>
            <td>
                <asp:GridView  runat="server" autogeneratecolumns ="False" ID="Gridview1">
                     <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="class" HeaderText="Class"></asp:BoundField>
                        <asp:CommandField ShowEditButton="True" CausesValidation="False"></asp:CommandField>
                         <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/admin/manageclass.aspx?{0}" Text="Details"></asp:HyperLinkField>
                    </Columns>

                </asp:GridView>

            </td>


        </tr>
        <tr>
                        <td>
                            <asp:LinkButton runat="server" ID="linkbterm">Add Class</asp:LinkButton>

                        </td>


                         </tr>
    </table>
    <asp:Panel id ="panel1" runat="server" Visible="False">
    <table>
        <tr><td>
            <h3>Class Details</h3>
    <div>
                            <span style="display:inline";>
                            <asp:DetailsView ID="DetailsView1" runat="server" Height="66px" Width="383px">
                            </asp:DetailsView>
                            
                                <asp:LinkButton runat="server" ID="lnkStudents">View Students</asp:LinkButton>


                            
                            



                            </span>
                            



                            <br />
                            

        <h4>Class Teachers</h4>
        <asp:GridView ID="GridView2"  AutoGenerateColumns="False" ShowHeader="False"  runat="server" Height="16px" Width="389px">

            <Columns>
                   <asp:BoundField DataField="S/N" HeaderText="Staff Id"></asp:BoundField>
                 <asp:boundfield datafield="name" headertext="Name"/>
                                  
                   <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
                                  
                 </Columns> 
        </asp:GridView>
        <asp:LinkButton runat="server" ID="LinkButton1">Add Class Teacher</asp:LinkButton>
                            <br />

                            <h4>Subject Details</h4>
        <asp:GridView ID="GridView3" AutoGenerateColumns ="False"  runat="server" Height="118px" Width="383px">
             <Columns>
                   <asp:BoundField DataField="S/N" HeaderText="S/N"></asp:BoundField>
                 <asp:boundfield datafield="subject" HeaderText="Subject" />
                                  <asp:boundfield datafield="name" HeaderText="Teacher" />

                                             <asp:BoundField DataField="periods" HeaderText="Periods"></asp:BoundField>
                   <asp:HyperLinkField DataTextField="View" DataNavigateUrlFields="subject"  DataNavigateUrlFormatString="~/Admin/subjectallocate.aspx?{0}" />
        
                 </Columns> 

        </asp:GridView>
                                    <asp:LinkButton runat="server" ID="lnkSubject">Add/Remove Subject</asp:LinkButton>

    </div>
   
    <br />



            </td></tr>




    </table>
        </asp:Panel>
                  </asp:Content>






