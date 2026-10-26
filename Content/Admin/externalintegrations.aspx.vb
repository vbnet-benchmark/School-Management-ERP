Imports System.IO
Imports System.Net
Imports System.Text

Partial Class Admin_externalintegrations
    Inherits System.Web.UI.Page

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder

    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
    End Sub

    ' ==== Direct tier: the destination URL is used exactly as entered, in this same method. ====

    Protected Sub btnTestWebhookDirect_Click(sender As Object, e As EventArgs)
        Try
            Dim destinationUrl As String = txtWebhookUrl.Text
            Dim req As HttpWebRequest = CType(WebRequest.Create(destinationUrl), HttpWebRequest)
            req.Method = "POST"
            req.ContentType = "application/json"
            Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes("{""event"":""test-ping""}")
            req.ContentLength = bodyBytes.Length
            Using reqStream As Stream = req.GetRequestStream()
                reqStream.Write(bodyBytes, 0, bodyBytes.Length)
            End Using
            ' SINK: PLANTED-VB-HR-456
            Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Show_Alert(True, "Webhook test ping sent, status " & CInt(resp.StatusCode))
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnTestWebhookDirectSafe_Click(sender As Object, e As EventArgs)
        Try
            Dim destinationUrl As String = txtWebhookUrl.Text
            Dim safeUri As Uri = Nothing
            If Not IntegrationWebhookGuard.TryResolveSafeDestination(destinationUrl, safeUri) Then
                Show_Alert(False, "Webhook destination rejected: must be a public http(s) host.")
                Exit Sub
            End If
            Dim req As HttpWebRequest = CType(WebRequest.Create(safeUri), HttpWebRequest)
            req.Method = "POST"
            req.ContentType = "application/json"
            Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes("{""event"":""test-ping""}")
            req.ContentLength = bodyBytes.Length
            Using reqStream As Stream = req.GetRequestStream()
                reqStream.Write(bodyBytes, 0, bodyBytes.Length)
            End Using
            ' SAFE_SINK: PLANTED-VB-HR-456-safe
            Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Show_Alert(True, "Webhook test ping sent, status " & CInt(resp.StatusCode))
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' ==== Indirect tier: a same-class private helper performs the request, with the destination
    ' URL passed through to it exactly as entered. ====

    Private Function PostFeeWebhookPing(ByVal destinationUrl As String, ByVal payload As String) As String
        Dim req As HttpWebRequest = CType(WebRequest.Create(destinationUrl), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(payload)
        req.ContentLength = bodyBytes.Length
        Using reqStream As Stream = req.GetRequestStream()
            reqStream.Write(bodyBytes, 0, bodyBytes.Length)
        End Using
        ' SINK: PLANTED-VB-HR-457
        Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Return "status " & CInt(resp.StatusCode)
        End Using
    End Function

    Private Function PostFeeWebhookPingSafe(ByVal destinationUrl As String, ByVal payload As String) As String
        Dim safeUri As Uri = Nothing
        If Not IntegrationWebhookGuard.TryResolveSafeDestination(destinationUrl, safeUri) Then
            Return Nothing
        End If
        Dim req As HttpWebRequest = CType(WebRequest.Create(safeUri), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "application/json"
        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(payload)
        req.ContentLength = bodyBytes.Length
        Using reqStream As Stream = req.GetRequestStream()
            reqStream.Write(bodyBytes, 0, bodyBytes.Length)
        End Using
        ' SAFE_SINK: PLANTED-VB-HR-457-safe
        Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Return "status " & CInt(resp.StatusCode)
        End Using
    End Function

    Protected Sub btnTestFeeWebhook_Click(sender As Object, e As EventArgs)
        Try
            Dim destinationUrl As String = txtFeeWebhookUrl.Text
            Dim result As String = PostFeeWebhookPing(destinationUrl, "{""event"":""fee-payment-test""}")
            Show_Alert(True, "Fee webhook test sent, " & result)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnTestFeeWebhookSafe_Click(sender As Object, e As EventArgs)
        Try
            Dim destinationUrl As String = txtFeeWebhookUrl.Text
            Dim result As String = PostFeeWebhookPingSafe(destinationUrl, "{""event"":""fee-payment-test""}")
            If result Is Nothing Then
                Show_Alert(False, "Fee webhook destination rejected: must be a public http(s) host.")
            Else
                Show_Alert(True, "Fee webhook test sent, " & result)
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' ==== Interprocedural tier: routed through a cross-file, multi-hop service class
    ' (App_Code/AttendanceWebhookService.vb: Dispatch -> Send -> TransmitPayload). ====

    Protected Sub btnSendAttendanceAlert_Click(sender As Object, e As EventArgs)
        Try
            Dim destinationUrl As String = txtAttendanceWebhookUrl.Text
            Dim result As String = AttendanceWebhookService.Dispatch(destinationUrl, "Test Student", "This is a test attendance alert.")
            Show_Alert(True, "Attendance alert dispatched, " & result)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSendAttendanceAlertSafe_Click(sender As Object, e As EventArgs)
        Try
            Dim destinationUrl As String = txtAttendanceWebhookUrl.Text
            Dim result As String = AttendanceWebhookService.DispatchSafe(destinationUrl, "Test Student", "This is a test attendance alert.")
            If result Is Nothing Then
                Show_Alert(False, "Attendance webhook destination rejected: must be a public http(s) host.")
            Else
                Show_Alert(True, "Attendance alert dispatched, " & result)
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' ==== Type/polymorphism-dependent tier: a single shared call site, roster-resolved
    ' (App_Code/IIntegrationChannel.vb, App_Code/IntegrationChannelRosterService.vb). ====

    Protected Sub btnDispatchChannel_Click(sender As Object, e As EventArgs)
        Try
            Dim result As String = IntegrationChannelRosterService.Dispatch(txtChannelType.Text, txtChannelDestination.Text, txtChannelMessage.Text)
            Show_Alert(True, "Channel dispatch result: " & result)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnDispatchChannelSafe_Click(sender As Object, e As EventArgs)
        Try
            Dim result As String = IntegrationChannelRosterService.DispatchSafe(txtChannelType.Text, txtChannelDestination.Text, txtChannelMessage.Text)
            If result Is Nothing Then
                Show_Alert(False, "Channel destination rejected: must be a public http(s) host.")
            Else
                Show_Alert(True, "Channel dispatch result: " & result)
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' ==== Diversity tier: field-store-then-bounded-retry-loop
    ' (App_Code/PartnerResourceSyncJob.vb: SetSource then FetchWithRetry). ====

    Protected Sub btnSyncResource_Click(sender As Object, e As EventArgs)
        Try
            Dim job As New PartnerResourceSyncJob()
            job.SetSource(txtPartnerHost.Text, txtResourcePath.Text)
            Dim data As Byte() = job.FetchWithRetry()
            If data Is Nothing Then
                Show_Alert(False, "Partner resource sync failed after 3 attempts.")
            Else
                Show_Alert(True, "Partner resource synced, " & data.Length.ToString() & " byte(s).")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSyncResourceSafe_Click(sender As Object, e As EventArgs)
        Try
            Dim job As New PartnerResourceSyncJob()
            job.SetSource(txtPartnerHost.Text, txtResourcePath.Text)
            Dim data As Byte() = job.FetchWithRetrySafe()
            If data Is Nothing Then
                Show_Alert(False, "Partner resource destination rejected or unreachable after 3 attempts.")
            Else
                Show_Alert(True, "Partner resource synced, " & data.Length.ToString() & " byte(s).")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' ==== Interprocedural tier: routed through a cross-file, multi-hop service class
    ' (App_Code/PartnerIntegrationGateway.vb: Connect -> App_Code/PartnerApiTransport.vb: Post). ====

    Protected Sub btnCheckPartnerApi_Click(sender As Object, e As EventArgs)
        Try
            Dim result As String = PartnerIntegrationGateway.Connect(txtPartnerCertApiUrl.Text)
            Show_Alert(True, "Partner API reachable, " & result)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnCheckPartnerApiSafe_Click(sender As Object, e As EventArgs)
        Try
            Dim result As String = PartnerIntegrationGateway.ConnectSafe(txtPartnerCertApiUrl.Text)
            Show_Alert(True, "Partner API reachable, " & result)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' ==== Type/polymorphism-dependent tier: a single shared call site, resolved by partner code
    ' (App_Code/PartnerCertificateTrustPolicy.vb). No branch at the call site itself -- which
    ' concrete policy's ValidateServerCertificate runs is decided entirely by
    ' PartnerCertificateTrustPolicyFactory.Resolve. ====

    Protected Sub btnCheckPartnerCertTrust_Click(sender As Object, e As EventArgs)
        Try
            Dim policy As PartnerCertificateTrustPolicyBase = PartnerCertificateTrustPolicyFactory.Resolve(txtPartnerCode.Text)
            ServicePointManager.ServerCertificateValidationCallback = AddressOf policy.ValidateServerCertificate
            Dim req As HttpWebRequest = CType(WebRequest.Create(txtPartnerTrustApiUrl.Text), HttpWebRequest)
            req.Method = "GET"
            Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Show_Alert(True, "Partner trust check for '" & txtPartnerCode.Text & "', status " & CInt(resp.StatusCode))
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

End Class
