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

   
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Dim ds As New DataTable
        ds.Columns.Add("name")
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
                        If msg.Item(1).ToString = "Admin" Or msg.Item(1).ToString = "Accounts" Then cboReceivetype.Text = "Staff"
                    Else
                        txtSubject.Text = msg.Item(2)
                        FreeTextBox1.Text = msg.Item(3)
                    End If


                    msg.Close()
                    If Session("responsetype") = "Reply" Then
                        If cboReceivetype.Text = "Student" Then
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sender1 & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            student.Read()
                            ds.Rows.Add(student.Item(0))
                            student.Close()
                        ElseIf cboReceivetype.Text = "Staff" Then
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
                            End If
                        ElseIf cboReceivetype.Text = "Parent" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sender1 & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            ds.Rows.Add(student1.Item(0))
                            student1.Close()
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
                Session("responsetype") = Nothing
                For Each row As GridViewRow In gridRecipients.Rows
                    ds.Rows.Add(row.Cells(0).Text)
                Next
                gridRecipients.DataSource = ds
                gridRecipients.DataBind()
                FreeTextBox1.Focus = True
                receiver = cboReceivetype.Text
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            If gridRecipients.Rows.Count = 0 Then
                Show_Alert(False, "Please add recipients.")
                Exit Sub
            End If
            Dim j As String = cboCategory.Text


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
                    If receiver = "Parent" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentId from parentProfile where parentname = ?", con)
                        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", item))
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        student.Read()
                        z = student.Item(0)
                        student.Close()
                    ElseIf receiver = "Student" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT admno from StudentsProfile where surname = ?", con)
                        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", item))
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        student.Read()
                        z = student.Item(0)
                        student.Close()
                    ElseIf receiver = "Staff" Then
                        If cboCategory.Text = "Admin" Then
                            z = "Admin"
                        ElseIf cboCategory.Text = "Accounts" Then
                            z = "Accounts"
                        Else
                            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StaffProfile where surname = ?", con)
                            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", item))
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                            student.Read()
                            z = student.Item(0)
                            student.Close()
                        End If
                    End If


                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into messages (id, sender, receiver, subject, message, date, session, sendertype, receivertype) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("StaffID")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", z))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", "Staff"))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", j))
                    cmdCheck20.ExecuteNonQuery()
                    If z = "Admin" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT username from admin", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            logify.Notifications("You have a new message - " & txtSubject.Text, reader(0).ToString, Session("staffid"), Session("Relationship"), "~/content/app/admin/readmsg.aspx?" & y, "Message")

                        Loop
                        reader.Close()
                    ElseIf z = "Accounts" Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT username from acclogin", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            logify.Notifications("You have a new message - " & txtSubject.Text, reader(0).ToString, Session("staffid"), Session("Relationship"), "~/content/app/account/readmsg.aspx?" & y, "Message")

                        Loop
                        reader.Close()
                    ElseIf receiver = "Staff" Then
                        logify.Notifications("You have a new message - " & txtSubject.Text, z, Session("staffid"), Session("Relationship"), "~/content/app/staff/readmsg.aspx?" & y, "Message")
                    ElseIf receiver = "Parent" Then
                        logify.Notifications("You have a new message - " & txtSubject.Text, z, Session("staffid"), Session("Relationship"), "~/content/app/parent/readmsg.aspx?" & y, "Message")
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("StaffID")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", recno & " Recipients"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", Session("Relationship")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", j))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiverreply", Session("ReceiverReply")))

                cmdCheck2.ExecuteNonQuery()

                logify.log(Session("staffid"), "A message was sent to " & cboCategory.Text & ". Message ref = " & y)
                Show_Alert(True, "Message sent successfully.")
                Session("Relationship") = Nothing
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
      
        Response.Redirect("~/content/app/staff/messages.aspx")
    End Sub




    Protected Sub cboReceivetype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboReceivetype.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                cboCategory.Items.Clear()
                Dim classgroups As New ArrayList
                Dim mydept As New ArrayList
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select id, dept, superior from depts where head = '" & Session("Staffid") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                cboCategory.Items.Clear()
                If cboReceivetype.Text = "Staff" Then
                    cboCategory.Items.Add("SELECT")
                    cboCategory.Items.Add("Admin")
                    cboCategory.Items.Add("Accounts")
                End If
                Dim superior As String = ""
                Dim g As Integer
                Do While student0.Read()
                    If g = 0 Then
                        superior = student0.Item(2)
                    End If
                    mydept.Add(student0.Item(1))
                    g = g + 1
                Loop
                student0.Close()

                If superior <> "None" And superior <> "" Then
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("Select headtitle, dept from depts where dept = '" & superior & "'", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    student.Read()
                    If cboReceivetype.Text = "Staff" Then
                        cboCategory.Items.Add(student.Item(0) & " - " & student.Item(1))
                    End If
                    student.Close()
                ElseIf superior = "" Then
                    Dim dept As New ArrayList
                    Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept from staffdept inner join depts on depts.id = staffdept.dept where staffdept.staff = '" & Session("Staffid") & "'", con)
                    Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                    Do While student11.Read
                        dept.Add(student11.Item(0))
                    Loop
                    student11.Close()
                    For Each item As String In dept
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("Select headtitle, dept from depts where dept = '" & item & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read()
                            If cboReceivetype.Text = "Staff" Then
                                cboCategory.Items.Add(student.Item(0) & " - " & student.Item(1))
                            End If
                        Loop
                        student.Close()
                    Next

                End If
                Dim titles As New ArrayList
                Dim classcontroled As New ArrayList
                Dim secsub As New ArrayList
                Dim mysub As New ArrayList
                For Each item As String In mydept
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    If student.Read() And cboReceivetype.Text = "Student" Then
                        cboCategory.Items.Add(student.Item(0) & " - STUDENTS")
                    End If
                    student.Close()
                    If cboReceivetype.Text = "Staff" Then
                        cboCategory.Items.Add(item & " - STAFF")
                    End If
                    mysub = Get_subordinates(item)

                    For Each subitem As String In mysub
                        classcontroled.Add(subitem)
                        secsub.Add(subitem)
                    Next
                Next
                Dim thirdsub As New ArrayList
                For Each item As String In secsub
                    Dim cmdLoad9 As New MySql.Data.MySqlClient.MySqlCommand("Select headtitle from depts where dept = '" & item & "'", con)
                    Dim student9 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad9.ExecuteReader
                    student9.Read()
                    titles.Add(student9.Item(0))
                    student9.Close()

                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    If student.Read() And cboReceivetype.Text = "Student" Then
                        cboCategory.Items.Add(student.Item(0) & " -  STUDENTS")
                    End If
                    student.Close()
                    mysub = Get_subordinates(item)
                    For Each subitem As String In mysub
                        classcontroled.Add(subitem)
                        thirdsub.Add(subitem)
                    Next
                Next
                Dim fourthsub As New ArrayList
                For Each item As String In thirdsub
                    Dim cmdLoad9 As New MySql.Data.MySqlClient.MySqlCommand("Select headtitle from depts where dept = '" & item & "'", con)
                    Dim student9 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad9.ExecuteReader
                    student9.Read()
                    titles.Add(student9.Item(0))
                    student9.Close()

                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    If student.Read() And cboReceivetype.Text = "Student" Then
                        cboCategory.Items.Add(student.Item(0) & " - STUDENTS")
                    End If
                    student.Close()
                    mysub = Get_subordinates(item)
                    For Each subitem As String In mysub
                        classcontroled.Add(subitem)
                        fourthsub.Add(subitem)
                    Next
                Next
                Dim fifthsub As New ArrayList
                For Each item As String In fourthsub
                    Dim cmdLoad9 As New MySql.Data.MySqlClient.MySqlCommand("Select headtitle from depts where dept = '" & item & "'", con)
                    Dim student9 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad9.ExecuteReader
                    student9.Read()
                    titles.Add(student9.Item(0))
                    student9.Close()

                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    If student.Read() And cboReceivetype.Text = "Student" Then
                        cboCategory.Items.Add(student.Item(0) & " - STUDENTS")
                    End If
                    student.Close()
                    mysub = Get_subordinates(item)
                    For Each subitem As String In mysub
                        classcontroled.Add(subitem)
                    Next
                Next

                Dim k As Integer
                For Each item As String In classcontroled
                    If cboReceivetype.Text = "Staff" Then
                        cboCategory.Items.Add(item & " - STAFF")
                        cboCategory.Items.Add(titles(k) & " - " & item)
                        k = k + 1
                    End If
                Next


                For Each item As String In mydept
                    Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                    Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                    Dim d As Boolean = False
                    Do While schclass.Read
                        d = True
                        If cboReceivetype.Text = "Staff" Then
                            cboCategory.Items.Add(schclass.Item(0) & " - CLASS TEACHER")
                        ElseIf cboReceivetype.Text = "Student" Then
                            cboCategory.Items.Add(schclass.Item(0) & " - STUDENTS")
                        ElseIf cboReceivetype.Text = "Parent" Then
                            cboCategory.Items.Add(schclass.Item(0) & " - PARENTS")
                        End If
                    Loop
                    If d = True Then
                        classgroups.Add(item)
                    End If
                    schclass.Close()
                Next
                Dim clashead As New MySql.Data.MySqlClient.MySqlCommand("Select depts.headtitle, depts.dept from class inner join depts on class.superior = depts.Id where class.id = '" & Session("ClassId") & "'", con)
                Dim schhead As MySql.Data.MySqlClient.MySqlDataReader = clashead.ExecuteReader
                If schhead.Read And cboReceivetype.Text = "Staff" Then
                    cboCategory.Items.Add(schhead.Item(0) & " - " & schhead.Item(1))
                End If
                schhead.Close()

                For Each item As String In classcontroled
                    Dim f As Boolean = False
                    Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                    Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                    Do While schclass.Read
                        f = True
                        If cboReceivetype.Text = "Staff" Then
                            cboCategory.Items.Add(schclass.Item(0) & " - CLASS TEACHER")
                        ElseIf cboReceivetype.Text = "Student" Then
                            cboCategory.Items.Add(schclass.Item(0) & " - STUDENTS")
                        ElseIf cboReceivetype.Text = "Parent" Then
                            cboCategory.Items.Add(schclass.Item(0) & " - PARENTS")
                        End If
                    Loop
                    If f = True Then
                        classgroups.Add(item)
                    End If
                    schclass.Close()

                Next
                For Each i As String In classgroups
                    If cboReceivetype.Text = "Staff" Then
                        cboCategory.Items.Add(i & " - CLASS TEACHERS")
                    ElseIf cboReceivetype.Text = "Parent" Then
                        cboCategory.Items.Add(i & " - PARENTS")
                    End If
                Next

                Dim cmdCheck22 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classteacher inner join class on class.Id = classteacher.class where classteacher.teacher = '" & Session("Staffid") & "'", con)
                Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck22.ExecuteReader
                Do While msg2.Read()
                    If cboReceivetype.Text = "Student" Then
                        Dim isthere As Boolean = False
                        For Each item As ListItem In cboCategory.Items
                            If item.Text = msg2.Item(0) & " - STUDENTS" Then
                                isthere = True
                            End If
                        Next
                        If isthere = False Then cboCategory.Items.Add(msg2.Item(0) & " - STUDENTS")
                    End If
                    If cboReceivetype.Text = "Parent" Then
                        Dim isthere As Boolean = False
                        For Each item As ListItem In cboCategory.Items
                            If item.Text = msg2.Item(0) & " - PARENTS" Then
                                isthere = True
                            End If
                        Next
                        If isthere = False Then cboCategory.Items.Add(msg2.Item(0) & " - PARENTS")
                    End If
                Loop
                msg2.Close()
                Dim cmdteach As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, class.class from classsubjects  inner join class on class.Id = classsubjects.class inner join subjects on classsubjects.subject = subjects.Id where classsubjects.teacher = '" & Session("Staffid") & "'", con)
                Dim teach As MySql.Data.MySqlClient.MySqlDataReader = cmdteach.ExecuteReader
                Do While teach.Read()
                    If cboReceivetype.Text = "Student" Then
                        cboCategory.Items.Add(teach.Item(1) & " - " & teach.Item(0) & " - STUDENTS")
                    ElseIf cboReceivetype.Text = "Parent" Then
                        cboCategory.Items.Add(teach.Item(1) & " - " & teach.Item(0) & " - PARENTS")
                    End If
                Loop
                teach.Close()
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
                Dim cmdLoad40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT headtitle, dept from depts where depts.head = '" & Session("StaffId") & "'", con)
                Dim student40 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad40.ExecuteReader
                If cboReceivetype.Text = "Staff" Then
                    If student40.Read() Then
                        Session("Relationship") = "HEAD - " & student40.Item(1)
                    End If
                ElseIf cboReceivetype.Text = "Parent" Then
                    If student40.Read() And check.Check_sh(Session("roles"), Session("usertype")) = True Then
                        Session("Relationship") = "HEAD - " & student40.Item(1)
                    End If
                End If
                student40.Close()
                If cboReceivetype.Text = "Staff" Then
                    If Session("Relationship") = Nothing Then
                        Dim cmdLoad12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept from depts inner join staffdept on staffdept.dept = depts.Id where staffdept.staff = '" & Session("StaffId") & "'", con)
                        Dim student12 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad12.ExecuteReader
                        If student12.Read Then
                            Session("Relationship") = "MEMBER" & " - " & student12.Item(0)
                        End If
                        student12.Close()
                    End If

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
                        ElseIf Trim(spec(1)) = "CLASS TEACHERS" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join (class inner join depts on depts.Id = class.superior) on classteacher.class = class.Id inner join staffprofile on staffprofile.staffid = classteacher.teacher where depts.dept = '" & RTrim(spec(0)) & "'", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            Do While student11.Read
                                ds.rows.Add(student11(0))
                                Session("ReceiverReply") = RTrim(spec(0)) & " CLASS TEACHER"
                            Loop
                            student11.Close()
                        ElseIf Trim(spec(1)) = "STAFF" And cboCategory.Text <> "ALL STAFF" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffdept inner join staffprofile on staffprofile.staffid = staffdept.staff inner join depts on depts.id = staffdept.dept where depts.dept = '" & RTrim(spec(0)) & "'", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            Do While student11.Read
                                ds.rows.Add(student11(0))
                                Session("ReceiverReply") = cboCategory.Text
                            Loop
                            student11.Close()
                            Dim firstsub As ArrayList = Get_subordinates(RTrim(spec(0)))
                            Dim secsub As New ArrayList
                            Dim thirdsub As New ArrayList
                            Dim fourthsub As New ArrayList
                            Dim fifthsub As New ArrayList
                            Dim sixsub As New ArrayList
                            Dim sevsub As New ArrayList
                            Dim eightsub As New ArrayList
                            Dim ninesub As New ArrayList
                            Dim tensub As New ArrayList
                            Session("ReceiverReply") = cboCategory.Text
                            For Each item As String In firstsub
                                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.dept = '" & item & "'", con)
                                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                Do While student.Read
                                    ds.rows.Add(student(0))
                                Loop
                                student.Close()

                                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffdept inner join staffprofile on staffprofile.staffid = staffdept.staff inner join depts on depts.id = staffdept.dept where depts.dept = '" & item & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                Do While student1.Read
                                    ds.rows.Add(student1(0))
                                Loop
                                student1.Close()
                                Dim subsub As New ArrayList
                                subsub = Get_subordinates(item)
                                For Each subitem As String In subsub
                                    secsub.Add(subitem)
                                Next
                            Next
                            If secsub.Count <> 0 Then
                                For Each item As String In secsub
                                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.dept = '" & item & "'", con)
                                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                    Do While student.Read
                                        ds.rows.Add(student(0))
                                    Loop
                                    student.Close()
                                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffdept inner join staffprofile on staffprofile.staffid = staffdept.staff inner join depts on depts.id = staffdept.dept where depts.dept = '" & item & "'", con)
                                    Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                    Do While student1.Read
                                        ds.rows.Add(student1(0))
                                    Loop
                                    student1.Close()
                                    Dim subsub As New ArrayList
                                    subsub = Get_subordinates(item)
                                    For Each subitem As String In subsub
                                        thirdsub.Add(subitem)
                                    Next
                                Next
                            End If
                            If thirdsub.Count <> 0 Then
                                For Each item As String In thirdsub
                                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.dept = '" & item & "'", con)
                                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                    Do While student.Read
                                        ds.rows.Add(student(0))
                                    Loop
                                    student.Close()
                                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffdept inner join staffprofile on staffprofile.staffid = staffdept.staff inner join depts on depts.id = staffdept.dept where depts.dept = '" & item & "'", con)
                                    Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                    Do While student1.Read
                                        ds.rows.Add(student1(0))
                                    Loop
                                    student1.Close()
                                    Dim subsub As New ArrayList
                                    subsub = Get_subordinates(item)
                                    For Each subitem As String In subsub
                                        fourthsub.Add(subitem)
                                    Next
                                Next
                            End If
                            If fourthsub.Count <> 0 Then
                                For Each item As String In fourthsub
                                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.dept = '" & item & "'", con)
                                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                    Do While student.Read
                                        ds.rows.Add(student(0))
                                    Loop
                                    student.Close()
                                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffdept inner join staffprofile on staffprofile.staffid = staffdept.staff inner join depts on depts.id = staffdept.dept where depts.dept = '" & item & "'", con)
                                    Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                    Do While student1.Read
                                        ds.rows.Add(student1(0))
                                    Loop
                                    student1.Close()
                                    Dim subsub As New ArrayList
                                    subsub = Get_subordinates(item)
                                    For Each subitem As String In subsub
                                        fifthsub.Add(subitem)
                                    Next
                                Next
                            End If
                        Else
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.headtitle = '" & RTrim(spec(0)) & "' and depts.dept = '" & LTrim(spec(1)) & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            Do While student.Read
                                ds.rows.Add(student(0))
                            Loop
                            student.Close()
                        End If
                    End If
                ElseIf cboReceivetype.Text = "Student" Then

                    Dim category As Array = Split(cboCategory.Text, "-")
                    If category.Length = 3 Then
                        Session("ReceiverReply") = category(0) & " - " & category(1) & " - STUDENT"
                        Session("Relationship") = category(1) & " TEACHER"
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from subjectreg inner join class on subjectreg.class = class.id inner join studentsprofile on subjectreg.student = studentsprofile.admno inner join subjects on subjects.id = subjectreg.subjectsofferred where subjectreg.session = '" & Session("SessionId") & "' and class.class = '" & RTrim(category(0)) & "' and subjects.subject = '" & LTrim(category(1)) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))
                        Loop
                        student.Close()
                    Else
                        Session("ReceiverReply") = category(0) & " - STUDENT"
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.headtitle, depts.dept from class inner join depts on class.superior = depts.id where class.class = '" & RTrim(category(0)) & "' and depts.head = '" & Session("StaffId") & "'", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        If Not Session("Relationship") = Nothing Then
                            If student11.Read Then
                                Session("Relationship") = student11.Item(0) & " - " & student11.Item(1)
                            Else
                                Session("Relationship") = " CLASS TEACHER"
                            End If
                        End If
                        student11.Close()
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from studentsummary inner join class on class.id = studentsummary.class inner join studentsprofile on studentsprofile.admno = studentsummary.student where class.class = '" & RTrim(category(0)) & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.rows.Add(student(0))

                        Loop
                        student.Close()
                        Dim clashead As New MySql.Data.MySqlClient.MySqlCommand("Select studentsprofile.surname, depts.headtitle, depts.dept from studentsummary inner join (class inner join depts on depts.Id = class.superior) on studentsummary.class = class.Id inner join studentsprofile on studentsprofile.admno = studentsummary.student where depts.dept = '" & RTrim(category(0)) & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                        Dim schhead As MySql.Data.MySqlClient.MySqlDataReader = clashead.ExecuteReader
                        schhead.Read()
                        Do While schhead.Read
                            ds.rows.Add(schhead(0))
                            Session("Relationship") = schhead.Item(1) & " - " & schhead.Item(2)

                        Loop
                        schhead.Close()

                    End If
                ElseIf cboReceivetype.Text = "Parent" Then

                    Dim category As Array = Split(cboCategory.Text, "-")
                    If category.Length = 3 Then
                        Session("ReceiverReply") = category(0) & " - " & category(1) & " - PARENT"
                        Session("Relationship") = category(0) & " - " & category(1) & " TEACHER"
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname from subjectreg inner join (studentsprofile inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentId) on parentward.ward = studentsprofile.admno) on studentsprofile.admno = subjectreg.student inner join  class on subjectreg.class = class.id inner join subjects on subjects.id = subjectreg.subjectsofferred where subjectreg.session = '" & Session("SessionId") & "' and class.class = '" & RTrim(category(0)) & "' and subjects.subject = '" & LTrim(category(1)) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Dim parents As New ArrayList
                        Do While student.Read
                            If Not parents.Contains(student.Item(0)) Then
                                parents.Add(student.Item(0))

                                ds.rows.Add(student(0))
                            End If
                        Loop
                        student.Close()
                    Else
                        Session("ReceiverReply") = category(0) & " - PARENT"
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.headtitle, depts.dept from class inner join depts on class.superior = depts.id where class.class = '" & RTrim(category(0)) & "' and depts.head = '" & Session("StaffId") & "'", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        If Session("Relationship") = Nothing Then

                            Session("Relationship") = RTrim(category(0)) & " CLASS TEACHER"
                        End If
                        student11.Close()
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname from studentsummary inner join (studentsprofile inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentId) on parentward.ward = studentsprofile.admno) on studentsprofile.admno = studentsummary.student inner join class on studentsummary.class = class.Id where class.class = '" & RTrim(category(0)) & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Dim parent2 As New ArrayList
                        Do While student.Read
                            If Not parent2.Contains(student.Item(0)) Then
                                parent2.Add(student.Item(0))
                                ds.rows.Add(student(0))
                            End If
                        Loop
                        student.Close()
                        Dim clashead As New MySql.Data.MySqlClient.MySqlCommand("Select parentprofile.parentname, depts.headtitle, depts.dept from studentsummary inner join (class inner join depts on depts.Id = class.superior) on studentsummary.class = class.Id inner join (studentsprofile inner join (parentward inner join parentprofile on parentprofile.parentId = parentward.parent) on studentsprofile.admno = parentward.ward) on studentsprofile.admno = studentsummary.student where depts.dept = '" & RTrim(category(0)) & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                        Dim schhead As MySql.Data.MySqlClient.MySqlDataReader = clashead.ExecuteReader
                        Dim parent3 As New ArrayList
                        Do While schhead.Read
                            If Not parent3.Contains(schhead.Item(0)) Then
                                parent3.Add(schhead.Item(0))
                                ds.rows.Add(schhead(0))
                            End If
                        Loop
                        schhead.Close()
                    End If
                End If
                cboReceivetype.Enabled = False

                Dim newadditions As New ArrayList

                For Each row As GridViewRow In gridRecipients.Rows
                    Dim exists As Boolean = False


                    For Each rows As DataRow In ds.Rows
                        If rows.Item(0).ToString = row.Cells(0).Text Then

                            exists = True

                        End If



                    Next
                    If exists = False Then

                        newadditions.Add(row.Cells(0).Text)
                    End If
                Next
                For Each item As String In newadditions
                    ds.Rows.Add(item)
                Next


                gridRecipients.DataSource = ds
                gridRecipients.DataBind()
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept from depts where superior = '" & dept & "'", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim subo As New ArrayList
            Do While student1.Read
                subo.Add(student1.Item(0))
            Loop
            student1.Close()
            Return subo
            con.Close()
        End Using
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

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = True
        Response.Redirect("~/content/app/staff/sentmsgs.aspx")
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
