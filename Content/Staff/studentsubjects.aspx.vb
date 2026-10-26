Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_Default
    Inherits System.Web.UI.Page
    Dim optionalfeeb As String
    Dim optionalfeef As String
    Dim optionalfeet As String
    Dim optionalfeec As New ArrayList
    Dim optionalfeenc As New ArrayList
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList
    Dim hostel As Boolean
    Dim transport As String
    Dim feeding As String
    Dim minimum As Double
    Dim i As Integer

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
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
    End Sub
    Private Sub update_class(cla As Integer)

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()

                Dim subjects As New ArrayList
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subject FROM ClassSubjects WHERE  class = '" & cla & "'", con)
                Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Do While reader2a.Read
                    subjects.Add(reader2a(0))
                Loop
                reader2a.Close()
                For Each subject As Integer In subjects
                    Dim Saverage As New MySql.Data.MySqlClient.MySqlCommand("SELECT Total, student FROM SubjectReg WHERE Session = ? AND Class = ? AND subjectsOfferred = ? Order by total Desc", con)
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjectsOffered", subject))
                    Dim readerT As MySql.Data.MySqlClient.MySqlDataReader = Saverage.ExecuteReader
                    Dim STotal As Double = 0
                    Dim SubjectAverage As Double = 0
                    Dim positionz As String = ""
                    Dim pos As New ArrayList
                    Dim posid As New ArrayList
                    Dim count As Integer = 0
                    Dim highest1 As Double = 0
                    Dim lowest1 As Double = 0
                    Dim y As Integer
                    Do While readerT.Read
                        count = count + 1
                        If count = 1 Then
                            highest1 = Val(readerT.Item("Total"))
                        End If
                        positionz = count
                        Select Case positionz
                            Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                                positionz = positionz.ToString + "st"
                            Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                                positionz = positionz.ToString + "nd"
                            Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                                positionz = positionz.ToString + "rd"
                            Case Else
                                positionz = positionz.ToString + "th"
                        End Select
                        posid.Add(readerT.Item("student"))
                        pos.Add(positionz)
                        lowest1 = Val(readerT.Item("Total"))
                        STotal = STotal + Val(readerT.Item("Total"))
                    Loop
                    SubjectAverage = FormatNumber(STotal / count, 2)
                    readerT.Close()
                    For y = 0 To posid.Count - 1
                        Dim Updatedatabase2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE SubjectReg SET avg = ?, Highest = ?, Lowest = ?, pos = ? WHERE Student = ? and SubjectsOfferred = ? and Session = ?", con)
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Average", SubjectAverage))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.highest", highest1))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.lowest", lowest1))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.position", pos(y)))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ID", posid(y)))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.subject", subject))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.session", Session("sessionid")))
                        Updatedatabase2.ExecuteNonQuery()
                    Next
                    count = Nothing
                Next




                Dim studentID As String
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM SubjectReg WHERE Student = ? And Session = ? And Class = ?", con)
                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM StudentSummary WHERE Class = ? And Session = ?", con)
                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader

                Dim i As Integer = 0
                Dim students As New ArrayList
                Do While reader3.Read
                    students.Add(reader3.Item("student"))
                Loop
                reader3.Close()
                Dim gtotals As New ArrayList

                Dim ct As Integer = 0
                For Each item As String In students
                    studentID = item

                    Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", studentID))
                    Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim param5 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    Dim readerf As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
                    Dim total As Integer = 0
                    Dim average As Double = 0
                    Dim count As Integer = 0
                    Do While readerf.Read()
                        count = count + 1
                        total = total + Val(readerf.Item("Total"))
                    Loop
                    gtotals.Add(total)
                    readerf.Close()
                    average = total / count

                    average = FormatNumber(average, 2)

                    Dim cmd22 As New MySql.Data.MySqlClient.MySqlCommand("SELECT grades.lowest, grades.average From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & cla & "' order by grades.lowest desc", con)
                    Dim graderead As MySql.Data.MySqlClient.MySqlDataReader = cmd22.ExecuteReader
                    Dim remarks As String = ""
                    Do While graderead.Read
                        If average >= Val(graderead.Item(0)) Then
                            remarks = graderead.Item(1).ToString
                            Exit Do
                        End If
                    Loop
                    graderead.Close()

                    Dim cmd2c As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Average = ?, principalremarks = ? WHERE student = ? And Session = ? And Class = ?", con)
                    Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Average", average))
                    Dim paramq As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ave", remarks))
                    Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentID))
                    Dim param6 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim param7 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))

                    cmd2c.ExecuteNonQuery()


                    cmd2c.Parameters.Remove(paramq)
                    cmd.Parameters.Remove(param)
                    cmd2c.Parameters.Remove(param2)
                    cmd2c.Parameters.Remove(param3)
                    cmd.Parameters.Remove(param4)
                    cmd.Parameters.Remove(param5)
                    cmd2c.Parameters.Remove(param6)
                    cmd2c.Parameters.Remove(param7)
                    total = Nothing
                    count = Nothing
                    ct = ct + 1

                Next
                gtotals.Sort()

                Dim cmd2x As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET classhigh = '" & gtotals(ct - 1) & "', classlow = '" & gtotals(0) & "' where Session = ? And Class = ?", con)
                Dim param6aaa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                Dim param7aa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmd2x.ExecuteNonQuery()
                Dim positionNo As Integer
                Dim position As String

                Dim cmdxx As New MySql.Data.MySqlClient.MySqlCommand("SELECT Average, student from StudentSummary WHERE Session = ? And Class = ? ORDER BY Average DESC", con)
                Dim cmd2xx As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Position = ? WHERE student = ? And Session = ? And Class = ?", con)
                cmdxx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                cmdxx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdxx.ExecuteReader
                Dim ix As Integer = 0
                Dim studentsx As New ArrayList
                Do While reader.Read
                    studentsx.Add(reader.Item("student"))
                Loop
                reader.Close()
                For Each item As String In studentsx
                    positionNo = positionNo + 1
                    Select Case positionNo
                        Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                            If positionNo = 1 Then
                            End If
                            position = positionNo.ToString + "st"
                        Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                            position = positionNo.ToString + "nd"
                        Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                            position = positionNo.ToString + "rd"
                        Case Else
                            position = positionNo.ToString + "th"
                    End Select
                    Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Position", position))
                    Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", item))
                    Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmd2xx.ExecuteNonQuery()
                    cmd2xx.Parameters.Remove(param)
                    cmd2xx.Parameters.Remove(param2)
                    cmd2xx.Parameters.Remove(param3)
                    cmd2xx.Parameters.Remove(param4)
                Next
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log("update class", Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Class(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try


            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classteacher inner join class on class.id = classteacher.class where classteacher .teacher = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("staffid")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    Do While student.Read
                        DropDownList1.Items.Add(student.Item(0).ToString)
                    Loop
                    panel3.Visible = False
                    student.Close()
                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    gridview1.DataSource = ds
                    gridview1.DataBind()
                    con.close()                End Using
                If Session("studentadd") <> Nothing Then
                    Student_Details()
                End If
            Else
                checkedSubjects.Clear()
                uncheckedSubjects.Clear()
                For Each i As ListItem In CheckBoxList1.Items

                    If i.Selected = True Then
                        checkedSubjects.Add(i.Text)
                    Else
                        uncheckedSubjects.Add(i.Text)
                    End If
                Next
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Private Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, surname from StudentsProfile where admno = ?", con)
            cmdLoad4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student8 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
            Dim pass As String = ""
            If student8.Read() Then pass = student8.Item("passport").ToString
            lblClass.Text = student8.Item(1).ToString
            student8.Close()
            If pass <> "" Then Image1.ImageUrl = pass
            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
            cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropDownList1.Text))
            Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
            student04.Read()
            Dim cla As Integer = student04.Item(0)
            student04.Close()

            Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ClassSubjects.ID, ClassSubjects.class, Subjects.Subject FROM Subjects INNER JOIN ClassSubjects ON Subjects.ID = ClassSubjects.subject WHERE ClassSubjects.class = ? And classSubjects.Type = ? and classsubjects.subjectnest = '" & 0 & "'", con)
            cmd4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ClassSubjects.Class", cla))
            cmd4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ClassSubjects.Type", "Optional"))
            Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
            CheckBoxList1.Items.Clear()
            Do While reader4.Read
                CheckBoxList1.Items.Add(reader4.Item(2))
            Loop
            reader4.Close()

            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT SubjectReg.ID, SubjectReg.Student, SubjectReg.Session, SubjectReg.Class, Subjects.Subject FROM Subjects INNER JOIN SubjectReg ON Subjects.ID = SubjectReg.SubjectsOfferred WHERE SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ? and subjectreg.nested = '" & 0 & "' order by subjectreg.id", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("studentadd")))
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("sessionid")))
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", cla))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim count As Integer = 1
            Do While reader2.Read
                Dim subject As New Label
                subject.ID = "cbosubject" & count
                subject.Text = count & ".   " & reader2.Item(4).ToString
                compulsory.Controls.Add(subject)
                For Each item As ListItem In CheckBoxList1.Items
                    If item.Text = reader2.Item(4).ToString Then
                        item.Selected = True
                    End If
                Next
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                compulsory.Controls.Add(MyLiteral)
                count = count + 1
            Loop
            reader2.Close()

            Dim cmd1sd As New MySql.Data.MySqlClient.MySqlCommand("SELECT SubjectReg.ID, SubjectReg.Student, SubjectReg.Session, SubjectReg.Class, Subjectnest.name FROM Subjectreg INNER JOIN subjectnest ON Subjectnest.id = SubjectReg.SubjectsOfferred WHERE SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ? and subjectreg.nested <> '" & 0 & "' order by subjectreg.id", con)
            cmd1sd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("studentadd")))
            cmd1sd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("sessionid")))
            cmd1sd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", cla))
            Dim reader234 As MySql.Data.MySqlClient.MySqlDataReader = cmd1sd.ExecuteReader
            Do While reader234.Read
                Dim subject As New Label
                subject.ID = "cbosubject" & count
                subject.Text = count & ".   " & reader234.Item(4).ToString
                compulsory.Controls.Add(subject)
                For Each item As ListItem In CheckBoxList1.Items
                    If item.Text = reader234.Item(4).ToString Then
                        item.Selected = True
                    End If
                Next
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                compulsory.Controls.Add(MyLiteral)
                count = count + 1
            Loop
            reader234.Close()

            con.close()        End Using
        panel3.Visible = True
        pnlAll.Visible = False
        gridview1.SelectedIndex = -1
    End Sub

    Protected Sub gridview1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gridview1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join (class inner join classteacher on classteacher.class = class.id) on studentsummary.class = class.id WHERE class.class = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                Session("studentadd") = Nothing
                gridview1.PageIndex = e.NewPageIndex
                gridview1.DataBind()
                gridview1.SelectedIndex = -1
                panel3.Visible = False
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            panel3.Visible = True
            pnlAll.Visible = False
            gridview1.SelectedIndex = -1
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join (class inner join classteacher on classteacher.class = class.id) on studentsummary.class = class.id WHERE class.class = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                pnlAll.Visible = True
                gridview1.SelectedIndex = -1
                panel3.Visible = False
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim isOfferred As Boolean = False
                Dim subId As Integer
                For Each item As String In checkedSubjects
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId = reader20.Item(0)
                    reader20.Close()

                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropDownList1.Text))
                    Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                    student04.Read()
                    Dim cla As Integer = student04.Item(0)
                    student04.Close()

                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From SubjectReg Where SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ? And SubjectReg.SubjectsOfferred = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("studentadd")))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("sessionid")))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", cla))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", subId))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    If reader3.Read Then
                        isOfferred = True
                    Else
                        isOfferred = False
                    End If
                    reader3.Close()
                    If isOfferred = False Then
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("sessionid")))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("studentadd")))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.ExecuteNonQuery()
                        logify.log(Session("staffid"), item & " was added to the subjects offerred by " & par.getstuname(Session("studentadd")))
                        logify.Notifications(item & " was added to your subjects.", Session("studentadd"), Session("staffid"), "Class Teacher", "~/content/parent/classdetails.aspx", "")
                        If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(item & " was added to the subjects offerred by " & par.getstuname(Session("studentadd")), par.getparent(Session("studentadd")), Session("staffid"), DropDownList1.Text & " Class Teacher", "~/content/parent/classdetails.aspx?" & Session("studentadd"), "")

                    End If
                Next


                For Each item As String In uncheckedSubjects
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId = reader20.Item(0)
                    reader20.Close()
                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropDownList1.Text))
                    Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                    student04.Read()
                    Dim cla As Integer = student04.Item(0)
                    student04.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From SubjectReg Where SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ? And SubjectReg.SubjectsOfferred = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("studentadd")))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("sessionid")))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", cla))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", subId))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    If reader3.Read Then
                        isOfferred = True
                    Else
                        isOfferred = False
                    End If
                    reader3.Close()

                    If isOfferred = True Then
                        Dim cmdDelete1 As New MySql.Data.MySqlClient.MySqlCommand("Delete From SubjectReg Where Student = ? And Session = ? And Class = ? And SubjectsOfferred = ?", con)
                        cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("studentadd")))
                        cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("sessionid")))
                        cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectsOfferred", subId))
                        cmdDelete1.ExecuteNonQuery()
                        logify.log(Session("staffid"), item & " was removed from the subjects offerred by " & par.getstuname(Session("studentadd")))
                        logify.Notifications(item & " was removed from your subjects.", Session("studentadd"), Session("staffid"), "Class Teacher", "~/content/student/classdetails.aspx?", "")
                        If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(item & " was removed from the subjects offerred by " & par.getstuname(Session("studentadd")), par.getparent(Session("studentadd")), Session("staffid"), DropDownList1.Text & " Class Teacher", "~/content/parent/classdetails.aspx?" & Session("studentadd"), "")

                    End If
                Next
                Dim cmdLoad10s As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                cmdLoad10s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropDownList1.Text))
                Dim student04s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10s.ExecuteReader
                student04s.Read()
                Dim clad As Integer = student04s.Item(0)
                student04s.Close()

                update_class(clad)
                con.close()            End Using
            Student_Details()
            Show_Alert(True, "Subjects updated")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
