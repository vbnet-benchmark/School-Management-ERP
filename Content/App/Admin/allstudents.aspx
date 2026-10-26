<%@ Page Title="" Language="VB" MasterPageFile="~/masterpage.master" AutoEventWireup="false" CodeFile="allstudents.aspx.vb" Inherits="Admin_allstudents" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
      <table style="width:100%;">
        <tr>
            <td>

                     <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    

    <asp:Label ID="lblSuccess" runat="server" style="color: #00B300; font-weight: 700"></asp:Label>
    
    

            </td>

        </tr>
        <tr style="width:100%;">
            <td>
                <h2>MANAGE STUDENTS</h2>

            </td>

                    </tr>


          <tr><td>
              <asp:TextBox ID="txtSearch" runat="server" style="width:254px;" class="form-control-small"></asp:TextBox>
&nbsp;
         <asp:Button ID="Button3" runat="server" style="padding:0px 0px 0px 0px; font-size: 14px;" runat="server" class="btn-small btn-skin btn-block btn-lg" Text="Search" />

              </td></tr>

          <tr><td>
              <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowPaging="True" PageSize="40" >
             <Columns>
        <asp:HyperLinkField DataTextField="admno" DataNavigateUrlFields="admno" DataNavigateUrlFormatString="~/Admin/studentprofile.aspx?{0}"
            HeaderText="Admission No" ItemStyle-Width = "150" >
<ItemStyle Width="150px"></ItemStyle>
                 </asp:HyperLinkField>
                 <asp:boundfield datafield="surname" readonly="true" headertext="Student's Name"/>
      <asp:boundfield datafield="sex" headertext="Sex"/>
      <asp:boundfield datafield="phone" headertext="Phone No"/>
                 <asp:ImageField DataImageUrlField="passport" HeaderText="Passport">
                 </asp:ImageField>
                 </Columns> 
        </asp:GridView>
              
              </td></tr>
          <tr><td>
              <asp:Button ID="btnPrevious" runat="server" style="padding:0px 0px 0px 0px; width:90px; font-size: 14px;" runat="server" class="btn-small btn-skin btn-block btn-lg" Text="Previous" />
                      <asp:Button ID="btnNext" runat="server" Text="Next" style="padding:0px 0px 0px 0px; width:90px; font-size: 14px;" runat="server" class="btn-small btn-skin btn-block btn-lg" />
              
              </td></tr>
          <tr><td><asp:LinkButton runat="server" ID="lnkAdd" OnClick="Unnamed1_Click">Add student</asp:LinkButton></td></tr>
        </table>


    
        
        
            
    
         </asp:Content>




