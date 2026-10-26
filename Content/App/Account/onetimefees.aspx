<%@ Page Title="" Language="VB" MasterPageFile="~/Account/accountmaster.master" AutoEventWireup="false" CodeFile="onetimefees.aspx.vb" Inherits="Account_salary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" OnNextButtonClick="Wizard1_NextButtonClick" style="font-size: small" DisplaySideBar="False" Height="187px" Width="697px" FinishDestinationPageUrl="~/Account/classfees.aspx">
        <StartNavigationTemplate>
            <asp:Button ID="StartNextButton" runat="server" BackColor="White" BorderColor="#C5BBAF" BorderStyle="Solid" BorderWidth="1px" CommandName="MoveNext" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#1C5E55" Text="Next" />
        </StartNavigationTemplate>
        <StepNavigationTemplate>
            <asp:Button ID="StepPreviousButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="MovePrevious" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" Text="Previous" />
            <asp:Button ID="StepNextButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CommandName="MoveNext" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" Text="Next" />
        </StepNavigationTemplate>
        <WizardSteps>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Upload passport">
                



                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label4" runat="server" style="font-size: large; font-weight: 700; text-align: center"></asp:Label>
                <br />
                <br />
              <asp:ListBox ID="ListBox2" runat="server" Width="629px" Height="159px"></asp:ListBox>
                <br />
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Fee Type:" style="font-size: medium; font-weight: 700"></asp:Label>&nbsp;&nbsp; <asp:TextBox ID="TextBox1" runat="server" Width="222px"></asp:TextBox>
                    <asp:Label ID="Label5" runat="server" style="font-size: medium; font-weight: 700" Text="Amount:  "></asp:Label>
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                
                <asp:Button ID="Button1" runat="server" Text="Add" />
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




