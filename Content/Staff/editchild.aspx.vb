Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
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

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        scriptman = Me.Master.FindControl("ScriptManager1")
        scriptman.RegisterPostBackControl(Button5)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Class(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

        If Session("edit") <> Nothing Then
            enter_values()
            If Session("edit") = "studentprofile" Then
                Wizard1.ActiveStepIndex = 0
                Wizard1.StartNextButtonText = "SAVE"
                Wizard1.StepPreviousButtonText = "BACK"
            ElseIf Session("edit") = "passport" Then
                Wizard1.ActiveStepIndex = 1
                Wizard1.FinishCompleteButtonText = "SAVE"
                Wizard1.FinishPreviousButtonText = "BACK"
            End If
            Session("edit") = Nothing
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub enter_values()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsprofile where admno = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            student.Read()
            Try
                txtID.Text = student.Item(0)
                txtSurname.Text = student.Item(1)
                txtPhone.Text = student.Item(6)
                cboSex.Text = student.Item(2)
                datepicker1.Text = student.Item(3).ToString
            Catch
            End Try
            student.Close()
            con.close()        End Using
    End Sub


    Protected Sub Wizard1_FinishButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick



    End Sub

    Protected Sub Wizard1_NextButtonClick(sender As Object, e As WizardNavigationEventArgs)
        Try
            If Wizard1.ActiveStep Is WizardStep1 Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    If datepicker1.Text = Nothing Then
                        Show_Alert(False, "Please select a valid Date of Birth")
                        Exit Sub
                    End If
                    Dim dob As Date = DateTime.ParseExact(datepicker1.Text, "dd/MM/yyyy", Nothing)
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from studentsprofile where admno = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    rcomfirm.Read()
                    If rcomfirm.HasRows Then
                        rcomfirm.Close()
                        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set surname = ?, sex = ?, dateofbirth = ?, phone = ? Where admno = ?", con)
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text.ToUpper))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dob", datepicker1.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                        cmdCheck3.ExecuteNonQuery()
                        Dim sage As TimeSpan = Now.Subtract(dob)
                        Dim age As Integer = sage.TotalDays \ 365
                        Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update studentSummary set age = '" & age & "' where session = '" & Session("sessionid") & "' and student = '" & txtID.Text & "'", con)
                        cmdInsert2.ExecuteNonQuery()

                        Dim msg As String = "Your profile has been updated by your Class teacher."
                        logify.log(Session("staffid"), "The profile of " & txtSurname.Text & " was updated.")
                        logify.Notifications(msg, txtID.Text, Session("staffid"), "CLASS TEACHER", "~/content/student/studentprofile.aspx", "")
                        logify.Notifications("The profile of " & par.getstuname(txtID.Text) & " was updated by the Class Teacher", par.getparent(txtID.Text), Session("staffid"), par.getstuname(txtID.Text) & " CLASS TEACHER", "~/content/parent/studentprofile.aspx?" & txtID.Text, "")

                        Response.Redirect("~/content/staff/studentprofile.aspx")
                    Else
                        Response.Redirect("~/content/staff/studentprofile.aspx")
                    End If
                    con.close()                End Using
            End If
            If Wizard1.StepNextButtonText = "SAVE" Then
                Response.Redirect("~/content/staff/studentprofile.aspx")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            If Session("StudentAdd") = "" Then
                Session("StudentAdd") = Request.QueryString.ToString
            End If
            Dim folderPath As String = Server.MapPath("~/img/")
            'Save the File to the Directory (Folder).
            FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
            Dim x As String = "~/img/" & FileUpload1.FileName
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Update Studentsprofile Set passport = ? Where admno = ?", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", x))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("StudentAdd").ToString))
                cmdCheck2.ExecuteNonQuery()
                Show_Alert(True, "Upload successful.")
                Image1.ImageUrl = x
                logify.log(Session("staffid"), "The passport of " & txtSurname.Text & " - student was updated.")

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



End Class
