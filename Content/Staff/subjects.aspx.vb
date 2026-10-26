Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Staff_classmng
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As String
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList


    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        load_subjects()
    End Sub
    Protected Sub load_subjects()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where surname = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Trim(DropDownList1.Text)))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            student.Read()
            studentId = student.Item(0)
            Session("Id") = student.Item(0)
            Session("studentId") = student.Item(1)
            student.Close()

            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader()
            reader3.Read()
            Session("termID") = reader3.Item(0)
            reader3.Close()

            Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ClassSubjects.ID, ClassSubjects.class, Subjects.Subject FROM Subjects INNER JOIN ClassSubjects ON Subjects.ID = ClassSubjects.subject WHERE ClassSubjects.class = ? And Type = ?", con)
            cmd4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ClassSubjects.Class", Session("ClassID").ToString))
            cmd4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ClassSubjects.Type", "Optional"))
            Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
            CheckBoxList1.Items.Clear()
            Do While reader4.Read
                CheckBoxList1.Items.Add(reader4.Item(2))
            Loop
            reader4.Close()

            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT SubjectReg.ID, SubjectReg.Student, SubjectReg.Session, SubjectReg.Class, Subjects.Subject FROM Subjects INNER JOIN SubjectReg ON Subjects.ID = SubjectReg.SubjectsOfferred WHERE SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ?", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", studentId))
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("termID")))
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("ClassID").ToString))
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

            con.close()        End Using
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblSuccess.Text = ""
        If Not IsPostBack Then
            If Session("ClassID") Is Nothing Then
                Session("ClassID") = Session("AddClass")
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.surname FROM StudentsProfile INNER JOIN StudentSummary ON StudentsProfile.admno = StudentSummary.student WHERE StudentSummary.Class = ? And StudentSummary.Session = ? ORDER BY StudentsProfile.surname", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("ClassID")))
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
                Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
                DropDownList1.Items.Clear()
                DropDownList1.Items.Add("Select a student")
                Do While studentsReader.Read
                    DropDownList1.Items.Add(studentsReader.Item(0).ToString)
                Loop
                studentsReader.Close()
                con.close()            End Using
            If Not Session("StudentId") Is Nothing Then
                DropDownList1.Text = Session("StudentId").ToString
                load_subjects()
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
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim isOfferred As Boolean = False
            Dim subId As Integer
            For Each item As String In checkedSubjects
                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                reader20.Read()
                subId = reader20.Item(0)
                reader20.Close()

                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From SubjectReg Where SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ? And SubjectReg.SubjectsOfferred = ?", con)
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Id")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("termID")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("ClassID").ToString))
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
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("termID")))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID").ToString))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Id")))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                    cmd3.ExecuteNonQuery()
                End If
            Next


            For Each item As String In uncheckedSubjects
                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                reader20.Read()
                subId = reader20.Item(0)
                reader20.Close()

                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From SubjectReg Where SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ? And SubjectReg.SubjectsOfferred = ?", con)
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Id")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("termID")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("ClassID").ToString))
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
                    cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Id")))
                    cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("termID")))
                    cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID").ToString))
                    cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectsOfferred", subId))
                    cmdDelete1.ExecuteNonQuery()
                End If
            Next
            con.close()        End Using
        load_subjects()
        lblSuccess.Text = "Subjects updated"
    End Sub
End Class
