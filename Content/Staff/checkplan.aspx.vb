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
    Dim templates As New Lesson_Plan
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
        scriptman.RegisterPostBackControl(btnSend)
        scriptman.RegisterPostBackControl(btnUpload)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_dh(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

        
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()


                If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                    logify.Read_notification("~/content/staff/checkplan.aspx?" & Request.QueryString.ToString, Session("staffid"))

                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT lessonplan.week, staffprofile.surname, subjects.subject, class.class, lessonplan.plan, lessonplan.remarks, staffprofile.staffid from lessonplan inner join staffprofile on staffprofile.staffid = lessonplan.teacher inner join class on lessonplan.class = class.id inner join subjects on lessonplan.subject = subjects.id where lessonplan.id  = '" & Request.QueryString.ToString & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    msg.Read()
                    txtTeacher.Text = msg.Item(1)
                    txtSubject.Text = msg.Item(2)
                    txtClass.Text = msg.Item(3)
                    txtWeek.Text = msg.Item(0)
                    txtRem.Text = msg.Item(5)
                    Session("lessonplansender") = msg(6)
                    Hidden1.Value = msg.Item(4).ToString
                    msg.Close()
                    txtTeacher.Enabled = False
                    txtSubject.Enabled = False
                    txtClass.Enabled = False
                    txtWeek.Enabled = False
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
                            del1.Text = "Delete"
                        ElseIf c = 2 Then

                            icon2.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton2.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic2") = msg2.Item(0)
                            del2.Text = "Delete"
                        ElseIf c = 3 Then
                            icon3.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton3.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic3") = msg2.Item(0)
                            del3.Text = "Delete"
                        ElseIf c = 4 Then
                            icon4.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton4.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic4") = msg2.Item(0)
                            del4.Text = "Delete"
                        End If
                        c = c + 1
                    Loop
                    msg2.Close()
                End If
                
                con.Close()                Dim l As New Literal
                l = Me.Master.FindControl("summerLit")
                l.Text = templates.Get_Js(Hidden1)

                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "this4", l.Text, False)            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            Dim message As String = Hidden1.Value
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim cmdCheck9 As New MySql.Data.MySqlClient.MySqlCommand("Delete from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                cmdCheck9.ExecuteNonQuery()
                Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Update lessonplan set plan = ?, date = ?, remarks = ?, status = ? where Id = '" & Request.QueryString.ToString & "'", con)
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("assignment", Hidden1.Value))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("deadline", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("title", txtRem.Text))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("assignfment", "Marked"))
                cmdCheck20.ExecuteNonQuery()
                
                If LinkButton1.Text <> "" Then

                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", Request.QueryString.ToString))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic1")))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att1.Text))

                    cmdCheck.ExecuteNonQuery()
                End If
                If LinkButton2.Text <> "" Then
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", Request.QueryString.ToString))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic2")))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att2.Text))
                    cmdCheck.ExecuteNonQuery()
                End If
                If LinkButton3.Text <> "" Then
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", Request.QueryString.ToString))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic3")))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att3.Text))
                    cmdCheck.ExecuteNonQuery()
                End If
                If LinkButton4.Text <> "" Then
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", Request.QueryString.ToString))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic4")))
                    cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att4.Text))
                    cmdCheck.ExecuteNonQuery()
                End If
            logify.log(Session("staffid"), "Lesson plan of " & par.getstaff(Session("lessonplansender")) & "was marked")
                logify.Notifications("Your lesson plan for Week " & txtWeek.Text & " " & txtSubject.Text & " - " & txtClass.Text & " has been marked", Session("lessonplansender"), Session("staffid"), "HOD", "~/content/staff/newplan.aspx?" & Request.QueryString.ToString, "")
                Show_Alert(True, "Lesson Plan marked successfully")
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Try
            Dim folderPath As String = Server.MapPath("~/Content/Uploads/")
            Dim q As String
            If FileUpload1.HasFile Then
                FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                q = "https://" & Request.Url.Authority & "/Content/Uploads/" & FileUpload1.FileName
                Dim picsource As New fileimage
                If LinkButton1.Text = "" Then
                    att1.Text = FileUpload1.PostedFile.ContentType
                    icon1.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton1.Text = Path.GetFileName(q)
                    Session("pic1") = q
                    Panel1.Visible = True
                    del1.Text = "Delete"
                ElseIf LinkButton2.Text = "" Then
                    att2.Text = FileUpload1.PostedFile.ContentType
                    icon2.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton2.Text = Path.GetFileName(q)
                    Session("pic2") = q
                    del2.Text = "Delete"

                ElseIf LinkButton3.Text = "" Then
                    att3.Text = FileUpload1.PostedFile.ContentType
                    icon3.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton3.Text = Path.GetFileName(q)
                    Session("pic3") = q
                    del3.Text = "Delete"

                ElseIf LinkButton4.Text = "" Then
                    att4.Text = FileUpload1.PostedFile.ContentType
                    icon4.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton4.Text = Path.GetFileName(q)
                    Session("pic4") = q
                    del4.Text = "Delete"

                Else
                    Show_Alert(False, "You cannot add more than 4 attachments.")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
   
    Protected Sub del1_Click(sender As Object, e As EventArgs) Handles del1.Click
        LinkButton1.Text = ""
        icon1.Src = ""
        del1.Text = ""
        Session("pic1") = Nothing
        If LinkButton2.Text <> "" Then
            LinkButton1.Text = LinkButton2.Text
            icon1.Src = icon2.Src
            att1.text = att2.text
            del1.Text = "Delete"
            LinkButton2.Text = ""
            icon2.Src = ""
            del2.Text = ""
            Session("pic1") = Session("pic2")
            Session("pic2") = Nothing
        End If
        If LinkButton3.Text <> "" Then
            LinkButton2.Text = LinkButton3.Text
            icon2.Src = icon3.Src
            att2.text = att3.text
            del2.Text = "Delete"
            LinkButton3.Text = ""
            icon3.Src = ""
            del3.Text = ""
            Session("pic2") = Session("pic3")
            Session("pic3") = Nothing
        End If
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.text = att4.text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
        If LinkButton1.Text = "" Then
            Panel1.Visible = False
        End If
    End Sub

    Protected Sub del2_Click(sender As Object, e As EventArgs) Handles del2.Click
        LinkButton2.Text = ""
        icon2.Src = ""
        del2.Text = ""
        Session("pic2") = Nothing
        If LinkButton3.Text <> "" Then
            LinkButton2.Text = LinkButton3.Text
            att2.Text = att3.Text
            icon2.Src = icon3.Src
            del2.Text = "Delete"
            LinkButton3.Text = ""
            icon3.Src = ""
            del3.Text = ""
            Session("pic2") = Session("pic3")
            Session("pic3") = Nothing
        End If
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.Text = att4.Text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
    End Sub

    Protected Sub del3_Click(sender As Object, e As EventArgs) Handles del3.Click
        LinkButton3.Text = ""
        icon3.Src = ""
        del3.Text = ""
        Session("pic3") = Nothing
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.Text = att4.Text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
    End Sub

    Protected Sub del4_Click(sender As Object, e As EventArgs) Handles del4.Click
        LinkButton4.Text = ""
        icon4.Src = ""
        del4.Text = ""
        Session("pic4") = Nothing
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
        Response.Redirect("~/content/staff/submittedplans.aspx")
    End Sub
End Class
