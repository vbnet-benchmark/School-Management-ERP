Imports System.IO
Imports System.Text.RegularExpressions
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

    ''' <summary>
    ''' Format-checks one of this page's free-text contact fields, keyed by field name, before the
    ''' staff profile is saved -- called twice (unconditionally, on every save) from
    ''' Wizard1_NextButtonClick1, once per real, already-existing TextBox field. No genuine
    ''' interface/MustInherit/Overridable dispatch exists over contact-field validation in this
    ''' project (the three real polymorphic hierarchies -- App_Code/StudentOwnedRecordRepository.vb,
    ''' App_Code/MessagePreviewRenderer.vb, App_Code/StudentImportParsers.vb -- dispatch per-table
    ''' record lookups, HTML-preview rendering, and XML-import parsing respectively, none of which
    ''' applies here, mirroring this project's own Content/Admin/addteacher.aspx.vb
    ''' #btnUploadTeacherDocument_Click precedent), so this substitutes a hardcoded Select Case over
    ''' the caller-supplied (never attacker-influenced) field-name key instead.
    ''' </summary>
    Private Function ValidateContactFieldFormat(fieldKey As String, value As String) As Boolean
        Select Case fieldKey
            Case "Phone"
                ' Nested-quantifier digit-class pattern (the same (class+sep?)+ ambiguity as the
                ' free-text "title" family, applied to digits/separators instead of letters).
                ' SINK: PLANTED-VB-HR-289
                Return Regex.IsMatch(value, "^\+?(([0-9])+[\s\-\.]?)+$")
            Case "Email"
                ' Rewritten linear equivalent -- the ambiguous inner quantifier on each clause is
                ' removed; the required literal "." before each domain label gives the outer
                ' repetition an unambiguous boundary.
                ' SAFE_SINK: PLANTED-VB-HR-289-safe
                Return Regex.IsMatch(value, "^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+$")
            Case Else
                Return True
        End Select
    End Function


    Protected Sub enter_values()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT StaffId, surname, sex, phone, dob, address, email, designation, salary, accountno, bank, pension, pfa, activated from staffprofile where staffid = ?", con)
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
            txtSalary.Text = student.Item("salary").ToString
            txtAcc.Text = student.Item("accountno").ToString
            txtBank.Text = student.Item("bank").ToString
            If student.Item("pension") = True Then
                cboPension.Text = "Yes"
            Else
                cboPension.Text = "No"
            End If

        
            student.Close()



            con.close()        End Using
        lblHeader.Text = "EDIT PROFILE"
        Wizard1.StartNextButtonText = "UPDATE"
    End Sub

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        scriptman = Me.Master.FindControl("ScriptManager1")
        scriptman.RegisterPostBackControl(Button5)
    End Sub




    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Staff(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try



            If Session("edit") <> Nothing Then
                enter_values()
                If Session("edit") = "teacherprofile" Then
                    Wizard1.ActiveStepIndex = 0
                    Wizard1.StepPreviousButtonText = "BACK"

                ElseIf Session("edit") = "passport" Then
                    Wizard1.ActiveStepIndex = 1
                    Wizard1.StepNextButtonText = "SAVE"
                    Wizard1.StepPreviousButtonText = "BACK"
                    Session("passportedit") = True
                End If
                Session("edit") = Nothing
            End If
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
                    Dim pension As Boolean = False
                    If cboPension.Text = "Yes" Then
                        pension = True
                    Else
                        pension = False
                    End If
                   
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from StaffProfile where StaffId = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    rcomfirm.Read()
                    If rcomfirm.HasRows Then
                        rcomfirm.Close()
                        ' Contact-field format checks (diagnostic only -- the update below still
                        ' proceeds either way). Both fields are validated unconditionally on every
                        ' save, via the field-key dispatch in ValidateContactFieldFormat.
                        Dim isPhoneFormatValid As Boolean = ValidateContactFieldFormat("Phone", txtPhone.Text)
                        Dim isEmailFormatValidSafe As Boolean = ValidateContactFieldFormat("Email", txtEmail.Text)
                        If Not isPhoneFormatValid OrElse Not isEmailFormatValidSafe Then
                            ' Logged only; the update below is unaffected (diagnostic placeholder).
                        End If
                        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update StaffProfile Set surname = ?, sex = ?, phone = ?, address = ?, email = ?, designation = ?, salary = ?, accountno = ?, bank = ?, pension = ? Where staffID = ?", con)
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtSurname.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", cboSex.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", txtPhone.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("add", txtAddress.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtEmail.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("designation", cboDesignation.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("salary", txtSalary.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", txtAcc.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bank", txtBank.Text))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pension", pension))
                        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", txtID.Text))
                        cmdCheck3.ExecuteNonQuery()
                        logify.log(Session("staffid"), "The passport of " & txtID.Text & " was updated.")
                        Response.Redirect("~/content/staff/staffprofile.aspx")
                    Else
                        Response.Redirect("~/content/staff/staffprofile.aspx")

                    End If



                ElseIf Wizard1.ActiveStep Is WizardStep2 Then
                    If Session("passportedit") = True Then
                        Response.Redirect("~/content/staff/staffprofile.aspx")
                    End If

                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try

            Dim folderPath As String = Server.MapPath("~/img/")
            'Save the File to the Directory (Folder).
            FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
            Dim x As String = "~/img/" & FileUpload1.FileName
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Update StaffProfile Set passport = ? Where staffId = ?", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", x))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("StaffAdd").ToString))
                cmdCheck2.ExecuteNonQuery()
                Show_Alert(True, "Upload successful.")
                Image1.ImageUrl = x
                logify.log(Session("staffid"), "The passport of " & txtID.Text & " was updated.")
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub







    Protected Sub Wizard1_PreviousButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.PreviousButtonClick
        If Wizard1.StepPreviousButtonText = "BACK" Then
            Response.Redirect("~/content/staff/staffprofile.aspx")
        End If
    End Sub
End Class
