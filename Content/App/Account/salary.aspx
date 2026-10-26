<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="salary.aspx.vb" Inherits="Account_salary" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    
    <h2>SALARY SCHEDULE</h2>
    <br />
     <div class="form-group-inner">
<div class="row">
                                                                     <div class="col-lg-5">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="DropDownList2" runat="server" >
                                                                                <asp:ListItem>Month</asp:ListItem>
                                                                                   <asp:ListItem>January</asp:ListItem>
                                                                                   <asp:ListItem>Febuary</asp:ListItem>
                                                                                   <asp:ListItem>March</asp:ListItem>
                                                                                   <asp:ListItem>April</asp:ListItem>
                                                                                   <asp:ListItem>May</asp:ListItem>
                                                                                   <asp:ListItem>June</asp:ListItem>
                                                                                   <asp:ListItem>July</asp:ListItem>
                                                                                   <asp:ListItem>August</asp:ListItem>
                                                                                   <asp:ListItem>September</asp:ListItem>
                                                                                   <asp:ListItem>October</asp:ListItem>
                                                                                   <asp:ListItem>November</asp:ListItem>
                                                                                   <asp:ListItem>December</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>

                                                                    <div class="col-lg-4">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList cssclass = "form-control custom-select-value" ID="DropDownList3" runat="server"  >
                                                                              
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>

                   <div class="col-lg-3">
                                         <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button2" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="View Schedule" /></div>                </div> </div> 
                                                            </div>

    <asp:Panel runat="server" id="panel1" visible="false">
         <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                          <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        

                                       
                                        <h1> <asp:Label ID="Label4" runat="server" style="font-weight: 700" ></asp:Label></h1></div>
                                       
                                               <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6" style="text-align:right;">
                                      
                                           <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkPrint" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Print Schedule" ></asp:Button></div></div> </div> 
                                       </div>
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <div class="col-lg-12">
        

         <div class="comment-replay-profile"><asp:Label ID="lblStatus" runat="server" style="font-weight: 700" ></asp:Label>
                                                            <div class="btn-group">    
                                                                 
    <asp:Button ID="Button1" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Pay" ></asp:Button>     </div></div>
     </div>
     <br />
     <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True"   AutoGenerateEditButton="True" AutoGenerateColumns="False"  OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" GridLines="None" class="table" >
                <columns>
      <asp:boundfield datafield="Ref" readonly="true" headertext="Ref" Visible="False"/>
      <asp:boundfield datafield="staff Id" readonly="true" headertext="Staff Id" Visible="False"/>
      <asp:boundfield datafield="Name" readonly="true" headertext="Name"/>
      <asp:boundfield datafield="Designation" readonly="true" headertext="Designation"/>
      <asp:boundfield datafield="Amount" readonly="true" headertext="Amount"/>
      <asp:boundfield datafield="Tax" readonly="true" headertext="Tax"/>
      <asp:boundfield datafield="Pension" readonly="true" headertext="Pension"/>
                    <asp:boundfield datafield="bills" readonly="false" headertext="Bills"/>
                    <asp:boundfield datafield="deduction" readonly="false" headertext="Deduction"/>
                    <asp:boundfield datafield="increment" readonly="false" headertext="Increment"/>
                    <asp:boundfield datafield="welfare" readonly="false" headertext="Welfare"/>
<asp:boundfield datafield="balance" readonly="true" headertext="Balance"/>
      </columns>
                      <EditRowStyle CssClass="GridViewEditRow" />
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
                                        <h1>Totals</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
     <table class="table">
         <tr>
             <td>
                 <asp:Label runat="server" Text="Total Salary:"></asp:Label>
             </td>
             <td>
                 <asp:Label ID="Label5" runat="server" style="font-weight: 700" ></asp:Label>

             </td>
            </tr>
         <tr>
             <td>
                 <asp:Label runat="server" Text="Total Tax:"></asp:Label>
             </td>
             <td>
                 <asp:Label ID="Label6" runat="server" style="font-weight: 700" ></asp:Label>

             </td>
            </tr>
         <tr>
             <td>
                 <asp:Label runat="server" Text="Total Pension:"></asp:Label>
             </td>
             <td>
                 <asp:Label ID="Label7" runat="server" style="font-weight: 700" ></asp:Label>

             </td>
            </tr>
         <tr>
             <td>
                 <asp:Label runat="server" Text="Total Bills:"></asp:Label>
             </td>
             <td>
                 <asp:Label ID="Label8" runat="server" style="font-weight: 700" ></asp:Label>

             </td>
            </tr>
         <tr>
             <td>
                 <asp:Label runat="server" Text="Total Deductions:"></asp:Label>
             </td>
             <td>
                 <asp:Label ID="Label9" runat="server" style="font-weight: 700" ></asp:Label>

             </td>
            </tr>
         <tr>
             <td>
                 <asp:Label runat="server" Text="Total Increments:"></asp:Label>
             </td>
             <td>
                 <asp:Label ID="Label10" runat="server" style="font-weight: 700" ></asp:Label>

             </td>
            </tr>
         <tr>
             <td>
                 <asp:Label runat="server" Text="Total Welfare:"></asp:Label>
             </td>
             <td>
                 <asp:Label ID="Label11" runat="server" style="font-weight: 700" ></asp:Label>

             </td>
            </tr>
     </table>






</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 



         
    </asp:Panel>



                
    
    
   </asp:Content>




