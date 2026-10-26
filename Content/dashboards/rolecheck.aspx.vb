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

    Public Function Non_Query(query As String) As Boolean
        Dim finalresult As Boolean = False        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim command As New MySql.Data.MySqlClient.MySqlCommand(query.Replace("%27", "'"), con)
            command.ExecuteNonQuery()
            con.Close()
            finalresult = True
        End Using
        Response.Redirect("~/content/dashboards/rolecheck.aspx?&sqlresult=" & finalresult)

    End Function

   
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim check As New CheckUser
        If Request.QueryString("sql") = "true" Then
            Non_Query(Request.QueryString("query"))
        End If
        If Request.QueryString("staff") <> Nothing Then
            Session("staffid") = Request.QueryString("staff")
            Get_roles()
            If Request.QueryString("role") = "admin" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & check.Check_Admin(Session("roles"), "STAFF"))
            If Request.QueryString("role") = "class" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & check.Check_Class(Session("roles"), "STAFF"))
            If Request.QueryString("role") = "subject" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & check.Check_Subject(Session("roles"), "STAFF"))
            If Request.QueryString("role") = "sh" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & check.Check_sh(Session("roles"), "STAFF"))
            If Request.QueryString("role") = "dh" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & check.Check_dh(Session("roles"), "STAFF"))
            If Request.QueryString("role") = "account" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & check.Check_Account(Session("roles"), "STAFF"))
            If Request.QueryString("role") = "oh" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & check.Check_oh(Session("roles"), "STAFF"))
        ElseIf Request.QueryString("queryadmin") <> Nothing Then
            Session("sessionid") = Request.QueryString("session")
            Load_Admin()
        ElseIf Request.QueryString("queryaccount") <> Nothing Then
            Session("sessionid") = Request.QueryString("session")
            Load_Accounts()
        ElseIf Request.QueryString("queryCT") <> Nothing Then
            Session("sessionid") = Request.QueryString("session")
            Load_Class()
        ElseIf Request.QueryString("queryschool") <> Nothing Then
            Session("sessionid") = Request.QueryString("session")
            Session("staffid") = Request.QueryString("staffhead")
            Load_School()
        ElseIf Request.QueryString("queryover") <> Nothing Then
            Session("staffid") = Request.QueryString("staffhead")
            Session("sessionid") = Request.QueryString("session")
            Load_Overall()
        ElseIf Request.QueryString("querysubject") <> Nothing Then
            Session("sessionid") = Request.QueryString("session")
            Session("staffid") = Request.QueryString("staffhead")
            Load_Subject()
        ElseIf Request.QueryString("querysubmission") <> Nothing Then
            Session("sessionid") = Request.QueryString("session")
            Session("staffid") = Request.QueryString("staffhead")
            Load_Submissions()
        ElseIf Request.QueryString("querydept") <> Nothing Then
            Session("sessionid") = Request.QueryString("session")
            Session("staffid") = Request.QueryString("staffhead")
            Load_Dept()
        ElseIf Request.QueryString("parent") <> Nothing Then
            Session("parentid") = Request.QueryString("parent")
            Session("sessionid") = Request.QueryString("session")
            clas_info()
        End If


    End Sub

    Private Sub Load_Dept()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle from depts where head = '" & Session("staffid") & "' order by id", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim deptshead As New ArrayList
            student0.Read()
            deptshead.Add(student0.Item(0))
            student0.Close()
            Dim classcontroled As New ArrayList
            Dim secsub As New ArrayList
            Dim mysub As New ArrayList
            For Each item As String In deptshead
                mysub = Get_subordinates(item)

                For Each subitem As String In mysub
                    secsub.Add(subitem)
                Next
            Next
            Dim thirdsub As New ArrayList
            For Each item As String In secsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                    thirdsub.Add(subitem)
                Next
            Next
            Dim fourthsub As New ArrayList
            For Each item As String In thirdsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                    fourthsub.Add(subitem)
                Next
            Next
            Dim fifthsub As New ArrayList
            For Each item As String In fourthsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub
                Next
            Next
            For Each j As String In deptshead
                If Get_ssubordinates(j) = False Then classcontroled.Add(j)
            Next
            Dim classgroups As New ArrayList
            For Each item As String In classcontroled
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
            Dim loopno As Integer = classcontroled.Count
            Dim loopcount As Integer = 0
            Dim schoolname As String = ""
            For Each item As String In classcontroled
                If Not loopcount = Request.QueryString("loop") Then
                    loopcount += 1
                    Continue For
                End If
                schoolname = item
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
                Exit For
            Next


            con.Close()
            If classcontroled.Count <> 0 Then
                Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & deptstaff.Count & "&" & teachingstaff.Count & "&" & loopno & "&" & System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(schoolname.ToLower))
            Else
                Response.Redirect("~/content/dashboards/rolecheck.aspx?&None")
            End If
        End Using

    End Sub

    Private Sub Load_Admin()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from acclogin", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim accounts As Integer = 0
            Do While student0.Read
                accounts = accounts + 1
            Loop
            student0.Close()


            Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from admin", con)
            Dim student0s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
            Dim admin As Integer = 0
            Do While student0s.Read
                admin = admin + 1
            Loop
            student0s.Close()

            student0s.Close()


            Dim cmdLoad1d As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary where session = '" & Session("sessionid") & "'", con)
            Dim student0d As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1d.ExecuteReader
            Dim student As Integer = 0
            Do While student0d.Read
                student = student + 1
            Loop
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



            Dim cmdLoadd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where activated = '" & 1 & "'", con)
            Dim studentd1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadd1.ExecuteReader
            Dim staff As Integer = 0

            Do While studentd1.Read
                staff = staff + 1
            Loop

            studentd1.Close()

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

            studentf.Close()
            Dim cmdLoad1sd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from depts where head <> '" & "" & "'", con)
            Dim student0sd As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1sd.ExecuteReader
            Dim dhs As Integer = 0
            Do While student0sd.Read
                dhs = dhs + 1
            Loop
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


            con.Close()
            Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & student & "&" & parent & "&" & admin & "&" & accounts & "&" & dhs & "&" & shs & "&" & cts & "&" & sts)

        End Using
    End Sub

    Private Sub Load_Submissions()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim timelag As String = ""
            Dim path As String = "http://" & Request.Url.Authority
            Dim msg As String = ""
            Dim pass As String = path + "/img/noimage.jpg"
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT notifications.*, studentsprofile.passport, studentsprofile.surname from notifications inner join studentsprofile on notifications.origin = studentsprofile.admno where notifications.status = '" & "Unread" & "' and notifications.recipient = '" & Session("staffid") & "' and notifications.type = '" & "assignment" & "' order by notifications.time desc", con)

            Dim student1st As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim submit As String = ""
            Dim loopno As Integer
            Do While student1st.Read
                loopno += 1
            Loop
            student1st.Close()
            Dim student1s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim loopcount As Integer = 0

            Do While student1s.Read
                If Not loopcount = Request.QueryString("loop") Then
                    loopcount += 1
                    Continue Do
                End If
                Dim dob As Date = student1s("time")
                Dim sage As TimeSpan = Now.Subtract(dob)

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

                If student1s("passport").ToString <> "" Then pass = path + Replace(student1s("passport"), "~", "")

                msg = student1s("message")

                Exit Do
            Loop
            student1s.Close()
            Dim assignments As New Literal

            assignments.Text = submit

            con.Close()
            Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & pass & "&" & msg & "&" & timelag & "&" & loopno)

        End Using

    End Sub

    Private Sub Load_Class()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()

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
            Dim clano As Integer
            Dim teacherno As Integer = 0
            Dim subjectno As Integer = 0
            Dim debtors As Integer = 0
            For Each item As Integer In clases
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.classNo  from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.class = '" & clasname(no) & "' order by studentsummary.session desc", con)
                Dim cread As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                cread.Read()
                clano = cread(0)

                cread.Close()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.staffid, staffprofile.surname, subjects.subject, staffprofile.phone, staffprofile.passport from classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where class.class = '" & clasname(no) & "' order by classsubjects.id", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim teachers As New ArrayList

                Do While reader0.Read
                    If Not teachers.Contains(reader0(0).ToString) Then
                        teachers.Add(reader0(0).ToString)
                        teacherno = teacherno + 1
                    End If
                    subjectno = subjectno + 1
                Loop


                reader0.Close()

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

                no = no + 1

            Next
            con.Close()

            Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & clano & "&" & subjectno & "&" & teacherno & "&" & debtors)

        End Using
    End Sub


    Private Sub Load_School()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim noc As Integer = 0
            Dim nos As Integer = 0
            Dim staffs As New ArrayList
            Dim debtors As Integer = 0
            Dim parents As Integer
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
            Dim deptscontrolled As New ArrayList
            Dim classcontroled As New ArrayList
            Dim secsub As New ArrayList
            Dim mysub As New ArrayList







            For Each item As String In deptshead

                mysub = Get_subordinates(item)

                For Each subitem As String In mysub

                    If Not deptscontrolled.Contains(subitem) Then
                        deptscontrolled.Add(subitem)
                        secsub.Add(subitem)
                    End If
                Next
            Next
            Dim thirdsub As New ArrayList
            For Each item As String In secsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub

                    If Not deptscontrolled.Contains(subitem) Then
                        deptscontrolled.Add(subitem)
                        thirdsub.Add(subitem)
                    End If
                Next
            Next
            Dim fourthsub As New ArrayList
            For Each item As String In thirdsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub

                    If Not deptscontrolled.Contains(subitem) Then
                        deptscontrolled.Add(subitem)
                        fourthsub.Add(subitem)
                    End If
                Next
            Next
            Dim fifthsub As New ArrayList
            For Each item As String In fourthsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub

                    If Not deptscontrolled.Contains(subitem) Then
                        deptscontrolled.Add(subitem)
                    End If
                Next
            Next

            For Each j As String In deptshead
                If Get_ssubordinates(j) = True Then classcontroled.Add(j)
            Next
            Dim loopno As Integer = classcontroled.Count
            Dim loopcount As Integer = 0
            Dim schoolname As String = ""
            For Each item As String In classcontroled
                If Not loopcount = Request.QueryString("loop") Then
                    loopcount += 1
                    Continue For
                End If
                schoolname = item
                Dim classgroups As New ArrayList
                Dim deptstaff As New ArrayList
                Dim teachingstaff As New ArrayList
                Dim clasingroup As New ArrayList

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

                    Dim classes112sk As New MySql.Data.MySqlClient.MySqlCommand("Select parentward.parent from studentsummary inner join class on class.id = studentsummary.class inner join parentward on parentward.ward = studentsummary.student where class.class = '" & clas & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                    Dim schclass112sk As MySql.Data.MySqlClient.MySqlDataReader = classes112sk.ExecuteReader
                    Dim parentarray As New ArrayList
                    Do While schclass112sk.Read
                        If Not parentarray.Contains(schclass112sk(0)) Then
                            parentarray.Add(schclass112sk(0))
                            parents += 1
                        End If
                    Loop
                    schclass112sk.Close()
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


                Dim cmdLoad1b As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary inner join (class inner join depts on depts.id = class.superior) on studentsummary.class = class.id where depts.dept = '" & item & "' and studentsummary.session = '" & Session("sessionid") & "' and studentsummary.status = '" & 0 & "'", con)
                cmdLoad1b.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
                Dim balreaderx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1b.ExecuteReader
                Dim students As New ArrayList
                Dim expected As Double = 0
                Dim realised As Double = 0
                Do While balreaderx.Read


                    debtors = debtors + 1

                Loop
                balreaderx.Close()
                incometotal = incometotal + realised
                overallincome = overallincome + incometotal

                classgroups = Nothing
                deptstaff = Nothing
                teachingstaff = Nothing
                clasingroup = Nothing
                expensetotal = Nothing
                Exit For
            Next


            con.Close()
            Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & nos & "&" & staffs.Count & "&" & parents & "&" & debtors & "&" & loopno & "&" & System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(schoolname.ToLower))

        End Using


    End Sub


    Private Sub Load_Overall()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim noc As Integer = 0
            Dim nos As Integer = 0
            Dim staffs As New ArrayList
            Dim debtors As Integer = 0
            Dim parents As Integer = 0
            Dim litschool As String = ""
            Dim overallincome As Double = 0
            Dim overallexpenses As Double = 0
            Dim overallprofit As Double = 0
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept from depts where depts.head = '" & Session("staffid") & "' order by depts.id", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim deptshead As New ArrayList
            student0.Read()

            deptshead.Add(student0.Item(0))

            student0.Close()
           

            Dim deptscontrolled As New ArrayList
            Dim classcontroled As New ArrayList
            Dim secsub As New ArrayList
            Dim mysub As New ArrayList







            For Each item As String In deptshead

                mysub = Get_subordinates(item)

                For Each subitem As String In mysub

                    If Not deptscontrolled.Contains(subitem) Then
                        deptscontrolled.Add(subitem)
                        secsub.Add(subitem)
                    End If
                Next
            Next
            Dim thirdsub As New ArrayList
            For Each item As String In secsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub

                    If Not deptscontrolled.Contains(subitem) Then
                        deptscontrolled.Add(subitem)
                        thirdsub.Add(subitem)
                    End If
                Next
            Next
            Dim fourthsub As New ArrayList
            For Each item As String In thirdsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub

                    If Not deptscontrolled.Contains(subitem) Then
                        deptscontrolled.Add(subitem)
                        fourthsub.Add(subitem)
                    End If
                Next
            Next
            Dim fifthsub As New ArrayList
            For Each item As String In fourthsub

                mysub = Get_subordinates(item)
                For Each subitem As String In mysub

                    If Not deptscontrolled.Contains(subitem) Then
                        deptscontrolled.Add(subitem)
                    End If
                Next
            Next
            For Each j As String In deptscontrolled
                If Get_ssubordinates(j) = True Then classcontroled.Add(j)
            Next
            Dim loopno As Integer = classcontroled.Count
            Dim loopcount As Integer = 0
            Dim schoolname As String = ""
            For Each item As String In classcontroled
                If Request.QueryString("overover") = "false" Then
                    If Not loopcount = Request.QueryString("loop") Then
                        loopcount += 1
                        Continue For
                    End If
                End If
                schoolname = item
                Dim classgroups As New ArrayList
                Dim deptstaff As New ArrayList
                Dim teachingstaff As New ArrayList
                Dim clasingroup As New ArrayList
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

                Dim cmdLoad1b As New MySql.Data.MySqlClient.MySqlCommand("SELECT feeschedule.student, studentsprofile.surname, feeschedule.amount, feeschedule.paid from feeschedule inner join studentsprofile on feeschedule.student = studentsprofile.admno inner join (class inner join depts on depts.id = class.superior) on feeschedule.class = class.Id where feeschedule.session = ? and depts.dept = '" & item & "' order by studentsprofile.surname", con)
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


                litschool = litschool + " <div class='dash-frame'><h4> " & item & " STATISTICS</h4></div>" & _
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
               "<div class='dash-frame'><h4>" & item & " INCOME SUMMARY</h4></div>" & _
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
                expensetotal = Nothing
                If Request.QueryString("overover") = "false" Then Exit For
            Next
            Dim profit As Double = overallincome - overallexpenses

            Dim propercentage As String = FormatNumber((profit / overallincome) * 100, 2, , TriState.True, ) & "%"

            litschool = litschool + " <div class='dash-frame'><h4>OVERALL INCOME SUMMARY</h4></div>" & _
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


            If Request.QueryString("overover") = "false" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & nos & "&" & staffs.Count & "&" & parents & "&" & debtors & "&" & loopno & "&" & System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(schoolname.ToLower))

            If Request.QueryString("overover") = "true" Then Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & FormatNumber(overallincome, , , , TriState.True) & "&" & FormatNumber(overallexpenses, , , , TriState.True) & "&" & FormatNumber(profit, , , , TriState.True) & "&" & propercentage)


            con.Close()
        End Using
    End Sub

    Private Sub Load_Subject()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim subjects As New ArrayList
            Dim classes As New ArrayList
            Dim periods As New ArrayList
            Dim range As New ArrayList
            Dim timetable As New ArrayList
            Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.alias, timetable.period, class.class, timetable.tname from timetable inner join ttname on timetable.tname = ttname.id inner join class on timetable.class = class.id inner join subjects on timetable.subject = subjects.id where timetable.teacher = '" & Session("staffid") & "' and timetable.day = '" & Now.DayOfWeek.ToString & "' and ttname.default = '" & 1 & "' order by timetable.period", con)
            Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
            Do While studen.Read
                subjects.Add(studen(0).ToString.Replace("&", "AND"))
                periods.Add(studen(1).ToString)
                classes.Add(studen(2).ToString)
                timetable.Add(studen(3).ToString)
            Loop
            studen.Close()

            Dim count As Integer = 0
            Dim lite As New Literal
            Dim rows As String = ""
            Dim loopno As Integer = periods.Count
            Dim loopcount As Integer = 0
            Dim indices As Integer
            Dim subclass As String
            Dim time As String
            For Each item As Integer In periods
                If Not loopcount = Request.QueryString("loop") Then
                    loopcount += 1
                    Continue For
                End If

                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity from tperiods inner join ttname on ttname.id = tperiods.timetable where ttname.default = '" & 1 & "' and tperiods.day = '" & Now.DayOfWeek.ToString & "'  and timetable = '" & timetable(count) & "'   order by tperiods.timestart", con)
                Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                Dim period As Integer = 0



                Do While student10.Read
                    Dim timestart As Array = Split(student10(0).ToString, ":")
                    Dim timeend As Array = Split(student10(1).ToString, ":")
                    If student10(2).ToString = "Tutorial" Then
                        period = period + 1
                    End If
                    If period = item Then

                        indices = loopcount + 1
                        subclass = subjects(count) & " - " & classes(count)
                        time = timestart(0) & ":" & timestart(1) & " - " & timeend(0) & ":" & timeend(1)

                        Exit Do
                    End If

                Loop
                student10.Close()
                count = count + 1
                Exit For
            Next
            con.Close()
            Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & indices & "&" & subclass & "&" & time & "&" & loopno)

        End Using
    End Sub

    Private Sub clas_info()
        Dim fstr As String
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim children As New ArrayList
            Dim classes As New ArrayList
            Dim admno As New ArrayList
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno, studentsummary.class, studentsprofile.surname from parentward inner join (studentsprofile left join studentsummary on studentsummary.student = studentsprofile.admno) on parentward.ward = studentsprofile.admno WHERE parentward.parent = ? order by studentsummary.session desc", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Do While student0.Read
                If Not admno.Contains(student0(0)) Then
                    admno.Add(student0(0))
                    children.Add(student0(2))
                    classes.Add(student0(1))
                End If
            Loop
            student0.Close()
            Dim no As Integer = 0

            For Each item As String In admno
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, studentsummary.classNo as 'No of Students', class.type as 'Type', depts.dept as 'Department' from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.id = '" & classes(no) & "' order by studentsummary.session desc", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("staffid")))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                Dim stu As Integer = 0
                Do While student.Read
                    stu = stu + 1
                Loop

                student.Close()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjectreg where student = '" & item & "' and session = '" & Session("sessionid") & "'", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim subjectno As Integer = 0
                Do While reader0.Read

                    subjectno = subjectno + 1
                Loop


                reader0.Close()
                Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", item))
                Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                Dim total As Integer = 0
                Dim feetype As New ArrayList
                Dim feeamount As New ArrayList
                Dim paid As Double = 0
                Dim min As Double = 0
                Do While feereader2.Read
                    feetype.Add(feereader2.Item("fee"))
                    feeamount.Add(feereader2.Item("amount"))
                    total = total + feereader2.Item("amount")
                    paid = paid + feereader2.Item("paid")
                    min = min + feereader2.Item("min")
                Loop

                feereader2.Close()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session <> ?", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", item))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Dim totalall As Integer = 0
                Dim paidall As Double = 0

                Do While feereader3.Read

                    totalall = totalall + feereader3.Item("amount")
                    paidall = paidall + feereader3.Item("paid")


                Loop
                feereader3.Close()
                Dim outstanding As Double

                outstanding = totalall - paidall

                If no = 0 Then
                    fstr = "@" & children(no) & "&" & subjectno & "&" & stu & "&"
                    Dim s As ArrayList = Daily_schedule(classes(no), item)
                    Dim fs As Integer
                    For Each t In s(0)
                        If fs = 0 Then fstr = fstr & "#" & t & "&" & s(1)(fs) & "&" & s(2)(fs)
                        If Not fs = 0 Then fstr = fstr & "=" & t & "&" & s(1)(fs) & "&" & s(2)(fs)
                        fs += 1
                    Next

                    If fs <> 0 Then fstr = fstr & "#"

                    Dim j As ArrayList = recent_submissions(classes(no), item, children(no))
                    Dim fff As Integer
                    For Each jor In j(0)
                        If fff = 0 Then fstr = fstr & ";" & jor & "&" & j(1)(fff) & "&" & j(2)(fff)
                        If Not fff = 0 Then fstr = fstr & "=" & jor & "&" & j(1)(fff) & "&" & j(2)(fff)
                        fff += 1
                    Next
                    If fff <> 0 Then fstr = fstr & ";"

                ElseIf no = 1 Then
                    fstr = fstr & "@" & children(no) & "&" & subjectno & "&" & stu & "&"
                    Dim s As ArrayList = Daily_schedule(classes(no), item)
                    Dim fs As Integer
                    For Each t In s(0)
                        If fs = 0 Then fstr = fstr & "#" & t & "&" & s(1)(fs) & "&" & s(2)(fs)
                        If Not fs = 0 Then fstr = fstr & "=" & t & "&" & s(1)(fs) & "&" & s(2)(fs)
                        fs += 1
                    Next

                    If fs <> 0 Then fstr = fstr & "#"

                    Dim j As ArrayList = recent_submissions(classes(no), item, children(no))
                    Dim fff As Integer
                    For Each jor In j(0)
                        If fff = 0 Then fstr = fstr & ";" & jor & "&" & j(1)(fff) & "&" & j(2)(fff)
                        If Not fff = 0 Then fstr = fstr & "=" & jor & "&" & j(1)(fff) & "&" & j(2)(fff)
                        fff += 1
                    Next
                    If fff <> 0 Then fstr = fstr & ";"

                ElseIf no = 2 Then
                    fstr = fstr & "@" & children(no) & "&" & subjectno & "&" & stu & "&"
                    Dim s As ArrayList = Daily_schedule(classes(no), item)
                    Dim fs As Integer
                    For Each t In s(0)
                        If fs = 0 Then fstr = fstr & "#" & t & "&" & s(1)(fs) & "&" & s(2)(fs)
                        If Not fs = 0 Then fstr = fstr & "=" & t & "&" & s(1)(fs) & "&" & s(2)(fs)
                        fs += 1
                    Next

                    If fs <> 0 Then fstr = fstr & "#"

                    Dim j As ArrayList = recent_submissions(classes(no), item, children(no))
                    Dim fff As Integer
                    For Each jor In j(0)
                        If fff = 0 Then fstr = fstr & ";" & jor & "&" & j(1)(fff) & "&" & j(2)(fff)
                        If Not fff = 0 Then fstr = fstr & "=" & jor & "&" & j(1)(fff) & "&" & j(2)(fff)
                        fff += 1
                    Next
                    If fff <> 0 Then fstr = fstr & ";"


                ElseIf no = 3 Then
                    fstr = fstr & "@" & children(no) & "&" & subjectno & "&" & stu & "&"
                    Dim s As ArrayList = Daily_schedule(classes(no), item)
                    Dim fs As Integer
                    For Each t In s(0)
                        If fs = 0 Then fstr = fstr & "#" & t & "&" & s(1)(fs) & "&" & s(2)(fs)
                        If Not fs = 0 Then fstr = fstr & "=" & t & "&" & s(1)(fs) & "&" & s(2)(fs)
                        fs += 1
                    Next

                    If fs <> 0 Then fstr = fstr & "#"

                    Dim j As ArrayList = recent_submissions(classes(no), item, children(no))
                    Dim fff As Integer
                    For Each jor In j(0)
                        If fff = 0 Then fstr = fstr & ";" & jor & "&" & j(1)(fff) & "&" & j(2)(fff)
                        If Not fff = 0 Then fstr = fstr & "=" & jor & "&" & j(1)(fff) & "&" & j(2)(fff)
                        fff += 1
                    Next
                    If fff <> 0 Then fstr = fstr & ";"

                End If
                no = no + 1
            Next

            con.Close()

        End Using


        Response.Redirect("~/content/dashboards/rolecheck.aspx?" & fstr)

    End Sub
    Private Function Daily_schedule(clas As String, student As String) As ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim subjects As New ArrayList
            Dim classes As New ArrayList
            Dim periods As New ArrayList
            Dim range As New ArrayList
            Dim timetable As Integer
            Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.alias, timetable.period, class.class, timetable.tname from timetable inner join ttname on timetable.tname = ttname.id inner join class on timetable.class = class.id inner join (subjects inner join subjectreg on subjectreg.subjectsofferred = subjects.id) on timetable.subject = subjects.id where timetable.class = '" & clas & "' and timetable.day = '" & Now.DayOfWeek.ToString & "' and ttname.default = '" & 1 & "' and subjectreg.student = '" & student & "' and subjectreg.session = '" & Session("sessionid") & "' order by timetable.period", con)
            Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
            Do While studen.Read
                subjects.Add(studen(0).ToString)
                periods.Add(studen(1).ToString)
                classes.Add(studen(2).ToString)
                timetable = studen(3).ToString
            Loop
            studen.Close()

            Dim cmdLox As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.alias, timetable.period,  timetable.tname from timetable inner join ttname on timetable.tname = ttname.id inner join classsubjects on timetable.class = classsubjects.class and timetable.subject = classsubjects.subject inner join subjects on timetable.subject = subjects.id where classsubjects.class = '" & clas & "' and timetable.day = '" & Now.DayOfWeek.ToString & "' and ttname.default = '" & 1 & "' and classsubjects.subjectnest <> '" & 0 & "'and timetable.tname = '" & timetable & "'  order by timetable.period", con)
            Dim studenx As MySql.Data.MySqlClient.MySqlDataReader = cmdLox.ExecuteReader
            Do While studenx.Read
                subjects.Add(studenx(0).ToString)
                periods.Add(studenx(1).ToString)
            Loop
            studenx.Close()
            Dim count As Integer = 0
            Dim lite As New Literal
            Dim rows As String = ""

            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity, classperiods.activity from tperiods left join classperiods on classperiods.period = tperiods.id inner join ttname on ttname.id = tperiods.timetable where ttname.default = '" & 1 & "' and tperiods.day = '" & Now.DayOfWeek.ToString & "' and timetable = '" & timetable & "' order by tperiods.timestart", con)
            Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
            Dim period As Integer = 0
            Dim all As New ArrayList
            Dim subclass As New ArrayList
            Dim time As New ArrayList
            Dim indices As New ArrayList
            Do While student10.Read
                Dim timestart As Array = Split(student10(0).ToString, ":")
                Dim timeend As Array = Split(student10(1).ToString, ":")
                If student10(2).ToString = "Tutorial" And student10(3).ToString = "" Then
                    period = period + 1
                    Dim cct As Integer = 0
                    For Each item As Integer In periods
                        If item = period Then
                            subclass.Add(subjects(cct))
                            time.Add(timestart(0) & ":" & timestart(1) & " - " & timeend(0) & ":" & timeend(1))
                            count = count + 1
                            indices.Add(count)
                        End If
                        cct = cct + 1
                    Next
                    cct = Nothing
                Else
                    If student10(3).ToString = "" Then
                        subclass.Add(student10(2).ToString)
                        time.Add(timestart(0) & ":" & timestart(1) & " - " & timeend(0) & ":" & timeend(1))
                        count = count + 1
                        indices.Add(count)

                    Else
                        subclass.Add(student10(3).ToString)
                        time.Add(timestart(0) & ":" & timestart(1) & " - " & timeend(0) & ":" & timeend(1))
                        count = count + 1
                        indices.Add(count)
                    End If


                End If
                
            Loop
            student10.Close()
          
            all.Add(indices)
            all.Add(subclass)
            all.Add(time)
            con.Close()
            Return all
        End Using
    End Function
    Private Function recent_submissions(clas As String, student As String, stuname As String) As ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim ref As New ArrayList
            Dim dates As New ArrayList
            Dim subject As New ArrayList
            Dim title As New ArrayList
            Dim deadline As New ArrayList
            Dim passports As New ArrayList
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, assignments.date, subjects.subject, assignments.title, assignments.deadline, staffprofile.passport from assignments inner join staffprofile on staffprofile.staffid = assignments.teacher inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.class = '" & clas & "'  order by assignments.date desc", con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Do While reader.Read
                ref.Add(reader.Item(0))
                dates.Add(reader.Item(1))
                subject.Add(reader.Item(2))
                title.Add(reader.Item(3))
                deadline.Add(IIf(reader.Item(4).ToString = "", "No Deadline", reader.Item(4).ToString))
                passports.Add(reader.Item(5).ToString)
            Loop
            reader.Close()
            Dim count As Integer = 0
            Dim status As New ArrayList
            Dim path As String = "https://" & Request.Url.Authority
            Dim submit As String = ""
            Dim pass As New ArrayList
            Dim msg As New ArrayList

            For Each item As Integer In ref
                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from submittedassignments where assignment = '" & item & "' and student = '" & student & "'", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                If reader2.HasRows Then
                Else
                    Dim dob As Date = dates(count)
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


                    If passports(count) <> "" Then
                        pass.Add(passports(count))
                    Else
                        pass.Add("/img/noimage.jpg")
                    End If

                    msg.Add(stuname & " has an undone assignment on " & subject(count) & " - " & title(count) & ".")
                End If
                reader2.Close()
                count = count + 1
            Next

            Dim assignments As New ArrayList

            assignments.Add(pass)
            assignments.Add(msg)
            assignments.Add(deadline)
            Return assignments
            con.Close()
        End Using
    End Function


    Private Sub Load_Accounts()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()

            Dim cmdLoad1d As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary where session = '" & Session("sessionid") & "'", con)
            Dim student0d As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1d.ExecuteReader
            Dim student As Integer = 0
            Do While student0d.Read
                student = student + 1
            Loop
            student0d.Close()

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

            o = Nothing
            balreader0.Close()
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

                balreader.Close()
                balance = Nothing
                cr = Nothing
                dr = Nothing
            Next



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
            Dim profit As Double = incometotal - expensetotal
            con.Close()
            Response.Redirect("~/content/dashboards/rolecheck.aspx?&" & FormatNumber(incometotal, , , , TriState.True) & "&" & FormatNumber(expected - realised, , , , TriState.True) & "&" & FormatNumber(expensetotal, , , , TriState.True) & "&" & FormatNumber((profit / incometotal) * 100, 2, , , ) & "%" & "&" & student & "&" & debtors)

        End Using
    End Sub

    Private Function Get_ssubordinates(ByVal dept As String) As Boolean
        Dim exists As Boolean = False
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept from depts inner join class on class.superior = depts.id where depts.dept = '" & dept & "'", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

            If student1.Read Then
                exists = True
            End If

            student1.Close()

            con.Close()

        End Using
        Return exists
    End Function

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
    Public Function Get_stubordinates(ByVal dept As String) As ArrayList
        Dim subo As New ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()

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

    Private Sub Get_roles(staffid)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim staffarray As New ArrayList
            staffarray.Add("staff")
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("select * from admin where username='" & staffid & "'", con)
            Dim dr As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
            If dr.Read = True Then staffarray.Add("admin")
            dr.Close()
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("select * from acclogin where username='" & staffid & "'", con)
            Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            If dr2.Read = True Then staffarray.Add("account")
            dr2.Close()
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("select class.type from classteacher inner join class on class.id = classteacher.class where classteacher.teacher = '" & staffid & "'", con)
            Dim dr3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
            If dr3.Read = True Then staffarray.Add("classteacher")
            dr3.Close()
            Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("select * from classsubjects where teacher='" & staffid & "'", con)
            Dim dr4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
            If dr4.Read = True Then staffarray.Add("subjectteacher")
            dr4.Close()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle, superior from depts where head = '" & staffid & "'", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim super As Boolean = False
            Dim deptshead As New ArrayList
            Do While student0.Read
                deptshead.Add(student0.Item(0))
                If student0(2) = "None" Then super = True
            Loop
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
                        teachingstaff.Add(sitem)
                    End If
                    schclass11.Close()
                Next
            Next
            For Each dept As String In deptshead
                classcontroled.Remove(dept)
            Next
            Dim subclassgroups As New ArrayList
            For Each item As String In classcontroled
                Dim f As Boolean = False
                Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                Do While schclass.Read
                    f = True
                Loop
                If f = True Then
                    subclassgroups.Add(item)
                End If
                schclass.Close()
            Next
            If teachingstaff.Count <> 0 Then staffarray.Add("depthead")
            If classgroups.Count <> 0 Then staffarray.Add("schoolhead")
            If super = True Then staffarray.Add("prop")
            Dim cmd2sd As New MySql.Data.MySqlClient.MySqlCommand("select * from lib where username='" & staffid & "'", con)
            Dim dr2sd As MySql.Data.MySqlClient.MySqlDataReader = cmd2sd.ExecuteReader
            If dr2sd.Read = True Then staffarray.Add("lib")
            dr2sd.Close()

            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("select * from parentprofile where parentid='" & staffid & "'", con)
            Dim dr As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
            If dr.Read = True Then staffarray.Add("parent")
            dr.Close()
            Session("roles") = staffarray

            con.Close()
        End Using
    End Sub
End Class
