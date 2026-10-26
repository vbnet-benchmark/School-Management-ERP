<%@ Page Title="" Language="VB" MasterPageFile="~/DigicontentApp.master" AutoEventWireup="false" CodeFile="dcourseoverview.aspx.vb" Inherits="Admin_addteacher" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     
                <h2 class="auto-style1">COURSE OVERVIEW</h2>
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
                                        <h1>Course Summary</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
      <asp:GridView ID="GridView3" AutoGenerateColumns ="False"  runat="server" GridLines="None" class="table">
             <Columns>
                   <asp:BoundField DataField="id" HeaderText="ID" Visible="False"></asp:BoundField>
                 <asp:boundfield datafield="subject" HeaderText="Subject" />
                                  <asp:boundfield datafield="class" HeaderText="Class" />

                                             <asp:BoundField DataField="overview" HeaderText="Course Overview"></asp:BoundField>
                   <asp:BoundField DataField="textbooks" HeaderText="Recommended Texts"></asp:BoundField>
                    
                 </Columns> 

        </asp:GridView></div> </div> 

     <br />
     

</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 




   
   </asp:Content>

