''' <summary>
''' Entry point for the "Partner Certificate Trust" admin feature on
''' Content/Admin/externalintegrations.aspx.vb -- checks connectivity to a partner exam-board /
''' government-portal API endpoint. Kept separate from the page's own code-behind and from the
''' actual transport (App_Code/PartnerApiTransport.vb), mirroring this project's own
''' App_Code/AttendanceWebhookService.vb precedent (Dispatch -> Send -> TransmitPayload), so a
''' future retry/backoff layer can wrap the transport without changing this entry point's own
''' signature. Connect/ConnectSafe are each two hops deep (page -> Connect -> PartnerApiTransport)
''' rather than doing the work directly, and PartnerApiTransport.Post/PostSafe stay the only place
''' in this chain that actually touches the network.
''' </summary>
Public Class PartnerIntegrationGateway

    ''' <summary>
    ''' Entry point the page calls. Delegates to PartnerApiTransport.Post -- the partner API URL
    ''' reaches Post exactly as the admin supplied it.
    ''' </summary>
    Public Shared Function Connect(ByVal partnerApiUrl As String) As String
        Return PartnerApiTransport.Post(partnerApiUrl, "{""check"":""status""}")
    End Function

    ''' <summary>
    ''' Safe twin of Connect -- routes to PartnerApiTransport.PostSafe, which leaves the default
    ''' (framework) certificate validation in place.
    ''' </summary>
    Public Shared Function ConnectSafe(ByVal partnerApiUrl As String) As String
        Return PartnerApiTransport.PostSafe(partnerApiUrl, "{""check"":""status""}")
    End Function

End Class
