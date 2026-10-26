Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_allstudents
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList

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
    ' --- Lesson Plan Template Export (PLANTED-VB-HR-383) ---
    ' New feature: let an admin download a saved lesson-plan template as an HTML file, naming
    ' the download after that template's own display name.
    '
    ' Interprocedural / stored-second-order tier, CWE-113: the template name is never
    ' request-derived HERE -- it is the admin free-text name typed into
    ' Content/Admin/newtemplate.aspx.vb's txtName TextBox on a PRIOR, separate request, saved
    ' unchanged via App_Code/Lesson_Plan.vb#New_Template into the lessontemplates table (a
    ' second file/class), and read back out here via Lesson_Plan#View_Template (the same class,
    ' a third file in the call chain from the admin's point of view: enter name on
    ' newtemplate.aspx -> persisted by Lesson_Plan.vb -> read back and emitted on
    ' lessonplans.aspx). ExportMode=safe is the hardened variant added alongside the
    ' already-existing export path; ExportMode absent/anything else uses the original,
    ' unsanitized path.
    Private Sub ExportLessonTemplateIfRequested()
        If Request.QueryString("ExportId") Is Nothing Then Exit Sub
        templates.Plan_Id = Request.QueryString("ExportId")
        Dim a As ArrayList = templates.View_Template
        Dim templateName As String = a(0).ToString
        Dim templateBody As String = a(1).ToString
        Response.Clear()
        If Request.QueryString("ExportMode") = "safe" Then
            Dim safeTemplateName As String = templateName.Replace(Chr(13), "").Replace(Chr(10), "")
            ' SAFE_SINK: PLANTED-VB-HR-383-safe
            Response.AppendHeader("content-disposition", "attachment; filename=" & safeTemplateName & ".html")
        Else
            ' SINK: PLANTED-VB-HR-383
            Response.AppendHeader("content-disposition", "attachment; filename=" & templateName & ".html")
        End If
        Response.ContentType = "text/html"
        Response.Write(templateBody)
        Response.End()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        ExportLessonTemplateIfRequested()
        Try
            Dim ds As New DataTable
            ds.Columns.Add("id")
            ds.Columns.Add("name")
            
            For j = 0 To templates.Load_Templates.GetLength(1) - 2
                ds.Rows.Add(templates.Load_Templates(0, j), templates.Load_Templates(1, j))
            Next
            GridView1.DataSource = ds
            GridView1.DataBind()
            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Dim ds As New DataTable
            ds.Columns.Add("id")
            ds.Columns.Add("name")
            For j = 0 To templates.Load_Templates.GetLength(1) - 1
                ds.Rows.Add(templates.Load_Templates(0, j), templates.Load_Templates(1, j))
            Next
            GridView1.DataSource = ds
            GridView1.DataBind()
            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
          


            If GridView1.PageIndex + 1 <= GridView1.PageCount Then
                GridView1.PageIndex = GridView1.PageIndex + 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Dim ds As New DataTable
            ds.Columns.Add("id")
            ds.Columns.Add("name")
            For j = 0 To templates.Load_Templates.GetLength(1) - 1
                ds.Rows.Add(templates.Load_Templates(0, j), templates.Load_Templates(1, j))
            Next
            GridView1.DataSource = ds
            GridView1.DataBind()
            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
         

            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataBind()

            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
        templates.Delete_Template(row.Cells(2).Text)
        Dim ds As New DataTable
        ds.Columns.Add("id")
        ds.Columns.Add("name")
        Show_Alert(True, "Template Deleted")
        For j = 0 To templates.Load_Templates.GetLength(1) - 2
            ds.Rows.Add(templates.Load_Templates(0, j), templates.Load_Templates(1, j))
        Next
        GridView1.DataSource = ds
        GridView1.DataBind()
        If GridView1.PageIndex = GridView1.PageCount - 1 Then
            btnNext.Visible = False
        Else
            btnNext.Visible = True
        End If
        If GridView1.PageIndex = 0 Then
            btnPrevious.Visible = False
        Else
            btnPrevious.Visible = True
        End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Dim ds As New DataTable
            ds.Columns.Add("id")
            ds.Columns.Add("name")
            For j = 0 To templates.Search_Template(txtSearch.Text).GetLength(1) - 1
                ds.Rows.Add(templates.Search_Template(txtSearch.Text)(0, j), templates.Search_Template(txtSearch.Text)(1, j))
            Next
            GridView1.AllowPaging = False
            GridView1.DataSource = ds
            GridView1.DataBind()
            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
           
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            Dim ds As New DataTable
            ds.Columns.Add("id")
            ds.Columns.Add("name")
            For j = 0 To templates.Load_Templates.GetLength(1) - 1
                ds.Rows.Add(templates.Load_Templates(0, j), templates.Load_Templates(1, j))
            Next
            GridView1.DataSource = ds
            GridView1.DataBind()
            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
          

            If GridView1.PageIndex - 1 >= 0 Then
                GridView1.PageIndex = GridView1.PageIndex - 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnMsg_Click(sender As Object, e As EventArgs) Handles btnMsg.Click
        Response.Redirect("~/Content/admin/newtemplate.aspx")
    End Sub

   
End Class
