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


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles bnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim teacher As String = ""
                Dim subject As Integer
                Dim tisclass As Integer
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
                Dim comfirm3 As New MySql.Data.MySqlClient.MySqlCommand("Select id, type from class where class = '" & cboClass.Text & "'", con)
                Dim rcomfirm3 As MySql.Data.MySqlClient.MySqlDataReader = comfirm3.ExecuteReader
                Dim clatype As String = ""
                If rcomfirm3.Read Then
                    clatype = rcomfirm3(1).ToString
                    tisclass = rcomfirm3.Item(0).ToString
                End If
                rcomfirm3.Close()
                Dim cmdLoad02s As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classsubjects where subject = '" & subject & "' and class = '" & tisclass & "'", con)
                Dim student02s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad02s.ExecuteReader
                If student02s.Read Then
                    Show_Alert(False, "Subject already offerred in class.")
                    Exit Sub
                End If
                student02s.Close()




                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Insert into classsubjects (teacher, periods, subject, class, type) values (?,?,?,?,?)", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", teacher))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", Val(txtPeriods.Text)))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("htitle", subject))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", tisclass))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", cboType.Text))

                cmdCheck3.ExecuteNonQuery()
                logify.log(Session("staffid"), cboSubject.Text & " was added as " & IIf(cboType.Text = "Compulsory", "a ", "an ") & cboType.Text & " to " & cboClass.Text)
                logify.log(Session("staffid"), cboTeacher.Text & " was made the subject teacher of " & cboSubject.Text & " in " & cboClass.Text)
                logify.Notifications("You are now the subject teacher of " & cboSubject.Text & " - " & cboClass.Text, teacher, Session("staffid"), "Admin", "~/content/App/App/staff/staffprofile.aspx?", "")
                If clatype <> "K.G 1 Special" Then
                    If cboType.Text = "Compulsory" Then
                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM studentsummary WHERE class = ? and session = ?", con)
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Type", tisclass))
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("Sessionid")))
                        Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                        Dim students As New ArrayList
                        Dim i As Integer = 0
                        Do While reader1.Read
                            students.Add(reader1.Item("student"))
                        Loop

                        reader1.Close()
                        For Each item As String In students

                            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)

                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", tisclass))
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", item))
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subjects", subject))
                            cmd.ExecuteNonQuery()
                            logify.Notifications("You now offer " & cboSubject.Text, item, Session("staffid"), "Admin", "~/content/App/App/student/classdetails.aspx", "")
                            logify.Notifications(cboSubject.Text & " is now offerred by " & par.getstuname(item), par.getparent(item), Session("staffid"), "Admin", "~/content/App/App/parent/classdetails.aspx?" & item, "")

                        Next






                    End If

                End If

                con.close()            End Using
            txtPeriods.Text = ""
            cboSubject.Text = "Select Subject"
            cboTeacher.Text = "Select Staff"
            Show_Alert(True, "Subject Added Successfully")

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

                    Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from subjects", con)
                    Dim rcomfirm2 As MySql.Data.MySqlClient.MySqlDataReader = comfirm2.ExecuteReader
                    cboSubject.Items.Clear()
                    cboSubject.Items.Add("Select Subject")
                    Do While rcomfirm2.Read
                        cboSubject.Items.Add(rcomfirm2.Item(0))
                    Loop
                    rcomfirm2.Close()


                    Dim comfirm3 As New MySql.Data.MySqlClient.MySqlCommand("Select class from class where class = '" & Session("tisclas") & "'", con)
                    Dim rcomfirm3 As MySql.Data.MySqlClient.MySqlDataReader = comfirm3.ExecuteReader
                    cboClass.Items.Clear()
                    Do While rcomfirm3.Read
                        cboClass.Items.Add(rcomfirm3.Item(0))
                    Loop
                    cboClass.Enabled = False
                    rcomfirm3.Close()
                End If

                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

   

    

   
    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect(Session("sender"))
    End Sub
End Class
