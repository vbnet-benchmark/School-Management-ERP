Imports System.IO
Imports System.Text.RegularExpressions
Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    

    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String


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

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                If Not IsPostBack Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where designation = '" & "Driver" & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboDriver.Items.Clear()
                    cboDriver.Items.Add("Select Staff")
                    Do While student0.Read
                        cboDriver.Items.Add(student0.Item(0).ToString & " - " & student0.Item(1).ToString)
                    Loop
                    student0.Close()
                End If
                lblhead.Text = "New Transport Route"
                If Not IsPostBack And Request.QueryString.ToString <> Nothing Then
                    lblhead.Text = "Edit Transport Route"
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select transportfees.*, staffprofile.surname from transportfees left join staffprofile on transportfees.driver = staffprofile.staffId where transportfees.route = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Replace(Request.QueryString.ToString, "+", " ")))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    rcomfirm.Read()
                    txtRoute.Text = rcomfirm(0)

                    cboDriver.Text = rcomfirm(2) & " - " & rcomfirm(6)
                    txtBus.Text = rcomfirm(3)
                    rcomfirm.Close()
                Else


                End If
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub








    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            If txtRoute.Text = "" Then
                Show_Alert(False, "Please enter a route")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()

                Dim s() As String
                Dim driver As String = ""
                If Not cboDriver.Text = "Select Staff" Then
                    s = Split(cboDriver.Text, " - ")
                    driver = s(0)
                End If
                If Request.QueryString.ToString <> Nothing Then
                    Dim comfirms As New MySql.Data.MySqlClient.MySqlCommand("Select transportfees.*, staffprofile.surname from transportfees left join staffprofile on transportfees.driver = staffprofile.staffId where transportfees.route = ?", con)
                    comfirms.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Replace(Request.QueryString.ToString, "+", " ")))
                    Dim rcomfirms As MySql.Data.MySqlClient.MySqlDataReader = comfirms.ExecuteReader
                    rcomfirms.Read()
                    Dim formerdriver As String = rcomfirms(2).ToString
                    Dim formerbusno As String = rcomfirms(3).ToString
                    rcomfirms.Close()
                    ' Route-name format check (diagnostic only -- the rename still proceeds either
                    ' way, matching this page's existing practice of never blocking a save on a
                    ' secondary validation). Bounded with an explicit match timeout so a crafted
                    ' route name can never hang the request thread.
                    Dim isRouteNameFormatValidSafe As Boolean
                    Try
                        ' SAFE_SINK: PLANTED-VB-HR-286-safe
                        isRouteNameFormatValidSafe = Regex.IsMatch(txtRoute.Text, "^(([a-zA-Z0-9])+\s?)+$", RegexOptions.None, TimeSpan.FromMilliseconds(200))
                    Catch rtex As RegexMatchTimeoutException
                        isRouteNameFormatValidSafe = False
                    End Try
                    If Not isRouteNameFormatValidSafe Then
                        ' Logged only; the rename below is unaffected (diagnostic placeholder).
                    End If
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update transportfees Set driver = ?, busno = ? Where route = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", driver))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", txtBus.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Replace(Request.QueryString.ToString, "+", " ")))
                    cmdCheck3.ExecuteNonQuery()
                    Session("currentroute") = Replace(Request.QueryString.ToString, "+", " ")
                    logify.log(Session("staffid"), "Transport route " & Replace(Request.QueryString.ToString, "+", " ") & " was edited by admin.")

                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select studentsprofile.admno, parentprofile.parentid, studentsprofile.surname from studentsprofile left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.transport = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Replace(Request.QueryString.ToString, "+", " ")))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    Do While rcomfirm.Read
                        If txtRoute.Text <> Replace(Request.QueryString.ToString, "+", " ") Then
                            logify.Notifications(Replace(Request.QueryString.ToString, "+", " ") & " transport route has been changed to " & txtRoute.Text, rcomfirm(0).ToString, Session("staffid"), "Admin", "~/content/student/studentprofile.aspx", "")
                            If Not rcomfirm(1).ToString = "" Then logify.Notifications(Replace(Request.QueryString.ToString, "+", " ") & " transport route has been changed to " & txtRoute.Text, rcomfirm(1).ToString, Session("staffid"), "Admin", "~/content/parent/studentprofile.aspx", "")
                        End If


                        If formerdriver <> driver And s(1) <> Nothing Then
                            logify.Notifications("You are now the driver of " & Replace(Request.QueryString.ToString, "+", " ") & " transport route. " & IIf(txtBus.Text <> "", "Bus no - " & txtBus.Text, ""), driver, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                            logify.Notifications("Your driver has been changed to " & s(1), rcomfirm(0).ToString, Session("staffid"), "Admin", "~/content/student/studentprofile.aspx", "")
                            If Not rcomfirm(1).ToString = "" Then logify.Notifications("The driver of " & rcomfirm(2).ToString & " has been changed to " & s(1), rcomfirm(1).ToString, Session("staffid"), "Admin", "~/content/parent/studentprofile.aspx", "")
                        End If

                    Loop
                    rcomfirm.Close()
                    If Not cboDriver.Text <> "Select Staff" Then
                   
                    If formerdriver <> driver And s(1) <> Nothing Then
                        logify.Notifications("You are now the driver of " & Replace(Request.QueryString.ToString, "+", " ") & " transport route. " & IIf(txtBus.Text <> "", "Bus no - " & txtBus.Text, ""), driver, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                    End If
                    end if
                    If formerdriver <> driver And formerdriver <> "" Then
                        logify.Notifications("You are no more the driver of " & Replace(Request.QueryString.ToString, "+", " ") & " transport route", formerdriver, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                    End If
                    If formerbusno <> txtBus.Text And txtBus.Text <> "" And formerdriver = driver Then
                        logify.Notifications("Your Bus Number has been changed to " & txtBus.Text, driver, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                    End If
                    Response.Redirect("~/content/admin/transport.aspx")
                Else
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select transportfees.*, staffprofile.surname from transportfees left join staffprofile on transportfees.driver = staffprofile.staffId where transportfees.route = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtRoute.Text))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    If rcomfirm.Read() Then
                        Show_Alert(False, "Route already exists")
                        Exit Sub
                    End If
                    rcomfirm.Close()
                    ' Route-name format check (diagnostic only -- the insert below still proceeds
                    ' either way, matching this page's existing practice of never blocking a save
                    ' on a secondary validation).
                    Dim isRouteNameFormatValid As Boolean
                    ' SINK: PLANTED-VB-HR-286
                    isRouteNameFormatValid = Regex.IsMatch(txtRoute.Text, "^(([a-zA-Z0-9])+\s?)+$")
                    If Not isRouteNameFormatValid Then
                        ' Logged only; the insert below is unaffected (diagnostic placeholder).
                    End If
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transportfees (route, driver, busno) Values (?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", txtRoute.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", driver))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", txtBus.Text))
                    cmdCheck2.ExecuteNonQuery()
                    logify.Notifications("You are now the driver of " & txtRoute.Text & " transport route. " & IIf(txtBus.Text <> "", "Bus no - " & txtBus.Text, ""), driver, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")

                    logify.log(Session("staffid"), txtRoute.Text & " transport route was added by admin.")
                    AdminAuditLogger.LogAdminActivity(Server.MapPath("~/App_Data/adminactivity.log"), "Transport", "New route added: " & txtRoute.Text & " (bus " & txtBus.Text & ") by staff " & Session("staffid").ToString())
                    Show_Alert(True, "Route Added Successfully")
                End If
                con.close()
            End Using

            txtBus.Text = ""
            txtRoute.Text = ""
            cboDriver.Text = "Select Staff"
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Session("currentroute") = Replace(Request.QueryString.ToString, "+", " ")
        Response.Redirect("~/content/admin/transport.aspx")

    End Sub
End Class
