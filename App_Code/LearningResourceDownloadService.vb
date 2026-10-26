Imports System
Imports System.IO
Imports System.Web

''' <summary>
''' Shared learning-material preview loading for Content/Staff/downloadresource.aspx's
''' "via Shared Service" link and, eventually, Content/Parent/elearning.aspx's own
''' "preview before download" feature for a parent checking what was assigned (not wired up
''' separately yet) -- both want the same "read back a learning-material file by name"
''' behaviour, so it lives here once instead of being duplicated per page.
''' </summary>
Public Class LearningResourceDownloadService

    ''' <summary>
    ''' Loads the full text of a learning-material file by its file name, for inline preview.
    ''' </summary>
    Public Shared Function LoadMaterialPreview(ByVal materialName As String) As String
        Try
            Dim materialPath As String = HttpContext.Current.Server.MapPath("~/content/Uploads/" & materialName)
            ' SINK: PLANTED-VB-HR-408
            Using sr As New StreamReader(materialPath)
                Return HttpUtility.HtmlEncode(sr.ReadToEnd())
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return "Learning material not found: " & HttpUtility.HtmlEncode(materialName)
        End Try
    End Function

    ''' <summary>
    ''' Same as LoadMaterialPreview, but the requested name is reduced to its bare file-name
    ''' component first, so the result can never resolve outside the fixed "Uploads" directory
    ''' regardless of what the caller passes in.
    ''' </summary>
    Public Shared Function LoadMaterialPreviewSafe(ByVal materialName As String) As String
        Try
            Dim safeFileName As String = Path.GetFileName(materialName)
            Dim materialPathSafe As String = HttpContext.Current.Server.MapPath("~/content/Uploads/" & safeFileName)
            ' SAFE_SINK: PLANTED-VB-HR-408-safe
            Using srSafe As New StreamReader(materialPathSafe)
                Return HttpUtility.HtmlEncode(srSafe.ReadToEnd())
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return "Learning material not found: " & HttpUtility.HtmlEncode(materialName)
        End Try
    End Function
End Class
