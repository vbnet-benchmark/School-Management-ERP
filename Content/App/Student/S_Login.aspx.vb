Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration

Public Class S_Login
    Inherits System.Web.UI.Page

    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter
    Dim ds As New DataSet
    Dim dr As MySql.Data.MySqlClient.MySqlDataReader
    Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()


            cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from pin where admno='" & txtID.Text & "' AND pin='" & txtpassword.Value & "'", con)

            dr = cmd.ExecuteReader


            If dr.Read = True Then
                If dr.Item("expiry") >= Now.Date Then
                    Dim sid As String = txtID.Text.ToString


                    'Session("StudentID") = txtreType.Text
                    Session("StudentID") = dr(1).ToString
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
                    Response.Redirect("~/Student/scores.aspx")
                Else
                    lblError.Text = "Your pin has expired. Please obtain a new pin."
                End If

            Else

                lblError.Text = "You have entered an invalid admission number or pin"
            End If


            dr.Close()
            cmd.Dispose()
            con.close()        End Using
    End Sub
End Class