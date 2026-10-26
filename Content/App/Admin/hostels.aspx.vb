Imports System.IO
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
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Not IsPostBack Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where designation = '" & "Hostel Master/Mistress" & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader

                    cboDriver.Items.Add("Select Staff")
                    Do While student0.Read
                        cboDriver.Items.Add(student0.Item(0).ToString.ToUpper & " - " & student0.Item(1).ToString.ToUpper)
                    Loop
                    student0.Close()

                End If
                lblhead.Text = "New Hostel"
                If Not IsPostBack And Request.QueryString.ToString <> Nothing Then
                    lblhead.Text = "Edit Hostel"
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select hostel.*, staffprofile.surname from hostel left join staffprofile on hostel.ward = staffprofile.staffId where hostel.hostel = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Replace(Request.QueryString.ToString, "+", " ")))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    rcomfirm.Read()
                    txtRoute.Text = rcomfirm(0).ToString
                    cboDriver.Text = rcomfirm(1) & " - " & rcomfirm(3)
                    rcomfirm.Close()


                End If
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub







    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim s() As String
                Dim driver As String = ""
                If Not cboDriver.Text = "Select Staff" Then
                    s = Split(cboDriver.Text, " - ")
                    driver = s(0)
                End If
                If Request.QueryString.ToString <> Nothing Then
                    Dim comfirms As New MySql.Data.MySqlClient.MySqlCommand("Select hostel.*, staffprofile.surname, staffprofile.staffid from hostel left join staffprofile on hostel.ward = staffprofile.staffId where hostel.hostel = ?", con)
                    comfirms.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Replace(Request.QueryString.ToString, "+", " ")))
                    Dim rcomfirms As MySql.Data.MySqlClient.MySqlDataReader = comfirms.ExecuteReader
                    Dim formerward As String
                    Dim fmrwardid As String
                    If rcomfirms.Read() Then
                        formerward = rcomfirms(3).ToString
                        fmrwardid = rcomfirms(4).ToString
                    End If

                    rcomfirms.Close()

                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update hostel Set hostel = '" & txtRoute.Text & "', ward = ? Where hostel = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", driver))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Replace(Request.QueryString.ToString, "+", " ")))
                    cmdCheck3.ExecuteNonQuery()
                    logify.log(Session("staffid"), "Hostel " & Replace(Request.QueryString.ToString, "+", " ") & " was edited by admin.")

                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select studentsprofile.admno, parentprofile.parentid, studentsprofile.surname from studentsprofile left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.hostel = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Replace(Request.QueryString.ToString, "+", " ")))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    Do While rcomfirm.Read
                        If txtRoute.Text <> Replace(Request.QueryString.ToString, "+", " ") Then
                            logify.Notifications(Replace(Request.QueryString.ToString, "+", " ") & " hostel has been changed to " & txtRoute.Text, rcomfirm(0).ToString, Session("staffid"), "Admin", "")
                            If Not rcomfirm(1).ToString = "" Then logify.Notifications(Replace(Request.QueryString.ToString, "+", " ") & " hostel has been changed to " & txtRoute.Text, rcomfirm(1).ToString, Session("staffid"), "Admin", "")
                        End If
                        If formerward <> s(1) And s(1) <> Nothing Then
                            logify.Notifications("Your hostel ward has been changed to " & s(1), rcomfirm(0).ToString, Session("staffid"), "Admin", "")
                            If Not rcomfirm(1).ToString = "" Then logify.Notifications("The hostel ward of " & rcomfirm(2).ToString & " has been changed to " & s(1), rcomfirm(1).ToString, Session("staffid"), "Admin", "")
                        End If
                    Loop
                    rcomfirm.Close()
                    If formerward <> s(1) And s(1) <> Nothing Then
                        logify.Notifications("You are no more the hostel ward of " & txtRoute.Text, fmrwardid, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                        logify.Notifications("You are now the hostel ward of " & txtRoute.Text, driver, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")

                    End If


                    Session("currenthostel") = txtRoute.Text
                    Response.Redirect("~/content/App/Admin/boarding.aspx")
                Else
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into hostel (hostel, ward) Values (?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", txtRoute.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", driver))
                    cmdCheck2.ExecuteNonQuery()
                    logify.log(Session("staffid"), txtRoute.Text & " was added as a hostel by admin.")
                    logify.Notifications("You are now the hostel ward of " & txtRoute.Text, driver, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                    cboDriver.Text = "Select Staff"
                    txtRoute.Text = ""
                    Show_Alert(True, "Hostel Added Successfully")
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/content/App/Admin/boarding.aspx")
    End Sub
End Class
