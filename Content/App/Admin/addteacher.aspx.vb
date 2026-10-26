Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

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



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If cboSubject.Text = "Select Subject" Or cboClass.Text = "Select Class" Then
            Show_Alert(False, "Please select subject and class to be taught")
        Else
            For Each item As GridViewRow In gridSubjects.Rows
                If item.Cells(0).Text = cboSubject.Text & " - " & cboClass.Text Then
                    Show_Alert(False, "Subject has already been added for this class")
                    Exit Sub
                End If
            Next
            Try
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdCheck1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Subjects Where subject = ?", con)
                    cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sub", cboSubject.Text))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck1.ExecuteReader
                    reader2.Read()
                    Dim subID As Integer = reader2.Item(0).ToString
                    reader2.Close()

                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class where class = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cboClass.Text))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                    reader3.Read()
                    Dim classid As Integer = reader3.Item(0).ToString
                    reader3.Close()


                    Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects set teacher = ? where subject = ? and class = ?", con)
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", subID))
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", classid))
                    insert.ExecuteNonQuery()
                    Dim msg2 As String = "You are now the subject teacher of " & cboSubject.Text & " - " & cboClass.Text
                    Dim ds As New DataTable
                    ds.Columns.Add("name")
                    For Each rrr As GridViewRow In gridSubjects.Rows
                        ds.Rows.Add(rrr.Cells(0).Text)
                    Next
                    ds.Rows.Add(cboSubject.Text & " - " & cboClass.Text)
                    gridSubjects.DataSource = ""
                    gridSubjects.DataSource = ds
                    gridSubjects.DataBind()

                    logify.log(Session("staffid"), txtID.Text & " was made the subject teacher of " & cboSubject.Text & " - " & cboClass.Text)
                    logify.Notifications(msg2, txtID.Text, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                    con.close()                End Using
            Catch ex As Exception
                Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
            End Try
        End If


    End Sub
    Protected Sub enter_values()
        Dim ds As New DataTable
        ds.Columns.Add("name")
        Dim ds2 As New DataTable
        ds2.Columns.Add("name")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT StaffId, surname, sex, phone, address, email, designation, salary, accountno, bank, pension, pfa, activated from staffprofile where staffid = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("staffadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            student.Read()
            txtID.Text = student.Item("staffid").ToString
            txtSurname.Text = student.Item("surname").ToString
            cboSex.Text = student.Item("sex").ToString
            txtPhone.Text = student.Item("phone").ToString
            txtAddress.Text = student.Item("address").ToString
            txtEmail.Text = student.Item("email").ToString
            cboDesignation.Text = student.Item("designation").ToString

          
            student.Close()
            txtID.Enabled = False

            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class, subjects.subject from classsubjects Inner Join Class on Class.ID = classsubjects.class Inner Join Subjects on Subjects.ID = classsubjects.subject  WHERE classsubjects.teacher = ?", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("StaffAdd")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Do While reader2.Read
                ds.Rows.Add(reader2.Item(1).ToString & " - " & reader2.Item(0).ToString)
            Loop
            reader2.Close()
            gridSubjects.DataSource = ds
            gridSubjects.DataBind()
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classteacher Inner Join Class on Class.ID = classteacher.class WHERE classteacher.teacher = ?", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("StaffAdd")))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            Do While reader3.Read
                ds2.Rows.Add(reader3.Item(0).ToString)
            Loop
            reader3.Close()
            gridClass.DataSource = ds2
            gridClass.DataBind()

            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from admin where username = ?", con)
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("StaffAdd")))
            Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
            If reader0.Read Then
                chkAdmin.Checked = True
            End If
            reader0.Close()

            Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from acclogin where username = ?", con)
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("StaffAdd")))
            Dim reader31 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader
            Dim count21 As Integer = 1
            If reader31.Read Then
                ChkAccount.Checked = True
            End If
            reader31.Close()
            con.Close()
        End Using
        txtID.Enabled = False
        lblHeader.Text = "EDIT STAFF"
        Wizard1.StartNextButtonText = "SAVE"
    End Sub


    Protected Sub Wizard1_FinishButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick

        Session("edit") = Nothing
    End Sub



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If

        Try


            If Not IsPostBack Then

                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Subjects", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    cboSubject.Items.Clear()
                    cboSubject.Items.Add("Select Subject")
                    Dim subject As String = ""
                    Do While reader2.Read
                        cboSubject.Items.Add(reader2.Item(1).ToString)
                    Loop
                    reader2.Close()

                    Dim cmdCheck1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class", con)
                    Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck1.ExecuteReader
                    cboClass1.Items.Clear()
                    cboClass1.Items.Add("None")
                    Do While reader1.Read
                        cboClass1.Items.Add(reader1.Item(1).ToString)
                    Loop
                    reader1.Close()
                    con.Close()                End Using

            Else



            End If
            If Session("edit") <> Nothing Then
                enter_values()
                If Session("edit") = "teacherprofile" Then
                    Wizard1.ActiveStepIndex = 0
                    Wizard1.StepPreviousButtonText = "BACK"
                ElseIf Session("edit") = "teachertaught" Then
                    Wizard1.ActiveStepIndex = 2
                    CheckBox2.Checked = True
                    panel1.Visible = True
                    Session("subjectedit") = True
                    Wizard1.StepNextButtonText = "SAVE"
                    Wizard1.StepPreviousButtonText = "BACK"
                ElseIf Session("edit") = "teacherclass" Then
                    Wizard1.ActiveStepIndex = 3
                    CheckBox1.Checked = True
                    panel2.Visible = True
                    Wizard1.StepNextButtonText = "SAVE"
                    Session("classedit") = True
                    Wizard1.StepPreviousButtonText = "BACK"
                ElseIf Session("edit") = "passport" Then
                    Wizard1.ActiveStepIndex = 1
                    Wizard1.StepNextButtonText = "SAVE"
                    Wizard1.StepPreviousButtonText = "BACK"
                    Session("passportedit") = True
                ElseIf Session("edit") = "roles" Then
                    Wizard1.ActiveStepIndex = 4
                    Wizard1.FinishCompleteButtonText = "SAVE"
                    Wizard1.StepPreviousButtonText = "BACK"
                End If
                Session("edit") = Nothing
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Subjects Where subject = ?", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sub", cboSubject.Text))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                reader2.Read()
                subselect = reader2.Item(0).ToString
                reader2.Close()

                Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ClassSubjects.class, Class.Class FROM Subjects INNER JOIN ClassSubjects ON Subjects.ID = ClassSubjects.subject INNER JOIN Class ON Class.ID = ClassSubjects.class WHERE ClassSubjects.subject = ? ", con)
                cmd4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ClassSubjects.Class", subselect))
                Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
                cboClass.Items.Clear()
                cboClass.Items.Add("Select Class")
                Do While reader4.Read
                    cboClass.Items.Add(reader4.Item(1).ToString)
                Loop
                reader4.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub gridSubjects_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gridSubjects.RowDeleting
        Dim rows As GridViewRow = gridSubjects.Rows(e.RowIndex)


        subremove = rows.Cells(0).Text

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If subremove = Nothing Then
                    Show_Alert(False, "Select a subject from the list to remove")
                    Exit Sub
                End If
                Dim s As Array = Split(subremove, " - ")
                Dim cmdCheck1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Subjects Where subject = ?", con)
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sub", s(0)))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck1.ExecuteReader
                reader2.Read()
                Dim subID As Integer = reader2.Item(0).ToString
                reader2.Close()

                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class where class = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", s(1)))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim classid As Integer = reader3.Item(0).ToString
                reader3.Close()
                Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects set teacher = ? where subject = ? and class = ?", con)
                insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", ""))
                insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", subID))
                insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", classid))
                insert.ExecuteNonQuery()
                Dim msg2 As String = "You have been removed as the subject teacher of " & s(0) & " - " & s(1)
                logify.log(Session("staffid"), txtID.Text & " was removed as the subject teacher of " & cboSubject.Text & " - " & cboClass.Text)
                logify.Notifications(msg2, txtID.Text, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                Dim ds As New DataTable
                ds.Columns.Add("name")
                For Each rrr As GridViewRow In gridSubjects.Rows
                    If Not rrr.Cells(0).Text = subremove Then
                        ds.Rows.Add(rrr.Cells(0).Text)
                    End If
                Next
                gridSubjects.DataSource = ds
                gridSubjects.DataBind()
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Wizard1_NextButtonClick(sender As Object, e As WizardNavigationEventArgs)

    End Sub

    Protected Sub Wizard1_NextButtonClick1(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.NextButtonClick
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Wizard1.ActiveStep Is WizardStep1 Then
                    Try
                     
                        Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from StaffProfile where StaffId = ?", con)
                        comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                        Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                        rcomfirm.Read()
                        If rcomfirm.HasRows Then
                            If txtID.Enabled = False Then
                                rcomfirm.Close()
                                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update StaffProfile Set surname = ?, sex = ?, phone = ?,  address = ?, email = ?, designation = ? Where staffID = ?", con)
                                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text.ToUpper))
                                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("add", txtAddress.Text))
                                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtEmail.Text))
                                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("designation", cboDesignation.Text))
                                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                                cmdCheck3.ExecuteNonQuery()
                                Dim msg As String = "Your profile has been updated by admin."
                                logify.log(Session("staffid"), "The profile of " & txtSurname.Text & " was updated.")
                                logify.Notifications(msg, txtID.Text, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                                Response.Redirect("~/content/App/Admin/staffprofile.aspx")
                            Else
                                Show_Alert(False, "Staff ID already exists")
                                e.Cancel = True
                            End If
                        Else
                            rcomfirm.Close()
                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into StaffProfile (StaffId, surname, password, sex, phone, address, email, designation) Values (?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text.ToUpper))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text.ToUpper))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", "password"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("add", txtAddress.Text))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtEmail.Text))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("designation", cboDesignation.Text))

                            cmdCheck2.ExecuteNonQuery()
                            Session("StaffAdd") = txtID.Text
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
                            Dim mailMessage As MailMessage = New MailMessage(email, txtEmail.Text)
                            Dim path As String = "http://" & Request.Url.Authority
                            mailMessage.IsBodyHtml = True
                            mailMessage.Body = String.Format("<img alt='' style='width:370px; height:90px;' src='" & path & Replace(logo, "~", "") & "' /> <br/> You have been registerred as a staff in " & smsname & ". Your login details are shown below - <br/> User Name : " & txtID.Text & " <br/> Password: password <br/> Please login <a href='" + path + "'>here </a> and change your password from the default password. <br/> Trough the portal, you can view your roles and perform necessaary activities. Click <a href='" + path + "'>here </a> to login.")
                            mailMessage.Subject = "Welcome to " & smsname
                            Try
                                smtpClient.Send(mailMessage)
                            Catch ex As Exception

                            End Try

                            logify.log(Session("staffid"), txtSurname.Text & " was added as a staff. StaffID = " & txtID.Text)



                        End If
                        Session("upload") = True

                    Catch ex As Exception


                        Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
                        e.Cancel = True
                    End Try

                ElseIf Wizard1.ActiveStep Is WizardStep2 Then

                    Session("upload") = False
                    If Session("passportedit") = True Then
                        Session("passportedit") = False
                        Response.Redirect("~/content/App/Admin/staffprofile.aspx")
                    End If
                ElseIf Wizard1.ActiveStep Is WizardStep3 Then

                    If Session("subjectedit") = True Then
                        Session("subjectedit") = False
                        Response.Redirect("~/content/App/Admin/staffprofile.aspx")
                    End If
                ElseIf Wizard1.ActiveStep Is classmanaged Then




                    If Session("classedit") = True Then
                        Session("classedit") = False
                        Response.Redirect("~/content/App/Admin/staffprofile.aspx")
                    End If
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If cboClass1.Text = "None" Then
            Exit Sub
        End If
        For Each item As GridViewRow In gridClass.Rows
            If item.Cells(0).Text = cboClass1.Text Then
                Show_Alert(False, "Class has already been added")
                Exit Sub
            End If
        Next

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class where class = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cboClass1.Text))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim classid As Integer = reader3.Item(0).ToString
                reader3.Close()

                Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into classteacher (teacher, class) Values (?,?)", con)
                insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", classid))
                insert.ExecuteNonQuery()
                Dim msg3 As String = "You are now the class teacher of " & cboClass1.Text
                logify.log(Session("staffid"), txtID.Text & " was made the class teacher of " & cboClass1.Text)
                logify.Notifications(msg3, txtID.Text, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                Dim ds As New DataTable
                ds.Columns.Add("name")
                For Each rrr As GridViewRow In gridClass.Rows
                    ds.Rows.Add(rrr.Cells(0).Text)
                Next
                ds.Rows.Add(cboClass1.Text)
                gridClass.DataSource = ""
                gridClass.DataSource = ds
                gridClass.DataBind()

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try

            Dim folderPath As String = Server.MapPath("~/img/")
            'Save the File to the Directory (Folder).
            If FileUpload1.HasFile Then
                If FileUpload1.PostedFile.ContentLength > 131072 Then
                    Show_Alert(False, "File not uploaded, the file selected is greater than 100kb.")
                    Exit Sub
                Else
                    FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                    Dim x As String = "~/img/" & FileUpload1.FileName
                    Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                        con.open()
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Update StaffProfile Set passport = ? Where staffId = ?", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", x))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("StaffAdd").ToString))
                        cmdCheck2.ExecuteNonQuery()
                        Show_Alert(True, "Upload successful.")
                        Image1.ImageUrl = x
                        logify.log(Session("staffid"), "The passport of " & txtSurname.Text & " - staff was updated.")
                        con.Close()
                    End Using
                End If
            End If

        Catch ex As Exception

            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub cboClass1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboClass1.SelectedIndexChanged


    End Sub

    Protected Sub gridClass_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gridClass.RowDeleting
        Dim rows As GridViewRow = gridClass.Rows(e.RowIndex)


        classremove = rows.Cells(0).Text


        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class where class = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", classremove))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim classid As Integer = reader3.Item(0).ToString
                reader3.Close()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From classteacher where teacher = ? and class = ?", con)
                delete.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                delete.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", classid))
                delete.ExecuteNonQuery()
                Dim msg As String = "You have been removed as the class teacher of " & classremove
                logify.log(Session("staffid"), txtID.Text & " was removed as the class teacher of " & classremove & ".")
                logify.Notifications(msg, txtID.Text, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                Dim ds As New DataTable
                ds.Columns.Add("name")
                For Each rrr As GridViewRow In gridClass.Rows
                    If Not rrr.Cells(0).Text = classremove Then
                        ds.Rows.Add(rrr.Cells(0).Text)
                    End If
                Next
                gridClass.DataSource = ds
                gridClass.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            panel1.Visible = True
        Else
            panel1.Visible = False
        End If
    End Sub

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            panel2.Visible = True
        Else
            panel2.Visible = False
        End If
    End Sub

    Protected Sub Wizard1_PreviousButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.PreviousButtonClick
        If Wizard1.StepPreviousButtonText = "BACK" Then
            Response.Redirect("~/content/App/Admin/staffprofile.aspx")
        End If
        If Wizard1.ActiveStep Is WizardStep3 Then
            e.Cancel = True
        End If
    End Sub

    Protected Sub ChkAccount_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAccount.CheckedChanged
        Try
            Dim msg As String
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If ChkAccount.Checked = True Then
                    Dim insert1 As New MySql.Data.MySqlClient.MySqlCommand("Delete from acclogin where username = ?", con)
                    insert1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert1.ExecuteNonQuery()
                    Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into acclogin (username) Values (?)", con)
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert.ExecuteNonQuery()
                    msg = "You are now an account officer."
                    logify.log(Session("staffid"), txtID.Text & " was made an account officer.")
                Else
                    Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Delete from acclogin where username = ?", con)
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert.ExecuteNonQuery()
                    msg = "You are no more an account officer."
                    logify.log(Session("staffid"), txtID.Text & " was removed as an account officer.")

                End If
                logify.Notifications(msg, txtID.Text, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub chkAdmin_CheckedChanged(sender As Object, e As EventArgs) Handles chkAdmin.CheckedChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim msg As String
                If chkAdmin.Checked = True Then
                    Dim insert1 As New MySql.Data.MySqlClient.MySqlCommand("Delete from admin where username = ?", con)
                    insert1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert1.ExecuteNonQuery()

                    Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into admin (username) Values (?)", con)
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert.ExecuteNonQuery()
                    msg = "You are now an admin."
                    logify.log(Session("staffid"), txtID.Text & " was made an admin.")

                Else
                    Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Delete from admin where username = ?", con)
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert.ExecuteNonQuery()
                    msg = "You are no more an admin."
                    logify.log(Session("staffid"), txtID.Text & " was removed as an admin.")

                End If
                logify.Notifications(msg, txtID.Text, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Protected Sub chkLib_CheckedChanged(sender As Object, e As EventArgs) Handles chkLib.CheckedChanged
        Try
            Dim msg As String
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If chkLib.Checked = True Then
                    Dim insert1 As New MySql.Data.MySqlClient.MySqlCommand("Delete from lib where username = ?", con)
                    insert1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert1.ExecuteNonQuery()
                    Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into lib (username) Values (?)", con)
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert.ExecuteNonQuery()
                    msg = "You are now a librarian."
                    logify.log(Session("staffid"), txtID.Text & " was made a librarian.")
                Else
                    Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Delete from lib where username = ?", con)
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffAdd")))
                    insert.ExecuteNonQuery()
                    msg = "You are no more a librarian."
                    logify.log(Session("staffid"), txtID.Text & " was removed as a librarian.")

                End If
                logify.Notifications(msg, txtID.Text, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
