<%@ Page Title="" Language="VB" MasterPageFile="~/Account/accountmaster.master" AutoEventWireup="false" CodeFile="classfees.aspx.vb" Inherits="Account_salary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="1" OnNextButtonClick="Wizard1_NextButtonClick" style="font-size: small" DisplaySideBar="False" Height="187px" Width="863px" FinishDestinationPageUrl="~/Account/classfees.aspx">
        <StartNavigationTemplate>
            <asp:Button ID="StartNextButton" runat="server" BackColor="White" BorderColor="#C5BBAF" BorderStyle="Solid" BorderWidth="1px" CommandName="MoveNext" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#1C5E55" Text="Next" />
        </StartNavigationTemplate>
        <StepNavigationTemplate>
            <asp:Button ID="StepPreviousButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="MovePrevious" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" Text="Previous" />
            <asp:Button ID="StepNextButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CommandName="MoveNext" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" Text="Next" />
        </StepNavigationTemplate>
        <WizardSteps>
           <asp:WizardStep ID="WizardStep1" runat="server" Title="Profile">
                <h3><asp:Label ID="Label1" runat="server" Text="Select Class"></asp:Label>:&nbsp;&nbsp;
                    <asp:DropDownList ID="DropDownList2" runat="server" Width="307px">
                        <asp:ListItem Value="Select Class">Select Class</asp:ListItem>
                    </asp:DropDownList>
                </h3>
                <p>
                    &nbsp;</p>
               </asp:WizardStep>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Upload passport">
                



                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label4" runat="server" style="font-size: large; font-weight: 700; text-align: center"></asp:Label>
                <br />
                <br />
              <asp:ListBox ID="ListBox2" runat="server" Width="860px" Height="185px"></asp:ListBox>
                <br />
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Fee Type:" style="font-size: medium; font-weight: 700"></asp:Label>&nbsp;&nbsp; <asp:TextBox ID="TextBox1" runat="server" Width="149px"></asp:TextBox>
                </p>



                <p>
                    <asp:Label ID="Label5" runat="server" style="font-size: medium; font-weight: 700" Text="Amount:  "></asp:Label>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </p>
                <p>
                    <asp:Label ID="Label6" runat="server" style="font-size: medium; font-weight: 700" Text="Discountable:"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:DropDownList ID="cboDiscount" runat="server" Width="98px">
                        <asp:ListItem>Select </asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:DropDownList>
                </p>
                <p>
                    <asp:Label ID="Label3" runat="server" style="font-size: medium; font-weight: 700" Text="Minimum (%)"></asp:Label>
                    <asp:TextBox ID="txtmin" runat="server"></asp:TextBox></p>



                <p>
                    <asp:Label ID="Label7" runat="server" style="font-size: medium; font-weight: 700" Text="Discountable:"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:DropDownList ID="DropDownList1" runat="server" Width="228px" Height="16px">
                        <asp:ListItem>Select </asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:DropDownList></p>



                <p>
                    <asp:Button ID="Button1" runat="server" Text="Add" Width="82px" />
                    <asp:Button ID="Button2" runat="server" Text="Remove" />
                </p>



                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                



            </asp:WizardStep>


            

            <asp:WizardStep runat="server" Title="Finish" ID ="Wizardstep3">
                <asp:Label ID="lblFsuccess" runat="server" style="font-size: xx-large; font-weight: 700"></asp:Label>
            </asp:WizardStep>


            

           </WizardSteps>
    </asp:Wizard>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblSuccess" style="color: #00B300; font-weight: 700; font-size: medium;"></asp:Label>

            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblError" runat="server" style="color: #FF0000; font-weight: 700"></asp:Label>
    
    
    
   &nbsp;
        
    
    
    
    
   </asp:Content>




