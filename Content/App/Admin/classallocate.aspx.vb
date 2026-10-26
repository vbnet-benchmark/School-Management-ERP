Imports System.IO
Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    

    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String

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


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles bnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim teacher As String
                Dim tisclass As String

                If cboTeacher.Text = "Select Staff" Then
                    Show_Alert(False, "Please select a staff")
                    Exit Sub
                End If
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where surname = '" & cboTeacher.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                If student0.Read Then
                    teacher = student0.Item(0).ToString
                End If
                student0.Close()


                Dim comfirm3 As New MySql.Data.MySqlClient.MySqlCommand("Select id from class where class = '" & cboClass.Text & "'", con)
                Dim rcomfirm3 As MySql.Data.MySqlClient.MySqlDataReader = comfirm3.ExecuteReader
                If rcomfirm3.Read Then
                    tisclass = rcomfirm3.Item(0).ToString
                End If
                rcomfirm3.Close()

                Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classteacher where class = '" & tisclass & "' and teacher = '" & teacher & "'", con)
                Dim student01 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01.ExecuteReader
                If student01.Read Then
                    Show_Alert(False, "Staff already allocated to this class")
                    student01.Close()
                    Exit Sub
                End If
                student01.Close()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Insert into classteacher (class, teacher) values (?,?)", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", tisclass))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", teacher))
                cmdCheck3.ExecuteNonQuery()
               
            logify.log(Session("staffid"), teacher & " was added as a class teacher in " & cboclass.Text)
            logify.Notifications("You are now the class teacher of " & cboclass.Text, teacher, Session("staffid"), "Admin", "")
            Show_Alert(True, "Class teacher added successfully")
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Not IsPostBack Then
                    Session("sender") = Request.UrlReferrer.PathAndQuery.ToString
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboTeacher.Items.Clear()
                    cboTeacher.Items.Add("Select Staff")
                    Do While student0.Read
                        cboTeacher.Items.Add(student0.Item(1).ToString)
                    Loop
                    student0.Close()


                    Dim comfirm3 As New MySql.Data.MySqlClient.MySqlCommand("Select class from class where id = '" & Request.QueryString.ToString & "'", con)
                    Dim rcomfirm3 As MySql.Data.MySqlClient.MySqlDataReader = comfirm3.ExecuteReader
                    cboclass.Items.Clear()
                    Do While rcomfirm3.Read
                        cboclass.Items.Add(rcomfirm3.Item(0))
                    Loop
                    cboclass.Enabled = False
                    rcomfirm3.Close()
                End If
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

   

    

   
    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles bnBack.Click
        Response.Redirect(Session("sender"))
    End Sub
End Class
