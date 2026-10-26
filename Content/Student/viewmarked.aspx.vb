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
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        scriptman = Me.Master.FindControl("ScriptManager1")
        scriptman.RegisterPostBackControl(LinkButton1)
        scriptman.RegisterPostBackControl(LinkButton2)
        scriptman.RegisterPostBackControl(LinkButton3)
        scriptman.RegisterPostBackControl(LinkButton4)
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Student(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

        
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()


                If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                    logify.Read_notification("~/content/student/viewmarked.aspx?" & Request.QueryString.ToString, Session("parentid"))

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class, assignments.title, submittedassignments.answer, staffprofile.surname, submittedassignments.remarks, submittedassignments.score, assignments.assignment from submittedassignments inner join (assignments inner join staffprofile on staffprofile.staffid = assignments.teacher) on submittedassignments.assignment = assignments.id inner join subjects on subjects.Id = submittedassignments.subject inner join class on class.id = submittedassignments.class where submittedassignments.id = '" & Request.QueryString.ToString & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    msg.Read()

                    txtSubject.Text = msg.Item(0)
                    txtClass.Text = msg.Item(1)
                    txtTitle.Text = msg.Item(2)
                    txtRem.Text = msg.Item(5)
                    txtScore.Text = msg.Item(6)
                    Dim lk As New Literal
                    lk.Text = msg.Item(3).ToString
                    plcAnswer.Controls.Add(lk)
                    Dim lkj As New Literal
                    lkj.Text = msg.Item(7).ToString
                    plcQuestion.Controls.Add(lkj)
                    msg.Close()

                    txtSubject.Enabled = False
                    txtClass.Enabled = False
                    txtTitle.Enabled = False
                    txtRem.Enabled = False
                    txtScore.Enabled = False
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select file, fileicon from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                    Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                    Dim c As Integer = 1
                    Do While msg2.Read
                        Dim imagesrc As New fileimage
                        If c = 1 Then
                            Panel1.Visible = True
                            icon1.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton1.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic1") = msg2.Item(0)
                        ElseIf c = 2 Then

                            icon2.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton2.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic2") = msg2.Item(0)
                        ElseIf c = 3 Then
                            icon3.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton3.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic3") = msg2.Item(0)
                        ElseIf c = 4 Then
                            icon4.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton4.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic4") = msg2.Item(0)
                        End If
                        c = c + 1
                    Loop
                    msg2.Close()
                End If
                con.Close()            End Using
        Catch ex As Exception
            If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Response.Redirect("~/content/student/markedassignments.aspx")


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

    

    
End Class
