Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_newterm
    Inherits System.Web.UI.Page

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

    Sub readd_subjects()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdclass As New MySql.Data.MySqlClient.MySqlCommand("Select * from class", con)
            Dim classes As New ArrayList
            Dim classread As MySql.Data.MySqlClient.MySqlDataReader = cmdclass.ExecuteReader
            Do While classread.Read
                classes.Add(classread.Item(0))
            Loop
            classread.Close()
            For Each cla As Integer In classes
                Dim students As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Studentsummary where class = ? and Session = ?", con)
                students.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                students.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term3", Session("sessionid")))
                Dim classstudents As New ArrayList
                Dim clasturead As MySql.Data.MySqlClient.MySqlDataReader = students.ExecuteReader
                Do While clasturead.Read
                    classstudents.Add(clasturead.Item(3))
                Loop
                clasturead.Close()
                For Each student As String In classstudents
                    Dim lastses As New MySql.Data.MySqlClient.MySqlCommand("delete FROM subjectreg where student = '" & student & "' and session = '" & Session("sessionid") & "'", con)
                    lastses.ExecuteNonQuery()
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM ClassSubjects WHERE Type= ? And class = ? and subjectnest = '" & 0 & "' order by subject", con)
                    Dim type As String = "Compulsory"
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Type", type))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                    Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    Dim subjectsID As New ArrayList
                    Dim i As Integer = 0
                    Do While reader1.Read
                        subjectsID.Add(Val(reader1.Item("subject")))

                    Loop
                    reader1.Close()
                    For Each item As String In subjectsID
                        Dim param As New MySql.Data.MySqlClient.MySqlParameter

                        param = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subjects", item))


                        cmd.ExecuteNonQuery()
                        cmd.Parameters.Remove(param)

                    Next
                    Dim cmd2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectnest FROM ClassSubjects WHERE class = '" & cla & "' and subjectnest <> '" & "" & "'", con)
                    Dim reader1a As MySql.Data.MySqlClient.MySqlDataReader = cmd2s.ExecuteReader
                    Dim subjectsIDnest As New ArrayList
                    Dim inest As Integer = 0
                    Do While reader1a.Read
                        If Not subjectsIDnest.Contains(Val(reader1a.Item(0))) Then
                            subjectsIDnest.Add(Val(reader1a.Item(0)))
                        End If
                    Loop
                    reader1a.Close()
                    For Each item As String In subjectsIDnest
                        Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", item))
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                        cmdds.ExecuteNonQuery()
                    Next

                Next


            Next
            con.close()        End Using
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try
            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()

                    Dim a As New DataTable
                    a.Columns.Add("ID")
                    a.Columns.Add("Session")
                    a.Columns.Add("Term")
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, session, term from session order by ID desc", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Do While msg.Read()
                        a.Rows.Add(msg.Item(0), msg.Item(1), msg.Item(2))
                    Loop
                    Gridview1.DataSource = a

                    Gridview1.DataBind()
                    msg.Close()
                    con.Close()                End Using
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub linkbterm_Click(sender As Object, e As EventArgs) Handles linkbterm.Click
        Panel1.Visible = True
        Session("Updateterm") = Nothing
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM sessioncreate", con)
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Do While reader20.Read
                    DropDownList1.Items.Add(reader20.Item(1))
                Loop
                DropDownList1.Text = reader20.Item(1)
                reader20.Close()
                DropDownList2.Text = "Select"
                txtClose.Text = ""
                txtOpen.Text = ""
                txtNext.Text = ""
                Button1.Text = "Add"
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If txtClose.Text = "" Or txtOpen.Text = "" Or txtNext.Text = "" Then
                Show_Alert(False, "Please enter valid dates")
                Exit Sub
            End If
            Dim closing As String
            Dim resumption As String
            Dim opening As String
            Try


                closing = txtClose.Text
                resumption = txtNext.Text
                opening = txtOpen.Text
            Catch ex As Exception
                Show_Alert(False, "Please enter valid dates")
                Exit Sub
            End Try
            If DropDownList2.Text = "Select" Then
                Show_Alert(False, "Please select a term")
                Exit Sub
            End If

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Session("Updateterm") <> Nothing Then
                    Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update Session set session = ?, term = ?, closingdate = ?, nextterm = ?, opendate = ? where ID = ?", con)
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session2", DropDownList1.Text))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term2", DropDownList2.Text))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", closing))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("nextterm", resumption))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("open", opening))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("Updateterm")))
                    cmdInsert2.ExecuteNonQuery()
                    Session("Updateterm") = Nothing
                    logify.log(Session("staffid"), DropDownList1.Text & " - " & DropDownList2.Text & " was updated.")
                    Show_Alert(True, "Term Updated Successfully")
                    Panel1.Visible = False
                Else
                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where session = ? and term = ?", con)
                    cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", DropDownList1.Text))
                    cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", DropDownList2.Text))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    If reader20.Read Then
                        reader20.Close()
                        Show_Alert(False, "Term already exists")
                        Exit Sub
                    Else
                        reader20.Close()
                        Dim lastses As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session order by ID Desc", con)
                        Dim sesread As MySql.Data.MySqlClient.MySqlDataReader = lastses.ExecuteReader
                        Dim prsmsno As Integer = 0
                        Do While sesread.Read()
                            prsmsno = prsmsno + Val(sesread("smsno"))
                        Loop
                        sesread.Close()
                        Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into Session (session, term, closingdate, nextterm, opendate, smsno) Values (?,?,?,?,?,?)", con)
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session2", DropDownList1.Text))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term2", DropDownList2.Text))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", closing))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("nextterm", resumption))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("open", opening))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("on", prsmsno))

                        cmdInsert2.ExecuteNonQuery()
                      
                        Dim cmdInsertz As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                        Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertz.ExecuteReader

                        reader2.Read()
                        Session("SessionID") = reader2(0).ToString
                        Session("SessionName") = reader2(1).ToString
                        Session("Term") = reader2(2).ToString
                        Dim thissession As Integer = reader2(0).ToString
                        reader2.Close()

                        Dim cmdInsert2dx As New MySql.Data.MySqlClient.MySqlCommand("Update Session set smsno = '0' where ID <> '" & thissession & "'", con)
                        cmdInsert2dx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("on", prsmsno))
                        cmdInsert2dx.ExecuteNonQuery()
                        Dim cmdInsert20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into scorespublish (term) Values (?)", con)
                        cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session2", thissession))
                        cmdInsert20.ExecuteNonQuery()
                        If Not DropDownList2.Text = "1st term" Then
                            Dim cmdclass As New MySql.Data.MySqlClient.MySqlCommand("Select * from class", con)
                            Dim classes As New ArrayList
                            Dim classread As MySql.Data.MySqlClient.MySqlDataReader = cmdclass.ExecuteReader
                            Do While classread.Read
                                classes.Add(classread.Item(0))
                            Loop
                            classread.Close()
                            Dim newterm As Integer
                            Dim iflast As Boolean
                            For Each item As Integer In classes
                                Dim lastsession As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session order by ID Desc", con)
                                Dim sessionread As MySql.Data.MySqlClient.MySqlDataReader = lastsession.ExecuteReader

                                Do Until iflast = True
                                    sessionread.Read()
                                    If sessionread.Item(1) = DropDownList1.Text And sessionread.Item(2) = DropDownList2.Text Then
                                        newterm = sessionread(0)
                                        iflast = True
                                    End If
                                Loop
                                sessionread.Read()
                                Dim last As Integer = sessionread.Item(0)
                                sessionread.Close()
                                Dim students As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Studentsummary where class = ? and Session = ?", con)
                                students.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", item))
                                students.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term3", last))
                                Dim classstudents As New ArrayList
                                Dim clasturead As MySql.Data.MySqlClient.MySqlDataReader = students.ExecuteReader
                                Do While clasturead.Read
                                    classstudents.Add(clasturead.Item(3))
                                Loop
                                clasturead.Close()
                                For Each student As String In classstudents

                                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
                                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", student))
                                    Dim studentread As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                    studentread.Read()
                                    Dim dob As TimeSpan = Now.Subtract(DateTime.ParseExact(studentread.Item("dateofbirth"), "dd/MM/yyyy", Nothing))
                                    Dim age As Integer = dob.TotalDays \ 365
                                    studentread.Close()

                                    Dim studentsinsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into Studentsummary (session, class, student, age) values (?,?,?,?)", con)
                                    studentsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session4", newterm))
                                    studentsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class2", item))
                                    studentsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                                    studentsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("age", age))
                                    studentsinsert.ExecuteNonQuery()

                                    fees(item, student, newterm)




                                    Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
                                    cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", newterm))
                                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2a.ExecuteReader
                                    reader2a.Read()
                                    Dim smsno As Integer = reader2a(0)
                                    reader2a.Close()
                                    Dim cmdInsert2a As New MySql.Data.MySqlClient.MySqlCommand("Update session set smsno = ? where id = ?", con)
                                    cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", smsno + 35))
                                    cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", newterm))
                                    cmdInsert2a.ExecuteNonQuery()


                                    Dim stusub As New MySql.Data.MySqlClient.MySqlCommand("Select * from subjectreg where session = ? and student = ? and class = ?", con)
                                    stusub.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session5", last))
                                    stusub.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student1", student))
                                    stusub.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class3", item))
                                    Dim subjects As MySql.Data.MySqlClient.MySqlDataReader = stusub.ExecuteReader
                                    Dim studentsub As New ArrayList
                                    Dim lastscore As New ArrayList
                                    Dim lastscore2 As New ArrayList
                                    Dim nested As New ArrayList
                                    Do While subjects.Read
                                        studentsub.Add(subjects.Item(4))
                                        lastscore.Add(subjects("total"))
                                        lastscore2.Add(subjects("lastscore").ToString)
                                        nested.Add(subjects("nested"))
                                    Loop
                                    subjects.Close()

                                    Dim ins As Integer = 0
                                    For Each subject As Integer In studentsub
                                        Dim subjectsinsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into subjectreg (session, class, student, subjectsofferred, lastscore, lastscore2, nested) values (?,?,?,?,?,?,?)", con)
                                        subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session6", newterm))
                                        subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class3", item))
                                        subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student2", student))
                                        subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", subject))
                                        subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjejsjct", lastscore(ins)))
                                        subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjdkjdejsjct", lastscore2(ins)))
                                        subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjdkjdejbdsjct", nested(ins)))
                                        subjectsinsert.ExecuteNonQuery()
                                        ins = ins + 1
                                    Next
                                Next
                                iflast = False
                            Next
                        End If
                        Panel1.Visible = False
                        logify.log(Session("staffid"), DropDownList1.Text & " - " & DropDownList2.Text & " was created.")


                        Show_Alert(True, "Term Added Successfully")
                    End If
                End If
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("Session")
                a.Columns.Add("Term")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, session, term from session order by ID desc", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1), msg.Item(2))
                Loop
                Gridview1.DataSource = a

                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub fees(cla As Integer, student As String, term As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim total As Double

            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classfees where class = ?", con)
            cmdLoad0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim classfee As New ArrayList

            Dim classamount As New ArrayList
            Dim min As New ArrayList
            Dim classi As Integer
            Do While student0.Read
                Dim z As Integer
                classfee.Add(student0.Item(2))
                classamount.Add(student0.Item(3))
                min.Add(student0.Item("amount") * (student0.Item("min") / 100))
            Loop
            student0.Close()
            For Each item As String In classfee
                Dim test2 As Boolean
                Dim f As New Random
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader

                Dim refs2 As New ArrayList
                Do While readref2.Read
                    refs2.Add(readref2.Item(0))
                Loop
                Dim d As Integer
                Do Until test2 = True
                    d = f.Next(100000, 999999)
                    If refs2.Contains(d) Then
                        test2 = False
                    Else
                        test2 = True
                    End If
                Loop
                readref2.Close()
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", classamount(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                cmdInsert22.ExecuteNonQuery()
                total = total + classamount(classi)


                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(classamount(classi), , , TriState.True)))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                cmdCheck2.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(classamount(classi), , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                cmdCheck4.ExecuteNonQuery()

                classi = classi + 1
            Next





            Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from optionalfees", con)
            Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader
            Dim genfee As New ArrayList
            Dim genamount As New ArrayList
            Dim geni As Integer
            Dim genmin As New ArrayList
            Do While student2.Read
                genfee.Add(student2.Item(1))
                genamount.Add(student2.Item(2))
                genmin.Add(student2.Item("amount") * (student2.Item("min") / 100))
            Loop
            student2.Close()
            For Each item As String In genfee
                Dim test21 As Boolean
                Dim f1 As New Random
                Dim ref21 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref21 As MySql.Data.MySqlClient.MySqlDataReader = ref21.ExecuteReader

                Dim refs21 As New ArrayList
                Do While readref21.Read
                    refs21.Add(readref21.Item(0))
                Loop
                Dim d1 As Integer
                Do Until test21 = True
                    d1 = f1.Next(100000, 999999)
                    If refs21.Contains(d1) Then
                        test21 = False
                    Else
                        test21 = True
                    End If
                Loop
                readref21.Close()
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", genamount(geni)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", genmin(geni)))
                cmdInsert22.ExecuteNonQuery()
                total = total + genamount(geni)

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(genamount(geni), , , TriState.True)))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                cmdCheck2.ExecuteNonQuery()


                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(genamount(geni), , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))
                cmdCheck4.ExecuteNonQuery()

                geni = geni + 1
            Next





            Dim cmdLoad3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
            Dim student3 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad3.ExecuteReader
            Dim admfee As New ArrayList
            Dim admamount As New ArrayList
            Dim admin As New ArrayList
            Dim admi As Integer
            Do While student3.Read

                admfee.Add(student3.Item(1))
                admamount.Add(student3.Item(2))
                admin.Add(student3.Item("amount") * (student3.Item("min") / 100))
            Loop
            student3.Close()
            Dim hostel As String
            Dim transport As String
            Dim feeding As String
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT admfees, hostelstay, transport from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", student))

            Dim studentadm As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            studentadm.Read()
            hostel = studentadm.Item(1)
            transport = studentadm.Item(2)
            Dim paid As String = studentadm.Item(0)
            studentadm.Close()
            If paid = "Not Paid" Then
                For Each item As String In admfee
                    Dim test22 As Boolean
                    Dim f2 As New Random
                    Dim ref22 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref22 As MySql.Data.MySqlClient.MySqlDataReader = ref22.ExecuteReader

                    Dim refs22 As New ArrayList
                    Do While readref22.Read
                        refs22.Add(readref22.Item(0))
                    Loop
                    Dim d2 As Integer
                    Do Until test22 = True
                        d2 = f2.Next(100000, 999999)
                        If refs22.Contains(d2) Then
                            test22 = False
                        Else
                            test22 = True
                        End If
                    Loop
                    readref22.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", admamount(admi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", admin(admi)))
                    cmdInsert22.ExecuteNonQuery()
                    total = total + admamount(admi)

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(admamount(admi), , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))
                    cmdCheck2.ExecuteNonQuery()

                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(admamount(admi), , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))
                    cmdCheck4.ExecuteNonQuery()
                    admi = admi + 1



                Next

            End If
            If Session("term") = "1st term" Then




                Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from sessionalfees", con)
                Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                Dim sesfee As New ArrayList
                Dim sesamount As New ArrayList
                Dim sesi As Integer
                Dim sesmin As New ArrayList
                Do While student24.Read

                    sesfee.Add(student24.Item(1))
                    sesamount.Add(student24.Item(2))
                    sesmin.Add(student24.Item("amount") * (student24.Item("min") / 100))
                Loop
                student24.Close()

                Dim cmdload3xcz1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classsessionfees where class = '" & cla & "'", con)
                Dim student3xcz1 As MySql.Data.MySqlClient.MySqlDataReader = cmdload3xcz1.ExecuteReader

                Do While student3xcz1.Read
                    sesfee.Add(student3xcz1.Item("fee"))
                    sesamount.Add(student3xcz1.Item("amount"))
                    sesmin.Add(student3xcz1.Item("amount") * (student3xcz1.Item("min") / 100))
                Loop
                student3xcz1.Close()

                For Each item As String In sesfee
                    Dim test23 As Boolean
                    Dim f3 As New Random
                    Dim ref23 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref23 As MySql.Data.MySqlClient.MySqlDataReader = ref23.ExecuteReader

                    Dim refs23 As New ArrayList
                    Do While readref23.Read
                        refs23.Add(readref23.Item(0))
                    Loop
                    Dim d3 As Integer
                    Do Until test23 = True
                        d3 = f3.Next(100000, 999999)
                        If refs23.Contains(d3) Then
                            test23 = False
                        Else
                            test23 = True
                        End If
                    Loop
                    readref23.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", sesfee(sesi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount(sesi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin(sesi)))
                    cmdInsert22.ExecuteNonQuery()
                    total = total + sesamount(sesi)


                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount(sesi), , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", sesfee(sesi) & " DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                    cmdCheck2.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount(sesi), , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                    cmdCheck4.ExecuteNonQuery()
                    sesi = sesi + 1
                Next
            End If
            If hostel = True Then

                Dim test24 As Boolean
                Dim f4 As New Random
                Dim ref24 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref24 As MySql.Data.MySqlClient.MySqlDataReader = ref24.ExecuteReader

                Dim refs24 As New ArrayList
                Do While readref24.Read
                    refs24.Add(readref24.Item(0))
                Loop
                Dim d4 As Integer
                Do Until test24 = True
                    d4 = f4.Next(100000, 999999)
                    If refs24.Contains(d4) Then
                        test24 = False
                    Else
                        test24 = True
                    End If
                Loop
                readref24.Close()
                Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                student24.Read()

                Dim sesamount As String = student24.Item(0)
                Dim sesmin As Integer = (student24.Item("cost") * (student24.Item("min") / 100))
                student24.Close()
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                cmdInsert22.ExecuteNonQuery()
                total = total + sesamount

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BOARDING DEBTS"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                cmdCheck2.ExecuteNonQuery()
                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                cmdCheck4.ExecuteNonQuery()

            End If
            If transport <> "" Then
                Dim test25 As Boolean
                Dim f5 As New Random
                Dim ref25 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref25 As MySql.Data.MySqlClient.MySqlDataReader = ref25.ExecuteReader

                Dim refs25 As New ArrayList
                Do While readref25.Read
                    refs25.Add(readref25.Item(0))
                Loop
                Dim d5 As Integer
                Do Until test25 = True
                    d5 = f5.Next(100000, 999999)
                    If refs25.Contains(d5) Then
                        test25 = False
                    Else
                        test25 = True
                    End If
                Loop
                readref25.Close()
                Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees where route = ?", con)
                cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", transport))
                Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                student24.Read()
                Dim sesfee As String = student24.Item(0)
                Dim sesamount As String = student24.Item(1)
                Dim sesmin As Integer = (student24.Item("amount") * (student24.Item("min") / 100))
                student24.Close()
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                cmdInsert22.ExecuteNonQuery()
                total = total + sesamount

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "TRANSPORT DEBTS"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                cmdCheck2.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))

                cmdCheck4.ExecuteNonQuery()
            End If







            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT fee, type, amount from discount where student = '" & student & "'", con)
            Dim reader22 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            Dim discountitems As New ArrayList
            Dim discountvalues As New ArrayList
            Dim discounttypes As New ArrayList
            Do While reader22.Read()
                discountitems.Add(reader22.Item(0))
                discounttypes.Add(reader22.Item(1))
                discountvalues.Add(reader22.Item(2))
            Loop
            reader22.Close()
            Dim count As Integer
            For Each item As String In discountitems
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ? and fee = ?", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", item))
                Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Dim presentdiscount As Boolean = False
                Dim fee As Integer
                Dim mini As Integer
                If feereader3.Read() Then
                    presentdiscount = True
                    fee = feereader3.Item(2)
                    mini = feereader3.Item("min")
                End If
                feereader3.Close()
                If presentdiscount = True Then
                    If discounttypes(count) = "Fixed" Then
                        fee = fee - discountvalues(count)
                        mini = mini - discountvalues(count)
                    Else
                        fee = fee - (fee * (discountvalues(count) / 100))
                        mini = mini - (mini * (discountvalues(count) / 100))
                    End If
                    Dim cmdInsert225 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = '" & fee & "', min = '" & mini & "' where session = ? and fee = ? and student = ?", con)
                    cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                    cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdInsert225.ExecuteNonQuery()

                    Dim cmdCheck4a As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                    cmdCheck4a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & "%"))
                    cmdCheck4a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdCheck4a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", term))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4a.ExecuteReader
                    readref220.Read()
                    Dim refe As Integer = readref220.Item(0).ToString
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set cr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & item & " DEBTS" & "'", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()

                    Dim cmdCheck041 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set dr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & "DEBTORS" & "'", con)
                    cmdCheck041.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck041.ExecuteNonQuery()
                End If
                count = count + 1
            Next
            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? and student = ? order by date desc", con)
            cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "ADVANCE FEE PAYMENT"))
            cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            balance = cr - dr
            balreader.Close()
            cr = Nothing
            dr = Nothing
            Dim init As Double = balance
            If balance > 0 Then
                Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                Dim total2 As Integer = 0
                Dim feetype As New ArrayList
                Dim feeamount As New ArrayList
                Dim paid2 As Double = 0
                Dim min2 As Double = 0
                Dim k As Integer = 0
                Do While feereader2.Read
                    If feereader2.Item("amount") = feereader2.Item("min") And balance > (feereader2.Item("amount") - feereader2.Item("paid")) Then
                        Session("ses" & k) = feereader2.Item("session")
                        Session("paid" & k) = feereader2.Item("paid")
                        Session("fee" & k) = feereader2.Item("fee")
                        Session("amount" & k) = feereader2.Item("amount")
                        balance = balance - (Session("amount" & k) - Session("paid" & k))
                        k = k + 1
                    ElseIf feereader2.Item("amount") = feereader2.Item("min") And balance < (feereader2.Item("amount") - feereader2.Item("paid")) And balance > 0 Then
                        Session("fee" & k) = feereader2.Item("fee")
                        Session("amount" & k) = balance
                        Session("ses" & k) = feereader2.Item("session")
                        Session("paid" & k) = feereader2.Item("paid")
                        balance = balance - (Session("amount" & k) - Session("paid" & k))
                        Exit Do
                    End If
                Loop
                feereader2.Close()

                Dim cmdInsert4 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))

                Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert4.ExecuteReader
                Do While feereader3.Read
                    If feereader3.Item("amount") <> feereader3.Item("min") And balance > (feereader3.Item("amount") - feereader3.Item("paid")) Then
                        Session("ses" & k) = feereader3.Item("session")
                        Session("paid" & k) = feereader3.Item("paid")
                        Session("fee" & k) = feereader3.Item("fee")
                        Session("amount" & k) = feereader3.Item("amount")
                        balance = balance - (Session("amount" & k) - Session("paid" & k))
                        k = k + 1
                    ElseIf feereader3.Item("amount") <> feereader3.Item("min") And balance < (feereader3.Item("amount") - feereader3.Item("paid")) And balance > 0 Then
                        Session("fee" & k) = feereader3.Item("fee")
                        Session("amount" & k) = balance
                        Session("ses" & k) = feereader3.Item("session")
                        Session("paid" & k) = feereader3.Item("paid")
                        balance = balance - (Session("amount" & k) - Session("paid" & k))
                        Exit Do
                    End If

                Loop
                feereader3.Close()
                Session("count") = k



                For k = 0 To Session("count")
                    Dim test25 As Boolean
                    Dim f5 As New Random
                    Dim ref25 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref25 As MySql.Data.MySqlClient.MySqlDataReader = ref25.ExecuteReader

                    Dim refs25 As New ArrayList
                    Do While readref25.Read
                        refs25.Add(readref25.Item(0))
                    Loop
                    Dim d5 As Integer
                    Do Until test25 = True
                        d5 = f5.Next(100000, 999999)
                        If refs25.Contains(d5) Then
                            test25 = False
                        Else
                            test25 = True
                        End If
                    Loop
                    readref25.Close()
                    If Session("amount" & k) <> Nothing Then
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule Set paid = ? where fee = ? and student = ? and session = ?", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", Session("amount" & k)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", Session("fee" & k)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("ses" & k)))
                        cmdInsert22.ExecuteNonQuery()

                        Dim cmdCheck03 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " DEBTS"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck03.ExecuteNonQuery()

                        Dim cmdCheck02 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " PAID"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck02.ExecuteNonQuery()
                        Session("fee" & k) = Nothing
                        Session("amount" & k) = Nothing
                        Session("paid" & k) = Nothing
                    End If
                Next
                Dim test26 As Boolean
                Dim f6 As New Random
                Dim ref26 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref26 As MySql.Data.MySqlClient.MySqlDataReader = ref26.ExecuteReader

                Dim refs26 As New ArrayList
                Do While readref26.Read
                    refs26.Add(readref26.Item(0))
                Loop
                Dim d6 As Integer
                Do Until test26 = True
                    d6 = f6.Next(100000, 999999)
                    If refs26.Contains(d6) Then
                        test26 = False
                    Else
                        test26 = True
                    End If
                Loop
                readref26.Close()
                Dim cmdInserta As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                cmdInserta.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInserta.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                Dim feereader As MySql.Data.MySqlClient.MySqlDataReader = cmdInserta.ExecuteReader
                Dim mini As Double
                Dim paid3 As Double
                Do While feereader.Read
                    mini = mini + feereader.Item("min")
                    paid3 = paid3 + feereader.Item("paid")
                Loop
                feereader.Close()
                If paid3 >= mini Then
                    Dim cmdInsert29 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? ", con)
                    cmdInsert29.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                    cmdInsert29.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdInsert29.ExecuteNonQuery()
                End If
                Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d6))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(init - balance, , , TriState.True)))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & student))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck.ExecuteNonQuery()

                Dim cmdCheck6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d6))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Liability"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(init - balance, , , TriState.True)))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ADVANCE FEE PAYMENT"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid in advance." & student))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck6.ExecuteNonQuery()

            End If
            con.Close()
        End Using
        Average_Age(cla, term)


        add_traits(student, term)
    End Sub

    Private Sub add_traits(student As String, term As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from traits where used = '" & 1 & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            Dim traits As New ArrayList
            Do While student03.Read()
                traits.Add(student03(0).ToString)
            Loop
            student03.Close()
            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("delete from termtraits where student = '" & student & "' and session = '" & term & "'", con)
            cmdInsert22.ExecuteNonQuery()
            For Each trait As String In traits
                Dim cmdInsert22a As New MySql.Data.MySqlClient.MySqlCommand("insert into termtraits (session, student, trait) values ('" & term & "', '" & student & "', '" & trait & "')", con)
                cmdInsert22a.ExecuteNonQuery()
            Next
            con.Close()
        End Using

    End Sub
    Protected Sub Average_Age(cla As Integer, term As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT age FROM StudentSummary WHERE class = ? and session = '" & term & "'", con)
            cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", cla))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
            Dim average As Integer
            Dim total As Integer
            Dim count As Integer
            Do While reader2.Read()
                total = total + reader2.Item("age")
                count = count + 1
            Loop
            Try
                average = total \ count
            Catch ex As Exception

            End Try

            reader2.Close()

            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set Aveage = ?, ClassNo = ? where class = ?", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", average))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", count))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            cmdInsert2.ExecuteNonQuery()
            con.Close()
        End Using
    End Sub

    Protected Sub Gridview1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Gridview1.SelectedIndexChanged

    End Sub

    Protected Sub Gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview1.SelectedIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim sessions As String = Gridview1.Rows(e.NewSelectedIndex).Cells(0).Text
                Dim terms As String = Gridview1.Rows(e.NewSelectedIndex).Cells(1).Text

                Panel1.Visible = True
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM sessioncreate", con)
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Do While reader20.Read
                    DropDownList1.Items.Add(reader20.Item(1))
                Loop
                reader20.Close()
                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM session where session = '" & sessions & "' and term = '" & terms & "'", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert2.ExecuteReader
                reader2.Read()
                Session("updateterm") = reader2(0).ToString
                DropDownList1.Text = reader2.Item(1)
                DropDownList2.Text = reader2.Item(2)
                Dim open As Date = reader2.Item(7).ToString
                Dim close As Date = reader2.Item(5).ToString
                Dim nxt As Date = reader2.Item(4).ToString
                txtOpen.Text = open
                txtClose.Text = close
                txtNext.Text = nxt
                reader2.Close()
                Button1.Text = "Update"


                con.close()            End Using
        Catch ex As Exception

            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
