<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/digidashboard.master" CodeFile="sviewtimetable.aspx.vb" Inherits="Default2"  %>
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
          
         
      <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Time Table</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
      <br />
      <asp:Panel ID="Panel1" runat="server">

          <asp:GridView ID="GridView1" AutoGenerateColumns ="False"   runat="server" GridLines="vertical" class="newtable">
             
          </asp:GridView>
      </asp:Panel>
                                                 </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
      <br />
     <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Subject Teachers</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
             
    <br />
    <asp:GridView ID="GridView3" AutoGenerateColumns ="False"  runat="server"  ShowFooter="True" GridLines="None" class="newtable" >
             <Columns>
                   <asp:BoundField DataField="S/N" HeaderText="S/N"></asp:BoundField>
                 <asp:boundfield datafield="subject" HeaderText="Subject" />
                                  <asp:BoundField DataField="alias" HeaderText="Alias"></asp:BoundField>
                                  <asp:boundfield datafield="name" HeaderText="Teacher" />

                                             <asp:BoundField DataField="periods" HeaderText="Periods"></asp:BoundField>
        
                 </Columns> 

        </asp:GridView>
    
        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
    
      
       </asp:Content> 
   

