<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pinslip.aspx.vb" Inherits="Admin_Default2" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="502px" Width="696px">
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
