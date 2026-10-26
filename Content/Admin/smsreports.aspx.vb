Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Diagnostics
Imports System.Text.RegularExpressions
Partial Class Admin_allstudents
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList
    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    ' New SMS Gateway Diagnostics collaborator (PLANTED-VB-HR-370's dispatch target) -- see the
    ' "--- SMS Gateway Diagnostics ---" region near the end of this file.
    Dim gatewayDiagnostics As New SmsGatewayDiagnostics
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
        Try

       
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
                cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
                Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2a.ExecuteReader
                reader2a.Read()
                Label1.Text = "Available Units: " & reader2a(0)

                reader2a.Close()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(sms.time as char) as date, staffprofile.surname as 'Sender', sms.subject, sms.pages, sms.units, sms.recipienttype, sms.recipientsno from sms inner join staffprofile on staffprofile.staffid = sms.sender order by sms.time desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("date")
                ds.Columns.Add("sender")

                ds.Columns.Add("subject")
                ds.Columns.Add("pages")
                ds.Columns.Add("units")
                ds.Columns.Add("recipienttype")
                ds.Columns.Add("recipients")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While reader.Read
                    ds.Rows.Add(Convert.ToDateTime(reader(0)).ToString("dd/MM/yyyy hh:mm tt"), reader(1), reader(2), reader(3), reader(4), reader(5), reader(6))
                Loop
                reader.Close()


                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(sms.time as char) as date, staffprofile.surname as 'Sender', sms.subject, sms.pages, sms.units, sms.recipienttype, sms.recipientsno from sms inner join staffprofile on staffprofile.staffid = sms.sender order by sms.time desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("date")
                ds.Columns.Add("sender")

                ds.Columns.Add("subject")
                ds.Columns.Add("pages")
                ds.Columns.Add("units")
                ds.Columns.Add("recipienttype")
                ds.Columns.Add("recipients")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While reader.Read
                    ds.Rows.Add(Convert.ToDateTime(reader(0)).ToString("dd/MM/yyyy hh:mm tt"), reader(1), reader(2), reader(3), reader(4), reader(5), reader(6))
                Loop
                reader.Close()



                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

                con.Close()            End Using

            If GridView1.PageIndex + 1 <= GridView1.PageCount Then
                GridView1.PageIndex = GridView1.PageIndex + 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(sms.time as char) as date, staffprofile.surname as 'Sender', sms.subject, sms.pages, sms.units, sms.recipienttype, sms.recipientsno from sms inner join staffprofile on staffprofile.staffid = sms.sender order by sms.time desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("date")
                ds.Columns.Add("sender")

                ds.Columns.Add("subject")
                ds.Columns.Add("pages")
                ds.Columns.Add("units")
                ds.Columns.Add("recipienttype")
                ds.Columns.Add("recipients")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While reader.Read
                    ds.Rows.Add(Convert.ToDateTime(reader(0)).ToString("dd/MM/yyyy hh:mm tt"), reader(1), reader(2), reader(3), reader(4), reader(5), reader(6))
                Loop
                reader.Close()


                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                con.Close()            End Using

            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataBind()

            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(sms.time as char) as date, staffprofile.surname as 'Sender', sms.subject, sms.pages, sms.units, sms.recipienttype, sms.recipientsno from sms inner join staffprofile on staffprofile.staffid = sms.sender where sms.subject like '%" & txtSearch.Text & "%' order by sms.time desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("date")
                ds.Columns.Add("sender")

                ds.Columns.Add("subject")
                ds.Columns.Add("pages")
                ds.Columns.Add("units")
                ds.Columns.Add("recipienttype")
                ds.Columns.Add("recipients")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While reader.Read
                    ds.Rows.Add(Convert.ToDateTime(reader(0)).ToString("dd/MM/yyyy hh:mm tt"), reader(1), reader(2), reader(3), reader(4), reader(5), reader(6))
                Loop
                reader.Close()


                GridView1.AllowPaging = False
                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(sms.time as char) as date, staffprofile.surname as 'Sender', sms.subject, sms.pages, sms.units, sms.recipienttype, sms.recipientsno from sms inner join staffprofile on staffprofile.staffid = sms.sender order by sms.time desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("date")
                ds.Columns.Add("sender")

                ds.Columns.Add("subject")
                ds.Columns.Add("pages")
                ds.Columns.Add("units")
                ds.Columns.Add("recipienttype")
                ds.Columns.Add("recipients")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While reader.Read
                    ds.Rows.Add(Convert.ToDateTime(reader(0)).ToString("dd/MM/yyyy hh:mm tt"), reader(1), reader(2), reader(3), reader(4), reader(5), reader(6))
                Loop
                reader.Close()


                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                con.Close()            End Using
            If GridView1.PageIndex - 1 >= 0 Then
                GridView1.PageIndex = GridView1.PageIndex - 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnMsg_Click(sender As Object, e As EventArgs) Handles btnMsg.Click
        Response.Redirect("~/Content/admin/newmsg.aspx")
    End Sub

    ' --- SMS Gateway Diagnostics (PLANTED-VB-HR-366, -367, -370) ---
    ' New admin-only quick-diagnostics group for this page's "SMS Reports" feature: a
    ' connectivity check against the configured SMS gateway host, a scheduled export-batch
    ' runner, and a named diagnostic dispatcher. Per vbnet-cwe-676-planting-research.md, this
    ' project's own organic SMS code (App_Code/notify.vb) never talks to a real gateway at all,
    ' so this mirrors the project's own already-established "new, invented-but-realistic admin
    ' feature" convention used elsewhere in this CWE-676 round rather than retrofitting
    ' nonexistent gateway code.

    ''' <summary>
    ''' Instance 1 (Direct): pings the admin-supplied gateway host to confirm reachability before
    ''' relying on it for an outbound SMS run. The host string is baked directly into Shell()'s
    ''' single command-line argument with no validation -- a value like
    ''' "example.com & del C:\Important" rides along unchanged.
    ''' </summary>
    Protected Sub btnTestGateway_Click(sender As Object, e As EventArgs) Handles btnTestGateway.Click
        Try
            Dim gatewayHost As String = Request.Params("GatewayHost")
            ' SINK: PLANTED-VB-HR-366
            Shell("ping.exe -n 2 " & gatewayHost, AppWinStyle.Hide, True, 5000)
            Show_Alert(True, "Gateway connectivity check started for " & gatewayHost)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="btnTestGateway_Click"/> -- the host is validated against a strict
    ''' hostname-shaped allow-list BEFORE it is used, and is then passed to the OS as its own,
    ''' already-delimited argument via ProcessStartInfo.ArgumentList rather than concatenated into
    ''' a single Shell() command-line string, so there is no string left for a crafted value to
    ''' inject an extra flag or command into.
    ''' </summary>
    Protected Sub btnTestGatewaySafe_Click(sender As Object, e As EventArgs) Handles btnTestGatewaySafe.Click
        Try
            Dim gatewayHost As String = Request.Params("GatewayHost")
            If Not Regex.IsMatch(gatewayHost, "^[a-zA-Z0-9][a-zA-Z0-9\.\-]{0,253}$") Then
                Show_Alert(False, "Invalid gateway host")
                Exit Sub
            End If
            Dim psi As New ProcessStartInfo("ping.exe")
            psi.ArgumentList.Add("-n")
            psi.ArgumentList.Add("2")
            psi.ArgumentList.Add(gatewayHost)
            psi.UseShellExecute = False
            ' SAFE_SINK: PLANTED-VB-HR-366-safe
            Process.Start(psi)
            Show_Alert(True, "Gateway connectivity check started for " & gatewayHost)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Instance 2 (Indirect, same-class private helper): the actual Shell() call lives in
    ''' <see cref="RunScheduledSmsExportBatch"/>, routed via cmd.exe so the admin-supplied export
    ''' label is subject to the OS shell's own command-line parsing -- genuine OS command
    ''' injection, not merely argument injection into the launched program.
    ''' </summary>
    Protected Sub btnRunExportBatch_Click(sender As Object, e As EventArgs) Handles btnRunExportBatch.Click
        Try
            RunScheduledSmsExportBatch(Request.Params("ExportLabel"))
            Show_Alert(True, "SMS export batch started")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub RunScheduledSmsExportBatch(exportLabel As String)
        Dim scriptPath As String = Server.MapPath("~/App_Data/smsexport.bat")
        ' SINK: PLANTED-VB-HR-367
        Shell("cmd.exe /c " & scriptPath & " " & exportLabel, AppWinStyle.Hide, True, 10000)
    End Sub

    Protected Sub btnRunExportBatchSafe_Click(sender As Object, e As EventArgs) Handles btnRunExportBatchSafe.Click
        Try
            RunScheduledSmsExportBatchSafe(Request.Params("ExportLabel"))
            Show_Alert(True, "SMS export batch started")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="RunScheduledSmsExportBatch"/> -- the export label is validated
    ''' against a strict allow-list shape and then passed to cmd.exe as its own ArgumentList
    ''' entry (never concatenated into the command-line string cmd.exe itself parses).
    ''' </summary>
    Private Sub RunScheduledSmsExportBatchSafe(exportLabel As String)
        If Not Regex.IsMatch(exportLabel, "^[a-zA-Z0-9_]{1,40}$") Then
            Show_Alert(False, "Invalid export label")
            Exit Sub
        End If
        Dim scriptPath As String = Server.MapPath("~/App_Data/smsexport.bat")
        Dim psi As New ProcessStartInfo("cmd.exe")
        psi.ArgumentList.Add("/c")
        psi.ArgumentList.Add(scriptPath)
        psi.ArgumentList.Add(exportLabel)
        psi.UseShellExecute = False
        ' SAFE_SINK: PLANTED-VB-HR-367-safe
        Process.Start(psi)
    End Sub

    ''' <summary>
    ''' Instance 5 (Diversity tier): a third, distinct CallByName construction from this round's
    ''' other two -- dispatched inline, directly at this page's own event handler (no shared
    ''' App_Code dispatcher class involved, unlike PLANTED-VB-HR-368/-369), and against a
    ''' dedicated collaborator object (<see cref="gatewayDiagnostics"/>) rather than against the
    ''' page itself. This project has only one admin role (no "Superadmin"/"Supervisor"-style
    ''' narrower tier the way ICMModules'/CammIntegrationPortal's own CWE-676 diversity instances
    ''' used), so the distinct shape here is the dispatch TARGET, not an extra role gate --
    ''' disclosed honestly rather than invented.
    ''' </summary>
    Protected Sub btnRunNamedDiagnostic_Click(sender As Object, e As EventArgs) Handles btnRunNamedDiagnostic.Click
        Try
            Dim diagnosticName As String = Request.Params("DiagnosticName")
            ' SINK: PLANTED-VB-HR-370
            CallByName(gatewayDiagnostics, diagnosticName, CallType.Method)
            Show_Alert(True, "Diagnostic executed")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="btnRunNamedDiagnostic_Click"/> -- the caller-supplied value is
    ''' only ever used as an allow-list KEY; every branch below calls a real, compiler-checked
    ''' method on <see cref="gatewayDiagnostics"/> directly, so no dynamic-dispatch surface
    ''' remains regardless of what the caller supplies.
    ''' </summary>
    Protected Sub btnRunNamedDiagnosticSafe_Click(sender As Object, e As EventArgs) Handles btnRunNamedDiagnosticSafe.Click
        Try
            Dim diagnosticKey As String = Request.Params("DiagnosticKey")
            Select Case diagnosticKey
                Case "balance"
                    ' SAFE_SINK: PLANTED-VB-HR-370-safe
                    gatewayDiagnostics.CheckBalance()
                Case "queue"
                    gatewayDiagnostics.CheckQueueDepth()
                Case "reset"
                    gatewayDiagnostics.ResetConnection()
                Case Else
                    ' Unrecognized key -- nothing is dispatched.
            End Select
            Show_Alert(True, "Diagnostic executed")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

End Class
