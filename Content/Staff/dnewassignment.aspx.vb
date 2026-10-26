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
    Dim par As New parentcheck
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
        If check.Check_dh(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()


                If Request.QueryString.ToString <> Nothing And Not IsPostBack Then

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class, assignments.title, assignments.assignment, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.id = '" & Request.QueryString.ToString & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    msg.Read()
                    cboSubject.Text = msg.Item(0)
                    cboClass.Text = msg.Item(1)
                    txtSubject.Text = msg.Item(2)
                    FreeTextBox1.Text = msg.Item(3).ToString
                    Dim dob As String = msg.Item(4).ToString
                    txtDeadline.Text = dob
                    txtDeadline.Enabled = False
                    txtSubject.Enabled = False
                    msg.Close()
                    cboSubject.Enabled = False
                    cboClass.Enabled = False
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select file, fileicon from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                    Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                    Dim c As Integer = 1
                    Do While msg2.Read
                        Dim imagesrc As New fileimage
                        Panel1.Visible = True
                        If c = 1 Then
                            icon1.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton1.Text = Path.GetFileName(msg2.Item(0).ToString)
                            Session("pic1") = msg2.Item(0)
                        ElseIf c = 2 Then
                            icon2.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton2.Text = Path.GetFileName(msg2.Item(0).ToString)
                            Session("pic2") = msg2.Item(0)
                        ElseIf c = 3 Then
                            icon3.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton3.Text = Path.GetFileName(msg2.Item(0).ToString)
                            Session("pic3") = msg2.Item(0)
                        ElseIf c = 4 Then
                            icon4.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton4.Text = Path.GetFileName(msg2.Item(0).ToString)
                            Session("pic4") = msg2.Item(0)
                        End If
                        c = c + 1

                    Loop
                    msg2.Close()
                End If
                FreeTextBox1.Focus = True
                FreeTextBox1.Toolbars.Clear()
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
        Response.Redirect("~/content/staff/dassignments.aspx")
    End Sub
End Class
