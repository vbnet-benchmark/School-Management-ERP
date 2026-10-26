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



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()

                If Not IsPostBack Then
                    DropDownList4.Items.Clear()
                    For i As Integer = 1 To 14

                        DropDownList4.Items.Add(i)
                    Next

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

                    Dim ref4 As New MySql.Data.MySqlClient.MySqlCommand("Select staffdept.dept, depts.superior from staffdept inner join depts on depts.id = staffdept.dept where staffdept.staff = '" & Session("StaffId") & "'", con)
                    Dim readref4 As MySql.Data.MySqlClient.MySqlDataReader = ref4.ExecuteReader
                    Dim heads As New ArrayList
                    Dim superiors As New ArrayList
                    Do While readref4.Read()
                        heads.Add(readref4.Item(0))
                        If Not readref4(1) = "none" Then superiors.Add(readref4(1))
                    Loop
                    readref4.Close()
                    For Each item As String In superiors
                        Dim ref40 As New MySql.Data.MySqlClient.MySqlCommand("Select id from depts where dept = '" & item & "'", con)
                        Dim readref40 As MySql.Data.MySqlClient.MySqlDataReader = ref40.ExecuteReader
                        readref40.Read()
                        heads.Add(readref40(0))
                        readref40.Close()
                    Next
                    For Each item In heads
                        Dim ref5 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname from depts inner join staffprofile on staffprofile.staffid = depts.head where depts.id = '" & item & "'", con)
                        Dim readref5 As MySql.Data.MySqlClient.MySqlDataReader = ref5.ExecuteReader
                        readref5.Read()
                        cboHOD.Items.Add(readref5.Item(0))
                        readref5.Close()
                    Next

                End If


                FreeTextBox1.Focus = True
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            Dim message As String = FreeTextBox1.Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim x As New Random
                Dim recno As Integer
                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select id from lessonplan", con)
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

                Dim ref30 As New MySql.Data.MySqlClient.MySqlCommand("Select staffid from staffprofile where surname = '" & cboHOD.Text & "'", con)
                Dim readref30 As MySql.Data.MySqlClient.MySqlDataReader = ref30.ExecuteReader
                readref30.Read()
                Dim head As String = readref30.Item(0)
                readref30.Close()

                If Request.QueryString.ToString <> Nothing Then
                    Dim cmdCheck9 As New MySql.Data.MySqlClient.MySqlCommand("Delete from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                    cmdCheck9.ExecuteNonQuery()
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Update lessonplan set plan = ?, date = ? where Id = '" & Request.QueryString.ToString & "'", con)
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("assignment", FreeTextBox1.Text))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("deadline", FormatDateTime(Now, DateFormat.GeneralDate)))
                    cmdCheck20.ExecuteNonQuery()
                    logify.log(Session("staffid"), "An updated lesson plan was submitted to " & par.getstaff(head))
                    logify.Notifications("An updated lesson plan was submitted by " & par.getstaff(Session("staffid")), head, Session("staffid"), "Subordinate", "")
                    If LinkButton1.Text <> "" Then

                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic1")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att1.Text))

                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton2.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic2")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att2.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton3.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic3")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att3.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton4.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic4")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att4.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If

                Else
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into lessonplan (id, teacher, head, subject, class, week, plan, date, session) values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("title", Session("StaffId")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("head", head))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", subject))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", clas))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("week", DropDownList4.Text))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("assignment", FreeTextBox1.Text))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
                    cmdCheck20.ExecuteNonQuery()
                    logify.log(Session("staffid"), "A new lesson plan was submitted to " & par.getstaff(head))
                    logify.Notifications("A new lesson plan has been submitted by " & par.getstaff(Session("staffid")), head, Session("staffid"), "Subordinate", "~/content/app/staff/checkplan.aspx?" & y, "")
                    If LinkButton1.Text <> "" Then

                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic1")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att1.Text))

                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton2.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic2")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att2.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton3.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic3")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att3.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton4.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic4")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att4.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If
                End If
                Show_Alert(True, "Lesson plan submitted successfully")
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub cboReceivetype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject.Text & "' and classsubjects.teacher = '" & Session("staffid") & "'", con)
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

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect(Session("pic1"))
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Response.Redirect(Session("pic2"))
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        Response.Redirect(Session("pic3"))

    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Response.Redirect(Session("pic4"))

    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Try
            Dim folderPath As String = Server.MapPath("~/content/Uploads/")
            Dim q As String
            If FileUpload1.HasFile Then
                FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                q = "http://" & Request.Url.Authority & "/Content/Uploads/" & FileUpload1.FileName
                Dim picsource As New fileimage
                If LinkButton1.Text = "" Then
                    att1.Text = FileUpload1.PostedFile.ContentType
                    icon1.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton1.Text = Path.GetFileName(q)
                    Session("pic1") = q
                    Panel1.Visible = True
                    del1.Text = "Delete"
                ElseIf LinkButton2.Text = "" Then
                    att2.Text = FileUpload1.PostedFile.ContentType
                    icon2.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton2.Text = Path.GetFileName(q)
                    Session("pic2") = q
                    del2.Text = "Delete"

                ElseIf LinkButton3.Text = "" Then
                    att3.Text = FileUpload1.PostedFile.ContentType
                    icon3.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton3.Text = Path.GetFileName(q)
                    Session("pic3") = q
                    del3.Text = "Delete"

                ElseIf LinkButton4.Text = "" Then
                    att4.Text = FileUpload1.PostedFile.ContentType
                    icon4.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton4.Text = Path.GetFileName(q)
                    Session("pic4") = q
                    del4.Text = "Delete"

                Else
                    Show_Alert(False, "You cannot add more than 4 attachments.")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub del1_Click(sender As Object, e As EventArgs) Handles del1.Click
        LinkButton1.Text = ""
        icon1.Src = ""
        del1.Text = ""
        Session("pic1") = Nothing
        If LinkButton2.Text <> "" Then
            LinkButton1.Text = LinkButton2.Text
            icon1.Src = icon2.Src
            att1.text = att2.text
            del1.Text = "Delete"
            LinkButton2.Text = ""
            icon2.Src = ""
            del2.Text = ""
            Session("pic1") = Session("pic2")
            Session("pic2") = Nothing
        End If
        If LinkButton3.Text <> "" Then
            LinkButton2.Text = LinkButton3.Text
            icon2.Src = icon3.Src
            att2.text = att3.text
            del2.Text = "Delete"
            LinkButton3.Text = ""
            icon3.Src = ""
            del3.Text = ""
            Session("pic2") = Session("pic3")
            Session("pic3") = Nothing
        End If
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.text = att4.text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
        If LinkButton1.Text = "" Then
            Panel1.Visible = False
        End If
    End Sub

    Protected Sub del2_Click(sender As Object, e As EventArgs) Handles del2.Click
        LinkButton2.Text = ""
        icon2.Src = ""
        del2.Text = ""
        Session("pic2") = Nothing
        If LinkButton3.Text <> "" Then
            LinkButton2.Text = LinkButton3.Text
            att2.Text = att3.Text
            icon2.Src = icon3.Src
            del2.Text = "Delete"
            LinkButton3.Text = ""
            icon3.Src = ""
            del3.Text = ""
            Session("pic2") = Session("pic3")
            Session("pic3") = Nothing
        End If
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.Text = att4.Text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
    End Sub

    Protected Sub del3_Click(sender As Object, e As EventArgs) Handles del3.Click
        LinkButton3.Text = ""
        icon3.Src = ""
        del3.Text = ""
        Session("pic3") = Nothing
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.Text = att4.Text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
    End Sub

    Protected Sub del4_Click(sender As Object, e As EventArgs) Handles del4.Click
        LinkButton4.Text = ""
        icon4.Src = ""
        del4.Text = ""
        Session("pic4") = Nothing
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("~/content/app/staff/myplans.aspx")
    End Sub
End Class
