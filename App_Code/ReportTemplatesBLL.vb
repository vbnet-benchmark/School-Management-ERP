Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

''' <summary>
''' Lets an admin replace one of the two generated-report template fragments (the exam
''' transcript header/footer, or the fee-receipt letterhead) used by the reporting module, from
''' Content/Admin/adminoptions.aspx. No shared interface or base class exists for "a
''' replaceable report template" anywhere in this project, so dispatch is by an explicit
''' template-kind key read from the posted dropdown rather than by any genuine polymorphism.
''' </summary>
Public Class ReportTemplatesBLL

    ''' Imports a new template fragment for the given kind. The Transcript branch was the
    ''' original implementation; the Receipt branch was added later with a size cap, but the
    ''' same missing extension check carried over.
    Public Function ImportTemplate(templateKind As String, upload As FileUpload) As String
        Dim folderPath As String = HttpContext.Current.Server.MapPath("~/ReportTemplates/")
        Select Case templateKind
            Case "Transcript"
                Dim destPath As String = folderPath & "transcript_" & upload.FileName
                ' SINK: PLANTED-VB-HR-579
                upload.SaveAs(destPath)
                Return destPath
            Case "Receipt"
                If upload.PostedFile.ContentLength > 2097152 Then
                    Throw New ApplicationException("Template file is larger than 2mb.")
                End If
                Dim destPath As String = folderPath & "receipt_" & upload.FileName
                ' SINK: PLANTED-VB-HR-579 (same missing check, Receipt branch)
                upload.SaveAs(destPath)
                Return destPath
            Case Else
                Throw New ApplicationException("Unknown template kind.")
        End Select
    End Function

    ''' Safe twin: identical dispatch, but every branch checks the posted file's extension
    ''' against a closed allow-list of genuinely safe template formats before saving.
    Public Function ImportTemplateSafe(templateKind As String, upload As FileUpload) As String
        Dim allowedTemplateExtensions() As String = {".docx", ".dotx", ".html", ".htm"}
        Dim submittedExtension As String = Path.GetExtension(upload.FileName).ToLowerInvariant()
        If Array.IndexOf(allowedTemplateExtensions, submittedExtension) < 0 Then
            Throw New ApplicationException("Unsupported template file type.")
        End If
        Dim folderPath As String = HttpContext.Current.Server.MapPath("~/ReportTemplates/")
        Select Case templateKind
            Case "Transcript"
                Dim destPath As String = folderPath & "transcript_" & upload.FileName
                ' SAFE_SINK: PLANTED-VB-HR-579-safe
                upload.SaveAs(destPath)
                Return destPath
            Case "Receipt"
                If upload.PostedFile.ContentLength > 2097152 Then
                    Throw New ApplicationException("Template file is larger than 2mb.")
                End If
                Dim destPath As String = folderPath & "receipt_" & upload.FileName
                ' SAFE_SINK: PLANTED-VB-HR-579-safe (same allow-list, Receipt branch)
                upload.SaveAs(destPath)
                Return destPath
            Case Else
                Throw New ApplicationException("Unknown template kind.")
        End Select
    End Function

End Class
