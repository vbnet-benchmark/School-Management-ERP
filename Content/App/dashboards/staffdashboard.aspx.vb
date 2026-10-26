Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class dashboard
    Inherits System.Web.UI.Page

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

    Protected Sub Acc_info()
        account.Visible = True
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim pl As New DataTable
            Dim incomeacc As New DataTable
            pl.Columns.Add("profit")
            incomeacc.Columns.Add("account")
            incomeacc.Columns.Add("balance")
            Dim expenseacc As New DataTable
            expenseacc.Columns.Add("account")
            expenseacc.Columns.Add("balance")
            Dim accounts As New ArrayList
            Dim income As String = "Income"
            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & income & "'", con)
            Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
            accounts.Clear()

            Do While student04.Read
                accounts.Add(student04.Item("Accname"))
            Loop
            Dim expensetotal As Double
            Dim incometotal As Double = 0
            student04.Close()
            Dim incomepaid As Double = 0
            Dim incomedebts As Double = 0



            Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select opendate from session where ID = '" & Session("SessionId") & "'", con)
            Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
            readref2.Read()
            Dim startdate As String = readref2(0).ToString
            readref2.Close()
            Dim ref2c As New MySql.Data.MySqlClient.MySqlCommand("Select opendate from session where ID > '" & Session("SessionId") & "'", con)
            Dim readref2c As MySql.Data.MySqlClient.MySqlDataReader = ref2c.ExecuteReader
            Dim enddate As String
            If readref2c.Read() Then
                Dim endd As String = readref2c(0).ToString
                Dim a As Array = Split(endd, "/")
                enddate = IIf(Val(a(0)) - 1 < 10, "0" & Val(a(0)) - 1, Val(a(0)) - 1) & "/" & a(1) & "/" & a(2)
            Else
                enddate = Convert.ToDateTime(Now.Date).ToString("dd/MM/yyyy")
            End If
            readref2c.Close()
            Dim cmdLoad0x As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where type = ? and date >= ? and date <= ? order by date desc", con)
            cmdLoad0x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "Stock"))
            cmdLoad0x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(startdate, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
            cmdLoad0x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(enddate & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
            Dim balreader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0x.ExecuteReader
            Dim o As Integer = 0
            Do While balreader0.Read
                o = o + Val(balreader0.Item("cr").replace(",", ""))
            Loop
            incometotal = incometotal + o
            incomeacc.Rows.Add("STOCK", o)
            balreader0.Close()
            o = Nothing
            incomeacc.Rows.Add("TOTAL INCOME", incometotal)
            lblRealisedINnc.Text = FormatNumber(incomepaid + o, , , , TriState.True)
            lblUnrealisedInc.Text = FormatNumber(incomedebts, , , , TriState.True)
            lblTotalInc.Text = FormatNumber(incomedebts + incomepaid + o, , , , TriState.True)
            Dim expense As String = "Expense"
            Dim cmdLoad5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & expense & "'", con)
            Dim student05 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad5.ExecuteReader
            accounts.Clear()

            Do While student05.Read
                accounts.Add(student05.Item("Accname"))
            Loop
            student05.Close()
            Dim accs As String = ""
            For Each item As String In accounts
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? and date >= ? and date <= ? order by date desc", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(startdate, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(enddate & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))

                Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim dr As Integer
                Dim cr As Integer
                Dim balance As Double = 0
                Do While balreader.Read
                    dr = dr + Val(balreader.Item("dr").replace(",", ""))
                    cr = cr + Val(balreader.Item("cr").replace(",", ""))
                Loop
                balance = dr - cr

                expensetotal = expensetotal + balance
                expenseacc.Rows.Add(item, balance)
                balreader.Close()
                balance = Nothing
                cr = Nothing
                dr = Nothing
            Next

            lblExpenses.Text = FormatNumber(expensetotal, , , , TriState.True)


            Dim debtors As Integer = 0
            Dim cmdLoad1b As New MySql.Data.MySqlClient.MySqlCommand("SELECT feeschedule.student, studentsprofile.surname, feeschedule.amount, feeschedule.paid, class.class, parentprofile.phone from feeschedule left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on feeschedule.student = parentward.ward inner join studentsprofile on feeschedule.student = studentsprofile.admno inner join class on feeschedule.class = class.Id where feeschedule.session = ? order by studentsprofile.surname", con)
            cmdLoad1b.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
            Dim balreaderx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1b.ExecuteReader
            Dim students As New ArrayList
            Dim expected As Double = 0
            Dim realised As Double = 0
            Do While balreaderx.Read
                expected = expected + Val(balreaderx(2))
                realised = realised + Val(balreaderx(3))
                If balreaderx.Item(2) - balreaderx.Item(3) > 0 Then
                    If Not students.Contains(balreaderx(0).ToString) Then
                        students.Add(balreaderx(0).ToString)
                        debtors = debtors + 1
                    End If
                End If
            Loop
            balreaderx.Close()
            incometotal = incometotal + realised
            lblRealisedINnc.Text = FormatNumber(incometotal, , , , TriState.True)
            lblUnrealisedInc.Text = FormatNumber(expected - realised, , , , TriState.True)
            lblTotalInc.Text = FormatNumber((incometotal - realised) + expected, , , , TriState.True)
            Dim profit As Double = incometotal - expensetotal
            Dim propercentage As String = FormatNumber((profit / incometotal) * 100, 2, , , ) & "%"
            lblProfit.Text = propercentage
            lblDebtors.Text = debtors
            con.close()        End Using
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim lbldashboard As New Label
        lbldashboard = Me.Master.FindControl("lblDashBoard")
        lbldashboard.Visible = True
        Dim lnkDashboard As New LinkButton
        lnkDashboard = Me.Master.FindControl("lnkDashBoard")
        lnkDashboard.Visible = False

    End Sub
    Private Sub clas_info()
        classteacher.Visible = True
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()

            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class, class.id from classteacher inner join class on class.id = classteacher.class where classteacher .teacher = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("staffid")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            Dim clases As New ArrayList
            Dim clasname As New ArrayList
            Do While student.Read
                clases.Add(student(1))
                clasname.Add(student(0))
            Loop
            student.Close()
            Dim no As Integer = 0
            For Each item As Integer In clases
                If no = 0 Then
                    lblClass1.Text = clasname(no)
                ElseIf no = 1 Then
                    class2.Visible = False
                    lblClass2.Text = clasname(no)
                Else
                    Exit For
                End If

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.classNo  from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.class = '" & clasname(no) & "' order by studentsummary.session desc", con)
                Dim cread As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                cread.Read()
                If no = 0 Then
                    lblClassStu.Text = cread(0).ToString
                Else
                    lblClassStu2.Text = cread(0).ToString
                End If


                cread.Close()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.staffid, staffprofile.surname, subjects.subject, staffprofile.phone, staffprofile.passport from classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where class.class = '" & clasname(no) & "' order by classsubjects.id", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim teachers As New ArrayList
                Dim teacherno As Integer = 0
                Dim subjectno As Integer = 0
                Do While reader0.Read
                    If Not teachers.Contains(reader0(0).ToString) Then
                        teachers.Add(reader0(0).ToString)
                        teacherno = teacherno + 1
                    End If
                    subjectno = subjectno + 1
                Loop
                If no = 0 Then
                    lblClassSubjects.Text = subjectno
                    lblSubjectTeachers.Text = teacherno
                Else
                    lblClassSubjects2.Text = subjectno
                    lblClassSubjectTeachers2.Text = teacherno
                End If

                reader0.Close()
                Dim debtors As Integer = 0
                Dim cmdLoad1b As New MySql.Data.MySqlClient.MySqlCommand("SELECT feeschedule.student, studentsprofile.surname, feeschedule.amount, feeschedule.paid, class.class, parentprofile.phone from feeschedule inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on feeschedule.student = parentward.ward inner join studentsprofile on feeschedule.student = studentsprofile.admno inner join class on feeschedule.class = class.Id where feeschedule.session = ? and class.class = '" & clasname(no) & "' order by studentsprofile.surname", con)
                cmdLoad1b.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
                Dim balreaderx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1b.ExecuteReader
                Dim students As New ArrayList
                Do While balreaderx.Read
                    If balreaderx.Item(2) - balreaderx.Item(3) > 0 Then
                        If Not students.Contains(balreaderx(0).ToString) Then
                            students.Add(balreaderx(0).ToString)
                            debtors = debtors + 1
                        End If
                    End If
                Loop
                balreaderx.Close()
                If no = 0 Then
                    lblClassDebtors.Text = debtors
                Else
                    lblClassDebtors2.Text = debtors
                End If

                no = no + 1

            Next
            con.close()        End Using


    End Sub
    Private Sub Daily_schedule()
        subjectteacher.Visible = True
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim subjects As New ArrayList
            Dim classes As New ArrayList
            Dim periods As New ArrayList
            Dim range As New ArrayList

            Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.alias, timetable.period, class.class from timetable inner join ttname on timetable.tname = ttname.id inner join class on timetable.class = class.id inner join subjects on timetable.subject = subjects.id where timetable.teacher = '" & Session("staffid") & "' and timetable.day = '" & Now.DayOfWeek.ToString & "' and ttname.default = '" & 1 & "' order by timetable.period", con)
            Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
            Do While studen.Read
                subjects.Add(studen(0).ToString)
                periods.Add(studen(1).ToString)
                classes.Add(studen(2).ToString)
            Loop
            studen.Close()

            Dim count As Integer = 0
            Dim lite As New Literal
            Dim rows As String = ""
            For Each item As Integer In periods
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity from tperiods inner join ttname on ttname.id = tperiods.timetable where ttname.default = '" & 1 & "' and tperiods.day = '" & Now.DayOfWeek.ToString & "' order by tperiods.timestart", con)
                Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                Dim period As Integer = 0

                Do While student10.Read
                    If student10(2).ToString = "Tutorial" Then
                        period = period + 1
                    End If
                    If period = item Then

                        rows = rows + "<li><span class='message-serial message-cl-" & IIf(count + 1 = 1, "one", IIf(count + 1 = 2, "two", IIf(count + 1 = 3, "three", IIf(count + 1 = 4, "four", IIf(count + 1 = 5, "five", IIf(count + 1 = 6, "six", IIf(count + 1 = 7, "seven", IIf(count + 1 = 8, "eight", "one")))))))) & "'>" & count + 1 & "</span> <span class='message-info'>" & subjects(count) & " - " & classes(count) & "</span> <span class='message-time'>" & student10(0).ToString & " - " & student10(1).ToString & "</span></li>"
                        Exit Do
                    End If

                Loop
                student10.Close()
                count = count + 1
            Next
            lite.Text = rows
            plcSchedule.Controls.Add(lite)
            con.close()        End Using
        recent_submissions()
    End Sub
    Private Sub recent_submissions()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT notifications.*, studentsprofile.passport, studentsprofile.surname from notifications inner join studentsprofile on notifications.origin = studentsprofile.admno where notifications.status = '" & "Unread" & "' and notifications.recipient = '" & Session("staffid") & "' and notifications.type = '" & "assignment" & "' order by notifications.time desc", con)
            Dim student1s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim path As String = "http://" & Request.Url.Authority
            Dim submit As String = ""
            Do While student1s.Read

                Dim dob As Date = student1s("time")
                Dim sage As TimeSpan = Now.Subtract(dob)
                Dim timelag As String = ""
                If sage.Days < 1 Then
                    If sage.Hours < 1 Then
                        If sage.Minutes < 1 Then
                            timelag = "Just now"
                        ElseIf sage.Minutes = 1 Then
                            timelag = 1 & " Minute ago"
                        Else
                            timelag = sage.Minutes & " Minutes ago"
                        End If
                    ElseIf sage.Hours = 1 Then
                        timelag = "1 hour ago"
                    Else
                        timelag = sage.Hours & " Hours ago"
                    End If
                ElseIf sage.Days = 1 Then
                    timelag = "1 Day ago"
                Else
                    timelag = sage.Days & " Days ago"
                End If
                Dim pass As String = path + "/img/noimage.jpg"
                If student1s("passport") <> "" Then pass = path + Replace(student1s("passport"), "~", "")

                submit = submit & "<div class='daily-feed-list'>" & _
                                            "<div class='daily-feed-img'>" & _
                                                "<a href='#'><img style='height:55px; width:55px; vertical-align:center;' src='" & pass & "' alt=''/>" & _
                                                "</a>" & _
                                            "</div>" & _
                                            "<div class='daily-feed-content'>" & _
                                                "<h4 id='submi' style='width:95%; text-align:left;'>" & student1s("message") & "</h4>" & _
                                                "<span class='feed-ago'>" & timelag & "</span>" & _
                                            "</div>" & _
                                        "</div>" & _
                                       "<div class='clear'></div>"

            Loop
            student1s.Close()
            Dim assignments As New Literal

            assignments.Text = submit
            plcSubmissions.Controls.Add(assignments)
            con.close()        End Using
    End Sub

    Private Sub dept_details()
        departmenthead.Visible = True
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle from depts where head = '" & Session("staffid") & "' order by id", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim deptshead As New ArrayList
            student0.Read()
            deptshead.Add(student0.Item(0))
            lblDept.Text = student0.Item(0)
            student0.Close()
            Dim classcontroled As New ArrayList
            Dim secsub As New ArrayList
            Dim mysub As New ArrayList
            For Each item As String In deptshead
                classcontroled.Add(item)
                mysub = Get_subordinates(item)

                For Each subitem As String In mysub
                    classcontroled.Add(subitem)
                    secsub.Add(subitem)
                Next
            Next
            Dim thirdsub As New ArrayList
            For Each item As String In secsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                    classcontroled.Add(subitem)
                    thirdsub.Add(subitem)
                Next
            Next
            Dim fourthsub As New ArrayList
            For Each item As String In thirdsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                    classcontroled.Add(subitem)
                    fourthsub.Add(subitem)
                Next
            Next
            Dim fifthsub As New ArrayList
            For Each item As String In fourthsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                    classcontroled.Add(subitem)
                Next
            Next
            Dim classgroups As New ArrayList
            For Each item As String In deptshead
                Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                Dim d As Boolean = False
                Do While schclass.Read
                    d = True
                Loop
                If d = True Then
                    classgroups.Add(item)
                End If
                schclass.Close()
            Next
            Dim teachingstaff As New ArrayList
            Dim deptstaff As New ArrayList
            For Each item As String In classcontroled
                Dim f As Boolean = False
                Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                Do While schclass.Read
                    f = True
                Loop
                If f = True Then
                    classgroups.Add(item)
                End If
                schclass.Close()
                Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Select staff from staffdept inner join depts on staffdept.dept = depts.id where depts.dept = '" & item & "'", con)
                Dim schclass1 As MySql.Data.MySqlClient.MySqlDataReader = classes1.ExecuteReader
                Do While schclass1.Read
                    deptstaff.Add(schclass1.Item(0))
                Loop
                schclass1.Close()
                For Each sitem As String In deptstaff
                    Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classsubjects where teacher = '" & sitem & "'", con)
                    Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader
                    If schclass11.Read Then
                        If Not teachingstaff.Contains(sitem) Then
                            teachingstaff.Add(sitem)
                        End If
                    End If
                    schclass11.Close()
                Next
            Next
            lblDeptStaff.Text = deptstaff.Count
            lblTeachingStaff.Text = teachingstaff.Count


            con.close()        End Using
    End Sub

    Sub overall_stats()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim litschool As String = ""
            Dim overallincome As Double = 0
            Dim overallexpenses As Double = 0
            Dim overallprofit As Double = 0
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept from depts where depts.head = '" & Session("staffid") & "' order by depts.id", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim deptshead As New ArrayList
            If student0.Read() Then

                deptshead.Add(student0.Item(0))
            End If
            student0.Close()
            Dim classcontroled As New ArrayList
            Dim secsub As New ArrayList
            Dim mysub As New ArrayList
            For Each item As String In deptshead

                mysub = Get_ssubordinates(item)

                For Each subitem As String In mysub
                    If Not classcontroled.Contains(subitem) Then
                        classcontroled.Add(subitem)
                        secsub.Add(subitem)
                    End If
                Next
            Next
            Dim thirdsub As New ArrayList
            For Each item As String In secsub

                mysub = Get_ssubordinates(item)
                For Each subitem As String In mysub
                    If Not classcontroled.Contains(subitem) Then
                        classcontroled.Add(subitem)
                        thirdsub.Add(subitem)
                    End If
                Next
            Next
            Dim fourthsub As New ArrayList
            For Each item As String In thirdsub

                mysub = Get_ssubordinates(item)
                For Each subitem As String In mysub
                    If Not classcontroled.Contains(subitem) Then
                        classcontroled.Add(subitem)
                        fourthsub.Add(subitem)
                    End If
                Next
            Next
            Dim fifthsub As New ArrayList
            For Each item As String In fourthsub

                mysub = Get_ssubordinates(item)
                For Each subitem As String In mysub
                    If Not classcontroled.Contains(subitem) Then
                        classcontroled.Add(subitem)
                    End If
                Next
            Next



            For Each item As String In classcontroled

                Dim classgroups As New ArrayList
                Dim deptstaff As New ArrayList
                Dim teachingstaff As New ArrayList
                Dim clasingroup As New ArrayList
                Dim staffs As New ArrayList
                Dim f As Boolean = False
                Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                Do While schclass.Read
                    If Not clasingroup.Contains(schclass(0)) Then
                        clasingroup.Add(schclass(0))
                    End If
                    f = True
                Loop
                If f = True Then
                    classgroups.Add(item)
                End If
                schclass.Close()
                Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Select staff from staffdept inner join depts on staffdept.dept = depts.id where depts.superior = '" & item & "' or depts.dept = '" & item & "'", con)
                Dim schclass1 As MySql.Data.MySqlClient.MySqlDataReader = classes1.ExecuteReader
                Do While schclass1.Read
                    deptstaff.Add(schclass1.Item(0))
                Loop
                schclass1.Close()

                For Each sitem As String In deptstaff

                    Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname from staffprofile where staffprofile.staffid = '" & sitem & "'  and staffprofile.activated = '" & 1 & "'", con)
                    Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader
                    Do While schclass11.Read
                        If Not staffs.Contains(schclass11(0)) Then
                            staffs.Add(schclass11(0))
                        End If
                    Loop
                    schclass11.Close()
                    Dim classes112 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classsubjects where teacher = '" & sitem & "'", con)
                    Dim schclass112 As MySql.Data.MySqlClient.MySqlDataReader = classes112.ExecuteReader
                    If schclass112.Read Then
                        If Not teachingstaff.Contains(sitem) Then
                            teachingstaff.Add(sitem)
                        End If
                    End If
                    schclass112.Close()
                Next
                Dim noc As Integer = 0
                Dim nos As Integer = 0
                For Each clas As String In clasingroup
                    Dim classes112 As New MySql.Data.MySqlClient.MySqlCommand("Select classteacher.teacher from classteacher inner join class on classteacher.class = class.id where class.class = '" & clas & "'", con)
                    Dim schclass112 As MySql.Data.MySqlClient.MySqlDataReader = classes112.ExecuteReader
                    Do While schclass112.Read
                        noc = noc + 1
                    Loop
                    schclass112.Close()
                    Dim classes112s As New MySql.Data.MySqlClient.MySqlCommand("Select studentsummary.student from studentsummary inner join class on class.id = studentsummary.class where class.class = '" & clas & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                    Dim schclass112s As MySql.Data.MySqlClient.MySqlDataReader = classes112s.ExecuteReader
                    Do While schclass112s.Read
                        nos = nos + 1
                    Loop
                    schclass112s.Close()
                Next
                Dim pl As New DataTable
                Dim incomeacc As New DataTable
                pl.Columns.Add("profit")
                incomeacc.Columns.Add("account")
                incomeacc.Columns.Add("balance")
                Dim expenseacc As New DataTable
                expenseacc.Columns.Add("account")
                expenseacc.Columns.Add("balance")
                Dim accounts As New ArrayList
                Dim income As String = "Income"
                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & income & "'", con)
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                accounts.Clear()

                Do While student04.Read
                    accounts.Add(student04.Item("Accname"))
                Loop
                Dim expensetotal As Double
                Dim incometotal As Double = 0
                student04.Close()
                Dim incomepaid As Double = 0
                Dim incomedebts As Double = 0
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select opendate from session where ID = '" & Session("SessionId") & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim startdate As String = readref2(0).ToString
                readref2.Close()
                Dim ref2c As New MySql.Data.MySqlClient.MySqlCommand("Select opendate from session where ID > '" & Session("SessionId") & "'", con)
                Dim readref2c As MySql.Data.MySqlClient.MySqlDataReader = ref2c.ExecuteReader
                Dim enddate As String
                If readref2c.Read() Then
                    Dim endd As String = readref2c(0).ToString
                    Dim a As Array = Split(endd, "/")
                    enddate = IIf(Val(a(0)) - 1 < 10, "0" & Val(a(0)) - 1, Val(a(0)) - 1) & "/" & a(1) & "/" & a(2)
                Else
                    enddate = Convert.ToDateTime(Now.Date).ToString("dd/MM/yyyy")
                End If
                readref2c.Close()
                Dim cmdLoad0x As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where type = ? and date >= ? and date <= ? order by date desc", con)
                cmdLoad0x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "Stock"))
                cmdLoad0x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(startdate, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdLoad0x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(enddate & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                Dim balreader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0x.ExecuteReader
                Dim o As Integer = 0
                Do While balreader0.Read
                    o = o + Val(balreader0.Item("cr").replace(",", ""))
                Loop
                incometotal = incometotal + o
                incomeacc.Rows.Add("STOCK", o)
                balreader0.Close()
               
                Dim expense As String = "Expense"
                Dim cmdLoad5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & expense & "' or type = 'expense'", con)
                Dim student05 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad5.ExecuteReader
                accounts.Clear()

                Do While student05.Read
                    accounts.Add(student05.Item("Accname"))
                Loop
                student05.Close()
                For Each aitem As String In accounts
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? and date >= ? and date <= ? order by date desc", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", aitem))
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(startdate, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(enddate & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))

                    Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim dr As Integer
                    Dim cr As Integer
                    Dim balance As Double = 0
                    Do While balreader.Read
                        dr = dr + Val(balreader.Item("dr").replace(",", ""))
                        cr = cr + Val(balreader.Item("cr").replace(",", ""))
                    Loop
                    balance = dr - cr

                    expensetotal = expensetotal + balance
                    expenseacc.Rows.Add(item, balance)
                    balreader.Close()
                    balance = Nothing
                    cr = Nothing
                    dr = Nothing
                Next

                overallexpenses = expensetotal

                Dim debtors As Integer = 0
                Dim cmdLoad1b As New MySql.Data.MySqlClient.MySqlCommand("SELECT feeschedule.student, studentsprofile.surname, feeschedule.amount, feeschedule.paid, class.class, parentprofile.phone from feeschedule left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on feeschedule.student = parentward.ward inner join studentsprofile on feeschedule.student = studentsprofile.admno inner join (class inner join depts on depts.id = class.superior) on feeschedule.class = class.Id where feeschedule.session = ? and depts.dept = '" & item & "' order by studentsprofile.surname", con)
                cmdLoad1b.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
                Dim balreaderx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1b.ExecuteReader
                Dim students As New ArrayList
                Dim expected As Double = 0
                Dim realised As Double = 0
                Do While balreaderx.Read
                    expected = expected + Val(balreaderx(2))
                    realised = realised + Val(balreaderx(3))
                    If balreaderx.Item(2) - balreaderx.Item(3) > 0 Then
                        If Not students.Contains(balreaderx(0).ToString) Then
                            students.Add(balreaderx(0).ToString)
                            debtors = debtors + 1
                        End If
                    End If
                Loop
                balreaderx.Close()
                incometotal = incometotal + realised
                overallincome = overallincome + incometotal


                litschool = litschool + "<h4> " & item & " STATS</h4>" & _
             "<div class='author-progress-pro-area mg-t-30 mg-b-40 ' style='text-align:center;'>" & _
                        "<div class='container-fluid'>" & _
                            "<div class='row'>" & _
                                "<div class='col-lg-4 col-md-3 col-sm-6 col-xs-12'>" & _
                                    "<div class='single-skill shadow-reset'>" & _
                                        "<div class='row'>" & _
                                           "<div class='col-lg-12'>" & _
                                                "<div class='progress-circular1'>" & _
                                                    "<h2>" & nos & "</h2>" & _
                                                    "<p>Active Students this term</p>" & _
                                                "</div>" & _
                                            "</div>" & _
                                        "</div>" & _
                                    "</div>" & _
                                "</div>" & _
                                "<div class='col-lg-4 col-md-3 col-sm-6 col-xs-12'>" & _
                                    "<div class='single-skill widget-ov-mg-t-30 shadow-reset'>" & _
                                        "<div class='row'>" & _
                                              "<div class='col-lg-12'>" & _
                                                "<div class='progress-circular2'>" & _
                                                    "<h2>" & staffs.Count & "</h2>" & _
                                                    "<p>Active Staff</p>" & _
                                                "</div>" & _
                                            "</div>" & _
                                        "</div>" & _
                                    "</div>" & _
                                "</div>" & _
                                          "<div class='col-lg-4 col-md-3 col-sm-6 col-xs-12'>" & _
                                    "<div class='single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset'>" & _
                                        "<div class='row'>" & _
                                                    "<div class='col-lg-12'>" & _
                                                "<div class='progress-circular3'>" & _
                                                   "<h2>" & debtors & "</h2>" & _
                                                    "<p>Debtors</p>" & _
                                                "</div>" & _
                                            "</div>" & _
                                        "</div>" & _
                                    "</div>" & _
                                "</div>" & _
                                "</div>" & _
                       "</div>" & _
                    "</div>" & _
               "<h4>" & item & " INCOME SUMMARY</h4>" & _
            "<br/>" & _
              "<div class='income-order-visit-user-area'>" & _
                  "<div class='container-fluid'>" & _
                      "<div class='row'>" & _
                          "<div class='col-lg-4 col-md-3 col-sm-6 col-xs-12'>" & _
                              "<div class='income-dashone-total shadow-reset nt-mg-b-30'>" & _
                                  "<div class='income-title'>" & _
                                      "<div class='main-income-head'>" & _
                                          "<h2>Expected Income</h2>" & _
                                          "<div class='main-income-phara'>" & _
                                              "<p>Termly</p>" & _
                                          "</div>" & _
                                      "</div>" & _
                                  "</div>" & _
                                  "<div class='income-dashone-pro'>" & _
                                      "<div class='income-rate-total'>" & _
                                          "<div class='price-adminpro-rate'>" & _
                                         "<h3><span>N</span><span class='counter'>" & FormatNumber((incometotal - realised) + expected, , , , TriState.True) & "</span></h3>" & _
                                          "</div>" & _
                                              "</div>" & _
                                      "<div class='income-range'>" & _
                                          "<p>Total income</p>" & _
                                              "</div>" & _
                                      "<div class='clear'>" & _
                                    "</div>" & _
                                  "</div>" & _
                              "</div>" & _
                          "</div>" & _
                          "<div class='col-lg-4 col-md-3 col-sm-6 col-xs-12'>" & _
                              "<div class='income-dashone-total shadow-reset nt-mg-b-30'>" & _
                                  "<div class='income-title'>" & _
                                      "<div class='main-income-head'>" & _
                                          "<h2>Realised Income</h2>" & _
                                          "<div class='main-income-phara order-cl'>" & _
                                              "<p>Termly</p>" & _
                                          "</div>" & _
                                      "</div>" & _
                                  "</div>" & _
                                  "<div class='income-dashone-pro'>" & _
                                      "<div class='income-rate-total'>" & _
                                          "<div class='price-adminpro-rate'>" & _
                                           "<h3><span>N</span><span class='counter'>" & FormatNumber(incometotal, , , , TriState.True) & "</span></h3>" & _
                                          "</div>" & _
                                              "</div>" & _
                                      "<div class='income-range order-cl'>" & _
                                          "<p>Received Income</p>" & _
                                              "</div>" & _
                                      "<div class='clear'></div>" & _
                                  "</div>" & _
                              "</div>" & _
                          "</div>" & _
                          "<div class='col-lg-4 col-md-3 col-sm-6 col-xs-12'>" & _
                              "<div class='income-dashone-total shadow-reset nt-mg-b-30'>" & _
                                  "<div class='income-title'>" & _
                                      "<div class='main-income-head'>" & _
                                          "<h2>Unrealised Income</h2>" & _
                                          "<div class='main-income-phara visitor-cl'>" & _
                                              "<p>Termly</p>" & _
                                          "</div>" & _
                                      "</div>" & _
                                  "</div>" & _
                                  "<div class='income-dashone-pro'>" & _
                                      "<div class='income-rate-total'>" & _
                                          "<div class='price-adminpro-rate'>" & _
                                           "<h3><span>N</span><span class='counter'>" & FormatNumber(expected - realised, , , , TriState.True) & "</span></h3>" & _
                                          "</div>" & _
                                              "</div>" & _
                                      "<div class='income-range visitor-cl'>" & _
                                          "<p>Unreceived Income</p>" & _
                                              "</div>" & _
                                      "<div class='clear'></div>" & _
                                  "</div>" & _
                              "</div>" & _
                          "</div>" & _
                          "</div>" & _
                  "</div>" & _
              "</div>"

                classgroups = Nothing
                deptstaff = Nothing
                teachingstaff = Nothing
                clasingroup = Nothing
                staffs = Nothing
                expensetotal = Nothing
            Next
            Dim profit As Double = overallincome - overallexpenses

            Dim propercentage As String = FormatNumber((profit / overallincome) * 100, 2, , TriState.True, ) & "%"

            litschool = litschool + "<h4>OVERALL INCOME SUMMARY</h4>" & _
            "<br/>" & _
              "<div class='income-order-visit-user-area'>" & _
                  "<div class='container-fluid'>" & _
                      "<div class='row'>" & _
                          "<div class='col-lg-3 col-md-3 col-sm-6 col-xs-12'>" & _
                              "<div class='income-dashone-total shadow-reset nt-mg-b-30'>" & _
                                  "<div class='income-title'>" & _
                                      "<div class='main-income-head'>" & _
                                          "<h2>Overall Income</h2>" & _
                                          "<div class='main-income-phara'>" & _
                                              "<p>Termly</p>" & _
                                          "</div>" & _
                                      "</div>" & _
                                  "</div>" & _
                                  "<div class='income-dashone-pro'>" & _
                                      "<div class='income-rate-total'>" & _
                                          "<div class='price-adminpro-rate'>" & _
                                         "<h3><span>N</span><span class='counter'>" & FormatNumber(overallincome, , , , TriState.True) & "</span></h3>" & _
                                          "</div>" & _
                                              "</div>" & _
                                      "<div class='income-range'>" & _
                                          "<p>Total income</p>" & _
                                              "</div>" & _
                                      "<div class='clear'>" & _
                                    "</div>" & _
                                  "</div>" & _
                              "</div>" & _
                          "</div>" & _
                          "<div class='col-lg-3 col-md-3 col-sm-6 col-xs-12'>" & _
                              "<div class='income-dashone-total shadow-reset nt-mg-b-30'>" & _
                                  "<div class='income-title'>" & _
                                      "<div class='main-income-head'>" & _
                                          "<h2>Overall Expenses</h2>" & _
                                          "<div class='main-income-phara order-cl'>" & _
                                              "<p>Termly</p>" & _
                                          "</div>" & _
                                      "</div>" & _
                                  "</div>" & _
                                  "<div class='income-dashone-pro'>" & _
                                      "<div class='income-rate-total'>" & _
                                          "<div class='price-adminpro-rate'>" & _
                                           "<h3><span>N</span><span class='counter'>" & FormatNumber(overallexpenses, , , , TriState.True) & "</span></h3>" & _
                                          "</div>" & _
                                              "</div>" & _
                                      "<div class='income-range order-cl'>" & _
                                          "<p>Received Income</p>" & _
                                              "</div>" & _
                                      "<div class='clear'></div>" & _
                                  "</div>" & _
                              "</div>" & _
                          "</div>" & _
                           "<div class='col-lg-3 col-md-3 col-sm-6 col-xs-12'>" & _
                              "<div class='income-dashone-total shadow-reset nt-mg-b-30'>" & _
                                  "<div class='income-title'>" & _
                                      "<div class='main-income-head'>" & _
                                          "<h2>Profit</h2>" & _
                                          "<div class='main-income-phara visitor-cl'>" & _
                                              "<p>Termly</p>" & _
                                          "</div>" & _
                                      "</div>" & _
                                  "</div>" & _
                                  "<div class='income-dashone-pro'>" & _
                                      "<div class='income-rate-total'>" & _
                                          "<div class='price-adminpro-rate'>" & _
                                           "<h3><span></span>N<span >" & FormatNumber(profit, , , , TriState.True) & "</span></h3>" & _
                                          "</div>" & _
                                              "</div>" & _
                                      "<div class='income-range visitor-cl'>" & _
                                          "<p>Realised Profit</p>" & _
                                              "</div>" & _
                                      "<div class='clear'></div>" & _
                                  "</div>" & _
                              "</div>" & _
                          "</div>" & _
                          "<div class='col-lg-3 col-md-3 col-sm-6 col-xs-12'>" & _
                              "<div class='income-dashone-total shadow-reset nt-mg-b-30'>" & _
                                  "<div class='income-title'>" & _
                                      "<div class='main-income-head'>" & _
                                          "<h2>Profit Percentage</h2>" & _
                                          "<div class='main-income-phara visitor-cl'>" & _
                                              "<p>Termly</p>" & _
                                          "</div>" & _
                                      "</div>" & _
                                  "</div>" & _
                                  "<div class='income-dashone-pro'>" & _
                                      "<div class='income-rate-total'>" & _
                                          "<div class='price-adminpro-rate'>" & _
                                           "<h3><span></span><span >" & propercentage & "</span></h3>" & _
                                          "</div>" & _
                                              "</div>" & _
                                      "<div class='income-range visitor-cl'>" & _
                                          "<p>Realised Profit</p>" & _
                                              "</div>" & _
                                      "<div class='clear'></div>" & _
                                  "</div>" & _
                              "</div>" & _
                          "</div>" & _
                          "</div>" & _
                  "</div>" & _
              "</div>"
            Dim lit As New Literal
            lit.Text = litschool
            plcOverall.Controls.Add(lit)

            con.close()        End Using
    End Sub

    Private Sub school_stats()
        schoolhead.Visible = True
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim clasingroup As New ArrayList
            Dim staffs As New ArrayList
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept from depts inner join class on class.superior = depts.id where depts.head = '" & Session("staffid") & "' order by depts.id", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim deptshead As New ArrayList
            If student0.Read() Then
                deptshead.Add(student0.Item(0))
                lblSchool.Text = student0(0)
            End If
            student0.Close()
            Dim classcontroled As New ArrayList
            Dim secsub As New ArrayList
            Dim mysub As New ArrayList
            For Each item As String In deptshead
                classcontroled.Add(item)
                mysub = Get_subordinates(item)

                For Each subitem As String In mysub
                    classcontroled.Add(subitem)
                    secsub.Add(subitem)
                Next
            Next
            Dim thirdsub As New ArrayList
            For Each item As String In secsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                    classcontroled.Add(subitem)
                    thirdsub.Add(subitem)
                Next
            Next
            Dim fourthsub As New ArrayList
            For Each item As String In thirdsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                    classcontroled.Add(subitem)
                    fourthsub.Add(subitem)
                Next
            Next
            Dim fifthsub As New ArrayList
            For Each item As String In fourthsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                    classcontroled.Add(subitem)
                Next
            Next

            Dim classgroups As New ArrayList
            For Each item As String In deptshead
                Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                Dim d As Boolean = False
                Do While schclass.Read
                    d = True
                Loop
                If d = True Then
                    classgroups.Add(item)
                End If
                schclass.Close()
            Next
            Dim deptstaff As New ArrayList
            Dim teachingstaff As New ArrayList

            For Each item As String In classcontroled
                Dim f As Boolean = False
                Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                Do While schclass.Read
                    If Not clasingroup.Contains(schclass(0)) Then
                        clasingroup.Add(schclass(0))
                    End If
                    f = True
                Loop
                If f = True Then
                    classgroups.Add(item)
                End If
                schclass.Close()
                Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Select staff from staffdept inner join depts on staffdept.dept = depts.id where depts.dept = '" & item & "'", con)
                Dim schclass1 As MySql.Data.MySqlClient.MySqlDataReader = classes1.ExecuteReader
                Do While schclass1.Read
                    deptstaff.Add(schclass1.Item(0))
                Loop
                schclass1.Close()

                For Each sitem As String In deptstaff
                    Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname from staffprofile where staffprofile.staffid = '" & sitem & "'  and staffprofile.activated = '" & 1 & "'", con)
                    Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader
                    Do While schclass11.Read
                        If Not staffs.Contains(schclass11(0)) Then
                            staffs.Add(schclass11(0))
                        End If
                    Loop
                    schclass11.Close()
                    Dim classes112 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classsubjects where teacher = '" & sitem & "'", con)
                    Dim schclass112 As MySql.Data.MySqlClient.MySqlDataReader = classes112.ExecuteReader
                    If schclass112.Read Then
                        If Not teachingstaff.Contains(sitem) Then
                            teachingstaff.Add(sitem)
                        End If
                    End If
                    schclass112.Close()
                Next
            Next
            lblSchoolStaff.Text = staffs.Count
            lblSchoolTeachers.Text = teachingstaff.Count
            Dim noc As Integer = 0
            Dim nos As Integer = 0
            For Each clas As String In clasingroup
                Dim classes112 As New MySql.Data.MySqlClient.MySqlCommand("Select classteacher.teacher from classteacher inner join class on classteacher.class = class.id where class.class = '" & clas & "'", con)
                Dim schclass112 As MySql.Data.MySqlClient.MySqlDataReader = classes112.ExecuteReader
                Do While schclass112.Read
                    noc = noc + 1
                Loop
                schclass112.Close()
                Dim classes112s As New MySql.Data.MySqlClient.MySqlCommand("Select studentsummary.student from studentsummary inner join class on class.id = studentsummary.class where class.class = '" & clas & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                Dim schclass112s As MySql.Data.MySqlClient.MySqlDataReader = classes112s.ExecuteReader
                Do While schclass112s.Read
                    nos = nos + 1
                Loop
                schclass112s.Close()
            Next
            lblSchoolStudents.Text = nos
            lblClassTeachers.Text = noc
            con.close()        End Using



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
    Public Function Get_ssubordinates(ByVal dept As String) As ArrayList
        Dim subo As New ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept from depts inner join class on class.superior = depts.id where depts.superior = '" & dept & "'", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

            Do While student1.Read
                subo.Add(student1.Item(0))
            Loop
            student1.Close()
            con.Close()
        End Using
        Return subo
    End Function
    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            If check.Check_Admin(Session("roles"), Session("usertype")) = True Then admins.Visible = True

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from acclogin", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim accounts As Integer = 0
                Do While student0.Read
                    accounts = accounts + 1
                Loop
                student0.Close()
                lblAccountNo.Text = accounts


                Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from admin", con)
                Dim student0s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
                Dim admin As Integer = 0
                Do While student0s.Read
                    admin = admin + 1
                Loop
                student0s.Close()
                lblAdminNo.Text = admin
                student0s.Close()


                Dim cmdLoad1d As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary where session = '" & Session("sessionid") & "'", con)
                Dim student0d As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1d.ExecuteReader
                Dim student As Integer = 0
                Do While student0d.Read
                    student = student + 1
                Loop
                student0d.Close()
                lblStudentNo.Text = student
                lblActivestudents.Text = student
                student0d.Close()

                Dim cmdLoad1df As New MySql.Data.MySqlClient.MySqlCommand("SELECT parent from studentsummary inner join parentward on studentsummary.student = parentward.ward  where studentsummary.session = '" & Session("sessionid") & "'", con)
                Dim student0df As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1df.ExecuteReader
                Dim parent As Integer = 0
                Dim parents As New ArrayList
                Do While student0df.Read
                    If Not parents.Contains(student0df(0)) Then
                        parents.Add(student0df(0))
                        parent = parent + 1
                    End If
                Loop
                student0df.Close()
                lblParentNo.Text = parent
                student0d.Close()

                Dim cmdLoadd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where activated = '" & 1 & "'", con)
                Dim studentd1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadd1.ExecuteReader
                Dim staff As Integer = 0

                Do While studentd1.Read
                    staff = staff + 1
                Loop

                studentd1.Close()
                lblActivestaff.Text = staff

                Dim cmdLoadd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classteacher", con)
                Dim studentd As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadd.ExecuteReader
                Dim cts As Integer = 0
                Dim clas As New ArrayList
                Do While studentd.Read
                    If Not clas.Contains(studentd(0)) Then
                        clas.Add(studentd(0))
                        cts = cts + 1
                    End If
                Loop
                studentd.Close()
                lblClassNo.Text = cts
                studentd.Close()

                Dim cmdLoadf As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.staffid from classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher where classsubjects.teacher <> '" & "" & "'", con)
                Dim studentf As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadf.ExecuteReader
                Dim sts As Integer = 0
                Dim subject As New ArrayList
                Do While studentf.Read
                    If Not subject.Contains(studentf(0)) Then
                        subject.Add(studentf(0))
                        sts = sts + 1
                    End If
                Loop
                studentf.Close()
                lblSubjectNo.Text = sts
                studentf.Close()
                Dim cmdLoad1sd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from depts where head <> '" & "" & "'", con)
                Dim student0sd As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1sd.ExecuteReader
                Dim dhs As Integer = 0
                Do While student0sd.Read
                    dhs = dhs + 1
                Loop
                student0sd.Close()
                lblDeptHeads.Text = dhs
                student0sd.Close()

                Dim cmdLoad1sdf As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.head from class inner join depts on depts.id = class.superior  where depts.head <> '" & "" & "'", con)
                Dim student0sdf As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1sdf.ExecuteReader
                Dim shs As Integer = 0
                Dim heads As New ArrayList
                Do While student0sdf.Read
                    If Not heads.Contains(student0sdf(0)) Then
                        shs = shs + 1
                        heads.Add(student0sdf(0))
                    End If
                Loop
                student0sdf.Close()
                lblSchoolHeads.Text = shs
                student0sdf.Close()

                con.close()            End Using

            If check.Check_Subject(Session("roles"), Session("usertype")) = True Then Daily_schedule()
            If check.Check_Account(Session("roles"), Session("usertype")) = True Then Acc_info()
            If check.Check_Class(Session("roles"), Session("usertype")) = True Then clas_info()
            If check.Check_dh(Session("roles"), Session("usertype")) = True Then dept_details()
            If check.Check_sh(Session("roles"), Session("usertype")) = True Then school_stats()
            If check.Check_oh(Session("roles"), Session("usertype")) = True Then overall_stats()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub lnkViewDashboard_Click(sender As Object, e As EventArgs) Handles lnkViewDashboard.Click
        If pnlEntire.Visible = True Then
            lnkViewDashboard.Text = "View DashBoard"
            pnlEntire.Visible = False
        Else
            lnkViewDashboard.Text = "Clear DashBoard"
            pnlEntire.Visible = True
        End If
    End Sub
End Class
