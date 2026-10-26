<%@ Page Title="" Language="VB" MasterPageFile="~/DigicontentApp.master" AutoEventWireup="false" CodeFile="dtimetable.aspx.vb" Inherits="Admin_staffprofile" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
        <h2>TIME TABLE</h2>
    <br />
    <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-1">
                                                                    <label class="login2 pull-right pull-right-pro">Subordinate</label>
                                                                </div>
                                                                <div class="col-lg-11">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="DropDownList1" runat="server" autopostback="true" >
                                                                               
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
               <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Subject Allocation</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
      <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="None" class="table" >
            <Columns>
                <asp:BoundField DataField="subject" HeaderText="Subject"></asp:BoundField>
                <asp:BoundField DataField="class" HeaderText="Class"></asp:BoundField>
                <asp:BoundField DataField="periods" HeaderText="Periods"></asp:BoundField>
            </Columns>
            
        </asp:GridView></div> </div> 
                     






</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

   <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Daily Schedule</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro">Day</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="cboDay" runat="server" autopostback="true" >
                                                                                <asp:ListItem Value="Select Day"></asp:ListItem>
                                                                                     <asp:ListItem Value="Monday"></asp:ListItem>
                                                                                     <asp:ListItem Value="Tuesday"></asp:ListItem>
                                                                                     <asp:ListItem Value="Wednesday"></asp:ListItem>
                                                                                     <asp:ListItem Value="Thursday"></asp:ListItem>
                                                                                     <asp:ListItem Value="Friday"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
     <br />
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
       <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" GridLines="None" class="table"  >
            <Columns>
                <asp:BoundField DataField="time" HeaderText="Time"></asp:BoundField>
                <asp:BoundField DataField="subject" HeaderText="Subject"></asp:BoundField>
                <asp:BoundField DataField="class" HeaderText="Class"></asp:BoundField>
            </Columns>
          
        </asp:GridView>   </div> </div> 


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 





                      
      
    </asp:Content>