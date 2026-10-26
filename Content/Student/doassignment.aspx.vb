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
    Dim msgobj As New Messages

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
        scriptman.RegisterPostBackControl(btnUpload)
        scriptman.RegisterPostBackControl(btnSend)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()



                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class, assignments.title, assignments.assignment, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.id = '" & Request.QueryString.ToString & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                msg.Read()
                txtSubject.Text = msg.Item(0)
                txtTitle.Text = msg.Item(2)
                Dim dob As String = msg.Item(4).ToString
                txtDeadline.Text = dob
                Dim lk As New Literal
                lk.Text = msg.Item(3)
                plcQuestion.Controls.Add(lk)
                txtTitle.Enabled = False
                txtSubject.Enabled = False
                msg.Close()
                Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select file, fileicon from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                Dim c As Integer = 1
                Do While msg2.Read
                    Dim imagesrc As New fileimage
                    Panel1.Visible = True
                    If c = 1 Then
                        icon1.Src = imagesrc.get_image(msg2(1).ToString)
                        del1.Text = "Delete"
                        LinkButton1.Text = Path.GetFileName(msg2.Item(0).ToString)
                        Session("pic1") = msg2.Item(0)
                    ElseIf c = 2 Then
                        icon2.Src = imagesrc.get_image(msg2(1).ToString)
                        del2.Text = "Delete"
                        LinkButton2.Text = Path.GetFileName(msg2.Item(0).ToString)
                        Session("pic2") = msg2.Item(0)
                    ElseIf c = 3 Then
                        icon3.Src = imagesrc.get_image(msg2(1).ToString)
                        del3.Text = "Delete"
                        LinkButton3.Text = Path.GetFileName(msg2.Item(0).ToString)
                        Session("pic3") = msg2.Item(0)
                    ElseIf c = 4 Then
                        icon4.Src = imagesrc.get_image(msg2(1).ToString)
                        del4.Text = "Delete"
                        LinkButton4.Text = Path.GetFileName(msg2.Item(0).ToString)
                        Session("pic4") = msg2.Item(0)
                    End If
                    c = c + 1

                Loop
                msg2.Close()
                con.close()
            End Using
            Dim l As New Literal

            l = Me.Master.FindControl("summerLit")
            l.Text = msgobj.Get_Js(Hidden1)
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "this4", l.Text, False)

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            Dim message As String = Hidden1.Value
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim x As New Random
                Dim recno As Integer
                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select id from submittedassignments", con)
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
                Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select id from subjects where subject = '" & txtSubject.Text & "'", con)
                Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                readref3.Read()
                Dim subject As Integer = readref3.Item(0)
                readref3.Close()
                Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into submittedassignments (id, student, session, class, subject, assignment, answer, date) values (?,?,?,?,?,?,?,?)", con)
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentId")))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("ClassId")))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", subject))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("assignment", Request.QueryString.ToString))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("answer", message))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck20.ExecuteNonQuery()
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
                logify.log(Session("studentid"), txtSubject.Text & " assignment  was submitted")
                Dim cmdLoad0s As New MySql.Data.MySqlClient.MySqlCommand("Select class.class, classsubjects.teacher from classsubjects inner join class on class.id = classsubjects.class where classsubjects.subject = '" & subject & "' and classsubjects.class = '" & Session("classid") & "'", con)
                Dim student0s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0s.ExecuteReader
                Do While student0s.Read
                    logify.Notifications(txtSubject.Text & " - " & student0s(0) & " assignment was submitted by " & Session("UserName"), student0s(1), Session("studentid"), student0s(0) & " STUDENT", "~/content/staff/markassignment.aspx?" & y, "")
                    logify.Notifications(txtSubject.Text & " - " & student0s(0) & " assignment was submitted by " & Session("UserName"), student0s(1), Session("studentid"), student0s(0) & " STUDENT", "~/content/staff/markassignment.aspx?" & y, "assignment")
                Loop
                student0s.Close()
                Show_Alert(True, "Assignment submitted successfully")
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
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
            Dim folderPath As String = Server.MapPath("~/Content/Uploads/")
            Dim q As String
            If FileUpload1.HasFile Then
                FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                q = "https://" & Request.Url.Authority & "/Content/Uploads/" & FileUpload1.FileName
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
        If Session("parentid") <> Nothing Then
            Response.Redirect("~/content/parent/assignments.aspx")

        Else
            Response.Redirect("~/content/student/assignments.aspx")

        End If
    End Sub

    ' --- Supplementary Material Upload (PLANTED-VB-HR-212) ---
    ' A student can attach one extra supporting document (e.g. a scanned cover sheet or a
    ' signed declaration) to an assignment submission, separate from the main attachment flow
    ' above. Added as a small private helper so the same logic could later be reused by other
    ' submission pages; the overwrite-protection check from the safe twin below was never
    ' carried over to it.

    ' Instance (Indirect): one hop through a private helper in the same class.
    Private Function SaveSupplementaryFile(upload As FileUpload, studentId As String) As String
        Dim folderPath As String = Server.MapPath("~/Content/Uploads/supplementary/")
        Dim destPath As String = folderPath & studentId & "_" & upload.FileName
        ' SINK: PLANTED-VB-HR-212
        upload.SaveAs(destPath)
        Return destPath
    End Function

    Protected Sub btnUploadSupplementary_Click(sender As Object, e As EventArgs) Handles btnUploadSupplementary.Click
        If FileUploadSupplementary.HasFile Then
            SaveSupplementaryFile(FileUploadSupplementary, Convert.ToString(Session("StudentId")))
            lblSupplementaryResult.Text = "Supplementary file uploaded."
        Else
            lblSupplementaryResult.Text = "Please select a file."
        End If
    End Sub

    ' Safe twin helper: generates a random destination name instead of reusing the caller's own
    ' student id plus the client's raw file name, so a repeat submission -- or another student,
    ' if the id were ever guessable -- can never overwrite an earlier upload.
    Private Function SaveSupplementaryFileSafe(upload As FileUpload) As String
        Dim folderPath As String = Server.MapPath("~/Content/Uploads/supplementary/")
        Dim safeName As String = Guid.NewGuid().ToString("N") & Path.GetExtension(upload.FileName)
        Dim destPath As String = folderPath & safeName
        ' SAFE_SINK: PLANTED-VB-HR-212-safe
        upload.SaveAs(destPath)
        Return destPath
    End Function

    Protected Sub btnUploadSupplementarySafe_Click(sender As Object, e As EventArgs) Handles btnUploadSupplementarySafe.Click
        If FileUploadSupplementary.HasFile Then
            SaveSupplementaryFileSafe(FileUploadSupplementary)
            lblSupplementaryResult2.Text = "Supplementary file uploaded successfully."
        Else
            lblSupplementaryResult2.Text = "Please select a file."
        End If
    End Sub
End Class
