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
    Public Shared receiver As String
    Public Shared recCategory As String

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim templates As New Lesson_Plan
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
        scriptman.RegisterPostBackControl(btnSend)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False
    End Sub
  
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            If Request.QueryString.ToString <> Nothing Then
                templates.Plan_Id = Request.QueryString.ToString
                Dim a As ArrayList = templates.View_Template
                Hidden1.Value = a(1)
                txtName.Text = a(0)
            End If
            If Not IsPostBack Then templates.Select_Classes(CheckBoxList2)
            Dim l As New Literal
            l = Me.Master.FindControl("summerLit")
            l.Text = templates.Get_Js(hidden1)

            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "this4", l.Text, False)


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try

            If Request.QueryString.ToString = Nothing Then
                If templates.Check_Template(txtName.Text) = False Then
                    templates.New_Template(txtName.Text, Hidden1.Value)
                    Show_Alert(True, "Lesson plan template added")
                Else
                    Show_Alert(False, "Template name already exists.")
                End If
            Else
                templates.Plan_Id = Request.QueryString.ToString
                templates.Edit_Template(txtName.Text, Hidden1.Value)
                Show_Alert(True, "Lesson plan template updated")
            End If
                Dim checked As New ArrayList
                Dim unchecked As New ArrayList
                For Each j As ListItem In CheckBoxList2.Items
                    If j.Selected = True Then
                    checked.Add(j.Text)
                    Else
                    unchecked.Add(j.Text)
                    End If
                Next
                templates.Register_Classes(checked)
                templates.DeRegister_Classes(unchecked)
           
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    ' --- Assigned-Classes Export (PLANTED-VB-HR-385) ---
    ' New feature: let an admin download one combined response header listing every class
    ' currently checked in the "Classes Affected" list, for a quick desktop-tool import.
    '
    ' 5th-tier diversity instance, CWE-113: builds ONE combined response header out of every
    ' CHECKED item's own Text in CheckBoxList2 -- a loop over a UI CONTROL COLLECTION (each
    ' class name itself admin-free-text, entered via Content/Admin/addclass.aspx) accumulated
    ' with System.Text.StringBuilder.Append -- as opposed to every other instance in this batch
    ' (a single scalar value from a query string, a textbox passed through a private helper, a
    ' DB-persisted value read back on a later request, or a dropdown-selected format key).
    Protected Sub btnExportClasses_Click(sender As Object, e As EventArgs) Handles btnExportClasses.Click
        Dim sb As New System.Text.StringBuilder
        For Each item As ListItem In CheckBoxList2.Items
            If item.Selected Then
                sb.Append(item.Text)
                sb.Append(";")
            End If
        Next
        ' SINK: PLANTED-VB-HR-385
        Response.AddHeader("X-Assigned-Classes", sb.ToString())
        Response.ContentType = "text/plain"
        Response.Write(sb.ToString())
        Response.End()
    End Sub

    ' Safe twin of btnExportClasses_Click, reached from a separate button -- identical
    ' loop/StringBuilder shape, but each checked class name has its own CR/LF characters
    ' stripped before being appended, so every tainted component folded into the combined
    ' header is independently neutralized (closing the obs-fold bypass per-component too, since
    ' no CR or LF survives anywhere in the accumulated string).
    Protected Sub btnExportClassesSafe_Click(sender As Object, e As EventArgs) Handles btnExportClassesSafe.Click
        Dim sbSafe As New System.Text.StringBuilder
        For Each item As ListItem In CheckBoxList2.Items
            If item.Selected Then
                sbSafe.Append(item.Text.Replace(Chr(13), "").Replace(Chr(10), ""))
                sbSafe.Append(";")
            End If
        Next
        ' SAFE_SINK: PLANTED-VB-HR-385-safe
        Response.AddHeader("X-Assigned-Classes", sbSafe.ToString())
        Response.ContentType = "text/plain"
        Response.Write(sbSafe.ToString())
        Response.End()
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = True
        Response.Redirect("~/Content/admin/lessonplans.aspx")
    End Sub


 

  

End Class
