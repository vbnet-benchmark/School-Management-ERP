Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Staff_mngclass
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList

    Protected Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblError.Text = ""
        lblSuccess.Text = ""
        If Not IsPostBack Then

            If Session("ClassID") Is Nothing Then
                Session("ClassID") = Session("AddClass")
            End If

            Label1.Text = Session("classname").ToString
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Staffprofile INNER JOIN classteacher ON Staffprofile.staffid = classteacher.teacher WHERE classteacher.class = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("ClassID")))
                Dim studentsReader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader()
                BulletedList2.BulletStyle = BulletStyle.Numbered
                Do While studentsReader1.Read
                    BulletedList2.Items.Add(studentsReader1.Item(1) & " " & studentsReader1.Item(3) & " " & studentsReader1.Item(4) & " - " & studentsReader1.Item(0))
                Loop
                studentsReader1.Close()

                Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname, subjects.subject From Staffprofile INNER JOIN subjectteacher ON Staffprofile.staffid = subjectteacher.teacher Inner Join subjects on subjects.ID = subjectteacher.subject WHERE subjectteacher.class = ?", con)
                cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("ClassID")))
                Dim studentsReader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader()
                BulletedList3.BulletStyle = BulletStyle.Numbered
                Do While studentsReader2.Read
                    BulletedList3.Items.Add(studentsReader2.Item(0) & " " & studentsReader2.Item(1))
                Loop
                studentsReader2.Close()
                con.close()            End Using
            Load_students()
        End If
    End Sub

    Protected Sub Load_students()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From StudentsProfile INNER JOIN StudentSummary ON StudentsProfile.admno = StudentSummary.student WHERE StudentSummary.Class = ? And StudentSummary.Session = ? ORDER BY StudentsProfile.surname", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("ClassID")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            BulletedList1.BulletStyle = BulletStyle.Numbered
            BulletedList1.Items.Clear()
            Do While studentsReader.Read
                BulletedList1.Items.Add(studentsReader.Item(1) & " - " & studentsReader.Item(0))
            Loop
            studentsReader.Close()

            con.close()        End Using
    End Sub


    Protected Sub Button1_Click1(sender As Object, e As EventArgs) Handles Button1.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim discount As String
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", TextBox1.Text))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            If Not student.Read() Then
                lblError.Text = "Admission number invalid"
                student.Close()

            Else
                discount = student.Item("discount")
                Dim dob As TimeSpan = Now.Subtract(student.Item("dateofbirth"))
                Dim age As Integer = dob.TotalDays \ 365
                student.Close()
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

                Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                reader2.Read()
                If reader2.HasRows Then
                    reader2.Close()
                    lblError.Text = "Student previously registerred in a class"
                    Exit Sub
                Else
                    Dim total As Double

                    reader2.Close()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classfees where class = ?", con)
                    cmdLoad0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("ClassID")))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Dim classfee As New ArrayList
                    Dim nondiscounted As Integer

                    Dim classamount As New ArrayList
                    Dim min As New ArrayList
                    Dim classi As Integer
                    Do While student0.Read
                        Dim z As Integer
                        If student0.Item("discountable") = "discountable" Then
                            classfee.Add(student0.Item(1))
                            nondiscounted = student0.Item(2)
                            Dim discounted As Double = nondiscounted * (discount / 100)
                            discounted = nondiscounted - discounted

                            classamount.Add(discounted)
                            min.Add(discounted * (student0.Item("min") / 100))
                        Else
                            classfee.Add(student0.Item(1))
                            classamount.Add(student0.Item(2))
                            min.Add(student0.Item("amount") * (student0.Item("min") / 100))
                        End If
                    Loop
                    student0.Close()
                    For Each item As String In classfee
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory,min) Values (?,?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", classamount(classi)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                        cmdInsert22.ExecuteNonQuery()
                        total = total + classamount(classi)


                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(classamount(classi), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdCheck2.ExecuteNonQuery()
                        classi = classi + 1
                    Next

                    Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from optionalfees", con)
                    Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader
                    Dim genfee As New ArrayList
                    Dim genamount As New ArrayList
                    Dim geni As Integer
                    Dim genmin As New ArrayList
                    Do While student2.Read
                        genfee.Add(student2.Item(0))
                        genamount.Add(student2.Item(1))
                        genmin.Add(student2.Item("amount") * (student2.Item("min") / 100))
                    Loop
                    student2.Close()
                    For Each item As String In genfee
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", genamount(geni)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", genmin(geni)))
                        cmdInsert22.ExecuteNonQuery()
                        total = total + genamount(geni)

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(genamount(geni), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdCheck2.ExecuteNonQuery()

                        geni = geni + 1
                    Next

                    Dim cmdLoad3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
                    Dim student3 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad3.ExecuteReader
                    Dim admfee As New ArrayList
                    Dim admamount As New ArrayList
                    Dim admin As New ArrayList
                    Dim admi As Integer
                    Do While student3.Read

                        admfee.Add(student3.Item(0))
                        admamount.Add(student3.Item(1))
                        admin.Add(student3.Item("amount") * (student3.Item("min") / 100))
                    Loop
                    student3.Close()
                    Dim hostel As String
                    Dim transport As String
                    Dim feeding As String
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT admfees, hostelstay, transport, feeding from StudentsProfile where admno = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", TextBox1.Text))

                    Dim studentadm As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    studentadm.Read()
                    hostel = studentadm.Item(1)
                    transport = studentadm.Item(2)
                    feeding = studentadm.Item(3)
                    Dim paid As String = studentadm.Item(0)
                    studentadm.Close()
                    If paid = "Not Paid" Then
                        For Each item As String In admfee
                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", admamount(admi)))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", admin(admi)))
                            cmdInsert22.ExecuteNonQuery()
                            total = total + admamount(admi)

                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(admamount(admi), , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck2.ExecuteNonQuery()
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

                            sesfee.Add(student24.Item(0))
                            sesamount.Add(student24.Item(1))
                            sesmin.Add(student24.Item("amount") * (student24.Item("min") / 100))
                        Loop
                        student24.Close()
                        For Each item As String In sesfee
                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", sesamount(sesi)))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin(sesi)))
                            cmdInsert22.ExecuteNonQuery()
                            total = total + sesamount(sesi)


                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", sesamount(sesi) & " DEBTS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck2.ExecuteNonQuery()

                            sesi = sesi + 1
                        Next
                    End If
                    If hostel = True Then
                        Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                        Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                        student24.Read()

                        Dim sesamount As String = student24.Item(0)
                        student24.Close()
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesamount))
                        cmdInsert22.ExecuteNonQuery()
                        total = total + sesamount

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BOARDING DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdCheck2.ExecuteNonQuery()


                    End If
                    If transport <> "None" Then
                        Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees where route = ?", con)
                        cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", transport))
                        Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                        student24.Read()
                        Dim sesfee As String = student24.Item(0)
                        Dim sesamount As String = student24.Item(1)

                        student24.Close()
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesamount))
                        cmdInsert22.ExecuteNonQuery()
                        total = total + sesamount

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "TRANSPORT DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdCheck2.ExecuteNonQuery()
                    End If
                    If feeding <> "No" Then
                        Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from feeding", con)
                        Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                        student24.Read()
                        Dim sesamount As String = student24.Item(0)

                        student24.Close()
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "FEEDING"))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesamount))
                        cmdInsert22.ExecuteNonQuery()


                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "FEEDING DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdCheck2.ExecuteNonQuery()
                        total = total + sesamount
                    End If




                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(total, , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                    cmdCheck4.ExecuteNonQuery()


                    Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO StudentSummary (Session, Class, student, age, trans) Values (?,?,?,?,?)", con)
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("age", age))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trans", d))

                    cmdInsert2.ExecuteNonQuery()

                    Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? and student = ? order by date desc", con)
                    cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "ADVANCE FEE PAYMENT"))
                    cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
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
                        cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
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
                        cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))

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
                            If Session("amount" & k) <> Nothing Then
                                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule Set paid = ? where fee = ? and student = ? and session = ?", con)
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", Session("amount" & k)))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", Session("fee" & k)))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("ses" & k)))
                                cmdInsert22.ExecuteNonQuery()

                                Dim cmdCheck03 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                                cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                                cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                                cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                                cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " DEBTS"))
                                cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                                cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                                cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                                cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdCheck03.ExecuteNonQuery()

                                Dim cmdCheck02 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                                cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                                cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                                cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                                cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " PAID"))
                                cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                                cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                                cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                                cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdCheck02.ExecuteNonQuery()
                                MsgBox(k & Session("fee" & k) & " " & Session("amount" & k))
                                Session("fee" & k) = Nothing
                                Session("amount" & k) = Nothing
                                Session("paid" & k) = Nothing
                            End If
                        Next
                        MsgBox("Overpaid" & Session("over"))

                        Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
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
                            cmdInsert29.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdInsert29.ExecuteNonQuery()
                        End If
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(init - balance, , , TriState.True)))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & TextBox1.Text))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdCheck.ExecuteNonQuery()

                        Dim cmdCheck6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Liability"))
                        cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(init - balance, , , TriState.True)))
                        cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ADVANCE FEE PAYMENT"))
                        cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid in advance." & TextBox1.Text))
                        cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date.ToShortDateString))
                        cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdCheck6.ExecuteNonQuery()

                    End If
                End If

                If Session("term") <> "1st term" Then
                    Dim lastterm As Integer
                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    For i As Integer = 1 To 2
                        If reader20.Read() Then
                            If reader20.Item(0) <> Session("SessionID") Then
                                lastterm = reader20.Item(0)
                                i = i + 1
                            End If
                        End If
                    Next
                    reader20.Close()
                    If lastterm = 0 Then
                        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)
                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT* FROM ClassSubjects WHERE Type= ? And class = ?", con)
                        Dim type As String = "Compulsory"
                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Type", type))
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("ClassID")))
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
                    Else
                        Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Subjectsofferred FROM SubjectReg WHERE SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ?", con)
                        cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", TextBox1.Text))
                        cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", lastterm))
                        cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("ClassID").ToString))
                        Dim reader21 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                        Dim subjectsofferred As New ArrayList
                        Do While reader21.Read
                            subjectsofferred.Add(reader21.Item(0))
                        Loop
                        reader21.Close()
                        If subjectsofferred.Count = 0 Then
                            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)
                            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT* FROM ClassSubjects WHERE Type= ? And class = ?", con)
                            Dim type As String = "Compulsory"
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))
                            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Type", type))
                            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("ClassID")))
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
                        Else
                            For Each item As Integer In subjectsofferred
                                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)
                                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))
                                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjects", item))
                                cmd.ExecuteNonQuery()
                            Next
                        End If
                    End If
                End If
                Average_Age()
                lblSuccess.Text = "Student successfully registerred in class"

            End If
            con.close()        End Using
        Load_students()

    End Sub

    Protected Sub Average_Age()
        Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT age FROM StudentSummary WHERE class = ?", con)
        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("ClassID")))
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
        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("ClassId")))
        cmdInsert2.ExecuteNonQuery()
    End Sub

    Protected Sub Button2_Click1(sender As Object, e As EventArgs) Handles Button2.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", TextBox1.Text))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            If Not student.Read() Then
                lblError.Text = "Admission number invalid"
                student.Close()
                con.close()end using
        Else
        student.Close()
        Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM StudentSummary WHERE student = ? And Session = ?", con)
        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
        cmdSelect2.ExecuteNonQuery()
        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("Delete from SubjectReg WHERE student = ? And Session = ?", con)
        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
        cmd.ExecuteNonQuery()
        Average_Age()
        lblSuccess.Text = "Student successfully deleted"
        con.close()end using
        Load_students()
        End If

    End Sub
End Class
