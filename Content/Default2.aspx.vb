Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Partial Class Default2
    Inherits System.Web.UI.Page

    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter
    Dim ds As New DataSet
    Dim dr As MySql.Data.MySqlClient.MySqlDataReader
    Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader
    
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblError.Text = ""
        Session.Clear()
        
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Session.Clear()
                cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from admin where username='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)

                dr = cmd.ExecuteReader


                If dr.Read = True Then
                    Session("usertype") = "super admin"
                End If

                dr.Close()








                cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from staffprofile where staffid='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)

                dr = cmd.ExecuteReader


                If dr.Read = True Then
                    If dr("activated") = False Then
                        lblError.Text = "You have been deactivated. Access denied."
                        Exit Sub
                    End If
                    Dim sid As String = txtID.Text.ToString
                    Session("usertype") = "staff"
                    'Session("StudentID") = txtreType.Text
                    Session("Staffid") = dr(0).ToString
                    'Session.Add("", auth)
                    Session("staff") = dr(1).ToString
                    dr.Close()

                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    Try
                        reader2.Read()
                        Session("SessionID") = reader2(0).ToString
                        Session("SessionName") = reader2(1).ToString
                        Session("Term") = reader2(2).ToString
                        reader2.Close()
                    Catch
                        Session("errmsg") = "No session"
                        reader2.Close()
                    End Try
                    FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
                End If
                dr.Close()
                cmd2 = New MySql.Data.MySqlClient.MySqlCommand("select * from parentprofile where parentId='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)

                dr = cmd2.ExecuteReader

                If dr.Read = True Then
                    Session("usertype") = "parent"
                    Dim sid As String = txtID.Text.ToString
                    'Session("StudentID") = txtreType.Text
                    Session("ParentID") = dr(0).ToString
                    'Session.Add("", auth)
                    Session("parent") = dr("parentname").ToString
                    dr.Close()
                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    reader2.Read()
                    Session("SessionID") = reader2(0).ToString
                    Session("SessionName") = reader2(1).ToString
                    Session("term") = reader2(2).ToString
                    reader2.Close()

                    Dim cmdSelect2sa As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.activated from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno where parentward.parent = '" & Session("ParentID") & "' and studentsprofile.activated = '" & 1 & "'", con)
                    Dim reader2sa As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2sa.ExecuteReader
                    If Not reader2sa.Read Then
                        lblError.Text = "You don't have any active child/ward. Access denied."
                        Session.Abandon()
                        Exit Sub
                    End If
                    reader2sa.Close()
                End If
                dr.Close()

                cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from studentsprofile where admno='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)

                dr = cmd.ExecuteReader


                If dr.Read = True Then
                    If dr("activated") = False Then
                        lblError.Text = "You have been deactivated. You no longer have access."
                        Exit Sub
                    End If
                    Dim sid As String = txtID.Text.ToString

                    Session("usertype") = "student"
                    'Session("StudentID") = txtreType.Text
                    Session("StudentID") = dr(0).ToString
                    'Session.Add("", auth)

                    dr.Close()

                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", txtID.Text))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    student.Read()
                    Session("student") = student(1).ToString
                    Session("studentpass") = student("passport")
                    student.Close()

                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    reader2.Read()
                    Session("SessionID") = reader2(0).ToString
                    Session("SessionName") = reader2(1).ToString
                    reader2.Close()

                    Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM studentSummary Where Session = ? And student = ?", con)
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Term", Session("SessionID")))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentID")))
                    Dim classreader As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert2.ExecuteReader
                    Try
                        classreader.Read()
                        Session("ClassID") = classreader.Item(2).ToString

                    Catch

                    End Try
                    classreader.Close()
                    Dim cmdInsert3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Class Where ID = ?", con)
                    cmdInsert3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                    Dim classreader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert3.ExecuteReader
                    Try
                        classreader2.Read()
                        Session("ClassName") = classreader2.Item(1).ToString
                    Catch

                    End Try
                    classreader2.Close()
                    FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
                End If

                dr.Close()

                If Session("usertype") <> Nothing Then
                    Session("isnew") = True
                    Response.Redirect("~/userwelcome.aspx")
                Else
                    lblError.Text = "Your user name or password is incorrect."


                End If
                con.close()            End Using
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
End Class
