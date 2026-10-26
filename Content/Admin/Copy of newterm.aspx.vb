Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_newterm
    Inherits System.Web.UI.Page


    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            For i As Integer = 1 To 31
                DropDownList3.Items.Add(i)
                DropDownList6.Items.Add(i)
            Next
            For j As Integer = 1990 To Now.Year
                DropDownList5.Items.Add(j)
                DropDownList8.Items.Add(j)
            Next
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM sessioncreate", con)
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Do While reader20.Read
                    DropDownList1.Items.Add(reader20.Item(1))
                Loop
                reader20.Close()
                con.close()            End Using
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim closing As Date = DropDownList3.Text & "/" & DropDownList4.Text & "/" & DropDownList5.Text
        Dim resumption As Date = DropDownList6.Text & "/" & DropDownList7.Text & "/" & DropDownList8.Text


        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where session = ? and term = ?", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", DropDownList1.Text))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", DropDownList2.Text))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
            If reader20.Read Then
                reader20.Close()
                lblError.Text = "Term already exists"
                Exit Sub
            Else
                reader20.Close()
                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into Session (session, term, closingdate, nextterm) Values (?,?,?,?)", con)
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session2", DropDownList1.Text))
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term2", DropDownList2.Text))
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", closing))
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("nextterm", resumption))
                cmdInsert2.ExecuteNonQuery()
                Dim lastses As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session order by ID Desc", con)
                Dim sesread As MySql.Data.MySqlClient.MySqlDataReader = lastses.ExecuteReader
                sesread.Read()
                Dim thissession As Integer = sesread.Item(0)
                sesread.Close()
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
                        For Each student As Integer In classstudents

                            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where ID = ?", con)
                            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", student))
                            Dim studentread As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                            studentread.Read()
                            Dim dob As TimeSpan = Now.Subtract(studentread.Item("dateofbirth"))
                            Dim age As Integer = dob.TotalDays \ 365
                            studentread.Close()


                            Dim studentsinsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into Studentsummary (session, class, student, age) values (?,?,?,?)", con)
                            studentsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session4", newterm))
                            studentsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class2", item))
                            studentsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                            studentsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("age", age))
                            studentsinsert.ExecuteNonQuery()

                            Dim stusub As New MySql.Data.MySqlClient.MySqlCommand("Select * from subjectreg where session = ? and student = ? and class = ?", con)
                            stusub.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session5", last))
                            stusub.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student1", student))
                            stusub.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class3", item))
                            Dim subjects As MySql.Data.MySqlClient.MySqlDataReader = stusub.ExecuteReader
                            Dim studentsub As New ArrayList
                            Do While subjects.Read
                                studentsub.Add(subjects.Item(4))
                            Loop
                            subjects.Close()
                            For Each subject As Integer In studentsub
                                Dim subjectsinsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into subjectreg (session, class, student, subjectsofferred) values (?,?,?,?)", con)
                                subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session6", newterm))
                                subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class3", item))
                                subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student2", student))
                                subjectsinsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", subject))
                                subjectsinsert.ExecuteNonQuery()

                            Next
                        Next
                        iflast = False
                    Next
                End If

                con.close()end using
        End If
    End Sub
End Class
