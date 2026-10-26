Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls

Partial Class Staff_scoresheet
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
    


    Private Sub Bind_Data()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Session("Selectterm") = Session("sessionid")
            If cboClass.Text = "Select Class" Then
                Show_Alert(False, "Please select a subject to continue")

                Exit Sub
            End If
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Class WHERE Class = ?", con)
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cboClass.Text))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
            reader2.Read()
            Session("thisclass") = reader2.Item(0)
            reader2.Close()

            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjects WHERE Subject = ?", con)
            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader()
            If reader3.Read() Then
                Session("thissubid") = reader3.Item(0)
            End If
            reader3.Close()

            Dim cmdcheck34 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjectnest WHERE name = ?", con)
            cmdcheck34.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
            Dim reader34 As MySql.Data.MySqlClient.MySqlDataReader = cmdcheck34.ExecuteReader()
            If reader34.Read() Then
                Session("thissubnest") = reader34.Item(0)
            End If
            reader34.Close()


            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cano from class Where id = ?", con)
            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("thisclass")))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
            reader20.Read()
            Dim caquery As Integer = 2
            If reader20(0) = 3 Then
                caquery = 3
            ElseIf reader20(0) = 4 Then
                caquery = 4
            End If
            reader20.Close()
            If Session("thissubid") <> Nothing Then
                If caquery = 2 Then
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.nested = '" & 0 & "' and SubjectReg.SubjectsOfferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubid")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1.SelectCommand = cmd
                    adapter1.Fill(ds)
                    GridView1.DataSource = ds
                    GridView1.Columns(5).Visible = False
                    GridView1.Columns(6).Visible = False
                    GridView1.DataBind()

                ElseIf caquery = 3 Then
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', subjectreg.ca3 as '3rd CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.nested = '" & 0 & "' and SubjectReg.SubjectsOfferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubid")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1.SelectCommand = cmd
                    adapter1.Fill(ds)
                    GridView1.DataSource = ds
                    GridView1.Columns(6).Visible = False
                    GridView1.DataBind()

                ElseIf caquery = 4 Then
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', subjectreg.ca3 as '3rd CA', subjectreg.project as '4th CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.nested = '" & 0 & "' and SubjectReg.SubjectsOfferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubid")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1.SelectCommand = cmd
                    adapter1.Fill(ds)
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If




            Else
                If caquery = 2 Then
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.nested = '" & 1 & "' and subjectreg.subjectsofferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubnest")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1.SelectCommand = cmd
                    adapter1.Fill(ds)
                    GridView1.DataSource = ds
                    GridView1.Columns(5).Visible = False
                    GridView1.Columns(6).Visible = False
                    GridView1.DataBind()

                ElseIf caquery = 3 Then
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', subjectreg.ca3 as '3rd CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.nested = '" & True & "' and subjectreg.subjectsofferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubnest")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1.SelectCommand = cmd
                    adapter1.Fill(ds)
                    GridView1.DataSource = ds
                    GridView1.Columns(6).Visible = False
                    GridView1.DataBind()

                ElseIf caquery = 4 Then
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', subjectreg.ca3 as '3rd CA', subjectreg.project as '4th CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg  ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.nested = '" & 1 & "' and subjectreg.subjectsofferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubnest")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1.SelectCommand = cmd
                    adapter1.Fill(ds)
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If


            End If
            If Session("thissubid") <> Nothing Then
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectreg.* from SubjectReg WHERE SubjectReg.nested = '" & 0 & "' and SubjectReg.SubjectsOfferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ?", con)
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubid")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Dim no As Integer
                Do While reader.Read()
                    lblSA.Text = reader.Item("avg").ToString
                    lblLowest.Text = reader.Item("lowest").ToString
                    lbhighest.Text = reader.Item("highest").ToString
                    no = no + 1
                Loop

                reader.Close()
                lblNo.Text = no
            Else
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectreg.* from SubjectReg WHERE SubjectReg.nested = '" & 1 & "' and subjectreg.subjectsofferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ?", con)
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubnest")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Dim no As Integer
                Do While reader.Read()
                    lblSA.Text = reader.Item("avg").ToString
                    lblLowest.Text = reader.Item("lowest").ToString
                    lbhighest.Text = reader.Item("highest").ToString
                    no = no + 1
                Loop

                reader.Close()
                lblNo.Text = no
            End If
            Session("thissubid") = Nothing
            con.close()        End Using
        panel1.Visible = True
        GridView1.DataBind()

    End Sub
    Protected Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjects WHERE Subject = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader()
                If reader3.Read() Then
                    Session("thissubid") = reader3.Item(0)
                End If
                reader3.Close()

                Dim cmdcheck34 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjectnest WHERE name = ?", con)
                cmdcheck34.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
                Dim reader34 As MySql.Data.MySqlClient.MySqlDataReader = cmdcheck34.ExecuteReader()
                If reader34.Read() Then
                    Session("thissubnest") = reader34.Item(0)
                End If
                reader34.Close()
                Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
                Dim ID As String = row.Cells(1).Text

                Dim CA1 As String = TryCast(row.Cells(3).Controls(0), TextBox).Text
                Dim CA2 As String = TryCast(row.Cells(4).Controls(0), TextBox).Text
                Dim CA3 As String
                Dim project As String
                Dim exam As String = TryCast(row.Cells(7).Controls(0), TextBox).Text

                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cano from class Where id = ?", con)
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("thisclass")))
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                reader20.Read()
                Dim caquery As Integer = 2
                If reader20(0) = 3 Then
                    caquery = 3
                    CA3 = TryCast(row.Cells(5).Controls(0), TextBox).Text
                ElseIf reader20(0) = 4 Then
                    caquery = 4
                    CA3 = TryCast(row.Cells(5).Controls(0), TextBox).Text
                    project = TryCast(row.Cells(6).Controls(0), TextBox).Text
                End If
                reader20.Close()

                Dim testtotal As Double = Val(CA1) + Val(CA2) + Val(CA3) + Val(project)
                Dim total As Double = testtotal + Val(exam)
                Dim cmd22 As New MySql.Data.MySqlClient.MySqlCommand("SELECT grades.lowest, grades.grade, grades.subject From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & Session("thisclass") & "' order by grades.lowest desc", con)
                Dim graderead As MySql.Data.MySqlClient.MySqlDataReader = cmd22.ExecuteReader
                Dim remarks As String = ""
                Dim grade As String = ""
                Do While graderead.Read
                    If total >= Val(graderead.Item(0)) Then
                        grade = graderead.Item(1)
                        remarks = graderead.Item(2)
                        Exit Do
                    End If
                Loop
                graderead.Close()
                If Session("thissubid") <> Nothing Then
                    Dim UpdateDatabase As New MySql.Data.MySqlClient.MySqlCommand("UPDATE SubjectReg SET SubjectReg.CA1 = ?, SubjectReg.CA2 = ?, SubjectReg.CA3 = ?, subjectreg.Grade = ?, subjectreg.Remarks = ?, SubjectReg.project = ?, SubjectReg.testtotal = ?, SubjectReg.Examination = ?, Total = ? WHERE SubjectReg.student = ? and subjectreg.nested = '" & 0 & "' and SubjectReg.Subjectsofferred = ? and subjectreg.class = ? and subjectreg.session = ?", con)
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.CA1", CA1))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.CA2", CA2))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.CA3", CA3))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.grade", grade))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.remarks", remarks))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("project", project))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.testtotal", testtotal))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Examination", exam))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Total", total))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ID", ID))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.subject", Session("thissubid")))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.class", Session("thisclass")))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.session", Session("Selectterm")))
                    UpdateDatabase.ExecuteNonQuery()
                    logify.log(Session("staffid"), "The " & cboSubject.Text & " scores of " & par.getstuname(ID) & " was updated")
                    Dim Saverage As New MySql.Data.MySqlClient.MySqlCommand("SELECT Total, student FROM SubjectReg WHERE Session = ? AND Class = ? and subjectreg.nested = '" & 0 & "' AND subjectsOfferred = ? Order by total Desc", con)
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Selectterm")))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("thisclass")))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjectsOffered", Session("thissubid")))
                    Dim readerT As MySql.Data.MySqlClient.MySqlDataReader = Saverage.ExecuteReader
                    Dim STotal As Double
                    Dim SubjectAverage As Double
                    Dim position As String
                    Dim pos As New ArrayList
                    Dim posid As New ArrayList
                    Dim count As Integer
                    Dim highest As Double
                    Dim lowest As Double
                    Dim y As Integer
                    Do While readerT.Read
                        count = count + 1
                        If count = 1 Then
                            highest = Val(readerT.Item("Total"))
                        End If
                        position = count
                        Select Case position
                            Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                                position = position.ToString + "st"
                            Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                                position = position.ToString + "nd"
                            Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                                position = position.ToString + "rd"
                            Case Else
                                position = position.ToString + "th"
                        End Select
                        posid.Add(readerT.Item("student"))
                        pos.Add(position)
                        lowest = Val(readerT.Item("Total"))
                        STotal = STotal + Val(readerT.Item("Total"))
                    Loop
                    SubjectAverage = FormatNumber(STotal / count, 2)
                    readerT.Close()
                    For y = 0 To posid.Count - 1
                        Dim Updatedatabase2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE SubjectReg SET avg = ?, Highest = ?, Lowest = ?, pos = ? WHERE Student = ?  and subjectreg.nested = '" & 0 & "' and SubjectsOfferred = ? and Session = ?", con)
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Average", SubjectAverage))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.highest", highest))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.lowest", lowest))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.position", pos(y)))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ID", posid(y)))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.subject", Session("thissubid")))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.session", Session("Selectterm")))
                        Updatedatabase2.ExecuteNonQuery()
                    Next
                Else
                    Dim UpdateDatabase As New MySql.Data.MySqlClient.MySqlCommand("UPDATE SubjectReg SET SubjectReg.CA1 = ?, SubjectReg.CA2 = ?, SubjectReg.CA3 = ?, subjectreg.Grade = ?, subjectreg.Remarks = ?, SubjectReg.project = ?, SubjectReg.testtotal = ?, SubjectReg.Examination = ?, Total = ? WHERE SubjectReg.student = ? and subjectreg.nested = '" & 1 & "' and SubjectReg.Subjectsofferred = ? and subjectreg.class = ? and subjectreg.session = ?", con)
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.CA1", CA1))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.CA2", CA2))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.CA3", CA3))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.grade", grade))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.remarks", remarks))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("project", project))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.testtotal", testtotal))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Examination", exam))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Total", total))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ID", ID))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.subject", Session("thissubnest")))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.class", Session("thisclass")))
                    UpdateDatabase.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.session", Session("Selectterm")))
                    UpdateDatabase.ExecuteNonQuery()
                    logify.log(Session("staffid"), "The " & cboSubject.Text & " scores of " & par.getstuname(ID) & " was updated")
                    Dim Saverage As New MySql.Data.MySqlClient.MySqlCommand("SELECT Total, student FROM SubjectReg WHERE Session = ? AND Class = ? and subjectreg.nested = '" & 1 & "' AND subjectsOfferred = ? Order by total Desc", con)
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Selectterm")))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("thisclass")))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjectsOffered", Session("thissubnest")))
                    Dim readerT As MySql.Data.MySqlClient.MySqlDataReader = Saverage.ExecuteReader
                    Dim STotal As Double
                    Dim SubjectAverage As Double
                    Dim position As String
                    Dim pos As New ArrayList
                    Dim posid As New ArrayList
                    Dim count As Integer
                    Dim highest As Double
                    Dim lowest As Double
                    Dim y As Integer
                    Do While readerT.Read
                        count = count + 1
                        If count = 1 Then
                            highest = Val(readerT.Item("Total"))
                        End If
                        position = count
                        Select Case position
                            Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                                position = position.ToString + "st"
                            Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                                position = position.ToString + "nd"
                            Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                                position = position.ToString + "rd"
                            Case Else
                                position = position.ToString + "th"
                        End Select
                        posid.Add(readerT.Item("student"))
                        pos.Add(position)
                        lowest = Val(readerT.Item("Total"))
                        STotal = STotal + Val(readerT.Item("Total"))
                    Loop
                    SubjectAverage = FormatNumber(STotal / count, 2)
                    readerT.Close()
                    For y = 0 To posid.Count - 1
                        Dim Updatedatabase2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE SubjectReg SET avg = ?, Highest = ?, Lowest = ?, pos = ? WHERE Student = ? and subjectreg.nested = '" & 1 & "' and SubjectsOfferred = ? and Session = ?", con)
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Average", SubjectAverage))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.highest", highest))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.lowest", lowest))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.position", pos(y)))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ID", posid(y)))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.subject", Session("thissubnest")))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.session", Session("Selectterm")))
                        Updatedatabase2.ExecuteNonQuery()
                    Next

                End If
                Session("thissubid") = Nothing
                con.close()            End Using
            Compute_Average()
            Class_Position()
            GridView1.EditIndex = -1
            Bind_Data()
            Show_Alert(True, "Scores updated")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub
    Sub Class_Position()
        Dim positionNo As Integer
        Dim position As String
        Dim highest As Integer
        Dim lowest As Integer
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT Average, student from StudentSummary WHERE Session = ? And Class = ? ORDER BY Average DESC", con)
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Position = ? WHERE student = ? And Session = ? And Class = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Selectterm")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("thisclass")))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
            Dim i As Integer = 0
            Dim students As New ArrayList
            Do While reader.Read
                students.Add(reader.Item("student"))
            Loop
            reader.Close()
            For Each item As String In students
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
                Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Position", position))
                Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", item))
                Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Selectterm")))
                Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("thisclass")))
                cmd2.ExecuteNonQuery()
                cmd2.Parameters.Remove(param)
                cmd2.Parameters.Remove(param2)
                cmd2.Parameters.Remove(param3)
                cmd2.Parameters.Remove(param4)
            Next
            con.close()        End Using
    End Sub
    Sub Compute_Average()
        Dim studentID As String
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM SubjectReg WHERE Student = ? And Session = ? And Class = ?", con)
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM StudentSummary WHERE Class = ? And Session = ?", con)
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("thisclass")))
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Selectterm")))
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
                Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Selectterm")))
                Dim param5 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("thisclass")))
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
                Dim total As Integer = 0
                Dim average As Double
                Dim count As Integer
                Do While reader.Read()
                    count = count + 1
                    total = total + Val(reader.Item("Total"))
                Loop
                gtotals.Add(total)
                reader.Close()
                average = total / count
                average = FormatNumber(average, 2)

                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Average = ? WHERE student = ? And Session = ? And Class = ?", con)
                Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Average", average))
                Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentID))
                Dim param6 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Selectterm")))
                Dim param7 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("thisclass")))

                cmd2.ExecuteNonQuery()
                cmd.Parameters.Remove(param)
                cmd2.Parameters.Remove(param2)
                cmd2.Parameters.Remove(param3)
                cmd.Parameters.Remove(param4)
                cmd.Parameters.Remove(param5)
                cmd2.Parameters.Remove(param6)
                cmd2.Parameters.Remove(param7)
                total = Nothing
                count = Nothing
                ct = ct + 1
            Next
            gtotals.Sort()

            Dim cmd2x As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET classhigh = '" & gtotals(ct - 1) & "', classlow = '" & gtotals(0) & "' where Session = ? And Class = ?", con)
            Dim param6aaa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Selectterm")))
            Dim param7aa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("thisclass")))
            cmd2x.ExecuteReader()
            con.close()        End Using

    End Sub


    Protected Sub GridView1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        Try
            GridView1.EditIndex = -1
            Bind_Data()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub GridView1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridView1.RowEditing
        Try
            GridView1.EditIndex = e.NewEditIndex

            Bind_Data()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub


    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjects.Subject FROM classsubjects INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjects ON classsubjects.subject = subjects.ID inner join staffprofile on staffprofile.staffid = classsubjects.teacher Where staffprofile.surname = '" & DropDownList1.Text & "'and classsubjects.subjectnest = '" & 0 & "' Order By subjects.subject ", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                cboClass.Items.Clear()
                cboSubject.Items.Clear()
                cboSubject.Items.Add("Select Subject")
                Dim subject As New ArrayList
                Do While reader2.Read
                    If Not subject.Contains(reader2.Item(1).ToString) Then
                        subject.Add(reader2.Item(1).ToString)
                        cboSubject.Items.Add(reader2.Item(1).ToString)
                    End If
                Loop
                reader2.Close()

                Dim cmdss3d As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjectnest.name FROM classsubjects INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjectnest ON classsubjects.subjectnest = subjectnest.ID inner join staffprofile on staffprofile.staffid = classsubjects.teacher Where staffprofile.surname = '" & DropDownList1.Text & "'", con)
                Dim reader223 As MySql.Data.MySqlClient.MySqlDataReader = cmdss3d.ExecuteReader
                cboClass.Items.Clear()
                Dim subject2 As New ArrayList
                Do While reader223.Read
                    If Not subject2.Contains(reader223.Item(1).ToString) Then
                        subject2.Add(reader223.Item(1).ToString)
                        cboSubject.Items.Add(reader223.Item(1).ToString)
                    End If
                Loop
                reader223.Close()

                GridView1.DataSource = ""
                panel1.Visible = False
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub KBind_Data()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Session("Selectterm") = Session("sessionid")
            If cboClass.Text = "Select Class" Then
                Show_Alert(False, "Please select a subject to continue")

                Exit Sub
            End If
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Class WHERE Class = ?", con)
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cboClass.Text))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
            reader2.Read()
            Session("thisclass") = reader2.Item(0)
            reader2.Close()

            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjects WHERE Subject = ?", con)
            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader()
            If reader3.Read() Then
                Session("thissubid") = reader3.Item(0)
            End If
            reader3.Close()







            Session("thissubid") = Nothing
            con.close()        End Using



    End Sub

  
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_dh(Session("roles"), Session("usertype")) = False And check.Check_sh(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try


            If IsPostBack Then
            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle from depts where head = '" & Session("staffid") & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Dim deptshead As New ArrayList
                    Do While student0.Read
                        deptshead.Add(student0.Item(0))
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
                            If Not deptstaff.Contains(schclass1(0)) Then deptstaff.Add(schclass1.Item(0))
                        Loop
                        schclass1.Close()
                        DropDownList1.Items.Clear()
                        For Each sitem As String In deptstaff
                            Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname from classsubjects inner join staffprofile on classsubjects.teacher = staffprofile.staffid where classsubjects.teacher = '" & sitem & "'  and staffprofile.activated = '" & 1 & "'", con)
                            Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader

                            If schclass11.Read Then
                                DropDownList1.Items.Add(schclass11.Item(0).ToString)
                            End If
                            schclass11.Close()
                        Next
                    Next

                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjects.Subject FROM classsubjects INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjects ON classsubjects.subject = subjects.ID inner join staffprofile on staffprofile.staffid = classsubjects.teacher Where staffprofile.surname = '" & DropDownList1.Text & "'and classsubjects.subjectnest = '" & 0 & "' Order By subjects.subject ", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                    cboClass.Items.Clear()
                    cboSubject.Items.Clear()
                    cboSubject.Items.Add("Select Subject")
                    Dim subject As New ArrayList
                    Do While reader2.Read
                        If Not subject.Contains(reader2.Item(1).ToString) Then
                            subject.Add(reader2.Item(1).ToString)
                            cboSubject.Items.Add(reader2.Item(1).ToString)
                        End If
                    Loop
                    reader2.Close()

                    Dim cmdss3d As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjectnest.name FROM classsubjects INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjectnest ON classsubjects.subjectnest = subjectnest.ID inner join staffprofile on staffprofile.staffid = classsubjects.teacher Where staffprofile.surname = '" & DropDownList1.Text & "'", con)
                    Dim reader223 As MySql.Data.MySqlClient.MySqlDataReader = cmdss3d.ExecuteReader
                    cboClass.Items.Clear()
                    Dim subject2 As New ArrayList
                    Do While reader223.Read
                        If Not subject2.Contains(reader223.Item(1).ToString) Then
                            subject2.Add(reader223.Item(1).ToString)
                            cboSubject.Items.Add(reader223.Item(1).ToString)
                        End If
                    Loop
                    reader223.Close()
                    con.close()                End Using
                GridView1.DataSource = ""
                panel1.Visible = False
                GridView1.DataBind()

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Try
            If cboSubject.Text = "Select Subject" Then
                Show_Alert(False, "Please select a subject to continue")
                cboClass.Items.Clear()
                Exit Sub
            End If

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim subID As Integer
                Dim subnest As Integer
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjects WHERE Subject = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader()
                Do While reader3.Read()
                    subID = reader3.Item(0)

                Loop
                reader3.Close()

                Dim cmdcheck3g As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjectnest WHERE name = ?", con)
                cmdcheck3g.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
                Dim reader2s3 As MySql.Data.MySqlClient.MySqlDataReader = cmdcheck3g.ExecuteReader()
                Do While reader2s3.Read()
                    subnest = reader2s3.Item(0)
                Loop
                reader2s3.Close()

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjects.Subject FROM classsubjects INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjects ON classsubjects.subject = subjects.ID inner join staffprofile on staffprofile.staffid = classsubjects.teacher Where staffprofile.surname = '" & DropDownList1.Text & "' And classsubjects.subject = '" & subID & "' and classsubjects.subjectnest = '" & 0 & "'", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                cboClass.Items.Clear()
                cboClass.Items.Add("Select Class")
                Do While reader2.Read
                    cboClass.Items.Add(reader2.Item(0).ToString)
                Loop
                reader2.Close()
                If subnest <> 0 Then
                    Dim cmdcheck2g As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjects.Subject FROM classsubjects INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjects ON classsubjects.subject = subjects.ID inner join staffprofile on staffprofile.staffid = classsubjects.teacher Where staffprofile.surname = '" & DropDownList1.Text & "' and classsubjects.subjectnest = '" & subnest & "'", con)
                    Dim reader2g As MySql.Data.MySqlClient.MySqlDataReader = cmdcheck2g.ExecuteReader
                    Dim classes As New ArrayList
                    Do While reader2g.Read
                        If Not classes.Contains(reader2g(0).ToString) Then
                            cboClass.Items.Add(reader2g.Item(0).ToString)
                            classes.Add(reader2g.Item(0).ToString)
                        End If
                    Loop
                    reader2g.Close()
                End If
                con.close()            End Using
            GridView1.DataSource = ""
            panel1.Visible = False
            GridView1.DataBind()
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



    Protected Sub cboClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboClass.SelectedIndexChanged
        Try
            Bind_Data()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



   
End Class
