<%@ Page Title="" Language="VB" MasterPageFile="~/digicontentapp.master" AutoEventWireup="false" CodeFile="feeschedule.aspx.vb" Inherits="Account_Default" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     <h2>TERMLY FEE SCHEDULE
        
    </h2>
         <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>  SCHOOL FEES SCHEDULE FOR
        <asp:Label ID="lblClass" runat="server" ForeColor="#454545"></asp:Label>
    </h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
  
   
   
        <table   class ="table">
                        <tr>
                <td  runat ="server" id="feecol"  >
                     <asp:BulletedList ID="BulletedList1" runat="server" style="margin-left: 0px; margin-bottom: 0px;">
                     </asp:BulletedList>
    </td>
                <td id="ammcol" runat ="server" style="text-align:right;"  >
                    
                    </td>
                
            </tr>
            
                <tr>
                <td  runat ="server" id="Td3" >
                     <asp:CheckBoxList ID="CheckBoxList1" style="margin-left: 0px; margin-bottom: 0px;" runat="server" AutoPostBack ="true" OnSelectedIndexChanged ="CheckBoxList1_SelectedIndexChanged" Height="16px">
                     </asp:CheckBoxList>
                    </td>
                <td id="Td4" runat ="server" style="text-align:right;" >
                    
                    <asp:Label ID="lblBoarding" runat="server"></asp:Label>
                    
                    </td>
                
            </tr>
            
             <tr>
                <td  runat ="server" id="Td9" >
                     <asp:CheckBoxList ID="chkChoice"  runat="server" style="margin-left: 0px; margin-bottom: 0px;"  AutoPostBack="true" OnSelectedIndexChanged ="chkChoice_SelectedIndexChanged" >
                     </asp:CheckBoxList>
                    </td>
                <td id="Td10" runat ="server" style="text-align:right;"  >
                    
                    </td>
                
            </tr>
             <tr>
                <td  runat ="server" id="Td1" >
                     <asp:CheckBoxList ID="CheckBoxList2" style="margin-left: 0px; margin-bottom: 0px;"  runat="server" AutoPostBack="true" OnSelectedIndexChanged ="CheckBoxList2_SelectedIndexChanged" >
                     </asp:CheckBoxList>
                    </td>
                <td id="Td2" runat ="server" style="text-align:right;"  >
                    
                    </td>
                
            </tr>
            <tr>
                <td  runat ="server" id="Td5" >
                    
                     <asp:RadioButtonList ID="RadioButtonList1" style="margin-left: 0px; margin-bottom: 0px;" runat="server" AutoPostBack ="true" OnSelectedIndexChanged ="RadioButtonList1_SelectedIndexChanged" >
                     </asp:RadioButtonList>
                    </td>
                <td  runat ="server" id="Td6" style="text-align:right;"  >
                   
                    </td>
                
            </tr>
            
            <tr>
                <td  >
                     
                    <asp:Label ID="lblPayable" runat="server" Text="Total Payable current term" ></asp:Label>
                     
    </td>
                <td style="text-align:right;"  >
                    
                   
                
                    
                   
                
                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                    
                   
                
            </tr>
           <tr>
                <td  >
                     
                    <asp:Label ID="Label7" runat="server" Text="Total Payed this term" ></asp:Label>
                     
    </td>
                <td  style="text-align:right;" >
                    
                   
                 
                   
                
                    <asp:Label ID="lblPaid" runat="server" ></asp:Label>
                    
                   
                
            </tr>
             <tr>
                <td  >
                     
                    <asp:Label ID="Label3" runat="server" Text="Balance from other terms" ></asp:Label>
                     
    </td>
                <td  style="text-align:right;">
                    
                   
                
                    <asp:Label ID="lblOutstanding" runat="server" ></asp:Label>
                    
                   
                
            </tr>
          
                
           <tr id="trdue" style="font-weight:500;">
                <td >
                     
                    <asp:Label ID="Label4" runat="server"  style="font-weight:700;" Text="Total Due" ></asp:Label>
                     
    </td>
                <td  style="text-align:right;"  >
                    
                   
                
                    <asp:Label ID="lblDue" style="font-weight:700;" runat="server"></asp:Label>
                    
                   
                
            </tr>
          
          
           
             
            <tr  >
                <td  >
                     
                    <asp:Label ID="Label5" runat="server"  style="font-weight:700;" Text="Minimum Installment" ></asp:Label>
                     
    </td>
                <td  style="text-align:right;" >
                    
                  
                   
                
                    <asp:Label ID="lblInstall"  style="font-weight:700;" runat="server" ></asp:Label>
                    
                   
                
            </tr>

             <tr  >
                <td  >
                     
                    <asp:Label ID="Label1" runat="server"  style="font-weight:700;" Text="Monthly Payment Option" ></asp:Label>
                     
    </td>
                <td  style="text-align:right;" >
                    
                  
                   
                
                    <asp:Label ID="lblMonthly"  style="font-weight:700;" runat="server" ></asp:Label>
                    
                   
                
            </tr>

            <tr>
                <td  >
                     
                  </td>
                <td   >
                    
                   
                
                  
                   
                
                    </tr> 
        </table>
     <br />

    <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-2">
                                                                    <label class="login2 pull-right pull-right-pro" id="lblam" style="font-weight:700;">Amount</label>
                                                                </div>
                                                                <div class="col-lg-10">
                                                                     <asp:Textbox cssclass="form-control" ID="txtAmount" runat="server"  ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnPay" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Pay Now" /></div>
</div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 
</asp:Content>

