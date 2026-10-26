Imports System.Text
Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_adminpage
    Inherits System.Web.UI.Page
    Public Shared receiver As String
    Public Shared recCategory As String

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
        If Session("staffid") = Nothing Then 
 Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password")) 
 If x.Count <> 0 Then 
 Session("staffid") = x.Item(0) 
 Session("sessionid") = x.Item(1) 
 End If 
 End If 
        Try

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                    logify.Read_notification("~/content/App/App/parent/viewassignment.aspx?" & Request.QueryString.ToString, Session("parentid"))

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class, assignments.title, assignments.assignment, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.id = '" & Request.QueryString.ToString & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    msg.Read()
                    txtSubject.Text = msg.Item(0)
                    txtTitle.Text = msg.Item(2)
                    FreeTextBox1.Text = msg.Item(3).ToString
                    Dim dob As String = msg.Item(4).ToString
                    txtDeadline.Text = dob
                    msg.Close()
                    txtSubject.Enabled = False
                    txtTitle.Enabled = False
                    txtDeadline.Enabled = False
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select file, fileicon from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                    Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                    Dim c As Integer = 1
                    Do While msg2.Read
                        Dim imagesrc As New fileimage
                        Panel1.Visible = True
                        If c = 1 Then
                            icon1.Src = imagesrc.get_image(msg2(1).ToString)
                            del1.Text = "Delete"
                            LinkButton1.Text = Path.GetFileName(msg2.Item(0).ToString)
                            Session("pic1") = msg2.Item(0)
                        ElseIf c = 2 Then
                            icon2.Src = imagesrc.get_image(msg2(1).ToString)
                            del2.Text = "Delete"
                            LinkButton2.Text = Path.GetFileName(msg2.Item(0).ToString)
                            Session("pic2") = msg2.Item(0)
                        ElseIf c = 3 Then
                            icon3.Src = imagesrc.get_image(msg2(1).ToString)
                            del3.Text = "Delete"
                            LinkButton3.Text = Path.GetFileName(msg2.Item(0).ToString)
                            Session("pic3") = msg2.Item(0)
                        ElseIf c = 4 Then
                            icon4.Src = imagesrc.get_image(msg2(1).ToString)
                            del4.Text = "Delete"
                            LinkButton4.Text = Path.GetFileName(msg2.Item(0).ToString)
                            Session("pic4") = msg2.Item(0)
                        End If
                        c = c + 1

                    Loop
                    msg2.Close()
                End If
                FreeTextBox1.Focus = True
                FreeTextBox1.Toolbars.Clear()
                FreeTextBox1.ReadOnly = True
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

  
    
   
   

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect(Session("pic1"))
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Response.Redirect(Session("pic2"))
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        Response.Redirect(Session("pic3"))

    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Response.Redirect(Session("pic4"))

    End Sub

    
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("~/content/App/App/parent/assignments.aspx")

    End Sub
End Class
