<%@ Page Title="" Language="VB" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="invoice.aspx.vb" Inherits="Account_Default" %>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    

    <script src="https://js.paystack.co/v1/inline.js"></script>
   <asp:PlaceHolder ID="PlaceHolder1" runat="server">

   </asp:PlaceHolder>

     <h2>DIGITAL SCHOOL LICENCE
        
    </h2>
         <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>Licence for         <asp:Label ID="lblClass" runat="server" ></asp:Label>
    </h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
        <asp:Panel ID="pnlunpaid" runat="server">
 <div class="all-form-element-inner">
  
   
   
        <table   class ="table">
                        <tr>
                <td  runat ="server" id="feecol"  >
                   Digital School Basic Package
    </td>
                <td id="ammcol" runat ="server" style="text-align:right;"  >
                    <asp:Label runat="server" id="lblAmt"></asp:Label> 
                    </td>
                
            </tr>
            
                 <tr>
                <td  runat ="server" id="Td1"  >
                   Sub Total
    </td>
                <td id="Td2" runat ="server" style="text-align:right;"  >
                    <asp:Label runat="server" id="lblSubTotal"></asp:Label> 
                    </td>
                
            </tr>
            <tr>
                <td  >
                     VAT (7.5%)
                  </td>
                <td id="Td3" runat ="server" style="text-align:right;"  >
                    <asp:Label runat="server" id="lblVAT"></asp:Label> 
                   </td> 
                
                  
                   
                
                    </tr> 
             <tr>
                <td  >
                     Total
                  </td>
                <td id="Td4" runat ="server" style="text-align:right;"  >
                    <asp:Label runat="server" id="lblTotal"></asp:Label> 
                   </td> 
                
                  
                   
                
                    </tr> 
              <tr>
                <td  >
                     Status
                  </td>
                <td id="Td5" runat ="server" style="text-align:right;"  >
                    <asp:Label runat="server" id="lblStatus"></asp:Label> 
                   </td> 
                
                  
                   
                
                    </tr> 
        </table>
     <br />

  
     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="btnPay" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Pay Now" /></div>
</div>
            </asp:Panel>
        <asp:Panel ID="pnlresult" visible ="false"  runat="server">


            <asp:Label ID="lblResult" runat="server" Text="Label"></asp:Label>
        </asp:Panel>
        <div class="login-button-pro" style="text-align:right;" ><asp:Button ID="btnRecheckPaymentSecure" runat="server" class="btn btn-sm btn-default login-submit-cs buttonsnew" Text="Recheck Payment (Secure)" /></div>
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
                     </div> 


</asp:Content>

