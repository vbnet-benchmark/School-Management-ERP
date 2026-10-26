Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls



Partial Class Admin_staffprofile
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
            panel1.Visible = False
            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees Order by Id", con)

                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    DropDownList1.Items.Clear()
                    DropDownList1.Items.Add("Select Route")

                    Do While student0.Read
                        DropDownList1.Items.Add(student0.Item(0).ToString)
                    Loop
                    student0.Close()
                    con.close()                End Using
            Else
                Session("currentroute") = Nothing
            End If
            If Session("currentroute") <> Nothing Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees where route = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("currentroute")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    student.Read()
                    DropDownList1.Text = student.Item(0).ToString
                    student.Close()
                    con.close()                End Using
                Staff_Details()
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Staff_Details()

        If DropDownList1.Text = "Select Route" Then
            Show_Alert(False, "Please select a route")
            Exit Sub
        End If
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT transportfees.route as Route, transportfees.amount as Cost, staffprofile.surname as Driver, staffprofile.passport, staffprofile.phone as 'Driver No', transportfees.busno as 'Bus No' from transportfees left Join staffprofile on transportfees.driver = staffprofile.StaffId where transportfees.route = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", DropDownList1.Text))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim pass As String
            If student.Read() Then pass = student.Item(3).ToString
            student.Close()
            Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT transportfees.route as Route, transportfees.amount as Cost, staffprofile.surname as Driver, staffprofile.phone as 'Driver No', transportfees.busno as 'Bus No' from transportfees left Join staffprofile on transportfees.driver = staffprofile.StaffId where transportfees.route = ?", con)
            cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", DropDownList1.Text))

            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad2
            adapter1.Fill(ds)
            DetailsView1.DataSource = ds
            DetailsView1.DataBind()

            panel1.Visible = True
            If Not pass = "" Then Image1.ImageUrl = pass
            con.close()        End Using
    End Sub
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click

       
        Response.Redirect("~/content/Admin/routes.aspx?" & DropDownList1.Text)

    End Sub


    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Response.Redirect("~/content/Admin/routes.aspx")
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Staff_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' --- Route Manifest Export (PLANTED-VB-HR-384) ---
    ' Export the selected route's manifest as a response header + body, in one of two output
    ' "formats" -- a legacy one (kept for an old desktop dispatch-board tool that expects a
    ' bare, unescaped filename) and a modern one (hardened). The format is chosen from a
    ' dropdown the admin picks explicitly on this SAME request -- never a value read back from
    ' a different, earlier request -- so this is genuine same-request format selection, not
    ' request-driven object dispatch.
    '
    ' Type/polymorphism-dependent tier, substituted: a project-wide grep for
    ' "MustInherit|MustOverride|Overrides |Interface |Implements " found four genuine
    ' polymorphic/interface hierarchies in this codebase -- App_Code/StudentOwnedRecordRepository.vb's
    ' StudentOwnedRecordRepositoryBase (CWE-639 round), App_Code/MessagePreviewRenderer.vb's
    ' IMessagePreviewStrategy (CWE-79 round), App_Code/StudentImportParsers.vb's
    ' XmlImportParserBase (CWE-611 round), and App_Code/LoginCompletionStrategy.vb's
    ' LoginCompletionStrategyBase (CWE-384 round) -- but none has any natural tie to
    ' header/export-format dispatch: the first dispatches per-table record lookups, the second
    ' dispatches HTML-preview rendering by audience, the third dispatches XML-import parsing,
    ' and the fourth dispatches SSO login completion. None applies here, so this substitutes a
    ' Select Case over a fixed format-key dropdown for genuine polymorphism, mirroring this
    ' project's own Content/Admin/addteacher.aspx.vb#btnUploadTeacherDocument_Click precedent
    ' (PLANTED-VB-HR-214, "no genuine interface/MustInherit/Overridable dispatch exists ...
    ' dispatches on an upload-type key read from a dropdown instead") and ICMServer's own
    ' EventsEdit.aspx.vb#EmitEventExportHeader precedent (PLANTED-VB-HR-274) for the same kind
    ' of honestly-disclosed substitution.
    Protected Sub btnExportManifest_Click(sender As Object, e As EventArgs) Handles btnExportManifest.Click
        If DropDownList1.Text = "Select Route" Then
            Show_Alert(False, "Please select a route")
            Exit Sub
        End If
        Dim routeName As String = DropDownList1.Text
        Select Case ddlManifestFormat.SelectedValue
            Case "Legacy"
                ' SINK: PLANTED-VB-HR-384
                Response.AddHeader("content-disposition", "attachment; filename=" & routeName & ".txt")
            Case "Modern"
                Dim safeRouteName As String = routeName.Replace(Chr(13), "").Replace(Chr(10), "")
                ' SAFE_SINK: PLANTED-VB-HR-384-safe
                Response.AddHeader("content-disposition", "attachment; filename=" & safeRouteName & ".txt")
        End Select
        Response.ContentType = "text/plain"
        Response.Write("Route manifest: " & routeName & vbCrLf)
        Response.End()
    End Sub
End Class
