Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_addsubject
    Inherits System.Web.UI.Page

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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If

        txtAlias.MaxLength = 3
        Try
            If Not IsPostBack Then
                Load_Subjects()

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub Load_Subjects()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim ds As New DataTable
            ds.Columns.Add("name")
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjects", con)
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim count As Integer = 1

            Do While student.Read
                ds.Rows.Add(student.Item(1).ToString)
            Loop
            student.Close()
            gridRecipients.DataSource = ds
            gridRecipients.DataBind()

            con.close()        End Using
    End Sub


    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For Each item As GridViewRow In gridRecipients.Rows
            If item.Cells(0).Text = txtSubject.Text Then
                Show_Alert(False, txtSubject.Text & " has already been added")
                Exit Sub
            End If
        Next
        If txtSubject.Text = "" Or
            txtAlias.Text = "" Then
            Show_Alert(False, "Please enter a subject to add and its alias.")
            Exit Sub
        End If
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into subjects (Subject, alias) Values (?,?)", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subct", txtAlias.Text.ToUpper))

                cmdLoad1.ExecuteNonQuery()
                con.close()            End Using
            logify.log(Session("staffid"), txtSubject.Text & " was added by admin.")
            Show_Alert(True, txtSubject.Text & " has been added")
            txtSubject.Text = ""
            txtAlias.Text = ""
            Load_Subjects()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub gridRecipients_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gridRecipients.RowDeleting
        Try


            Dim row As GridViewRow = gridRecipients.Rows(e.RowIndex)




            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjects where subject = ?", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", row.Cells(0).Text))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                student.Read()
                Dim i As Integer = student.Item(0)
                student.Close()
                Dim chk As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjectreg where SubjectsOfferred = ?", con)
                chk.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", i))
                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = chk.ExecuteReader
                If student1.Read Then
                    Show_Alert(False, "You cannot remove this subject because it is offerred.")
                    student1.Close()
                   
            Exit Sub
                End If
            student1.Close()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("Delete From subjects Where Subject = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", row.Cells(0).Text))
            cmdLoad1.ExecuteNonQuery()
            con.close()end using
            Show_Alert(True, row.Cells(0).Text().ToString & " has been removed")
            logify.log(Session("staffid"), row.Cells(0).Text().ToString & " was removed by admin")
            Load_Subjects()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub
End Class
