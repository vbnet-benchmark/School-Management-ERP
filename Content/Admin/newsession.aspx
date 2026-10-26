<%@ Page Title="" Language="VB" MasterPageFile="~/Digidashboard.master" AutoEventWireup="false" CodeFile="newsession.aspx.vb" Inherits="Admin_newsession" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
                                  <div class="alert-title">
                                    <h1>MANAGE SESSIONS</h1>
                                </div>
                               <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="linkbterm" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="New Term" ></asp:Button></div></div> </div> </div> 
    <br />
    <div class="row">
        <div class="col-lg-4">
                <asp:GridView  runat="server" autogeneratecolumns ="False" ID="Gridview1" showheader="False" CssClass="newtable tableflat"  BorderColor="#CCCCCC" GridLines="Horizontal" BorderStyle="None" width="100%">
                     <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" visible="false"></asp:BoundField>
                        <asp:BoundField DataField="session" HeaderText="Session"></asp:BoundField>
                        <asp:CommandField ShowEditButton="True" CausesValidation="False"></asp:CommandField>
                         <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
                    </Columns>

                </asp:GridView>
    <div class="row" style="margin:1px;">
        <asp:Label ID="Label13" runat="server" Text="Search sessions:   "></asp:Label><asp:TextBox ID="txtSessionSearch" class="form-control" runat="server" ></asp:TextBox>
        <asp:Button ID="btnSessionSearch" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Search" />
    </div>
            </div>

    </div>
                                <br />

           <h4>Add new session</h4>
                                <div class="row" style="margin:1px;">
        <asp:Label ID="Label12" runat="server" Text="Session Name:   "></asp:Label><asp:TextBox ID="txtID" class="form-control" runat="server" ></asp:TextBox>
          </div>
                                <div class="login-button-pro" style="text-align:left;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Add" />

                    </div> 
                         

                                      
                           

       </asp:Content>






