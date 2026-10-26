Imports System.IO
Imports System.Net
Imports System.Text

''' <summary>
''' The "webhook" integration channel -- posts the message to an external destination URL over
''' HTTP(S). Selected by IntegrationChannelRosterService.GetChannel whenever the caller requests
''' channel type "webhook" (also the fallback for any unrecognized/unsupplied type).
''' </summary>
Public Class WebhookIntegrationChannel
    Implements IIntegrationChannel

    ''' <summary>
    ''' The destination is used exactly as supplied by the caller -- reached only through the
    ''' IIntegrationChannel reference the roster hands back, so which concrete Send this call
    ''' ends up in depends on the requested channel type.
    ''' </summary>
    Public Function Send(destination As String, message As String) As String Implements IIntegrationChannel.Send
        Dim req As HttpWebRequest = CType(WebRequest.Create(destination), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes("{""message"":""" & message & """}")
        req.ContentLength = bodyBytes.Length
        Using reqStream As Stream = req.GetRequestStream()
            reqStream.Write(bodyBytes, 0, bodyBytes.Length)
        End Using
        ' SINK: PLANTED-VB-HR-459
        Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Return "status " & CInt(resp.StatusCode)
        End Using
    End Function

    ''' <summary>
    ''' Safe counterpart: the destination is validated against IntegrationWebhookGuard before the
    ''' request is ever built.
    ''' </summary>
    Public Function SendSafe(destination As String, message As String) As String Implements IIntegrationChannel.SendSafe
        Dim safeUri As Uri = Nothing
        If Not IntegrationWebhookGuard.TryResolveSafeDestination(destination, safeUri) Then
            Return Nothing
        End If

        Dim req As HttpWebRequest = CType(WebRequest.Create(safeUri), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes("{""message"":""" & message & """}")
        req.ContentLength = bodyBytes.Length
        Using reqStream As Stream = req.GetRequestStream()
            reqStream.Write(bodyBytes, 0, bodyBytes.Length)
        End Using
        ' SAFE_SINK: PLANTED-VB-HR-459-safe
        Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Return "status " & CInt(resp.StatusCode)
        End Using
    End Function
End Class
