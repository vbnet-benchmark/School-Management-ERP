Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_allstudents
    Inherits System.Web.UI.Page
    Implements IAdminQuickActionTarget
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

       
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, cast(time as char) as date, object, message from error order by id desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("date")
                ds.Columns.Add("object")
                ds.Columns.Add("message")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While reader.Read
                    ds.Rows.Add(reader(0).ToString, Convert.ToDateTime(reader(1).ToString).ToString("dd/MM/yyyy hh:mm tt"), reader(2).ToString, reader(3).ToString)

                Loop
                reader.Close()


                GridView1.DataSource = ds
                GridView1.DataBind()


                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub


    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, cast(time as char) as date, object, message from error where id = '" & txtSearch.Text & "'", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("date")
                ds.Columns.Add("object")
                ds.Columns.Add("message")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While reader.Read
                    ds.Rows.Add(reader(0).ToString, Convert.ToDateTime(reader(1).ToString).ToString("dd/MM/yyyy hh:mm tt"), reader(2).ToString, reader(3).ToString)

                Loop
                reader.Close()


                GridView1.AllowPaging = False
                GridView1.DataSource = ds
                GridView1.DataBind()

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("delete from error", con)
                cmdLoad1.ExecuteNonQuery()


                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' --- Error Log Quick Actions (PLANTED-VB-HR-368, -369) ---
    ' New admin-only "quick action" group for this page's error log: a single named dispatch and
    ' a batch dispatch, both routed through AdminQuickActionDispatcher (App_Code, cross-file).

    ''' <summary>Real "Archive" maintenance action -- appends an audit-trail entry for this run.</summary>
    Public Sub ArchiveLog() Implements IAdminQuickActionTarget.ArchiveLog
        AdminAuditLogger.LogAdminActivity(Server.MapPath("~/App_Data/adminactivity.log"), "ErrorLogQuickAction", "Error log archived via quick-action dispatch by staff " & Session("staffid").ToString())
    End Sub

    ''' <summary>Real "Export" maintenance action -- appends an audit-trail entry for this run.</summary>
    Public Sub ExportLog() Implements IAdminQuickActionTarget.ExportLog
        AdminAuditLogger.LogAdminActivity(Server.MapPath("~/App_Data/adminactivity.log"), "ErrorLogQuickAction", "Error log export triggered via quick-action dispatch by staff " & Session("staffid").ToString())
    End Sub

    ''' <summary>
    ''' Instance 3 (Interprocedural, cross-file): the caller-supplied action name is handed,
    ''' unvalidated, straight across to App_Code/AdminQuickActionDispatcher.vb, which is where the
    ''' actual CallByName() call lives.
    ''' </summary>
    Protected Sub btnQuickAction_Click(sender As Object, e As EventArgs) Handles btnQuickAction.Click
        Try
            Dim actionName As String = Request.Params("QuickAction")
            AdminQuickActionDispatcher.Dispatch(Me, actionName)
            Show_Alert(True, "Quick action dispatched")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="btnQuickAction_Click"/> -- routes through
    ''' AdminQuickActionDispatcher.DispatchSafe instead, which only ever uses the caller-supplied
    ''' value as an allow-list key.
    ''' </summary>
    Protected Sub btnQuickActionSafe_Click(sender As Object, e As EventArgs) Handles btnQuickActionSafe.Click
        Try
            Dim actionKey As String = Request.Params("QuickActionKey")
            AdminQuickActionDispatcher.DispatchSafe(Me, actionKey)
            Show_Alert(True, "Quick action dispatched")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Instance 4 (substituted for type/polymorphism-dependent): batch-dispatches a
    ''' comma-separated list of caller-supplied action names, invoked inline against this page
    ''' directly -- a distinct construction from PLANTED-VB-HR-368's single-call,
    ''' shared-dispatcher-class shape. No genuine polymorphic hierarchy in this project ties
    ''' naturally to quick-action routing (see App_Code/AdminQuickActionDispatcher.vb's own
    ''' doc comment), so this substitutes a loop-batch CallByName shape instead, mirroring this
    ''' project's own App_Code/AdminAuditLogger.vb#LogStockAdjustment precedent for the same kind
    ''' of substitution -- disclosed honestly rather than claimed as something it is not. The
    ''' per-iteration Try/Catch is robustness only (an unrecognized/unsupported action name for
    ''' one list element should not abort the rest of the batch), not a security control --
    ''' mirrors ICMModules' own InvokeNamedPageActions precedent for the same reason.
    ''' </summary>
    Protected Sub btnQuickActionsBatch_Click(sender As Object, e As EventArgs) Handles btnQuickActionsBatch.Click
        Try
            Dim requestedActions() As String = Split(Request.Params("QuickActions"), ",")
            For Each rawActionName As String In requestedActions
                Dim actionName As String = Trim(rawActionName)
                If actionName <> "" Then
                    Try
                        ' SINK: PLANTED-VB-HR-369
                        CallByName(Me, actionName, CallType.Method)
                    Catch exDispatch As Exception
                        ' Unrecognized/unsupported action name -- skipped, not fatal to the batch.
                    End Try
                End If
            Next
            Show_Alert(True, "Batch quick actions dispatched")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="btnQuickActionsBatch_Click"/> -- each list element is only ever
    ''' used as an allow-list key; every branch below calls a real, compiler-checked method
    ''' directly, so no dynamic-dispatch surface remains regardless of what the caller supplies.
    ''' </summary>
    Protected Sub btnQuickActionsBatchSafe_Click(sender As Object, e As EventArgs) Handles btnQuickActionsBatchSafe.Click
        Try
            Dim requestedActionKeys() As String = Split(Request.Params("QuickActionsKeys"), ",")
            For Each rawActionKey As String In requestedActionKeys
                Dim actionKey As String = Trim(rawActionKey)
                Select Case actionKey
                    Case "archive"
                        ' SAFE_SINK: PLANTED-VB-HR-369-safe
                        ArchiveLog()
                    Case "export"
                        ExportLog()
                    Case Else
                        ' Unrecognized key -- nothing is dispatched for this batch element.
                End Select
            Next
            Show_Alert(True, "Batch quick actions dispatched")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
