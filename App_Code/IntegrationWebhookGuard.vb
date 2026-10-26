Imports System.Net

''' <summary>
''' Shared SSRF guard for every outbound integration-webhook call on
''' Content/Admin/externalintegrations.aspx.vb (Webhook Test Console, Fee Payment Webhook,
''' Attendance Alert Webhook, Integration Channel Dispatch, Partner Resource Sync) and for
''' App_Code/AttendanceWebhookService.vb, App_Code/WebhookIntegrationChannel.vb and
''' App_Code/PartnerResourceSyncJob.vb, which all call into it. Resolves the candidate
''' destination's host and rejects it unless it is a well-formed http(s) URL whose every resolved
''' address is a public (non-loopback, non-link-local, non-RFC1918-private) address -- the
''' link-local range also covers the 169.254.169.254 cloud-metadata address. A resolve-then-
''' reject approach is used rather than a fixed host allow-list, since an administrator
''' legitimately configuring a new third-party SaaS integration cannot be expected to pick
''' from a small fixed set of hosts.
''' </summary>
Public Module IntegrationWebhookGuard

    ''' <summary>
    ''' Parses candidateUrl and, only if it is an absolute http(s) URL whose host resolves to at
    ''' least one address and none of those addresses are disallowed, returns True with the
    ''' parsed Uri in safeUri. The caller must make the outbound request against safeUri (the
    ''' parsed, re-serialized value) rather than the original raw string, so nothing downstream
    ''' can re-interpret a value this check never actually looked at.
    ''' </summary>
    Public Function TryResolveSafeDestination(ByVal candidateUrl As String, ByRef safeUri As Uri) As Boolean
        safeUri = Nothing
        If String.IsNullOrWhiteSpace(candidateUrl) Then Return False

        Dim parsedUri As Uri = Nothing
        If Not Uri.TryCreate(candidateUrl, UriKind.Absolute, parsedUri) Then Return False
        If parsedUri.Scheme <> Uri.UriSchemeHttp AndAlso parsedUri.Scheme <> Uri.UriSchemeHttps Then Return False

        Try
            Dim addresses As IPAddress() = Dns.GetHostAddresses(parsedUri.DnsSafeHost)
            If addresses.Length = 0 Then Return False
            For Each address As IPAddress In addresses
                If IsDisallowedDestinationAddress(address) Then Return False
            Next
        Catch
            ' Could not resolve the host -- treat as unsafe rather than letting the request through.
            Return False
        End Try

        safeUri = parsedUri
        Return True
    End Function

    ''' <summary>
    ''' True for loopback, link-local (169.254.0.0/16, which covers the cloud-metadata address
    ''' 169.254.169.254), and the three RFC1918 private ranges -- everything an integration
    ''' destination must not be allowed to resolve to.
    ''' </summary>
    Private Function IsDisallowedDestinationAddress(ByVal address As IPAddress) As Boolean
        If IPAddress.IsLoopback(address) Then Return True

        If address.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
            Dim octets As Byte() = address.GetAddressBytes()
            If octets(0) = 10 Then Return True ' 10.0.0.0/8
            If octets(0) = 172 AndAlso octets(1) >= 16 AndAlso octets(1) <= 31 Then Return True ' 172.16.0.0/12
            If octets(0) = 192 AndAlso octets(1) = 168 Then Return True ' 192.168.0.0/16
            If octets(0) = 169 AndAlso octets(1) = 254 Then Return True ' 169.254.0.0/16 (link-local + metadata)
        End If

        Return False
    End Function

End Module
