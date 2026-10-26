Imports System.IO
Imports System.Net
Imports System.Text

''' <summary>
''' The actual outbound transport for the "Partner Certificate Trust" admin feature on
''' Content/Admin/externalintegrations.aspx.vb -- App_Code/PartnerIntegrationGateway.vb is the
''' only caller. Several partner exam-board / government-portal endpoints integrated with this
''' school's portal present certificates that are self-signed or chain to a root the server's
''' trust store does not carry, so Post disables certificate validation entirely for the whole
''' AppDomain rather than trusting the one partner certificate specifically -- a prior developer's
''' blanket "fix" for the resulting SSL handshake failures.
''' </summary>
Public Class PartnerApiTransport

    ''' <summary>
    ''' The only place in this chain that actually touches the network, and therefore the only
    ''' place TLS/certificate validation is configured.
    ''' </summary>
    Public Shared Function Post(ByVal partnerApiUrl As String, ByVal payload As String) As String
        ServicePointManager.ServerCertificateValidationCallback = Function(certSender, certificate, chain, sslPolicyErrors) True
        Dim req As HttpWebRequest = CType(WebRequest.Create(partnerApiUrl), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(payload)
        req.ContentLength = bodyBytes.Length
        Using reqStream As Stream = req.GetRequestStream()
            reqStream.Write(bodyBytes, 0, bodyBytes.Length)
        End Using
        ' SINK: PLANTED-VB-HR-643
        Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Return "status " & CInt(resp.StatusCode)
        End Using
    End Function

    ''' <summary>
    ''' Safe twin -- explicitly restores the framework's default certificate validation instead of
    ''' disabling it.
    ''' </summary>
    Public Shared Function PostSafe(ByVal partnerApiUrl As String, ByVal payload As String) As String
        ServicePointManager.ServerCertificateValidationCallback = Nothing
        Dim req As HttpWebRequest = CType(WebRequest.Create(partnerApiUrl), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(payload)
        req.ContentLength = bodyBytes.Length
        Using reqStream As Stream = req.GetRequestStream()
            reqStream.Write(bodyBytes, 0, bodyBytes.Length)
        End Using
        ' SAFE_SINK: PLANTED-VB-HR-643-safe
        Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Return "status " & CInt(resp.StatusCode)
        End Using
    End Function

End Class
