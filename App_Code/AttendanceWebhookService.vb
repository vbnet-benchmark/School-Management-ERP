Imports System.IO
Imports System.Net
Imports System.Text

''' <summary>
''' Cross-file service backing the "Attendance Alert Webhook" feature on
''' Content/Admin/externalintegrations.aspx.vb. Kept separate from the page's own code-behind,
''' mirroring this project's own App_Code/StudentImportService.vb precedent, so the same
''' attendance-alert dispatch can later be reused by a scheduled background job (e.g. a nightly
''' absentee sweep) without duplicating it on the admin page. Dispatch/DispatchSafe are each
''' three hops deep (page -> Dispatch -> Send -> TransmitPayload) rather than doing the work
''' directly, so a future retry/queueing layer can wrap Send without changing Dispatch's own
''' signature, and TransmitPayload stays the single place that actually touches the network.
''' </summary>
Public Class AttendanceWebhookService

    ''' <summary>
    ''' Entry point the page calls. Delegates to Send -- the destination URL reaches Send, then
    ''' TransmitPayload, exactly as the caller supplied it.
    ''' </summary>
    Public Shared Function Dispatch(ByVal destinationUrl As String, ByVal studentName As String, ByVal message As String) As String
        Return Send(destinationUrl, studentName, message)
    End Function

    ''' <summary>
    ''' Builds the JSON payload, then hands the destination URL -- still exactly as the caller
    ''' supplied it -- to TransmitPayload, which is the only place in this chain that actually
    ''' touches the network.
    ''' </summary>
    Private Shared Function Send(ByVal destinationUrl As String, ByVal studentName As String, ByVal message As String) As String
        Dim payload As String = "{""student"":""" & studentName & """,""alert"":""" & message & """}"
        Return TransmitPayload(destinationUrl, payload)
    End Function

    ''' <summary>
    ''' The actual outbound call -- the one place in this chain that reaches the network.
    ''' </summary>
    Private Shared Function TransmitPayload(ByVal destinationUrl As String, ByVal payload As String) As String
        Dim req As HttpWebRequest = CType(WebRequest.Create(destinationUrl), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(payload)
        req.ContentLength = bodyBytes.Length
        Using reqStream As Stream = req.GetRequestStream()
            reqStream.Write(bodyBytes, 0, bodyBytes.Length)
        End Using
        ' SINK: PLANTED-VB-HR-458
        Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Return "status " & CInt(resp.StatusCode)
        End Using
    End Function

    ''' <summary>
    ''' Safe twin of Dispatch -- validates the destination via IntegrationWebhookGuard before the
    ''' chain ever reaches SendSafe/TransmitPayloadSafe.
    ''' </summary>
    Public Shared Function DispatchSafe(ByVal destinationUrl As String, ByVal studentName As String, ByVal message As String) As String
        Dim safeUri As Uri = Nothing
        If Not IntegrationWebhookGuard.TryResolveSafeDestination(destinationUrl, safeUri) Then
            Return Nothing
        End If
        Return SendSafe(safeUri, studentName, message)
    End Function

    Private Shared Function SendSafe(ByVal safeUri As Uri, ByVal studentName As String, ByVal message As String) As String
        Dim payload As String = "{""student"":""" & studentName & """,""alert"":""" & message & """}"
        Return TransmitPayloadSafe(safeUri, payload)
    End Function

    Private Shared Function TransmitPayloadSafe(ByVal safeUri As Uri, ByVal payload As String) As String
        Dim req As HttpWebRequest = CType(WebRequest.Create(safeUri), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(payload)
        req.ContentLength = bodyBytes.Length
        Using reqStream As Stream = req.GetRequestStream()
            reqStream.Write(bodyBytes, 0, bodyBytes.Length)
        End Using
        ' SAFE_SINK: PLANTED-VB-HR-458-safe
        Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Return "status " & CInt(resp.StatusCode)
        End Using
    End Function

End Class
