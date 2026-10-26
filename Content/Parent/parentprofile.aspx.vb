Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls



Partial Class Admin_staffprofile
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList

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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Parent(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
        If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentid as 'Parent ID', parentname as 'Name', sex as Sex, phone as 'Phone No', address as 'Address', email as 'Email' from parentprofile where parentid = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("ParentID")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    Dim pass As String

                    adapter1.SelectCommand = cmdLoad1
                    adapter1.Fill(ds)
                    DetailsView1.DataSource = ds
                    DetailsView1.DataBind()
                    Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from parentprofile where parentid = ?", con)
                    cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("ParentID")))
                    Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader

                    student2.Read()
                    pass = student2.Item("passport").ToString
                    student2.Close()
                    If pass = "" Then
                        pass = "~/image/noimage.jpg"
                    End If
                    Image1.ImageUrl = pass


                    con.close()                End Using
                roles()
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Private Sub roles()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno WHERE parentward.parent = ?", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim count As Integer = 1
            Dim mylit2 As New LiteralControl
            Do While reader2.Read
                Dim subjects As New Label
                If FindControl("cbosubject" & count) Is Nothing Then
                    subjects.ID = "cbosubject" & count
                End If
                mylit2.Text = "<p>"
                taught.Controls.Add(mylit2)
                subjects.Text = count & ".   " & reader2.Item(0)
                taught.Controls.Add(subjects)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "</p>"
                taught.Controls.Add(MyLiteral)
                count = count + 1
            Loop
            reader2.Close()


            con.close()        End Using
    End Sub
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Try
            Panel1.Visible = True
            roles()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If txtPassword.Text <> txtPassword0.Text Then
                    Show_Alert(False, "Your password entries do not match")
                ElseIf txtPassword.Text = "" Or txtPassword0.Text = "" Then
                    Show_Alert(False, "Password field is blank")
                Else
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update parentProfile Set password = ? where parentID = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", txtPassword.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("ParentID")))
                    cmdCheck3.ExecuteNonQuery()
                    Show_Alert(True, "Password updated successfully")

                    Panel1.Visible = False
                End If
                con.close()            End Using
            roles()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkEdit_Click(sender As Object, e As EventArgs) Handles lnkEdit.Click
        Session("ParentAdd") = Session("Parentid")
        Session("edit") = "teacherprofile"
        Response.Redirect("~/content/parent/editprofile.aspx")
    End Sub

    Protected Sub lnkpassport_Click(sender As Object, e As EventArgs) Handles lnkpassport.Click
        Session("ParentAdd") = Session("Parentid")
        Session("edit") = "passport"
        Response.Redirect("~/content/parent/editprofile.aspx")
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Panel1.Visible = False
            roles()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
