Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources


Partial Class Admin_adminpage
    Inherits System.Web.UI.Page

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
    Private Sub update_class(cla As Integer)

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim subjects As New ArrayList
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subject FROM ClassSubjects WHERE  class = '" & cla & "'", con)
                Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Do While reader2a.Read
                    subjects.Add(reader2a(0))
                Loop
                reader2a.Close()
                For Each subject As Integer In subjects
                    Dim Saverage As New MySql.Data.MySqlClient.MySqlCommand("SELECT Total, student FROM SubjectReg WHERE Session = ? AND Class = ? AND subjectsOfferred = ? Order by total Desc", con)
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjectsOffered", subject))
                    Dim readerT As MySql.Data.MySqlClient.MySqlDataReader = Saverage.ExecuteReader
                    Dim STotal As Double = 0
                    Dim SubjectAverage As Double = 0
                    Dim positionz As String = ""
                    Dim pos As New ArrayList
                    Dim posid As New ArrayList
                    Dim count As Integer = 0
                    Dim highest1 As Double = 0
                    Dim lowest1 As Double = 0
                    Dim y As Integer
                    Do While readerT.Read
                        count = count + 1
                        If count = 1 Then
                            highest1 = Val(readerT.Item("Total"))
                        End If
                        positionz = count
                        Select Case positionz
                            Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                                positionz = positionz.ToString + "st"
                            Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                                positionz = positionz.ToString + "nd"
                            Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                                positionz = positionz.ToString + "rd"
                            Case Else
                                positionz = positionz.ToString + "th"
                        End Select
                        posid.Add(readerT.Item("student"))
                        pos.Add(positionz)
                        lowest1 = Val(readerT.Item("Total"))
                        STotal = STotal + Val(readerT.Item("Total"))
                    Loop
                    SubjectAverage = FormatNumber(STotal / count, 2)
                    readerT.Close()
                    For y = 0 To posid.Count - 1
                        Dim Updatedatabase2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE SubjectReg SET avg = ?, Highest = ?, Lowest = ?, pos = ? WHERE Student = ? and SubjectsOfferred = ? and Session = ?", con)
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Average", SubjectAverage))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.highest", highest1))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.lowest", lowest1))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.position", pos(y)))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ID", posid(y)))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.subject", subject))
                        Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.session", Session("sessionid")))
                        Updatedatabase2.ExecuteNonQuery()
                    Next
                    count = Nothing
                Next




                Dim studentID As String
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM SubjectReg WHERE Student = ? And Session = ? And Class = ?", con)
                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM StudentSummary WHERE Class = ? And Session = ?", con)
                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader

                Dim i As Integer = 0
                Dim students As New ArrayList
                Do While reader3.Read
                    students.Add(reader3.Item("student"))
                Loop
                reader3.Close()
                Dim gtotals As New ArrayList

                Dim ct As Integer = 0
                For Each item As String In students
                    studentID = item

                    Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", studentID))
                    Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim param5 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    Dim readerf As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
                    Dim total As Integer = 0
                    Dim average As Double = 0
                    Dim count As Integer = 0
                    Do While readerf.Read()
                        count = count + 1
                        total = total + Val(readerf.Item("Total"))
                    Loop
                    gtotals.Add(total)
                    readerf.Close()
                    average = total / count

                    average = FormatNumber(average, 2)

                    Dim cmd22 As New MySql.Data.MySqlClient.MySqlCommand("SELECT grades.lowest From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & cla & "' order by grades.lowest desc", con)
                    Dim graderead As MySql.Data.MySqlClient.MySqlDataReader = cmd22.ExecuteReader
                    Dim remarks As String = ""
                    Do While graderead.Read
                        If average >= Val(graderead.Item(0)) Then

                            Exit Do
                        End If
                    Loop
                    graderead.Close()

                    Dim cmd2c As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Average = ?, principalremarks = ? WHERE student = ? And Session = ? And Class = ?", con)
                    Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Average", average))
                    Dim paramq As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ave", remarks))
                    Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentID))
                    Dim param6 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim param7 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))

                    cmd2c.ExecuteNonQuery()



                    cmd2c.Parameters.Remove(paramq)
                    cmd.Parameters.Remove(param)
                    cmd2c.Parameters.Remove(param2)
                    cmd2c.Parameters.Remove(param3)
                    cmd.Parameters.Remove(param4)
                    cmd.Parameters.Remove(param5)
                    cmd2c.Parameters.Remove(param6)
                    cmd2c.Parameters.Remove(param7)
                    total = Nothing
                    count = Nothing
                    ct = ct + 1

                Next
                gtotals.Sort()

                Dim cmd2x As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET classhigh = '" & gtotals(ct - 1) & "', classlow = '" & gtotals(0) & "' where Session = ? And Class = ?", con)
                Dim param6aaa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                Dim param7aa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmd2x.ExecuteNonQuery()
                Dim positionNo As Integer
                Dim position As String

                Dim cmdxx As New MySql.Data.MySqlClient.MySqlCommand("SELECT Average, student from StudentSummary WHERE Session = ? And Class = ? ORDER BY Average DESC", con)
                Dim cmd2xx As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Position = ? WHERE student = ? And Session = ? And Class = ?", con)
                cmdxx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                cmdxx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdxx.ExecuteReader
                Dim ix As Integer = 0
                Dim studentsx As New ArrayList
                Do While reader.Read
                    studentsx.Add(reader.Item("student"))
                Loop
                reader.Close()
                For Each item As String In studentsx
                    positionNo = positionNo + 1
                    Select Case positionNo
                        Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                            If positionNo = 1 Then
                            End If
                            position = positionNo.ToString + "st"
                        Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                            position = positionNo.ToString + "nd"
                        Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                            position = positionNo.ToString + "rd"
                        Case Else
                            position = positionNo.ToString + "th"
                    End Select
                    Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Position", position))
                    Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", item))
                    Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmd2xx.ExecuteNonQuery()
                    cmd2xx.Parameters.Remove(param)
                    cmd2xx.Parameters.Remove(param2)
                    cmd2xx.Parameters.Remove(param3)
                    cmd2xx.Parameters.Remove(param4)
                Next
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log("update class", Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        scriptman = Me.Master.FindControl("ScriptManager1")
        scriptman.RegisterPostBackControl(Button5)
        If Wizard1.ActiveStep Is WizardStep1 Or Wizard1.ActiveStep Is WizardStep2 Or Wizard1.ActiveStep Is WizardStep3 Then scriptman.RegisterPostBackControl(Wizard1)
        scriptman.RegisterPostBackControl(Button2)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Class(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

            If Not IsPostBack Then



                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.open()


                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from hostel Order by Id", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboHostel.Items.Clear()
                    cboHostel.Items.Add("Select Hostel")
                    Do While student0.Read
                        cboHostel.Items.Add(student0.Item(0).ToString)
                    Loop
                    student0.Close()

                    Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees Order by Id", con)
                    Dim student01 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01.ExecuteReader
                    cboTransport.Items.Clear()
                    cboTransport.Items.Add("Select Route")
                    Do While student01.Read
                        cboTransport.Items.Add(student01.Item(0).ToString)
                    Loop
                    student01.Close()

                    Dim cmd40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from options", con)
                    Dim reader40 As MySql.Data.MySqlClient.MySqlDataReader = cmd40.ExecuteReader
                    reader40.Read()
                    Dim board As Boolean = reader40.Item(0)
                    Dim trans As Boolean = reader40.Item(1)
                    reader40.Close()

                    If board = True Then
                        CheckBox1.Visible = True
                        cboHostel.Visible = True
                    End If
                    If trans = True Then
                        CheckBox2.Visible = True
                        cboTransport.Visible = True
                    End If
                    con.close()
                End Using

            Else

            End If
            If Session("edit") = Nothing Then

            Else
                enter_values()
                If Session("edit") = "studentprofile" Then
                    Wizard1.ActiveStepIndex = 0
                    Wizard1.StartNextButtonText = "SAVE"
                    Wizard1.StepPreviousButtonText = "BACK"
                ElseIf Session("edit") = "passport" Then
                    Wizard1.ActiveStepIndex = 1
                    Wizard1.StepNextButtonText = "SAVE"
                    Wizard1.StepPreviousButtonText = "BACK"
                ElseIf Session("edit") = "Changeclass" Then
                    Wizard1.ActiveStepIndex = 3
                    Wizard1.StepNextButtonText = "SAVE"
                    Wizard1.StepPreviousButtonText = "BACK"
                ElseIf Session("edit") = "parent" Then
                    Wizard1.ActiveStepIndex = 4
                    Wizard1.FinishCompleteButtonText = "SAVE"
                    Wizard1.FinishPreviousButtonText = "BACK"
                End If



            End If
            Session("edit") = Nothing
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub enter_values()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()


            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsprofile where admno = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            student.Read()
            txtID.Text = student.Item(0)

            txtSurname.Text = student.Item(1)
            txtPhone.Text = student.Item(6)
            cboSex.Text = student.Item(2)
            Try
                Dim dob As String = student.Item(3).ToString
                datepicker1.Text = dob
            Catch
            End Try
            student.Close()
            con.close()
        End Using

    End Sub


    Protected Sub Wizard1_FinishButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick
        Try
            Class_Reg()
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                If txtExistPPhone.Text <> "" Then
                    Dim comfirms As New MySql.Data.MySqlClient.MySqlCommand("Select * from parentprofile where phone = ?", con)
                    comfirms.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtExistPPhone.Text))
                    Dim rcomfirms As MySql.Data.MySqlClient.MySqlDataReader = comfirms.ExecuteReader
                    rcomfirms.Read()
                    If Not rcomfirms.HasRows Then
                        Show_Alert(False, "Parent does not exist. Check the Phone number or add new parent.")
                        e.Cancel = True
                        Exit Sub
                    Else
                        Dim pid As String = rcomfirms(0)
                        rcomfirms.Close()
                        Dim comfirms2 As New MySql.Data.MySqlClient.MySqlCommand("Select * from parentward where ward = ?", con)
                        comfirms2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("Studentadd")))
                        Dim rcomfirms2 As MySql.Data.MySqlClient.MySqlDataReader = comfirms2.ExecuteReader
                        Dim hasguardian As Boolean = False
                        If rcomfirms2.Read Then
                            hasguardian = True
                        End If
                        rcomfirms2.Close()
                        If hasguardian = False Then
                            Dim cmdCheck2a As New MySql.Data.MySqlClient.MySqlCommand("Insert Into parentward (ward, parent) values (?,?)", con)
                            cmdCheck2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("Studentadd")))
                            cmdCheck2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", pid))
                            cmdCheck2a.ExecuteNonQuery()
                            logify.log(Session("staffid"), txtPName.Text & " was made the parent of " & txtSurname.Text)
                            logify.Notifications("You have been made the parent of " & txtSurname.Text, txtPID.Text, Session("staffid"), "Class Teacher", "~/content/parent/studentprofile.aspx", "")
                        Else
                            Dim cmdCheck2a As New MySql.Data.MySqlClient.MySqlCommand("Update parentward set parent = ? where ward = ?", con)
                            cmdCheck2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", pid))
                            cmdCheck2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("Studentadd")))
                            cmdCheck2a.ExecuteNonQuery()
                            logify.log(Session("staffid"), txtPName.Text & " is now the parent of " & txtSurname.Text)
                            logify.Notifications("You have been made the parent of " & txtSurname.Text, txtPID.Text, Session("staffid"), "Class Teacher", "~/content/parent/studentprofile.aspx", "")
                        End If
                    End If
                Else
                    If txtPID.Text = "" Then
                        Show_Alert(False, "Please add parent information.")
                        e.Cancel = True
                        Exit Sub
                    End If
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from parentprofile where parentID = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtPID.Text))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader

                    If rcomfirm.Read Then
                        rcomfirm.Close()
                        Show_Alert(False, "Id already exists.")
                        e.Cancel = True
                        Exit Sub
                    Else
                        rcomfirm.Close()
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into parentprofile (parentID, parentName, sex, address, email, phone, passport) values (?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtPID.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtPName.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboPSex.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("add", txtPAddress.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtPEmail.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPPhone.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phe", txtPass.Text))
                        cmdCheck2.ExecuteNonQuery()
                        logify.log(Session("staffid"), txtPName.Text & " was added as a parent. Parent ID = " & txtPID.Text)
                        Dim cmdCheck2a As New MySql.Data.MySqlClient.MySqlCommand("Insert Into parentward (ward, parent) values (?,?)", con)
                        cmdCheck2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("studentadd")))
                        cmdCheck2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtPID.Text))
                        cmdCheck2a.ExecuteNonQuery()
                        logify.log(Session("staffid"), txtPName.Text & " was made the parent of " & txtSurname.Text)
                        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp, smsapi, smsname, logo from options", con)
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                        reader3.Read()
                        Dim email As String = reader3(0).ToString
                        Dim password As String = reader3(1).ToString
                        Dim port As String = reader3(2).ToString
                        Dim smtp As String = reader3(3).ToString
                        Dim smsapi As String = reader3(4).ToString
                        Dim smsname As String = reader3(5).ToString
                        Dim logo As String = reader3(6).ToString
                        reader3.Close()

                        Dim smtpClient As SmtpClient = New SmtpClient(smtp, port)
                        smtpClient.EnableSsl = True
                        smtpClient.Credentials = New System.Net.NetworkCredential(email, password)
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
                        Dim mailMessage As MailMessage = New MailMessage(email, txtPEmail.Text)
                        Dim path As String = "https://" & Request.Url.Authority
                        mailMessage.IsBodyHtml = True
                        mailMessage.Body = String.Format("<img alt='' style='width:370px; height:90px;' src='" & path & Replace(logo, "~", "") & "' /> <br/> You have been registerred as a parent in " & smsname & ". Your login details are shown below - <br/> User Name : " & txtPID.Text & " <br/> Password: password <br/> Please login <a href='" + path + "'>here </a> and change your password from the default password. <br/> Trough the portal, you can access every information about your children/wards. Click <a href='" + path + "'>here </a> to login.")
                        mailMessage.Subject = "Welcome to " & smsname
                        Try
                            smtpClient.Send(mailMessage)
                        Catch ex As Exception

                        End Try


                    End If
                End If
                con.Close()
            End Using

            Show_Alert(True, "Here")
            Exit Sub

            Response.Redirect("~/content/staff/studentprofile.aspx")


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Private Sub add_traits()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT traits.id from traits inner join class on class.traitgroup = traits.traitgroup where traits.used = '" & 1 & "' and class.class = '" & Session("classname") & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            Dim traits As New ArrayList
            Do While student03.Read()
                traits.Add(student03(0).ToString)
            Loop
            student03.Close()
            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("delete from termtraits where student = '" & Session("studentadd") & "' and session = '" & Session("sessionid") & "'", con)
            cmdInsert22.ExecuteNonQuery()
            For Each trait As String In traits
                Dim cmdInsert22a As New MySql.Data.MySqlClient.MySqlCommand("insert into termtraits (session, student, trait) values ('" & Session("sessionid") & "', '" & Session("studentadd") & "', '" & trait & "')", con)
                cmdInsert22a.ExecuteNonQuery()
            Next

            con.close()
        End Using

    End Sub
    Protected Sub Average_Age()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, type from class where class = ?", con)
            cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("classname")))
            Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
            student04.Read()
            Dim cla As Integer = student04.Item(0)
            Dim clatype As String = student04(1).ToString
            student04.Close()

            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT age FROM StudentSummary WHERE class = ?", con)
            cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", cla))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
            Dim average As Integer
            Dim total As Integer
            Dim count As Integer
            Do While reader2.Read()
                total = total + reader2.Item("age")
                count = count + 1
            Loop
            average = total \ count

            reader2.Close()

            Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
            cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2a.ExecuteReader
            reader2a.Read()
            Dim smsno As Integer = reader2a(0)
            reader2a.Close()
            Dim cmdInsert2a As New MySql.Data.MySqlClient.MySqlCommand("Update session set smsno = ? where id = ?", con)
            cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", smsno + 35))
            cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", Session("Sessionid")))
            cmdInsert2a.ExecuteNonQuery()


            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set Aveage = ?, ClassNo = ? where class = ?", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", average))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", count))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            cmdInsert2.ExecuteNonQuery()
            If clatype <> "K.G 1 Special" Then update_class(cla)
            con.Close()
        End Using
    End Sub

    Protected Sub Wizard1_NextButtonClick(sender As Object, e As WizardNavigationEventArgs)
        Try
            If Wizard1.ActiveStep Is WizardStep1 Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.Open()
                    Dim a As Array = Split(datepicker1.Text, "/")
                    If datepicker1.Text = Nothing Then
                        Show_Alert(False, "Please select a valid Date of Birth")
                        e.Cancel = True
                        Exit Sub
                    ElseIf a(0).ToString.Count <> 2 Or a(1).ToString.Count <> 2 Or a(2).ToString.Count <> 4 Then
                        Show_Alert(False, "Date of Birth not in the correct format. Use the Datepicker tool")
                        e.Cancel = True
                        Exit Sub
                    End If
                    Dim dob As Date = DateTime.ParseExact(datepicker1.Text, "dd/MM/yyyy", Nothing)
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from studentsprofile where admno = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    rcomfirm.Read()
                   
                    If Session("studentadd") <> Nothing Then
                        If (rcomfirm.HasRows = True And Session("studentadd") = txtID.Text) Or (rcomfirm.HasRows = False And Session("studentadd") <> txtID.Text) Then
                            rcomfirm.Close()

                            If Session("studentadd") <> txtID.Text Then

                            Else

                            End If
                            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set surname = ?, sex = ?, dateofbirth = ?, phone = ?, admno = ? where admno = '" & Session("studentadd") & "'", con)
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text.ToUpper))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dob", datepicker1.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                            cmdCheck3.ExecuteNonQuery()
                            Dim sage As TimeSpan = Now.Subtract(dob)
                            Dim age As Integer = sage.TotalDays \ 365
                            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                            reader2.Read()
                            Dim currentsession As String = reader2(0)
                            reader2.Close()
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update studentSummary set age = '" & age & "' where session = '" & currentsession & "' and student = '" & txtID.Text & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update studentSummary set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update attendance set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update messages set sender = '" & txtID.Text & "' where sender = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update messages set receiver = '" & txtID.Text & "' where receiver = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update studentSummary set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If

                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update discount set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update notifications set origin = '" & txtID.Text & "' where origin = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update notifications set recipient = '" & txtID.Text & "' where recipient = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update parentward set ward = '" & txtID.Text & "' where ward = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update sentmsgs set sender = '" & txtID.Text & "' where sender = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update sentmsgs set receiver = '" & txtID.Text & "' where receiver = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update subjectreg set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()

                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update communicate set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()

                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update submittedassignments set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update transactions set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("studentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update termtraits set student = '" & txtID.Text & "' where student = '" & Session("studentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            Session("studentadd") = txtID.Text
                            Dim msg As String = "Your profile has been updated by your class teacher."
                            logify.log(Session("staffid"), "The profile of " & txtSurname.Text & " was updated.")
                            logify.Notifications(msg, txtID.Text, Session("staffid"), "Class Teacher", "~/content/student/studentprofile.aspx", "")
                            logify.Notifications("The profile of " & par.getstuname(txtID.Text) & " was updated by your class teacher", par.getparent(txtID.Text), Session("staffid"), "Class", "~/content/parent/studentprofile.aspx?" & txtID.Text, "")

                            If Session("senders") <> Nothing Then
                                Dim x As String = Session("senders")
                                Session("senders") = Nothing
                                Response.Redirect(x)
                            Else
                                Response.Redirect("~/content/staff/studentprofile.aspx?" & Request.QueryString.ToString)

                            End If
                        Else
                            rcomfirm.Close()
                            Show_Alert(False, "Admission number exists.")
                            e.Cancel = True
                        End If
                    ElseIf rcomfirm.HasRows = False And Session("studentadd") = Nothing Then
                        rcomfirm.Close()
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into studentsprofile (admno, surname, sex, dateofbirth, phone) Values (?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text.ToUpper))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text.ToUpper))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dob", datepicker1.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                        cmdCheck2.ExecuteNonQuery()
                        Session("StudentAdd") = txtID.Text
                        logify.log(Session("staffid"), txtSurname.Text & " was added as a student. Admission no = " & txtID.Text)
                    Else
                        rcomfirm.Close()
                        Show_Alert(False, "Admission number exists.")
                        e.Cancel = True
                    End If
                    con.Close()
                End Using



            ElseIf Wizard1.ActiveStep Is WizardStep2 Then

            ElseIf Wizard1.ActiveStep Is School Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.open()
                    If CheckBox1.Checked = True Then
                        If cboHostel.Text = "SELECT" Then
                        Else
                            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostel = ? Where admno = ?", con)
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", cboHostel.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", txtID.Text))
                            cmdCheck3.ExecuteNonQuery()
                            logify.log(Session("staffid"), txtSurname.Text & " was made a boarder in " & cboHostel.Text)
                            logify.Notifications("You are now a boarder in " & cboHostel.Text, txtID.Text, Session("staffid"), "Class Teacher", "~/content/student/studentprofile.aspx", "")
                        End If
                    End If
                    If CheckBox2.Checked = True Then
                        If cboTransport.Text = "SELECT" Then
                        Else
                            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set transport = ? Where admno = ?", con)
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", cboTransport.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", txtID.Text))
                            cmdCheck3.ExecuteNonQuery()
                            logify.log(Session("staffid"), txtSurname.Text & " was put on transport in " & cboTransport.Text)
                            logify.Notifications("You are now on transport. Route = " & cboTransport.Text, txtID.Text, Session("staffid"), "Class Teacher", "~/content/student/studentprofile.aspx", "")
                        End If
                    End If
                    If cboAdm.Text = "Paid" Then
                        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set admfees = ? Where admno = ?", con)
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", "Paid"))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", txtID.Text))
                        cmdCheck3.ExecuteNonQuery()
                    End If
                    con.close()
                End Using



            End If
            If Wizard1.StepNextButtonText = "SAVE" Then
                If Session("senders") <> Nothing Then
                    Dim x As String = Session("senders")
                    Session("senders") = Nothing
                    Response.Redirect(x)
                Else
                    Response.Redirect("~/content/staff/studentprofile.aspx?" & Request.QueryString.ToString)

                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub Class_Reg()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, type from class where class = '" & Session("classname") & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            student03.Read()
            Dim cla As String = student03.Item(0).ToString
            Dim clatype As String = student03(1).ToString
            student03.Close()
            Dim dob As Date = DateTime.ParseExact(datepicker1.Text, "dd/MM/yyyy", Nothing)
            Dim sage As TimeSpan = Now.Subtract(dob)
            Dim age As Integer = sage.TotalDays \ 365


            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
            cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
            cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
            reader2.Read()
            If reader2.HasRows Then
                reader2.Close()
                Show_Alert(False, "Student previously registerred in a class")
                Exit Sub
            Else
                Dim total As Double

                reader2.Close()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classfees where class = ?", con)
                cmdLoad0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim classfee As New ArrayList
                Dim quarterly As New ArrayList
                Dim classamount As New ArrayList
                Dim min As New ArrayList
                Dim monthly As New ArrayList
                Dim classi As Integer
                Do While student0.Read
                    Dim z As Integer
                    classfee.Add(student0.Item(2))
                    classamount.Add(student0.Item(3))
                    min.Add(student0.Item("amount") * (student0.Item("min") / 100))
                    monthly.Add(student0.Item(5))
                    quarterly.Add(student0.Item(6))
                Loop
                student0.Close()
                For Each item As String In classfee
                    Dim test2 As Boolean
                    Dim f As New Random
                    Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader

                    Dim refs2 As New ArrayList
                    Do While readref2.Read
                        refs2.Add(readref2.Item(0))
                    Loop
                    Dim d As Integer
                    Do Until test2 = True
                        d = f.Next(100000, 999999)
                        If refs2.Contains(d) Then
                            test2 = False
                        Else
                            test2 = True
                        End If
                    Loop
                    readref2.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min, monthly, quarterly, termly) Values (?,?,?,?,?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", classamount(classi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", monthly(classi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amgsdgt", quarterly(classi)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amsat", classamount(classi)))
                    cmdInsert22.ExecuteNonQuery()
                    total = total + classamount(classi)



                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(classamount(classi), , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck2.ExecuteNonQuery()

                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(classamount(classi), , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck4.ExecuteNonQuery()

                    classi = classi + 1
                    d = Nothing
                Next





                Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from optionalfees", con)
                Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader
                Dim genfee As New ArrayList
                Dim genamount As New ArrayList
                Dim geni As Integer
                Dim genmin As New ArrayList
                Do While student2.Read
                    genfee.Add(student2.Item(1))
                    genamount.Add(student2.Item(2))
                    genmin.Add(student2.Item("amount") * (student2.Item("min") / 100))
                Loop
                student2.Close()
                For Each item As String In genfee
                    Dim test21 As Boolean = False
                    Dim f1 As New Random
                    Dim ref21 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref21 As MySql.Data.MySqlClient.MySqlDataReader = ref21.ExecuteReader

                    Dim refs21 As New ArrayList
                    Do While readref21.Read
                        refs21.Add(readref21.Item(0))
                    Loop
                    Dim d1 As Integer
                    Do Until test21 = True
                        d1 = f1.Next(100000, 999999)
                        If refs21.Contains(d1) Then
                            test21 = False
                        Else
                            test21 = True
                        End If
                    Loop
                    readref21.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", genamount(geni)))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", genmin(geni)))
                    cmdInsert22.ExecuteNonQuery()
                    total = total + genamount(geni)

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(genamount(geni), , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck2.ExecuteNonQuery()


                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(genamount(geni), , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    cmdCheck4.ExecuteNonQuery()

                    geni = geni + 1
                    d1 = Nothing
                Next





                Dim cmdLoad3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
                Dim student3 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad3.ExecuteReader
                Dim admfee As New ArrayList
                Dim admamount As New ArrayList
                Dim admin As New ArrayList
                Dim admi As Integer
                Do While student3.Read

                    admfee.Add(student3.Item(1))
                    admamount.Add(student3.Item(2))
                    admin.Add(student3.Item("amount") * (student3.Item("min") / 100))
                Loop
                student3.Close()

                Dim cmdload3xcz As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classonefees where class = '" & cla & "'", con)
                Dim student3xcz As MySql.Data.MySqlClient.MySqlDataReader = cmdload3xcz.ExecuteReader

                Do While student3xcz.Read

                    admfee.Add(student3xcz.Item("fee"))
                    admamount.Add(student3xcz.Item("amount"))
                    admin.Add(student3xcz.Item("amount") * (student3xcz.Item("min") / 100))
                Loop
                student3xcz.Close()

                Dim hostel As String
                Dim transport As String
                Dim feeding As String
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT admfees, hostelstay, transport from StudentsProfile where admno = ?", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", txtID.Text))

                Dim studentadm As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                studentadm.Read()
                hostel = studentadm.Item(1)
                transport = studentadm.Item(2)
                Dim paid As String = studentadm.Item(0)
                studentadm.Close()
                If paid = "Not Paid" Then
                    For Each item As String In admfee
                        Dim test22 As Boolean = False
                        Dim f2 As New Random
                        Dim ref22 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                        Dim readref22 As MySql.Data.MySqlClient.MySqlDataReader = ref22.ExecuteReader

                        Dim refs22 As New ArrayList
                        Do While readref22.Read
                            refs22.Add(readref22.Item(0))
                        Loop
                        Dim d2 As Integer
                        Do Until test22 = True
                            d2 = f2.Next(100000, 999999)
                            If refs22.Contains(d2) Then
                                test22 = False
                            Else
                                test22 = True
                            End If
                        Loop
                        readref22.Close()
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", admamount(admi)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", admin(admi)))
                        cmdInsert22.ExecuteNonQuery()
                        total = total + admamount(admi)

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(admamount(admi), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                        cmdCheck2.ExecuteNonQuery()

                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(admamount(admi), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                        cmdCheck4.ExecuteNonQuery()
                        admi = admi + 1
                        d2 = Nothing


                    Next

                End If
                If Session("term") = "1st term" Then




                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from sessionalfees", con)
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    Dim sesfee As New ArrayList
                    Dim sesamount As New ArrayList
                    Dim sesi As Integer
                    Dim sesmin As New ArrayList
                    Do While student24.Read

                        sesfee.Add(student24.Item(1))
                        sesamount.Add(student24.Item(2))
                        sesmin.Add(student24.Item("amount") * (student24.Item("min") / 100))
                    Loop
                    student24.Close()

                    Dim cmdload3xcz1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classsessionfees where class = '" & cla & "'", con)
                    Dim student3xcz1 As MySql.Data.MySqlClient.MySqlDataReader = cmdload3xcz1.ExecuteReader

                    Do While student3xcz1.Read

                        sesfee.Add(student3xcz1.Item("fee"))
                        sesamount.Add(student3xcz1.Item("amount"))
                        sesmin.Add(student3xcz1.Item("amount") * (student3xcz1.Item("min") / 100))
                    Loop
                    student3xcz1.Close()
                    For Each item As String In sesfee
                        Dim test23 As Boolean = False
                        Dim f3 As New Random
                        Dim ref23 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                        Dim readref23 As MySql.Data.MySqlClient.MySqlDataReader = ref23.ExecuteReader

                        Dim refs23 As New ArrayList
                        Do While readref23.Read
                            refs23.Add(readref23.Item(0))
                        Loop
                        Dim d3 As Integer
                        Do Until test23 = True
                            d3 = f3.Next(100000, 999999)
                            If refs23.Contains(d3) Then
                                test23 = False
                            Else
                                test23 = True
                            End If
                        Loop
                        readref23.Close()
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", sesfee(sesi)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount(sesi)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin(sesi)))
                        cmdInsert22.ExecuteNonQuery()
                        total = total + sesamount(sesi)


                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount(sesi), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", sesfee(sesi) & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount(sesi), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                        cmdCheck4.ExecuteNonQuery()
                        sesi = sesi + 1
                        d3 = Nothing
                    Next
                End If
                If hostel = True Then

                    Dim test24 As Boolean = False
                    Dim f4 As New Random
                    Dim ref24 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref24 As MySql.Data.MySqlClient.MySqlDataReader = ref24.ExecuteReader

                    Dim refs24 As New ArrayList
                    Do While readref24.Read
                        refs24.Add(readref24.Item(0))
                    Loop
                    Dim d4 As Integer
                    Do Until test24 = True
                        d4 = f4.Next(100000, 999999)
                        If refs24.Contains(d4) Then
                            test24 = False
                        Else
                            test24 = True
                        End If
                    Loop
                    readref24.Close()
                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    student24.Read()

                    Dim sesamount As String = student24.Item(0)
                    Dim sesmin As Integer = (student24.Item("cost") * (student24.Item("min") / 100))
                    student24.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                    cmdInsert22.ExecuteNonQuery()
                    total = total + sesamount

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BOARDING DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck2.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck4.ExecuteNonQuery()

                End If
                If transport <> "" Then
                    Dim test25 As Boolean = False
                    Dim f5 As New Random
                    Dim ref25 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref25 As MySql.Data.MySqlClient.MySqlDataReader = ref25.ExecuteReader

                    Dim refs25 As New ArrayList
                    Do While readref25.Read
                        refs25.Add(readref25.Item(0))
                    Loop
                    Dim d5 As Integer
                    Do Until test25 = True
                        d5 = f5.Next(100000, 999999)
                        If refs25.Contains(d5) Then
                            test25 = False
                        Else
                            test25 = True
                        End If
                    Loop
                    readref25.Close()
                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees where route = ?", con)
                    cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", transport))
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    student24.Read()
                    Dim sesfee As String = student24.Item(0)
                    Dim sesamount As String = student24.Item(1)
                    Dim sesmin As Integer = (student24.Item("amount") * (student24.Item("min") / 100))
                    student24.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                    cmdInsert22.ExecuteNonQuery()
                    total = total + sesamount

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "TRANSPORT DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck2.ExecuteNonQuery()

                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck4.ExecuteNonQuery()
                End If




                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO StudentSummary (Session, Class, student, age) Values (?,?,?,?)", con)
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", txtID.Text))
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("age", age))

                cmdInsert2.ExecuteNonQuery()
                If clatype <> "K.G 1 Special" Then

                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM ClassSubjects WHERE Type= ? And class = ? and subjectnest = '" & 0 & "'", con)
                    Dim type As String = "Compulsory"
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", txtID.Text))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Type", type))
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                    Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    Dim subjectsID As New ArrayList
                    Dim i As Integer = 0
                    Do While reader1.Read
                        subjectsID.Add(Val(reader1.Item("subject")))

                    Loop
                    reader1.Close()
                    For Each item As String In subjectsID
                        Dim param As New MySql.Data.MySqlClient.MySqlParameter

                        param = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subjects", item))


                        cmd.ExecuteNonQuery()
                        cmd.Parameters.Remove(param)

                    Next
                    Dim cmd2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectnest FROM ClassSubjects WHERE class = '" & cla & "' and subjectnest <> '" & "" & "'", con)
                    Dim reader1a As MySql.Data.MySqlClient.MySqlDataReader = cmd2s.ExecuteReader
                    Dim subjectsIDnest As New ArrayList
                    Dim inest As Integer = 0
                    Do While reader1a.Read
                        If Not subjectsIDnest.Contains(Val(reader1a.Item(0))) Then
                            subjectsIDnest.Add(Val(reader1a.Item(0)))
                        End If
                    Loop
                    reader1a.Close()
                    For Each item As String In subjectsIDnest
                        Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", txtID.Text))
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", item))
                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                        cmdds.ExecuteNonQuery()
                    Next
                Else
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, subject FROM kcourseoutline where session = '" & Session("sessionid") & "' and class = '" & cla & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    Dim topics As New ArrayList
                    Dim subjectss As New ArrayList
                    Do While reader20.Read()
                        topics.Add(reader20(0))
                        subjectss.Add(reader20(1))
                    Loop
                    reader20.Close()
                    Dim llt As Integer
                    For Each topic As Integer In topics
                        Dim cmdceck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into kscoresheet (session, class, subject, topic, student) Values (?,?,?,?,?)", con)
                        cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("sessionid")))
                        cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", cla))
                        cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", subjectss(llt)))
                        cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", topic))
                        cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", txtID.Text))
                        cmdceck2.ExecuteNonQuery()
                        llt = llt + 1
                    Next
                End If
                Average_Age()


                add_traits()
            End If
            logify.log(Session("staffid"), txtSurname.Text & " was admitted into " & Session("classname"))
            logify.Notifications("You are now a student of " & Session("classname"), txtID.Text, Session("staffid"), "Class Teacher", "~/content/student/classdetails.aspx", "")
            con.Close()
        End Using
       
    End Sub


    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            If Session("StudentAdd") = "" Then
                Session("StudentAdd") = Request.QueryString.ToString
            End If
            Dim folderPath As String = Server.MapPath("~/img/")
            'Save the File to the Directory (Folder).
            If FileUpload1.HasFile Then
                If FileUpload1.PostedFile.ContentLength > 131072 Then
                    Show_Alert(False, "File not uploaded, the file selected is greater than 100kb.")
                    Exit Sub
                Else
                    FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                    Dim x As String = "~/img/" & FileUpload1.FileName
                    Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                        con.open()
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Update Studentsprofile Set passport = ? Where admno = ?", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", x))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("StudentAdd").ToString))
                        cmdCheck2.ExecuteNonQuery()
                        Show_Alert(True, "Upload successful.")
                        Image1.ImageUrl = x
                        logify.log(Session("staffid"), "The passport of " & txtSurname.Text & " - student was updated.")
                        con.Close()
                    End Using
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

      

    End Sub

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                If CheckBox1.Checked = True Then
                    cboHostel.Enabled = True
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostelstay = ? Where admno = ?", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", 1))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("StudentAdd").ToString))
                    cmdCheck2.ExecuteNonQuery()
                Else
                    cboHostel.Enabled = False
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostelstay = ? Where admno = ?", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", 0))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("StudentAdd").ToString))
                    cmdCheck2.ExecuteNonQuery()
                End If



                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged

        If CheckBox2.Checked = True Then
            cboTransport.Enabled = True
        Else
            cboTransport.Enabled = False
        End If
    End Sub

    Protected Sub radParent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles radParent.SelectedIndexChanged
        If radParent.SelectedValue = "Existing Parent" Then
            Panel1.Visible = True
            panel2.Visible = False
            txtExistPPhone.Text = ""
        Else
            panel2.Visible = True
            Panel1.Visible = False
            txtPId.Text = ""
            txtPname.Text = ""
            txtPAddress.Text = ""
            txtPPhone.Text = ""
            txtPEmail.Text = ""
            txtPass.Text = ""
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from parentprofile where phone = ?", con)
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtExistPPhone.Text))
                Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                rcomfirm.Read()
                If Not rcomfirm.HasRows Then
                    Show_Alert(False, "Parent does not exist. Check the Phone number")
                    Exit Sub
                End If

                Show_Alert(True, rcomfirm(0) & " - " & rcomfirm(1))
                rcomfirm.Close()
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim folderPath As String = Server.MapPath("~/img/")
                'Save the File to the Directory (Folder).
                If FileUpload2.HasFile Then
                    If FileUpload2.PostedFile.ContentLength > 131072 Then
                        Show_Alert(False, "File not uploaded, the file selected is greater than 100kb.")
                        Exit Sub
                    Else
                        FileUpload2.SaveAs(folderPath & Path.GetFileName(FileUpload2.FileName))
                        Dim x As String = "~/img/" & FileUpload2.FileName
                        txtPass.Text = x

                        Show_Alert(True, "Upload successful.")
                        Image2.ImageUrl = x
                    End If
                End If
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
      

    End Sub

   
End Class
