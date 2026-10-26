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
        scriptman.RegisterPostBackControl(cboCategory)
        scriptman.RegisterPostBackControl(btnSend)
        scriptman.RegisterPostBackControl(Button1)
        scriptman.RegisterPostBackControl(del1)
        scriptman.RegisterPostBackControl(del2)
        scriptman.RegisterPostBackControl(del3)
        scriptman.RegisterPostBackControl(del4)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Parent(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim ds As New DataTable
                ds.Columns.Add("name")
                If Not IsPostBack Then
                    cboCategory.Items.Add("SELECT")
                    cboCategory.Items.Add("Admin")
                    cboCategory.Items.Add("Accounts")

                   
                    Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class, depts.headtitle, depts.dept, depts.superior from studentsummary inner join (studentsprofile inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentId) on parentward.ward = studentsprofile.admno) on studentsprofile.admno = studentsummary.student inner join (class inner join depts on depts.id = class.superior) on studentsummary.class = class.Id where parentprofile.parentId = '" & Session("ParentID") & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                    Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader
                    Dim parent2 As New ArrayList
                    Dim sup As New ArrayList
                    Do While student2.Read
                       
                        If Not parent2.Contains(student2.Item(1) & " - " & student2.Item(2)) Then
                            parent2.Add(student2.Item(1) & " - " & student2.Item(2))
                            cboCategory.Items.Add(student2.Item(1) & " - " & student2.Item(2))
                        End If
                        If Not sup.Contains(student2.Item(3)) Then
                            sup.Add(student2.Item(3))
                        End If
                    Loop
                    student2.Close()
                    Dim sup2 As New ArrayList
                    For Each item As String In sup
                        Dim cmdLoad20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT headtitle, superior from depts where dept = '" & item & "'", con)
                        Dim student20 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad20.ExecuteReader
                        Do While student20.Read
                            cboCategory.Items.Add(student20.Item(0) & " - " & item)
                            sup2.Add(student20.Item(1))
                        Loop
                        student20.Close()
                    Next
                    Dim sup3 As New ArrayList
                    For Each item As String In sup2
                        Dim cmdLoad20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT headtitle, superior from depts where dept = '" & item & "'", con)
                        Dim student20 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad20.ExecuteReader
                        Do While student20.Read
                            cboCategory.Items.Add(student20.Item(0) & " - " & item)
                            sup3.Add(student20.Item(1))
                        Loop
                        student20.Close()
                    Next
                    Dim sup4 As New ArrayList
                    For Each item As String In sup3
                        Dim cmdLoad20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT headtitle, superior from depts where dept = '" & item & "'", con)
                        Dim student20 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad20.ExecuteReader
                        Do While student20.Read
                            cboCategory.Items.Add(student20.Item(0) & " - " & item)
                            sup4.Add(student20.Item(1))
                        Loop
                        student20.Close()
                    Next
                    Dim sup5 As New ArrayList
                    For Each item As String In sup4
                        Dim cmdLoad20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT headtitle, superior from depts where dept = '" & item & "'", con)
                        Dim student20 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad20.ExecuteReader
                        Do While student20.Read
                            cboCategory.Items.Add(student20.Item(0) & " - " & item)
                            sup5.Add(student20.Item(1))
                        Loop
                        student20.Close()
                    Next
                    For Each item As String In sup5
                        Dim cmdLoad20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT headtitle, superior from depts where dept = '" & item & "'", con)
                        Dim student20 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad20.ExecuteReader
                        Do While student20.Read
                            cboCategory.Items.Add(student20.Item(0) & " - " & item)
                        Loop
                        student20.Close()
                    Next
                End If
                If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select messages.sender, messages.sendertype, messages.subject, messages.message, messages.receivertype, sentmsgs.sendertype, sentmsgs.receiverreply from messages inner join sentmsgs on sentmsgs.id = messages.id where messages.id = '" & Request.QueryString.ToString & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    msg.Read()
                    Dim sender1 As String = msg.Item(0)
                    If Session("responsetype") = "Reply" Then
                        txtSubject.Text = "Re: " & msg.Item(2)
                        cboCategory.Items.Add(msg.Item(5))
                        Session("ReceiverReply") = msg.Item(5)
                        Session("Relationship") = msg.Item(6)
                        cboCategory.Text = msg.Item(5)
                    Else
                        txtSubject.Text = msg.Item(2)
                        FreeTextBox1.Text = msg.Item(3)
                    End If


                    msg.Close()
                    If Session("responsetype") = "Reply" Then
                        If cboCategory.Text = "Admin" Then
                            ds.Rows.Add("Admin")
                        ElseIf cboCategory.Text = "Accounts" Then
                            ds.Rows.Add("Accounts")
                        Else
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sender1 & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            ds.Rows.Add(student1.Item(0))
                            student1.Close()
                            cboCategory.Enabled = False
                        End If
                    End If

                    If Session("responsetype") = "Forward" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select file from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                        Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                        Dim c As Integer = 1
                        Do While msg2.Read
                            Dim imagesrc As New fileimage
                            Panel1.Visible = True
                            If c = 1 Then
                                icon1.Src = imagesrc.get_image(msg2(1).ToString)
                                del1.Text = "Delete"
                                LinkButton1.Text = Path.GetFileName(msg2.Item(0))
                                Session("pic1") = msg2.Item(0)
                            ElseIf c = 2 Then
                                icon2.Src = imagesrc.get_image(msg2(1).ToString)
                                del2.Text = "Delete"
                                LinkButton2.Text = Path.GetFileName(msg2.Item(0))
                                Session("pic2") = msg2.Item(0)
                            ElseIf c = 3 Then
                                icon3.Src = imagesrc.get_image(msg2(1).ToString)
                                del3.Text = "Delete"
                                LinkButton3.Text = Path.GetFileName(msg2.Item(0))
                                Session("pic3") = msg2.Item(0)
                            ElseIf c = 4 Then
                                icon4.Src = imagesrc.get_image(msg2(1).ToString)
                                del4.Text = "Delete"
                                LinkButton4.Text = Path.GetFileName(msg2.Item(0))
                                Session("pic4") = msg2.Item(0)
                            End If
                            c = c + 1

                        Loop
                        msg2.Close()
                    End If

                End If

                FreeTextBox1.Focus = True


                For Each row As GridViewRow In gridRecipients.Rows
                    ds.Rows.Add(row.Cells(0).Text)
                Next
                gridRecipients.DataSource = ds
                gridRecipients.DataBind()
                Dim l As New Literal

                l = Me.Master.FindControl("summerLit")
                l.Text = msgobj.Get_Js(Hidden1)
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "this4", l.Text, False)

                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            Dim j As String = cboCategory.Text
            Dim message As String = Hidden1.Value
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim x As New Random
                Dim recno As Integer
                Dim recipient As New ArrayList
                Dim no As Integer
                For Each row As GridViewRow In gridRecipients.Rows
                    If Not recipient.Contains(row.Cells(0).Text) Then
                        recipient.Add(row.Cells(0).Text)
                        no = no + 1
                    End If
                Next
                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select id from messages", con)
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
                For Each item As String In recipient

                    Dim z As String
                    If cboCategory.Text = "Admin" Then
                        z = "Admin"
                    ElseIf cboCategory.Text = "Accounts" Then
                        z = "Accounts"
                    Else
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StaffProfile where surname = ?", con)
                        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Trim(item)))
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        student.Read()
                        z = student.Item(0)
                        student.Close()
                    End If
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into messages (id, sender, receiver, subject, message, date, session, sendertype, receivertype) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("ParentID")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", z))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", "Parent"))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", IIf(j = "Admin", "Admin", IIf(j = "Accounts", "Accounts", "Staff"))))
                    cmdCheck20.ExecuteNonQuery()
                    Dim s as new ds_functions
                     Dim msgobj as new Messages
                        If z = "Admin" Then
                        Dim d As ArrayList = s.Fetch_admins
                        For Each q In d
                             msgobj.Send_Email(msgobj.Get_Email(q), txtSubject.Text, message, "http://" & Request.Url.Authority, Session("Relationship"), s.Get_Parent_Name(Session("parentid")))

                        Next
                    ElseIf z = "Accounts" Then
                        Dim d As ArrayList = s.Fetch_accounts
                         For Each q In d
                             msgobj.Send_Email(msgobj.Get_Email(q), txtSubject.Text, message, "http://" & Request.Url.Authority, Session("Relationship"), s.Get_Parent_Name(Session("parentid")))

                        Next
                            else

                             msgobj.Send_Email(msgobj.Get_Email(z), txtSubject.Text, message, "http://" & Request.Url.Authority, Session("Relationship"), s.Get_Parent_Name(Session("parentid")))

                        end if
 
                    If z = "Admin" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT username from admin", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            logify.Notifications("You have a new message - " & txtSubject.Text, reader(0).ToString, Session("parentid"), Session("Relationship"), "~/content/admin/readmsg.aspx?" & y, "Message")

                        Loop
                        reader.Close()
                    ElseIf z = "Accounts" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT username from acclogin", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            logify.Notifications("You have a new message - " & txtSubject.Text, reader(0).ToString, Session("parentid"), Session("Relationship"), "~/content/account/readmsg.aspx?" & y, "Message")

                        Loop
                        reader.Close()
                    Else
                        logify.Notifications("You have a new message - " & txtSubject.Text, z, Session("parentid"), Session("Relationship"), "~/content/staff/readmsg.aspx?" & y, "Message")
                    End If
                    recno = recno + 1
                Next
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


                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into sentmsgs (id, sender, receiver, subject, message, date, session, sendertype, receivertype, receiverreply) Values (?,?,?,?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("ParentID")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", recno & " Recipients"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", Session("Relationship")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", j))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiverreply", Session("ReceiverReply")))

                cmdCheck2.ExecuteNonQuery()

                logify.log(Session("parentid"), "A message was sent to " & cboCategory.Text & ". Message ref = " & y)
                Show_Alert(True, "Message sent successfully.")
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Response.Redirect("~/content/parent/messages.aspx")
    End Sub




    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim ds As New DataTable
            ds.Columns.Add("name")
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                If cboCategory.Text = "SELECT" Then
                    Show_Alert(False, "Please select a category")
                ElseIf cboCategory.Text = "Admin" Then
                    ds.rows.Add("Admin")
                ElseIf cboCategory.Text = "Accounts" Then
                    ds.rows.Add("Accounts")
                Else
                    Dim spec As Array = Split(cboCategory.Text, "-")
                    If Trim(spec(1)) = "CLASS TEACHER" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid inner join class on classteacher.class = class.id where class.class = '" & RTrim(spec(0)) & "'", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        Do While student11.Read
                            ds.rows.Add(student11(0))
                            Session("ReceiverReply") = cboCategory.Text
                        Loop
                        student11.Close()
                        Session("Relationship") = spec(0) & " PARENT"
                    ElseIf spec.Length = 3 Then
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classsubjects inner join class on classsubjects.class = class.id inner join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject where class.class = '" & RTrim(spec(0)) & "' and subjects.subject = '" & LTrim(spec(1)) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))
                        Loop
                        student.Close()
                        Session("ReceiverReply") = cboCategory.Text
                        Session("Relationship") = spec(0) & " PARENT"
                    Else
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.headtitle = '" & RTrim(spec(0)) & "' and depts.dept = '" & LTrim(spec(1)) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))
                        Loop
                        student.Close()
                        Session("ReceiverReply") = cboCategory.Text
                        Session("Relationship") = "PARENT"
                    End If
                End If
                If Session("Relationship") = "" Then Session("Relationship") = "PARENT"
                gridRecipients.DataSource = ds
                gridRecipients.DataBind()
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedIndexChanged
        recCategory = cboCategory.Text
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


    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Response.Redirect("~/content/parent/sentmsgs.aspx")
    End Sub

    Protected Sub gridRecipients_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gridRecipients.RowDeleting
        Try


            Dim rows As GridViewRow = gridRecipients.Rows(e.RowIndex)
            Dim DS As New DataTable
            DS.Columns.Add("name")
            For Each row As GridViewRow In gridRecipients.Rows
                If Not row Is rows Then
                    DS.Rows.Add(row.Cells(0).Text)
                End If
            Next
            gridRecipients.DataSource = ds
            gridRecipients.DataBind()
        Catch ex As Exception

        End Try
    End Sub
End Class
