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
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ds As New DataTable
        ds.Columns.Add("name")
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select messages.sender, messages.sendertype, messages.subject, messages.message, messages.receivertype, sentmsgs.sendertype, sentmsgs.receiverreply from messages inner join sentmsgs on sentmsgs.id = messages.id where messages.id = '" & Request.QueryString.ToString & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    msg.Read()
                    Dim sender1 As String = msg.Item(0)
                    If Session("responsetype") = "Reply" Then
                        cboReceivetype.Text = msg.Item(1)
                        txtSubject.Text = "Re " & msg.Item(2)
                        cboCategory.Items.Add(msg.Item(5))
                        Session("ReceiverReply") = msg.Item(5)
                        Session("Relationship") = msg.Item(6)
                        If msg.Item(1).ToString = "Admin" Then cboReceivetype.Text = "Staff"
                    Else
                        txtSubject.Text = msg.Item(2)
                        FreeTextBox1.Text = msg.Item(3)

                    End If


                    msg.Close()
                    If Session("responsetype") = "Reply" Then
                        If cboReceivetype.Text = "Parent" Then
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sender1 & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            student.Read()
                            ds.Rows.Add(student.Item(0))
                            student.Close()
                        ElseIf cboReceivetype.Text = "Staff" Then
                            If cboCategory.Text = "Admin" Then
                                ds.Rows.Add("Admin")
                            Else


                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sender1 & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                ds.Rows.Add(student1.Item(0))
                                student1.Close()
                            End If
                        End If
                        cboReceivetype.Enabled = False
                        cboCategory.Enabled = False

                    End If
                    If Session("responsetype") = "Forward" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select file, fileicon from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                        Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                        Dim c As Integer = 1
                        Do While msg2.Read
                            Dim imagesrc As New fileimage
                            If c = 1 Then
                                Panel1.Visible = True
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
                receiver = cboReceivetype.Text
                con.Close()            End Using
            For Each row As GridViewRow In gridRecipients.Rows
                ds.Rows.Add(row.Cells(0).Text)
            Next
            gridRecipients.DataSource = ds
            gridRecipients.DataBind()
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
                    If receiver = "Student" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT admno from StudentsProfile where surname = ?", con)
                        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", item))
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        student.Read()
                        z = student.Item(0)
                        student.Close()
                    ElseIf receiver = "Staff" Then
                        If cboCategory.Text = "Admin" Then
                            z = "Admin"
                        Else
                            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StaffProfile where surname = ?", con)
                            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", item))
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                            student.Read()
                            z = student.Item(0)
                            student.Close()

                        End If
                    ElseIf receiver = "Parent" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentId from parentProfile where parentname = ?", con)
                        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", item))
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        student.Read()
                        z = student.Item(0)
                        student.Close()
                    End If
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into messages (id, sender, receiver, subject, message, date, session, sendertype, receivertype) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("StaffId")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", z))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", "Accounts"))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", cboReceivetype.Text))
                    cmdCheck20.ExecuteNonQuery()
                    recno = recno + 1
                    If z = "Admin" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT username from admin", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            logify.Notifications("You have a new message - " & txtSubject.Text, reader(0).ToString, Session("staffid"), "Accounts", "~/content/app/admin/readmsg.aspx?" & y, "Message")

                        Loop
                        reader.Close()
                    ElseIf receiver = "Staff" Then
                        logify.Notifications("You have a new message from Accounts - " & txtSubject.Text, z, Session("staffid"), "Accounts", "~/content/app/staff/readmsg.aspx?" & y, "Message")
                    ElseIf receiver = "Parent" Then
                        logify.Notifications("You have a new message from Accounts - " & txtSubject.Text, z, Session("staffid"), "Accounts", "~/content/app/parent/readmsg.aspx?" & y, "Message")
                    End If
                Next
                If LinkButton1.Text <> "" Then
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file) values (?,?)", con)
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic1")))
                    cmdCheck.ExecuteNonQuery()
                End If
                If LinkButton2.Text <> "" Then
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file) values (?,?)", con)
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic2")))
                    cmdCheck.ExecuteNonQuery()
                End If
                If LinkButton3.Text <> "" Then
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file) values (?,?)", con)
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic3")))
                    cmdCheck.ExecuteNonQuery()
                End If
                If LinkButton4.Text <> "" Then
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file) values (?,?)", con)
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic4")))
                    cmdCheck.ExecuteNonQuery()
                End If

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into sentmsgs (id, sender, receiver, subject, message, date, session, sendertype, receivertype, receiverreply) Values (?,?,?,?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("StaffId")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", recno & " Recipients"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", "Accounts"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", cboCategory.Text))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiverreply", Session("ReceiverReply")))

                cmdCheck2.ExecuteNonQuery()
                logify.log(Session("staffid"), "A message was sent to " & cboCategory.Text & ". Message ref = " & y)
                Show_Alert(True, "Message sent successfully.")
                con.Close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Response.Redirect("~/content/app/account/messages.aspx")
    End Sub




    Protected Sub cboReceivetype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboReceivetype.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                cboCategory.Items.Clear()
                If cboReceivetype.Text = "Staff" Then
                    cboCategory.Items.Add("SELECT")
                    cboCategory.Items.Add("Admin")
                    Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle from depts", con)
                    Dim student01 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01.ExecuteReader
                    cboCategory.Items.Add("ALL STAFF")
                    Do While student01.Read
                        cboCategory.Items.Add(student01.Item(0) & " - STAFF")
                        cboCategory.Items.Add(student01.Item(0) & " - " & student01.Item(1))
                    Loop
                    cboCategory.Items.Add("CLASS TEACHERS")
                    student01.Close()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("Select depts.dept from class inner join depts on depts.id = class.superior", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    Dim deptclass As New ArrayList
                    Do While student.Read()
                        If Not deptclass.Contains(student.Item(0) & " - CLASS TEACHERS") Then
                            deptclass.Add(student.Item(0) & " - CLASS TEACHERS")
                            cboCategory.Items.Add(student.Item(0) & " - CLASS TEACHERS")
                        End If
                    Loop
                    student.Close()
                ElseIf cboReceivetype.Text = "Student" Then
                    Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class", con)
                    Dim student01 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01.ExecuteReader
                    cboCategory.Items.Clear()
                    cboCategory.Items.Add("All Students")
                    Do While student01.Read
                        cboCategory.Items.Add(student01.Item(0) & " - STUDENTS")
                    Loop
                    student01.Close()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("Select depts.dept from class inner join depts on depts.id = class.superior", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    Dim deptclass As New ArrayList
                    Do While student.Read()
                        If Not deptclass.Contains(student.Item(0) & " - STUDENTS") Then
                            deptclass.Add(student.Item(0) & " - STUDENTS")
                            cboCategory.Items.Add(student.Item(0) & " - STUDENTS")
                        End If
                    Loop
                    student.Close()
                ElseIf cboReceivetype.Text = "Parent" Then
                    Dim cmdLoad02 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class", con)
                    Dim student02 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad02.ExecuteReader
                    cboCategory.Items.Clear()
                    cboCategory.Items.Add("ALL PARENTS")
                    cboCategory.Items.Add("DEBTORS")
                    Do While student02.Read
                        cboCategory.Items.Add(student02.Item(0) & " - PARENTS")
                    Loop
                    student02.Close()
                End If
                Dim classgroups As New ArrayList
                Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select depts.dept from class inner join depts on class.superior = depts.Id", con)
                Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                Do While schclass.Read
                    If Not classgroups.Contains(schclass.Item(0)) Then
                        classgroups.Add(schclass.Item(0))
                    End If
                Loop
                schclass.Close()
                For Each i As String In classgroups
                    If cboReceivetype.Text = "Staff" Then
                        cboCategory.Items.Add(i & " - CLASS TEACHERS")
                    ElseIf cboReceivetype.Text = "Student" Then
                        cboCategory.Items.Add(i & " - STUDENTS")
                    ElseIf cboReceivetype.Text = "Parent" Then
                        cboCategory.Items.Add(i & " - PARENTS")
                    End If
                Next

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("name")
                Dim spec As Array = Split(cboCategory.Text, "-")
                If cboReceivetype.Text = "Staff" Then
                    If cboCategory.Text = "ALL STAFF" Then
                        Session("ReceiverReply") = "STAFF"
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))

                        Loop
                        student.Close()
                    ElseIf cboCategory.Text = "CLASS TEACHERS" Then
                        Session("ReceiverReply") = "CLASS TEACHER"
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join classteacher on classteacher.teacher = staffprofile.staffid", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        Do While student11.Read
                            ds.rows.Add(student11(0))

                        Loop
                        student11.Close()
                    ElseIf cboCategory.Text = "SELECT" Then
                        Show_Alert(False, "Please select a category")
                    ElseIf cboCategory.Text = "Admin" Then
                        ds.rows.Add("Admin")
                    ElseIf LTrim(spec(1)) = "CLASS TEACHERS" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join (class inner join depts on depts.Id = class.superior) on classteacher.class = class.Id inner join staffprofile on staffprofile.staffid = classteacher.teacher where depts.dept = '" & RTrim(spec(0)) & "'", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        Do While student11.Read
                            ds.rows.Add(student11(0))

                            Session("ReceiverReply") = RTrim(spec(0)) & " CLASS TEACHER"
                        Loop
                        student11.Close()
                    ElseIf LTrim(spec(1)) = "STAFF" And cboCategory.Text <> "ALL STAFF" Then
                        Session("ReceiverReply") = RTrim(spec(0)) & " STAFF"
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join (staffdept inner join depts on depts.id = staffdept.dept) on staffprofile.staffid = staffdept.staff where depts.dept = '" & RTrim(spec(0)) & "'", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        Do While student11.Read
                            ds.rows.Add(student11(0))
                        Loop
                        student11.Close()
                        Dim firstsub As ArrayList = Get_subordinates(RTrim(spec(0)))
                        Dim secsub As New ArrayList
                        Dim thirdsub As New ArrayList
                        Dim fourthsub As ArrayList
                        Dim fifthsub As New ArrayList
                        Dim sixsub As New ArrayList
                        Dim sevsub As New ArrayList
                        Dim eightsub As New ArrayList
                        Dim ninesub As New ArrayList
                        Dim tensub As New ArrayList

                        For Each item As String In firstsub
                            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join (staffdept inner join depts on depts.id = staffdept.dept) on staffprofile.staffid = staffdept.staff where depts.dept = '" & item & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                            Do While student1.Read
                                ds.rows.Add(student1(0))
                            Loop
                            student1.Close()
                            secsub = Get_subordinates(item)
                        Next
                        If secsub.Count <> 0 Then
                            For Each item As String In secsub
                                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join (staffdept inner join depts on depts.id = staffdept.dept) on staffprofile.staffid = staffdept.staff where depts.dept = '" & item & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                Do While student1.Read
                                    ds.rows.Add(student1(0))
                                Loop
                                student1.Close()
                                thirdsub = Get_subordinates(item)
                            Next
                        End If
                        If thirdsub.Count <> 0 Then
                            For Each item As String In secsub
                                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join (staffdept inner join depts on depts.id = staffdept.dept) on staffprofile.staffid = staffdept.staff where depts.dept = '" & item & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                Do While student1.Read
                                    ds.rows.Add(student1(0))
                                Loop
                                student1.Close()
                                fourthsub = Get_subordinates(item)
                            Next
                        End If
                    Else
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join depts on depts.head = staffprofile.staffid where depts.dept = '" & RTrim(spec(0)) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))
                        Loop
                        student.Close()
                        Session("ReceiverReply") = cboCategory.Text
                    End If
                ElseIf cboReceivetype.Text = "Student" Then
                    If Not cboCategory.Text = "All Students" Then
                        Session("ReceiverReply") = RTrim(spec(0)) & " STUDENT"
                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from class where class = '" & RTrim(spec(0)) & "'", con)
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        student0.Read()

                        Dim thisclass As Integer = student0.Item(0)

                        student0.Close()
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from studentsummary inner join class on studentsummary.class = class.id inner join studentsprofile on studentsummary.student = studentsprofile.admno where studentsummary.session = '" & 31 & "' and studentsummary.class = '" & thisclass & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))
                        Loop
                        student.Close()
                    Else
                        Session("ReceiverReply") = "STUDENT"
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from studentsprofile", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))
                        Loop
                        student.Close()
                    End If
                ElseIf cboReceivetype.Text = "Parent" Then
                    If Not cboCategory.Text = "ALL PARENTS" Then
                        Session("ReceiverReply") = RTrim(spec(0)) & " PARENT"

                        If cboCategory.Text = "DEBTORS" Then
                            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT feeschedule.student, studentsprofile.surname, feeschedule.amount, feeschedule.paid, class.class, parentprofile.parentname from feeschedule inner join (studentsprofile inner join (parentward inner join parentprofile on parentprofile.parentid = parentward.parent) on studentsprofile.admno = parentward.ward) on feeschedule.student = studentsprofile.admno inner join class on feeschedule.class = class.Id where feeschedule.session = ?", con)
                            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
                            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                            Dim students As New ArrayList
                            Dim total As Integer = 0
                            Dim count As Integer = 0
                            Dim currentstudent As String = ""
                            Do While balreader.Read
                                If balreader.Item(2) - balreader.Item(3) > 0 Then
                                    If Not students.Contains(balreader(5).ToString) Then
                                        total = 0
                                        students.Add(balreader(5).ToString)
                                        currentstudent = balreader(5).ToString
                                        total = balreader.Item(2) - balreader.Item(3)
                                        count = count + 1
                                        ds.rows.Add(balreader(1))
                                    Else
                                        total = total + (balreader.Item(2) - balreader.Item(3))
                                    End If
                                End If
                            Loop
                            balreader.Close()
                            count = Nothing
                            total = Nothing
                            currentstudent = Nothing
                            students = Nothing

                        Else
                            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from class where class = '" & RTrim(spec(0)) & "'", con)
                            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                            Dim thisclass As Integer
                            If student0.Read() Then
                                thisclass = student0.Item(0)
                            End If
                            student0.Close()
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname from studentsummary inner join (studentsprofile inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentId) on parentward.ward = studentsprofile.admno) on studentsprofile.admno = studentsummary.student where studentsummary.session = '" & Session("SessionId") & "' and studentsummary.class = '" & thisclass & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            Dim parents As New ArrayList
                            Do While student.Read
                                If Not parents.Contains(student.Item(0)) Then
                                    parents.Add(student.Item(0))
                                    ds.rows.Add(student(0))
                                End If
                            Loop
                            student.Close()
                            Dim clashead As New MySql.Data.MySqlClient.MySqlCommand("Select parentprofile.parentname, depts.headtitle, depts.dept from studentsummary inner join (class inner join depts on depts.Id = class.superior) on studentsummary.class = class.Id inner join (studentsprofile inner join (parentward inner join parentprofile on parentprofile.parentId = parentward.parent) on studentsprofile.admno = parentward.ward) on studentsprofile.admno = studentsummary.student where depts.dept = '" & RTrim(spec(0)) & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                            Dim schhead As MySql.Data.MySqlClient.MySqlDataReader = clashead.ExecuteReader
                            Dim parents2 As New ArrayList
                            Do While schhead.Read
                                If Not parents2.Contains(schhead.Item(0)) Then
                                    parents2.Add(schhead.Item(0))
                                    ds.rows.Add(schhead(0))
                                    Session("Relationship") = schhead.Item(1) & " - " & schhead.Item(2)
                                End If
                            Loop
                            schhead.Close()
                        End If
                    Else
                        Session("ReceiverReply") = "PARENT"

                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname from parentprofile", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))
                        Loop
                        student.Close()
                    End If
                End If
                cboReceivetype.Enabled = False
                gridRecipients.DataSource = ds
                gridRecipients.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Dim subo As New ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept from depts where superior = '" & dept & "'", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

            Do While student1.Read
                subo.Add(student1.Item(0))
            Loop
            student1.Close()
            con.Close()
        End Using
        Return subo
    End Function
    Protected Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedIndexChanged
        receiver = cboReceivetype.Text
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
            Dim folderPath As String = Server.MapPath("~/content/app/Uploads/")
            Dim q As String
            If FileUpload1.HasFile Then
                FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                Dim picsource As New fileimage
                q = "http://" & Request.Url.Authority & "/Content//Uploads/" & FileUpload1.FileName
                If LinkButton1.Text = "" Then
                    icon1.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton1.Text = Path.GetFileName(q)
                    Session("pic1") = q
                    Panel1.Visible = True
                    del1.Text = "Delete"
                ElseIf LinkButton2.Text = "" Then
                    icon2.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton2.Text = Path.GetFileName(q)
                    Session("pic2") = q
                    del2.Text = "Delete"

                ElseIf LinkButton3.Text = "" Then
                    icon3.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton3.Text = Path.GetFileName(q)
                    Session("pic3") = q
                    del3.Text = "Delete"

                ElseIf LinkButton4.Text = "" Then
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
    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Response.Redirect("~/content/app/account/sentmsgs.aspx")
    End Sub
End Class
