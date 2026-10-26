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
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")

        Try

       
            If Session("edit") = Nothing Then
            Else

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

            End If
            Session("edit") = Nothing

            ' Show a "resuming edit" banner for a parent record the admin was in the middle
            ' of editing. Quick-resume (opened from the dashboard's recent-activity widget)
            ' trusts the referenced parent code as-is; a normal resume (opened from the
            ' parent list page, which already validated the reference against the database)
            ' encodes it before display.
            Dim parentRef As String = Request.QueryString("parentRef")
            If Not String.IsNullOrEmpty(parentRef) Then
                If Request.QueryString("resumeMode") = "quick" Then
                    ' SINK: PLANTED-VB-HR-30
                    Response.Write("<div class='resume-banner'>Resuming quick edit for parent reference " & parentRef & "</div>")
                Else
                    ' SAFE_SINK: PLANTED-VB-HR-30-safe
                    Response.Write("<div class='resume-banner'>Resuming edit for parent reference " & Server.HtmlEncode(parentRef) & "</div>")
                End If
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
            Response.Redirect("~/content/Admin/parentprofile.aspx?" & Request.QueryString.ToString)
        Else
            Response.Redirect("~/content/Admin/parentprofile.aspx?" & Session("ParentAdd"))
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
                    If Session("parentadd") <> Nothing Then
                        If (rcomfirm.HasRows = True And Session("parentadd") = txtID.Text) Or (rcomfirm.HasRows = False And Session("parentadd") <> txtID.Text) Then
                            rcomfirm.Close()

                            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update parentprofile Set  parentName = ?, sex = ?, address = ?, email = ?,  phone = ?, parentid = ?  Where parentID = '" & Session("parentadd") & "'", con)
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text.ToUpper))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("add", txtAddress.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtEmail.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                            cmdCheck3.ExecuteNonQuery()


                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update notifications set origin = '" & txtID.Text & "' where origin = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update notifications set recipient = '" & txtID.Text & "' where recipient = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update notifications set origin = '" & txtID.Text & "' where origin = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update parentward set parent = '" & txtID.Text & "' where parent = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update sentmsgs set sender = '" & txtID.Text & "' where sender = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update messages set sender = '" & txtID.Text & "' where sender = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update sentmsgs set receiver = '" & txtID.Text & "' where receiver = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update messages set receiver = '" & txtID.Text & "' where receiver = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update communicate set sender = '" & txtID.Text & "' where sender = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            If Session("parentadd") <> Nothing Then
                                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update communicate set receiver = '" & txtID.Text & "' where receiver = '" & Session("parentadd") & "'", con)
                                cmdInsert2.ExecuteNonQuery()
                            End If
                            Session("parentadd") = txtID.Text
                            Dim msg As String = "Your profile has been updated by admin."
                            logify.log(Session("staffid"), "The profile of " & txtSurname.Text & " was updated.")
                            logify.Notifications(msg, txtID.Text, Session("staffid"), "Admin", "")
                            Response.Redirect("~/content/Admin/parentprofile.aspx")
                        Else
                            rcomfirm.Close()
                            Show_Alert(False, "Parent ID exists.")
                            e.Cancel = True
                        End If

                    ElseIf rcomfirm.HasRows = False And Session("parentadd") = Nothing Then
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
                    Else
                        rcomfirm.Close()
                        Show_Alert(False, "Parent ID exists.")
                        e.Cancel = True
                    End If
                        con.Close()                End Using


            ElseIf Wizard1.ActiveStep Is WizardStep2 Then
                If Session("passportedit") = True Then
                    Session("passportedit") = False
                    Response.Redirect("~/content/Admin/parentprofile.aspx")
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

    ' --- Guardian ID Document Upload (PLANTED-VB-HR-577) ---
    ' Lets office staff attach a scan of the guardian's photo ID to the parent's file
    ' while the enrollment wizard is still open, reusing the same ParentAdd session value
    ' Button5_Click (above, real pre-existing code) uses for the passport photo. Routed
    ' through a private same-class helper so the file-naming convention (keyed off the
    ' parent id) lives in one place; the extension of the client's own file name is never
    ' checked before the helper saves it.

    ' Instance (Indirect): one hop through a private helper in the same class.
    Private Function SaveGuardianIdDocument(upload As FileUpload, parentId As String) As String
        Dim folderPath As String = Server.MapPath("~/Uploads/guardian-id/")
        Dim destPath As String = folderPath & parentId & "_" & upload.FileName
        ' SINK: PLANTED-VB-HR-577
        upload.SaveAs(destPath)
        Return destPath
    End Function

    Protected Sub btnUploadGuardianId_Click(sender As Object, e As EventArgs) Handles btnUploadGuardianId.Click
        Try
            If Not FileUploadGuardianId.HasFile Then
                Show_Alert(False, "Please select a file.")
                Exit Sub
            End If
            SaveGuardianIdDocument(FileUploadGuardianId, Convert.ToString(Session("ParentAdd")))
            Show_Alert(True, "Guardian ID document uploaded.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' Safe twin helper (PLANTED-VB-HR-577-safe): checks the client's file extension
    ' against a closed allow-list of genuinely safe document/image types before ever
    ' calling SaveAs.
    Private Function SaveGuardianIdDocumentSafe(upload As FileUpload, parentId As String) As String
        Dim allowedIdExtensions() As String = {".pdf", ".jpg", ".jpeg", ".png"}
        Dim submittedExtension As String = Path.GetExtension(upload.FileName).ToLowerInvariant()
        If Array.IndexOf(allowedIdExtensions, submittedExtension) < 0 Then
            Throw New ApplicationException("Unsupported file type for a guardian ID document.")
        End If
        Dim folderPath As String = Server.MapPath("~/Uploads/guardian-id/")
        Dim destPath As String = folderPath & parentId & "_" & upload.FileName
        ' SAFE_SINK: PLANTED-VB-HR-577-safe
        upload.SaveAs(destPath)
        Return destPath
    End Function

    Protected Sub btnUploadGuardianIdSafe_Click(sender As Object, e As EventArgs) Handles btnUploadGuardianIdSafe.Click
        Try
            If Not FileUploadGuardianIdSafe.HasFile Then
                Show_Alert(False, "Please select a file.")
                Exit Sub
            End If
            SaveGuardianIdDocumentSafe(FileUploadGuardianIdSafe, Convert.ToString(Session("ParentAdd")))
            Show_Alert(True, "Guardian ID document uploaded.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Protected Sub Wizard1_PreviousButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.PreviousButtonClick
        If Wizard1.StepPreviousButtonText = "BACK" Then
            Response.Redirect("~/content/Admin/parentprofile.aspx")
        End If
    End Sub
End Class
