Imports System.Net

''' <summary>
''' Resolves a partner-portal curriculum-resource URL from a separately-supplied partner host
''' plus a relative resource path, then fetches it with a bounded number of retries -- partner
''' LMS/curriculum-sharing portals are not guaranteed reliable, so a single failed attempt should
''' not abandon the sync. Backs the "Partner Resource Sync" feature on
''' Content/Admin/externalintegrations.aspx.vb.
''' </summary>
Public Class PartnerResourceSyncJob

    Private _resolvedUrl As String

    ''' <summary>
    ''' Combines the two admin-supplied values (partner host, relative resource path) into the
    ''' URL that FetchWithRetry/FetchWithRetrySafe will later actually request.
    ''' </summary>
    Public Sub SetSource(ByVal partnerHost As String, ByVal resourcePath As String)
        _resolvedUrl = partnerHost & "/" & resourcePath
    End Sub

    ''' <summary>
    ''' Fetches _resolvedUrl, retrying up to 3 times.
    ''' </summary>
    Public Function FetchWithRetry() As Byte()
        Dim result As Byte() = Nothing
        For attempt As Integer = 1 To 3
            Try
                Dim client As New WebClient()
                ' SINK: PLANTED-VB-HR-460
                result = client.DownloadData(_resolvedUrl)
                Exit For
            Catch ex As Exception
                ' Partner portal unreachable on this attempt -- retried, not fatal until the
                ' bound above is exhausted.
            End Try
        Next
        Return result
    End Function

    ''' <summary>
    ''' Safe twin of FetchWithRetry -- validates _resolvedUrl's destination via
    ''' IntegrationWebhookGuard once, before the retry loop ever makes a request.
    ''' </summary>
    Public Function FetchWithRetrySafe() As Byte()
        Dim safeUri As Uri = Nothing
        If Not IntegrationWebhookGuard.TryResolveSafeDestination(_resolvedUrl, safeUri) Then
            Return Nothing
        End If

        Dim result As Byte() = Nothing
        For attempt As Integer = 1 To 3
            Try
                Dim client As New WebClient()
                ' SAFE_SINK: PLANTED-VB-HR-460-safe
                result = client.DownloadData(safeUri)
                Exit For
            Catch ex As Exception
                ' Partner portal unreachable on this attempt -- retried, not fatal until the
                ' bound above is exhausted.
            End Try
        Next
        Return result
    End Function

End Class
