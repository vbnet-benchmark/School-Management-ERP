<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/DigicontentApp.master" CodeFile="viewtimetable.aspx.vb" Inherits="Default2"  %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>SCHOOL TIME TABLE</h2>

                 <h4>
                     <asp:label runat="server" ID="lblTimetable"></asp:label>
                 </h4>
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back To Class" ></asp:Button></div></div> </div> </div> 
    <br />
     <div class="form-group-inner">
<div class="row">
                                                               
                                                                <div class="col-lg-12">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList AutoPostBack="true"  cssclass = "form-control custom-select-value" ID="cboDay" runat="server"  >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
          
         
      <br />
      <asp:Panel ID="Panel1" runat="server">

          <asp:GridView ID="GridView1" AutoGenerateColumns ="False"   runat="server" GridLines="vertical" class="table">
             
          </asp:GridView>
      </asp:Panel>
      <br />
              <h4>Subject Teachers</h4>
    <br />
    <asp:GridView ID="GridView3" AutoGenerateColumns ="False"  runat="server"  ShowFooter="True" GridLines="None" class="table">
             <Columns>
                   <asp:BoundField DataField="S/N" HeaderText="S/N"></asp:BoundField>
                 <asp:boundfield datafield="subject" HeaderText="Subject" />
                                  <asp:BoundField DataField="alias" HeaderText="Alias"></asp:BoundField>
                                  <asp:boundfield datafield="name" HeaderText="Teacher" />

                                             <asp:BoundField DataField="periods" HeaderText="Periods"></asp:BoundField>
        
                 </Columns> 

        </asp:GridView>
    
      
       </asp:Content> 