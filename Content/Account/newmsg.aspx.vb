Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

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
        If Type = True Then
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
        scriptman.RegisterPostBackControl(cboReceivetype)
        scriptman.RegisterPostBackControl(cboCategory)
        scriptman.RegisterPostBackControl(btnSend)
        scriptman.RegisterPostBackControl(Button1)
        scriptman.RegisterPostBackControl(ChkPortal)
        scriptman.RegisterPostBackControl(chkSendSMS)
        scriptman.RegisterPostBackControl(del1)
        scriptman.RegisterPostBackControl(del2)
        scriptman.RegisterPostBackControl(del3)
        scriptman.RegisterPostBackControl(del4)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False
    End Sub
  
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Account(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

         
            Dim ds As New DataTable
            ds.Columns.Add("name")

            If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                Dim same As ArrayList = msgobj.Use_Same_Msg(Request.QueryString.ToString)
                Dim sender1 As String = same(0)
                If Session("responsetype") = "Reply" Then
                    cboReceivetype.Text = IIf(same(1).ToString.ToUpper = "STAFF", same(1).ToString.ToUpper, "PARENTS")
                    txtSubject.Text = "Re " & same(2)
                    cboCategory.Items.Add(same(5))
                    Session("ReceiverReply") = same(5)
                    Session("Relationship") = same(6)
                    If same(1).ToString = "Accounts" Then cboReceivetype.Text = "Staff"
                Else
                    txtSubject.Text = same(2)
                    FreeTextBox1.Text = same(3)

                End If

               Dim s As New ds_functions
                If Session("responsetype") = "Reply" Then
                    If cboReceivetype.Text = "Student" Then
                        Dim stu As String = s.Get_Stu_name(sender1)
                        ds.Rows.Add(stu)
                    ElseIf cboReceivetype.Text = "Staff" Or cboReceivetype.Text = "STAFF" Then
                        If cboCategory.Text = "Admin" Then
                            ds.Rows.Add("Admin")
                        ElseIf cboCategory.Text = "Accounts" Then
                            ds.Rows.Add("Accounts")
                        Else
                            Dim stu As String = s.Get_Staff_name(sender1)
                            ds.Rows.Add(stu)
                        End If
                    Else
                        Dim stu As String = s.Get_parent_name(sender1)
                        ds.Rows.Add(stu)
                    End If
                    cboReceivetype.Enabled = False
                    cboCategory.Enabled = False
                End If
                If Session("responsetype") = "Forward" Then
                    Dim pics As ArrayList = msgobj.Attach_Files(Request.QueryString.ToString)
                    Dim c As Integer = 1
                    For Each x In pics
                        Dim imagesrc As New fileimage
                        Panel1.Visible = True
                        If c = 1 Then
                            icon1.Src = imagesrc.get_image(x(1).ToString)
                            del1.Text = "Delete"
                            LinkButton1.Text = Path.GetFileName(x(0))
                            Session("pic1") = x(0)
                        ElseIf c = 2 Then
                            icon2.Src = imagesrc.get_image(x(1).ToString)
                            del2.Text = "Delete"
                            LinkButton2.Text = Path.GetFileName(x(0))
                            Session("pic2") = x(0)
                        ElseIf c = 3 Then
                            icon3.Src = imagesrc.get_image(x(1).ToString)
                            del3.Text = "Delete"
                            LinkButton3.Text = Path.GetFileName(x(0))
                            Session("pic3") = x(0)
                        ElseIf c = 4 Then
                            icon4.Src = imagesrc.get_image(x(1).ToString)
                            del4.Text = "Delete"
                            LinkButton4.Text = Path.GetFileName(x(0))
                            Session("pic4") = x(0)
                        End If
                        c = c + 1
                    Next
                End If
            End If
                Session("responsetype") = Nothing
                For Each row As GridViewRow In gridRecipients.Rows
                    ds.Rows.Add(row.Cells(0).Text)
                Next
                gridRecipients.DataSource = ds
                gridRecipients.DataBind()
               

                receiver = cboReceivetype.Text

                   Dim l As New Literal

            l = Me.Master.FindControl("summerLit")
            l.Text = msgobj.Get_Js_Admin(Hidden1, counter, txtSMS, ChkPortal)
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "this4", l.Text, False)

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            If ChkPortal.Checked = False And chkSendSMS.Checked = False Then
                Show_Alert(False, "Select an option for either Portal message, SMS or both")
                Exit Sub
            End If
            If gridRecipients.Rows.Count = 0 Then
                Show_Alert(False, "Please add recipients.")
                Exit Sub
            End If
            Dim message As String = Hidden1.Value
            Dim j As String = cboCategory.Text
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
            Dim y As Integer
            Dim s As New ds_functions
            If ChkPortal.Checked = True Then
                For Each item As String In recipient

                    Dim z As String
                    If receiver = "PARENTS" Then
                        z = s.Get_parent_ID(item)
                    ElseIf receiver = "STUDENTS" Then
                        z = s.Get_Stu_ID(item)
                    ElseIf receiver = "STAFF" Then
                        If cboCategory.Text = "ADMIN" or cboCategory.Text = "Admin" Then
                            z = "Admin"
                        ElseIf cboCategory.Text = "ACCOUNTS" or cboCategory.Text = "Accounts" Then
                            z = "Accounts"
                        Else
                            z = s.Get_Staff_ID(item)
                        End If
                    End If
                    
                    If y = 0 Then
                        y = msgobj.Send_Msg(z, Session("staffid"), txtSubject.Text, message, Session("sessionid"), "Staff", j)

                    Else
                        msgobj.Send_Msg_Cont(z, Session("staffid"), txtSubject.Text, message, Session("sessionid"), "Staff", j, y)

                    End If
                   If z = "Admin" Then
                        Dim d As ArrayList = s.Fetch_admins
                        For Each q In d
                             msgobj.Send_Email(msgobj.Get_Email(q), txtSubject.Text, message, "http://" & Request.Url.Authority, "Accounts", s.Get_Staff_Name(Session("staffid")))

                        Next
                    ElseIf z = "Accounts" Then
                        Dim d As ArrayList = s.Fetch_accounts
                         For Each q In d
                             msgobj.Send_Email(msgobj.Get_Email(q), txtSubject.Text, message, "http://" & Request.Url.Authority, "Accounts", s.Get_Staff_Name(Session("staffid")))

                        Next
                            else

                             msgobj.Send_Email(msgobj.Get_Email(z), txtSubject.Text, message, "http://" & Request.Url.Authority, "Accounts", s.Get_Staff_Name(Session("staffid")))

                        end if
        If z = "Admin" Then
            Dim d As ArrayList = s.Fetch_admins
            For Each q In d
                logify.Notifications("You have a new message - " & txtSubject.Text, q, Session("staffid"), "Accounts", "~/content/admin/readmsg.aspx?" & y, "Message")
            Next
        ElseIf z = "Accounts" Then
            Dim d As ArrayList = s.Fetch_admins
            For Each q In d
                logify.Notifications("You have a new message - " & txtSubject.Text, q, Session("staffid"), "Accounts", "~/content/account/readmsg.aspx?" & y, "Message")
            Next
        ElseIf receiver = "Staff" Then
            logify.Notifications("You have a new message - " & txtSubject.Text, z, Session("staffid"), "Accounts", "~/content/staff/readmsg.aspx?" & y, "Message")
        ElseIf receiver = "Parent" Then
            logify.Notifications("You have a new message - " & txtSubject.Text, z, Session("staffid"), "Accounts", "~/content/parent/readmsg.aspx?" & y, "Message")
        End If
        recno = recno + 1
                Next
            End If
            Dim k As Integer
            For Each row As GridViewRow In gridRecipients.Rows
                If Not recipient.Contains(row.Cells(0).Text) Then
                    recipient.Add(row.Cells(0).Text)
                    k = k + 1
                End If
            Next
            Dim pages As Integer = Math.Ceiling(txtSMS.Text.Count / 160)


            Dim sentany As Boolean
            Dim inisms As Integer
            Dim smsno As Integer
            If chkSendSMS.Checked = True Then
                If no * pages > msgobj.Check_SMS_Units() Then
                    Show_Alert(False, "You don't have enough credits for the SMS operation. Contact developer")
                    Exit Sub
                End If
                Dim sent As Boolean
                For Each item As String In recipient
                    Dim phone As String = msgobj.Get_phone(item)
                    Dim n As Array = Split(phone, ",")
                    Dim o As New ArrayList
                    Try
                        o.Add(n(0))
                        o.Add(n(1))
                    Catch
                    End Try
                    For Each j In o
                        sent = msgobj.Send_SMS(txtSMS.Text, Trim(j), Session("sessionid"))
                        If sent = True Then
                            sentany = True
                            smsno += 1
                        End If
                    Next
                Next
                If sent = True Then msgobj.Record_SMS(txtSubject.Text, txtSMS.Text, sent * pages, pages, sent, receiver, Session("staffid"))
            End If

           
            If ChkPortal.Checked = True Then
               If LinkButton1.Text <> "" Then
                    msgobj.Att_Files(y, Session("pic1"), att1.Text)
                End If
                If LinkButton2.Text <> "" Then
                    msgobj.Att_Files(y, Session("pic2"), att2.Text)
                End If
                If LinkButton3.Text <> "" Then
                    msgobj.Att_Files(y, Session("pic3"), att3.Text)
                End If
                If LinkButton4.Text <> "" Then
                    msgobj.Att_Files(y, Session("pic4"), att4.Text)
                End If
                msgobj.Send_Msg_Sent(y, recno & " Recipients", "Accounts", Session("staffid"), txtSubject.Text, message, Session("sessionid"), j, Session("ReceiverReply"))

            End If

            Show_Alert(True, "Message sent. " & IIf(chkSendSMS.Checked = True, IIf(sentany = False, 0, smsno) & " SMS sent. Length = " & pages & " pages", ""))
            logify.log(Session("staffid"), "A message was sent to " & cboCategory.Text & " " & IIf(chkSendSMS.Checked = True, "with SMS.", "without SMS") & ". Message ref = " & y)
           
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = True
        Response.Redirect("~/Content/admin/messages.aspx")
    End Sub


    Protected Sub cboReceivetype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboReceivetype.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                If cboReceivetype.Text = "STAFF" Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle from depts", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboCategory.Items.Clear()

                    cboCategory.Items.Add("ALL STAFF")
                    cboCategory.Items.Add("ACCOUNTS")
                    cboCategory.Items.Add("ADMIN")
                    Do While student0.Read
                        cboCategory.Items.Add(student0.Item(0) & " - STAFF")
                        cboCategory.Items.Add(student0.Item(0) & " - " & student0.Item(1))
                    Loop
                    cboCategory.Items.Add("CLASS TEACHERS")
                    student0.Close()

                ElseIf cboReceivetype.Text = "STUDENTS" Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboCategory.Items.Clear()
                    cboCategory.Items.Add("ALL STUDENTS")
                    Do While student0.Read
                        cboCategory.Items.Add(student0.Item(0) & " - STUDENTS")
                    Loop
                    student0.Close()
                ElseIf cboReceivetype.Text = "PARENTS" Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboCategory.Items.Clear()
                    cboCategory.Items.Add("ALL PARENTS")
                    Do While student0.Read
                        cboCategory.Items.Add(student0.Item(0) & " - PARENTS")
                    Loop
                    student0.Close()
                    cboCategory.Items.Add("DEBTORS")

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
                con.Close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim ds As New DataTable
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()

                ds.Columns.Add("name")
                Dim spec As Array = Split(cboCategory.Text, "-")
                If cboReceivetype.Text = "STAFF" Then
                    If cboCategory.Text = "ALL STAFF" Then
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.Rows.Add(student(0))
                        Loop
                        Session("ReceiverReply") = "STAFF"

                    ElseIf cboCategory.Text = "CLASS TEACHERS" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join classteacher on classteacher.teacher = staffprofile.staffid", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        Do While student11.Read
                            ds.Rows.Add(student11(0))
                        Loop
                        Session("ReceiverReply") = "CLASS TEACHER"

                        student11.Close()
                    ElseIf cboCategory.Text = "ACCOUNTS" Then
                        ds.Rows.Add("Accounts")
                    ElseIf cboCategory.Text = "ADMIN" Then
                        ds.Rows.Add("Admin")
                    ElseIf Trim(spec(1)) = "CLASS TEACHERS" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join (class inner join depts on depts.Id = class.superior) on classteacher.class = class.Id inner join staffprofile on staffprofile.staffid = classteacher.teacher where depts.dept = '" & RTrim(spec(0)) & "'", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        Do While student11.Read
                            ds.Rows.Add(student11(0))
                        Loop
                        student11.Close()
                    ElseIf Trim(spec(1)) = "STAFF" And cboCategory.Text <> "ALL STAFF" Then
                        Session("ReceiverReply") = RTrim(spec(0)) & " STAFF"

                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join (staffdept inner join depts on depts.id = staffdept.dept) on staffprofile.staffid = staffdept.staff where depts.dept = '" & RTrim(spec(0)) & "'", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        Do While student11.Read
                            ds.Rows.Add(student11(0))
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
                                ds.Rows.Add(student1(0))
                            Loop
                            student1.Close()
                            secsub = Get_subordinates(item)
                        Next
                        If secsub.Count <> 0 Then
                            For Each item As String In secsub
                                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join (staffdept inner join depts on depts.id = staffdept.dept) on staffprofile.staffid = staffdept.staff where depts.dept = '" & item & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                Do While student1.Read
                                    ds.Rows.Add(student1(0))
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
                                    ds.Rows.Add(student1(0))
                                Loop
                                student1.Close()
                                fourthsub = Get_subordinates(item)
                            Next
                        End If
                    Else
                        Session("ReceiverReply") = cboCategory.Text

                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile inner join depts on depts.head = staffprofile.staffid where depts.dept = '" & RTrim(spec(0)) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.Rows.Add(student(0))
                        Loop
                        student.Close()

                    End If
                ElseIf cboReceivetype.Text = "STUDENTS" Then
                    If Not cboCategory.Text = "ALL STUDENTS" Then
                        Session("ReceiverReply") = RTrim(spec(0)) & " STUDENT"

                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from class where class = '" & RTrim(spec(0)) & "'", con)
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        Dim thisclass As Integer
                        If student0.Read() Then
                            thisclass = student0.Item(0)
                        End If
                        student0.Close()
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from studentsummary inner join class on studentsummary.class = class.id inner join studentsprofile on studentsummary.student = studentsprofile.admno where studentsummary.session = '" & Session("SessionId") & "' and studentsummary.class = '" & thisclass & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.Rows.Add(student(0))
                        Loop
                        student.Close()
                        Dim clashead As New MySql.Data.MySqlClient.MySqlCommand("Select studentsprofile.surname, depts.headtitle, depts.dept from studentsummary inner join (class inner join depts on depts.Id = class.superior) on studentsummary.class = class.Id inner join studentsprofile on studentsprofile.admno = studentsummary.student where depts.dept = '" & RTrim(spec(0)) & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                        Dim schhead As MySql.Data.MySqlClient.MySqlDataReader = clashead.ExecuteReader
                        schhead.Read()
                        Do While schhead.Read
                            ds.Rows.Add(schhead(0))

                        Loop
                        schhead.Close()
                    Else
                        Session("ReceiverReply") = "STUDENT"

                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from studentsprofile", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.Rows.Add(student(0))
                        Loop
                        student.Close()

                    End If
                ElseIf cboReceivetype.Text = "PARENTS" Then
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
                                        ds.Rows.Add(balreader(5))
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
                                    ds.Rows.Add(student(0))
                                End If
                            Loop
                            student.Close()
                            Dim clashead As New MySql.Data.MySqlClient.MySqlCommand("Select parentprofile.parentname, depts.headtitle, depts.dept from studentsummary inner join (class inner join depts on depts.Id = class.superior) on studentsummary.class = class.Id inner join (studentsprofile inner join (parentward inner join parentprofile on parentprofile.parentId = parentward.parent) on studentsprofile.admno = parentward.ward) on studentsprofile.admno = studentsummary.student where depts.dept = '" & RTrim(spec(0)) & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                            Dim schhead As MySql.Data.MySqlClient.MySqlDataReader = clashead.ExecuteReader
                            Dim parents2 As New ArrayList
                            Do While schhead.Read
                                If Not parents2.Contains(schhead.Item(0)) Then
                                    parents2.Add(schhead.Item(0))
                                    ds.Rows.Add(schhead(0))
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
                            ds.Rows.Add(student(0))
                        Loop
                        student.Close()
                    End If
                End If
                cboReceivetype.Enabled = False

                con.Close()
            End Using

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
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Dim subo As New ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()

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
            Dim upload As New ds_functions
            upload.Upload(FileUpload1, Server.MapPath("~/Content/Uploads/"))
            Dim q As String
            If FileUpload1.HasFile Then
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

    ' Validated counterpart to btnUpload_Click above: routes through ds_functions.UploadSafe
    ' (App_Code/ds_functions.vb) instead of Upload, so the posted attachment's extension is
    ' checked against a closed allow-list before it is ever saved.
    Protected Sub btnUploadValidated_Click(sender As Object, e As EventArgs) Handles btnUploadValidated.Click
        Try
            Dim upload As New ds_functions
            upload.UploadSafe(FileUploadValidated, Server.MapPath("~/Content/Uploads/"))
            Dim q As String
            If FileUploadValidated.HasFile Then
                q = "https://" & Request.Url.Authority & "/Content/Uploads/" & FileUploadValidated.FileName
                Dim picsource As New fileimage
                If LinkButton1.Text = "" Then
                    att1.Text = FileUploadValidated.PostedFile.ContentType
                    icon1.Src = picsource.get_image(FileUploadValidated.PostedFile.ContentType)
                    LinkButton1.Text = Path.GetFileName(q)
                    Session("pic1") = q
                    Panel1.Visible = True
                    del1.Text = "Delete"
                ElseIf LinkButton2.Text = "" Then
                    att2.Text = FileUploadValidated.PostedFile.ContentType
                    icon2.Src = picsource.get_image(FileUploadValidated.PostedFile.ContentType)
                    LinkButton2.Text = Path.GetFileName(q)
                    Session("pic2") = q
                    del2.Text = "Delete"
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

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = True
        Response.Redirect("~/Content/admin/sentmsgs.aspx")
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
            gridRecipients.DataSource = DS
            gridRecipients.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub chkSendSMS_CheckedChanged(sender As Object, e As EventArgs) Handles chkSendSMS.CheckedChanged
        If chkSendSMS.Checked = True Then
            pnlsms.Visible = True
        Else
            pnlsms.Visible = False
        End If
    End Sub

    Protected Sub ChkPortal_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPortal.CheckedChanged
        If ChkPortal.Checked = True Then
            PnlMsg.Visible = True
        Else
            PnlMsg.Visible = False
        End If
    End Sub


End Class
