<%@ Page Title="" Language="VB" MasterPageFile="~/digicontent.master" AutoEventWireup="false" CodeFile="classdetails.aspx.vb" Inherits="Admin_studentprofile" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Class Information</h1>
                                       
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
     <asp:GridView ID="GridView1"  AutoGenerateColumns="False" ShowHeader="False"  runat="server" GridLines="None" class="table">

            <Columns>
                   <asp:BoundField DataField="s/n"></asp:BoundField>
                 <asp:boundfield datafield="name" headertext="Name"/>
                                  
                   <asp:BoundField DataField="phone" HeaderText="Phone Number"></asp:BoundField>
                   <asp:ImageField DataImageUrlField="passport">
                        <controlstyle BorderStyle="None" Height="50px" Width="50px" />
                   </asp:ImageField>
                                  
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
     <asp:GridView ID="GridView2" AutoGenerateColumns ="False" ShowHeader="False"  runat="server" GridLines="None" class="table" >
             <Columns>
                   <asp:BoundField DataField="s/n" HeaderText="S/N"></asp:BoundField>
                                  <asp:boundfield datafield="subject" />

                 <asp:boundfield datafield="name" />
                                  <asp:BoundField DataField="phone" HeaderText="Phone"></asp:BoundField>
                   <asp:ImageField DataImageUrlField="passport" HeaderText="passport">
                        <controlstyle BorderStyle="None" Height="50px" Width="50px" />
                   </asp:ImageField>

                                             <asp:HyperLinkField DataNavigateUrlFields="subject"  DataNavigateUrlFormatString="~/content/Student/courseoverview.aspx?{0}" Text="Course Details" > <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" /></asp:HyperLinkField>
        
                 </Columns> 

         

        </asp:GridView></div> </div> 


     <br />
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkTimeTable" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="View Time Table" ></asp:Button></div></div> </div> </div> 


</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 


    
    </asp:Content>