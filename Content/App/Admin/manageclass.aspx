<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="manageclass.aspx.vb" Inherits="Admin_newsession" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    <h2>MANAGE CLASSES</h2>

    <br />
    <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div2" style="width:100%;">
     <asp:GridView  runat="server" autogeneratecolumns ="False" ID="Gridview1" ShowHeader="False" GridLines="None" class="table" >
                     <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" Visible="False"></asp:BoundField>
                        <asp:BoundField DataField="class" HeaderText="Class"></asp:BoundField>
                        <asp:CommandField ShowSelectButton="True"  CausesValidation="False" SelectText="Details"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>

                     </Columns>

                 

                </asp:GridView></div> </div> 


   

           <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="linkBTerm" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Class" ></asp:Button></div></div> </div> </div> 

                     
    <asp:Panel id ="panel1" runat="server" Visible="False">

    <br />


     
    
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>CLASS DETAILS</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">

      
                            <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" class="table" >
                                <EditRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            </asp:DetailsView>
                            
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnEdit" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Edit Class" ></asp:Button> <asp:Button ID="lnkStudents" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="View Students" ></asp:Button></div></div> </div> </div> 


      <asp:Panel id="pnlEditClass" visible="false" runat="server">
    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class Name</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                     <asp:Textbox class="form-control" ID="txtId" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class type</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cbotype" runat="server"  >
                                                                                <asp:ListItem>Select Type</asp:ListItem>
                                                                            <asp:ListItem>K.G 1 Special</asp:ListItem>
                                                                                   <asp:ListItem>Nursery</asp:ListItem>
                                                                                   <asp:ListItem>Primary</asp:ListItem>
                                                                           <asp:ListItem>Primary 6</asp:ListItem>
                                                                                   <asp:ListItem>Junior Secondary</asp:ListItem>
                                                                                   <asp:ListItem>Senior Secondary</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Department</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cboDept" runat="server"  >
                                                                                
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>
      <div class="form-group-inner">
<div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">No Of CA</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList class = "form-control custom-select-value" ID="cboCaNo" runat="server"  >
                                                                                  <asp:ListItem>2</asp:ListItem>
                                                                                <asp:ListItem>3</asp:ListItem>
                                                                                <asp:ListItem>4</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div></div>

        <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnClassUpdate" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Update" /><asp:Button ID="btnClassCancel" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Cancel" /></div>

        </asp:Panel>



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
                                        <h1>Class Teachers</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
     <asp:GridView ID="GridView2"  AutoGenerateColumns="False" ShowHeader="False"  runat="server" GridLines="None" class="table">

            <Columns>
                   <asp:BoundField DataField="S/N" HeaderText="Staff Id"></asp:BoundField>
                 <asp:boundfield datafield="name" headertext="Name"/>
                                  
                   <asp:CommandField ShowDeleteButton="True" DeleteText="Remove"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
                                  
                 </Columns> 
            
        </asp:GridView></div> </div> 

     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="LinkButton1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Class Teacher" ></asp:Button></div></div> </div> </div> 




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
                                        <h1>Subjects and Subject Teachers</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="Div1" style="width:100%;">
     <asp:GridView ID="GridView3" AutoGenerateColumns ="False"   runat="server" GridLines="None" class="table" >
              <Columns>
                   <asp:BoundField DataField="S/N" HeaderText="S/N"></asp:BoundField>
                 <asp:boundfield datafield="subject" HeaderText="Subject" />
                                  <asp:boundfield datafield="name" HeaderText="Teacher" />

                                             <asp:BoundField DataField="periods" HeaderText="Periods"></asp:BoundField>
                   <asp:HyperLinkField DataTextField="View" DataNavigateUrlFields="subject"  DataNavigateUrlFormatString="~/content/App/Admin/subjectallocate.aspx?{0}" ><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:hyperlinkfield>
        
                   <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"><controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile " /></asp:CommandField>
        
                 </Columns> 

         

        </asp:GridView></div> </div> 


     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
   <asp:Button ID="lnkSubject" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Add Subject" ></asp:Button> <asp:Button ID="lnkTimeTable" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="View Time Table" ></asp:Button></div></div> </div> </div> 


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 

        </asp:Panel> 
                        
   
    </asp:Content>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       