Imports System.IO
Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    
    Dim group As Integer
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
                Dim subject As String
                Dim tisclass As String
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where surname = '" & cboTeacher.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                If student0.Read Then
                    teacher = student0.Item(0).ToString
                End If
                student0.Close()

                Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjects where subject = '" & cboSubject.Text & "'", con)
                Dim student01 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01.ExecuteReader
                If student01.Read Then
                    subject = student01.Item(0).ToString
                End If
                student01.Close()

                Dim comfirm3 As New MySql.Data.MySqlClient.MySqlCommand("Select id from class where class = '" & cboClass.Text & "'", con)
                Dim rcomfirm3 As MySql.Data.MySqlClient.MySqlDataReader = comfirm3.ExecuteReader
                If rcomfirm3.Read Then
                    tisclass = rcomfirm3.Item(0).ToString
                End If
                rcomfirm3.Close()

                Dim comfirm3s As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.teacher, classsubjects.periods from classsubjects inner join class on class.id = classsubjects.class inner join subjects on subjects.id = classsubjects.subject where class.class = '" & Session("tisclas") & "' and subjects.subject = '" & RTrim(Replace(Request.QueryString.ToString, "+", " ")) & "'", con)
                Dim rcomfirm3s As MySql.Data.MySqlClient.MySqlDataReader = comfirm3s.ExecuteReader
                rcomfirm3s.Read()
                Dim formerteacher As String = rcomfirm3s(0).ToString
                Dim formerperiods As String = rcomfirm3s(1).ToString
                rcomfirm3s.Close()


                rcomfirm3s.Close()

                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects Set teacher = ?, periods = ? Where subject = ? and class = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", teacher))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", Val(txtPeriods.Text)))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("htitle", subject))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", tisclass))
                cmdCheck3.ExecuteNonQuery()
                If teacher <> formerteacher Then
                    logify.Notifications("You are now the subject teacher of " & cboSubject.Text & " - " & cboClass.Text, teacher, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                    If formerteacher <> "" Then logify.Notifications("You are no more the subject teacher of " & cboSubject.Text & " - " & cboClass.Text, formerteacher, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                End If
                logify.log(Session("staffid"), cboClass.Text & " - " & cboSubject.Text & " was updated")
                Dim cmdCheck3a As New MySql.Data.MySqlClient.MySqlCommand("Update timetable Set teacher = ? Where subject = ? and class = ?", con)
                cmdCheck3a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", teacher))
                cmdCheck3a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("htitle", subject))
                cmdCheck3a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", tisclass))
                cmdCheck3a.ExecuteNonQuery()

                Dim comfirm5 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.periods, class.class, staffprofile.surname, classsubjects.sgroup from classsubjects  left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject inner join class on classsubjects.class = class.id where class.id = '" & Replace(tisclass, "?", "") & "' and subjects.subject = '" & RTrim(Replace(Request.QueryString.ToString, "+", " ")) & "'", con)
                Dim rcomfirm5 As MySql.Data.MySqlClient.MySqlDataReader = comfirm5.ExecuteReader
                If rcomfirm5.Read Then
                    group = rcomfirm5.Item(3).ToString
                End If
                rcomfirm5.Close()
                If group <> 0 Then
                    Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects Set periods = ? Where sgroup = ? and class = ?", con)
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", Val(txtPeriods.Text)))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("htitle", group))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", tisclass))
                    cmdCheck5.ExecuteNonQuery()
                End If
                con.close()            End Using
            Show_Alert(True, "Updated Successfully. Time table may need to be adjusted.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub





    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Not IsPostBack Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where activated = '" & 1 & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboTeacher.Items.Clear()
                    cboTeacher.Items.Add("Select Staff")
                    Do While student0.Read
                        cboTeacher.Items.Add(student0.Item(1).ToString)
                    Loop
                    student0.Close()

                    Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where class.class = '" & Session("tisclas") & "'", con)
                    Dim rcomfirm2 As MySql.Data.MySqlClient.MySqlDataReader = comfirm2.ExecuteReader
                    cboSubject.Items.Clear()
                    cboSubject.Items.Add("Select Subject")
                    Do While rcomfirm2.Read
                        cboSubject.Items.Add(rcomfirm2.Item(0))
                    Loop
                    rcomfirm2.Close()


                    Dim comfirm3 As New MySql.Data.MySqlClient.MySqlCommand("Select class from class", con)
                    Dim rcomfirm3 As MySql.Data.MySqlClient.MySqlDataReader = comfirm3.ExecuteReader
                    cboClass.Items.Clear()
                    Do While rcomfirm3.Read
                        cboClass.Items.Add(rcomfirm3.Item(0))
                    Loop
                    cboClass.Enabled = False
                    rcomfirm3.Close()
                End If
                If Not IsPostBack And Request.QueryString.ToString <> Nothing Then
                    Dim tisclass As String = Request.UrlReferrer.Query.ToString
                    Session("sender") = Request.UrlReferrer.PathAndQuery.ToString
                    cboSubject.Text = Replace(Request.QueryString.ToString, "+", " ")
                    Dim comfirm3 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.periods, class.class, staffprofile.surname, classsubjects.sgroup from classsubjects  left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject inner join class on classsubjects.class = class.id where class.class = '" & Session("tisclas") & "' and subjects.subject = '" & RTrim(Replace(Request.QueryString.ToString, "+", " ")) & "'", con)
                    Dim rcomfirm3 As MySql.Data.MySqlClient.MySqlDataReader = comfirm3.ExecuteReader
                    If rcomfirm3.Read Then
                        txtPeriods.Text = rcomfirm3.Item(0).ToString
                        cboClass.Text = rcomfirm3(1).ToString
                        cboTeacher.Text = rcomfirm3.Item(2).ToString
                        rcomfirm3.Close()
                    End If


                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

   

    

   
    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles bnBack.Click
        Response.Redirect(Session("sender"))
    End Sub
End Class
