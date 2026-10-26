Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_newterm
    Inherits System.Web.UI.Page
    Dim db As New DB_Interface
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


    Sub All_Last()
        Dim studentsub As New ArrayList
        Dim students As New ArrayList
        Dim lastscore As New ArrayList
        Dim lastscore2 As New ArrayList
        Dim nested As New ArrayList
        Dim clas As New ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim stusub As New MySql.Data.MySqlClient.MySqlCommand("Select * from subjectreg where session = ?", con)
            stusub.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session5", 22))
            Dim subjects As MySql.Data.MySqlClient.MySqlDataReader = stusub.ExecuteReader

            Do While subjects.Read
                students.Add(subjects.Item(3))
                studentsub.Add(subjects.Item(4))
                lastscore.Add(subjects("total"))
                lastscore2.Add(subjects("lastscore").ToString)
                nested.Add(subjects("nested"))
                clas.Add(subjects(2))
            Loop
            subjects.Close()
            con.Close()
        End Using

        Dim db As New DB_Interface


        Dim ins As Integer = 0
        For Each student As String In students
            Dim third As Double = db.Select_single("select total from subjectreg where student = '" & student & "' and session = '" & Session("sessionid") & "' and subjectsofferred = '" & studentsub(ins) & "'")
            Dim remarks As String = ""
            Dim grade As String = ""
            Dim count As Integer = 0
            Dim first As Double = db.Select_single("select total from subjectreg where student = '" & student & "' and session = '" & 20 & "' and subjectsofferred = '" & studentsub(ins) & "'")
            Dim second As Double = lastscore(ins)
            Dim total As Double = 0
            If first <> 0 Then
                total += first
                count += 1
            End If
            If second <> 0 Then
                total += second
                count += 1
            End If
            If third <> 0 Then
                total += third
                count += 1
            End If
            Dim avg As Double = total / count
            Dim k As Array = db.Select_Query("SELECT grades.lowest, grades.grade, grades.subject From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & clas(ins) & "' order by grades.lowest desc")
            For l = 0 To k.GetLength(1) - 2
                If avg >= k(0, l) Then
                    grade = k(1, l)
                    remarks = k(2, l)
                    Exit For
                End If
            Next
            db.Non_Query("update subjectreg set lastscore = '" & second & "', lastscore2 = '" & first & "',  cumaverage = '" & FormatNumber(avg, 2) & "', cumgrade = '" & grade & "', cumremarks = '" & remarks & "', nested = '" & nested(ins) & "' where student = '" & student & "' and session = '" & Session("sessionid") & "'  and subjectsofferred = '" & studentsub(ins) & "'")

            ins = ins + 1



        Next

    End Sub
    Sub optional_sub()
        Dim classes As ArrayList = db.Select_1D("Select id from class")
        For Each cla In classes
            Dim subjects As ArrayList = db.Select_1D("select subject from classsubjects where class = '" & cla & "' and type = 'Optional'")
            Dim students As ArrayList = db.Select_1D("select student from studentsummary where class = '" & cla & "' and session = '" & Session("sessionid") & "'")
            For Each st In students
                For Each subject In subjects
                    If db.Select_single("select count(*) from subjectreg where student = '" & st & "' and session = '" & Session("sessionid") & "' and subjectsofferred = '" & subject & "'") = 0 Then
                        db.Non_Query("insert into subjectreg (student, class, session, subjectsofferred) values ('" & st & "', '" & cla & "', '" & Session("sessionid") & "', '" & subject & "')")
                    End If
                Next
            Next
        Next
    End Sub



    Sub roll_back()
        Dim sid As Integer = db.Select_single("select id from session where session = '" & DropDownList1.Text & "' and term = '" & DropDownList2.Text & "'")
        If sid <> 0 Then
            db.Non_Query("delete from session where id = '" & sid & "'")
            db.Non_Query("delete from studentsummary where session = '" & sid & "'")
            db.Non_Query("delete from subjectreg where session = '" & sid & "'")
            db.Non_Query("delete from kscoresheet where session = '" & sid & "'")
            db.Non_Query("delete from feeschedule where session = '" & sid & "'")
            db.Non_Query("delete from transactions where session = '" & sid & "'")


        End If
    End Sub

    Sub promote_students()
        Dim classes As ArrayList = db.Select_1D("select id from class")
        For Each tisclass In classes
            Dim nextclas As Integer = db.Select_single("select nextclass from class where id = '" & tisclass & "'")
            Dim clatype As String = db.Select_single("select type from class where id = '" & nextclas & "'")
            Dim lassession As Integer = Session("last")
            Dim tissession As Integer = db.Select_single("select id from session order by id desc")

            Dim students As ArrayList = db.Select_1D("select student from studentsummary where class = '" & tisclass & "' and session = '" & lassession & "'")



            For Each st In students

                If db.Select_single("select count(*) from studentsummary where session = '" & tissession & "' and student = '" & st & "'") = 0 Then
                    Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                        con.Open()
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
                        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", st))
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        student.Read()
                        Dim dob As TimeSpan = Now.Subtract(DateTime.ParseExact(student.Item("dateofbirth"), "dd/MM/yyyy", Nothing))
                        Dim age As Integer = dob.TotalDays \ 365
                        student.Close()




                        Dim cmdSelect2ax As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
                        cmdSelect2ax.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                        Dim reader2av As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2ax.ExecuteReader
                        reader2av.Read()
                        Dim smsnos As Integer = reader2av(0)
                        reader2av.Close()
                        Dim cmdInsert2ac As New MySql.Data.MySqlClient.MySqlCommand("Update session set smsno = ? where id = ?", con)
                        cmdInsert2ac.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", smsnos + 35))
                        cmdInsert2ac.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", tissession))
                        cmdInsert2ac.ExecuteNonQuery()



                        Dim cmdLoadd As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref from transactions where account = '" & "ADVANCE FEE PAYMENT" & "' and student = '" & st & "' order by session desc", con)
                        Dim reads As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadd.ExecuteReader
                        Dim ref As Integer
                        If reads.Read() Then ref = reads.Item(0)
                        reads.Close()

                        con.Close()
                        add_traits(nextclas, st, tissession)
                        Re_register(age, tissession, st, nextclas, clatype)

                    End Using



                End If
            Next
        Next

    End Sub

    Private Sub add_traits(tisclass As Integer, st As String, tissession As Integer)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT traits.id from traits inner join class on class.traitgroup = traits.traitgroup where traits.used = '" & 1 & "' and class.id = '" & tisclass & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            Dim traits As New ArrayList
            Do While student03.Read()
                traits.Add(student03(0).ToString)
            Loop
            student03.Close()
            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("delete from termtraits where student = '" & st & "' and session = '" & tissession & "'", con)
            cmdInsert22.ExecuteNonQuery()
            For Each trait As String In traits
                Dim cmdInsert22a As New MySql.Data.MySqlClient.MySqlCommand("insert into termtraits (session, student, trait) values ('" & tissession & "', '" & st & "', '" & trait & "')", con)
                cmdInsert22a.ExecuteNonQuery()
            Next

            con.Close()
        End Using

    End Sub
    Protected Sub Re_register(age As String, tissession As Integer, st As String, cla As Integer, clatype As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()

            Dim total As Double
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classfees where class = ?", con)
            cmdLoad0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim classfee As New ArrayList

            Dim classamount As New ArrayList
            Dim min As New ArrayList
            Dim monthly As New ArrayList
            Dim quarterly As New ArrayList
            Dim classi As Integer
            Do While student0.Read
                Dim z As Integer
                classfee.Add(student0.Item(2))
                classamount.Add(student0.Item(3))
                min.Add(student0.Item("amount") * (student0.Item("min") / 100))
                monthly.Add(student0.Item(5))
                quarterly.Add(student0(6))
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
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min, monthly, quarterly, termly) Values (?,?,?,?,?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", classamount(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", monthly(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mozdfn", quarterly(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mozdsfn", classamount(classi)))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

                cmdCheck2.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(classamount(classi), , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

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
                Dim test21 As Boolean = False
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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", genamount(geni)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

                cmdCheck2.ExecuteNonQuery()


                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(genamount(geni), , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))
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

            Dim cmdload3xcz As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classonefees where class = '" & cla & "'", con)
            Dim student3xcz As MySql.Data.MySqlClient.MySqlDataReader = cmdload3xcz.ExecuteReader

            Do While student3xcz.Read

                admfee.Add(student3xcz.Item("fee"))
                admamount.Add(student3xcz.Item("amount"))
                admin.Add(student3xcz.Item("amount") * (student3xcz.Item("min") / 100))
            Loop
            student3xcz.Close()


            Dim hostel As String
            Dim transport As String
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT admfees, hostelstay, transport from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", st))

            Dim studentadm As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            studentadm.Read()
            hostel = studentadm.Item(1)
            transport = studentadm.Item(2)
            Dim paid As String = studentadm.Item(0)
            studentadm.Close()
            If paid = "Not Paid" Then
                For Each item As String In admfee
                    Dim test22 As Boolean = False
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
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", admamount(admi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
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
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))
                    cmdCheck2.ExecuteNonQuery()

                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(admamount(admi), , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))
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
                    Dim test23 As Boolean = False
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
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", sesfee(sesi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount(sesi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
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
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

                    cmdCheck2.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount(sesi), , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

                    cmdCheck4.ExecuteNonQuery()
                    sesi = sesi + 1
                Next
            End If
            If hostel = True Then

                Dim test24 As Boolean = False
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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

                cmdCheck2.ExecuteNonQuery()
                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

                cmdCheck4.ExecuteNonQuery()

            End If
            If transport <> "" Then
                Dim test25 As Boolean = False
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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

                cmdCheck2.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))

                cmdCheck4.ExecuteNonQuery()
            End If
            add_discount(st, tissession)



            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO StudentSummary (Session, Class, student, age) Values (?,?,?,?)", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("age", age))

            cmdInsert2.ExecuteNonQuery()


            If clatype <> "K.G 1 Special" Then
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)

                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM ClassSubjects WHERE Type= ? And class = ? and subjectnest = '" & 0 & "'", con)
                Dim type As String = "Compulsory"
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", st))
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
                    Dim paramc As New MySql.Data.MySqlClient.MySqlParameter

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
                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", st))
                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", item))
                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                    cmdds.ExecuteNonQuery()

                Next
            Else
                Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, subject FROM kcourseoutline where session = '" & tissession & "' and class = '" & cla & "'", con)
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                Dim topics As New ArrayList
                Dim subjectss As New ArrayList
                Do While reader20.Read()
                    topics.Add(reader20(0))
                    subjectss.Add(reader20(1))
                Loop
                reader20.Close()
                Dim llt As Integer
                For Each topic As Integer In topics
                    Dim cmdceck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into kscoresheet (session, class, subject, topic, student) Values (?,?,?,?,?)", con)
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", tissession))
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", cla))
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", subjectss(llt)))
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", topic))
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", st))
                    cmdceck2.ExecuteNonQuery()
                    llt = llt + 1
                Next
            End If



            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? and student = ? order by date desc", con)
            cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "ADVANCE FEE PAYMENT"))
            cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            Dim fbalance As Double = cr - dr
            balance = (cr - dr)
            balreader.Close()

            cr = Nothing
            dr = Nothing
            Dim init As Double = balance
            If balance > 0 Then
                Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
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
                cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))

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
                    Dim test2 As Boolean = False
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
                    If Session("amount" & k) <> Nothing Then
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule Set paid = ? where fee = ? and student = ? and session = ?", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", Session("amount" & k)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", Session("fee" & k)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("ses" & k)))
                        cmdInsert22.ExecuteNonQuery()

                        Dim cmdCheck03 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " DEBTS"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))
                        cmdCheck03.ExecuteNonQuery()

                        Dim cmdCheck02 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " PAID"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))
                        cmdCheck02.ExecuteNonQuery()
                        Session("fee" & k) = Nothing
                        Session("amount" & k) = Nothing
                        Session("paid" & k) = Nothing
                        d = Nothing
                    End If
                Next

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", tissession))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                Dim feereader As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
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
                    cmdInsert29.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    cmdInsert29.ExecuteNonQuery()


                End If
                Dim test23 As Boolean = False
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
                Dim diff As Double = fbalance - balance
                Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(init - balance, , , TriState.True)))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & st))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck.ExecuteNonQuery()

                Dim cmdCheck6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(init - balance, , , TriState.True)))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by student." & st))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                cmdCheck6.ExecuteNonQuery()
                If diff > 0 Then
                    Dim test24 As Boolean = False
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
                    Dim advance As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Lability"))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(diff, , , TriState.True)))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ADVANCE FEE PAYMENT"))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & st))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    advance.ExecuteNonQuery()

                    Dim advance6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(diff, , , TriState.True)))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by student." & st))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    advance6.ExecuteNonQuery()
                ElseIf diff < 0 Then
                    Dim testr3r3 As Boolean
                    Dim fr3 As New Random
                    Dim refr3r3 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readrefr3r3 As MySql.Data.MySqlClient.MySqlDataReader = refr3r3.ExecuteReader

                    Dim refsr3r3 As New ArrayList
                    Do While readrefr3r3.Read
                        refsr3r3.Add(readrefr3r3.Item(0))
                    Loop
                    Dim dr3 As Integer
                    Do Until testr3r3 = True
                        dr3 = fr3.Next(100000, 999999)
                        If refsr3r3.Contains(dr3) Then
                            testr3r3 = False
                        Else
                            testr3r3 = True
                        End If
                    Loop
                    readrefr3r3.Close()
                    Dim cmdccheck6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", dr3))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Liability"))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(0 - diff, , , TriState.True)))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ADVANCE FEE PAYMENT"))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid in advance." & Request.QueryString.ToString))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    cmdccheck6.ExecuteNonQuery()

                    Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", dr3))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(0 - diff, , , TriState.True)))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & Request.QueryString.ToString))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", st))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", tissession))
                    cmdCheck5.ExecuteNonQuery()

                End If
            End If

            Average_Age(cla, tissession)
            con.Close()
        End Using




    End Sub



    Sub add_discount(st As String, tissession As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()


            Dim currentsession As String
            Dim currentterm As String

            Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where id = '" & tissession & "' Order by ID Desc", con)
            Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
            reader2f.Read()
            currentsession = reader2f(0).ToString
            currentterm = reader2f(2).ToString
            reader2f.Close()

            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from discount where student = ? and (recurring = '" & -Val(True) & "' or session = '" & currentsession & "')", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
            Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
            Dim discountfee As New ArrayList
            Dim discountamt As New ArrayList
            Dim discounttype As New ArrayList
            Do While feereader3.Read()
                discountfee.Add(feereader3(2))
                discountamt.Add(feereader3(4).ToString)
                discounttype.Add(feereader3(3))
            Loop
            feereader3.Close()
            Dim ct As Integer

            For Each item As String In discountfee
                Dim cmdInsertx As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ? and fee = ?", con)
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", item))
                Dim feereader3x As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertx.ExecuteReader
                feereader3x.Read()
                Dim fee As Integer = feereader3x.Item(2)
                Dim min As Integer = feereader3x.Item("min")
                Dim month As Integer = feereader3x.Item("monthly")
                Dim quarters As Integer = feereader3x.Item("quarterly")
                feereader3x.Close()
                If discounttype(ct) = "Fixed" Then
                    fee = fee - discountamt(ct)
                    min = min - discountamt(ct)
                Else
                    fee = fee - (fee * (discountamt(ct) / 100))
                    min = min - (min * (discountamt(ct) / 100))

                End If

                Dim cmdInsert225 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = '" & fee & "', min = '" & IIf(min < 0, 0, min) & "' where session = ? and fee = ? and student = ?", con)

                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsert225.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & "%"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                readref220.Read()
                Dim refe As Integer = readref220.Item(0).ToString
                readref220.Close()
                Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set cr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & item & " DEBTS" & "'", con)
                cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck04.ExecuteNonQuery()

                Dim cmdCheck041 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set dr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & "DEBTORS" & "'", con)
                cmdCheck041.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck041.ExecuteNonQuery()

                Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                Dim totals As Integer = 0

                Do While feereader2.Read
                    totals = totals + feereader2.Item("amount")
                Loop
                feereader2.Close()
                If totals = 0 Then
                    Dim cmdInsertcv As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? and session = '" & currentsession & "'", con)
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                    cmdInsertcv.ExecuteNonQuery()
                End If
                ct = ct + 1
            Next
            con.Close()
        End Using
    End Sub



    Sub readd_subjects()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
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
            con.Close()        End Using
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
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
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
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
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub Adjust_Attendance()

        Dim db As New DB_Interface
        Dim opendate As Date = DateTime.ParseExact(db.Select_single("Select opendate from session where ID = '" & Session("SessionId") & "'").ToString, "dd/MM/yyyy", Nothing)
        Dim classes As ArrayList = db.Select_1D("select id from class")
        For Each c In classes

            If db.Select_single("Select count(*) from attendance where term = '" & Session("SessionId") & "' and class = '" & c & "' order by STR_TO_DATE(date,'%W, %M %d, %Y')") <> 0 Then
                Dim old As Boolean
                Dim any As Boolean = False
                Do
                    Dim thisdate As Date = db.Select_single("Select date, week from attendance where term = '" & Session("SessionId") & "' and class = '" & c & "' order by STR_TO_DATE(date,'%W, %M %d, %Y')")

                    If thisdate < opendate Then
                        old = True
                        any = True
                        db.Non_Query("delete from attendance where term = '" & Session("sessionid") & "' and class = '" & c & "' and date = '" & thisdate.ToLongDateString & "'")
                    Else
                        old = False
                    End If


                Loop While old = True
                If any = True Then Adjust_Weeks(c)
            End If

        Next

    End Sub
    Sub Adjust_Weeks(c As Integer)
        Dim db As New DB_Interface
        Dim week As Integer = 0
        Dim first As Boolean = True
        Dim days As New ArrayList
        Dim alldays As ArrayList = db.Select_1D("Select date from attendance where term = '" & Session("SessionId") & "' and class = '" & c & "' order by STR_TO_DATE(date,'%W, %M %d, %Y')")
        For Each j In alldays
            If Not days.Contains(j) Then days.Add(j)
        Next
        For Each d As Date In days
            If first = True Or d.DayOfWeek = DayOfWeek.Monday Then week += 1
            db.Non_Query("update attendance set week = '" & week & "' where date = '" & d.ToLongDateString & "'")
            first = False
        Next
    End Sub

    Sub Update_All_Attendance()
        Dim db As New DB_Interface
        Dim classes As ArrayList = db.Select_1D("select id from class")
        For Each c In classes
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim ref30 As New MySql.Data.MySqlClient.MySqlCommand("Select studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno where studentsummary.class = '" & c & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                Dim readref30 As MySql.Data.MySqlClient.MySqlDataReader = ref30.ExecuteReader
                Dim students As New ArrayList
                Do While readref30.Read
                    students.Add(readref30.Item(0))
                Loop
                readref30.Close()
                For Each student As String In students
                    Dim present As Integer = 0
                    Dim absent As Integer = 0
                    Dim opened As Integer = 0
                    Dim early As Integer = 0
                    Dim late As Integer = 0
                    Dim atall As Integer = 0
                    Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select morning, afternoon, punctual from attendance where term = '" & Session("SessionId") & "' and student = '" & student & "' order by id desc", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                    Do While readref.Read
                        present = present - Val(readref.Item(0)) - Val(readref.Item(1))
                        early = early - Val(readref.Item(2))
                        If -Val(readref.Item(0)) = 1 Or -Val(readref.Item(1)) = 1 Then atall = atall + 1
                    Loop
                    readref.Close()
                    Dim refs As New MySql.Data.MySqlClient.MySqlCommand("Select date from attendance where term = '" & Session("SessionId") & "' and class = '" & c & "' and holiday = '" & -Val(False) & "'", con)
                    Dim readrefs As MySql.Data.MySqlClient.MySqlDataReader = refs.ExecuteReader
                    Dim dates As New ArrayList
                    Do While readrefs.Read
                        If Not dates.Contains(readrefs.Item(0)) Then
                            dates.Add(readrefs.Item(0))
                        End If
                    Loop
                    readrefs.Close()

                    opened = dates.Count * 2
                    absent = opened - present
                    late = atall - early

                    Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsummary set present = '" & present & "', absent = '" & absent & "', late = '" & late & "', atall = '" & atall & "' where student = '" & student & "' and session = '" & Session("sessionid") & "'", con)
                    ref3.ExecuteNonQuery()
                Next
                con.Close()
            End Using
        Next
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

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Session("Updateterm") <> Nothing Then
                    Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update Session set session = ?, term = ?, closingdate = ?, nextterm = ?, opendate = ? where ID = ?", con)
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session2", DropDownList1.Text))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term2", DropDownList2.Text))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", closing))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("nextterm", resumption))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("open", opening))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("Updateterm")))
                    cmdInsert2.ExecuteNonQuery()
                    Adjust_Attendance()
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
                        Session("last") = Session("SessionID")
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

                        Else
                            con.Close()
                            promote_students()
                            con.Open()
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
                con.Close()            End Using
        Catch ex As Exception
            roll_back()
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Sub payment_schedules(student As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim cmdloadfs As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class,  studentsummary.class, class.type From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdloadfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", student))
            cmdloadfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("sessionid")))
            Dim studentsreadsse As MySql.Data.MySqlClient.MySqlDataReader = cmdloadfs.ExecuteReader()
            studentsreadsse.Read()
            Dim classtype As String
            classtype = studentsreadsse.Item(3)
            studentsreadsse.Close()
            If classtype <> "Early Years" Then
                Session("scheduletype") = "Termly"
            Else

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? order by id", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Dim totalall As Integer = 0
                Dim paidall As Double = 0

                Do While feereader3.Read
                    If feereader3("payment").ToString <> "" And Session("scheduletype") = Nothing Then Session("scheduletype") = feereader3("payment").ToString
                    totalall = totalall + feereader3.Item("amount")
                    paidall = paidall + feereader3.Item("paid")
                Loop
                feereader3.Close()
            End If
            Dim currentsession As String
            Dim currentterm As String

            Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where id = '" & Session("sessionid") & "' Order by ID Desc", con)
            Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
            reader2f.Read()
            currentsession = reader2f(0).ToString
            currentterm = reader2f(2).ToString
            reader2f.Close()

            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", student))
            Dim feereader As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert22.ExecuteReader()
            Dim monthly As New ArrayList
            Dim amount As New ArrayList
            Dim termly As New ArrayList
            Dim quarterly As New ArrayList
            Dim id As New ArrayList
            Dim fee As New ArrayList
            Do While feereader.Read
                id.Add(feereader("id"))
                monthly.Add(feereader("monthly"))
                amount.Add(feereader("amount"))
                termly.Add(feereader("termly"))
                fee.Add(feereader("fee"))
                quarterly.Add(feereader("quarterly"))
            Loop
            feereader.Close()

            If Session("scheduletype") = "Monthly" Then
                Dim x As Integer
                For Each s As Integer In id
                    If Val(monthly(x)) <> 0 Then
                        Dim cmdInsertf5 As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set amount = '" & IIf(amount(x) <> 0, Val(monthly(x)) * 4, 0) & "' where id = '" & s & "'", con)
                        cmdInsertf5.ExecuteNonQuery()

                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", fee(x) & " DEBTS"))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                        Dim refe As Integer
                        If readref220.Read() Then
                            refe = readref220.Item(0).ToString
                        End If
                        readref220.Close()
                        Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                        cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck04.ExecuteNonQuery()

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

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(IIf(amount(x) <> 0, Val(monthly(x)) * 4, 0), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", fee(x) & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(IIf(amount(x) <> 0, Val(monthly(x)) * 4, 0), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()

                    End If
                    x += 1
                Next
                Session("scheduletype") = "Monthly"
            ElseIf Session("scheduletype") = "Termly" Then
                Dim x As Integer
                For Each s As Integer In id
                    If Val(monthly(x)) <> 0 Then

                        Dim cmdInsertf5 As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set amount = '" & IIf(amount(x) <> 0, Val(termly(x)), 0) & "' where id = '" & s & "'", con)
                        cmdInsertf5.ExecuteNonQuery()

                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", fee(x) & " DEBTS"))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                        Dim refe As Integer
                        If readref220.Read() Then
                            refe = readref220.Item(0).ToString
                        End If
                        readref220.Close()
                        Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                        cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck04.ExecuteNonQuery()

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

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(IIf(amount(x) <> 0, Val(termly(x)), 0), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", fee(x) & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(IIf(amount(x) <> 0, Val(termly(x)), 0), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()
                    End If
                    x += 1
                Next
                Session("scheduletype") = "Termly"

            ElseIf Session("scheduletype") = "Quarterly" Then
                Dim x As Integer
                For Each s As Integer In id
                    If Val(quarterly(x)) <> 0 Then
                        Dim cmdInsertf5 As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set amount = '" & IIf(amount(x) <> 0 And currentterm = "2nd term", Val(quarterly(x)) * 2, IIf(amount(x) <> 0 And currentterm <> "2nd term", Val(quarterly(x)), 0)) & "' where id = '" & s & "'", con)
                        cmdInsertf5.ExecuteNonQuery()

                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", fee(x) & " DEBTS"))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                        Dim refe As Integer
                        If readref220.Read() Then
                            refe = readref220.Item(0).ToString
                        End If
                        readref220.Close()
                        Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                        cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck04.ExecuteNonQuery()

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

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(IIf(amount(x) <> 0 And Session("Term") = "2nd term", Val(quarterly(x)) * 2, IIf(amount(x) <> 0 And Session("Term") <> "2nd term", Val(quarterly(x)), 0)), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", fee(x) & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(IIf(amount(x) <> 0 And Session("Term") = "2nd term", Val(quarterly(x)) * 2, IIf(amount(x) <> 0 And Session("Term") <> "2nd term", Val(quarterly(x)), 0)), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()
                    End If
                    x += 1
                Next
                Session("scheduletype") = "Quarterly"



            End If
            add_discount(student)
            con.Close()
        End Using
    End Sub

    Sub add_discount(student As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdInsertv As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? order by id", con)
            cmdInsertv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
            Dim feereader3v As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertv.ExecuteReader
            Dim totalall As Integer = 0
            Dim paidall As Double = 0

            Do While feereader3v.Read
                If feereader3v("payment").ToString <> "" And Session("scheduletype") = Nothing Then Session("scheduletype") = feereader3v("payment").ToString
                totalall = totalall + feereader3v.Item("amount")
                paidall = paidall + feereader3v.Item("paid")
            Loop
            feereader3v.Close()

            Dim currentsession As String
            Dim currentterm As String

            Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where id = '" & Session("sessionid") & "' Order by ID Desc", con)
            Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
            reader2f.Read()
            currentsession = reader2f(0).ToString
            currentterm = reader2f(2).ToString
            reader2f.Close()

            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from discount where student = ? and recurring = '" & -Val(True) & "' or session = '" & currentsession & "'", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
            Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
            Dim discountfee As New ArrayList
            Dim discountamt As New ArrayList
            Dim discounttype As New ArrayList
            Do While feereader3.Read()
                discountfee.Add(feereader3(2))
                discountamt.Add(feereader3(4))
                discounttype.Add(feereader3(3))
            Loop
            feereader3.Close()
            Dim ct As Integer
            For Each item As String In discountfee
                Dim cmdInsertx As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ? and fee = ?", con)
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", item))
                Dim feereader3x As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertx.ExecuteReader
                feereader3x.Read()
                Dim fee As Integer = feereader3x.Item(2)
                Dim min As Integer = feereader3x.Item("min")
                Dim month As Integer = feereader3x.Item("monthly")
                Dim quarters As Integer = feereader3x.Item("quarterly")
                feereader3x.Close()
                If discounttype(ct) <> "Months" Then
                    If discounttype(ct) = "Fixed" Then
                        fee = fee - discountamt(ct)
                        min = min - discountamt(ct)
                    ElseIf discounttype(ct) = "Percentage" Then
                        fee = fee - (IIf(quarters <> 0 And currentterm = "2nd term", quarters / 2, quarters) * (discountamt(ct).Replace(",", "") / 100))
                        min = min - (IIf(quarters <> 0 And currentterm = "2nd term", quarters / 2, quarters) * (discountamt(ct).Replace(",", "") / 100))
                    End If
                Else
                    fee = fee - (discountamt(ct) * month)
                    min = fee
                End If

                Dim cmdInsert225 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = '" & fee & "', min = '" & IIf(min < 0, 0, min) & "' where session = ? and fee = ? and student = ?", con)
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdInsert225.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & "%"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                readref220.Read()
                Dim refe As Integer = readref220.Item(0).ToString
                readref220.Close()
                Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set cr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & item & " DEBTS" & "'", con)
                cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck04.ExecuteNonQuery()

                Dim cmdCheck041 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set dr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & "DEBTORS" & "'", con)
                cmdCheck041.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck041.ExecuteNonQuery()

                Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                Dim totals As Integer = 0

                Do While feereader2.Read
                    totals = totals + feereader2.Item("amount")
                Loop
                feereader2.Close()
                If totals = 0 Then
                    Dim cmdInsertcv As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? and session = '" & currentsession & "'", con)
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    cmdInsertcv.ExecuteNonQuery()
                End If
                ct = ct + 1
            Next
            con.Close()
        End Using
    End Sub
    Private Sub fees(cla As Integer, student As String, term As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim total As Double

            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classfees where class = ?", con)
            cmdLoad0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim classfee As New ArrayList
            Dim quarterly As New ArrayList
            Dim classamount As New ArrayList
            Dim min As New ArrayList
            Dim monthly As New ArrayList
            Dim classi As Integer
            Do While student0.Read
                classfee.Add(student0.Item(2))
                classamount.Add(student0.Item(3))
                min.Add(student0.Item("amount") * (student0.Item("min") / 100))
                monthly.Add(student0.Item(5))
                quarterly.Add(student0.Item(6))
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
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min, monthly, quarterly, termly) Values (?,?,?,?,?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", term))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", classamount(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", monthly(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("msdson", quarterly(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aasfwqfwmt", classamount(classi)))
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

            payment_schedules(student)
            add_discount(student)
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
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
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
                Dim open As String = reader2.Item(7).ToString
                Dim close As String = reader2.Item(5).ToString
                Dim nxt As String = reader2.Item(4).ToString
                txtOpen.Text = open
                txtClose.Text = close
                txtNext.Text = nxt
                reader2.Close()
                Button1.Text = "Update"


                con.Close()            End Using
        Catch ex As Exception

            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
