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
    Dim par As New parentcheck
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
        If check.Check_Student(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

      
        If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    logify.Read_notification("~/content/student/studentprofile.aspx", Session("studentid"))
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, activated, hostelstay, hostel, admfees, transport from StudentsProfile where admno = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentId")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    Dim pass As String
                    student.Read()
                    pass = student.Item("passport").ToString
                    Dim hostel As String = student(3).ToString
                    Dim transport As String = student(5).ToString
                    student.Close()
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno as 'Admission number', studentsprofile.surname as Name, studentsprofile.Sex, studentsprofile.dateOfBirth as Birthday, studentsummary.age as 'Age', studentsprofile.phone as 'Phone number' from studentsprofile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentId")))
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1.SelectCommand = cmdLoad1
                    adapter1.Fill(ds)

                    DetailsView1.DataSource = ds
                    DetailsView1.DataBind()

                    If pass <> "" Then Image1.ImageUrl = pass
                    Dim cmdLoad1xa As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.passport from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
                    cmdLoad1xa.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentid")))
                    Dim studentxa As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1xa.ExecuteReader
                    studentxa.Read()
                    If Not studentxa(0).ToString = "" Then Image4.ImageUrl = studentxa(0).ToString
                    studentxa.Close()

                    Dim cmdLoad1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname as 'Parents/Guardians', parentprofile.phone as 'Parents Phone number', parentprofile.address as Address from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
                    cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentid")))
                    cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                    Dim dsa As New DataTable
                    Dim adapter1a As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1a.SelectCommand = cmdLoad1a
                    adapter1a.Fill(dsa)

                    DetailsView3.DataSource = dsa
                    DetailsView3.DataBind()

                    If hostel <> "" Then
                        Dim cmdLoad1z As New MySql.Data.MySqlClient.MySqlCommand("SELECT hostel.hostel as Hostel, staffprofile.surname as 'Hostel Ward', staffprofile.passport, staffprofile.phone as 'Phone No' from hostel left Join staffprofile on hostel.ward = staffprofile.StaffId where hostel.hostel = ?", con)
                        cmdLoad1z.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", hostel))
                        Dim studentz As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1z.ExecuteReader
                        Dim passz As String
                        If studentz.Read() Then passz = studentz.Item(2).ToString
                        studentz.Close()
                        Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT hostel.hostel as Hostel, staffprofile.surname as 'Hostel Ward', staffprofile.phone as 'Phone No' from hostel left Join staffprofile on hostel.ward = staffprofile.StaffId where hostel.hostel = ?", con)
                        cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", hostel))

                        Dim dsz As New DataTable
                        Dim adapter1z As New MySql.Data.MySqlClient.MySqlDataAdapter
                        adapter1z.SelectCommand = cmdLoad2
                        adapter1z.Fill(dsz)
                        DetailsView2.DataSource = dsz
                        DetailsView2.DataBind()
                        panel1.Visible = True

                        If Not passz = "" Then Image2.ImageUrl = passz
                    End If
                    If transport <> "" Then
                        Dim cmdLoad1z As New MySql.Data.MySqlClient.MySqlCommand("SELECT transportfees.route as Route, transportfees.amount as Cost, staffprofile.surname as Driver, staffprofile.passport, staffprofile.phone as 'Driver No', transportfees.busno as 'Bus No' from transportfees left Join staffprofile on transportfees.driver = staffprofile.StaffId where transportfees.route = ?", con)
                        cmdLoad1z.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", transport))
                        Dim studentz As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1z.ExecuteReader
                        Dim passz As String
                        If studentz.Read() Then passz = studentz.Item(3).ToString
                        studentz.Close()
                        Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT transportfees.route as Route, staffprofile.surname as Driver, staffprofile.phone as 'Driver No', transportfees.busno as 'Bus No' from transportfees left Join staffprofile on transportfees.driver = staffprofile.StaffId where transportfees.route = ?", con)
                        cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", transport))

                        Dim dsz As New DataTable
                        Dim adapter1z As New MySql.Data.MySqlClient.MySqlDataAdapter
                        adapter1z.SelectCommand = cmdLoad2
                        adapter1z.Fill(dsz)
                        DetailsView4.DataSource = dsz
                        DetailsView4.DataBind()
                        panel2.Visible = True
                        If Not passz = "" Then Image3.ImageUrl = passz
                    End If




                    con.close()                End Using

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click

        Panel3.Visible = True


    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                If txtPassword.Text = "" Or txtPassword0.Text = "" Then
                    Show_Alert(False, "Password field is blank")
                ElseIf txtPassword.Text <> txtPassword0.Text Then
                    Show_Alert(False, "Your password entries do not match")
                Else
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentsProfile Set password = ? where admno = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", txtPassword.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StudentID")))
                    cmdCheck3.ExecuteNonQuery()
                    Show_Alert(True, "Password updated successfully")

                    Panel3.Visible = False
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Panel3.Visible = False
    End Sub
End Class
