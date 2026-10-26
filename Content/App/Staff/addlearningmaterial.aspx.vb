Imports System.Text
Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_adminpage
    Inherits System.Web.UI.Page
    Public Shared receiver As String
    Public Shared recCategory As String

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
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        scriptman = Me.Master.FindControl("ScriptManager1")
        scriptman.RegisterPostBackControl(btnSend)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()

                If Not IsPostBack Then
                    Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join subjects on classsubjects.subject = subjects.ID where classsubjects.teacher = '" & Session("StaffId") & "'", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                    cboSubject.Items.Clear()
                    cboSubject.Items.Add("SELECT")
                    Dim subjects As New ArrayList
                    Do While readref.Read
                        If Not subjects.Contains(readref.Item(0).ToString) Then
                            cboSubject.Items.Add(readref.Item(0))
                            subjects.Add(readref.Item(0).ToString)
                        End If
                    Loop
                    readref.Close()
                End If

                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim x As New Random
                Dim recno As Integer
                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select id from elearning", con)
                Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                Dim test As Boolean = False
                Dim refs As New ArrayList
                Do While readref.Read
                    refs.Add(readref.Item(0))
                Loop
                Dim y As Integer
                Do Until test = True
                    y = x.Next(100000, 999999)
                    If refs.Contains(y) Then
                        test = False
                    Else
                        test = True
                    End If
                Loop
                readref.Close()
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select id from class where class = '" & cboClass.Text & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim clas As Integer = readref2.Item(0)
                readref2.Close()
                Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select id from subjects where subject = '" & cboSubject.Text & "'", con)
                Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                readref3.Read()
                Dim subject As Integer = readref3.Item(0)
                readref3.Close()
                Dim folderPath As String = Server.MapPath("~/content/app/Uploads/")
                If FileUpload1.HasFile Then
                    Dim type As String = FileUpload1.PostedFile.ContentType
                    If FileUpload1.PostedFile.ContentLength > 131072000 Then
                        Show_Alert(False, "File not uploaded, the file selected is greater than 100mb.")
                        Exit Sub
                    End If
                    FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                    Dim q As String = "~/Uploads/" & FileUpload1.FileName
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into elearning (id, title, session, class, date, subject, file, teacher, type) values (?,?,?,?,?,?,?,?,?)", con)

                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("title", txtTitle.Text))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", clas))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", subject))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("assignment", q))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("teacher", Session("staffid")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("teach", type))
                    cmdCheck20.CommandTimeout = 1200

                    cmdCheck20.ExecuteNonQuery()

                    Show_Alert(True, "Learning material added successfully.")

                    logify.log(Session("staffid"), "A learning material - " & txtTitle.Text & " was uploaded for " & cboSubject.Text & " - " & cboClass.Text)

                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select studentsummary.student from studentsummary inner join class on studentsummary.class = class.id where class.class = '" & cboClass.Text & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboClass.Items.Clear()
                    Do While student0.Read
                        logify.Notifications("Learning material - " & txtTitle.Text & " was uploaded for " & cboSubject.Text, student0(0), Session("staffid"), cboSubject.Text & " TEACHER", "~/content/app/student/viewelearning.aspx?" & y, "")
                    Loop
                    student0.Close()
                    con.Dispose()
                    txtTitle.Text = ""
                    cboSubject.Text = "SELECT"

                Else
                    Show_Alert(False, "Please select a file")
                    Exit Sub
                End If
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub cboReceivetype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id INNER JOIN staffprofile on staffprofile.staffid = classsubjects.teacher where subjects.subject = '" & cboSubject.Text & "' and classsubjects.teacher = '" & Session("staffid") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                cboClass.Items.Clear()
                Do While student0.Read
                    cboClass.Items.Add(student0.Item(0))
                Loop
                student0.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboClass.SelectedIndexChanged
        receiver = cboSubject.Text
        recCategory = cboClass.Text
    End Sub




    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("~/content/app/staff/elearning.aspx")
    End Sub
End Class
