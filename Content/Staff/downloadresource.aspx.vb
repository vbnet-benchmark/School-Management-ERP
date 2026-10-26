Imports System.IO
Partial Class Staff_DownloadResource
    Inherits System.Web.UI.Page

    Dim check As New CheckUser

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Subject(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
    End Sub

    ' --- Learning Material / Supplementary Resource Preview (PLANTED-VB-HR-406..410) ---
    ' Lets a teacher re-check the content of a previously uploaded learning material, a student's
    ' supplementary answer attachment (Content/Student/doassignment.aspx), or an archived
    ' class-materials file, by file name -- without downloading the binary and opening it locally.
    ' Complements the existing upload-only flows in Content/Staff/addlearningmaterial.aspx and
    ' Content/Student/doassignment.aspx, neither of which has a matching "read it back"
    ' counterpart today.

    ''' <summary>
    ''' Instance 1 (Direct): straight-line Server.MapPath + StreamReader, no validation at all.
    ''' </summary>
    Protected Sub btnPreviewMaterial_Click(sender As Object, e As EventArgs) Handles btnPreviewMaterial.Click
        Try
            Dim materialPath As String = Server.MapPath("~/content/Uploads/" & txtMaterialName.Text)
            ' SINK: PLANTED-VB-HR-406
            Using sr As New StreamReader(materialPath)
                lblMaterialPreview.Text = Server.HtmlEncode(sr.ReadToEnd())
            End Using
        Catch ex As Exception
            lblMaterialPreview.Text = "Learning material not found: " & Server.HtmlEncode(txtMaterialName.Text)
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin: reduces the requested name to its bare file-name component before it ever
    ''' becomes part of the physical path, so it can never resolve outside the fixed "Uploads"
    ''' directory.
    ''' </summary>
    Protected Sub btnPreviewMaterialSafe_Click(sender As Object, e As EventArgs) Handles btnPreviewMaterialSafe.Click
        Try
            Dim safeFileName As String = Path.GetFileName(txtMaterialNameSafe.Text)
            Dim materialPathSafe As String = Server.MapPath("~/content/Uploads/" & safeFileName)
            ' SAFE_SINK: PLANTED-VB-HR-406-safe
            Using srSafe As New StreamReader(materialPathSafe)
                lblMaterialPreviewSafe.Text = Server.HtmlEncode(srSafe.ReadToEnd())
            End Using
        Catch ex As Exception
            lblMaterialPreviewSafe.Text = "Learning material not found: " & Server.HtmlEncode(txtMaterialNameSafe.Text)
        End Try
    End Sub

    ''' <summary>
    ''' Instance 2 (Indirect): routed through a private, same-class helper that performs the path
    ''' resolution and the open together -- the helper adds no validation of its own.
    ''' </summary>
    Protected Sub btnPreviewSupplementaryAnswer_Click(sender As Object, e As EventArgs) Handles btnPreviewSupplementaryAnswer.Click
        lblSupplementaryAnswerPreview.Text = ReadSupplementaryAnswer(txtSupplementaryAnswer.Text)
    End Sub

    Protected Sub btnPreviewSupplementaryAnswerSafe_Click(sender As Object, e As EventArgs) Handles btnPreviewSupplementaryAnswerSafe.Click
        lblSupplementaryAnswerPreviewSafe.Text = ReadSupplementaryAnswerSafe(txtSupplementaryAnswerSafe.Text)
    End Sub

    ''' <summary>
    ''' Reads back a student's supplementary assignment-answer attachment
    ''' (Content/Student/doassignment.aspx's own supplementary-upload folder) by file name.
    ''' </summary>
    Private Function ReadSupplementaryAnswer(ByVal fileName As String) As String
        Try
            Dim answerPath As String = Server.MapPath("~/Content/Uploads/supplementary/" & fileName)
            ' SINK: PLANTED-VB-HR-407
            Using sr As New StreamReader(answerPath)
                Return Server.HtmlEncode(sr.ReadToEnd())
            End Using
        Catch ex As Exception
            Return "Supplementary answer not found: " & Server.HtmlEncode(fileName)
        End Try
    End Function

    ''' <summary>
    ''' Same as ReadSupplementaryAnswer, but the requested name is reduced to its bare file-name
    ''' component first, so the result can never resolve outside the fixed "supplementary"
    ''' directory.
    ''' </summary>
    Private Function ReadSupplementaryAnswerSafe(ByVal fileName As String) As String
        Try
            Dim safeFileName As String = Path.GetFileName(fileName)
            Dim answerPathSafe As String = Server.MapPath("~/Content/Uploads/supplementary/" & safeFileName)
            ' SAFE_SINK: PLANTED-VB-HR-407-safe
            Using srSafe As New StreamReader(answerPathSafe)
                Return Server.HtmlEncode(srSafe.ReadToEnd())
            End Using
        Catch ex As Exception
            Return "Supplementary answer not found: " & Server.HtmlEncode(fileName)
        End Try
    End Function

    ''' <summary>
    ''' Instance 3 (Interprocedural, cross-file): routed through the shared
    ''' App_Code/LearningResourceDownloadService.vb, so the same "load a learning material's text
    ''' preview" logic used here is also available to Content/Parent/elearning.aspx's own future
    ''' "preview before download" link (not wired up separately yet).
    ''' </summary>
    Protected Sub btnPreviewViaService_Click(sender As Object, e As EventArgs) Handles btnPreviewViaService.Click
        lblServicePreview.Text = LearningResourceDownloadService.LoadMaterialPreview(txtServiceMaterial.Text)
    End Sub

    Protected Sub btnPreviewViaServiceSafe_Click(sender As Object, e As EventArgs) Handles btnPreviewViaServiceSafe.Click
        lblServicePreviewSafe.Text = LearningResourceDownloadService.LoadMaterialPreviewSafe(txtServiceMaterialSafe.Text)
    End Sub

    ''' <summary>
    ''' Instance 4 (Type/polymorphism-dependent): which trust tier applies is chosen per request
    ''' via "resolverMode" (typed into txtResolverMode), and the concrete
    ''' App_Code/LearningResourcePathResolver.vb implementation this picks changes how the raw
    ''' name is turned into a physical path -- "teacher" resolves against the teacher
    ''' learning-material folder with no validation of its own; anything else resolves against the
    ''' student supplementary-answer folder, whose own Resolve() already reduces the name to its
    ''' bare file-name component.
    ''' </summary>
    Protected Sub btnPreviewPolyResource_Click(sender As Object, e As EventArgs) Handles btnPreviewPolyResource.Click
        Try
            If String.Equals(txtResolverMode.Text, "teacher", StringComparison.OrdinalIgnoreCase) Then
                Dim resolver As LearningResourcePathResolver = New TeacherMaterialPathResolver()
                Dim resolvedPath As String = resolver.Resolve(txtPolyResource.Text)
                ' SINK: PLANTED-VB-HR-409
                Using sr As New StreamReader(resolvedPath)
                    lblPolyPreview.Text = Server.HtmlEncode(sr.ReadToEnd())
                End Using
            Else
                Dim resolver As LearningResourcePathResolver = New StudentSubmissionPathResolver()
                Dim resolvedPathSafe As String = resolver.Resolve(txtPolyResource.Text)
                ' SAFE_SINK: PLANTED-VB-HR-409-safe
                Using srSafe As New StreamReader(resolvedPathSafe)
                    lblPolyPreview.Text = Server.HtmlEncode(srSafe.ReadToEnd())
                End Using
            End If
        Catch ex As Exception
            lblPolyPreview.Text = "Resource not found: " & Server.HtmlEncode(txtPolyResource.Text)
        End Try
    End Sub

    ''' <summary>
    ''' Instance 5 (Diversity): class materials can also be organized into nested archive
    ''' categories, submitted as a single comma-separated list of path segments rather than one
    ''' flat name -- e.g. a worksheet filed under "Class10,Mathematics,chapter3.txt". Each segment
    ''' is joined onto the running path in turn, with no validation of any individual segment.
    ''' </summary>
    Protected Sub btnPreviewArchive_Click(sender As Object, e As EventArgs) Handles btnPreviewArchive.Click
        Try
            Dim basePath As String = Server.MapPath("~/Content/Uploads/archive/")
            Dim assembledPath As String = basePath
            For Each seg As String In txtArchiveSegments.Text.Split(","c)
                assembledPath = Path.Combine(assembledPath, seg)
            Next
            ' SINK: PLANTED-VB-HR-410
            Using sr As New StreamReader(assembledPath)
                lblArchivePreview.Text = Server.HtmlEncode(sr.ReadToEnd())
            End Using
        Catch ex As Exception
            lblArchivePreview.Text = "Archived material not found."
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin: canonicalizes the FULLY-ASSEMBLED path (not each segment individually) and
    ''' confirms it still falls under the intended archive directory before opening it -- a
    ''' per-segment Path.GetFileName-only strip would still let ".." segments walk the assembled
    ''' path back out of the archive directory one hop at a time, so the check has to happen after
    ''' every segment has been joined, not before.
    ''' </summary>
    Protected Sub btnPreviewArchiveSafe_Click(sender As Object, e As EventArgs) Handles btnPreviewArchiveSafe.Click
        Try
            Dim basePathSafe As String = Server.MapPath("~/Content/Uploads/archive/")
            Dim assembledPathSafe As String = basePathSafe
            For Each seg As String In txtArchiveSegmentsSafe.Text.Split(","c)
                assembledPathSafe = Path.Combine(assembledPathSafe, seg)
            Next
            Dim canonicalPath As String = Path.GetFullPath(assembledPathSafe)
            Dim canonicalBase As String = Path.GetFullPath(basePathSafe)
            If canonicalPath.StartsWith(canonicalBase & Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) Then
                ' SAFE_SINK: PLANTED-VB-HR-410-safe
                Using srSafe As New StreamReader(canonicalPath)
                    lblArchivePreviewSafe.Text = Server.HtmlEncode(srSafe.ReadToEnd())
                End Using
            Else
                lblArchivePreviewSafe.Text = "Invalid archive path."
            End If
        Catch ex As Exception
            lblArchivePreviewSafe.Text = "Archived material not found."
        End Try
    End Sub
End Class
