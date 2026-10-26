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
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If

        Try


            If Session("edit") <> Nothing Then
                enter_values()
                If Session("edit") = "parentprofile" Then
                    Wizard1.ActiveStepIndex = 0
                    Wizard1.StartNextButtonText = "SAVE"
                ElseIf Session("edit") = "passport" Then
                    Wizard1.ActiveStepIndex = 1
                    Wizard1.StepNextButtonText = "SAVE"
                    Wizard1.StepPreviousButtonText = "BACK"
                    Session("passportedit") = True
                ElseIf Session("edit") = "parentward" Then
                    Wizard1.ActiveStepIndex = 2
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
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from parentprofile where parentID = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("parentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            student.Read()
            Try
                txtID.Text = student.Item(0)
                txtID.Enabled = False
                txtSurname.Text = student.Item(1)
                txtPhone.Text = student.Item(5)
                cboSex.Text = student.Item(2)
                txtAddress.Text = student.Item(3)
                txtEmail.Text = student.Item(4)
            Catch
            End Try
            student.Close()

            con.close()        End Using
    End Sub


    Protected Sub Wizard1_FinishButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick

        If Request.QueryString.ToString <> "" Then
            Response.Redirect("~/content/App/Admin/parentprofile.aspx?" & Request.QueryString.ToString)
        Else
            Response.Redirect("~/content/App/Admin/parentprofile.aspx?" & Session("ParentAdd"))
        End If
        Session("ParentAdd") = Nothing
    End Sub

    Protected Sub Wizard1_NextButtonClick(sender As Object, e As WizardNavigationEventArgs)
        Try
            If Wizard1.ActiveStep Is WizardStep1 Then

                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from parentprofile where parentID = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    rcomfirm.Read()
                    If rcomfirm.HasRows Then
                        rcomfirm.Close()
                        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update parentprofile Set parentName = ?, sex = ?, address = ?, email = ?,  phone = ?  Where parentID = ?", con)
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text.ToUpper))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("add", txtAddress.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtEmail.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                        cmdCheck3.ExecuteNonQuery()
                        Dim msg As String = "Your profile has been updated by admin."
                        logify.log(Session("staffid"), "The profile of " & txtSurname.Text & " was updated.")
                        logify.Notifications(msg, txtID.Text, Session("staffid"), "Admin", "")
                        Response.Redirect("~/content/App/Admin/parentprofile.aspx")
                    Else
                        rcomfirm.Close()
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into parentprofile (parentID, parentName, sex, address, email, phone) values (?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text.ToUpper))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text.ToUpper))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("add", txtAddress.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtEmail.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                        cmdCheck2.ExecuteNonQuery()
                        Session("ParentAdd") = txtID.Text

                        logify.log(Session("staffid"), txtSurname.Text & " was added as a parent. ParentID = " & txtID.Text)
                    End If
                    con.close()                End Using


            ElseIf Wizard1.ActiveStep Is WizardStep2 Then
                If Session("passportedit") = True Then
                    Session("passportedit") = False
                    Response.Redirect("~/content/App/Admin/parentprofile.aspx")
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
            e.Cancel = True
        End Try

    End Sub

    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            If Session("ParentAdd") = "" Then
                Session("ParentAdd") = Request.QueryString.ToString
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
                    Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                        con.open()
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Update ParentProfile Set passport = ? Where ParentId = ?", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", x))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("ParentAdd").ToString))
                        cmdCheck2.ExecuteNonQuery()
                        Show_Alert(True, "Upload successful.")
                        Image1.ImageUrl = x
                        logify.log(Session("staffid"), "The passport of parent - " & txtSurname.Text & " was updated.")
                        con.Close()
                    End Using
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub





    Protected Sub Wizard1_PreviousButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.PreviousButtonClick
        If Wizard1.StepPreviousButtonText = "BACK" Then
            Response.Redirect("~/content/App/Admin/parentprofile.aspx")
        End If
    End Sub
End Class
